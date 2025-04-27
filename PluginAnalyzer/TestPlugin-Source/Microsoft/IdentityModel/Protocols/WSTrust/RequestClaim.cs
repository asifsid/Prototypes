using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class RequestClaim
	{
		private string _claimType;

		private bool _isOptional;

		private string _value;

		public string ClaimType => _claimType;

		public bool IsOptional
		{
			get
			{
				return _isOptional;
			}
			set
			{
				_isOptional = value;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public RequestClaim(string claimType)
			: this(claimType, isOptional: false)
		{
		}

		public RequestClaim(string claimType, bool isOptional)
			: this(claimType, isOptional, null)
		{
		}

		public RequestClaim(string claimType, bool isOptional, string value)
		{
			if (string.IsNullOrEmpty(claimType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "claimType"));
			}
			_claimType = claimType;
			_isOptional = isOptional;
			_value = value;
		}
	}
}
