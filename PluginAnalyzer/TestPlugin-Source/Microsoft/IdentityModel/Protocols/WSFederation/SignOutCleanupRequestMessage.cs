using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public class SignOutCleanupRequestMessage : WSFederationMessage
	{
		public string Reply
		{
			get
			{
				return GetParameter("wreply");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wreply");
				}
				else
				{
					SetUriParameter("wreply", value);
				}
			}
		}

		public SignOutCleanupRequestMessage(Uri baseUrl)
			: base(baseUrl, "wsignoutcleanup1.0")
		{
		}

		public SignOutCleanupRequestMessage(Uri baseUrl, string reply)
			: base(baseUrl, "wsignoutcleanup1.0")
		{
			SetUriParameter("wreply", reply);
		}

		protected override void Validate()
		{
			base.Validate();
			string parameter = GetParameter("wa");
			if (string.IsNullOrEmpty(parameter) || !parameter.Equals("wsignoutcleanup1.0"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3000", parameter)));
			}
		}

		public override void Write(TextWriter writer)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			Validate();
			writer.Write(WriteQueryString());
		}
	}
}
