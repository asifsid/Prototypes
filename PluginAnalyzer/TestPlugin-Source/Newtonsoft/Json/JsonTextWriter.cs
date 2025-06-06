using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	public class JsonTextWriter : JsonWriter
	{
		private const int IndentCharBufferSize = 12;

		private readonly TextWriter _writer;

		private Base64Encoder _base64Encoder;

		private char _indentChar;

		private int _indentation;

		private char _quoteChar;

		private bool _quoteName;

		private bool[] _charEscapeFlags;

		private char[] _writeBuffer;

		private IArrayPool<char> _arrayPool;

		private char[] _indentChars;

		private Base64Encoder Base64Encoder
		{
			get
			{
				if (_base64Encoder == null)
				{
					_base64Encoder = new Base64Encoder(_writer);
				}
				return _base64Encoder;
			}
		}

		public IArrayPool<char> ArrayPool
		{
			get
			{
				return _arrayPool;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				_arrayPool = value;
			}
		}

		public int Indentation
		{
			get
			{
				return _indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				_indentation = value;
			}
		}

		public char QuoteChar
		{
			get
			{
				return _quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				_quoteChar = value;
				UpdateCharEscapeFlags();
			}
		}

		public char IndentChar
		{
			get
			{
				return _indentChar;
			}
			set
			{
				if (value != _indentChar)
				{
					_indentChar = value;
					_indentChars = null;
				}
			}
		}

		public bool QuoteName
		{
			get
			{
				return _quoteName;
			}
			set
			{
				_quoteName = value;
			}
		}

		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			_writer = textWriter;
			_quoteChar = '"';
			_quoteName = true;
			_indentChar = ' ';
			_indentation = 2;
			UpdateCharEscapeFlags();
		}

		public override void Flush()
		{
			_writer.Flush();
		}

		public override void Close()
		{
			base.Close();
			CloseBufferAndWriter();
		}

		private void CloseBufferAndWriter()
		{
			if (_writeBuffer != null)
			{
				BufferUtils.ReturnBuffer(_arrayPool, _writeBuffer);
				_writeBuffer = null;
			}
			if (base.CloseOutput)
			{
				_writer?.Close();
			}
		}

		public override void WriteStartObject()
		{
			InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
			_writer.Write('{');
		}

		public override void WriteStartArray()
		{
			InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
			_writer.Write('[');
		}

		public override void WriteStartConstructor(string name)
		{
			InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
			_writer.Write("new ");
			_writer.Write(name);
			_writer.Write('(');
		}

		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				_writer.Write('}');
				break;
			case JsonToken.EndArray:
				_writer.Write(']');
				break;
			case JsonToken.EndConstructor:
				_writer.Write(')');
				break;
			default:
				throw JsonWriterException.Create(this, "Invalid JsonToken: " + token, null);
			}
		}

		public override void WritePropertyName(string name)
		{
			InternalWritePropertyName(name);
			WriteEscapedString(name, _quoteName);
			_writer.Write(':');
		}

		public override void WritePropertyName(string name, bool escape)
		{
			InternalWritePropertyName(name);
			if (escape)
			{
				WriteEscapedString(name, _quoteName);
			}
			else
			{
				if (_quoteName)
				{
					_writer.Write(_quoteChar);
				}
				_writer.Write(name);
				if (_quoteName)
				{
					_writer.Write(_quoteChar);
				}
			}
			_writer.Write(':');
		}

		internal override void OnStringEscapeHandlingChanged()
		{
			UpdateCharEscapeFlags();
		}

		private void UpdateCharEscapeFlags()
		{
			_charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(base.StringEscapeHandling, _quoteChar);
		}

		protected override void WriteIndent()
		{
			int num = base.Top * _indentation;
			int num2 = SetIndentChars();
			_writer.Write(_indentChars, 0, num2 + Math.Min(num, 12));
			while ((num -= 12) > 0)
			{
				_writer.Write(_indentChars, num2, Math.Min(num, 12));
			}
		}

		private int SetIndentChars()
		{
			string newLine = _writer.NewLine;
			int length = newLine.Length;
			bool flag = _indentChars != null && _indentChars.Length == 12 + length;
			if (flag)
			{
				for (int i = 0; i != length; i++)
				{
					if (newLine[i] != _indentChars[i])
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				_indentChars = (newLine + new string(_indentChar, 12)).ToCharArray();
			}
			return length;
		}

		protected override void WriteValueDelimiter()
		{
			_writer.Write(',');
		}

		protected override void WriteIndentSpace()
		{
			_writer.Write(' ');
		}

		private void WriteValueInternal(string value, JsonToken token)
		{
			_writer.Write(value);
		}

		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				InternalWriteValue(JsonToken.Integer);
				WriteValueInternal(((BigInteger)value).ToString(CultureInfo.InvariantCulture), JsonToken.String);
			}
			else
			{
				base.WriteValue(value);
			}
		}

		public override void WriteNull()
		{
			InternalWriteValue(JsonToken.Null);
			WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		public override void WriteUndefined()
		{
			InternalWriteValue(JsonToken.Undefined);
			WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		public override void WriteRaw(string json)
		{
			InternalWriteRaw();
			_writer.Write(json);
		}

		public override void WriteValue(string value)
		{
			InternalWriteValue(JsonToken.String);
			if (value == null)
			{
				WriteValueInternal(JsonConvert.Null, JsonToken.Null);
			}
			else
			{
				WriteEscapedString(value, quote: true);
			}
		}

		private void WriteEscapedString(string value, bool quote)
		{
			EnsureWriteBuffer();
			JavaScriptUtils.WriteEscapedJavaScriptString(_writer, value, _quoteChar, quote, _charEscapeFlags, base.StringEscapeHandling, _arrayPool, ref _writeBuffer);
		}

		public override void WriteValue(int value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(long value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value, negative: false);
		}

		public override void WriteValue(float value)
		{
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, QuoteChar, nullable: false), JsonToken.Float);
		}

		public override void WriteValue(float? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, QuoteChar, nullable: true), JsonToken.Float);
		}

		public override void WriteValue(double value)
		{
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, QuoteChar, nullable: false), JsonToken.Float);
		}

		public override void WriteValue(double? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value.GetValueOrDefault(), base.FloatFormatHandling, QuoteChar, nullable: true), JsonToken.Float);
		}

		public override void WriteValue(bool value)
		{
			InternalWriteValue(JsonToken.Boolean);
			WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		public override void WriteValue(short value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(char value)
		{
			InternalWriteValue(JsonToken.String);
			WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		public override void WriteValue(byte value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(decimal value)
		{
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		public override void WriteValue(DateTime value)
		{
			InternalWriteValue(JsonToken.Date);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				int count = WriteValueToBuffer(value);
				_writer.Write(_writeBuffer, 0, count);
			}
			else
			{
				_writer.Write(_quoteChar);
				_writer.Write(value.ToString(base.DateFormatString, base.Culture));
				_writer.Write(_quoteChar);
			}
		}

		private int WriteValueToBuffer(DateTime value)
		{
			EnsureWriteBuffer();
			int start = 0;
			_writeBuffer[start++] = _quoteChar;
			start = DateTimeUtils.WriteDateTimeString(_writeBuffer, start, value, null, value.Kind, base.DateFormatHandling);
			_writeBuffer[start++] = _quoteChar;
			return start;
		}

		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.Bytes);
			_writer.Write(_quoteChar);
			Base64Encoder.Encode(value, 0, value.Length);
			Base64Encoder.Flush();
			_writer.Write(_quoteChar);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			InternalWriteValue(JsonToken.Date);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				int count = WriteValueToBuffer(value);
				_writer.Write(_writeBuffer, 0, count);
			}
			else
			{
				_writer.Write(_quoteChar);
				_writer.Write(value.ToString(base.DateFormatString, base.Culture));
				_writer.Write(_quoteChar);
			}
		}

		private int WriteValueToBuffer(DateTimeOffset value)
		{
			EnsureWriteBuffer();
			int start = 0;
			_writeBuffer[start++] = _quoteChar;
			start = DateTimeUtils.WriteDateTimeString(_writeBuffer, start, (base.DateFormatHandling == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, value.Offset, DateTimeKind.Local, base.DateFormatHandling);
			_writeBuffer[start++] = _quoteChar;
			return start;
		}

		public override void WriteValue(Guid value)
		{
			InternalWriteValue(JsonToken.String);
			string value2 = value.ToString("D", CultureInfo.InvariantCulture);
			_writer.Write(_quoteChar);
			_writer.Write(value2);
			_writer.Write(_quoteChar);
		}

		public override void WriteValue(TimeSpan value)
		{
			InternalWriteValue(JsonToken.String);
			string value2 = value.ToString(null, CultureInfo.InvariantCulture);
			_writer.Write(_quoteChar);
			_writer.Write(value2);
			_writer.Write(_quoteChar);
		}

		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.String);
			WriteEscapedString(value.OriginalString, quote: true);
		}

		public override void WriteComment(string text)
		{
			InternalWriteComment();
			_writer.Write("/*");
			_writer.Write(text);
			_writer.Write("*/");
		}

		public override void WriteWhitespace(string ws)
		{
			InternalWriteWhitespace(ws);
			_writer.Write(ws);
		}

		private void EnsureWriteBuffer()
		{
			if (_writeBuffer == null)
			{
				_writeBuffer = BufferUtils.RentBuffer(_arrayPool, 35);
			}
		}

		private void WriteIntegerValue(long value)
		{
			if (value >= 0 && value <= 9)
			{
				_writer.Write((char)(48 + value));
				return;
			}
			bool flag = value < 0;
			WriteIntegerValue((ulong)(flag ? (-value) : value), flag);
		}

		private void WriteIntegerValue(ulong value, bool negative)
		{
			if (!negative && value <= 9)
			{
				_writer.Write((char)(48 + value));
				return;
			}
			int count = WriteNumberToBuffer(value, negative);
			_writer.Write(_writeBuffer, 0, count);
		}

		private int WriteNumberToBuffer(ulong value, bool negative)
		{
			if (value <= uint.MaxValue)
			{
				return WriteNumberToBuffer((uint)value, negative);
			}
			EnsureWriteBuffer();
			int num = MathUtils.IntLength(value);
			if (negative)
			{
				num++;
				_writeBuffer[0] = '-';
			}
			int num2 = num;
			do
			{
				ulong num3 = value / 10uL;
				ulong num4 = value - num3 * 10;
				_writeBuffer[--num2] = (char)(48 + num4);
				value = num3;
			}
			while (value != 0L);
			return num;
		}

		private void WriteIntegerValue(int value)
		{
			if (value >= 0 && value <= 9)
			{
				_writer.Write((char)(48 + value));
				return;
			}
			bool flag = value < 0;
			WriteIntegerValue((uint)(flag ? (-value) : value), flag);
		}

		private void WriteIntegerValue(uint value, bool negative)
		{
			if (!negative && value <= 9)
			{
				_writer.Write((char)(48 + value));
				return;
			}
			int count = WriteNumberToBuffer(value, negative);
			_writer.Write(_writeBuffer, 0, count);
		}

		private int WriteNumberToBuffer(uint value, bool negative)
		{
			EnsureWriteBuffer();
			int num = MathUtils.IntLength(value);
			if (negative)
			{
				num++;
				_writeBuffer[0] = '-';
			}
			int num2 = num;
			do
			{
				uint num3 = value / 10u;
				uint num4 = value - num3 * 10;
				_writeBuffer[--num2] = (char)(48 + num4);
				value = num3;
			}
			while (value != 0);
			return num;
		}
	}
}
