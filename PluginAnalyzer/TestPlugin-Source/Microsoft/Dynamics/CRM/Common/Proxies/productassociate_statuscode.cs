using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.CRM.Common.Proxies
{
	[DataContract]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public enum productassociate_statuscode
	{
		[EnumMember]
		Active = 1,
		[EnumMember]
		Inactive = 2,
		[EnumMember]
		Draft = 0,
		[EnumMember]
		DraftActive = 3
	}
}
