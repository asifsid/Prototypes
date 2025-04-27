using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustResponseBodyWriter : BodyWriter
	{
		private WSTrustResponseSerializer _serializer;

		private RequestSecurityTokenResponse _rstr;

		private WSTrustSerializationContext _context;

		public WSTrustResponseBodyWriter(RequestSecurityTokenResponse rstr, WSTrustResponseSerializer serializer, WSTrustSerializationContext context)
			: base(isBuffered: true)
		{
			if (serializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serializer");
			}
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			_serializer = serializer;
			_rstr = rstr;
			_context = context;
		}

		protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
		{
			_serializer.WriteXml(_rstr, writer, _context);
		}
	}
}
