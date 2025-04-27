using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class DisplayToken
	{
		private string _language;

		private DisplayClaimCollection _displayClaims;

		public DisplayClaimCollection DisplayClaims => _displayClaims;

		public string Language => _language;

		public DisplayToken(string language, IEnumerable<DisplayClaim> displayClaims)
		{
			if (string.IsNullOrEmpty(language))
			{
				language = CultureInfo.CurrentUICulture.Name;
			}
			if (displayClaims == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("displayClaims");
			}
			_language = language;
			_displayClaims = new DisplayClaimCollection(displayClaims);
		}
	}
}
