using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public class Claim
	{
		private string _issuer;

		private string _originalIssuer;

		private Dictionary<string, string> _properties;

		private IClaimsIdentity _subject;

		private string _type;

		private string _value;

		private string _valueType;

		public virtual string ClaimType => _type;

		public virtual string Issuer => _issuer;

		public virtual string OriginalIssuer => _originalIssuer;

		public virtual IDictionary<string, string> Properties
		{
			get
			{
				if (_properties == null)
				{
					_properties = new Dictionary<string, string>();
				}
				return _properties;
			}
		}

		public virtual IClaimsIdentity Subject => _subject;

		public virtual string Value => _value;

		public virtual string ValueType => _valueType;

		public Claim(string claimType, string value)
			: this(claimType, value, "http://www.w3.org/2001/XMLSchema#string", "LOCAL AUTHORITY")
		{
		}

		public Claim(string claimType, string value, string valueType)
			: this(claimType, value, valueType, "LOCAL AUTHORITY")
		{
		}

		public Claim(string claimType, string value, string valueType, string issuer)
			: this(claimType, value, valueType, issuer, issuer)
		{
		}

		public Claim(string claimType, string value, string valueType, string issuer, string originalIssuer)
		{
			if (claimType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("type");
			}
			if (value == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			_originalIssuer = originalIssuer;
			_type = claimType;
			_value = value;
			if (string.IsNullOrEmpty(valueType))
			{
				_valueType = "http://www.w3.org/2001/XMLSchema#string";
			}
			else
			{
				_valueType = valueType;
			}
			if (string.IsNullOrEmpty(issuer))
			{
				_issuer = "LOCAL AUTHORITY";
				if (string.IsNullOrEmpty(originalIssuer))
				{
					_originalIssuer = "LOCAL AUTHORITY";
				}
			}
			else
			{
				_issuer = issuer;
			}
			_type = StringUtil.OptimizeString(_type);
			_value = StringUtil.OptimizeString(_value);
			_valueType = StringUtil.OptimizeString(_valueType);
			_issuer = StringUtil.OptimizeString(_issuer);
			_originalIssuer = StringUtil.OptimizeString(_originalIssuer);
		}

		public Claim(System.IdentityModel.Claims.Claim claim, string issuer)
		{
			if (claim == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claim");
			}
			if (claim.Resource == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4031")));
			}
			if (string.IsNullOrEmpty(issuer))
			{
				_issuer = "LOCAL AUTHORITY";
			}
			else
			{
				_issuer = issuer;
			}
			_originalIssuer = issuer;
			_valueType = "http://www.w3.org/2001/XMLSchema#string";
			if (claim.Resource is string)
			{
				AssignClaimFromStringResourceSysClaim(claim);
			}
			else
			{
				AssignClaimFromSysClaim(claim);
			}
			if (_value == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4030", claim.ClaimType, claim.Resource.GetType())));
			}
		}

		private void AssignClaimFromStringResourceSysClaim(System.IdentityModel.Claims.Claim claim)
		{
			_type = claim.ClaimType;
			_value = (string)claim.Resource;
			if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"))
			{
				if (claim.Right == Rights.Identity)
				{
					_type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
				}
				else
				{
					_type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";
				}
			}
		}

		private void AssignClaimFromSysClaim(System.IdentityModel.Claims.Claim claim)
		{
			if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid") && claim.Resource is SecurityIdentifier)
			{
				if (claim.Right == Rights.Identity)
				{
					_type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
				}
				else
				{
					_type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";
				}
				_value = ((SecurityIdentifier)claim.Resource).Value;
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") && claim.Resource is MailAddress)
			{
				_type = claim.ClaimType;
				_value = ((MailAddress)claim.Resource).Address;
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint") && claim.Resource is byte[])
			{
				_type = claim.ClaimType;
				_value = Convert.ToBase64String((byte[])claim.Resource);
				_valueType = "http://www.w3.org/2001/XMLSchema#base64Binary";
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash") && claim.Resource is byte[])
			{
				_type = claim.ClaimType;
				_value = Convert.ToBase64String((byte[])claim.Resource);
				_valueType = "http://www.w3.org/2001/XMLSchema#base64Binary";
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") && claim.Resource is SamlNameIdentifierClaimResource)
			{
				_type = claim.ClaimType;
				_value = ((SamlNameIdentifierClaimResource)claim.Resource).Name;
				if (((SamlNameIdentifierClaimResource)claim.Resource).Format != null)
				{
					if (_properties == null)
					{
						_properties = new Dictionary<string, string>();
					}
					_properties.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format", ((SamlNameIdentifierClaimResource)claim.Resource).Format);
				}
				if (((SamlNameIdentifierClaimResource)claim.Resource).NameQualifier != null)
				{
					if (_properties == null)
					{
						_properties = new Dictionary<string, string>();
					}
					_properties.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier", ((SamlNameIdentifierClaimResource)claim.Resource).NameQualifier);
				}
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname") && claim.Resource is X500DistinguishedName)
			{
				_type = claim.ClaimType;
				_value = ((X500DistinguishedName)claim.Resource).Name;
				_valueType = "urn:oasis:names:tc:xacml:1.0:data-type:x500Name";
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri") && claim.Resource is Uri)
			{
				_type = claim.ClaimType;
				_value = ((Uri)claim.Resource).ToString();
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa") && claim.Resource is RSA)
			{
				_type = claim.ClaimType;
				_value = ((RSA)claim.Resource).ToXmlString(includePrivateParameters: false);
				_valueType = "http://www.w3.org/2000/09/xmldsig#RSAKeyValue";
			}
			else if (StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid") && claim.Resource is SecurityIdentifier)
			{
				_type = claim.ClaimType;
				_value = ((SecurityIdentifier)claim.Resource).Value;
			}
		}

		public virtual Claim Copy()
		{
			Claim claim = new Claim(_type, _value, _valueType, _issuer, _originalIssuer);
			if (_properties != null)
			{
				foreach (string key in _properties.Keys)
				{
					claim.Properties[key] = _properties[key];
				}
				return claim;
			}
			return claim;
		}

		public virtual void SetSubject(IClaimsIdentity subject)
		{
			_subject = subject;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", new object[2] { _type, _value });
		}
	}
}
