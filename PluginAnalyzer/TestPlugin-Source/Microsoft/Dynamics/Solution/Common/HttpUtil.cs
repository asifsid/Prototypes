using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class HttpUtil
	{
		private const int RateLimitCode = 429;

		public static string GetWebResponse(CommonWebRequest request)
		{
			using CommonWebResponse commonWebResponse = (CommonWebResponse)request.GetResponse();
			using Stream responseStream = commonWebResponse.GetResponseStream();
			return ReadResponseStream(responseStream);
		}

		public static Task<string> GetWebResponseAsync(CommonWebRequest request)
		{
			TaskCompletionSource<string> responseTask = new TaskCompletionSource<string>();
			request.BeginGetResponse(delegate(IAsyncResult result)
			{
				try
				{
					using CommonWebResponse commonWebResponse = (CommonWebResponse)request.EndGetResponse(result);
					using (commonWebResponse.GetResponseStream())
					{
						string result2 = ReadResponseStream(commonWebResponse.GetResponseStream());
						responseTask.SetResult(result2);
					}
				}
				catch (WebException exception)
				{
					responseTask.SetException(exception);
				}
			}, null);
			return responseTask.Task;
		}

		public static string GetWebResponse(HttpMethod httpMethod, Dictionary<string, string> headers, Uri uri, string content, bool isJsonContentType)
		{
			return GetWebResponse(httpMethod, headers, uri, content, isJsonContentType, -1);
		}

		public static string GetWebResponse(HttpMethod httpMethod, Dictionary<string, string> headers, Uri uri, string content, bool isJsonContentType, int maxExecutionTimeInMilliseconds)
		{
			CommonWebRequest commonWebRequest = PrepareBasicAuthRequest(uri, headers, httpMethod.Method, content, isJsonContentType);
			if (maxExecutionTimeInMilliseconds > 0)
			{
				commonWebRequest.Timeout = maxExecutionTimeInMilliseconds;
			}
			using WebResponse webResponse = commonWebRequest.GetResponse();
			using Stream responseStream = webResponse.GetResponseStream();
			return ReadResponseStream(responseStream);
		}

		public static WebResponseAndHeader GetResponse(HttpMethod httpMethod, Dictionary<string, string> headers, Uri uri, string content, bool isJsonContentType)
		{
			CommonWebRequest commonWebRequest = PrepareBasicAuthRequest(uri, headers, httpMethod.Method, content, isJsonContentType);
			WebResponseAndHeader webResponseAndHeader = new WebResponseAndHeader();
			using (WebResponse webResponse = commonWebRequest.GetResponse())
			{
				using (Stream responseStream = webResponse.GetResponseStream())
				{
					webResponseAndHeader.Response = ReadResponseStream(responseStream);
				}
				webResponseAndHeader.Headers = webResponse.Headers;
			}
			return webResponseAndHeader;
		}

		public static WebHeaderCollection GetResponseHeaders(HttpMethod httpMethod, Dictionary<string, string> headers, Uri uri, string content, bool isJsonContentType)
		{
			CommonWebRequest commonWebRequest = PrepareBasicAuthRequest(uri, headers, httpMethod.Method, content, isJsonContentType);
			using WebResponse webResponse = commonWebRequest.GetResponse();
			return webResponse.Headers;
		}

		public static CommonWebRequest PrepareBasicAuthRequest(Uri uri, Dictionary<string, string> headers, string method, string payload, bool isJsonContentType)
		{
			return PrepareBasicAuthRequest(uri, headers, method, payload, isJsonContentType, null);
		}

		public static CommonWebRequest PrepareBasicAuthRequest(Uri uri, Dictionary<string, string> headers, string method, string payload, bool isJsonContentType, X509Certificate2 cert)
		{
			CommonWebRequest commonWebRequest = new CommonWebRequest(uri);
			if (cert != null)
			{
				commonWebRequest.AddClientCertificate(cert);
			}
			commonWebRequest.ContentLength = 0L;
			if (!string.IsNullOrEmpty(method))
			{
				commonWebRequest.Method = method;
			}
			commonWebRequest.ContentType = (isJsonContentType ? "application/json" : "application/xml");
			if (headers.Count > 0)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					commonWebRequest.Headers.Add(header.Key, header.Value);
				}
			}
			if (!string.IsNullOrEmpty(payload))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(payload);
				commonWebRequest.ContentLength = bytes.Length;
				using Stream stream = commonWebRequest.GetRequestStream();
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();
			}
			return commonWebRequest;
		}

		private static string ReadResponseStream(Stream responseStream)
		{
			if (responseStream == null)
			{
				return null;
			}
			string result;
			using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}
	}
}
