using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public class EmptySecurityKeyIdentifierClause : SecurityKeyIdentifierClause
	{
		private object _context;

		public object Context => _context;

		public EmptySecurityKeyIdentifierClause()
			: this(null)
		{
		}

		public EmptySecurityKeyIdentifierClause(object context)
			: base(typeof(EmptySecurityKeyIdentifierClause).ToString())
		{
			_context = context;
		}
	}
}
