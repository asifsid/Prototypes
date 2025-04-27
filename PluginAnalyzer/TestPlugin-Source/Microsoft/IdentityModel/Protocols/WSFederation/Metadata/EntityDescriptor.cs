using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class EntityDescriptor : MetadataBase
	{
		private Collection<ContactPerson> _contacts = new Collection<ContactPerson>();

		private EntityId _entityId;

		private string _federationId;

		private Organization _organization;

		private Collection<RoleDescriptor> _roleDescriptors = new Collection<RoleDescriptor>();

		public ICollection<ContactPerson> Contacts => _contacts;

		public EntityId EntityId
		{
			get
			{
				return _entityId;
			}
			set
			{
				_entityId = value;
			}
		}

		public string FederationId
		{
			get
			{
				return _federationId;
			}
			set
			{
				_federationId = value;
			}
		}

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

		public ICollection<RoleDescriptor> RoleDescriptors => _roleDescriptors;

		public EntityDescriptor()
			: this(null)
		{
		}

		public EntityDescriptor(EntityId entityId)
		{
			_entityId = entityId;
		}
	}
}
