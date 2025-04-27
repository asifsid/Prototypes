using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public enum product_statuscode
	{
		[EnumMember]
		Active = 1,
		[EnumMember]
		Retired = 2,
		[EnumMember]
		Draft = 0,
		[EnumMember]
		UnderRevision = 3
	}
}
