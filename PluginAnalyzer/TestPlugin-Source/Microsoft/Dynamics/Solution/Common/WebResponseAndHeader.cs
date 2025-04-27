using System.Net;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class WebResponseAndHeader
	{
		public string Response { get; set; }

		public WebHeaderCollection Headers { get; set; }
	}
}
