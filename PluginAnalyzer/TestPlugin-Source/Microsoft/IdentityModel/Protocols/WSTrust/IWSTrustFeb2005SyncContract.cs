using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceContract(Name = "IWSTrustFeb2005Sync", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
	[ComVisible(true)]
	public interface IWSTrustFeb2005SyncContract
	{
		[OperationContract(Name = "TrustFeb2005Cancel", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
		Message ProcessTrustFeb2005Cancel(Message message);

		[OperationContract(Name = "TrustFeb2005Issue", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
		Message ProcessTrustFeb2005Issue(Message message);

		[OperationContract(Name = "TrustFeb2005Renew", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
		Message ProcessTrustFeb2005Renew(Message message);

		[OperationContract(Name = "TrustFeb2005Validate", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
		Message ProcessTrustFeb2005Validate(Message message);

		[OperationContract(Name = "TrustFeb2005CancelResponse", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel")]
		Message ProcessTrustFeb2005CancelResponse(Message message);

		[OperationContract(Name = "TrustFeb2005IssueResponse", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue")]
		Message ProcessTrustFeb2005IssueResponse(Message message);

		[OperationContract(Name = "TrustFeb2005RenewResponse", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew")]
		Message ProcessTrustFeb2005RenewResponse(Message message);

		[OperationContract(Name = "TrustFeb2005ValidateResponse", Action = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", ReplyAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate")]
		Message ProcessTrustFeb2005ValidateResponse(Message message);
	}
}
