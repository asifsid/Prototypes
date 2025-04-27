using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2SubjectConfirmationData
	{
		private string _address;

		private Saml2Id _inResponseTo;

		private Collection<SecurityKeyIdentifier> _keyIdentifiers = new Collection<SecurityKeyIdentifier>();

		private DateTime? _notBefore;

		private DateTime? _notOnOrAfter;

		private Uri _recipient;

		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public Saml2Id InResponseTo
		{
			get
			{
				return _inResponseTo;
			}
			set
			{
				_inResponseTo = value;
			}
		}

		public Collection<SecurityKeyIdentifier> KeyIdentifiers => _keyIdentifiers;

		public DateTime? NotBefore
		{
			get
			{
				return _notBefore;
			}
			set
			{
				_notBefore = DateTimeUtil.ToUniversalTime(value);
			}
		}

		public DateTime? NotOnOrAfter
		{
			get
			{
				return _notOnOrAfter;
			}
			set
			{
				_notOnOrAfter = DateTimeUtil.ToUniversalTime(value);
			}
		}

		public Uri Recipient
		{
			get
			{
				return _recipient;
			}
			set
			{
				if (null != value && !value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0013"));
				}
				_recipient = value;
			}
		}
	}
}
