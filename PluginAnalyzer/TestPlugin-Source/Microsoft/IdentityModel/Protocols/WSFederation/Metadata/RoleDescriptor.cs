using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public abstract class RoleDescriptor
	{
		private Collection<ContactPerson> _contacts = new Collection<ContactPerson>();

		private Uri _errorUrl;

		private Collection<KeyDescriptor> _keys = new Collection<KeyDescriptor>();

		private Organization _organization;

		private Collection<Uri> _protocolsSupported = new Collection<Uri>();

		private DateTime _validUntil = DateTime.MaxValue;

		public ICollection<ContactPerson> Contacts => _contacts;

		public Uri ErrorUrl
		{
			get
			{
				return _errorUrl;
			}
			set
			{
				_errorUrl = value;
			}
		}

		public ICollection<KeyDescriptor> Keys => _keys;

		public Organization Organization
		{
			get
			{
				return _organization;
			}
			set
			{
				_organization = value;
			}
		}

		public ICollection<Uri> ProtocolsSupported => _protocolsSupported;

		public DateTime ValidUntil
		{
			get
			{
				return _validUntil;
			}
			set
			{
				_validUntil = value;
			}
		}

		protected RoleDescriptor()
			: this(new Collection<Uri>())
		{
		}

		protected RoleDescriptor(Collection<Uri> protocolsSupported)
		{
			_protocolsSupported = protocolsSupported;
		}
	}
}
