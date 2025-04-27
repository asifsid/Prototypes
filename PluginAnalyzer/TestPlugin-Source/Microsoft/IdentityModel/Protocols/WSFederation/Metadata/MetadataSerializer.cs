using System;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class MetadataSerializer
	{
		public const string LanguagePrefix = "xml";

		public const string LanguageLocalname = "lang";

		public const string LanguageAttribute = "xml:lang";

		public const string LanguageNamespaceUri = "http://www.w3.org/XML/1998/namespace";

		private const string _uriReference = "_metadata";

		private SecurityTokenSerializer _tokenSerializer;

		public SecurityTokenSerializer SecurityTokenSerializer => _tokenSerializer;

		public MetadataSerializer()
			: this(new WSSecurityTokenSerializer(SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, emitBspRequiredAttributes: false, null, null, null, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationOffset, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationLabelLength, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationNonceLength))
		{
		}

		public MetadataSerializer(SecurityTokenSerializer tokenSerializer)
		{
			if (tokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenSerializer");
			}
			_tokenSerializer = tokenSerializer;
		}

		protected virtual ApplicationServiceDescriptor CreateApplicationServiceInstance()
		{
			return new ApplicationServiceDescriptor();
		}

		protected virtual ContactPerson CreateContactPersonInstance()
		{
			return new ContactPerson();
		}

		protected virtual ProtocolEndpoint CreateProtocolEndpointInstance()
		{
			return new ProtocolEndpoint();
		}

		protected virtual EntitiesDescriptor CreateEntitiesDescriptorInstance()
		{
			return new EntitiesDescriptor();
		}

		protected virtual EntityDescriptor CreateEntityDescriptorInstance()
		{
			return new EntityDescriptor();
		}

		protected virtual IdentityProviderSingleSignOnDescriptor CreateIdentityProviderSingleSignOnDescriptorInstance()
		{
			return new IdentityProviderSingleSignOnDescriptor();
		}

		protected virtual IndexedProtocolEndpoint CreateIndexedProtocolEndpointInstance()
		{
			return new IndexedProtocolEndpoint();
		}

		protected virtual KeyDescriptor CreateKeyDescriptorInstance()
		{
			return new KeyDescriptor();
		}

		protected virtual LocalizedName CreateLocalizedNameInstance()
		{
			return new LocalizedName();
		}

		protected virtual LocalizedUri CreateLocalizedUriInstance()
		{
			return new LocalizedUri();
		}

		protected virtual Organization CreateOrganizationInstance()
		{
			return new Organization();
		}

		protected virtual SecurityTokenServiceDescriptor CreateSecurityTokenServiceDescriptorInstance()
		{
			return new SecurityTokenServiceDescriptor();
		}

		protected virtual ServiceProviderSingleSignOnDescriptor CreateServiceProviderSingleSignOnDescriptorInstance()
		{
			return new ServiceProviderSingleSignOnDescriptor();
		}

		protected virtual AddressingVersion GetAddressingVersion(string namespaceUri)
		{
			if (string.IsNullOrEmpty(namespaceUri))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("namespaceUri");
			}
			if (StringComparer.Ordinal.Equals(namespaceUri, "http://www.w3.org/2005/08/addressing"))
			{
				return AddressingVersion.WSAddressing10;
			}
			if (StringComparer.Ordinal.Equals(namespaceUri, "http://schemas.xmlsoap.org/ws/2004/08/addressing"))
			{
				return AddressingVersion.WSAddressingAugust2004;
			}
			return null;
		}

		private static ContactType GetContactPersonType(string conactType, out bool found)
		{
			if (conactType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("conactType");
			}
			found = true;
			if (StringComparer.Ordinal.Equals(conactType, "unspecified"))
			{
				return ContactType.Unspecified;
			}
			if (StringComparer.Ordinal.Equals(conactType, "administrative"))
			{
				return ContactType.Administrative;
			}
			if (StringComparer.Ordinal.Equals(conactType, "billing"))
			{
				return ContactType.Billing;
			}
			if (StringComparer.Ordinal.Equals(conactType, "other"))
			{
				return ContactType.Other;
			}
			if (StringComparer.Ordinal.Equals(conactType, "support"))
			{
				return ContactType.Support;
			}
			if (StringComparer.Ordinal.Equals(conactType, "technical"))
			{
				return ContactType.Technical;
			}
			found = false;
			return ContactType.Unspecified;
		}

		private static KeyType GetKeyDescriptorType(string keyType)
		{
			if (keyType == null)
			{
				return KeyType.Unspecified;
			}
			if (StringComparer.Ordinal.Equals(keyType, "encryption"))
			{
				return KeyType.Encryption;
			}
			if (StringComparer.Ordinal.Equals(keyType, "signing"))
			{
				return KeyType.Signing;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "use", keyType)));
		}

		protected virtual ApplicationServiceDescriptor ReadApplicationServiceDescriptor(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			ApplicationServiceDescriptor applicationServiceDescriptor = CreateApplicationServiceInstance();
			ReadWebServiceDescriptorAttributes(reader, applicationServiceDescriptor);
			ReadCustomAttributes(reader, applicationServiceDescriptor);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("ApplicationServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
					{
						isEmptyElement = reader.IsEmptyElement;
						reader.ReadStartElement();
						if (!isEmptyElement && reader.IsStartElement())
						{
							EndpointAddress item = EndpointAddress.ReadFrom(GetAddressingVersion(reader.NamespaceURI), reader);
							applicationServiceDescriptor.Endpoints.Add(item);
							reader.ReadEndElement();
						}
					}
					else if (reader.IsStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
					{
						isEmptyElement = reader.IsEmptyElement;
						reader.ReadStartElement();
						if (!isEmptyElement && reader.IsStartElement())
						{
							EndpointAddress item2 = EndpointAddress.ReadFrom(GetAddressingVersion(reader.NamespaceURI), reader);
							applicationServiceDescriptor.PassiveRequestorEndpoints.Add(item2);
							reader.ReadEndElement();
						}
					}
					else if (!ReadWebServiceDescriptorElement(reader, applicationServiceDescriptor) && !ReadCustomElement(reader, applicationServiceDescriptor))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return applicationServiceDescriptor;
		}

		protected virtual ContactPerson ReadContactPerson(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			ContactPerson contactPerson = CreateContactPersonInstance();
			string attribute = reader.GetAttribute("contactType", null);
			bool found = false;
			contactPerson.Type = GetContactPersonType(attribute, out found);
			if (!found)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3201", typeof(ContactType), attribute)));
			}
			ReadCustomAttributes(reader, contactPerson);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("Company", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						contactPerson.Company = reader.ReadElementContentAsString("Company", "urn:oasis:names:tc:SAML:2.0:metadata");
					}
					else if (reader.IsStartElement("GivenName", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						contactPerson.GivenName = reader.ReadElementContentAsString("GivenName", "urn:oasis:names:tc:SAML:2.0:metadata");
					}
					else if (reader.IsStartElement("SurName", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						contactPerson.Surname = reader.ReadElementContentAsString("SurName", "urn:oasis:names:tc:SAML:2.0:metadata");
					}
					else if (reader.IsStartElement("EmailAddress", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						string text = reader.ReadElementContentAsString("EmailAddress", "urn:oasis:names:tc:SAML:2.0:metadata");
						if (!string.IsNullOrEmpty(text))
						{
							contactPerson.EmailAddresses.Add(text);
						}
					}
					else if (reader.IsStartElement("TelephoneNumber", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						string text2 = reader.ReadElementContentAsString("TelephoneNumber", "urn:oasis:names:tc:SAML:2.0:metadata");
						if (!string.IsNullOrEmpty(text2))
						{
							contactPerson.TelephoneNumbers.Add(text2);
						}
					}
					else if (!ReadCustomElement(reader, contactPerson))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return contactPerson;
		}

		protected virtual void ReadCustomAttributes<T>(XmlReader reader, T target)
		{
		}

		protected virtual bool ReadCustomElement<T>(XmlReader reader, T target)
		{
			return false;
		}

		protected virtual void ReadCustomRoleDescriptor(string xsiType, XmlReader reader, EntityDescriptor entityDescriptor)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID3274", xsiType));
			reader.Skip();
		}

		protected virtual DisplayClaim ReadDisplayClaim(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			string attribute = reader.GetAttribute("Uri", null);
			if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "ClaimType", attribute)));
			}
			DisplayClaim displayClaim = new DisplayClaim(attribute);
			bool optional = true;
			string attribute2 = reader.GetAttribute("Optional");
			if (!string.IsNullOrEmpty(attribute2))
			{
				try
				{
					optional = XmlConvert.ToBoolean(attribute2.ToLowerInvariant());
				}
				catch (FormatException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "Optional", attribute2)));
				}
			}
			displayClaim.Optional = optional;
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("DisplayName", "http://docs.oasis-open.org/wsfed/authorization/200706"))
					{
						displayClaim.DisplayTag = reader.ReadElementContentAsString("DisplayName", "http://docs.oasis-open.org/wsfed/authorization/200706");
					}
					else if (reader.IsStartElement("Description", "http://docs.oasis-open.org/wsfed/authorization/200706"))
					{
						displayClaim.Description = reader.ReadElementContentAsString("Description", "http://docs.oasis-open.org/wsfed/authorization/200706");
					}
					else
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return displayClaim;
		}

		protected virtual EntitiesDescriptor ReadEntitiesDescriptor(XmlReader reader, SecurityTokenResolver tokenResolver)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			EntitiesDescriptor entitiesDescriptor = CreateEntitiesDescriptorInstance();
			EnvelopedSignatureReader envelopedSignatureReader = new EnvelopedSignatureReader(reader, SecurityTokenSerializer, tokenResolver, requireSignature: false, automaticallyReadSignature: false, resolveIntrinsicSigningKeys: true);
			string attribute = envelopedSignatureReader.GetAttribute("Name", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				entitiesDescriptor.Name = attribute;
			}
			ReadCustomAttributes(envelopedSignatureReader, entitiesDescriptor);
			bool isEmptyElement = envelopedSignatureReader.IsEmptyElement;
			envelopedSignatureReader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (envelopedSignatureReader.IsStartElement())
				{
					if (envelopedSignatureReader.IsStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						entitiesDescriptor.ChildEntities.Add(ReadEntityDescriptor(envelopedSignatureReader, tokenResolver));
					}
					else if (envelopedSignatureReader.IsStartElement("EntitiesDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						entitiesDescriptor.ChildEntityGroups.Add(ReadEntitiesDescriptor(envelopedSignatureReader, tokenResolver));
					}
					else if (!envelopedSignatureReader.TryReadSignature() && !ReadCustomElement(envelopedSignatureReader, entitiesDescriptor))
					{
						envelopedSignatureReader.Skip();
					}
				}
				envelopedSignatureReader.ReadEndElement();
			}
			entitiesDescriptor.SigningCredentials = envelopedSignatureReader.SigningCredentials;
			if (entitiesDescriptor.ChildEntityGroups.Count == 0 && entitiesDescriptor.ChildEntities.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3200", "EntityDescriptor")));
			}
			foreach (EntityDescriptor childEntity in entitiesDescriptor.ChildEntities)
			{
				if (!string.IsNullOrEmpty(childEntity.FederationId) && !StringComparer.Ordinal.Equals(childEntity.FederationId, entitiesDescriptor.Name))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "FederationID", childEntity.FederationId)));
				}
			}
			return entitiesDescriptor;
		}

		protected virtual EntityDescriptor ReadEntityDescriptor(XmlReader inputReader, SecurityTokenResolver tokenResolver)
		{
			if (inputReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("inputReader");
			}
			EntityDescriptor entityDescriptor = CreateEntityDescriptorInstance();
			EnvelopedSignatureReader envelopedSignatureReader = new EnvelopedSignatureReader(inputReader, SecurityTokenSerializer, tokenResolver, requireSignature: false, automaticallyReadSignature: false, resolveIntrinsicSigningKeys: true);
			string attribute = envelopedSignatureReader.GetAttribute("entityID", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				entityDescriptor.EntityId = new EntityId(attribute);
			}
			string attribute2 = envelopedSignatureReader.GetAttribute("FederationID", "http://docs.oasis-open.org/wsfed/federation/200706");
			if (!string.IsNullOrEmpty(attribute2))
			{
				entityDescriptor.FederationId = attribute2;
			}
			ReadCustomAttributes(envelopedSignatureReader, entityDescriptor);
			bool isEmptyElement = envelopedSignatureReader.IsEmptyElement;
			envelopedSignatureReader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (envelopedSignatureReader.IsStartElement())
				{
					if (envelopedSignatureReader.IsStartElement("SPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						entityDescriptor.RoleDescriptors.Add(ReadServiceProviderSingleSignOnDescriptor(envelopedSignatureReader));
					}
					else if (envelopedSignatureReader.IsStartElement("IDPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						entityDescriptor.RoleDescriptors.Add(ReadIdentityProviderSingleSignOnDescriptor(envelopedSignatureReader));
					}
					else if (envelopedSignatureReader.IsStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						string attribute3 = envelopedSignatureReader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
						if (string.IsNullOrEmpty(attribute3))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID0001", "xsi:type", "RoleDescriptor")));
						}
						int num = attribute3.IndexOf(":", 0, StringComparison.Ordinal);
						if (num < 0)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3207", "xsi:type", "RoleDescriptor", attribute3)));
						}
						string text = attribute3.Substring(0, num);
						string text2 = envelopedSignatureReader.LookupNamespace(text);
						if (string.IsNullOrEmpty(text2))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", text, text2)));
						}
						if (!StringComparer.Ordinal.Equals(text2, "http://docs.oasis-open.org/wsfed/federation/200706"))
						{
							ReadCustomRoleDescriptor(attribute3, envelopedSignatureReader, entityDescriptor);
							continue;
						}
						if (StringComparer.Ordinal.Equals(attribute3, text + ":ApplicationServiceType"))
						{
							entityDescriptor.RoleDescriptors.Add(ReadApplicationServiceDescriptor(envelopedSignatureReader));
							continue;
						}
						if (!StringComparer.Ordinal.Equals(attribute3, text + ":SecurityTokenServiceType"))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3207", "xsi:type", "RoleDescriptor", attribute3)));
						}
						entityDescriptor.RoleDescriptors.Add(ReadSecurityTokenServiceDescriptor(envelopedSignatureReader));
					}
					else if (envelopedSignatureReader.IsStartElement("Organization", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						entityDescriptor.Organization = ReadOrganization(envelopedSignatureReader);
					}
					else if (envelopedSignatureReader.IsStartElement("ContactPerson", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						entityDescriptor.Contacts.Add(ReadContactPerson(envelopedSignatureReader));
					}
					else if (!envelopedSignatureReader.TryReadSignature() && !ReadCustomElement(envelopedSignatureReader, entityDescriptor))
					{
						envelopedSignatureReader.Skip();
					}
				}
				envelopedSignatureReader.ReadEndElement();
			}
			entityDescriptor.SigningCredentials = envelopedSignatureReader.SigningCredentials;
			return entityDescriptor;
		}

		protected virtual IdentityProviderSingleSignOnDescriptor ReadIdentityProviderSingleSignOnDescriptor(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			IdentityProviderSingleSignOnDescriptor identityProviderSingleSignOnDescriptor = CreateIdentityProviderSingleSignOnDescriptorInstance();
			ReadSingleSignOnDescriptorAttributes(reader, identityProviderSingleSignOnDescriptor);
			ReadCustomAttributes(reader, identityProviderSingleSignOnDescriptor);
			string attribute = reader.GetAttribute("WantAuthnRequestsSigned");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					identityProviderSingleSignOnDescriptor.WantAuthenticationRequestsSigned = XmlConvert.ToBoolean(attribute.ToLowerInvariant());
				}
				catch (FormatException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "WantAuthnRequestsSigned", attribute)));
				}
			}
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("SingleSignOnService", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						ProtocolEndpoint item = ReadProtocolEndpoint(reader);
						identityProviderSingleSignOnDescriptor.SingleSignOnServices.Add(item);
					}
					else if (reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						identityProviderSingleSignOnDescriptor.SupportedAttributes.Add(ReadAttribute(reader));
					}
					else if (!ReadSingleSignOnDescriptorElement(reader, identityProviderSingleSignOnDescriptor) && !ReadCustomElement(reader, identityProviderSingleSignOnDescriptor))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return identityProviderSingleSignOnDescriptor;
		}

		protected virtual IndexedProtocolEndpoint ReadIndexedProtocolEndpoint(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			IndexedProtocolEndpoint indexedProtocolEndpoint = CreateIndexedProtocolEndpointInstance();
			string attribute = reader.GetAttribute("Binding", null);
			if (!UriUtil.TryCreateValidUri(attribute, UriKind.RelativeOrAbsolute, out var result))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "Binding", attribute)));
			}
			indexedProtocolEndpoint.Binding = result;
			string attribute2 = reader.GetAttribute("Location", null);
			if (!UriUtil.TryCreateValidUri(attribute2, UriKind.RelativeOrAbsolute, out var result2))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "Location", attribute2)));
			}
			indexedProtocolEndpoint.Location = result2;
			string attribute3 = reader.GetAttribute("index", null);
			if (!int.TryParse(attribute3, out var result3))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "index", attribute3)));
			}
			indexedProtocolEndpoint.Index = result3;
			string attribute4 = reader.GetAttribute("ResponseLocation", null);
			if (!string.IsNullOrEmpty(attribute4))
			{
				if (!UriUtil.TryCreateValidUri(attribute4, UriKind.RelativeOrAbsolute, out var result4))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "ResponseLocation", attribute4)));
				}
				indexedProtocolEndpoint.ResponseLocation = result4;
			}
			string attribute5 = reader.GetAttribute("isDefault", null);
			if (!string.IsNullOrEmpty(attribute5))
			{
				try
				{
					indexedProtocolEndpoint.IsDefault = XmlConvert.ToBoolean(attribute5.ToLowerInvariant());
				}
				catch (FormatException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "isDefault", attribute5)));
				}
			}
			ReadCustomAttributes(reader, indexedProtocolEndpoint);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (!ReadCustomElement(reader, indexedProtocolEndpoint))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return indexedProtocolEndpoint;
		}

		protected virtual KeyDescriptor ReadKeyDescriptor(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			KeyDescriptor keyDescriptor = CreateKeyDescriptorInstance();
			string attribute = reader.GetAttribute("use", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				keyDescriptor.Use = GetKeyDescriptorType(attribute);
			}
			ReadCustomAttributes(reader, keyDescriptor);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
					{
						keyDescriptor.KeyInfo = SecurityTokenSerializer.ReadKeyIdentifier(reader);
					}
					else if (reader.IsStartElement("EncryptionMethod", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						string attribute2 = reader.GetAttribute("Algorithm");
						if (!string.IsNullOrEmpty(attribute2) && UriUtil.CanCreateValidUri(attribute2, UriKind.Absolute))
						{
							keyDescriptor.EncryptionMethods.Add(new EncryptionMethod(new Uri(attribute2)));
						}
						isEmptyElement = reader.IsEmptyElement;
						reader.ReadStartElement("EncryptionMethod", "urn:oasis:names:tc:SAML:2.0:metadata");
						if (isEmptyElement)
						{
							continue;
						}
						while (reader.IsStartElement())
						{
							if (!ReadCustomElement(reader, keyDescriptor))
							{
								reader.Skip();
							}
						}
						reader.ReadEndElement();
					}
					else if (!ReadCustomElement(reader, keyDescriptor))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			if (keyDescriptor.KeyInfo == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3200", "KeyInfo")));
			}
			return keyDescriptor;
		}

		protected virtual LocalizedName ReadLocalizedName(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			LocalizedName localizedName = CreateLocalizedNameInstance();
			string attribute = reader.GetAttribute("lang", "http://www.w3.org/XML/1998/namespace");
			try
			{
				localizedName.Language = CultureInfo.GetCultureInfo(attribute);
			}
			catch (ArgumentNullException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "lang", "null")));
			}
			catch (ArgumentException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "lang", attribute)));
			}
			ReadCustomAttributes(reader, localizedName);
			bool isEmptyElement = reader.IsEmptyElement;
			string name = reader.Name;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				localizedName.Name = reader.ReadContentAsString();
				while (reader.IsStartElement())
				{
					if (!ReadCustomElement(reader, localizedName))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			if (string.IsNullOrEmpty(localizedName.Name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3200", name)));
			}
			return localizedName;
		}

		protected virtual LocalizedUri ReadLocalizedUri(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			LocalizedUri localizedUri = CreateLocalizedUriInstance();
			string attribute = reader.GetAttribute("lang", "http://www.w3.org/XML/1998/namespace");
			try
			{
				localizedUri.Language = CultureInfo.GetCultureInfo(attribute);
			}
			catch (ArgumentNullException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "lang", "null")));
			}
			catch (ArgumentException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "lang", attribute)));
			}
			ReadCustomAttributes(reader, localizedUri);
			bool isEmptyElement = reader.IsEmptyElement;
			string name = reader.Name;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				string text = reader.ReadContentAsString();
				if (!UriUtil.TryCreateValidUri(text, UriKind.RelativeOrAbsolute, out var result))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", name, text)));
				}
				localizedUri.Uri = result;
				while (reader.IsStartElement())
				{
					if (!ReadCustomElement(reader, localizedUri))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			if (localizedUri.Uri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3200", name)));
			}
			return localizedUri;
		}

		public MetadataBase ReadMetadata(Stream stream)
		{
			if (stream == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("stream");
			}
			XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max);
			return ReadMetadata(reader);
		}

		public MetadataBase ReadMetadata(XmlReader reader)
		{
			return ReadMetadata(reader, EmptySecurityTokenResolver.Instance);
		}

		public MetadataBase ReadMetadata(XmlReader reader, SecurityTokenResolver tokenResolver)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (tokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenResolver");
			}
			if (!(reader is XmlDictionaryReader))
			{
				reader = XmlDictionaryReader.CreateDictionaryReader(reader);
			}
			return ReadMetadataCore(reader, tokenResolver);
		}

		protected virtual MetadataBase ReadMetadataCore(XmlReader reader, SecurityTokenResolver tokenResolver)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (tokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenResolver");
			}
			if (reader.IsStartElement("EntitiesDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				return ReadEntitiesDescriptor(reader, tokenResolver);
			}
			if (reader.IsStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				return ReadEntityDescriptor(reader, tokenResolver);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3260")));
		}

		protected virtual Organization ReadOrganization(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			Organization organization = CreateOrganizationInstance();
			ReadCustomAttributes(reader, organization);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("OrganizationName", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						organization.Names.Add(ReadLocalizedName(reader));
					}
					else if (reader.IsStartElement("OrganizationDisplayName", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						organization.DisplayNames.Add(ReadLocalizedName(reader));
					}
					else if (reader.IsStartElement("OrganizationURL", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						organization.Urls.Add(ReadLocalizedUri(reader));
					}
					else if (!ReadCustomElement(reader, organization))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return organization;
		}

		protected virtual ProtocolEndpoint ReadProtocolEndpoint(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			ProtocolEndpoint protocolEndpoint = CreateProtocolEndpointInstance();
			string attribute = reader.GetAttribute("Binding", null);
			if (!UriUtil.TryCreateValidUri(attribute, UriKind.RelativeOrAbsolute, out var result))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "Binding", attribute)));
			}
			protocolEndpoint.Binding = result;
			string attribute2 = reader.GetAttribute("Location", null);
			if (!UriUtil.TryCreateValidUri(attribute2, UriKind.RelativeOrAbsolute, out var result2))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "Location", attribute2)));
			}
			protocolEndpoint.Location = result2;
			string attribute3 = reader.GetAttribute("ResponseLocation", null);
			if (!string.IsNullOrEmpty(attribute3))
			{
				if (!UriUtil.TryCreateValidUri(attribute3, UriKind.RelativeOrAbsolute, out var result3))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "ResponseLocation", attribute3)));
				}
				protocolEndpoint.ResponseLocation = result3;
			}
			ReadCustomAttributes(reader, protocolEndpoint);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (!ReadCustomElement(reader, protocolEndpoint))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return protocolEndpoint;
		}

		protected virtual void ReadRoleDescriptorAttributes(XmlReader reader, RoleDescriptor roleDescriptor)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (roleDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor");
			}
			if (roleDescriptor.ProtocolsSupported == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.ProtocolsSupported");
			}
			string attribute = reader.GetAttribute("validUntil", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				if (!DateTime.TryParse(attribute, out var result))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "validUntil", attribute)));
				}
				roleDescriptor.ValidUntil = result;
			}
			string attribute2 = reader.GetAttribute("errorURL", null);
			if (!string.IsNullOrEmpty(attribute2))
			{
				if (!UriUtil.TryCreateValidUri(attribute2, UriKind.RelativeOrAbsolute, out var result2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "errorURL", attribute2)));
				}
				roleDescriptor.ErrorUrl = result2;
			}
			string attribute3 = reader.GetAttribute("protocolSupportEnumeration", null);
			if (string.IsNullOrEmpty(attribute3))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "protocolSupportEnumeration", attribute3)));
			}
			string[] array = attribute3.Split(' ');
			foreach (string text in array)
			{
				string text2 = text.Trim();
				if (!string.IsNullOrEmpty(text2))
				{
					roleDescriptor.ProtocolsSupported.Add(new Uri(text2));
				}
			}
			ReadCustomAttributes(reader, roleDescriptor);
		}

		protected virtual bool ReadRoleDescriptorElement(XmlReader reader, RoleDescriptor roleDescriptor)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (roleDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor");
			}
			if (roleDescriptor.Contacts == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Contacts");
			}
			if (roleDescriptor.Keys == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Keys");
			}
			if (reader.IsStartElement("Organization", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				roleDescriptor.Organization = ReadOrganization(reader);
				return true;
			}
			if (reader.IsStartElement("KeyDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				roleDescriptor.Keys.Add(ReadKeyDescriptor(reader));
				return true;
			}
			if (reader.IsStartElement("ContactPerson", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				roleDescriptor.Contacts.Add(ReadContactPerson(reader));
				return true;
			}
			return ReadCustomElement(reader, roleDescriptor);
		}

		protected virtual SecurityTokenServiceDescriptor ReadSecurityTokenServiceDescriptor(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			SecurityTokenServiceDescriptor securityTokenServiceDescriptor = CreateSecurityTokenServiceDescriptorInstance();
			ReadWebServiceDescriptorAttributes(reader, securityTokenServiceDescriptor);
			ReadCustomAttributes(reader, securityTokenServiceDescriptor);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("SecurityTokenServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
					{
						isEmptyElement = reader.IsEmptyElement;
						reader.ReadStartElement();
						if (!isEmptyElement && reader.IsStartElement())
						{
							EndpointAddress item = EndpointAddress.ReadFrom(GetAddressingVersion(reader.NamespaceURI), reader);
							securityTokenServiceDescriptor.SecurityTokenServiceEndpoints.Add(item);
							reader.ReadEndElement();
						}
					}
					else if (reader.IsStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706"))
					{
						isEmptyElement = reader.IsEmptyElement;
						reader.ReadStartElement();
						if (!isEmptyElement && reader.IsStartElement())
						{
							EndpointAddress item2 = EndpointAddress.ReadFrom(GetAddressingVersion(reader.NamespaceURI), reader);
							securityTokenServiceDescriptor.PassiveRequestorEndpoints.Add(item2);
							reader.ReadEndElement();
						}
					}
					else if (!ReadWebServiceDescriptorElement(reader, securityTokenServiceDescriptor) && !ReadCustomElement(reader, securityTokenServiceDescriptor))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return securityTokenServiceDescriptor;
		}

		protected virtual ServiceProviderSingleSignOnDescriptor ReadServiceProviderSingleSignOnDescriptor(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			ServiceProviderSingleSignOnDescriptor serviceProviderSingleSignOnDescriptor = CreateServiceProviderSingleSignOnDescriptorInstance();
			string attribute = reader.GetAttribute("AuthnRequestsSigned");
			if (!string.IsNullOrEmpty(attribute))
			{
				try
				{
					serviceProviderSingleSignOnDescriptor.AuthenticationRequestsSigned = XmlConvert.ToBoolean(attribute.ToLowerInvariant());
				}
				catch (FormatException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "AuthnRequestsSigned", attribute)));
				}
			}
			string attribute2 = reader.GetAttribute("WantAssertionsSigned");
			if (!string.IsNullOrEmpty(attribute2))
			{
				try
				{
					serviceProviderSingleSignOnDescriptor.WantAssertionsSigned = XmlConvert.ToBoolean(attribute2.ToLowerInvariant());
				}
				catch (FormatException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "WantAssertionsSigned", attribute2)));
				}
			}
			ReadSingleSignOnDescriptorAttributes(reader, serviceProviderSingleSignOnDescriptor);
			ReadCustomAttributes(reader, serviceProviderSingleSignOnDescriptor);
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("AssertionConsumerService", "urn:oasis:names:tc:SAML:2.0:metadata"))
					{
						IndexedProtocolEndpoint indexedProtocolEndpoint = ReadIndexedProtocolEndpoint(reader);
						serviceProviderSingleSignOnDescriptor.AssertionConsumerService.Add(indexedProtocolEndpoint.Index, indexedProtocolEndpoint);
					}
					else if (!ReadSingleSignOnDescriptorElement(reader, serviceProviderSingleSignOnDescriptor) && !ReadCustomElement(reader, serviceProviderSingleSignOnDescriptor))
					{
						reader.Skip();
					}
				}
				reader.ReadEndElement();
			}
			return serviceProviderSingleSignOnDescriptor;
		}

		protected virtual void ReadSingleSignOnDescriptorAttributes(XmlReader reader, SingleSignOnDescriptor roleDescriptor)
		{
			ReadRoleDescriptorAttributes(reader, roleDescriptor);
			ReadCustomAttributes(reader, roleDescriptor);
		}

		protected virtual bool ReadSingleSignOnDescriptorElement(XmlReader reader, SingleSignOnDescriptor ssoDescriptor)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (ssoDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("ssoDescriptor");
			}
			if (ReadRoleDescriptorElement(reader, ssoDescriptor))
			{
				return true;
			}
			if (reader.IsStartElement("ArtifactResolutionService", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				IndexedProtocolEndpoint indexedProtocolEndpoint = ReadIndexedProtocolEndpoint(reader);
				ssoDescriptor.ArtifactResolutionServices.Add(indexedProtocolEndpoint.Index, indexedProtocolEndpoint);
				return true;
			}
			if (reader.IsStartElement("SingleLogoutService", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				ssoDescriptor.SingleLogoutServices.Add(ReadProtocolEndpoint(reader));
				return true;
			}
			if (reader.IsStartElement("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:metadata"))
			{
				string uriString = reader.ReadElementContentAsString("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:metadata");
				if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID0014", "NameIDFormat")));
				}
				ssoDescriptor.NameIdentifierFormats.Add(new Uri(uriString));
				return true;
			}
			return ReadCustomElement(reader, ssoDescriptor);
		}

		protected virtual void ReadWebServiceDescriptorAttributes(XmlReader reader, WebServiceDescriptor roleDescriptor)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (roleDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor");
			}
			ReadRoleDescriptorAttributes(reader, roleDescriptor);
			string attribute = reader.GetAttribute("ServiceDisplayName", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				roleDescriptor.ServiceDisplayName = attribute;
			}
			string attribute2 = reader.GetAttribute("ServiceDescription", null);
			if (!string.IsNullOrEmpty(attribute2))
			{
				roleDescriptor.ServiceDescription = attribute2;
			}
			ReadCustomAttributes(reader, roleDescriptor);
		}

		public virtual bool ReadWebServiceDescriptorElement(XmlReader reader, WebServiceDescriptor roleDescriptor)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (roleDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor");
			}
			if (roleDescriptor.TargetScopes == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.TargetScopes");
			}
			if (roleDescriptor.ClaimTypesOffered == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.TargetScopes");
			}
			if (roleDescriptor.TokenTypesOffered == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.TokenTypesOffered");
			}
			if (ReadRoleDescriptorElement(reader, roleDescriptor))
			{
				return true;
			}
			if (reader.IsStartElement("TargetScopes", "http://docs.oasis-open.org/wsfed/federation/200706"))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				reader.ReadStartElement();
				if (!isEmptyElement)
				{
					while (reader.IsStartElement())
					{
						roleDescriptor.TargetScopes.Add(EndpointAddress.ReadFrom(GetAddressingVersion(reader.NamespaceURI), reader));
					}
					reader.ReadEndElement();
				}
				return true;
			}
			if (reader.IsStartElement("ClaimTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706"))
			{
				bool isEmptyElement2 = reader.IsEmptyElement;
				reader.ReadStartElement();
				if (!isEmptyElement2)
				{
					while (reader.IsStartElement())
					{
						if (reader.IsStartElement("ClaimType", "http://docs.oasis-open.org/wsfed/authorization/200706"))
						{
							roleDescriptor.ClaimTypesOffered.Add(ReadDisplayClaim(reader));
						}
						else
						{
							reader.Skip();
						}
					}
					reader.ReadEndElement();
				}
				return true;
			}
			if (reader.IsStartElement("ClaimTypesRequested", "http://docs.oasis-open.org/wsfed/federation/200706"))
			{
				bool isEmptyElement3 = reader.IsEmptyElement;
				reader.ReadStartElement();
				if (!isEmptyElement3)
				{
					while (reader.IsStartElement())
					{
						if (reader.IsStartElement("ClaimType", "http://docs.oasis-open.org/wsfed/authorization/200706"))
						{
							roleDescriptor.ClaimTypesRequested.Add(ReadDisplayClaim(reader));
						}
						else
						{
							reader.Skip();
						}
					}
					reader.ReadEndElement();
				}
				return true;
			}
			if (reader.IsStartElement("TokenTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706"))
			{
				bool isEmptyElement4 = reader.IsEmptyElement;
				reader.ReadStartElement("TokenTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706");
				if (!isEmptyElement4)
				{
					while (reader.IsStartElement())
					{
						if (reader.IsStartElement("TokenType", "http://docs.oasis-open.org/wsfed/federation/200706"))
						{
							string attribute = reader.GetAttribute("Uri", null);
							if (!UriUtil.TryCreateValidUri(attribute, UriKind.Absolute, out var result))
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3202", "TokenType", attribute)));
							}
							roleDescriptor.TokenTypesOffered.Add(result);
							isEmptyElement4 = reader.IsEmptyElement;
							reader.ReadStartElement();
							if (!isEmptyElement4)
							{
								reader.ReadEndElement();
							}
						}
						else
						{
							reader.Skip();
						}
					}
					reader.ReadEndElement();
				}
				return true;
			}
			return ReadCustomElement(reader, roleDescriptor);
		}

		protected virtual void WriteApplicationServiceDescriptor(XmlWriter writer, ApplicationServiceDescriptor appService)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (appService == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("appService");
			}
			if (appService.Endpoints == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("appService.Endpoints");
			}
			if (appService.PassiveRequestorEndpoints == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("appService.PassiveRequestorEndpoints");
			}
			writer.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "fed:ApplicationServiceType");
			writer.WriteAttributeString("xmlns", "fed", null, "http://docs.oasis-open.org/wsfed/federation/200706");
			WriteWebServiceDescriptorAttributes(writer, appService);
			WriteCustomAttributes(writer, appService);
			WriteWebServiceDescriptorElements(writer, appService);
			foreach (EndpointAddress endpoint in appService.Endpoints)
			{
				writer.WriteStartElement("ApplicationServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
				endpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
				writer.WriteEndElement();
			}
			foreach (EndpointAddress passiveRequestorEndpoint in appService.PassiveRequestorEndpoints)
			{
				writer.WriteStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
				passiveRequestorEndpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
				writer.WriteEndElement();
			}
			WriteCustomElements(writer, appService);
			writer.WriteEndElement();
		}

		protected virtual void WriteContactPerson(XmlWriter writer, ContactPerson contactPerson)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (contactPerson == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contactPerson");
			}
			if (contactPerson.EmailAddresses == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contactPerson.EmailAddresses");
			}
			if (contactPerson.TelephoneNumbers == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contactPerson.TelephoneNumbers");
			}
			writer.WriteStartElement("ContactPerson", "urn:oasis:names:tc:SAML:2.0:metadata");
			if (contactPerson.Type == ContactType.Unspecified)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "contactType")));
			}
			writer.WriteAttributeString("contactType", null, contactPerson.Type.ToString().ToLowerInvariant());
			WriteCustomAttributes(writer, contactPerson);
			if (!string.IsNullOrEmpty(contactPerson.Company))
			{
				writer.WriteElementString("Company", "urn:oasis:names:tc:SAML:2.0:metadata", contactPerson.Company);
			}
			if (!string.IsNullOrEmpty(contactPerson.GivenName))
			{
				writer.WriteElementString("GivenName", "urn:oasis:names:tc:SAML:2.0:metadata", contactPerson.GivenName);
			}
			if (!string.IsNullOrEmpty(contactPerson.Surname))
			{
				writer.WriteElementString("SurName", "urn:oasis:names:tc:SAML:2.0:metadata", contactPerson.Surname);
			}
			foreach (string emailAddress in contactPerson.EmailAddresses)
			{
				writer.WriteElementString("EmailAddress", "urn:oasis:names:tc:SAML:2.0:metadata", emailAddress);
			}
			foreach (string telephoneNumber in contactPerson.TelephoneNumbers)
			{
				writer.WriteElementString("TelephoneNumber", "urn:oasis:names:tc:SAML:2.0:metadata", telephoneNumber);
			}
			WriteCustomElements(writer, contactPerson);
			writer.WriteEndElement();
		}

		protected virtual void WriteCustomAttributes<T>(XmlWriter writer, T source)
		{
		}

		protected virtual void WriteCustomElements<T>(XmlWriter writer, T source)
		{
		}

		protected virtual void WriteProtocolEndpoint(XmlWriter writer, ProtocolEndpoint endpoint, XmlQualifiedName element)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (endpoint == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpoint");
			}
			if (element == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("element");
			}
			writer.WriteStartElement(element.Name, element.Namespace);
			if (endpoint.Binding == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "Binding")));
			}
			writer.WriteAttributeString("Binding", null, endpoint.Binding.IsAbsoluteUri ? endpoint.Binding.AbsoluteUri : endpoint.Binding.ToString());
			if (endpoint.Location == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "Location")));
			}
			writer.WriteAttributeString("Location", null, endpoint.Location.IsAbsoluteUri ? endpoint.Location.AbsoluteUri : endpoint.Location.ToString());
			if (endpoint.ResponseLocation != null)
			{
				writer.WriteAttributeString("ResponseLocation", null, endpoint.ResponseLocation.IsAbsoluteUri ? endpoint.ResponseLocation.AbsoluteUri : endpoint.ResponseLocation.ToString());
			}
			WriteCustomAttributes(writer, endpoint);
			WriteCustomElements(writer, endpoint);
			writer.WriteEndElement();
		}

		protected virtual void WriteDisplayClaim(XmlWriter writer, DisplayClaim claim)
		{
			writer.WriteStartElement("auth", "ClaimType", "http://docs.oasis-open.org/wsfed/authorization/200706");
			if (string.IsNullOrEmpty(claim.ClaimType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "ClaimType")));
			}
			if (!UriUtil.CanCreateValidUri(claim.ClaimType, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID0014", claim.ClaimType)));
			}
			writer.WriteAttributeString("Uri", claim.ClaimType);
			writer.WriteAttributeString("Optional", XmlConvert.ToString(claim.Optional));
			if (!string.IsNullOrEmpty(claim.DisplayTag))
			{
				writer.WriteElementString("auth", "DisplayName", "http://docs.oasis-open.org/wsfed/authorization/200706", claim.DisplayTag);
			}
			if (!string.IsNullOrEmpty(claim.Description))
			{
				writer.WriteElementString("auth", "Description", "http://docs.oasis-open.org/wsfed/authorization/200706", claim.Description);
			}
			writer.WriteEndElement();
		}

		protected virtual void WriteEntitiesDescriptor(XmlWriter inputWriter, EntitiesDescriptor entitiesDescriptor)
		{
			if (inputWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("inputWriter");
			}
			if (entitiesDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entitiesDescriptor");
			}
			if (entitiesDescriptor.ChildEntities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entitiesDescriptor.ChildEntities");
			}
			if (entitiesDescriptor.ChildEntityGroups == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entitiesDescriptor.ChildEntityGroups");
			}
			string text = "_" + Guid.NewGuid().ToString();
			XmlWriter xmlWriter = inputWriter;
			EnvelopedSignatureWriter envelopedSignatureWriter = null;
			if (entitiesDescriptor.SigningCredentials != null)
			{
				envelopedSignatureWriter = new EnvelopedSignatureWriter(inputWriter, entitiesDescriptor.SigningCredentials, text, SecurityTokenSerializer);
				xmlWriter = envelopedSignatureWriter;
			}
			xmlWriter.WriteStartElement("EntitiesDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			xmlWriter.WriteAttributeString("ID", null, text);
			if (entitiesDescriptor.ChildEntities.Count == 0 && entitiesDescriptor.ChildEntityGroups.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "EntitiesDescriptor")));
			}
			foreach (EntityDescriptor childEntity in entitiesDescriptor.ChildEntities)
			{
				if (!string.IsNullOrEmpty(childEntity.FederationId) && !StringComparer.Ordinal.Equals(childEntity.FederationId, entitiesDescriptor.Name))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "FederationID")));
				}
			}
			if (!string.IsNullOrEmpty(entitiesDescriptor.Name))
			{
				xmlWriter.WriteAttributeString("Name", null, entitiesDescriptor.Name);
			}
			WriteCustomAttributes(xmlWriter, entitiesDescriptor);
			envelopedSignatureWriter?.WriteSignature();
			foreach (EntityDescriptor childEntity2 in entitiesDescriptor.ChildEntities)
			{
				WriteEntityDescriptor(xmlWriter, childEntity2);
			}
			foreach (EntitiesDescriptor childEntityGroup in entitiesDescriptor.ChildEntityGroups)
			{
				WriteEntitiesDescriptor(xmlWriter, childEntityGroup);
			}
			WriteCustomElements(xmlWriter, entitiesDescriptor);
			xmlWriter.WriteEndElement();
		}

		protected virtual void WriteEntityDescriptor(XmlWriter inputWriter, EntityDescriptor entityDescriptor)
		{
			if (inputWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("inputWriter");
			}
			if (entityDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entityDescriptor");
			}
			if (entityDescriptor.Contacts == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entityDescriptor.Contacts");
			}
			if (entityDescriptor.RoleDescriptors == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entityDescriptor.RoleDescriptors");
			}
			string text = "_" + Guid.NewGuid().ToString();
			XmlWriter xmlWriter = inputWriter;
			EnvelopedSignatureWriter envelopedSignatureWriter = null;
			if (entityDescriptor.SigningCredentials != null)
			{
				envelopedSignatureWriter = new EnvelopedSignatureWriter(inputWriter, entityDescriptor.SigningCredentials, text, SecurityTokenSerializer);
				xmlWriter = envelopedSignatureWriter;
			}
			xmlWriter.WriteStartElement("EntityDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			xmlWriter.WriteAttributeString("ID", null, text);
			if (entityDescriptor.EntityId == null || entityDescriptor.EntityId.Id == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "entityID")));
			}
			xmlWriter.WriteAttributeString("entityID", null, entityDescriptor.EntityId.Id);
			if (!string.IsNullOrEmpty(entityDescriptor.FederationId))
			{
				xmlWriter.WriteAttributeString("FederationID", "http://docs.oasis-open.org/wsfed/federation/200706", entityDescriptor.FederationId);
			}
			WriteCustomAttributes(xmlWriter, entityDescriptor);
			envelopedSignatureWriter?.WriteSignature();
			if (entityDescriptor.RoleDescriptors.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "RoleDescriptor")));
			}
			foreach (RoleDescriptor roleDescriptor in entityDescriptor.RoleDescriptors)
			{
				ServiceProviderSingleSignOnDescriptor serviceProviderSingleSignOnDescriptor = roleDescriptor as ServiceProviderSingleSignOnDescriptor;
				if (serviceProviderSingleSignOnDescriptor != null)
				{
					WriteServiceProviderSingleSignOnDescriptor(xmlWriter, serviceProviderSingleSignOnDescriptor);
				}
				IdentityProviderSingleSignOnDescriptor identityProviderSingleSignOnDescriptor = roleDescriptor as IdentityProviderSingleSignOnDescriptor;
				if (identityProviderSingleSignOnDescriptor != null)
				{
					WriteIdentityProviderSingleSignOnDescriptor(xmlWriter, identityProviderSingleSignOnDescriptor);
				}
				ApplicationServiceDescriptor applicationServiceDescriptor = roleDescriptor as ApplicationServiceDescriptor;
				if (applicationServiceDescriptor != null)
				{
					WriteApplicationServiceDescriptor(xmlWriter, applicationServiceDescriptor);
				}
				SecurityTokenServiceDescriptor securityTokenServiceDescriptor = roleDescriptor as SecurityTokenServiceDescriptor;
				if (securityTokenServiceDescriptor != null)
				{
					WriteSecurityTokenServiceDescriptor(xmlWriter, securityTokenServiceDescriptor);
				}
			}
			if (entityDescriptor.Organization != null)
			{
				WriteOrganization(xmlWriter, entityDescriptor.Organization);
			}
			foreach (ContactPerson contact in entityDescriptor.Contacts)
			{
				WriteContactPerson(xmlWriter, contact);
			}
			WriteCustomElements(xmlWriter, entityDescriptor);
			xmlWriter.WriteEndElement();
		}

		protected virtual void WriteIdentityProviderSingleSignOnDescriptor(XmlWriter writer, IdentityProviderSingleSignOnDescriptor idpssoDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (idpssoDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("idpssoDescriptor");
			}
			if (idpssoDescriptor.SupportedAttributes == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("idpssoDescriptor.SupportedAttributes");
			}
			if (idpssoDescriptor.SingleSignOnServices == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("idpssoDescriptor.SingleSignOnServices");
			}
			writer.WriteStartElement("IDPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			if (idpssoDescriptor.WantAuthenticationRequestsSigned)
			{
				writer.WriteAttributeString("WantAuthnRequestsSigned", null, XmlConvert.ToString(idpssoDescriptor.WantAuthenticationRequestsSigned));
			}
			WriteSingleSignOnDescriptorAttributes(writer, idpssoDescriptor);
			WriteCustomAttributes(writer, idpssoDescriptor);
			WriteSingleSignOnDescriptorElements(writer, idpssoDescriptor);
			if (idpssoDescriptor.SingleSignOnServices.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "SingleSignOnService")));
			}
			foreach (ProtocolEndpoint singleSignOnService in idpssoDescriptor.SingleSignOnServices)
			{
				if (singleSignOnService.ResponseLocation != null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3249", "ResponseLocation")));
				}
				XmlQualifiedName element = new XmlQualifiedName("SingleSignOnService", "urn:oasis:names:tc:SAML:2.0:metadata");
				WriteProtocolEndpoint(writer, singleSignOnService, element);
			}
			foreach (Saml2Attribute supportedAttribute in idpssoDescriptor.SupportedAttributes)
			{
				WriteAttribute(writer, supportedAttribute);
			}
			WriteCustomElements(writer, idpssoDescriptor);
			writer.WriteEndElement();
		}

		protected virtual void WriteIndexedProtocolEndpoint(XmlWriter writer, IndexedProtocolEndpoint indexedEP, XmlQualifiedName element)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (indexedEP == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("indexedEP");
			}
			if (element == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("element");
			}
			writer.WriteStartElement(element.Name, element.Namespace);
			if (indexedEP.Binding == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "Binding")));
			}
			writer.WriteAttributeString("Binding", null, indexedEP.Binding.IsAbsoluteUri ? indexedEP.Binding.AbsoluteUri : indexedEP.Binding.ToString());
			if (indexedEP.Location == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "Location")));
			}
			writer.WriteAttributeString("Location", null, indexedEP.Location.IsAbsoluteUri ? indexedEP.Location.AbsoluteUri : indexedEP.Location.ToString());
			if (indexedEP.Index < 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "index")));
			}
			writer.WriteAttributeString("index", null, indexedEP.Index.ToString(CultureInfo.InvariantCulture));
			if (indexedEP.ResponseLocation != null)
			{
				writer.WriteAttributeString("ResponseLocation", null, indexedEP.ResponseLocation.IsAbsoluteUri ? indexedEP.ResponseLocation.AbsoluteUri : indexedEP.ResponseLocation.ToString());
			}
			if (indexedEP.IsDefault.HasValue)
			{
				writer.WriteAttributeString("isDefault", null, XmlConvert.ToString(indexedEP.IsDefault.Value));
			}
			WriteCustomAttributes(writer, indexedEP);
			WriteCustomElements(writer, indexedEP);
			writer.WriteEndElement();
		}

		protected virtual void WriteKeyDescriptor(XmlWriter writer, KeyDescriptor keyDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (keyDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyDescriptor");
			}
			writer.WriteStartElement("KeyDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			if (keyDescriptor.Use == KeyType.Encryption || keyDescriptor.Use == KeyType.Signing)
			{
				writer.WriteAttributeString("use", null, keyDescriptor.Use.ToString().ToLowerInvariant());
			}
			WriteCustomAttributes(writer, keyDescriptor);
			if (keyDescriptor.KeyInfo == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "KeyInfo")));
			}
			SecurityTokenSerializer.WriteKeyIdentifier(writer, keyDescriptor.KeyInfo);
			if (keyDescriptor.EncryptionMethods != null && keyDescriptor.EncryptionMethods.Count > 0)
			{
				foreach (EncryptionMethod encryptionMethod in keyDescriptor.EncryptionMethods)
				{
					if (encryptionMethod.Algorithm == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "Algorithm")));
					}
					if (!encryptionMethod.Algorithm.IsAbsoluteUri)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID0014", "Algorithm")));
					}
					writer.WriteStartElement("EncryptionMethod", "urn:oasis:names:tc:SAML:2.0:metadata");
					writer.WriteAttributeString("Algorithm", null, encryptionMethod.Algorithm.AbsoluteUri);
					writer.WriteEndElement();
				}
			}
			WriteCustomElements(writer, keyDescriptor);
			writer.WriteEndElement();
		}

		protected virtual void WriteLocalizedName(XmlWriter writer, LocalizedName name, XmlQualifiedName element)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (name == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name");
			}
			if (element == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("element");
			}
			if (name.Name == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name.Name");
			}
			writer.WriteStartElement(element.Name, element.Namespace);
			if (name.Language == null || string.IsNullOrEmpty(name.Name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "lang")));
			}
			writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", name.Language.Name);
			WriteCustomAttributes(writer, name);
			writer.WriteString(name.Name);
			WriteCustomElements(writer, name);
			writer.WriteEndElement();
		}

		protected virtual void WriteLocalizedUri(XmlWriter writer, LocalizedUri uri, XmlQualifiedName element)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (uri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("uri");
			}
			if (element == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("element");
			}
			writer.WriteStartElement(element.Name, element.Namespace);
			if (uri.Language == null || uri.Uri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "lang")));
			}
			writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", uri.Language.Name);
			WriteCustomAttributes(writer, uri);
			writer.WriteString(uri.Uri.IsAbsoluteUri ? uri.Uri.AbsoluteUri : uri.Uri.ToString());
			WriteCustomElements(writer, uri);
			writer.WriteEndElement();
		}

		public void WriteMetadata(Stream stream, MetadataBase metadata)
		{
			if (stream == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("stream");
			}
			if (metadata == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("metadata");
			}
			using XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false);
			WriteMetadata(writer, metadata);
		}

		public void WriteMetadata(XmlWriter writer, MetadataBase metadata)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (metadata == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("metadata");
			}
			WriteMetadataCore(writer, metadata);
		}

		protected virtual void WriteMetadataCore(XmlWriter writer, MetadataBase metadataBase)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (metadataBase == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("metadataBase");
			}
			EntitiesDescriptor entitiesDescriptor = metadataBase as EntitiesDescriptor;
			if (entitiesDescriptor != null)
			{
				WriteEntitiesDescriptor(writer, entitiesDescriptor);
				return;
			}
			EntityDescriptor entityDescriptor = metadataBase as EntityDescriptor;
			if (entityDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "EntitiesDescriptor")));
			}
			WriteEntityDescriptor(writer, entityDescriptor);
		}

		protected virtual void WriteOrganization(XmlWriter writer, Organization organization)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (organization == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization");
			}
			if (organization.DisplayNames == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization.DisplayNames");
			}
			if (organization.Names == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization.Names");
			}
			if (organization.Urls == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("organization.Urls");
			}
			writer.WriteStartElement("Organization", "urn:oasis:names:tc:SAML:2.0:metadata");
			if (organization.Names.Count < 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "OrganizationName")));
			}
			foreach (LocalizedName name in organization.Names)
			{
				XmlQualifiedName element = new XmlQualifiedName("OrganizationName", "urn:oasis:names:tc:SAML:2.0:metadata");
				WriteLocalizedName(writer, name, element);
			}
			if (organization.DisplayNames.Count < 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "OrganizationDisplayName")));
			}
			foreach (LocalizedName displayName in organization.DisplayNames)
			{
				XmlQualifiedName element2 = new XmlQualifiedName("OrganizationDisplayName", "urn:oasis:names:tc:SAML:2.0:metadata");
				WriteLocalizedName(writer, displayName, element2);
			}
			if (organization.Urls.Count < 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "OrganizationURL")));
			}
			foreach (LocalizedUri url in organization.Urls)
			{
				XmlQualifiedName element3 = new XmlQualifiedName("OrganizationURL", "urn:oasis:names:tc:SAML:2.0:metadata");
				WriteLocalizedUri(writer, url, element3);
			}
			WriteCustomAttributes(writer, organization);
			WriteCustomElements(writer, organization);
			writer.WriteEndElement();
		}

		protected virtual void WriteRoleDescriptorAttributes(XmlWriter writer, RoleDescriptor roleDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (roleDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor");
			}
			if (roleDescriptor.ProtocolsSupported == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.ProtocolsSupported");
			}
			_ = roleDescriptor.ValidUntil;
			if (roleDescriptor.ValidUntil != DateTime.MaxValue)
			{
				writer.WriteAttributeString("validUntil", null, roleDescriptor.ValidUntil.ToString("s", CultureInfo.InvariantCulture));
			}
			if (roleDescriptor.ErrorUrl != null)
			{
				writer.WriteAttributeString("errorURL", null, roleDescriptor.ErrorUrl.IsAbsoluteUri ? roleDescriptor.ErrorUrl.AbsoluteUri : roleDescriptor.ErrorUrl.ToString());
			}
			if (roleDescriptor.ProtocolsSupported.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "protocolSupportEnumeration")));
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Uri item in roleDescriptor.ProtocolsSupported)
			{
				stringBuilder.AppendFormat("{0} ", item.IsAbsoluteUri ? item.AbsoluteUri : item.ToString());
			}
			string text = stringBuilder.ToString();
			writer.WriteAttributeString("protocolSupportEnumeration", null, text.Trim());
			WriteCustomAttributes(writer, roleDescriptor);
		}

		protected virtual void WriteRoleDescriptorElements(XmlWriter writer, RoleDescriptor roleDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (roleDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor");
			}
			if (roleDescriptor.Contacts == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Contacts");
			}
			if (roleDescriptor.Keys == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("roleDescriptor.Keys");
			}
			if (roleDescriptor.Organization != null)
			{
				WriteOrganization(writer, roleDescriptor.Organization);
			}
			foreach (KeyDescriptor key in roleDescriptor.Keys)
			{
				WriteKeyDescriptor(writer, key);
			}
			foreach (ContactPerson contact in roleDescriptor.Contacts)
			{
				WriteContactPerson(writer, contact);
			}
			WriteCustomElements(writer, roleDescriptor);
		}

		protected virtual void WriteSecurityTokenServiceDescriptor(XmlWriter writer, SecurityTokenServiceDescriptor securityTokenServiceDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (securityTokenServiceDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceDescriptor");
			}
			if (securityTokenServiceDescriptor.SecurityTokenServiceEndpoints == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceDescriptor.Endpoints");
			}
			if (securityTokenServiceDescriptor.PassiveRequestorEndpoints == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceDescriptor.PassiveRequestorEndpoints");
			}
			writer.WriteStartElement("RoleDescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			writer.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", "fed:SecurityTokenServiceType");
			writer.WriteAttributeString("xmlns", "fed", null, "http://docs.oasis-open.org/wsfed/federation/200706");
			WriteWebServiceDescriptorAttributes(writer, securityTokenServiceDescriptor);
			WriteCustomAttributes(writer, securityTokenServiceDescriptor);
			WriteWebServiceDescriptorElements(writer, securityTokenServiceDescriptor);
			if (securityTokenServiceDescriptor.SecurityTokenServiceEndpoints.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "SecurityTokenServiceEndpoint")));
			}
			foreach (EndpointAddress securityTokenServiceEndpoint in securityTokenServiceDescriptor.SecurityTokenServiceEndpoints)
			{
				writer.WriteStartElement("SecurityTokenServiceEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
				securityTokenServiceEndpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
				writer.WriteEndElement();
			}
			foreach (EndpointAddress passiveRequestorEndpoint in securityTokenServiceDescriptor.PassiveRequestorEndpoints)
			{
				writer.WriteStartElement("PassiveRequestorEndpoint", "http://docs.oasis-open.org/wsfed/federation/200706");
				passiveRequestorEndpoint.WriteTo(AddressingVersion.WSAddressing10, writer);
				writer.WriteEndElement();
			}
			WriteCustomElements(writer, securityTokenServiceDescriptor);
			writer.WriteEndElement();
		}

		protected virtual void WriteServiceProviderSingleSignOnDescriptor(XmlWriter writer, ServiceProviderSingleSignOnDescriptor spssoDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (spssoDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("spssoDescriptor");
			}
			if (spssoDescriptor.AssertionConsumerService == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("spssoDescriptor.AssertionConsumerService");
			}
			writer.WriteStartElement("SPSSODescriptor", "urn:oasis:names:tc:SAML:2.0:metadata");
			if (spssoDescriptor.AuthenticationRequestsSigned)
			{
				writer.WriteAttributeString("AuthnRequestsSigned", null, XmlConvert.ToString(spssoDescriptor.AuthenticationRequestsSigned));
			}
			if (spssoDescriptor.WantAssertionsSigned)
			{
				writer.WriteAttributeString("WantAssertionsSigned", null, XmlConvert.ToString(spssoDescriptor.WantAssertionsSigned));
			}
			WriteSingleSignOnDescriptorAttributes(writer, spssoDescriptor);
			WriteCustomAttributes(writer, spssoDescriptor);
			WriteSingleSignOnDescriptorElements(writer, spssoDescriptor);
			if (spssoDescriptor.AssertionConsumerService.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "AssertionConsumerService")));
			}
			foreach (IndexedProtocolEndpoint value in spssoDescriptor.AssertionConsumerService.Values)
			{
				XmlQualifiedName element = new XmlQualifiedName("AssertionConsumerService", "urn:oasis:names:tc:SAML:2.0:metadata");
				WriteIndexedProtocolEndpoint(writer, value, element);
			}
			WriteCustomElements(writer, spssoDescriptor);
			writer.WriteEndElement();
		}

		protected virtual void WriteSingleSignOnDescriptorAttributes(XmlWriter writer, SingleSignOnDescriptor ssoDescriptor)
		{
			WriteRoleDescriptorAttributes(writer, ssoDescriptor);
			WriteCustomAttributes(writer, ssoDescriptor);
		}

		protected virtual void WriteSingleSignOnDescriptorElements(XmlWriter writer, SingleSignOnDescriptor ssoDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (ssoDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("ssoDescriptor");
			}
			WriteRoleDescriptorElements(writer, ssoDescriptor);
			if (ssoDescriptor.ArtifactResolutionServices != null && ssoDescriptor.ArtifactResolutionServices.Count > 0)
			{
				foreach (IndexedProtocolEndpoint value in ssoDescriptor.ArtifactResolutionServices.Values)
				{
					if (value.ResponseLocation != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3249", "ResponseLocation")));
					}
					XmlQualifiedName element = new XmlQualifiedName("ArtifactResolutionService", "urn:oasis:names:tc:SAML:2.0:metadata");
					WriteIndexedProtocolEndpoint(writer, value, element);
				}
			}
			if (ssoDescriptor.SingleLogoutServices != null && ssoDescriptor.SingleLogoutServices.Count > 0)
			{
				foreach (ProtocolEndpoint singleLogoutService in ssoDescriptor.SingleLogoutServices)
				{
					XmlQualifiedName element2 = new XmlQualifiedName("SingleLogoutService", "urn:oasis:names:tc:SAML:2.0:metadata");
					WriteProtocolEndpoint(writer, singleLogoutService, element2);
				}
			}
			if (ssoDescriptor.NameIdentifierFormats != null && ssoDescriptor.NameIdentifierFormats.Count > 0)
			{
				foreach (Uri nameIdentifierFormat in ssoDescriptor.NameIdentifierFormats)
				{
					if (!nameIdentifierFormat.IsAbsoluteUri)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID0014", "NameIDFormat")));
					}
					writer.WriteStartElement("NameIDFormat", "urn:oasis:names:tc:SAML:2.0:metadata");
					writer.WriteString(nameIdentifierFormat.AbsoluteUri);
					writer.WriteEndElement();
				}
			}
			WriteCustomElements(writer, ssoDescriptor);
		}

		protected virtual void WriteWebServiceDescriptorAttributes(XmlWriter writer, WebServiceDescriptor wsDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (wsDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor");
			}
			WriteRoleDescriptorAttributes(writer, wsDescriptor);
			if (!string.IsNullOrEmpty(wsDescriptor.ServiceDisplayName))
			{
				writer.WriteAttributeString("ServiceDisplayName", null, wsDescriptor.ServiceDisplayName);
			}
			if (!string.IsNullOrEmpty(wsDescriptor.ServiceDescription))
			{
				writer.WriteAttributeString("ServiceDescription", null, wsDescriptor.ServiceDescription);
			}
			WriteCustomAttributes(writer, wsDescriptor);
		}

		protected virtual void WriteWebServiceDescriptorElements(XmlWriter writer, WebServiceDescriptor wsDescriptor)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (wsDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor");
			}
			if (wsDescriptor.TargetScopes == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor.TargetScopes");
			}
			if (wsDescriptor.ClaimTypesOffered == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor.ClaimTypesOffered");
			}
			if (wsDescriptor.TokenTypesOffered == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wsDescriptor.TokenTypesOffered");
			}
			WriteRoleDescriptorElements(writer, wsDescriptor);
			if (wsDescriptor.TokenTypesOffered.Count > 0)
			{
				writer.WriteStartElement("TokenTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706");
				foreach (Uri item in wsDescriptor.TokenTypesOffered)
				{
					writer.WriteStartElement("TokenType", "http://docs.oasis-open.org/wsfed/federation/200706");
					if (!item.IsAbsoluteUri)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new MetadataSerializationException(SR.GetString("ID3203", "ClaimType")));
					}
					writer.WriteAttributeString("Uri", item.AbsoluteUri);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			if (wsDescriptor.ClaimTypesOffered.Count > 0)
			{
				writer.WriteStartElement("ClaimTypesOffered", "http://docs.oasis-open.org/wsfed/federation/200706");
				foreach (DisplayClaim item2 in wsDescriptor.ClaimTypesOffered)
				{
					WriteDisplayClaim(writer, item2);
				}
				writer.WriteEndElement();
			}
			if (wsDescriptor.ClaimTypesRequested.Count > 0)
			{
				writer.WriteStartElement("ClaimTypesRequested", "http://docs.oasis-open.org/wsfed/federation/200706");
				foreach (DisplayClaim item3 in wsDescriptor.ClaimTypesRequested)
				{
					WriteDisplayClaim(writer, item3);
				}
				writer.WriteEndElement();
			}
			if (wsDescriptor.TargetScopes.Count > 0)
			{
				writer.WriteStartElement("TargetScopes", "http://docs.oasis-open.org/wsfed/federation/200706");
				foreach (EndpointAddress targetScope in wsDescriptor.TargetScopes)
				{
					targetScope.WriteTo(AddressingVersion.WSAddressing10, writer);
				}
				writer.WriteEndElement();
			}
			WriteCustomElements(writer, wsDescriptor);
		}

		protected virtual Saml2Attribute ReadAttribute(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AttributeType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = reader.GetAttribute("Name");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "Name", "Attribute"));
				}
				Saml2Attribute saml2Attribute = new Saml2Attribute(attribute);
				attribute = reader.GetAttribute("NameFormat");
				if (!string.IsNullOrEmpty(attribute))
				{
					if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0011", "Namespace", "Action"));
					}
					saml2Attribute.NameFormat = new Uri(attribute);
				}
				saml2Attribute.FriendlyName = reader.GetAttribute("FriendlyName");
				reader.Read();
				if (!isEmptyElement)
				{
					while (reader.IsStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						bool isEmptyElement2 = reader.IsEmptyElement;
						bool flag = XmlUtil.IsNil(reader);
						XmlUtil.ValidateXsiType(reader, "string", "http://www.w3.org/2001/XMLSchema");
						if (flag)
						{
							reader.Read();
							if (!isEmptyElement2)
							{
								reader.ReadEndElement();
							}
							saml2Attribute.Values.Add(null);
						}
						else if (isEmptyElement2)
						{
							reader.Read();
							saml2Attribute.Values.Add("");
						}
						else
						{
							saml2Attribute.Values.Add(reader.ReadElementString());
						}
					}
					reader.ReadEndElement();
				}
				return saml2Attribute;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAttribute(XmlWriter writer, Saml2Attribute data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
			writer.WriteAttributeString("Name", data.Name);
			if (null != data.NameFormat)
			{
				writer.WriteAttributeString("NameFormat", data.NameFormat.AbsoluteUri);
			}
			if (data.FriendlyName != null)
			{
				writer.WriteAttributeString("FriendlyName", data.FriendlyName);
			}
			foreach (string value in data.Values)
			{
				writer.WriteStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
				if (value == null)
				{
					writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", XmlConvert.ToString(value: true));
				}
				else if (value.Length > 0)
				{
					writer.WriteString(value);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		private static Exception TryWrapReadException(XmlReader reader, Exception inner)
		{
			if (inner is FormatException || inner is ArgumentException || inner is InvalidOperationException || inner is OverflowException)
			{
				return DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4125"), inner);
			}
			return null;
		}
	}
}
