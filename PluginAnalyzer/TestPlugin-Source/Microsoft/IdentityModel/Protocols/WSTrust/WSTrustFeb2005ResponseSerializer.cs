using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustFeb2005ResponseSerializer : WSTrustResponseSerializer
	{
		public override RequestSecurityTokenResponse ReadXml(XmlReader reader, WSTrustSerializationContext context)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			return WSTrustSerializationHelper.CreateResponse(reader, context, this, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void ReadXmlElement(XmlReader reader, RequestSecurityTokenResponse rstr, WSTrustSerializationContext context)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.ReadRSTRXml(reader, rstr, context, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void WriteKnownResponseElement(RequestSecurityTokenResponse rstr, XmlWriter writer, WSTrustSerializationContext context)
		{
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.WriteKnownResponseElement(rstr, writer, context, this, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void WriteXml(RequestSecurityTokenResponse response, XmlWriter writer, WSTrustSerializationContext context)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.WriteResponse(response, writer, context, this, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void WriteXmlElement(XmlWriter writer, string elementName, object elementValue, RequestSecurityTokenResponse rstr, WSTrustSerializationContext context)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (string.IsNullOrEmpty(elementName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("elementName");
			}
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.WriteRSTRXml(writer, elementName, elementValue, context, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override bool CanRead(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}
	}
}
