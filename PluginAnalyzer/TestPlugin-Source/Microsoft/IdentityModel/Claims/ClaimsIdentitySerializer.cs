using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

namespace Microsoft.IdentityModel.Claims
{
	internal class ClaimsIdentitySerializer
	{
		public const string ActorKey = "_actor";

		public const string AuthenticationTypeKey = "_authenticationType";

		public const string BootstrapTokenKey = "_bootstrapToken";

		public const string ClaimsKey = "_claims";

		public const string LabelKey = "_label";

		public const string NameClaimTypeKey = "_nameClaimType";

		public const string RoleClaimTypeKey = "_roleClaimType";

		private SerializationInfo _info;

		private StreamingContext _context;

		public static SecurityToken DeserializeBootstrapTokenFromString(string bootstrapTokenString)
		{
			if (!string.IsNullOrEmpty(bootstrapTokenString))
			{
				byte[] array = Convert.FromBase64String(bootstrapTokenString);
				using XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateBinaryReader(array, 0, array.Length, SessionDictionary.Instance, XmlDictionaryReaderQuotas.Max);
				Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = ServiceConfiguration.GetCurrent().SecurityTokenHandlers;
				xmlDictionaryReader.MoveToContent();
				string s = xmlDictionaryReader.ReadOuterXml();
				using StringReader input = new StringReader(s);
				XmlTextReader xmlTextReader = new XmlTextReader(input);
				xmlTextReader.Normalization = false;
				using XmlDictionaryReader xmlDictionaryReader2 = new IdentityModelWrappedXmlDictionaryReader(xmlTextReader, XmlDictionaryReaderQuotas.Max);
				xmlDictionaryReader2.MoveToContent();
				if (securityTokenHandlers.CanReadToken(xmlDictionaryReader2))
				{
					return securityTokenHandlers.ReadToken(xmlDictionaryReader2);
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8001"));
				}
			}
			return null;
		}

		public ClaimsIdentitySerializer(SerializationInfo info, StreamingContext context)
		{
			_info = info;
			_context = context;
		}

		public IClaimsIdentity DeserializeActor()
		{
			string @string = _info.GetString("_actor");
			if (@string == null)
			{
				return null;
			}
			BinaryFormatter binaryFormatter = new BinaryFormatter(null, _context);
			using MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(@string));
			return (IClaimsIdentity)binaryFormatter.Deserialize(serializationStream);
		}

		public void SerializeActor(IClaimsIdentity actor)
		{
			_info.AddValue("_actor", SerializeActorToString(actor));
		}

		public string DeserializeAuthenticationType()
		{
			return _info.GetString("_authenticationType");
		}

		public void SerializeAuthenticationType(string authenticationType)
		{
			_info.AddValue("_authenticationType", authenticationType);
		}

		public string GetSerializedBootstrapTokenString()
		{
			return _info.GetString("_bootstrapToken");
		}

		public void SerializeBootstrapToken(SecurityToken bootstrapToken)
		{
			_info.AddValue("_bootstrapToken", SerializeBootstrapTokenToString(bootstrapToken));
		}

		public void DeserializeClaims(ClaimCollection claims)
		{
			string @string = _info.GetString("_claims");
			DeserializeClaimsFromString(claims, @string);
		}

		public void SerializeClaims(IEnumerable<Claim> claims)
		{
			IList<Claim> list = claims as IList<Claim>;
			if (claims == null || (list != null && list.Count == 0))
			{
				_info.AddValue("_claims", string.Empty);
			}
			else
			{
				_info.AddValue("_claims", SerializeClaimsToString(claims));
			}
		}

		public string DeserializeLabel()
		{
			return _info.GetString("_label");
		}

		public void SerializeLabel(string label)
		{
			_info.AddValue("_label", label);
		}

		public string DeserializeNameClaimType()
		{
			return _info.GetString("_nameClaimType");
		}

		public void SerializeNameClaimType(string nameClaimType)
		{
			_info.AddValue("_nameClaimType", nameClaimType);
		}

		public string DeserializeRoleClaimType()
		{
			return _info.GetString("_roleClaimType");
		}

		public void SerializeRoleClaimType(string roleClaimType)
		{
			_info.AddValue("_roleClaimType", roleClaimType);
		}

		private void DeserializeClaimsFromString(ClaimCollection claims, string claimsString)
		{
			if (!string.IsNullOrEmpty(claimsString))
			{
				byte[] array = Convert.FromBase64String(claimsString);
				SessionDictionary instance = SessionDictionary.Instance;
				using XmlDictionaryReader dictionaryReader = XmlDictionaryReader.CreateBinaryReader(array, 0, array.Length, instance, XmlDictionaryReaderQuotas.Max);
				ClaimsSerializer claimsSerializer = new ClaimsSerializer(instance);
				claimsSerializer.ReadClaims(dictionaryReader, claims);
			}
		}

		private string SerializeActorToString(IClaimsIdentity actor)
		{
			if (actor != null)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter(null, _context);
				using MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, actor);
				return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			}
			return null;
		}

		private string SerializeBootstrapTokenToString(SecurityToken bootstrapToken)
		{
			if (bootstrapToken == null)
			{
				return string.Empty;
			}
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = ServiceConfiguration.GetCurrent().SecurityTokenHandlers;
			if (securityTokenHandlers.CanWriteToken(bootstrapToken))
			{
				MemoryStream memoryStream = new MemoryStream();
				using XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(memoryStream);
				securityTokenHandlers.WriteToken(xmlDictionaryWriter, bootstrapToken);
				xmlDictionaryWriter.Flush();
				return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8000", bootstrapToken.GetType().ToString()));
			}
			return string.Empty;
		}

		private string SerializeClaimsToString(IEnumerable<Claim> claims)
		{
			MemoryStream memoryStream = new MemoryStream();
			using XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(memoryStream);
			ClaimsSerializer claimsSerializer = new ClaimsSerializer(SessionDictionary.Instance);
			claimsSerializer.WriteClaims(xmlDictionaryWriter, claims);
			xmlDictionaryWriter.Flush();
			return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
		}
	}
}
