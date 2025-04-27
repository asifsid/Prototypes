using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Security;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public class ClaimsIdentity : IClaimsIdentity, IIdentity, ISerializable
	{
		public const string DefaultNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

		public const string DefaultIssuer = "LOCAL AUTHORITY";

		private string _authenticationType;

		private ClaimCollection _claims;

		private IClaimsIdentity _actor;

		private string _label;

		private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

		private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

		private SecurityToken _bootstrapToken;

		private string _bootstrapTokenString;

		public string AuthenticationType
		{
			get
			{
				if (_authenticationType != null)
				{
					return _authenticationType;
				}
				return string.Empty;
			}
		}

		public bool IsAuthenticated => _claims.Count > 0;

		public string Name
		{
			get
			{
				string result = null;
				using IEnumerator<Claim> enumerator = _claims.FindAll(NameClaimPredicate).GetEnumerator();
				if (enumerator.MoveNext())
				{
					Claim current = enumerator.Current;
					if (string.IsNullOrEmpty(current.Value))
					{
						return null;
					}
					return current.Value;
				}
				return result;
			}
		}

		public IClaimsIdentity Actor
		{
			get
			{
				return _actor;
			}
			set
			{
				if (value != null && IsCircular(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4035"));
				}
				_actor = value;
			}
		}

		public ClaimCollection Claims => _claims;

		public string Label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
			}
		}

		public string NameClaimType
		{
			get
			{
				return _nameClaimType;
			}
			set
			{
				_nameClaimType = value;
			}
		}

		public string RoleClaimType
		{
			get
			{
				return _roleClaimType;
			}
			set
			{
				_roleClaimType = value;
			}
		}

		public SecurityToken BootstrapToken
		{
			get
			{
				if (_bootstrapToken == null && !string.IsNullOrEmpty(_bootstrapTokenString))
				{
					_bootstrapToken = ClaimsIdentitySerializer.DeserializeBootstrapTokenFromString(_bootstrapTokenString);
				}
				return _bootstrapToken;
			}
			set
			{
				_bootstrapToken = value;
			}
		}

		public static IClaimsIdentity AnonymousIdentity => new ClaimsIdentity();

		public ClaimsIdentity()
			: this((IEnumerable<Claim>)null, (string)null, (SecurityToken)null)
		{
		}

		public ClaimsIdentity(IIdentity identity)
			: this((IEnumerable<Claim>)null, (string)null, (SecurityToken)null)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			_authenticationType = identity.AuthenticationType;
			IClaimsIdentity claimsIdentity = identity as IClaimsIdentity;
			if (claimsIdentity != null)
			{
				if (claimsIdentity.Claims != null)
				{
					_claims = claimsIdentity.Claims.CopyWithSubject(this);
				}
				_label = claimsIdentity.Label;
				_nameClaimType = claimsIdentity.NameClaimType;
				_roleClaimType = claimsIdentity.RoleClaimType;
				_bootstrapToken = claimsIdentity.BootstrapToken;
				if (claimsIdentity.Actor != null)
				{
					if (IsCircular(claimsIdentity.Actor))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4035"));
					}
					_actor = claimsIdentity.Actor.Copy();
				}
			}
			else if (identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name))
			{
				_claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", identity.Name));
			}
		}

		public ClaimsIdentity(IEnumerable<Claim> claims)
			: this(claims, null, null)
		{
		}

		public ClaimsIdentity(IEnumerable<Claim> claims, SecurityToken bootstrapToken)
			: this(claims, null, bootstrapToken)
		{
		}

		public ClaimsIdentity(string authenticationType)
			: this((IEnumerable<Claim>)null, authenticationType, (SecurityToken)null)
		{
		}

		public ClaimsIdentity(X509Certificate2 certificate, string issuer)
			: this(GetClaimsFromCertificate(certificate, issuer), "X509")
		{
		}

		public ClaimsIdentity(X509Certificate2 certificate, string issuer, string authenticationType)
			: this(GetClaimsFromCertificate(certificate, issuer), authenticationType)
		{
		}

		public ClaimsIdentity(IEnumerable<Claim> claims, string authenticationType)
			: this(claims, authenticationType, null)
		{
		}

		public ClaimsIdentity(IEnumerable<Claim> claims, string authenticationType, SecurityToken bootstrapToken)
		{
			_claims = new ClaimCollection(this);
			_authenticationType = authenticationType;
			if (claims != null)
			{
				_claims.AddRange(claims);
			}
			_bootstrapToken = bootstrapToken;
		}

		public ClaimsIdentity(string authenticationType, string nameClaimType, string roleClaimType)
			: this((IEnumerable<Claim>)null, authenticationType, nameClaimType, roleClaimType)
		{
		}

		public ClaimsIdentity(IEnumerable<Claim> claims, string authenticationType, string nameClaimType, string roleClaimType)
			: this(claims, authenticationType, nameClaimType, roleClaimType, null)
		{
		}

		public ClaimsIdentity(IEnumerable<Claim> claims, string authenticationType, string nameClaimType, string roleClaimType, SecurityToken bootstrapToken)
			: this(claims, authenticationType, bootstrapToken)
		{
			_nameClaimType = nameClaimType;
			_roleClaimType = roleClaimType;
		}

		protected ClaimsIdentity(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			try
			{
				Deserialize(info, context);
			}
			catch (Exception ex)
			{
				if (DiagnosticUtil.IsFatal(ex))
				{
					throw;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SerializationException(SR.GetString("ID4282", "ClaimsIdentity"), ex));
			}
		}

		internal ClaimsIdentity(ClaimSet claimSet)
			: this(claimSet, null)
		{
		}

		internal ClaimsIdentity(ClaimSet claimSet, string authenticationType)
		{
			if (claimSet == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimSet");
			}
			_claims = new ClaimCollection(this);
			_authenticationType = authenticationType;
			string issuer = null;
			if (claimSet.Issuer == null)
			{
				issuer = "LOCAL AUTHORITY";
			}
			else
			{
				foreach (System.IdentityModel.Claims.Claim item in claimSet.Issuer.FindClaims("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", Rights.Identity))
				{
					if (item != null && item.Resource is string)
					{
						issuer = item.Resource as string;
						break;
					}
				}
			}
			for (int i = 0; i < claimSet.Count; i++)
			{
				if (string.Equals(claimSet[i].Right, Rights.PossessProperty, StringComparison.Ordinal))
				{
					_claims.Add(new Claim(claimSet[i], issuer));
				}
			}
		}

		internal ClaimsIdentity(ClaimSet claimSet, string authenticationType, string nameClaimType, string roleClaimType)
			: this(claimSet, authenticationType)
		{
			_nameClaimType = nameClaimType;
			_roleClaimType = roleClaimType;
		}

		public override string ToString()
		{
			return Name;
		}

		public IClaimsIdentity Copy()
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(AuthenticationType);
			if (Claims != null)
			{
				claimsIdentity._claims = Claims.CopyWithSubject(claimsIdentity);
			}
			claimsIdentity.Label = Label;
			claimsIdentity.NameClaimType = NameClaimType;
			claimsIdentity.RoleClaimType = RoleClaimType;
			claimsIdentity.BootstrapToken = BootstrapToken;
			if (Actor != null)
			{
				if (IsCircular(Actor))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4035"));
				}
				claimsIdentity.Actor = Actor.Copy();
			}
			return claimsIdentity;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			GetObjectData(info, context);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			ClaimsIdentitySerializer claimsIdentitySerializer = new ClaimsIdentitySerializer(info, context);
			claimsIdentitySerializer.SerializeNameClaimType(_nameClaimType);
			claimsIdentitySerializer.SerializeRoleClaimType(_roleClaimType);
			claimsIdentitySerializer.SerializeLabel(_label);
			claimsIdentitySerializer.SerializeActor(_actor);
			claimsIdentitySerializer.SerializeClaims(_claims);
			claimsIdentitySerializer.SerializeBootstrapToken(_bootstrapToken);
			claimsIdentitySerializer.SerializeAuthenticationType(_authenticationType);
		}

		public static IEnumerable<Claim> GetClaimsFromCertificate(X509Certificate2 certificate, string issuer)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			ICollection<Claim> collection = new Collection<Claim>();
			string value = Convert.ToBase64String(certificate.GetCertHash());
			collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint", value, "http://www.w3.org/2001/XMLSchema#base64Binary", issuer));
			string name = certificate.SubjectName.Name;
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			name = certificate.GetNameInfo(X509NameType.DnsName, forIssuer: false);
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			name = certificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false);
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			name = certificate.GetNameInfo(X509NameType.EmailName, forIssuer: false);
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			name = certificate.GetNameInfo(X509NameType.UpnName, forIssuer: false);
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			name = certificate.GetNameInfo(X509NameType.UrlName, forIssuer: false);
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			RSA rSA = certificate.PublicKey.Key as RSA;
			if (rSA != null)
			{
				collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa", rSA.ToXmlString(includePrivateParameters: false), "http://www.w3.org/2000/09/xmldsig#RSAKeyValue", issuer));
			}
			DSA dSA = certificate.PublicKey.Key as DSA;
			if (dSA != null)
			{
				collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/dsa", dSA.ToXmlString(includePrivateParameters: false), "http://www.w3.org/2000/09/xmldsig#DSAKeyValue", issuer));
			}
			name = certificate.SerialNumber;
			if (!string.IsNullOrEmpty(name))
			{
				collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber", name, "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			return collection;
		}

		internal static IClaimsIdentity CreateFromIdentity(IIdentity identity)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			IClaimsIdentity claimsIdentity = identity as IClaimsIdentity;
			if (claimsIdentity != null)
			{
				return claimsIdentity;
			}
			WindowsIdentity windowsIdentity = identity as WindowsIdentity;
			if (windowsIdentity != null)
			{
				return new WindowsClaimsIdentity(windowsIdentity);
			}
			FormsIdentity formsIdentity = identity as FormsIdentity;
			if (formsIdentity != null)
			{
				ClaimsIdentity claimsIdentity2 = new ClaimsIdentity(formsIdentity);
				FormsAuthenticationTicket ticket = formsIdentity.Ticket;
				if (ticket != null)
				{
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(ticket.IssueDate.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/cookiepath", ticket.CookiePath));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration", XmlConvert.ToString(ticket.Expiration.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/expired", XmlConvert.ToString(ticket.Expired), "http://www.w3.org/2001/XMLSchema#boolean"));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/ispersistent", XmlConvert.ToString(ticket.IsPersistent), "http://www.w3.org/2001/XMLSchema#boolean"));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata", ticket.UserData));
					claimsIdentity2.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/version", XmlConvert.ToString(ticket.Version), "http://www.w3.org/2001/XMLSchema#integer"));
				}
				return claimsIdentity2;
			}
			return new ClaimsIdentity(identity);
		}

		private bool NameClaimPredicate(Claim c)
		{
			return StringComparer.Ordinal.Equals(c.ClaimType, _nameClaimType);
		}

		private bool IsCircular(IClaimsIdentity subject)
		{
			if (object.ReferenceEquals(this, subject))
			{
				return true;
			}
			IClaimsIdentity claimsIdentity = subject;
			while (claimsIdentity.Actor != null)
			{
				if (object.ReferenceEquals(this, claimsIdentity.Actor))
				{
					return true;
				}
				claimsIdentity = claimsIdentity.Actor;
			}
			return false;
		}

		private void Deserialize(SerializationInfo info, StreamingContext context)
		{
			ClaimsIdentitySerializer claimsIdentitySerializer = new ClaimsIdentitySerializer(info, context);
			_nameClaimType = claimsIdentitySerializer.DeserializeNameClaimType();
			_roleClaimType = claimsIdentitySerializer.DeserializeRoleClaimType();
			_label = claimsIdentitySerializer.DeserializeLabel();
			_actor = claimsIdentitySerializer.DeserializeActor();
			if (_claims == null)
			{
				_claims = new ClaimCollection(this);
			}
			claimsIdentitySerializer.DeserializeClaims(_claims);
			_bootstrapToken = null;
			_bootstrapTokenString = claimsIdentitySerializer.GetSerializedBootstrapTokenString();
			_authenticationType = claimsIdentitySerializer.DeserializeAuthenticationType();
		}
	}
}
