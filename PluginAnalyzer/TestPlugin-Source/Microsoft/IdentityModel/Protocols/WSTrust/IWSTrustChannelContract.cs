using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceContract]
	[ComVisible(false)]
	public interface IWSTrustChannelContract : IWSTrustContract
	{
		RequestSecurityTokenResponse Cancel(RequestSecurityToken request);

		IAsyncResult BeginCancel(RequestSecurityToken request, AsyncCallback callback, object state);

		void EndCancel(IAsyncResult result, out RequestSecurityTokenResponse response);

		SecurityToken Issue(RequestSecurityToken request);

		SecurityToken Issue(RequestSecurityToken request, out RequestSecurityTokenResponse response);

		IAsyncResult BeginIssue(RequestSecurityToken request, AsyncCallback callback, object asyncState);

		SecurityToken EndIssue(IAsyncResult result, out RequestSecurityTokenResponse response);

		RequestSecurityTokenResponse Renew(RequestSecurityToken request);

		IAsyncResult BeginRenew(RequestSecurityToken request, AsyncCallback callback, object state);

		void EndRenew(IAsyncResult result, out RequestSecurityTokenResponse response);

		RequestSecurityTokenResponse Validate(RequestSecurityToken request);

		IAsyncResult BeginValidate(RequestSecurityToken request, AsyncCallback callback, object state);

		void EndValidate(IAsyncResult result, out RequestSecurityTokenResponse response);
	}
}
