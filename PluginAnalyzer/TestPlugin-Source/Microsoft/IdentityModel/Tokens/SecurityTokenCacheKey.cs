using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenCacheKey
	{
		private System.Xml.UniqueId _contextId;

		private System.Xml.UniqueId _keyGeneration;

		private string _endpointId;

		private bool _isSessionMode;

		private bool _canIgnoreContextId;

		private bool _canIgnoreKeyGeneration;

		private bool _canIgnoreEndpointId;

		public System.Xml.UniqueId ContextId => _contextId;

		public System.Xml.UniqueId KeyGeneration => _keyGeneration;

		public string EndpointId => _endpointId;

		public bool CanIgnoreContextId
		{
			get
			{
				return _canIgnoreContextId;
			}
			set
			{
				_canIgnoreContextId = value;
			}
		}

		public bool CanIgnoreKeyGeneration
		{
			get
			{
				return _canIgnoreKeyGeneration;
			}
			set
			{
				_canIgnoreKeyGeneration = value;
			}
		}

		public bool CanIgnoreEndpointId
		{
			get
			{
				return _canIgnoreEndpointId;
			}
			set
			{
				_canIgnoreEndpointId = value;
			}
		}

		public bool IsSessionMode => _isSessionMode;

		public SecurityTokenCacheKey(string endpointId, System.Xml.UniqueId contextId, System.Xml.UniqueId keyGeneration, bool isSessionMode)
		{
			_endpointId = endpointId;
			_contextId = contextId;
			_keyGeneration = keyGeneration;
			_canIgnoreContextId = false;
			_canIgnoreKeyGeneration = false;
			_canIgnoreEndpointId = false;
			_isSessionMode = isSessionMode;
		}

		public override int GetHashCode()
		{
			if (_keyGeneration == null)
			{
				return _contextId.GetHashCode();
			}
			return _contextId.GetHashCode() ^ _keyGeneration.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is SecurityTokenCacheKey)
			{
				SecurityTokenCacheKey securityTokenCacheKey = (SecurityTokenCacheKey)obj;
				bool flag = true;
				if (!_canIgnoreEndpointId && !securityTokenCacheKey._canIgnoreEndpointId)
				{
					flag = StringComparer.Ordinal.Equals(securityTokenCacheKey.EndpointId, _endpointId);
				}
				if (flag && !_canIgnoreContextId && !securityTokenCacheKey._canIgnoreContextId)
				{
					flag = securityTokenCacheKey.ContextId == _contextId;
				}
				if (flag && !_canIgnoreKeyGeneration && !securityTokenCacheKey.CanIgnoreKeyGeneration)
				{
					flag = securityTokenCacheKey.KeyGeneration == _keyGeneration;
				}
				return flag;
			}
			return false;
		}

		public static bool operator ==(SecurityTokenCacheKey a, SecurityTokenCacheKey b)
		{
			if (object.ReferenceEquals(a, null))
			{
				return object.ReferenceEquals(b, null);
			}
			return a.Equals(b);
		}

		public static bool operator !=(SecurityTokenCacheKey a, SecurityTokenCacheKey b)
		{
			return !(a == b);
		}
	}
}
