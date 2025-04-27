using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class RequestedSecurityToken
	{
		private XmlElement _tokenAsXml;

		private SecurityToken _requestedToken;

		public virtual XmlElement SecurityTokenXml => _tokenAsXml;

		public SecurityToken SecurityToken => _requestedToken;

		public RequestedSecurityToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			_requestedToken = token;
		}

		public RequestedSecurityToken(XmlElement tokenAsXml)
		{
			if (tokenAsXml == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenAsXml");
			}
			_tokenAsXml = tokenAsXml;
		}
	}
}
