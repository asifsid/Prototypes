using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class X509SubjectAndIssuer
	{
		private EkuPolicy _ekuPolicy;

		private string _x509Issuer;

		private string _x509Subject;

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

		public string X509Issuer
		{
			get
			{
				return _x509Issuer;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_x509Issuer = value;
			}
		}

		public string X509Subject
		{
			get
			{
				return _x509Subject;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_x509Subject = value;
			}
		}

		public X509SubjectAndIssuer(string x509Subject, string x509Issuer)
			: this(x509Subject, x509Issuer, null)
		{
		}

		public X509SubjectAndIssuer(string x509Subject, string x509Issuer, EkuPolicy ekuPolicy)
		{
			if (string.IsNullOrEmpty(x509Subject))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("x509Subject");
			}
			if (string.IsNullOrEmpty(x509Issuer))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("x509Issuer");
			}
			_x509Issuer = x509Issuer;
			_x509Subject = x509Subject;
			_ekuPolicy = ekuPolicy;
		}
	}
}
