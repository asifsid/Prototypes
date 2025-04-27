using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public enum sharepoint_validationstatus
	{
		[EnumMember]
		NotValidated = 1,
		[EnumMember]
		InProgress,
		[EnumMember]
		Invalid,
		[EnumMember]
		Valid,
		[EnumMember]
		Couldnotvalidate
	}
}
