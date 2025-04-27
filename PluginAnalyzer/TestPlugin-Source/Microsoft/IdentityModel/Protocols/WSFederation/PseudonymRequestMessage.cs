using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public class PseudonymRequestMessage : WSFederationMessage
	{
		public string Pseudonym
		{
			get
			{
				return GetParameter("wpseudo");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wpseudo");
				}
				else
				{
					SetParameter("wpseudo", value);
				}
			}
		}

		public string PseudonymPtr
		{
			get
			{
				return GetParameter("wpseudoptr");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wpseudoptr");
				}
				else
				{
					SetUriParameter("wpseudoptr", value);
				}
			}
		}

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

		public string Result
		{
			get
			{
				return GetParameter("wresult");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wresult");
				}
				else
				{
					SetParameter("wresult", value);
				}
			}
		}

		public string ResultPtr
		{
			get
			{
				return GetParameter("wresultptr");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wresultptr");
				}
				else
				{
					SetUriParameter("wresultptr", value);
				}
			}
		}

		public PseudonymRequestMessage(Uri baseUrl)
			: base(baseUrl, "wpseudo1.0")
		{
		}

		protected override void Validate()
		{
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
