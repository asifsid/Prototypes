using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public sealed class ChunkedCookieHandler : CookieHandler
	{
		public const int DefaultChunkSize = 2000;

		public const int MinimumChunkSize = 1000;

		private int _chunkSize;

		public int ChunkSize => _chunkSize;

		public ChunkedCookieHandler()
			: this(2000)
		{
		}

		public ChunkedCookieHandler(int chunkSize)
		{
			if (chunkSize < 1000)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("chunkSize", SR.GetString("ID1016", 1000));
			}
			_chunkSize = chunkSize;
		}

		protected override void DeleteCore(string name, string path, string domain, HttpContext context)
		{
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			DeleteInternal(name, path, domain, context.Request.Cookies, context.Response.Cookies);
		}

		internal void DeleteInternal(string name, string path, string domain, HttpCookieCollection requestCookies, HttpCookieCollection responseCookies)
		{
			foreach (HttpCookie cookieChunk in GetCookieChunks(name, requestCookies))
			{
				HttpCookie httpCookie = new HttpCookie(cookieChunk.Name, null);
				httpCookie.Path = path;
				httpCookie.Expires = DateTime.UtcNow.AddDays(-1.0);
				if (!string.IsNullOrEmpty(domain))
				{
					httpCookie.Domain = domain;
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, null, new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Deleting, cookieChunk, path), null);
				}
				responseCookies.Set(httpCookie);
			}
		}

		private IEnumerable<KeyValuePair<string, string>> GetCookieChunks(string baseName, string cookieValue)
		{
			int chunksRequired = CryptoUtil.CeilingDivide(cookieValue.Length, _chunkSize);
			if (chunksRequired > 20 && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8008"));
			}
			for (int i = 0; i < chunksRequired; i++)
			{
				yield return new KeyValuePair<string, string>(GetChunkName(baseName, i), cookieValue.Substring(i * _chunkSize, Math.Min(cookieValue.Length - i * _chunkSize, _chunkSize)));
			}
		}

		private IEnumerable<HttpCookie> GetCookieChunks(string baseName, HttpCookieCollection cookies)
		{
			int chunkIndex = 0;
			string chunkName = GetChunkName(baseName, chunkIndex);
			while (true)
			{
				HttpCookie httpCookie;
				HttpCookie cookie = (httpCookie = cookies[chunkName]);
				if (httpCookie != null)
				{
					yield return cookie;
					int chunkIndex2;
					chunkIndex = (chunkIndex2 = chunkIndex + 1);
					chunkName = GetChunkName(baseName, chunkIndex2);
					continue;
				}
				break;
			}
		}

		protected override byte[] ReadCore(string name, HttpContext context)
		{
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			return ReadInternal(name, context.Request.Cookies);
		}

		internal byte[] ReadInternal(string name, HttpCookieCollection requestCookies)
		{
			StringBuilder stringBuilder = null;
			foreach (HttpCookie cookieChunk in GetCookieChunks(name, requestCookies))
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder();
				}
				stringBuilder.Append(cookieChunk.Value);
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, null, new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Reading, cookieChunk, cookieChunk.Path), null);
				}
			}
			if (stringBuilder != null)
			{
				return Convert.FromBase64String(stringBuilder.ToString());
			}
			return null;
		}

		protected override void WriteCore(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpContext context)
		{
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WriteInternal(value, name, path, domain, expirationTime, secure, httpOnly, context.Request.Cookies, context.Response.Cookies);
		}

		internal void WriteInternal(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpCookieCollection requestCookies, HttpCookieCollection responseCookies)
		{
			string cookieValue = Convert.ToBase64String(value);
			DeleteInternal(name, path, domain, requestCookies, responseCookies);
			foreach (KeyValuePair<string, string> cookieChunk in GetCookieChunks(name, cookieValue))
			{
				HttpCookie httpCookie = new HttpCookie(cookieChunk.Key, cookieChunk.Value);
				httpCookie.Secure = secure;
				httpCookie.HttpOnly = httpOnly;
				httpCookie.Path = path;
				if (!string.IsNullOrEmpty(domain))
				{
					httpCookie.Domain = domain;
				}
				if (expirationTime != DateTime.MinValue)
				{
					httpCookie.Expires = expirationTime;
				}
				responseCookies.Set(httpCookie);
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, null, new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Writing, httpCookie, httpCookie.Path), null);
				}
			}
		}

		private static string GetChunkName(string baseName, int chunkIndex)
		{
			if (chunkIndex != 0)
			{
				return baseName + chunkIndex.ToString(CultureInfo.InvariantCulture);
			}
			return baseName;
		}
	}
}
