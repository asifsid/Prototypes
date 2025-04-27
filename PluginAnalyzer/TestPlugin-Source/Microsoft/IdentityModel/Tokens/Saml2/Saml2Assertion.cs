using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Assertion
	{
		private Saml2Advice _advice;

		private Saml2Conditions _conditions;

		private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;

		private Collection<EncryptedKeyIdentifierClause> _externalEncryptedKeys = new Collection<EncryptedKeyIdentifierClause>();

		private Saml2Id _id = new Saml2Id();

		private DateTime _issueInstant = DateTime.UtcNow;

		private Saml2NameIdentifier _issuer;

		private SigningCredentials _signingCredentials;

		private XmlTokenStream _sourceData;

		private Collection<Saml2Statement> _statements = new Collection<Saml2Statement>();

		private Saml2Subject _subject;

		private string _version = "2.0";

		public Saml2Advice Advice
		{
			get
			{
				return _advice;
			}
			set
			{
				_advice = value;
			}
		}

		public virtual bool CanWriteSourceData => null != _sourceData;

		public Saml2Conditions Conditions
		{
			get
			{
				return _conditions;
			}
			set
			{
				_conditions = value;
			}
		}

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

		public Saml2Id Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_id = value;
				_sourceData = null;
			}
		}

		public DateTime IssueInstant
		{
			get
			{
				return _issueInstant;
			}
			set
			{
				_issueInstant = DateTimeUtil.ToUniversalTime(value);
			}
		}

		public Saml2NameIdentifier Issuer
		{
			get
			{
				return _issuer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_issuer = value;
			}
		}

		public SigningCredentials SigningCredentials
		{
			get
			{
				return _signingCredentials;
			}
			set
			{
				_signingCredentials = value;
			}
		}

		public Saml2Subject Subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}

		public Collection<Saml2Statement> Statements => _statements;

		public string Version => _version;

		public Saml2Assertion(Saml2NameIdentifier issuer)
		{
			if (issuer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("issuer");
			}
			_issuer = issuer;
		}

		public virtual void CaptureSourceData(EnvelopedSignatureReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			_sourceData = reader.XmlTokens;
		}

		public virtual void WriteSourceData(XmlWriter writer)
		{
			if (!CanWriteSourceData)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4140")));
			}
			XmlDictionaryWriter writer2 = XmlDictionaryWriter.CreateDictionaryWriter(writer);
			_sourceData.SetElementExclusion(null, null);
			_sourceData.GetWriter().WriteTo(writer2);
		}
	}
}
