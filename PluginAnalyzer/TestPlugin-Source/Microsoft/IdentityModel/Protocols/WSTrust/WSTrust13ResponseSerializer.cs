using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrust13ResponseSerializer : WSTrustResponseSerializer
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
			bool flag = false;
			if (reader.IsStartElement("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				reader.ReadStartElement("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
				flag = true;
			}
			RequestSecurityTokenResponse requestSecurityTokenResponse = WSTrustSerializationHelper.CreateResponse(reader, context, this, WSTrustConstantsAdapter.Trust13);
			requestSecurityTokenResponse.IsFinal = flag;
			if (flag)
			{
				reader.ReadEndElement();
			}
			return requestSecurityTokenResponse;
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
			if (reader.IsStartElement("KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				rstr.KeyWrapAlgorithm = reader.ReadElementContentAsString();
			}
			else
			{
				WSTrustSerializationHelper.ReadRSTRXml(reader, rstr, context, WSTrustConstantsAdapter.Trust13);
			}
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
			WSTrustSerializationHelper.WriteKnownResponseElement(rstr, writer, context, this, WSTrustConstantsAdapter.Trust13);
			if (!string.IsNullOrEmpty(rstr.KeyWrapAlgorithm))
			{
				WriteXmlElement(writer, "KeyWrapAlgorithm", rstr.KeyWrapAlgorithm, rstr, context);
			}
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
			if (response.IsFinal)
			{
				writer.WriteStartElement("trust", "RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			}
			WSTrustSerializationHelper.WriteResponse(response, writer, context, this, WSTrustConstantsAdapter.Trust13);
			if (response.IsFinal)
			{
				writer.WriteEndElement();
			}
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
			if (StringComparer.Ordinal.Equals(elementName, "KeyWrapAlgorithm"))
			{
				writer.WriteElementString("trust", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (string)elementValue);
			}
			else
			{
				WSTrustSerializationHelper.WriteRSTRXml(writer, elementName, elementValue, context, WSTrustConstantsAdapter.Trust13);
			}
		}

		public override bool CanRead(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				return reader.IsStartElement("RequestSecurityTokenResponse", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			}
			return true;
		}
	}
}
