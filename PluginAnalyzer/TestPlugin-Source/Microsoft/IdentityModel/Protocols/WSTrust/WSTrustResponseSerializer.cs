using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public abstract class WSTrustResponseSerializer
	{
		public abstract RequestSecurityTokenResponse ReadXml(XmlReader reader, WSTrustSerializationContext context);

		public abstract void ReadXmlElement(XmlReader reader, RequestSecurityTokenResponse rstr, WSTrustSerializationContext context);

		public abstract void WriteKnownResponseElement(RequestSecurityTokenResponse rstr, XmlWriter writer, WSTrustSerializationContext context);

		public abstract void WriteXml(RequestSecurityTokenResponse response, XmlWriter writer, WSTrustSerializationContext context);

		public abstract void WriteXmlElement(XmlWriter writer, string elementName, object elementValue, RequestSecurityTokenResponse rstr, WSTrustSerializationContext context);

		public virtual RequestSecurityTokenResponse CreateInstance()
		{
			return new RequestSecurityTokenResponse();
		}

		public virtual void Validate(RequestSecurityTokenResponse rstr)
		{
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
		}

		public abstract bool CanRead(XmlReader reader);
	}
}
