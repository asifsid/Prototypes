using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Tokens
{
	internal class SecurityTokenSpecificationEnumerable : IEnumerable<SecurityTokenSpecification>, IEnumerable
	{
		private SecurityMessageProperty _securityMessageProperty;

		public SecurityTokenSpecificationEnumerable(SecurityMessageProperty securityMessageProperty)
		{
			if (securityMessageProperty == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityMessageProperty");
			}
			_securityMessageProperty = securityMessageProperty;
		}

		public IEnumerator<SecurityTokenSpecification> GetEnumerator()
		{
			if (_securityMessageProperty.InitiatorToken != null)
			{
				yield return _securityMessageProperty.InitiatorToken;
			}
			if (_securityMessageProperty.ProtectionToken != null)
			{
				yield return _securityMessageProperty.ProtectionToken;
			}
			if (!_securityMessageProperty.HasIncomingSupportingTokens)
			{
				yield break;
			}
			foreach (SupportingTokenSpecification tokenSpecification in _securityMessageProperty.IncomingSupportingTokens)
			{
				if (tokenSpecification != null)
				{
					yield return tokenSpecification;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException());
		}
	}
}
