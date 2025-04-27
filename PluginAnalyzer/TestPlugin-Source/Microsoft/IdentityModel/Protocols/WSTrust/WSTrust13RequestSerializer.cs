using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrust13RequestSerializer : WSTrustRequestSerializer
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
			return WSTrustSerializationHelper.CreateRequest(reader, context, this, WSTrustConstantsAdapter.Trust13);
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
			if (reader.IsStartElement("SecondaryParameters", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				rst.SecondaryParameters = ReadSecondaryParameters(reader, context);
			}
			else if (reader.IsStartElement("KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				rst.KeyWrapAlgorithm = reader.ReadElementContentAsString();
				if (!UriUtil.CanCreateValidUri(rst.KeyWrapAlgorithm, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", rst.KeyWrapAlgorithm)));
				}
			}
			else if (reader.IsStartElement("ValidateTarget", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				if (!reader.IsEmptyElement)
				{
					rst.ValidateTarget = new SecurityTokenElement(WSTrustSerializationHelper.ReadInnerXml(reader), context.SecurityTokenHandlers);
				}
				if (rst.ValidateTarget == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3221")));
				}
			}
			else
			{
				WSTrustSerializationHelper.ReadRSTXml(reader, rst, context, WSTrustConstantsAdapter.Trust13);
			}
		}

		protected virtual RequestSecurityToken ReadSecondaryParameters(XmlReader reader, WSTrustSerializationContext context)
		{
			RequestSecurityToken requestSecurityToken = CreateRequestSecurityToken();
			if (reader.IsEmptyElement)
			{
				reader.Read();
				reader.MoveToContent();
				return requestSecurityToken;
			}
			reader.ReadStartElement();
			while (reader.IsStartElement())
			{
				if (reader.IsStartElement("KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
				{
					requestSecurityToken.KeyWrapAlgorithm = reader.ReadElementContentAsString();
					if (!UriUtil.CanCreateValidUri(requestSecurityToken.KeyWrapAlgorithm, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", requestSecurityToken.KeyWrapAlgorithm)));
					}
				}
				else
				{
					if (reader.IsStartElement("SecondaryParameters", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3130")));
					}
					WSTrustSerializationHelper.ReadRSTXml(reader, requestSecurityToken, context, WSTrustConstantsAdapter.GetConstantsAdapter(reader.NamespaceURI) ?? WSTrustConstantsAdapter.TrustFeb2005);
				}
			}
			reader.ReadEndElement();
			return requestSecurityToken;
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
			WSTrustSerializationHelper.WriteKnownRequestElement(rst, writer, context, this, WSTrustConstantsAdapter.Trust13);
			if (!string.IsNullOrEmpty(rst.KeyWrapAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri(rst.KeyWrapAlgorithm, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", rst.KeyWrapAlgorithm)));
				}
				WriteXmlElement(writer, "KeyWrapAlgorithm", rst.KeyWrapAlgorithm, rst, context);
			}
			if (rst.SecondaryParameters != null)
			{
				WriteXmlElement(writer, "SecondaryParameters", rst.SecondaryParameters, rst, context);
			}
			if (rst.ValidateTarget != null)
			{
				WriteXmlElement(writer, "ValidateTarget", rst.ValidateTarget, rst, context);
			}
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
			WSTrustSerializationHelper.WriteRequest(request, writer, context, this, WSTrustConstantsAdapter.Trust13);
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
			if (StringComparer.Ordinal.Equals(elementName, "SecondaryParameters"))
			{
				RequestSecurityToken requestSecurityToken = elementValue as RequestSecurityToken;
				if (requestSecurityToken == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2064", "SecondaryParameters")));
				}
				if (requestSecurityToken.SecondaryParameters != null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2055")));
				}
				writer.WriteStartElement("trust", "SecondaryParameters", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
				WriteKnownRequestElement(requestSecurityToken, writer, context);
				foreach (KeyValuePair<string, object> property in requestSecurityToken.Properties)
				{
					WriteXmlElement(writer, property.Key, property.Value, rst, context);
				}
				writer.WriteEndElement();
			}
			else if (StringComparer.Ordinal.Equals(elementName, "KeyWrapAlgorithm"))
			{
				writer.WriteElementString("trust", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, "ValidateTarget"))
			{
				SecurityTokenElement securityTokenElement = elementValue as SecurityTokenElement;
				if (securityTokenElement == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", "ValidateTarget", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", typeof(SecurityTokenElement), elementValue));
				}
				writer.WriteStartElement("trust", "ValidateTarget", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
				if (securityTokenElement.SecurityTokenXml != null)
				{
					securityTokenElement.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
				}
				writer.WriteEndElement();
			}
			else
			{
				WSTrustSerializationHelper.WriteRSTXml(writer, elementName, elementValue, context, WSTrustConstantsAdapter.Trust13);
			}
		}

		public override bool CanRead(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}
	}
}
