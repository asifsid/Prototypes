using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class InformationCardReference
	{
		private string _cardId;

		private long _cardVersion;

		public string CardId => _cardId;

		public long CardVersion
		{
			get
			{
				return _cardVersion;
			}
			set
			{
				_cardVersion = value;
			}
		}

		public InformationCardReference()
		{
			_cardId = UniqueId.CreateRandomUri();
			_cardVersion = 1L;
		}

		public InformationCardReference(string cardId, long cardVersion)
		{
			if (string.IsNullOrEmpty(cardId))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("cardId");
			}
			if (cardVersion < 1 || cardVersion > uint.MaxValue)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("cardVersion", SR.GetString("ID2027", 4294967295L)));
			}
			_cardId = cardId;
			_cardVersion = cardVersion;
		}
	}
}
