using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public enum product_productstructure
	{
		[EnumMember]
		Product = 1,
		[EnumMember]
		ProductFamily,
		[EnumMember]
		ProductBundle
	}
}
