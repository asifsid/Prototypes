using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public sealed class DeflateCookieTransform : CookieTransform
	{
		private int _maxDecompressedSize = 1048576;

		public int MaxDecompressedSize
		{
			get
			{
				return _maxDecompressedSize;
			}
			set
			{
				_maxDecompressedSize = value;
			}
		}

		public override byte[] Decode(byte[] encoded)
		{
			if (encoded == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encoded");
			}
			if (encoded.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("encoded", SR.GetString("ID6045"));
			}
			MemoryStream stream = new MemoryStream(encoded);
			using DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, leaveOpen: false);
			using MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[1024];
			int num;
			do
			{
				num = deflateStream.Read(array, 0, array.Length);
				memoryStream.Write(array, 0, num);
				if (memoryStream.Length > MaxDecompressedSize)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID1068", MaxDecompressedSize)));
				}
			}
			while (num > 0);
			return memoryStream.ToArray();
		}

		public override byte[] Encode(byte[] value)
		{
			if (value == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			if (value.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID6044"));
			}
			using MemoryStream memoryStream = new MemoryStream();
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
			{
				deflateStream.Write(value, 0, value.Length);
			}
			byte[] array = memoryStream.ToArray();
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, null, new DeflateCookieTraceRecord(value.Length, array.Length), null);
			}
			return array;
		}
	}
}
