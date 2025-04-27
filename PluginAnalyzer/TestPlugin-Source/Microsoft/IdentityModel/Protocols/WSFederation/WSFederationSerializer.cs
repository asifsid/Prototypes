using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSTrust;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public class WSFederationSerializer
	{
		private WSTrustRequestSerializer _requestSerializer;

		private WSTrustResponseSerializer _responseSerializer;

		public WSFederationSerializer()
			: this(new WSTrustFeb2005RequestSerializer(), new WSTrustFeb2005ResponseSerializer())
		{
		}

		public WSFederationSerializer(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (reader.NamespaceURI == "http://docs.oasis-open.org/ws-sx/ws-trust/200512")
			{
				_requestSerializer = new WSTrust13RequestSerializer();
				_responseSerializer = new WSTrust13ResponseSerializer();
				return;
			}
			if (reader.NamespaceURI == "http://schemas.xmlsoap.org/ws/2005/02/trust")
			{
				_requestSerializer = new WSTrustFeb2005RequestSerializer();
				_responseSerializer = new WSTrustFeb2005ResponseSerializer();
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID5004", reader.NamespaceURI));
		}

		public WSFederationSerializer(WSTrustRequestSerializer requestSerializer, WSTrustResponseSerializer responseSerializer)
		{
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			_requestSerializer = requestSerializer;
			_responseSerializer = responseSerializer;
		}

		public virtual RequestSecurityToken CreateRequest(WSFederationMessage message, WSTrustSerializationContext context)
		{
			if (message == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("message");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			SignInRequestMessage signInRequestMessage = message as SignInRequestMessage;
			if (signInRequestMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID3005"), "message"));
			}
			string text = signInRequestMessage.Request;
			bool flag = !string.IsNullOrEmpty(signInRequestMessage.RequestPtr);
			if (!string.IsNullOrEmpty(text))
			{
				if (flag && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID3211", "request", "wreq", "wreqptr"));
				}
			}
			else if (flag)
			{
				text = GetReferencedRequest(signInRequestMessage.RequestPtr);
			}
			RequestSecurityToken requestSecurityToken;
			if (!string.IsNullOrEmpty(text))
			{
				using XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(text), XmlDictionaryReaderQuotas.Max);
				try
				{
					requestSecurityToken = _requestSerializer.ReadXml(reader, context);
				}
				catch (XmlException inner)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3273"), inner));
				}
			}
			else
			{
				requestSecurityToken = new RequestSecurityToken();
			}
			if (string.IsNullOrEmpty(requestSecurityToken.RequestType))
			{
				requestSecurityToken.RequestType = "http://schemas.microsoft.com/idfx/requesttype/issue";
			}
			if (!string.IsNullOrEmpty(signInRequestMessage.AuthenticationType) && string.IsNullOrEmpty(requestSecurityToken.AuthenticationType))
			{
				if (!UriUtil.CanCreateValidUri(signInRequestMessage.AuthenticationType, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3143", "wauth", signInRequestMessage.AuthenticationType)));
				}
				requestSecurityToken.AuthenticationType = signInRequestMessage.AuthenticationType;
			}
			if (string.IsNullOrEmpty(requestSecurityToken.KeyType))
			{
				requestSecurityToken.KeyType = "http://schemas.microsoft.com/idfx/keytype/bearer";
			}
			string context2 = signInRequestMessage.Context;
			if (!string.IsNullOrEmpty(context2) && string.IsNullOrEmpty(requestSecurityToken.Context))
			{
				requestSecurityToken.Context = context2;
			}
			string realm = signInRequestMessage.Realm;
			if (!string.IsNullOrEmpty(realm) && requestSecurityToken.AppliesTo == null)
			{
				if (!UriUtil.CanCreateValidUri(signInRequestMessage.Realm, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3143", "wtrealm", signInRequestMessage.Realm)));
				}
				requestSecurityToken.AppliesTo = new EndpointAddress(realm);
			}
			requestSecurityToken.ReplyTo = string.Empty;
			if (!string.IsNullOrEmpty(signInRequestMessage.Reply))
			{
				requestSecurityToken.ReplyTo = signInRequestMessage.Reply;
			}
			return requestSecurityToken;
		}

		public virtual RequestSecurityTokenResponse CreateResponse(WSFederationMessage message, WSTrustSerializationContext context)
		{
			if (message == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("message");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			SignInResponseMessage signInResponseMessage = message as SignInResponseMessage;
			if (signInResponseMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID3005"), "message"));
			}
			string text = signInResponseMessage.Result;
			bool flag = !string.IsNullOrEmpty(signInResponseMessage.ResultPtr);
			if (!string.IsNullOrEmpty(text))
			{
				if (flag && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID3211", "result", "wresult", "wresultptr"));
				}
			}
			else
			{
				if (!flag)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID3019")));
				}
				text = GetReferencedResult(signInResponseMessage.ResultPtr);
			}
			using XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(text), XmlDictionaryReaderQuotas.Max);
			xmlDictionaryReader.MoveToContent();
			return _responseSerializer.ReadXml(xmlDictionaryReader, context);
		}

		public virtual string GetReferencedRequest(string wreqptr)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3210", "wreqptr")));
		}

		public virtual string GetReferencedResult(string wresultptr)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3210", "wresultptr")));
		}

		public virtual string GetRequestAsString(RequestSecurityToken request, WSTrustSerializationContext context)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			StringBuilder stringBuilder = new StringBuilder();
			using (StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
				_requestSerializer.WriteXml(request, xmlTextWriter, context);
				xmlTextWriter.Flush();
			}
			return stringBuilder.ToString();
		}

		public virtual string GetResponseAsString(RequestSecurityTokenResponse response, WSTrustSerializationContext context)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			StringBuilder stringBuilder = new StringBuilder();
			using (StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
				_responseSerializer.WriteXml(response, xmlTextWriter, context);
				xmlTextWriter.Flush();
			}
			return stringBuilder.ToString();
		}

		public virtual bool CanReadRequest(string trustMessage)
		{
			if (string.IsNullOrEmpty(trustMessage))
			{
				return false;
			}
			try
			{
				using XmlReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(trustMessage), XmlDictionaryReaderQuotas.Max);
				return _requestSerializer.CanRead(reader);
			}
			catch (XmlException)
			{
				return false;
			}
		}

		public virtual bool CanReadResponse(string trustMessage)
		{
			if (string.IsNullOrEmpty(trustMessage))
			{
				return false;
			}
			try
			{
				using XmlReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(trustMessage), XmlDictionaryReaderQuotas.Max);
				return _responseSerializer.CanRead(reader);
			}
			catch (XmlException)
			{
				return false;
			}
		}
	}
}
