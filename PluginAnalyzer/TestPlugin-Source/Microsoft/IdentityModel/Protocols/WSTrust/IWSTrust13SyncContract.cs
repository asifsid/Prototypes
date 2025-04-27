using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceContract(Name = "IWSTrust13Sync", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice")]
	[ComVisible(true)]
	public interface IWSTrust13SyncContract
	{
		[OperationContract(Name = "Trust13Cancel", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal")]
		Message ProcessTrust13Cancel(Message message);

		[OperationContract(Name = "Trust13Issue", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal")]
		Message ProcessTrust13Issue(Message message);

		[OperationContract(Name = "Trust13Renew", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal")]
		Message ProcessTrust13Renew(Message message);

		[OperationContract(Name = "Trust13Validate", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", ReplyAction = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal")]
		Message ProcessTrust13Validate(Message message);

		[OperationContract(Name = "Trust13CancelResponse", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", ReplyAction = "*")]
		Message ProcessTrust13CancelResponse(Message message);

		[OperationContract(Name = "Trust13IssueResponse", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", ReplyAction = "*")]
		Message ProcessTrust13IssueResponse(Message message);

		[OperationContract(Name = "Trust13RenewResponse", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", ReplyAction = "*")]
		Message ProcessTrust13RenewResponse(Message message);

		[OperationContract(Name = "Trust13ValidateResponse", Action = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", ReplyAction = "*")]
		Message ProcessTrust13ValidateResponse(Message message);
	}
}
