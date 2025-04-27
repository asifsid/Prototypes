using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceContract(Name = "IWSTrust13Async", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
	[ComVisible(true)]
	public interface IWSTrust13AsyncContract
	{
		[OperationContract(Name = "Trust13CancelAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal")]
		IAsyncResult BeginTrust13Cancel(Message request, AsyncCallback callback, object state);

		Message EndTrust13Cancel(IAsyncResult ar);

		[OperationContract(Name = "Trust13IssueAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal")]
		IAsyncResult BeginTrust13Issue(Message request, AsyncCallback callback, object state);

		Message EndTrust13Issue(IAsyncResult ar);

		[OperationContract(Name = "Trust13RenewAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal")]
		IAsyncResult BeginTrust13Renew(Message request, AsyncCallback callback, object state);

		Message EndTrust13Renew(IAsyncResult ar);

		[OperationContract(Name = "Trust13ValidateAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal")]
		IAsyncResult BeginTrust13Validate(Message request, AsyncCallback callback, object state);

		Message EndTrust13Validate(IAsyncResult ar);

		[OperationContract(Name = "Trust13CancelResponseAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", ReplyAction = "*")]
		IAsyncResult BeginTrust13CancelResponse(Message request, AsyncCallback callback, object state);

		Message EndTrust13CancelResponse(IAsyncResult ar);

		[OperationContract(Name = "Trust13IssueResponseAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", ReplyAction = "*")]
		IAsyncResult BeginTrust13IssueResponse(Message request, AsyncCallback callback, object state);

		Message EndTrust13IssueResponse(IAsyncResult ar);

		[OperationContract(Name = "Trust13RenewResponseAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", ReplyAction = "*")]
		IAsyncResult BeginTrust13RenewResponse(Message request, AsyncCallback callback, object state);

		Message EndTrust13RenewResponse(IAsyncResult ar);

		[OperationContract(Name = "Trust13ValidateResponseAsync", AsyncPattern = true, Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", ReplyAction = "*")]
		IAsyncResult BeginTrust13ValidateResponse(Message request, AsyncCallback callback, object state);

		Message EndTrust13ValidateResponse(IAsyncResult ar);
	}
}
