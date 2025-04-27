using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class AuthenticationContext
	{
		private Collection<string> _authorities;

		private string _contextClass;

		private string _contextDeclaration;

		public Collection<string> Authorities => _authorities;

		public string ContextClass
		{
			get
			{
				return _contextClass;
			}
			set
			{
				_contextClass = value;
			}
		}

		public string ContextDeclaration
		{
			get
			{
				return _contextDeclaration;
			}
			set
			{
				_contextDeclaration = value;
			}
		}

		public AuthenticationContext()
		{
			_authorities = new Collection<string>();
		}
	}
}
