using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2SubjectLocality
	{
		private string _address;

		private string _dnsName;

		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public string DnsName
		{
			get
			{
				return _dnsName;
			}
			set
			{
				_dnsName = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public Saml2SubjectLocality()
		{
		}

		public Saml2SubjectLocality(string address, string dnsName)
		{
			Address = address;
			DnsName = dnsName;
		}
	}
}
