using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
	internal class ClaimsSerializer
	{
		private SessionDictionary _sd;

		public ClaimsSerializer(SessionDictionary sessionDictionary)
		{
			if (sessionDictionary == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionDictionary");
			}
			_sd = sessionDictionary;
		}

		public void ReadClaims(XmlDictionaryReader dictionaryReader, ClaimCollection claims)
		{
			if (dictionaryReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryReader");
			}
			if (claims == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claims");
			}
			if (!dictionaryReader.IsStartElement(_sd.ClaimCollection, _sd.EmptyString))
			{
				return;
			}
			dictionaryReader.ReadStartElement();
			while (dictionaryReader.IsStartElement(_sd.Claim, _sd.EmptyString))
			{
				Claim claim = new Claim(dictionaryReader.GetAttribute(_sd.Type, _sd.EmptyString), dictionaryReader.GetAttribute(_sd.Value, _sd.EmptyString), dictionaryReader.GetAttribute(_sd.ValueType, _sd.EmptyString), dictionaryReader.GetAttribute(_sd.Issuer, _sd.EmptyString), dictionaryReader.GetAttribute(_sd.OriginalIssuer, _sd.EmptyString));
				dictionaryReader.ReadFullStartElement();
				if (dictionaryReader.IsStartElement(_sd.ClaimProperties, _sd.EmptyString))
				{
					ReadClaimProperties(dictionaryReader, claim.Properties);
				}
				dictionaryReader.ReadEndElement();
				claims.Add(claim);
			}
			dictionaryReader.ReadEndElement();
		}

		private void ReadClaimProperties(XmlDictionaryReader dictionaryReader, IDictionary<string, string> properties)
		{
			if (properties == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("properties");
			}
			dictionaryReader.ReadStartElement();
			while (dictionaryReader.IsStartElement(_sd.ClaimProperty, _sd.EmptyString))
			{
				string attribute = dictionaryReader.GetAttribute(_sd.ClaimPropertyName, _sd.EmptyString);
				string attribute2 = dictionaryReader.GetAttribute(_sd.ClaimPropertyValue, _sd.EmptyString);
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4249")));
				}
				if (string.IsNullOrEmpty(attribute2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4250")));
				}
				properties.Add(new KeyValuePair<string, string>(attribute, attribute2));
				dictionaryReader.ReadFullStartElement();
				dictionaryReader.ReadEndElement();
			}
			dictionaryReader.ReadEndElement();
		}

		public void WriteClaims(XmlDictionaryWriter dictionaryWriter, IEnumerable<Claim> claims)
		{
			if (dictionaryWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dictionaryWriter");
			}
			if (claims == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claims");
			}
			dictionaryWriter.WriteStartElement(_sd.ClaimCollection, _sd.EmptyString);
			foreach (Claim claim in claims)
			{
				if (claim != null)
				{
					dictionaryWriter.WriteStartElement(_sd.Claim, _sd.EmptyString);
					if (!string.IsNullOrEmpty(claim.Issuer))
					{
						dictionaryWriter.WriteAttributeString(_sd.Issuer, _sd.EmptyString, claim.Issuer);
					}
					if (!string.IsNullOrEmpty(claim.OriginalIssuer))
					{
						dictionaryWriter.WriteAttributeString(_sd.OriginalIssuer, _sd.EmptyString, claim.OriginalIssuer);
					}
					dictionaryWriter.WriteAttributeString(_sd.Type, _sd.EmptyString, claim.ClaimType);
					dictionaryWriter.WriteAttributeString(_sd.Value, _sd.EmptyString, claim.Value);
					dictionaryWriter.WriteAttributeString(_sd.ValueType, _sd.EmptyString, claim.ValueType);
					if (claim.Properties != null && claim.Properties.Count > 0)
					{
						WriteClaimProperties(dictionaryWriter, claim.Properties);
					}
					dictionaryWriter.WriteEndElement();
				}
			}
			dictionaryWriter.WriteEndElement();
		}

		private void WriteClaimProperties(XmlDictionaryWriter dictionaryWriter, IDictionary<string, string> properties)
		{
			if (properties == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("properties");
			}
			if (properties.Count <= 0)
			{
				return;
			}
			dictionaryWriter.WriteStartElement(_sd.ClaimProperties, _sd.EmptyString);
			foreach (KeyValuePair<string, string> property in properties)
			{
				dictionaryWriter.WriteStartElement(_sd.ClaimProperty, _sd.EmptyString);
				dictionaryWriter.WriteAttributeString(_sd.ClaimPropertyName, _sd.EmptyString, property.Key);
				dictionaryWriter.WriteAttributeString(_sd.ClaimPropertyValue, _sd.EmptyString, property.Value);
				dictionaryWriter.WriteEndElement();
			}
			dictionaryWriter.WriteEndElement();
		}
	}
}
