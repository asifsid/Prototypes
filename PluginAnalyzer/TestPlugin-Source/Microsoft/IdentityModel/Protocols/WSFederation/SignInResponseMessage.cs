using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.WSTrust;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public class SignInResponseMessage : WSFederationMessage
	{
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

		public SignInResponseMessage(Uri baseUrl, string result)
			: base(baseUrl, "wsignin1.0")
		{
			if (string.IsNullOrEmpty(result))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "result"));
			}
			SetParameter("wresult", result);
		}

		public SignInResponseMessage(Uri baseUrl, Uri resultPtr)
			: base(baseUrl, "wsignin1.0")
		{
			if (resultPtr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("resultPtr");
			}
			SetParameter("wresultptr", resultPtr.AbsoluteUri);
		}

		public SignInResponseMessage(Uri baseUrl, RequestSecurityTokenResponse response, WSFederationSerializer federationSerializer, WSTrustSerializationContext context)
			: base(baseUrl, "wsignin1.0")
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (federationSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("federationSerializer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			SetParameter("wresult", federationSerializer.GetResponseAsString(response, context));
			base.Context = response.Context;
		}

		protected override void Validate()
		{
			base.Validate();
			string parameter = GetParameter("wa");
			if (parameter != "wsignin1.0")
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3000", parameter)));
			}
			bool flag = !string.IsNullOrEmpty(GetParameter("wresult"));
			bool flag2 = !string.IsNullOrEmpty(GetParameter("wresultptr"));
			if (flag && flag2)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3016")));
			}
			if (!flag && !flag2)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3001")));
			}
		}

		public override void Write(TextWriter writer)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			Validate();
			writer.Write(WriteFormPost());
		}
	}
}
