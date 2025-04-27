using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class UserCredentialCollection : KeyedCollection<UserCredentialType, IUserCredential>
	{
		public UserCredentialCollection()
			: this(null, null, null)
		{
		}

		public UserCredentialCollection(UserNamePasswordCredential userName)
			: this(userName, null, null)
		{
		}

		public UserCredentialCollection(X509CertificateCredential x509Certificate)
			: this(null, x509Certificate, null)
		{
		}

		public UserCredentialCollection(SelfIssuedCredentials selfIssuedCredential)
			: this(null, null, selfIssuedCredential)
		{
		}

		public UserCredentialCollection(UserNamePasswordCredential userName, X509CertificateCredential x509Certificate, SelfIssuedCredentials selfIssuedCredential)
		{
			if (userName != null)
			{
				Add(userName);
			}
			if (x509Certificate != null)
			{
				Add(x509Certificate);
			}
			if (selfIssuedCredential != null)
			{
				Add(selfIssuedCredential);
			}
		}

		public UserCredentialCollection(IEnumerable<IUserCredential> collection)
		{
			AddRange(collection);
		}

		protected override UserCredentialType GetKeyForItem(IUserCredential item)
		{
			return item.CredentialType;
		}

		public void AddRange(IEnumerable<IUserCredential> collection)
		{
			if (collection == null)
			{
				return;
			}
			foreach (IUserCredential item in collection)
			{
				Add(item);
			}
		}
	}
}
