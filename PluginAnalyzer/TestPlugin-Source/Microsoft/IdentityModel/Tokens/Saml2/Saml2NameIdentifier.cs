using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2NameIdentifier
	{
		private Uri _format;

		private string _nameQualifier;

		private string _spNameQualifier;

		private string _spProvidedId;

		private string _value;

		private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;

		private Collection<EncryptedKeyIdentifierClause> _externalEncryptedKeys;

		public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials
		{
			get
			{
				return _encryptingCredentials;
			}
			set
			{
				_encryptingCredentials = value;
			}
		}

		public Collection<EncryptedKeyIdentifierClause> ExternalEncryptedKeys => _externalEncryptedKeys;

		public Uri Format
		{
			get
			{
				return _format;
			}
			set
			{
				if (null != value && !value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0013"));
				}
				_format = value;
			}
		}

		public string NameQualifier
		{
			get
			{
				return _nameQualifier;
			}
			set
			{
				_nameQualifier = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public string SPNameQualifier
		{
			get
			{
				return _spNameQualifier;
			}
			set
			{
				_spNameQualifier = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public string SPProvidedId
		{
			get
			{
				return _spProvidedId;
			}
			set
			{
				_spProvidedId = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_value = value;
			}
		}

		public Saml2NameIdentifier(string name)
			: this(name, null)
		{
		}

		public Saml2NameIdentifier(string name, Uri format)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name");
			}
			if (null != format && !format.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("format", SR.GetString("ID0013"));
			}
			_format = format;
			_value = name;
			_externalEncryptedKeys = new Collection<EncryptedKeyIdentifierClause>();
		}
	}
}
