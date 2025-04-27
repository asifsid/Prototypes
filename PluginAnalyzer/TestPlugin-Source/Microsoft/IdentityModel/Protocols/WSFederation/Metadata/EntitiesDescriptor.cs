using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class EntitiesDescriptor : MetadataBase
	{
		private Collection<EntitiesDescriptor> _entityGroupCollection = new Collection<EntitiesDescriptor>();

		private Collection<EntityDescriptor> _entityCollection = new Collection<EntityDescriptor>();

		private string _name;

		public ICollection<EntityDescriptor> ChildEntities => _entityCollection;

		public ICollection<EntitiesDescriptor> ChildEntityGroups => _entityGroupCollection;

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public EntitiesDescriptor()
			: this(new Collection<EntityDescriptor>(), new Collection<EntitiesDescriptor>())
		{
		}

		public EntitiesDescriptor(Collection<EntitiesDescriptor> entityGroupList)
		{
			_entityGroupCollection = entityGroupList;
		}

		public EntitiesDescriptor(Collection<EntityDescriptor> entityList)
		{
			_entityCollection = entityList;
		}

		public EntitiesDescriptor(Collection<EntityDescriptor> entityList, Collection<EntitiesDescriptor> entityGroupList)
		{
			_entityCollection = entityList;
			_entityGroupCollection = entityGroupList;
		}
	}
}
