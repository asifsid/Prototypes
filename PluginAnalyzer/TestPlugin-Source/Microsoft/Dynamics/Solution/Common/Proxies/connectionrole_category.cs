using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public enum connectionrole_category
	{
		[EnumMember]
		Business = 1,
		[EnumMember]
		Family = 2,
		[EnumMember]
		Social = 3,
		[EnumMember]
		Sales = 4,
		[EnumMember]
		Other = 5,
		[EnumMember]
		Stakeholder = 1000,
		[EnumMember]
		SalesTeam = 1001,
		[EnumMember]
		Service = 1002
	}
}
