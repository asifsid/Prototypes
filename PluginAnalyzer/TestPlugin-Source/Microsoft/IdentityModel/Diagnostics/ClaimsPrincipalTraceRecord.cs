using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class ClaimsPrincipalTraceRecord : TraceRecord
	{
		internal new const string ElementName = "ClaimsPrincipalTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/ClaimsPrincipalTraceRecord";

		private IClaimsPrincipal _claimsPrincipal;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/ClaimsPrincipalTraceRecord";

		public ClaimsPrincipalTraceRecord(IClaimsPrincipal claimsPrincipal)
		{
			_claimsPrincipal = claimsPrincipal;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("ClaimsPrincipalTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteStartElement("ClaimsPrincipal");
			writer.WriteAttributeString("Identity.Name", _claimsPrincipal.Identity.Name);
			foreach (IClaimsIdentity identity in _claimsPrincipal.Identities)
			{
				WriteClaimsIdentity(identity, writer);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		private void WriteClaimsIdentity(IClaimsIdentity ci, XmlWriter writer)
		{
			writer.WriteStartElement("ClaimsIdentity");
			writer.WriteAttributeString("Name", ci.Name);
			writer.WriteAttributeString("NameClaimType", ci.NameClaimType);
			writer.WriteAttributeString("RoleClaimType", ci.RoleClaimType);
			writer.WriteAttributeString("Label", ci.Label);
			if (ci.Actor != null)
			{
				writer.WriteStartElement("Actor");
				WriteClaimsIdentity(ci.Actor, writer);
				writer.WriteEndElement();
			}
			foreach (Claim claim in ci.Claims)
			{
				writer.WriteStartElement("Claim");
				writer.WriteAttributeString("Value", claim.Value);
				writer.WriteAttributeString("Type", claim.ClaimType);
				writer.WriteAttributeString("ValueType", claim.ValueType);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}
}
