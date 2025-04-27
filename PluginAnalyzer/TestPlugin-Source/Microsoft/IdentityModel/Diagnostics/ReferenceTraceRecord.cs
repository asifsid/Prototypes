using System;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class ReferenceTraceRecord : TraceRecord
	{
		internal new const string ElementName = "ReferenceTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/ReferenceTraceRecord";

		private bool _areEqual;

		private byte[] _computedDigest;

		private byte[] _referenceDigest;

		private string _uri;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/ReferenceTraceRecord";

		public ReferenceTraceRecord(bool areEqual, byte[] computedDigest, byte[] referenceDigest, string uri)
		{
			_areEqual = areEqual;
			_computedDigest = computedDigest;
			_referenceDigest = referenceDigest;
			_uri = uri;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("ReferenceTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteElementString("Reference", _uri);
			writer.WriteElementString("Equal", _areEqual.ToString());
			writer.WriteElementString("ComputedDigestBase64", Convert.ToBase64String(_computedDigest));
			writer.WriteElementString("ReferenceDigestBase64", Convert.ToBase64String(_referenceDigest));
			writer.WriteEndElement();
		}
	}
}
