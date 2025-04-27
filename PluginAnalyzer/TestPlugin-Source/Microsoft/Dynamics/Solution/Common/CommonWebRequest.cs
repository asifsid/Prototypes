using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public sealed class CommonWebRequest : WebRequest
	{
		private HttpWebRequest request;

		public string Accept
		{
			get
			{
				return request.Accept;
			}
			set
			{
				request.Accept = value;
			}
		}

		public override string Method
		{
			get
			{
				return request.Method;
			}
			set
			{
				request.Method = value;
			}
		}

		public override string ContentType
		{
			get
			{
				return request.ContentType;
			}
			set
			{
				request.ContentType = value;
			}
		}

		public override WebHeaderCollection Headers => request.Headers;

		public override int Timeout
		{
			get
			{
				return request.Timeout;
			}
			set
			{
				request.Timeout = value;
			}
		}

		public override long ContentLength
		{
			get
			{
				return request.ContentLength;
			}
			set
			{
				request.ContentLength = value;
			}
		}

		public CommonWebRequest(Uri uri)
		{
			request = (HttpWebRequest)WebRequest.Create(uri);
		}

		private CommonWebRequest(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override WebResponse GetResponse()
		{
			using CommonWebResponse result = new CommonWebResponse((HttpWebResponse)request.GetResponse());
			return result;
		}

		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			return request.BeginGetResponse(callback, state);
		}

		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
			return new CommonWebResponse(response);
		}

		public override Stream GetRequestStream()
		{
			return request.GetRequestStream();
		}

		public void AddClientCertificate(X509Certificate2 cert)
		{
			request.ClientCertificates.Add(cert);
		}
	}
}
