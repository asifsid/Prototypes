using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class X509Principal
	{
		private EkuPolicy _ekuPolicy;

		private string _principalName;

		public EkuPolicy EkuPolicy
		{
			get
			{
				return _ekuPolicy;
			}
			set
			{
				_ekuPolicy = value;
			}
		}

		public string PrincipalName
		{
			get
			{
				return _principalName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_principalName = value;
			}
		}

		public X509Principal(string principalName)
			: this(principalName, null)
		{
		}

		public X509Principal(string principalName, EkuPolicy ekuPolicy)
		{
			if (string.IsNullOrEmpty(principalName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("principalName");
			}
			_principalName = principalName;
			_ekuPolicy = ekuPolicy;
		}
	}
}
