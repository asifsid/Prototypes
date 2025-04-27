using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using Microsoft.Dynamics.Solution.Common.Extensions;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public class CrmException : FaultException<OrganizationServiceFault>
	{
		public HttpStatusCode StatusCode { get; set; }

		public int ErrorCode => base.HResult;

		public CrmException(string formattedErrorMessage, int errorCode, HttpStatusCode statusCode, params object[] arguments)
			: this(formattedErrorMessage, null, errorCode, statusCode, isFlowControlException: false)
		{
			int num = 0;
			foreach (object value in arguments)
			{
				Data.Add(num++, value);
			}
			StatusCode = statusCode;
		}

		public CrmException(string formattedErrorMessage, int errorCode, params object[] arguments)
			: this(formattedErrorMessage, null, errorCode, HttpStatusCode.BadRequest, isFlowControlException: false)
		{
		}

		public CrmException(string message)
			: this(message, null, -2147220970, HttpStatusCode.BadRequest, isFlowControlException: false)
		{
		}

		public CrmException(string message, int errorCode, bool enableTrace)
			: this(message, null, errorCode, HttpStatusCode.BadRequest, isFlowControlException: false, enableTrace)
		{
		}

		public CrmException(string message, int errorCode)
			: this(message, null, errorCode, HttpStatusCode.BadRequest, isFlowControlException: false)
		{
		}

		public CrmException(string message, Exception innerException)
			: this(message, innerException, -2147220970, HttpStatusCode.BadRequest, isFlowControlException: false)
		{
		}

		public CrmException(string message, Exception innerException, int errorCode)
			: this(message, innerException, errorCode, HttpStatusCode.BadRequest, isFlowControlException: false)
		{
		}

		public CrmException(string message, int errorCode, HttpStatusCode statusCode, bool enableTrace)
			: this(message, null, errorCode, statusCode, isFlowControlException: false, enableTrace)
		{
		}

		public CrmException(string message, int errorCode, HttpStatusCode statusCode)
			: this(message, null, errorCode, statusCode, isFlowControlException: false)
		{
		}

		public CrmException(string message, Exception innerException, int errorCode, HttpStatusCode statusCode)
			: this(message, innerException, errorCode, statusCode, isFlowControlException: false)
		{
		}

		protected CrmException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		protected CrmException(string message, Exception innerException, int errorCode, HttpStatusCode statusCode, bool isFlowControlException)
			: this(message, innerException, errorCode, statusCode, isFlowControlException, enableTrace: true)
		{
		}

		protected CrmException(string message, Exception innerException, int errorCode, HttpStatusCode statusCode, bool isFlowControlException, bool enableTrace)
			: base(BuildOrganizationServiceFault(innerException, errorCode, statusCode, message), new FaultReason(message))
		{
			StatusCode = statusCode;
			base.HResult = errorCode;
			((BaseServiceFault)base.Detail).set_Timestamp(DateTime.UtcNow);
			if (innerException == null)
			{
				if (!enableTrace)
				{
				}
			}
			else if (!enableTrace)
			{
			}
		}

		private static OrganizationServiceFault BuildOrganizationServiceFault(Exception innerException, int errorCode, HttpStatusCode statusCode, string message)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			OrganizationServiceFault val = new OrganizationServiceFault();
			((BaseServiceFault)val).set_ErrorCode(errorCode);
			((BaseServiceFault)val).set_Message(message);
			val.set_InnerFault(innerException?.ConvertToOrganizationServiceFault());
			((DataCollection<string, object>)(object)((BaseServiceFault)val).get_ErrorDetails()).set_Item("CallStack", (object)Environment.StackTrace);
			((DataCollection<string, object>)(object)((BaseServiceFault)val).get_ErrorDetails()).set_Item("HttpStatusCode", (object)statusCode);
			return val;
		}

		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				throw new CrmException(message, -2147220970);
			}
		}
	}
}
