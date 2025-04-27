using System;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.Extensions
{
	[ComVisible(true)]
	public static class ExceptionExtensions
	{
		public static OrganizationServiceFault ConvertToOrganizationServiceFault(this Exception exception)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_003d: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			OrganizationServiceFault val = new OrganizationServiceFault();
			((BaseServiceFault)val).set_Message(exception.Message);
			((BaseServiceFault)val).set_ErrorCode(exception.HResult);
			ErrorDetailCollection val2 = new ErrorDetailCollection();
			((DataCollection<string, object>)val2).set_Item("CallStack", (object)exception.StackTrace);
			((BaseServiceFault)val).set_ErrorDetails(val2);
			val.set_InnerFault(exception.InnerException?.ConvertToOrganizationServiceFault());
			return val;
		}
	}
}
