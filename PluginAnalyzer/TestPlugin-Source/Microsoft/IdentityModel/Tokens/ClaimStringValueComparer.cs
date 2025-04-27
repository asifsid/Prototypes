using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	internal class ClaimStringValueComparer : IEqualityComparer<Claim>
	{
		public bool Equals(Claim claim1, Claim claim2)
		{
			if (object.ReferenceEquals(claim1, claim2))
			{
				return true;
			}
			if (claim1 == null || claim2 == null)
			{
				return false;
			}
			if (claim1.ClaimType != claim2.ClaimType || claim1.Right != claim2.Right)
			{
				return false;
			}
			return StringComparer.OrdinalIgnoreCase.Equals(claim1.Resource, claim2.Resource);
		}

		public int GetHashCode(Claim claim)
		{
			if (claim == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claim");
			}
			return claim.ClaimType.GetHashCode() ^ claim.Right.GetHashCode() ^ ((claim.Resource != null) ? claim.Resource.GetHashCode() : 0);
		}
	}
}
