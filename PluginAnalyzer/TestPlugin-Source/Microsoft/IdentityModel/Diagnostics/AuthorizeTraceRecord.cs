using System.Web;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class AuthorizeTraceRecord : TraceRecord
	{
		internal new const string ElementName = "AuthorizeTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/AuthorizeTraceRecord";

		private IClaimsPrincipal _claimsPrincipal;

		private string _url;

		private string _action;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/AuthorizeTraceRecord";

		public AuthorizeTraceRecord(IClaimsPrincipal claimsPrincipal, HttpRequest request)
		{
			_claimsPrincipal = claimsPrincipal;
			_url = request.Url.AbsoluteUri;
			_action = request.HttpMethod;
		}

		public AuthorizeTraceRecord(IClaimsPrincipal claimsPrincipal, string url, string action)
		{
			_claimsPrincipal = claimsPrincipal;
			_url = url;
			_action = action;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("AuthorizeTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteStartElement("Authorize");
			writer.WriteElementString("Url", _url);
			writer.WriteElementString("Action", _action);
			writer.WriteStartElement("ClaimsPrincipal");
			writer.WriteAttributeString("Identity.Name", _claimsPrincipal.Identity.Name);
			foreach (IClaimsIdentity identity in _claimsPrincipal.Identities)
			{
				writer.WriteStartElement("ClaimsIdentity");
				writer.WriteAttributeString("name", identity.Name);
				foreach (Claim claim in identity.Claims)
				{
					writer.WriteStartElement("Claim");
					writer.WriteAttributeString("Value", claim.Value);
					writer.WriteAttributeString("Type", claim.ClaimType);
					writer.WriteAttributeString("ValueType", claim.ValueType);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}
}
