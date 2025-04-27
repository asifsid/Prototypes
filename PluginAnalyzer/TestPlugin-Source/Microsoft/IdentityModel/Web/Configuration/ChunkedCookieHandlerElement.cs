using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
	[ComVisible(true)]
	public class ChunkedCookieHandlerElement : ConfigurationElement
	{
		[ConfigurationProperty("chunkSize", IsRequired = false, DefaultValue = 2000)]
		public int ChunkSize
		{
			get
			{
				return (int)base["chunkSize"];
			}
			set
			{
				base["chunkSize"] = value;
			}
		}

		public bool IsConfigured => ChunkSize != 2000;
	}
}
