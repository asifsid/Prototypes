using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Web
{
	[Serializable]
	[ComVisible(true)]
	public class FederatedSessionExpiredException : FederatedAuthenticationSessionEndingException
	{
		private DateTime _tested;

		private DateTime _expired;

		public DateTime Expired => _expired;

		public DateTime Tested => _tested;

		public FederatedSessionExpiredException()
		{
		}

		public FederatedSessionExpiredException(DateTime tested, DateTime expired)
			: this(SR.GetString("ID1004", tested, expired))
		{
			_tested = tested;
			_expired = expired;
		}

		public FederatedSessionExpiredException(DateTime tested, DateTime expired, Exception inner)
			: this(SR.GetString("ID1004", tested, expired), inner)
		{
			_tested = tested;
			_expired = expired;
		}

		public FederatedSessionExpiredException(string message)
			: base(message)
		{
		}

		public FederatedSessionExpiredException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected FederatedSessionExpiredException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			_expired = info.GetDateTime("Expired");
			_tested = info.GetDateTime("Tested");
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Expired", _expired, typeof(DateTime));
			info.AddValue("Tested", _tested, typeof(DateTime));
		}
	}
}
