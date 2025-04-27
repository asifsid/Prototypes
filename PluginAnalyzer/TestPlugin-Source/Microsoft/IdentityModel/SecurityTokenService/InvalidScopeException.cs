using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidScopeException : RequestException
	{
		private const string ScopeProperty = "Scope";

		private string _address;

		public string Scope => _address;

		public InvalidScopeException()
			: this(string.Empty)
		{
		}

		public InvalidScopeException(string address)
			: base(SR.GetString("ID2010", address))
		{
			_address = address;
		}

		public InvalidScopeException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidScopeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			_address = info.GetValue("Scope", typeof(string)) as string;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			info.AddValue("Scope", Scope);
			base.GetObjectData(info, context);
		}
	}
}
