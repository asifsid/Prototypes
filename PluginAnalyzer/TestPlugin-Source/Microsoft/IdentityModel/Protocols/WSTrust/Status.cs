using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class Status
	{
		private string _code;

		private string _reason;

		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("code");
				}
				_code = value;
			}
		}

		public string Reason
		{
			get
			{
				return _reason;
			}
			set
			{
				_reason = value;
			}
		}

		public Status(string code, string reason)
		{
			if (string.IsNullOrEmpty(code))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("code");
			}
			_code = code;
			_reason = reason;
		}
	}
}
