using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Id
	{
		private string _value;

		public string Value => _value;

		public Saml2Id()
			: this(UniqueId.CreateRandomId())
		{
		}

		public Saml2Id(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			try
			{
				_value = XmlConvert.VerifyNCName(value);
			}
			catch (XmlException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID4128"), "value", innerException));
			}
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			Saml2Id saml2Id = obj as Saml2Id;
			if (saml2Id != null)
			{
				return StringComparer.Ordinal.Equals(_value, saml2Id.Value);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public override string ToString()
		{
			return _value;
		}
	}
}
