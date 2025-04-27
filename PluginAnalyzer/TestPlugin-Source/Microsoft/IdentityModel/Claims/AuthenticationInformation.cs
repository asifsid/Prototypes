using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public class AuthenticationInformation
	{
		private string _address;

		private Collection<AuthenticationContext> _authContexts;

		private string _dnsName;

		private DateTime? _notOnOrAfter;

		private string _session;

		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = value;
			}
		}

		public Collection<AuthenticationContext> AuthorizationContexts => _authContexts;

		public string DnsName
		{
			get
			{
				return _dnsName;
			}
			set
			{
				_dnsName = value;
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
				_notOnOrAfter = value;
			}
		}

		public string Session
		{
			get
			{
				return _session;
			}
			set
			{
				_session = value;
			}
		}

		public AuthenticationInformation()
		{
			_authContexts = new Collection<AuthenticationContext>();
		}
	}
}
