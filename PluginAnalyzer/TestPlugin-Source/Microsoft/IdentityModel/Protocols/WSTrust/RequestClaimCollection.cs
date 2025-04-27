using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class RequestClaimCollection : Collection<RequestClaim>
	{
		private string _dialect = "http://schemas.xmlsoap.org/ws/2005/05/identity";

		public string Dialect
		{
			get
			{
				return _dialect;
			}
			set
			{
				_dialect = value;
			}
		}
	}
}
