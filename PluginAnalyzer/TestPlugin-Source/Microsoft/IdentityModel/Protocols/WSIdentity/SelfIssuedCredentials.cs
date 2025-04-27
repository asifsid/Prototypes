using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class SelfIssuedCredentials : IUserCredential
	{
		private string _ppid;

		public string PPID => _ppid;

		public UserCredentialType CredentialType => UserCredentialType.SelfIssuedCredential;

		public SelfIssuedCredentials(string ppid)
		{
			if (string.IsNullOrEmpty(ppid))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "ppid"));
			}
			_ppid = ppid;
		}
	}
}
