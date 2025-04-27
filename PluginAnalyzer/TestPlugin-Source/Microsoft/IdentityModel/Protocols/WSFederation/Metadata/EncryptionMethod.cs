using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class EncryptionMethod
	{
		private Uri _algorithm;

		public Uri Algorithm
		{
			get
			{
				return _algorithm;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_algorithm = value;
			}
		}

		public EncryptionMethod(Uri algorithm)
		{
			if (algorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("algorithm");
			}
			_algorithm = algorithm;
		}
	}
}
