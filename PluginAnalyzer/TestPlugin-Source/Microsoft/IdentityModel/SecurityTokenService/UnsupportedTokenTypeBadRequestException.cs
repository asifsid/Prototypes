using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedTokenTypeBadRequestException : BadRequestException
	{
		private const string TokenTypeProperty = "TokenType";

		private string _tokenType;

		public string TokenType
		{
			get
			{
				return _tokenType;
			}
			set
			{
				_tokenType = value;
			}
		}

		public UnsupportedTokenTypeBadRequestException()
		{
			_tokenType = string.Empty;
		}

		public UnsupportedTokenTypeBadRequestException(string tokenType)
			: base(SR.GetString("ID2014", tokenType))
		{
			_tokenType = tokenType;
		}

		public UnsupportedTokenTypeBadRequestException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnsupportedTokenTypeBadRequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			_tokenType = info.GetValue("TokenType", typeof(string)) as string;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			info.AddValue("TokenType", TokenType);
			base.GetObjectData(info, context);
		}
	}
}
