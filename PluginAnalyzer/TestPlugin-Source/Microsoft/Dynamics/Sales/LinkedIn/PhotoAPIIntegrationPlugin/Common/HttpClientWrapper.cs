using System;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[ComVisible(true)]
	public sealed class HttpClientWrapper
	{
		private static readonly Lazy<HttpClientWrapper> _instance = new Lazy<HttpClientWrapper>(() => new HttpClientWrapper());

		public HttpClient Client;

		public static HttpClientWrapper Instance => _instance.Value;

		private HttpClientWrapper()
		{
			if (Client == null)
			{
				Client = new HttpClient();
			}
		}
	}
}
