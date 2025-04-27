using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustFeb2005RequestSerializer : WSTrustRequestSerializer
	{
		public override RequestSecurityToken ReadXml(XmlReader reader, WSTrustSerializationContext context)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			return WSTrustSerializationHelper.CreateRequest(reader, context, this, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void ReadXmlElement(XmlReader reader, RequestSecurityToken rst, WSTrustSerializationContext context)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.ReadRSTXml(reader, rst, context, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void WriteKnownRequestElement(RequestSecurityToken rst, XmlWriter writer, WSTrustSerializationContext context)
		{
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.WriteKnownRequestElement(rst, writer, context, this, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void WriteXml(RequestSecurityToken request, XmlWriter writer, WSTrustSerializationContext context)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.WriteRequest(request, writer, context, this, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override void WriteXmlElement(XmlWriter writer, string elementName, object elementValue, RequestSecurityToken rst, WSTrustSerializationContext context)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (string.IsNullOrEmpty(elementName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("elementName");
			}
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			WSTrustSerializationHelper.WriteRSTXml(writer, elementName, elementValue, context, WSTrustConstantsAdapter.TrustFeb2005);
		}

		public override bool CanRead(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}
	}
}
