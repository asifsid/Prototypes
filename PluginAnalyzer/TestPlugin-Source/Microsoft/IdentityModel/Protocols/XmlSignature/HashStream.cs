using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class HashStream : Stream
	{
		private HashAlgorithm _hash;

		private long _length;

		private bool _hashNeedsReset;

		private MemoryStream _logStream;

		private TraceEventType _traceEventType = TraceEventType.Verbose;

		private bool _disposed;

		public override bool CanRead => false;

		public override bool CanWrite => true;

		public override bool CanSeek => false;

		public HashAlgorithm Hash => _hash;

		public override long Length => _length;

		public override long Position
		{
			get
			{
				return _length;
			}
			set
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
			}
		}

		public HashStream(HashAlgorithm hash)
		{
			if (hash == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("hash");
			}
			Reset(hash);
		}

		public override void Flush()
		{
		}

		public void FlushHash()
		{
			FlushHash(null);
		}

		public void FlushHash(MemoryStream preCanonicalBytes)
		{
			_hash.TransformFinalBlock(new byte[0], 0, 0);
			if (_logStream != null)
			{
				DiagnosticUtil.TraceUtil.Trace(_traceEventType, TraceCode.Diagnostics, null, new HashTraceRecord(Convert.ToBase64String(_hash.Hash), _logStream.ToArray(), preCanonicalBytes?.ToArray()), null);
			}
		}

		public byte[] FlushHashAndGetValue()
		{
			return FlushHashAndGetValue(null);
		}

		public byte[] FlushHashAndGetValue(MemoryStream preCanonicalBytes)
		{
			FlushHash(preCanonicalBytes);
			return _hash.Hash;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
		}

		public void Reset()
		{
			if (_hashNeedsReset)
			{
				_hash.Initialize();
				_hashNeedsReset = false;
			}
			_length = 0L;
			if (DiagnosticUtil.TraceUtil.ShouldTrace(_traceEventType))
			{
				_logStream = new MemoryStream();
			}
		}

		public void Reset(HashAlgorithm hash)
		{
			_hash = hash;
			_hashNeedsReset = false;
			_length = 0L;
			if (DiagnosticUtil.TraceUtil.ShouldTrace(_traceEventType))
			{
				_logStream = new MemoryStream();
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_hash.TransformBlock(buffer, offset, count, buffer, offset);
			_length += count;
			_hashNeedsReset = true;
			if (_logStream != null)
			{
				_logStream.Write(buffer, offset, count);
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
		}

		public override void SetLength(long length)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!_disposed)
			{
				if (disposing && _logStream != null)
				{
					_logStream.Dispose();
					_logStream = null;
				}
				_disposed = true;
			}
		}
	}
}
