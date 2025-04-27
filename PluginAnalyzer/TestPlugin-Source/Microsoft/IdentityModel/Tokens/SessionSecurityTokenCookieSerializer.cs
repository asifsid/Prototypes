using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Web;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SessionSecurityTokenCookieSerializer
	{
		protected delegate bool OutboundClaimsFilter(Microsoft.IdentityModel.Claims.Claim claim);

		private const string SupportedVersion = "1";

		private const string WindowsSecurityTokenStubElementName = "WindowsSecurityTokenStub";

		private const int MaxDomainNameMapSize = 50;

		private static Dictionary<string, string> DomainNameMap = new Dictionary<string, string>(50, StringComparer.OrdinalIgnoreCase);

		private static Random rnd = new Random();

		private SecurityTokenHandlerCollection _bootstrapTokenHandlers;

		private bool _saveBootstrapTokens;

		private bool _useWindowsTokenService;

		private string _windowsIssuerName;

		public SecurityTokenHandlerCollection BootstrapTokenHandler => _bootstrapTokenHandlers;

		public SessionSecurityTokenCookieSerializer()
			: this(SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(), saveBootstrapTokens: true, useWindowsTokenService: false, "LOCAL AUTHORITY")
		{
		}

		public SessionSecurityTokenCookieSerializer(SecurityTokenHandlerCollection bootstrapTokenHandlers, bool saveBootstrapTokens, bool useWindowsTokenService, string windowsIssuerName)
		{
			_bootstrapTokenHandlers = bootstrapTokenHandlers;
			_saveBootstrapTokens = saveBootstrapTokens;
			_useWindowsTokenService = useWindowsTokenService;
			_windowsIssuerName = windowsIssuerName;
		}

		public virtual byte[] Serialize(SessionSecurityToken sessionToken)
		{
			if (sessionToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionToken");
			}
			MemoryStream memoryStream = new MemoryStream();
			SessionDictionary instance = SessionDictionary.Instance;
			using XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(memoryStream, instance);
			if (sessionToken.IsSecurityContextSecurityTokenWrapper)
			{
				xmlDictionaryWriter.WriteStartElement(instance.SecurityContextToken, instance.EmptyString);
			}
			else
			{
				xmlDictionaryWriter.WriteStartElement(instance.SessionToken, instance.EmptyString);
				if (sessionToken.IsPersistent)
				{
					xmlDictionaryWriter.WriteAttributeString(instance.PersistentTrue, instance.EmptyString, "");
				}
				if (sessionToken.IsSessionMode)
				{
					xmlDictionaryWriter.WriteAttributeString(instance.SessionModeTrue, instance.EmptyString, "");
				}
				if (!string.IsNullOrEmpty(sessionToken.Context))
				{
					xmlDictionaryWriter.WriteElementString(instance.Context, instance.EmptyString, sessionToken.Context);
				}
			}
			xmlDictionaryWriter.WriteStartElement(instance.Version, instance.EmptyString);
			xmlDictionaryWriter.WriteValue("1");
			xmlDictionaryWriter.WriteEndElement();
			xmlDictionaryWriter.WriteElementString(instance.SecureConversationVersion, instance.EmptyString, sessionToken.SecureConversationVersion.Namespace.Value);
			xmlDictionaryWriter.WriteElementString(instance.Id, instance.EmptyString, sessionToken.Id);
			XmlUtil.WriteElementStringAsUniqueId(xmlDictionaryWriter, instance.ContextId, instance.EmptyString, sessionToken.ContextId.ToString());
			byte[] symmetricKey = ((SymmetricSecurityKey)sessionToken.SecurityKeys[0]).GetSymmetricKey();
			xmlDictionaryWriter.WriteStartElement(instance.Key, instance.EmptyString);
			xmlDictionaryWriter.WriteBase64(symmetricKey, 0, symmetricKey.Length);
			xmlDictionaryWriter.WriteEndElement();
			if (sessionToken.KeyGeneration != null)
			{
				XmlUtil.WriteElementStringAsUniqueId(xmlDictionaryWriter, instance.KeyGeneration, instance.EmptyString, sessionToken.KeyGeneration.ToString());
			}
			XmlUtil.WriteElementContentAsInt64(xmlDictionaryWriter, instance.EffectiveTime, instance.EmptyString, sessionToken.ValidFrom.ToUniversalTime().Ticks);
			XmlUtil.WriteElementContentAsInt64(xmlDictionaryWriter, instance.ExpiryTime, instance.EmptyString, sessionToken.ValidTo.ToUniversalTime().Ticks);
			XmlUtil.WriteElementContentAsInt64(xmlDictionaryWriter, instance.KeyEffectiveTime, instance.EmptyString, sessionToken.KeyEffectiveTime.ToUniversalTime().Ticks);
			XmlUtil.WriteElementContentAsInt64(xmlDictionaryWriter, instance.KeyExpiryTime, instance.EmptyString, sessionToken.KeyExpirationTime.ToUniversalTime().Ticks);
			WritePrincipal(xmlDictionaryWriter, instance, sessionToken.ClaimsPrincipal);
			for (int i = 0; i < sessionToken.SecurityContextSecurityToken.AuthorizationPolicies.Count; i++)
			{
				IAuthorizationPolicy authorizationPolicy = sessionToken.SecurityContextSecurityToken.AuthorizationPolicies[i];
				SctAuthorizationPolicy sctAuthorizationPolicy = authorizationPolicy as SctAuthorizationPolicy;
				if (sctAuthorizationPolicy != null)
				{
					xmlDictionaryWriter.WriteStartElement(instance.SctAuthorizationPolicy, instance.EmptyString);
					System.IdentityModel.Claims.Claim claim = ((DefaultClaimSet)((IAuthorizationPolicy)sctAuthorizationPolicy).Issuer)[0];
					SerializeSysClaim(claim, xmlDictionaryWriter);
					xmlDictionaryWriter.WriteEndElement();
				}
			}
			xmlDictionaryWriter.WriteElementString(instance.EndpointId, instance.EmptyString, sessionToken.EndpointId);
			xmlDictionaryWriter.WriteEndElement();
			xmlDictionaryWriter.Flush();
			return memoryStream.ToArray();
		}

		public virtual SessionSecurityToken Deserialize(byte[] cookie)
		{
			if (cookie == null || cookie.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("cookie");
			}
			SessionDictionary instance = SessionDictionary.Instance;
			using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateBinaryReader(cookie, 0, cookie.Length, instance, XmlDictionaryReaderQuotas.Max, null, null))
			{
				bool flag = false;
				bool isPersistent = true;
				bool isSessionMode = false;
				string context = string.Empty;
				if (xmlDictionaryReader.IsStartElement(instance.SecurityContextToken, instance.EmptyString))
				{
					flag = true;
				}
				else
				{
					if (!xmlDictionaryReader.IsStartElement(instance.SessionToken, instance.EmptyString))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.SecurityContextToken.Value, xmlDictionaryReader.Name)));
					}
					if (xmlDictionaryReader.GetAttribute(instance.PersistentTrue, instance.EmptyString) == null)
					{
						isPersistent = false;
					}
					if (xmlDictionaryReader.GetAttribute(instance.SessionModeTrue, instance.EmptyString) != null)
					{
						isSessionMode = true;
					}
					xmlDictionaryReader.ReadFullStartElement();
					xmlDictionaryReader.MoveToContent();
					if (xmlDictionaryReader.IsStartElement(instance.Context, instance.EmptyString))
					{
						context = xmlDictionaryReader.ReadElementContentAsString();
					}
				}
				string text = xmlDictionaryReader.ReadElementString();
				if (text != "1")
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4232", text, "1")));
				}
				string text2 = xmlDictionaryReader.ReadElementString();
				SecureConversationVersion version;
				if (text2 == SecureConversationVersion.WSSecureConversationFeb2005.Namespace.Value)
				{
					version = SecureConversationVersion.WSSecureConversationFeb2005;
				}
				else
				{
					if (!(text2 == SecureConversationVersion.WSSecureConversation13.Namespace.Value))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4232", text, "1")));
					}
					version = SecureConversationVersion.WSSecureConversation13;
				}
				string text3 = null;
				if (xmlDictionaryReader.IsStartElement(instance.Id, instance.EmptyString))
				{
					text3 = xmlDictionaryReader.ReadElementString();
				}
				if (string.IsNullOrEmpty(text3))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4239", instance.Id.Value)));
				}
				if (!xmlDictionaryReader.IsStartElement(instance.ContextId, instance.EmptyString))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.ContextId.Value, xmlDictionaryReader.Name)));
				}
				System.Xml.UniqueId contextId = xmlDictionaryReader.ReadElementContentAsUniqueId();
				if (!xmlDictionaryReader.IsStartElement(instance.Key, instance.EmptyString))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.Key.Value, xmlDictionaryReader.Name)));
				}
				byte[] key = xmlDictionaryReader.ReadElementContentAsBase64();
				System.Xml.UniqueId keyGeneration = null;
				if (xmlDictionaryReader.IsStartElement(instance.KeyGeneration, instance.EmptyString))
				{
					keyGeneration = xmlDictionaryReader.ReadElementContentAsUniqueId();
				}
				if (!xmlDictionaryReader.IsStartElement(instance.EffectiveTime, instance.EmptyString))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.EffectiveTime.Value, xmlDictionaryReader.Name)));
				}
				DateTime validFrom = new DateTime(XmlUtil.ReadElementContentAsInt64(xmlDictionaryReader), DateTimeKind.Utc);
				if (!xmlDictionaryReader.IsStartElement(instance.ExpiryTime, instance.EmptyString))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.ExpiryTime.Value, xmlDictionaryReader.Name)));
				}
				DateTime validTo = new DateTime(XmlUtil.ReadElementContentAsInt64(xmlDictionaryReader), DateTimeKind.Utc);
				if (!xmlDictionaryReader.IsStartElement(instance.KeyEffectiveTime, instance.EmptyString))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.KeyEffectiveTime.Value, xmlDictionaryReader.Name)));
				}
				DateTime keyEffectiveTime = new DateTime(XmlUtil.ReadElementContentAsInt64(xmlDictionaryReader), DateTimeKind.Utc);
				if (!xmlDictionaryReader.IsStartElement(instance.KeyExpiryTime, instance.EmptyString))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", instance.KeyExpiryTime.Value, xmlDictionaryReader.Name)));
				}
				DateTime keyExpirationTime = new DateTime(XmlUtil.ReadElementContentAsInt64(xmlDictionaryReader), DateTimeKind.Utc);
				if (xmlDictionaryReader.IsStartElement(instance.ClaimsPrincipal, instance.EmptyString))
				{
					List<IAuthorizationPolicy> list = new List<IAuthorizationPolicy>();
					IClaimsPrincipal claimsPrincipal = ReadPrincipal(xmlDictionaryReader, instance);
					if (claimsPrincipal != null)
					{
						list.Add(new AuthorizationPolicy(claimsPrincipal.Identities));
					}
					if (xmlDictionaryReader.IsStartElement(instance.SctAuthorizationPolicy, instance.EmptyString))
					{
						xmlDictionaryReader.ReadStartElement(instance.SctAuthorizationPolicy, instance.EmptyString);
						System.IdentityModel.Claims.Claim claim = DeserializeSysClaim(xmlDictionaryReader);
						xmlDictionaryReader.ReadEndElement();
						list.Add(new SctAuthorizationPolicy(claim));
					}
					string text4 = null;
					if (xmlDictionaryReader.IsStartElement(instance.EndpointId, instance.EmptyString))
					{
						text4 = xmlDictionaryReader.ReadElementContentAsString();
						list.Add(new EndpointAuthorizationPolicy(text4));
					}
					xmlDictionaryReader.ReadEndElement();
					if (flag)
					{
						SecurityContextSecurityToken securityContextToken = new SecurityContextSecurityToken(contextId, text3, key, validFrom, validTo, keyGeneration, keyEffectiveTime, keyExpirationTime, list.AsReadOnly());
						return new SessionSecurityToken(securityContextToken, version);
					}
					list.Add(new EndpointAuthorizationPolicy(text4 ?? string.Empty));
					SessionSecurityToken sessionSecurityToken = new SessionSecurityToken(contextId, text3, context, key, text4, validFrom, validTo, keyGeneration, keyEffectiveTime, keyExpirationTime, list.AsReadOnly());
					sessionSecurityToken.IsPersistent = isPersistent;
					sessionSecurityToken.IsSessionMode = isSessionMode;
					return sessionSecurityToken;
				}
			}
			return null;
		}

		protected virtual System.IdentityModel.Claims.Claim DeserializeSysClaim(XmlDictionaryReader reader)
		{
			SessionDictionary instance = SessionDictionary.Instance;
			if (reader.IsStartElement(instance.NullValue, instance.EmptyString))
			{
				reader.ReadElementString();
				return null;
			}
			if (reader.IsStartElement(instance.WindowsSidClaim, instance.EmptyString))
			{
				string right = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				byte[] binaryForm = reader.ReadContentAsBase64();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Sid, new SecurityIdentifier(binaryForm, 0), right);
			}
			if (reader.IsStartElement(instance.DenyOnlySidClaim, instance.EmptyString))
			{
				string right2 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				byte[] binaryForm2 = reader.ReadContentAsBase64();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.DenyOnlySid, new SecurityIdentifier(binaryForm2, 0), right2);
			}
			if (reader.IsStartElement(instance.X500DistinguishedNameClaim, instance.EmptyString))
			{
				string right3 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				byte[] encodedDistinguishedName = reader.ReadContentAsBase64();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.X500DistinguishedName, new X500DistinguishedName(encodedDistinguishedName), right3);
			}
			if (reader.IsStartElement(instance.X509ThumbprintClaim, instance.EmptyString))
			{
				string right4 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				byte[] resource = reader.ReadContentAsBase64();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Thumbprint, resource, right4);
			}
			if (reader.IsStartElement(instance.NameClaim, instance.EmptyString))
			{
				string right5 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string resource2 = reader.ReadString();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Name, resource2, right5);
			}
			if (reader.IsStartElement(instance.DnsClaim, instance.EmptyString))
			{
				string right6 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string resource3 = reader.ReadString();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Dns, resource3, right6);
			}
			if (reader.IsStartElement(instance.RsaClaim, instance.EmptyString))
			{
				string right7 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string xmlString = reader.ReadString();
				reader.ReadEndElement();
				RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString(xmlString);
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Rsa, rSACryptoServiceProvider, right7);
			}
			if (reader.IsStartElement(instance.MailAddressClaim, instance.EmptyString))
			{
				string right8 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string address = reader.ReadString();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Email, new MailAddress(address), right8);
			}
			if (reader.IsStartElement(instance.SystemClaim, instance.EmptyString))
			{
				reader.ReadElementString();
				return System.IdentityModel.Claims.Claim.System;
			}
			if (reader.IsStartElement(instance.HashClaim, instance.EmptyString))
			{
				string right9 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				byte[] resource4 = reader.ReadContentAsBase64();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Hash, resource4, right9);
			}
			if (reader.IsStartElement(instance.SpnClaim, instance.EmptyString))
			{
				string right10 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string resource5 = reader.ReadString();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Spn, resource5, right10);
			}
			if (reader.IsStartElement(instance.UpnClaim, instance.EmptyString))
			{
				string right11 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string resource6 = reader.ReadString();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Upn, resource6, right11);
			}
			if (reader.IsStartElement(instance.UrlClaim, instance.EmptyString))
			{
				string right12 = ReadRightAttribute(reader, instance);
				reader.ReadStartElement();
				string uriString = reader.ReadString();
				reader.ReadEndElement();
				return new System.IdentityModel.Claims.Claim(System.IdentityModel.Claims.ClaimTypes.Uri, new Uri(uriString), right12);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4289", reader.LocalName, reader.NamespaceURI)));
		}

		protected virtual void SerializeSysClaim(System.IdentityModel.Claims.Claim claim, XmlDictionaryWriter writer)
		{
			SessionDictionary instance = SessionDictionary.Instance;
			if (claim == null)
			{
				writer.WriteElementString(instance.NullValue, instance.EmptyString, string.Empty);
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Sid.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.WindowsSidClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				SerializeSid((SecurityIdentifier)claim.Resource, instance, writer);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.DenyOnlySid.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.DenyOnlySidClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				SerializeSid((SecurityIdentifier)claim.Resource, instance, writer);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.X500DistinguishedName.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.X500DistinguishedNameClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				byte[] rawData = ((X500DistinguishedName)claim.Resource).RawData;
				writer.WriteBase64(rawData, 0, rawData.Length);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Thumbprint.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.X509ThumbprintClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				byte[] array = (byte[])claim.Resource;
				writer.WriteBase64(array, 0, array.Length);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Name.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.NameClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString((string)claim.Resource);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Dns.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.DnsClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString((string)claim.Resource);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Rsa.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.RsaClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString(((RSA)claim.Resource).ToXmlString(includePrivateParameters: false));
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Email.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.MailAddressClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString(((MailAddress)claim.Resource).Address);
				writer.WriteEndElement();
				return;
			}
			if (claim == System.IdentityModel.Claims.Claim.System)
			{
				writer.WriteElementString(instance.SystemClaim, instance.EmptyString, string.Empty);
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Hash.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.HashClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				byte[] array2 = (byte[])claim.Resource;
				writer.WriteBase64(array2, 0, array2.Length);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Spn.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.SpnClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString((string)claim.Resource);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Upn.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.UpnClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString((string)claim.Resource);
				writer.WriteEndElement();
				return;
			}
			if (System.IdentityModel.Claims.ClaimTypes.Uri.Equals(claim.ClaimType))
			{
				writer.WriteStartElement(instance.UrlClaim, instance.EmptyString);
				WriteRightAttribute(claim, instance, writer);
				writer.WriteString(((Uri)claim.Resource).AbsoluteUri);
				writer.WriteEndElement();
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4290", claim)));
		}

		protected virtual SecurityToken ReadBootstrapToken(XmlDictionaryReader dictionaryReader, SessionDictionary dictionary, IClaimsIdentity identity)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (_bootstrapTokenHandlers == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4273")));
			}
			SecurityToken result = null;
			dictionaryReader.ReadStartElement(dictionary.BootstrapToken, dictionary.EmptyString);
			if (dictionaryReader.IsStartElement("WindowsSecurityTokenStub", string.Empty))
			{
				bool isEmptyElement = dictionaryReader.IsEmptyElement;
				dictionaryReader.ReadStartElement();
				if (!isEmptyElement)
				{
					dictionaryReader.ReadEndElement();
				}
				WindowsClaimsIdentity windowsClaimsIdentity = identity as WindowsClaimsIdentity;
				if (windowsClaimsIdentity != null && IsNonX509Identity(windowsClaimsIdentity))
				{
					result = new WindowsSecurityToken(windowsClaimsIdentity);
				}
			}
			else
			{
				string s = dictionaryReader.ReadOuterXml();
				using StringReader input = new StringReader(s);
				XmlTextReader xmlTextReader = new XmlTextReader(input);
				xmlTextReader.Normalization = false;
				using XmlDictionaryReader xmlDictionaryReader = new IdentityModelWrappedXmlDictionaryReader(xmlTextReader, XmlDictionaryReaderQuotas.Max);
				xmlDictionaryReader.MoveToContent();
				result = _bootstrapTokenHandlers.ReadToken(xmlDictionaryReader);
			}
			dictionaryReader.ReadEndElement();
			return result;
		}

		protected virtual IClaimsPrincipal ReadPrincipal(XmlDictionaryReader dictionaryReader, SessionDictionary dictionary)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			IClaimsPrincipal claimsPrincipal = null;
			ClaimsIdentityCollection claimsIdentityCollection = new ClaimsIdentityCollection();
			dictionaryReader.MoveToContent();
			if (dictionaryReader.IsStartElement(dictionary.ClaimsPrincipal, dictionary.EmptyString))
			{
				dictionaryReader.ReadFullStartElement();
				ReadIdentities(dictionaryReader, dictionary, claimsIdentityCollection);
				dictionaryReader.ReadEndElement();
			}
			WindowsClaimsIdentity windowsClaimsIdentity = null;
			foreach (IClaimsIdentity item in claimsIdentityCollection)
			{
				windowsClaimsIdentity = item as WindowsClaimsIdentity;
				if (windowsClaimsIdentity != null)
				{
					claimsPrincipal = new WindowsClaimsPrincipal(windowsClaimsIdentity);
					break;
				}
			}
			if (claimsPrincipal != null)
			{
				claimsIdentityCollection.Remove(windowsClaimsIdentity);
			}
			else if (claimsIdentityCollection.Count > 0)
			{
				claimsPrincipal = new ClaimsPrincipal();
			}
			claimsPrincipal?.Identities.AddRange(claimsIdentityCollection);
			return claimsPrincipal;
		}

		protected virtual void ReadIdentities(XmlDictionaryReader dictionaryReader, SessionDictionary dictionary, ClaimsIdentityCollection identities)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (identities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identities");
			}
			dictionaryReader.MoveToContent();
			if (dictionaryReader.IsStartElement(dictionary.Identities, dictionary.EmptyString))
			{
				dictionaryReader.ReadFullStartElement();
				while (dictionaryReader.IsStartElement(dictionary.Identity, dictionary.EmptyString))
				{
					identities.Add(ReadIdentity(dictionaryReader, dictionary));
				}
				dictionaryReader.ReadEndElement();
			}
		}

		protected virtual IClaimsIdentity ReadIdentity(XmlDictionaryReader dictionaryReader, SessionDictionary dictionary)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			dictionaryReader.MoveToContent();
			if (dictionaryReader.IsStartElement(dictionary.Identity, dictionary.EmptyString))
			{
				string attribute = dictionaryReader.GetAttribute(dictionary.WindowsLogonName, dictionary.EmptyString);
				string attribute2 = dictionaryReader.GetAttribute(dictionary.AuthenticationType, dictionary.EmptyString);
				IClaimsIdentity claimsIdentity = (IClaimsIdentity)((!string.IsNullOrEmpty(attribute)) ? ((object)WindowsClaimsIdentity.CreateFromUpn(GetUpn(attribute), attribute2, _useWindowsTokenService, _windowsIssuerName)) : ((object)new ClaimsIdentity(attribute2)));
				claimsIdentity.Label = dictionaryReader.GetAttribute(dictionary.Label, dictionary.EmptyString);
				claimsIdentity.NameClaimType = dictionaryReader.GetAttribute(dictionary.NameClaimType, dictionary.EmptyString);
				claimsIdentity.RoleClaimType = dictionaryReader.GetAttribute(dictionary.RoleClaimType, dictionary.EmptyString);
				dictionaryReader.ReadFullStartElement();
				if (dictionaryReader.IsStartElement(dictionary.ClaimCollection, dictionary.EmptyString))
				{
					dictionaryReader.ReadStartElement();
					ReadClaims(dictionaryReader, dictionary, claimsIdentity.Claims);
					dictionaryReader.ReadEndElement();
				}
				if (dictionaryReader.IsStartElement(dictionary.Actor, dictionary.EmptyString))
				{
					dictionaryReader.ReadStartElement();
					claimsIdentity.Actor = ReadIdentity(dictionaryReader, dictionary);
					dictionaryReader.ReadEndElement();
				}
				if (dictionaryReader.IsStartElement(dictionary.BootstrapToken, dictionary.EmptyString))
				{
					SecurityToken bootstrapToken = ReadBootstrapToken(dictionaryReader, dictionary, claimsIdentity);
					if (_saveBootstrapTokens)
					{
						claimsIdentity.BootstrapToken = bootstrapToken;
					}
				}
				dictionaryReader.ReadEndElement();
				return claimsIdentity;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID3007", dictionaryReader.LocalName, dictionaryReader.NamespaceURI)));
		}

		protected virtual void ReadClaims(XmlDictionaryReader dictionaryReader, SessionDictionary dictionary, ClaimCollection claims)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (claims == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claims");
			}
			while (dictionaryReader.IsStartElement(dictionary.Claim, dictionary.EmptyString))
			{
				Microsoft.IdentityModel.Claims.Claim claim = new Microsoft.IdentityModel.Claims.Claim(dictionaryReader.GetAttribute(dictionary.Type, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.Value, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.ValueType, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.Issuer, dictionary.EmptyString), dictionaryReader.GetAttribute(dictionary.OriginalIssuer, dictionary.EmptyString));
				dictionaryReader.ReadFullStartElement();
				if (dictionaryReader.IsStartElement(dictionary.ClaimProperties, dictionary.EmptyString))
				{
					ReadClaimProperties(dictionaryReader, dictionary, claim.Properties);
				}
				dictionaryReader.ReadEndElement();
				claims.Add(claim);
			}
		}

		protected virtual void ReadClaimProperties(XmlDictionaryReader dictionaryReader, SessionDictionary dictionary, IDictionary<string, string> properties)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (properties == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("properties");
			}
			dictionaryReader.ReadStartElement();
			while (dictionaryReader.IsStartElement(dictionary.ClaimProperty, dictionary.EmptyString))
			{
				string attribute = dictionaryReader.GetAttribute(dictionary.ClaimPropertyName, dictionary.EmptyString);
				string attribute2 = dictionaryReader.GetAttribute(dictionary.ClaimPropertyValue, dictionary.EmptyString);
				if (string.IsNullOrEmpty(attribute))
				{
					DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4249")));
				}
				if (string.IsNullOrEmpty(attribute2))
				{
					DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4250")));
				}
				properties.Add(new KeyValuePair<string, string>(attribute, attribute2));
				dictionaryReader.ReadFullStartElement();
				dictionaryReader.ReadEndElement();
			}
			dictionaryReader.ReadEndElement();
		}

		protected virtual void WriteBootstrapToken(XmlDictionaryWriter dictionaryWriter, SessionDictionary dictionary, SecurityToken token)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (token == null)
			{
				return;
			}
			dictionaryWriter.WriteStartElement(dictionary.BootstrapToken, dictionary.EmptyString);
			if (token is KerberosReceiverSecurityToken || token is WindowsSecurityToken)
			{
				dictionaryWriter.WriteStartElement("WindowsSecurityTokenStub", string.Empty);
				dictionaryWriter.WriteEndElement();
			}
			else
			{
				if (!_bootstrapTokenHandlers.CanWriteToken(token))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4010", token.GetType().ToString()));
				}
				Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityToken saml2SecurityToken = token as Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityToken;
				Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = null;
				if (saml2SecurityToken != null && saml2SecurityToken.Assertion.EncryptingCredentials != null)
				{
					encryptingCredentials = saml2SecurityToken.Assertion.EncryptingCredentials;
					saml2SecurityToken.Assertion.EncryptingCredentials = null;
				}
				_bootstrapTokenHandlers.WriteToken(dictionaryWriter, token);
				if (saml2SecurityToken != null && encryptingCredentials != null)
				{
					saml2SecurityToken.Assertion.EncryptingCredentials = encryptingCredentials;
				}
			}
			dictionaryWriter.WriteEndElement();
		}

		protected virtual void WritePrincipal(XmlDictionaryWriter dictionaryWriter, SessionDictionary dictionary, IClaimsPrincipal principal)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryWriter");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			dictionaryWriter.WriteStartElement(dictionary.ClaimsPrincipal, dictionary.EmptyString);
			if (principal.Identities != null)
			{
				WriteIdentities(dictionaryWriter, dictionary, principal.Identities);
			}
			dictionaryWriter.WriteEndElement();
		}

		protected virtual void WriteIdentities(XmlDictionaryWriter dictionaryWriter, SessionDictionary dictionary, ClaimsIdentityCollection identities)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryWriter");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (identities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identities");
			}
			dictionaryWriter.WriteStartElement(dictionary.Identities, dictionary.EmptyString);
			foreach (IClaimsIdentity identity in identities)
			{
				WriteIdentity(dictionaryWriter, dictionary, identity);
			}
			dictionaryWriter.WriteEndElement();
		}

		protected virtual void WriteIdentity(XmlDictionaryWriter dictionaryWriter, SessionDictionary dictionary, IClaimsIdentity identity)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryWriter");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			dictionaryWriter.WriteStartElement(dictionary.Identity, dictionary.EmptyString);
			WindowsClaimsIdentity windowsClaimsIdentity = identity as WindowsClaimsIdentity;
			if (windowsClaimsIdentity != null)
			{
				dictionaryWriter.WriteAttributeString(dictionary.WindowsLogonName, dictionary.EmptyString, windowsClaimsIdentity.Name);
			}
			if (!string.IsNullOrEmpty(identity.AuthenticationType))
			{
				dictionaryWriter.WriteAttributeString(dictionary.AuthenticationType, dictionary.EmptyString, identity.AuthenticationType);
			}
			if (!string.IsNullOrEmpty(identity.Label))
			{
				dictionaryWriter.WriteAttributeString(dictionary.Label, dictionary.EmptyString, identity.Label);
			}
			if (identity.NameClaimType != null)
			{
				dictionaryWriter.WriteAttributeString(dictionary.NameClaimType, dictionary.EmptyString, identity.NameClaimType);
			}
			if (identity.RoleClaimType != null)
			{
				dictionaryWriter.WriteAttributeString(dictionary.RoleClaimType, dictionary.EmptyString, identity.RoleClaimType);
			}
			if (identity.Claims != null && identity.Claims.Count > 0)
			{
				dictionaryWriter.WriteStartElement(dictionary.ClaimCollection, dictionary.EmptyString);
				WriteClaims(dictionaryWriter, dictionary, identity.Claims, (windowsClaimsIdentity == null) ? null : ((OutboundClaimsFilter)((Microsoft.IdentityModel.Claims.Claim c) => (c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid" || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid" || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid" || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid" || c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid" || (c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" && c.Issuer == "LOCAL AUTHORITY" && c.ValueType == "http://www.w3.org/2001/XMLSchema#string")) ? true : false)));
				dictionaryWriter.WriteEndElement();
			}
			if (identity.Actor != null)
			{
				dictionaryWriter.WriteStartElement(dictionary.Actor, dictionary.EmptyString);
				WriteIdentity(dictionaryWriter, dictionary, identity.Actor);
				dictionaryWriter.WriteEndElement();
			}
			if (_saveBootstrapTokens)
			{
				WriteBootstrapToken(dictionaryWriter, dictionary, identity.BootstrapToken);
			}
			dictionaryWriter.WriteEndElement();
		}

		protected virtual void WriteClaims(XmlDictionaryWriter dictionaryWriter, SessionDictionary dictionary, ClaimCollection claims, OutboundClaimsFilter outboundClaimsFilter)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryWriter");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (claims == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claims");
			}
			foreach (Microsoft.IdentityModel.Claims.Claim claim in claims)
			{
				if (claim != null && (outboundClaimsFilter == null || !outboundClaimsFilter(claim)))
				{
					dictionaryWriter.WriteStartElement(dictionary.Claim, dictionary.EmptyString);
					if (!string.IsNullOrEmpty(claim.Issuer))
					{
						dictionaryWriter.WriteAttributeString(dictionary.Issuer, dictionary.EmptyString, claim.Issuer);
					}
					if (!string.IsNullOrEmpty(claim.OriginalIssuer))
					{
						dictionaryWriter.WriteAttributeString(dictionary.OriginalIssuer, dictionary.EmptyString, claim.OriginalIssuer);
					}
					dictionaryWriter.WriteAttributeString(dictionary.Type, dictionary.EmptyString, claim.ClaimType);
					dictionaryWriter.WriteAttributeString(dictionary.Value, dictionary.EmptyString, claim.Value);
					dictionaryWriter.WriteAttributeString(dictionary.ValueType, dictionary.EmptyString, claim.ValueType);
					if (claim.Properties != null && claim.Properties.Count > 0)
					{
						WriteClaimProperties(dictionaryWriter, dictionary, claim.Properties);
					}
					dictionaryWriter.WriteEndElement();
				}
			}
		}

		protected virtual void WriteClaimProperties(XmlDictionaryWriter dictionaryWriter, SessionDictionary dictionary, IDictionary<string, string> properties)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryWriter");
			}
			if (dictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionary");
			}
			if (properties == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("properties");
			}
			if (properties.Count <= 0)
			{
				return;
			}
			dictionaryWriter.WriteStartElement(dictionary.ClaimProperties, dictionary.EmptyString);
			foreach (KeyValuePair<string, string> property in properties)
			{
				dictionaryWriter.WriteStartElement(dictionary.ClaimProperty, dictionary.EmptyString);
				dictionaryWriter.WriteAttributeString(dictionary.ClaimPropertyName, dictionary.EmptyString, property.Key);
				dictionaryWriter.WriteAttributeString(dictionary.ClaimPropertyValue, dictionary.EmptyString, property.Value);
				dictionaryWriter.WriteEndElement();
			}
			dictionaryWriter.WriteEndElement();
		}

		private static bool IsNonX509Identity(WindowsClaimsIdentity windowsIdentity)
		{
			Microsoft.IdentityModel.Claims.Claim claim2 = windowsIdentity.Claims.FirstOrDefault((Microsoft.IdentityModel.Claims.Claim claim) => claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint");
			if (claim2 == null)
			{
				return true;
			}
			return false;
		}

		private static void SerializeSid(SecurityIdentifier sid, SessionDictionary dictionary, XmlDictionaryWriter writer)
		{
			byte[] array = new byte[sid.BinaryLength];
			sid.GetBinaryForm(array, 0);
			writer.WriteBase64(array, 0, array.Length);
		}

		private static void WriteRightAttribute(System.IdentityModel.Claims.Claim claim, SessionDictionary dictionary, XmlDictionaryWriter writer)
		{
			if (!Rights.PossessProperty.Equals(claim.Right))
			{
				writer.WriteAttributeString(dictionary.Right, dictionary.EmptyString, claim.Right);
			}
		}

		private static string ReadRightAttribute(XmlDictionaryReader reader, SessionDictionary dictionary)
		{
			string attribute = reader.GetAttribute(dictionary.Right, dictionary.EmptyString);
			if (!string.IsNullOrEmpty(attribute))
			{
				return attribute;
			}
			return Rights.PossessProperty;
		}

		private static void WriteSidAttribute(SecurityIdentifier sid, SessionDictionary dictionary, XmlDictionaryWriter writer)
		{
			byte[] array = new byte[sid.BinaryLength];
			sid.GetBinaryForm(array, 0);
			writer.WriteAttributeString(dictionary.Sid, dictionary.EmptyString, Convert.ToBase64String(array));
		}

		private static SecurityIdentifier ReadSidAttribute(XmlDictionaryReader reader, SessionDictionary dictionary)
		{
			byte[] binaryForm = Convert.FromBase64String(reader.GetAttribute(dictionary.Sid, dictionary.EmptyString));
			return new SecurityIdentifier(binaryForm, 0);
		}

		protected virtual string GetUpn(string windowsLogonName)
		{
			if (string.IsNullOrEmpty(windowsLogonName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("windowsLogonName");
			}
			int num = windowsLogonName.IndexOf('\\');
			if (num < 0 || num == 0 || num == windowsLogonName.Length - 1)
			{
				if (IsPossibleUpn(windowsLogonName))
				{
					return windowsLogonName;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4248", windowsLogonName)));
			}
			string text = windowsLogonName.Substring(0, num + 1);
			string text2 = windowsLogonName.Substring(num + 1);
			bool flag;
			string value;
			lock (DomainNameMap)
			{
				flag = DomainNameMap.TryGetValue(text, out value);
			}
			if (!flag)
			{
				uint size = 50u;
				StringBuilder stringBuilder = new StringBuilder((int)size);
				if (!NativeMethods.TranslateName(text, EXTENDED_NAME_FORMAT.NameSamCompatible, EXTENDED_NAME_FORMAT.NameCanonical, stringBuilder, out size))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 122)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4248", windowsLogonName), new Win32Exception(lastWin32Error)));
					}
					stringBuilder = new StringBuilder((int)size);
					if (!NativeMethods.TranslateName(text, EXTENDED_NAME_FORMAT.NameSamCompatible, EXTENDED_NAME_FORMAT.NameCanonical, stringBuilder, out size))
					{
						lastWin32Error = Marshal.GetLastWin32Error();
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4248", windowsLogonName), new Win32Exception(lastWin32Error)));
					}
				}
				stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
				value = stringBuilder.ToString();
				lock (DomainNameMap)
				{
					if (DomainNameMap.Count >= 50)
					{
						if (rnd == null)
						{
							rnd = new Random((int)DateTime.Now.Ticks);
						}
						int num2 = rnd.Next() % DomainNameMap.Count;
						foreach (string key in DomainNameMap.Keys)
						{
							if (num2 <= 0)
							{
								DomainNameMap.Remove(key);
								break;
							}
							num2--;
						}
					}
					DomainNameMap[text] = value;
				}
			}
			return text2 + "@" + value;
		}

		private static bool IsPossibleUpn(string name)
		{
			int num = name.IndexOf('@');
			if (name.Length < 3 || num < 0 || num == 0 || num == name.Length - 1)
			{
				return false;
			}
			return true;
		}
	}
}
