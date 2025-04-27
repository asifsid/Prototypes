using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public abstract class OpenObject
	{
		private Dictionary<string, object> _properties = new Dictionary<string, object>();

		public Dictionary<string, object> Properties => _properties;
	}
}
