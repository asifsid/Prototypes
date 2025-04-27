using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class EntityId
	{
		private const int MaximumLength = 1024;

		private string _id;

		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != null && value.ToString().Length > 1024)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID3199"));
				}
				_id = value;
			}
		}

		public EntityId()
			: this(null)
		{
		}

		public EntityId(string id)
		{
			_id = id;
		}
	}
}
