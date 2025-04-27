using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.WindowsTokenService;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public class WindowsClaimsIdentity : WindowsIdentity, IClaimsIdentity, IIdentity, ISerializable
	{
		private IClaimsIdentity _actor;

		private bool _claimsInitialized;

		private bool _nameInitialized;

		private ClaimCollection _claims;

		private string _label;

		private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";

		private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

		private SecurityToken _bootstrapToken;

		private string _bootstrapTokenString;

		private string _issuerName;

		public new IClaimsIdentity Actor
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

		public new ClaimCollection Claims
		{
			get
			{
				if (!_claimsInitialized)
				{
					InitializeClaims();
				}
				return _claims;
			}
		}

		public new string Label
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

		public new string NameClaimType
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

		public override string Name
		{
			get
			{
				string text = null;
				if (!_nameInitialized)
				{
					InitializeName();
				}
				foreach (Claim item in _claims.FindAll(NameClaimPredicate))
				{
					if (!string.IsNullOrEmpty(item.Value))
					{
						text = item.Value;
						break;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = base.Name;
				}
				return text;
			}
		}

		public new string RoleClaimType
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

		public WindowsClaimsIdentity(IntPtr userToken, string authenticationType)
			: this(userToken, authenticationType, "LOCAL AUTHORITY")
		{
		}

		public WindowsClaimsIdentity(IntPtr userToken)
			: this(userToken, "Windows")
		{
		}

		public WindowsClaimsIdentity(IntPtr userToken, string authenticationType, string issuerName)
			: base(GetValidToken(userToken), authenticationType)
		{
			_issuerName = issuerName;
		}

		internal WindowsClaimsIdentity(WindowsIdentity identity, string authenticationType)
			: this(GetValidWindowsIdentity(identity).Token, authenticationType)
		{
		}

		internal WindowsClaimsIdentity(WindowsIdentity identity, string authenticationType, string issuerName)
			: this(GetValidWindowsIdentity(identity).Token, authenticationType, issuerName)
		{
		}

		internal WindowsClaimsIdentity(WindowsIdentity identity)
			: this(GetValidWindowsIdentity(identity).Token, identity.AuthenticationType)
		{
		}

		private static WindowsIdentity GetValidWindowsIdentity(WindowsIdentity identity)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			GetValidToken(identity.Token);
			return identity;
		}

		private static IntPtr GetValidToken(IntPtr token)
		{
			if (token == IntPtr.Zero)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID4281"));
			}
			return token;
		}

		public new static WindowsClaimsIdentity GetCurrent()
		{
			using WindowsIdentity identity = WindowsIdentity.GetCurrent();
			return new WindowsClaimsIdentity(identity);
		}

		internal static WindowsClaimsIdentity GetCurrent(string authenticationType)
		{
			using WindowsIdentity identity = WindowsIdentity.GetCurrent();
			return new WindowsClaimsIdentity(identity, authenticationType);
		}

		protected WindowsClaimsIdentity(SerializationInfo info, StreamingContext context)
			: base(info, context)
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
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SerializationException(SR.GetString("ID4282", "WindowsClaimsIdentity"), ex));
			}
		}

		public static WindowsClaimsIdentity CreateFromUpn(string upn, string authenticationType, bool useWindowsTokenService)
		{
			return CreateFromUpn(upn, authenticationType, useWindowsTokenService, "LOCAL AUTHORITY");
		}

		public static WindowsClaimsIdentity CreateFromUpn(string upn, string authenticationType, bool useWindowsTokenService, string issuerName)
		{
			if (useWindowsTokenService)
			{
				using (WindowsIdentity windowsIdentity = S4UClient.UpnLogon(upn))
				{
					return new WindowsClaimsIdentity(windowsIdentity.Token, authenticationType, issuerName);
				}
			}
			using WindowsIdentity windowsIdentity2 = new WindowsIdentity(upn);
			return new WindowsClaimsIdentity(windowsIdentity2.Token, authenticationType, issuerName);
		}

		public static WindowsClaimsIdentity CreateFromCertificate(X509Certificate2 certificate, bool useWindowsTokenService)
		{
			return CreateFromCertificate(certificate, useWindowsTokenService, "LOCAL AUTHORITY");
		}

		public static WindowsClaimsIdentity CreateFromCertificate(X509Certificate2 certificate, bool useWindowsTokenService, string issuerName)
		{
			if (useWindowsTokenService)
			{
				using (WindowsIdentity windowsIdentity = S4UClient.CertificateLogon(certificate))
				{
					return new WindowsClaimsIdentity(windowsIdentity.Token, "X509", issuerName);
				}
			}
			using WindowsIdentity windowsIdentity2 = CertificateLogon(certificate);
			return new WindowsClaimsIdentity(windowsIdentity2.Token, "X509", issuerName);
		}

		public static WindowsClaimsIdentity CertificateLogon(X509Certificate2 x509Certificate)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				using (WindowsIdentity windowsIdentity = Microsoft.IdentityModel.Tokens.X509SecurityTokenHandler.KerberosCertificateLogon(x509Certificate))
				{
					return new WindowsClaimsIdentity(windowsIdentity.Token);
				}
			}
			string nameInfo = x509Certificate.GetNameInfo(X509NameType.UpnName, forIssuer: false);
			if (string.IsNullOrEmpty(nameInfo))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4067", X509Util.GetCertificateId(x509Certificate))));
			}
			using WindowsIdentity windowsIdentity2 = new WindowsIdentity(nameInfo);
			return new WindowsClaimsIdentity(windowsIdentity2.Token);
		}

		private bool NameClaimPredicate(Claim c)
		{
			return StringComparer.Ordinal.Equals(c.ClaimType, _nameClaimType);
		}

		public IClaimsIdentity Copy()
		{
			WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(this);
			if (_claims != null)
			{
				windowsClaimsIdentity._claims = _claims.CopyWithSubject(windowsClaimsIdentity);
			}
			windowsClaimsIdentity._claimsInitialized = _claimsInitialized;
			windowsClaimsIdentity._nameInitialized = _nameInitialized;
			windowsClaimsIdentity._issuerName = _issuerName;
			windowsClaimsIdentity.Label = Label;
			windowsClaimsIdentity.NameClaimType = NameClaimType;
			windowsClaimsIdentity.RoleClaimType = RoleClaimType;
			windowsClaimsIdentity.BootstrapToken = BootstrapToken;
			if (Actor != null)
			{
				if (IsCircular(Actor))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4035"));
				}
				windowsClaimsIdentity.Actor = Actor.Copy();
			}
			return windowsClaimsIdentity;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			GetObjectData(info, context);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			info.AddValue("m_userToken", Token);
			ClaimsIdentitySerializer claimsIdentitySerializer = new ClaimsIdentitySerializer(info, context);
			claimsIdentitySerializer.SerializeNameClaimType(_nameClaimType);
			claimsIdentitySerializer.SerializeRoleClaimType(_roleClaimType);
			claimsIdentitySerializer.SerializeLabel(_label);
			claimsIdentitySerializer.SerializeActor(_actor);
			List<Claim> claims = null;
			if (_claims != null)
			{
				claims = new List<Claim>(_claims.Where((Claim c) => !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid") && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid") && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid") && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid") && !StringComparer.Ordinal.Equals(c.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid")));
			}
			claimsIdentitySerializer.SerializeClaims(claims);
			claimsIdentitySerializer.SerializeBootstrapToken(_bootstrapToken);
		}

		public override string ToString()
		{
			return Name;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		protected Claim CreatePrimarySidClaim()
		{
			SafeHGlobalHandle safeHGlobalHandle = SafeHGlobalHandle.InvalidHandle;
			Claim result = null;
			try
			{
				safeHGlobalHandle = GetTokenInformation(Token, TokenInformationClass.TokenUser, out var _);
				SID_AND_ATTRIBUTES sID_AND_ATTRIBUTES = (SID_AND_ATTRIBUTES)Marshal.PtrToStructure(safeHGlobalHandle.DangerousGetHandle(), typeof(SID_AND_ATTRIBUTES));
				uint num = 16u;
				if (sID_AND_ATTRIBUTES.Attributes == 0)
				{
					return new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid", new SecurityIdentifier(sID_AND_ATTRIBUTES.Sid).Value, "http://www.w3.org/2001/XMLSchema#string", _issuerName);
				}
				if ((sID_AND_ATTRIBUTES.Attributes & num) == 16)
				{
					return new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid", new SecurityIdentifier(sID_AND_ATTRIBUTES.Sid).Value, "http://www.w3.org/2001/XMLSchema#string", _issuerName);
				}
				return result;
			}
			finally
			{
				safeHGlobalHandle.Close();
			}
		}

		protected ICollection<Claim> GetGroupSidClaims()
		{
			Collection<Claim> collection = new Collection<Claim>();
			if (Token != IntPtr.Zero)
			{
				SafeHGlobalHandle safeHGlobalHandle = SafeHGlobalHandle.InvalidHandle;
				SafeHGlobalHandle safeHGlobalHandle2 = SafeHGlobalHandle.InvalidHandle;
				try
				{
					safeHGlobalHandle2 = GetTokenInformation(Token, TokenInformationClass.TokenPrimaryGroup, out var dwLength);
					SecurityIdentifier securityIdentifier = new SecurityIdentifier(((TOKEN_PRIMARY_GROUP)Marshal.PtrToStructure(safeHGlobalHandle2.DangerousGetHandle(), typeof(TOKEN_PRIMARY_GROUP))).PrimaryGroup);
					bool flag = false;
					safeHGlobalHandle = GetTokenInformation(Token, TokenInformationClass.TokenGroups, out dwLength);
					int num = Marshal.ReadInt32(safeHGlobalHandle.DangerousGetHandle());
					IntPtr intPtr = new IntPtr((long)safeHGlobalHandle.DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(TOKEN_GROUPS), "Groups"));
					for (int i = 0; i < num; i++)
					{
						SID_AND_ATTRIBUTES sID_AND_ATTRIBUTES = (SID_AND_ATTRIBUTES)Marshal.PtrToStructure(intPtr, typeof(SID_AND_ATTRIBUTES));
						uint num2 = 3221225492u;
						SecurityIdentifier securityIdentifier2 = new SecurityIdentifier(sID_AND_ATTRIBUTES.Sid);
						if ((sID_AND_ATTRIBUTES.Attributes & num2) == 4)
						{
							if (!flag && StringComparer.Ordinal.Equals(securityIdentifier2.Value, securityIdentifier.Value))
							{
								collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", _issuerName));
								flag = true;
							}
							collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", _issuerName));
						}
						else if ((sID_AND_ATTRIBUTES.Attributes & num2) == 16)
						{
							if (!flag && StringComparer.Ordinal.Equals(securityIdentifier2.Value, securityIdentifier.Value))
							{
								collection.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", _issuerName));
								flag = true;
							}
							collection.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid", securityIdentifier2.Value, "http://www.w3.org/2001/XMLSchema#string", _issuerName));
						}
						intPtr = new IntPtr((long)intPtr + SID_AND_ATTRIBUTES.SizeOf);
					}
					return collection;
				}
				finally
				{
					safeHGlobalHandle.Close();
					safeHGlobalHandle2.Close();
				}
			}
			return collection;
		}

		private void InitializeName()
		{
			if (_nameInitialized)
			{
				return;
			}
			_nameInitialized = true;
			if (!IsAuthenticated)
			{
				return;
			}
			string name = base.Name;
			if (!string.IsNullOrEmpty(name))
			{
				if (_claims == null)
				{
					_claims = new ClaimCollection(this);
				}
				_claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name, "http://www.w3.org/2001/XMLSchema#string", _issuerName));
			}
		}

		private void InitializeClaims()
		{
			_claimsInitialized = true;
			if (_claims == null)
			{
				_claims = new ClaimCollection(this);
			}
			if (IsAuthenticated)
			{
				InitializeName();
				Claim claim = CreatePrimarySidClaim();
				if (claim != null)
				{
					_claims.Add(claim);
				}
				_claims.AddRange(GetGroupSidClaims());
			}
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
		}

		private static SafeHGlobalHandle GetTokenInformation(IntPtr tokenHandle, TokenInformationClass tokenInformationClass, out uint dwLength)
		{
			SafeHGlobalHandle invalidHandle = SafeHGlobalHandle.InvalidHandle;
			dwLength = (uint)Marshal.SizeOf(typeof(uint));
			NativeMethods.GetTokenInformation(tokenHandle, (uint)tokenInformationClass, invalidHandle, 0u, out dwLength);
			int lastWin32Error = Marshal.GetLastWin32Error();
			int num = lastWin32Error;
			if (num == 24 || num == 122)
			{
				invalidHandle = SafeHGlobalHandle.AllocHGlobal(dwLength);
				bool tokenInformation = NativeMethods.GetTokenInformation(tokenHandle, (uint)tokenInformationClass, invalidHandle, dwLength, out dwLength);
				lastWin32Error = Marshal.GetLastWin32Error();
				if (!tokenInformation)
				{
					invalidHandle.Close();
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
				}
				return invalidHandle;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
		}
	}
}
