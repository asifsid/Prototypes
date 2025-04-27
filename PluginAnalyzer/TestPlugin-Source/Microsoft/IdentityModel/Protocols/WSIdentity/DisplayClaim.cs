using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class DisplayClaim
	{
		private static Dictionary<string, string> _claimDescriptionMap = PopulateClaimDescriptionMap();

		private static Dictionary<string, string> _claimTagMap = PopulateClaimTagMap();

		private string _claimType;

		private string _displayTag;

		private string _displayValue;

		private string _description;

		private bool _optional;

		public string ClaimType => _claimType;

		public string DisplayTag
		{
			get
			{
				return _displayTag;
			}
			set
			{
				_displayTag = value;
			}
		}

		public string DisplayValue
		{
			get
			{
				return _displayValue;
			}
			set
			{
				_displayValue = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		public bool Optional
		{
			get
			{
				return _optional;
			}
			set
			{
				_optional = value;
			}
		}

		private static Dictionary<string, string> PopulateClaimTagMap()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country", SR.GetString("CountryText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth", SR.GetString("DateOfBirthText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", SR.GetString("EmailAddressText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender", SR.GetString("GenderText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", SR.GetString("GivenNameText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone", SR.GetString("HomePhoneText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality", SR.GetString("LocalityText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone", SR.GetString("MobilePhoneText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", SR.GetString("NameText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone", SR.GetString("OtherPhoneText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode", SR.GetString("PostalCodeText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier", SR.GetString("PPIDText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince", SR.GetString("StateOrProvinceText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress", SR.GetString("StreetAddressText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", SR.GetString("SurnameText"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage", SR.GetString("WebPageText"));
			dictionary.Add("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", SR.GetString("RoleText"));
			return dictionary;
		}

		private static Dictionary<string, string> PopulateClaimDescriptionMap()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country", SR.GetString("CountryDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth", SR.GetString("DateOfBirthDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", SR.GetString("EmailAddressDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender", SR.GetString("GenderDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", SR.GetString("GivenNameDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone", SR.GetString("HomePhoneDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality", SR.GetString("LocalityDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone", SR.GetString("MobilePhoneDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", SR.GetString("NameDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone", SR.GetString("OtherPhoneDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode", SR.GetString("PostalCodeDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier", SR.GetString("PPIDDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince", SR.GetString("StateOrProvinceDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress", SR.GetString("StreetAddressDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", SR.GetString("SurnameDescription"));
			dictionary.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage", SR.GetString("WebPageDescription"));
			dictionary.Add("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", SR.GetString("RoleDescription"));
			return dictionary;
		}

		private static string ClaimTagForClaimType(string claimType)
		{
			string value = null;
			_claimTagMap.TryGetValue(claimType, out value);
			return value;
		}

		private static string ClaimDescriptionForClaimType(string claimType)
		{
			string value = null;
			_claimDescriptionMap.TryGetValue(claimType, out value);
			return value;
		}

		public static DisplayClaim CreateDisplayClaimFromClaimType(string claimType)
		{
			DisplayClaim displayClaim = new DisplayClaim(claimType);
			displayClaim.DisplayTag = ClaimTagForClaimType(claimType);
			displayClaim.Description = ClaimDescriptionForClaimType(claimType);
			return displayClaim;
		}

		public DisplayClaim(string claimType)
			: this(claimType, null, null, null)
		{
		}

		public DisplayClaim(string claimType, string displayTag, string description)
			: this(claimType, displayTag, description, null)
		{
		}

		public DisplayClaim(string claimType, string displayTag, string description, string displayValue)
			: this(claimType, displayTag, description, displayValue, optional: true)
		{
		}

		public DisplayClaim(string claimType, string displayTag, string description, string displayValue, bool optional)
		{
			if (string.IsNullOrEmpty(claimType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimType");
			}
			_claimType = claimType;
			_displayTag = displayTag;
			_description = description;
			_displayValue = displayValue;
			_optional = optional;
		}
	}
}
