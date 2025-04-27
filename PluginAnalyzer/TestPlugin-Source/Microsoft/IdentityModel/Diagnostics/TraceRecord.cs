using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal abstract class TraceRecord
	{
		protected const string EventIdBase = "http://schemas.microsoft.com/2009/06/IdentityModel/";

		protected const string ElementName = "TraceRecord";

		internal const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/EmptyTraceRecord";

		public virtual string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/EmptyTraceRecord";

		public abstract void WriteTo(XmlWriter writer);
	}
}
