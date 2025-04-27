using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Protocols.XmlSignature;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
	[ComVisible(true)]
	public class Saml11Assertion : SamlAssertion
	{
		private XmlTokenStream _sourceData;

		private SecurityToken _issuerToken;

		public new virtual bool CanWriteSourceData => null != _sourceData;

		public SecurityToken IssuerToken
		{
			get
			{
				return _issuerToken;
			}
			set
			{
				_issuerToken = value;
			}
		}

		public Saml11Assertion()
		{
		}

		public Saml11Assertion(string assertionId, string issuer, DateTime issueInstant, SamlConditions samlConditions, SamlAdvice samlAdvice, IEnumerable<SamlStatement> samlStatements)
			: base(assertionId, issuer, issueInstant, samlConditions, samlAdvice, samlStatements)
		{
		}

		public virtual void CaptureSourceData(EnvelopedSignatureReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			_sourceData = reader.XmlTokens;
		}

		public new virtual void WriteSourceData(XmlWriter writer)
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
