using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public class SignInRequestMessage : WSFederationMessage
	{
		public string RequestUrl
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(128);
				using StringWriter writer = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
				Write(writer);
				return stringBuilder.ToString();
			}
		}

		public string Federation
		{
			get
			{
				return GetParameter("wfed");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wfed");
				}
				else
				{
					SetParameter("wfed", value);
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

		public string CurrentTime
		{
			get
			{
				return GetParameter("wct");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wct");
					return;
				}
				if (!DateTime.TryParseExact(value, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var _))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0010", value), "value"));
				}
				SetParameter("wct", value);
			}
		}

		public string Freshness
		{
			get
			{
				return GetParameter("wfresh");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wfresh");
					return;
				}
				int result = -1;
				if (!int.TryParse(value, out result))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0018", result.GetType()), "value"));
				}
				SetParameter("wfresh", value);
			}
		}

		public string HomeRealm
		{
			get
			{
				return GetParameter("whr");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("whr");
				}
				else
				{
					SetParameter("whr", value);
				}
			}
		}

		public string AuthenticationType
		{
			get
			{
				return GetParameter("wauth");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wauth");
				}
				else
				{
					SetUriParameter("wauth", value);
				}
			}
		}

		public string Policy
		{
			get
			{
				return GetParameter("wp");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wp");
				}
				else
				{
					SetUriParameter("wp", value);
				}
			}
		}

		public string Resource
		{
			get
			{
				return GetParameter("wres");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wres");
				}
				else
				{
					SetUriParameter("wres", value);
				}
			}
		}

		public string Realm
		{
			get
			{
				return GetParameter("wtrealm");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wtrealm");
				}
				else
				{
					SetUriParameter("wtrealm", value);
				}
			}
		}

		public string Request
		{
			get
			{
				return GetParameter("wreq");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wreq");
				}
				else
				{
					SetParameter("wreq", value);
				}
			}
		}

		public string RequestPtr
		{
			get
			{
				return GetParameter("wreqptr");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wreqptr");
				}
				else
				{
					SetUriParameter("wreqptr", value);
				}
			}
		}

		public SignInRequestMessage(Uri baseUrl, string realm)
			: this(baseUrl, realm, null)
		{
		}

		internal SignInRequestMessage(Uri baseUrl)
			: base(baseUrl, "wsignin1.0")
		{
		}

		public SignInRequestMessage(Uri baseUrl, string realm, string reply)
			: base(baseUrl, "wsignin1.0")
		{
			if (string.IsNullOrEmpty(realm) && string.IsNullOrEmpty(reply))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3204")));
			}
			if (!string.IsNullOrEmpty(realm))
			{
				SetParameter("wtrealm", realm);
			}
			if (!string.IsNullOrEmpty(reply))
			{
				SetParameter("wreply", reply);
			}
		}

		protected override void Validate()
		{
			base.Validate();
			string parameter = GetParameter("wa");
			if (parameter != "wsignin1.0")
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3000", parameter)));
			}
			string parameter2 = GetParameter("wtrealm");
			string parameter3 = GetParameter("wreply");
			if (string.IsNullOrEmpty(parameter2) && string.IsNullOrEmpty(parameter3))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3204")));
			}
			string parameter4 = GetParameter("wreq");
			string parameter5 = GetParameter("wreqptr");
			if (!string.IsNullOrEmpty(parameter4) && !string.IsNullOrEmpty(parameter5))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3142")));
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
