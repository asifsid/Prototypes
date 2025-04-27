using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class InformationCard
	{
		public const long DefaultCardVersion = 1L;

		private Dictionary<string, object> _properties = new Dictionary<string, object>();

		private CardImage _cardImage;

		private InformationCardReference _cardReference;

		private Uri _cardType;

		private string _cardName;

		private string _issuer;

		private string _issuerName;

		private PrivacyNotice _privacyNotice;

		private AppliesToOption? _appliesToOptions;

		private Collection<string> _tokenTypeCollections;

		private DisplayClaimCollection _displayClaimCollection;

		private TokenServiceCollection _tokenServiceCollection;

		private DateTime? _timeExpires;

		private DateTime? _timeIssued;

		private string _language;

		private SigningCredentials _signingCredentials;

		private bool? _requireStrongRecipientIdentity;

		private Collection<IssuerInformation> _issuerInfo = new Collection<IssuerInformation>();

		public AppliesToOption? AppliesToOption
		{
			get
			{
				return _appliesToOptions;
			}
			set
			{
				_appliesToOptions = value;
			}
		}

		public CardImage CardImage
		{
			get
			{
				return _cardImage;
			}
			set
			{
				_cardImage = value;
			}
		}

		public string CardName
		{
			get
			{
				return _cardName;
			}
			set
			{
				_cardName = value;
			}
		}

		public Uri CardType
		{
			get
			{
				return _cardType;
			}
			set
			{
				_cardType = value;
			}
		}

		public InformationCardReference InformationCardReference
		{
			get
			{
				return _cardReference;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_cardReference = value;
			}
		}

		public string Issuer
		{
			get
			{
				return _issuer;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_issuer = value;
			}
		}

		public string IssuerName
		{
			get
			{
				return _issuerName;
			}
			set
			{
				if (value != null && (value.Length < 1 || value.Length > 64))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID3259"));
				}
				_issuerName = value;
			}
		}

		public Collection<IssuerInformation> IssuerInformation => _issuerInfo;

		public string Language
		{
			get
			{
				return _language;
			}
			set
			{
				_language = value;
			}
		}

		public PrivacyNotice PrivacyNotice
		{
			get
			{
				return _privacyNotice;
			}
			set
			{
				_privacyNotice = value;
			}
		}

		public Dictionary<string, object> Properties => _properties;

		public bool? RequireStrongRecipientIdentity
		{
			get
			{
				return _requireStrongRecipientIdentity;
			}
			set
			{
				_requireStrongRecipientIdentity = value;
			}
		}

		public DisplayClaimCollection SupportedClaimTypeList
		{
			get
			{
				if (_displayClaimCollection == null)
				{
					_displayClaimCollection = new DisplayClaimCollection();
				}
				return _displayClaimCollection;
			}
		}

		public Collection<string> SupportedTokenTypeList
		{
			get
			{
				if (_tokenTypeCollections == null)
				{
					_tokenTypeCollections = new Collection<string>();
				}
				return _tokenTypeCollections;
			}
		}

		public TokenServiceCollection TokenServiceList
		{
			get
			{
				if (_tokenServiceCollection == null)
				{
					_tokenServiceCollection = new TokenServiceCollection();
				}
				return _tokenServiceCollection;
			}
		}

		public DateTime? TimeExpires
		{
			get
			{
				return _timeExpires;
			}
			set
			{
				if (value.HasValue)
				{
					value = value.Value.ToUniversalTime();
				}
				_timeExpires = value;
			}
		}

		public DateTime? TimeIssued
		{
			get
			{
				return _timeIssued;
			}
			set
			{
				if (value.HasValue)
				{
					value = value.Value.ToUniversalTime();
				}
				_timeIssued = value;
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

		public InformationCard(string issuer)
			: this((SigningCredentials)null, issuer)
		{
		}

		public InformationCard(X509Certificate2 certificate, string issuer)
			: this(new Microsoft.IdentityModel.SecurityTokenService.X509SigningCredentials(certificate, "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "http://www.w3.org/2000/09/xmldsig#sha1"), issuer)
		{
		}

		public InformationCard(SigningCredentials signingCredentials, string issuer)
			: this(signingCredentials, issuer, new InformationCardReference(), DateTime.UtcNow)
		{
		}

		public InformationCard(X509Certificate2 certificate, string issuer, InformationCardReference reference, DateTime timeIssued)
			: this(new Microsoft.IdentityModel.SecurityTokenService.X509SigningCredentials(certificate, "http://www.w3.org/2000/09/xmldsig#rsa-sha1", "http://www.w3.org/2000/09/xmldsig#sha1"), issuer, reference, timeIssued)
		{
		}

		public InformationCard(SigningCredentials signingCredentials, string issuer, InformationCardReference reference, DateTime timeIssued)
		{
			_signingCredentials = signingCredentials;
			InformationCardReference = reference;
			Issuer = issuer;
			TimeIssued = timeIssued;
			AppliesToOption = Microsoft.IdentityModel.Protocols.WSIdentity.AppliesToOption.Required;
		}
	}
}
