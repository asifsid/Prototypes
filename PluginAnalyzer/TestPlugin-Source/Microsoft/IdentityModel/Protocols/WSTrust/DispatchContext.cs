using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class DispatchContext
	{
		private IClaimsPrincipal _principal;

		private string _requestAction;

		private WSTrustMessage _requestMessage;

		private string _responseAction;

		private RequestSecurityTokenResponse _responseMessage;

		private Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService _securityTokenService;

		private string _trustNamespace;

		public IClaimsPrincipal Principal
		{
			get
			{
				return _principal;
			}
			set
			{
				_principal = value;
			}
		}

		public string RequestAction
		{
			get
			{
				return _requestAction;
			}
			set
			{
				_requestAction = value;
			}
		}

		public WSTrustMessage RequestMessage
		{
			get
			{
				return _requestMessage;
			}
			set
			{
				_requestMessage = value;
			}
		}

		public string ResponseAction
		{
			get
			{
				return _responseAction;
			}
			set
			{
				_responseAction = value;
			}
		}

		public RequestSecurityTokenResponse ResponseMessage
		{
			get
			{
				return _responseMessage;
			}
			set
			{
				_responseMessage = value;
			}
		}

		public Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService SecurityTokenService
		{
			get
			{
				return _securityTokenService;
			}
			set
			{
				_securityTokenService = value;
			}
		}

		public string TrustNamespace
		{
			get
			{
				return _trustNamespace;
			}
			set
			{
				_trustNamespace = value;
			}
		}
	}
}
