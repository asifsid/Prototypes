using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class IndexedProtocolEndpoint : ProtocolEndpoint
	{
		private int _index;

		private bool? _isDefault = null;

		public int Index
		{
			get
			{
				return _index;
			}
			set
			{
				_index = value;
			}
		}

		public bool? IsDefault
		{
			get
			{
				return _isDefault;
			}
			set
			{
				_isDefault = value;
			}
		}

		public IndexedProtocolEndpoint()
		{
		}

		public IndexedProtocolEndpoint(int index, Uri binding, Uri location)
			: base(binding, location)
		{
			_index = index;
		}
	}
}
