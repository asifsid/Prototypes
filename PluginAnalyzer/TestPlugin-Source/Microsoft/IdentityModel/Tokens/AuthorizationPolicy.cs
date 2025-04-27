using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class AuthorizationPolicy : IAuthorizationPolicy, IAuthorizationComponent
	{
		public const string PrincipalKey = "Principal";

		public const string IdentitiesKey = "Identities";

		private ClaimsIdentityCollection _identityCollection = new ClaimsIdentityCollection();

		private ClaimSet _issuer = new DefaultClaimSet(new System.IdentityModel.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims", null, Rights.Identity));

		private string _id = UniqueId.CreateUniqueId();

		public ClaimsIdentityCollection IdentityCollection => _identityCollection;

		public ClaimSet Issuer => _issuer;

		public string Id => _id;

		public AuthorizationPolicy()
		{
		}

		public AuthorizationPolicy(IClaimsIdentity identity)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			_identityCollection.Add(identity);
		}

		public AuthorizationPolicy(ClaimsIdentityCollection identityCollection)
		{
			if (identityCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identityCollection");
			}
			_identityCollection = identityCollection;
		}

		public bool Evaluate(EvaluationContext evaluationContext, ref object state)
		{
			if (evaluationContext == null || evaluationContext.Properties == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("evaluationContext");
			}
			if (_identityCollection.Count == 0)
			{
				return true;
			}
			object value = null;
			if (!evaluationContext.Properties.TryGetValue("Principal", out value))
			{
				IClaimsPrincipal claimsPrincipal = ClaimsPrincipal.CreateFromIdentities(_identityCollection);
				evaluationContext.Properties.Add("Principal", claimsPrincipal);
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, SR.GetString("TraceSetPrincipalOnEvaluationContext"), new ClaimsPrincipalTraceRecord(claimsPrincipal), null);
				}
			}
			else
			{
				IClaimsPrincipal claimsPrincipal2 = value as IClaimsPrincipal;
				if (claimsPrincipal2 != null && claimsPrincipal2.Identities != null)
				{
					claimsPrincipal2.Identities.AddRange(_identityCollection);
				}
				else if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Error))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Error, SR.GetString("ID8004", "Principal"));
				}
			}
			object value2 = null;
			if (!evaluationContext.Properties.TryGetValue("Identities", out value2))
			{
				List<IIdentity> list = new List<IIdentity>();
				foreach (IClaimsIdentity item in _identityCollection)
				{
					list.Add(item);
				}
				evaluationContext.Properties.Add("Identities", list);
			}
			else
			{
				List<IIdentity> list2 = value2 as List<IIdentity>;
				foreach (IClaimsIdentity item2 in _identityCollection)
				{
					list2.Add(item2);
				}
			}
			return true;
		}
	}
}
