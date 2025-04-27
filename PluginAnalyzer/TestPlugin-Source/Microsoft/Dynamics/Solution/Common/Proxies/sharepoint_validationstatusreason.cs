using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public enum sharepoint_validationstatusreason
	{
		[EnumMember]
		ThisrecordsURLhasnotbeenvalidated = 1,
		[EnumMember]
		ThisrecordsURLisvalid,
		[EnumMember]
		ThisrecordsURLisnotvalid,
		[EnumMember]
		TheURLschemesofMicrosoftDynamics365andSharePointaredifferent,
		[EnumMember]
		TheURLcouldnotbeaccessedbecauseofInternetExplorersecuritysettings,
		[EnumMember]
		Authenticationfailure,
		[EnumMember]
		Invalidcertificates
	}
}
