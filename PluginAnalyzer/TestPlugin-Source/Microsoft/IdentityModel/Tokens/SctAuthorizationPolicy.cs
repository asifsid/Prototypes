using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Microsoft.IdentityModel.Tokens
{
	internal class SctAuthorizationPolicy : IAuthorizationPolicy, IAuthorizationComponent
	{
		private ClaimSet _issuer;

		private string _id = UniqueId.CreateUniqueId();

		ClaimSet IAuthorizationPolicy.Issuer => _issuer;

		string IAuthorizationComponent.Id => _id;

		internal SctAuthorizationPolicy(Claim claim)
		{
			_issuer = new DefaultClaimSet(claim);
		}

		bool IAuthorizationPolicy.Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			if (evaluationContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("evaluationContext");
			}
			evaluationContext.AddClaimSet(this, _issuer);
			return true;
		}
	}
}
