using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public abstract class WSTrustRequestSerializer
	{
		public abstract RequestSecurityToken ReadXml(XmlReader reader, WSTrustSerializationContext context);

		public abstract void ReadXmlElement(XmlReader reader, RequestSecurityToken rst, WSTrustSerializationContext context);

		public abstract void WriteKnownRequestElement(RequestSecurityToken rst, XmlWriter writer, WSTrustSerializationContext context);

		protected virtual void ReadCustomElement(XmlReader reader, WSTrustSerializationContext context)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID2072", reader.LocalName)));
		}

		public abstract void WriteXml(RequestSecurityToken request, XmlWriter writer, WSTrustSerializationContext context);

		public abstract void WriteXmlElement(XmlWriter writer, string elementName, object elementValue, RequestSecurityToken rst, WSTrustSerializationContext context);

		public virtual RequestSecurityToken CreateRequestSecurityToken()
		{
			return new RequestSecurityToken();
		}

		public virtual void Validate(RequestSecurityToken rst)
		{
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if ((StringComparer.Ordinal.Equals(rst.RequestType, "http://schemas.microsoft.com/idfx/requesttype/issue") || rst.RequestType == null) && StringComparer.Ordinal.Equals(rst.KeyType, "http://schemas.microsoft.com/idfx/keytype/asymmetric") && (rst.UseKey == null || (rst.UseKey.SecurityKeyIdentifier == null && rst.UseKey.Token == null)))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3091")));
			}
		}

		public abstract bool CanRead(XmlReader reader);
	}
}
