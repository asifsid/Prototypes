using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Model
{
	[ComVisible(true)]
	public class AttributeMetadataWrapper
	{
		public bool? IsSecured { get; set; }

		public bool? CanBeSecuredForCreate { get; set; }

		public bool? IsValidForCreate { get; set; }

		public AttributeMetadataWrapper()
		{
			IsSecured = false;
			CanBeSecuredForCreate = false;
			IsValidForCreate = false;
		}
	}
}
