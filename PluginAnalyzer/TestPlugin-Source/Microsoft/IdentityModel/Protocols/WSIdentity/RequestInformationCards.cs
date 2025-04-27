using System;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class RequestInformationCards
	{
		private Uri _cardIdentifier;

		private CardSignatureFormatType _cardSignatureFormat;

		private Uri _cardType;

		private Uri _issuer;

		private SecurityTokenElement _onBehalfOf;

		public Uri CardIdentifier
		{
			get
			{
				return _cardIdentifier;
			}
			set
			{
				_cardIdentifier = value;
			}
		}

		public CardSignatureFormatType CardSignatureFormat
		{
			get
			{
				return _cardSignatureFormat;
			}
			set
			{
				_cardSignatureFormat = value;
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

		public Uri Issuer
		{
			get
			{
				return _issuer;
			}
			set
			{
				_issuer = value;
			}
		}

		public SecurityTokenElement OnBehalfOf
		{
			get
			{
				return _onBehalfOf;
			}
			set
			{
				_onBehalfOf = value;
			}
		}
	}
}
