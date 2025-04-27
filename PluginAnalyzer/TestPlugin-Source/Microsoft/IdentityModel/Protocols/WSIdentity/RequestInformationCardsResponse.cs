using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class RequestInformationCardsResponse
	{
		private Collection<string> _informationCards = new Collection<string>();

		public Collection<string> InformationCards => _informationCards;

		public RequestInformationCardsResponse()
		{
		}

		public RequestInformationCardsResponse(IEnumerable<string> informationCards)
		{
			if (informationCards == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("informationCards");
			}
			foreach (string informationCard in informationCards)
			{
				if (string.IsNullOrEmpty(informationCard))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("informationCards", SR.GetString("ID3282"));
				}
				_informationCards.Add(informationCard);
			}
		}
	}
}
