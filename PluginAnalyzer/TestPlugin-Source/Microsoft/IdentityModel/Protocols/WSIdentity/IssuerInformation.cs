using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class IssuerInformation
	{
		private string _key;

		private string _value;

		public string Key => _key;

		public string Value => _value;

		public IssuerInformation(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("key");
			}
			if (string.IsNullOrEmpty(value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
			}
			_key = key;
			_value = value;
		}
	}
}
