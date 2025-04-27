using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public sealed class CommonWebResponse : WebResponse
	{
		public HttpWebResponse Response { get; set; }

		public override long ContentLength
		{
			get
			{
				return Response.ContentLength;
			}
			set
			{
				Response.ContentLength = value;
			}
		}

		public override string ContentType
		{
			get
			{
				return Response.ContentType;
			}
			set
			{
				Response.ContentType = value;
			}
		}

		public override Uri ResponseUri => Response.ResponseUri;

		public override WebHeaderCollection Headers => Response.Headers;

		public HttpStatusCode StatusCode => Response.StatusCode;

		public CommonWebResponse(HttpWebResponse response)
		{
			Response = response;
		}

		private CommonWebResponse(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override Stream GetResponseStream()
		{
			return Response.GetResponseStream();
		}
	}
}
