using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Microsoft.IdentityModel.Claims
{
	internal class ClaimFactoryPolicy : IAuthorizationPolicy, IAuthorizationComponent
	{
		private ReadOnlyCollection<ClaimSet> _claimSets;

		private DateTime _expirationTime;

		private string _id = UniqueId.CreateUniqueId("ClaimFactoryPolicy");

		public ClaimSet Issuer => ClaimSet.System;

		public string Id => _id;

		public ClaimFactoryPolicy(ReadOnlyCollection<ClaimSet> claimSets)
			: this(claimSets, DateTime.MaxValue)
		{
		}

		public ClaimFactoryPolicy(ReadOnlyCollection<ClaimSet> claimSets, DateTime expirationTime)
		{
			_claimSets = claimSets ?? EmptyReadOnlyCollection<ClaimSet>.Instance;
			_expirationTime = expirationTime;
		}

		public bool Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			if (evaluationContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("evaluationContext");
			}
			evaluationContext.RecordExpirationTime(_expirationTime);
			foreach (ClaimSet claimSet in _claimSets)
			{
				evaluationContext.AddClaimSet(this, claimSet);
			}
			return true;
		}
	}
}
