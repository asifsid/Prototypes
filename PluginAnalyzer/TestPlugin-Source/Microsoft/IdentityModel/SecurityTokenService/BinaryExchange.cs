using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class BinaryExchange
	{
		private byte[] _binaryData;

		private Uri _valueType;

		private Uri _encodingType;

		public byte[] BinaryData => _binaryData;

		public Uri ValueType => _valueType;

		public Uri EncodingType => _encodingType;

		public BinaryExchange(byte[] binaryData, Uri valueType)
			: this(binaryData, valueType, new Uri("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"))
		{
		}

		public BinaryExchange(byte[] binaryData, Uri valueType, Uri encodingType)
		{
			if (binaryData == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("binaryData");
			}
			if (valueType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("valueType");
			}
			if (encodingType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encodingType");
			}
			if (!valueType.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("valueType", SR.GetString("ID0013"));
			}
			if (!encodingType.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("encodingType", SR.GetString("ID0013"));
			}
			_binaryData = binaryData;
			_valueType = valueType;
			_encodingType = encodingType;
		}
	}
}
