using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustRequestBodyWriter : BodyWriter
	{
		private WSTrustSerializationContext _serializationContext;

		private RequestSecurityToken _requestSecurityToken;

		private WSTrustRequestSerializer _serializer;

		public WSTrustRequestBodyWriter(RequestSecurityToken requestSecurityToken, WSTrustRequestSerializer serializer, WSTrustSerializationContext serializationContext)
			: base(isBuffered: true)
		{
			if (requestSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSecurityToken");
			}
			if (serializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serializer");
			}
			if (serializationContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serializationContext");
			}
			_requestSecurityToken = requestSecurityToken;
			_serializer = serializer;
			_serializationContext = serializationContext;
		}

		protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
		{
			_serializer.WriteXml(_requestSecurityToken, writer, _serializationContext);
		}
	}
}
