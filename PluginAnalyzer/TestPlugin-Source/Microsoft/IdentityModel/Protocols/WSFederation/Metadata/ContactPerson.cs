using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class ContactPerson
	{
		private ContactType _type;

		private string _company;

		private string _givenName;

		private string _surname;

		private Collection<string> _emailAddresses = new Collection<string>();

		private Collection<string> _telephoneNumbers = new Collection<string>();

		public string Company
		{
			get
			{
				return _company;
			}
			set
			{
				_company = value;
			}
		}

		public ICollection<string> EmailAddresses => _emailAddresses;

		public string GivenName
		{
			get
			{
				return _givenName;
			}
			set
			{
				_givenName = value;
			}
		}

		public string Surname
		{
			get
			{
				return _surname;
			}
			set
			{
				_surname = value;
			}
		}

		public ICollection<string> TelephoneNumbers => _telephoneNumbers;

		public ContactType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public ContactPerson()
		{
		}

		public ContactPerson(ContactType contactType)
		{
			_type = contactType;
		}
	}
}
