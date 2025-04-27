using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class KeyDescriptor
	{
		private SecurityKeyIdentifier _ski;

		private KeyType _use;

		private Collection<EncryptionMethod> _encryptionMethods = new Collection<EncryptionMethod>();

		public SecurityKeyIdentifier KeyInfo
		{
			get
			{
				return _ski;
			}
			set
			{
				_ski = value;
			}
		}

		public KeyType Use
		{
			get
			{
				return _use;
			}
			set
			{
				_use = value;
			}
		}

		public ICollection<EncryptionMethod> EncryptionMethods => _encryptionMethods;

		public KeyDescriptor()
			: this(null)
		{
		}

		public KeyDescriptor(SecurityKeyIdentifier ski)
		{
			_ski = ski;
		}
	}
}
