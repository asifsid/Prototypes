using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustSerializationContext
	{
		private SecurityTokenResolver _securityTokenResolver;

		private SecurityTokenResolver _useKeyTokenResolver;

		private SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;

		private SecurityTokenSerializer _securityTokenSerializer;

		public SecurityTokenResolver TokenResolver
		{
			get
			{
				return _securityTokenResolver;
			}
			set
			{
				_securityTokenResolver = value;
			}
		}

		public SecurityTokenResolver UseKeyTokenResolver
		{
			get
			{
				return _useKeyTokenResolver;
			}
			set
			{
				_useKeyTokenResolver = value;
			}
		}

		public SecurityTokenSerializer SecurityTokenSerializer
		{
			get
			{
				return _securityTokenSerializer;
			}
			set
			{
				_securityTokenSerializer = value;
			}
		}

		public SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager
		{
			get
			{
				return _securityTokenHandlerCollectionManager;
			}
			set
			{
				_securityTokenHandlerCollectionManager = value;
			}
		}

		public SecurityTokenHandlerCollection SecurityTokenHandlers => _securityTokenHandlerCollectionManager[""];

		public WSTrustSerializationContext()
			: this(SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())
		{
		}

		public WSTrustSerializationContext(SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
			: this(securityTokenHandlerCollectionManager, EmptySecurityTokenResolver.Instance, EmptySecurityTokenResolver.Instance)
		{
		}

		public WSTrustSerializationContext(SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager, SecurityTokenResolver securityTokenResolver, SecurityTokenResolver useKeyTokenResolver)
		{
			if (securityTokenHandlerCollectionManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollectionManager");
			}
			if (securityTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenResolver");
			}
			if (useKeyTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("useKeyTokenResolver");
			}
			_securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager;
			_securityTokenSerializer = new SecurityTokenSerializerAdapter(securityTokenHandlerCollectionManager[""]);
			_securityTokenResolver = securityTokenResolver;
			_useKeyTokenResolver = useKeyTokenResolver;
		}
	}
}
