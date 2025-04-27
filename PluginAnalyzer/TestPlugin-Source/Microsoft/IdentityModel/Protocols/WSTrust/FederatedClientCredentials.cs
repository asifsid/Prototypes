using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class FederatedClientCredentials : ClientCredentials
	{
		private SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;

		public new SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager => _securityTokenHandlerCollectionManager;

		public FederatedClientCredentials()
			: this(SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())
		{
		}

		public FederatedClientCredentials(SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
		{
			if (securityTokenHandlerCollectionManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollectionManager");
			}
			_securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager;
		}

		public FederatedClientCredentials(ClientCredentials other)
			: this(other, GetSecurityTokenHandlerCollectionManagerForCredentials(other))
		{
		}

		public FederatedClientCredentials(ClientCredentials other, SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
			: base(other)
		{
			if (securityTokenHandlerCollectionManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollectionManager");
			}
			_securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager;
		}

		protected override ClientCredentials CloneCore()
		{
			return new FederatedClientCredentials(this, _securityTokenHandlerCollectionManager);
		}

		public override SecurityTokenManager CreateSecurityTokenManager()
		{
			return new FederatedClientCredentialsSecurityTokenManager(this);
		}

		private static SecurityTokenHandlerCollectionManager GetSecurityTokenHandlerCollectionManagerForCredentials(ClientCredentials other)
		{
			FederatedClientCredentials federatedClientCredentials = other as FederatedClientCredentials;
			if (federatedClientCredentials != null)
			{
				return federatedClientCredentials._securityTokenHandlerCollectionManager;
			}
			return SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager();
		}
	}
}
