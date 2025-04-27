using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class CardImage
	{
		private byte[] _image;

		private string _mimeType;

		public Bitmap BitmapImage
		{
			get
			{
				using MemoryStream stream = new MemoryStream(_image);
				Image original = Image.FromStream(stream);
				return new Bitmap(original);
			}
		}

		public string MimeType => _mimeType;

		public static CardImage CreateFromByteArray(byte[] image)
		{
			if (image == null || image.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("image", SR.GetString("ID0015"));
			}
			CardImage cardImage = new CardImage();
			using MemoryStream stream = new MemoryStream(image);
			using Image bitmapImage = Image.FromStream(stream);
			cardImage.InitializeFromImage(bitmapImage);
			return cardImage;
		}

		public static CardImage CreateFromImage(Image image)
		{
			if (image == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("image");
			}
			CardImage cardImage = new CardImage();
			cardImage.InitializeFromImage(image);
			return cardImage;
		}

		private CardImage()
		{
		}

		public CardImage(byte[] image, string mimeType)
		{
			if (string.IsNullOrEmpty(mimeType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("mimeType");
			}
			if (image == null || image.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("image", SR.GetString("ID0015"));
			}
			if (!IsValidMimeType(mimeType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("mimeType", SR.GetString("ID2042"));
			}
			_mimeType = mimeType;
			_image = image;
		}

		public CardImage(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("fileName");
			}
			try
			{
				Image bitmapImage = Image.FromFile(fileName);
				InitializeFromImage(bitmapImage);
			}
			catch (Exception ex)
			{
				if (ex is FileNotFoundException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("fileName", SR.GetString("ID2017", fileName));
				}
				if (ex is OutOfMemoryException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("fileName", SR.GetString("ID2018", fileName));
				}
				throw;
			}
		}

		private static string GetMimeType(ImageFormat format)
		{
			if (format.Equals(ImageFormat.Bmp))
			{
				return "image/bmp";
			}
			if (format.Equals(ImageFormat.Gif))
			{
				return "image/gif";
			}
			if (format.Equals(ImageFormat.Jpeg))
			{
				return "image/jpeg";
			}
			if (format.Equals(ImageFormat.Png))
			{
				return "image/png";
			}
			if (format.Equals(ImageFormat.Tiff))
			{
				return "image/tiff";
			}
			return null;
		}

		public byte[] GetImage()
		{
			return _image;
		}

		private void InitializeFromImage(Image bitmapImage)
		{
			if (bitmapImage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("bitmapImage");
			}
			ImageFormat format = (IsSupportedImageFormat(bitmapImage.RawFormat) ? bitmapImage.RawFormat : ImageFormat.Jpeg);
			_mimeType = GetMimeType(format);
			using MemoryStream memoryStream = new MemoryStream();
			bitmapImage.Save(memoryStream, format);
			byte[] array = new byte[memoryStream.Length];
			memoryStream.Position = 0L;
			memoryStream.Read(array, 0, (int)memoryStream.Length);
			_image = array;
		}

		internal static bool IsValidMimeType(string mimeType)
		{
			if (string.IsNullOrEmpty(mimeType))
			{
				return false;
			}
			if (!StringComparer.Ordinal.Equals(mimeType, "image/bmp") && !StringComparer.Ordinal.Equals(mimeType, "image/gif") && !StringComparer.Ordinal.Equals(mimeType, "image/jpeg") && !StringComparer.Ordinal.Equals(mimeType, "image/png"))
			{
				return StringComparer.Ordinal.Equals(mimeType, "image/tiff");
			}
			return true;
		}

		private static bool IsSupportedImageFormat(ImageFormat format)
		{
			return GetMimeType(format) != null;
		}
	}
}
