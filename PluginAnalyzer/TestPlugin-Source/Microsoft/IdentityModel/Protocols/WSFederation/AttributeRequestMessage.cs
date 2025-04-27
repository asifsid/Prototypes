using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public class AttributeRequestMessage : WSFederationMessage
	{
		public string Attribute
		{
			get
			{
				return GetParameter("wattr");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wattr");
				}
				else
				{
					SetParameter("wattr", value);
				}
			}
		}

		public string AttributePtr
		{
			get
			{
				return GetParameter("wattrptr");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wattrptr");
				}
				else
				{
					SetUriParameter("wattrptr", value);
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

		public AttributeRequestMessage(Uri baseUrl)
			: base(baseUrl, "wattr1.0")
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
