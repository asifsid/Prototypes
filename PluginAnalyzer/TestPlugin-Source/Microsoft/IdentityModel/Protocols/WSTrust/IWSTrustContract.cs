using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceContract]
	[ComVisible(true)]
	public interface IWSTrustContract
	{
		[OperationContract(Name = "Cancel", Action = "*", ReplyAction = "*")]
		Message Cancel(Message message);

		[OperationContract(AsyncPattern = true, Name = "Cancel", Action = "*", ReplyAction = "*")]
		IAsyncResult BeginCancel(Message message, AsyncCallback callback, object asyncState);

		Message EndCancel(IAsyncResult asyncResult);

		[OperationContract(Name = "Issue", Action = "*", ReplyAction = "*")]
		Message Issue(Message message);

		[OperationContract(AsyncPattern = true, Name = "Issue", Action = "*", ReplyAction = "*")]
		IAsyncResult BeginIssue(Message message, AsyncCallback callback, object asyncState);

		Message EndIssue(IAsyncResult asyncResult);

		[OperationContract(Name = "Renew", Action = "*", ReplyAction = "*")]
		Message Renew(Message message);

		[OperationContract(AsyncPattern = true, Name = "Renew", Action = "*", ReplyAction = "*")]
		IAsyncResult BeginRenew(Message message, AsyncCallback callback, object asyncState);

		Message EndRenew(IAsyncResult asyncResult);

		[OperationContract(Name = "Validate", Action = "*", ReplyAction = "*")]
		Message Validate(Message message);

		[OperationContract(AsyncPattern = true, Name = "Validate", Action = "*", ReplyAction = "*")]
		IAsyncResult BeginValidate(Message message, AsyncCallback callback, object asyncState);

		Message EndValidate(IAsyncResult asyncResult);
	}
}
