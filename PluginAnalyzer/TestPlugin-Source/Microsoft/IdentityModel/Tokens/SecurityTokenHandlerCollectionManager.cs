using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenHandlerCollectionManager
	{
		public static class Usage
		{
			public const string Default = "";

			public const string ActAs = "ActAs";

			public const string OnBehalfOf = "OnBehalfOf";
		}

		private Dictionary<string, SecurityTokenHandlerCollection> _collections = new Dictionary<string, SecurityTokenHandlerCollection>();

		private string _serviceName = "";

		public SecurityTokenHandlerCollection this[string usage]
		{
			get
			{
				if (usage == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("usage");
				}
				return _collections[usage];
			}
			set
			{
				if (usage == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("usage");
				}
				_collections[usage] = value;
			}
		}

		public int Count => _collections.Count;

		public string ServiceName => _serviceName;

		public IEnumerable<SecurityTokenHandlerCollection> SecurityTokenHandlerCollections => _collections.Values;

		public static SecurityTokenHandlerCollectionManager CreateEmptySecurityTokenHandlerCollectionManager()
		{
			return new SecurityTokenHandlerCollectionManager("");
		}

		public static SecurityTokenHandlerCollectionManager CreateDefaultSecurityTokenHandlerCollectionManager()
		{
			SecurityTokenHandlerCollection value = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
			SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager = new SecurityTokenHandlerCollectionManager("");
			securityTokenHandlerCollectionManager._collections.Clear();
			securityTokenHandlerCollectionManager._collections.Add("", value);
			return securityTokenHandlerCollectionManager;
		}

		private SecurityTokenHandlerCollectionManager()
			: this("")
		{
		}

		public bool ContainsKey(string usage)
		{
			return _collections.ContainsKey(usage);
		}

		public SecurityTokenHandlerCollectionManager(string serviceName)
		{
			if (serviceName == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceName");
			}
			_serviceName = serviceName;
		}
	}
}
