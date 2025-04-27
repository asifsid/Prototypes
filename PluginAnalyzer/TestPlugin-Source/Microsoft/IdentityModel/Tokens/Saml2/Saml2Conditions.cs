using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Conditions
	{
		private Collection<Saml2AudienceRestriction> _audienceRestrictions = new Collection<Saml2AudienceRestriction>();

		private DateTime? _notBefore;

		private DateTime? _notOnOrAfter;

		private bool _oneTimeUse;

		private Saml2ProxyRestriction _proxyRestriction;

		public Collection<Saml2AudienceRestriction> AudienceRestrictions => _audienceRestrictions;

		public DateTime? NotBefore
		{
			get
			{
				return _notBefore;
			}
			set
			{
				value = DateTimeUtil.ToUniversalTime(value);
				if (value.HasValue && _notOnOrAfter.HasValue && value.Value >= _notOnOrAfter.Value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4116"));
				}
				_notBefore = value;
			}
		}

		public DateTime? NotOnOrAfter
		{
			get
			{
				return _notOnOrAfter;
			}
			set
			{
				value = DateTimeUtil.ToUniversalTime(value);
				if (value.HasValue && _notBefore.HasValue && value.Value <= _notBefore.Value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4116"));
				}
				_notOnOrAfter = value;
			}
		}

		public bool OneTimeUse
		{
			get
			{
				return _oneTimeUse;
			}
			set
			{
				_oneTimeUse = value;
			}
		}

		public Saml2ProxyRestriction ProxyRestriction
		{
			get
			{
				return _proxyRestriction;
			}
			set
			{
				_proxyRestriction = value;
			}
		}
	}
}
