using System;
using System.IO;

namespace Newtonsoft.Json.Utilities
{
	internal class Base64Encoder
	{
		private const int Base64LineSize = 76;

		private const int LineSizeInBytes = 57;

		private readonly char[] _charsLine = new char[76];

		private readonly TextWriter _writer;

		private byte[] _leftOverBytes;

		private int _leftOverBytesCount;

		public Base64Encoder(TextWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			_writer = writer;
		}

		private void ValidateEncode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
		}

		public void Encode(byte[] buffer, int index, int count)
		{
			ValidateEncode(buffer, index, count);
			if (_leftOverBytesCount > 0)
			{
				if (FulfillFromLeftover(buffer, index, ref count))
				{
					return;
				}
				int count2 = Convert.ToBase64CharArray(_leftOverBytes, 0, 3, _charsLine, 0);
				WriteChars(_charsLine, 0, count2);
			}
			StoreLeftOverBytes(buffer, index, ref count);
			int num = index + count;
			int num2 = 57;
			while (index < num)
			{
				if (index + num2 > num)
				{
					num2 = num - index;
				}
				int count3 = Convert.ToBase64CharArray(buffer, index, num2, _charsLine, 0);
				WriteChars(_charsLine, 0, count3);
				index += num2;
			}
		}

		private void StoreLeftOverBytes(byte[] buffer, int index, ref int count)
		{
			int num = count % 3;
			if (num > 0)
			{
				count -= num;
				if (_leftOverBytes == null)
				{
					_leftOverBytes = new byte[3];
				}
				for (int i = 0; i < num; i++)
				{
					_leftOverBytes[i] = buffer[index + count + i];
				}
			}
			_leftOverBytesCount = num;
		}

		private bool FulfillFromLeftover(byte[] buffer, int index, ref int count)
		{
			int leftOverBytesCount = _leftOverBytesCount;
			while (leftOverBytesCount < 3 && count > 0)
			{
				_leftOverBytes[leftOverBytesCount++] = buffer[index++];
				count--;
			}
			if (count == 0 && leftOverBytesCount < 3)
			{
				_leftOverBytesCount = leftOverBytesCount;
				return true;
			}
			return false;
		}

		public void Flush()
		{
			if (_leftOverBytesCount > 0)
			{
				int count = Convert.ToBase64CharArray(_leftOverBytes, 0, _leftOverBytesCount, _charsLine, 0);
				WriteChars(_charsLine, 0, count);
				_leftOverBytesCount = 0;
			}
		}

		private void WriteChars(char[] chars, int index, int count)
		{
			_writer.Write(chars, index, count);
		}
	}
}
