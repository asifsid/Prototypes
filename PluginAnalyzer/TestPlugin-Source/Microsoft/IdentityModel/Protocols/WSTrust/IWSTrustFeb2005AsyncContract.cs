using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceContract(Name = "IWSTrustFeb2005Async", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
	[ComVisible(true)]
	public interface IWSTrustFeb2005AsyncContract
	{
		[OperationContract(Name = "TrustFeb2005CancelAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
		IAsyncResult BeginTrustFeb2005Cancel(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005Cancel(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005IssueAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
		IAsyncResult BeginTrustFeb2005Issue(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005Issue(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005RenewAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
		IAsyncResult BeginTrustFeb2005Renew(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005Renew(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005ValidateAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
		IAsyncResult BeginTrustFeb2005Validate(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005Validate(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005CancelResponseAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
		IAsyncResult BeginTrustFeb2005CancelResponse(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005CancelResponse(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005IssueResponseAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
		IAsyncResult BeginTrustFeb2005IssueResponse(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005IssueResponse(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005RenewResponseAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
		IAsyncResult BeginTrustFeb2005RenewResponse(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005RenewResponse(IAsyncResult ar);

		[OperationContract(Name = "TrustFeb2005ValidateResponseAsync", AsyncPattern = true, Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
		IAsyncResult BeginTrustFeb2005ValidateResponse(Message request, AsyncCallback callback, object state);

		Message EndTrustFeb2005ValidateResponse(IAsyncResult ar);
	}
}
