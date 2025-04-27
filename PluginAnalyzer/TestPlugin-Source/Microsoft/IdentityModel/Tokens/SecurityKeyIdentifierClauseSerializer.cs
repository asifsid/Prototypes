using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class SecurityKeyIdentifierClauseSerializer
	{
		public abstract bool CanReadKeyIdentifierClause(XmlReader reader);

		public abstract bool CanWriteKeyIdentifierClause(SecurityKeyIdentifierClause securityKeyIdentifierClause);

		public abstract SecurityKeyIdentifierClause ReadKeyIdentifierClause(XmlReader reader);

		public abstract void WriteKeyIdentifierClause(XmlWriter writer, SecurityKeyIdentifierClause securityKeyIdentifierClause);
	}
}
