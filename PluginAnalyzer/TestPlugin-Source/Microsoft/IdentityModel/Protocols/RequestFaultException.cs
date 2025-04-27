using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols
{
	[Serializable]
	[ComVisible(true)]
	public abstract class RequestFaultException : FaultException
	{
		protected RequestFaultException(FaultReason reason, FaultCode code)
			: base(reason, code)
		{
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
