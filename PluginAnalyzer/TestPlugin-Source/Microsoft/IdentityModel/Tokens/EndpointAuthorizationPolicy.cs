using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class EndpointAuthorizationPolicy : IAuthorizationPolicy, IAuthorizationComponent
	{
		private string _endpointId;

		private string _id = UniqueId.CreateUniqueId();

		public string EndpointId => _endpointId;

		ClaimSet IAuthorizationPolicy.Issuer => null;

		string IAuthorizationComponent.Id => _id;

		public EndpointAuthorizationPolicy(string endpointId)
		{
			if (endpointId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpointId");
			}
			_endpointId = endpointId;
		}

		bool IAuthorizationPolicy.Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			return true;
		}
	}
}
