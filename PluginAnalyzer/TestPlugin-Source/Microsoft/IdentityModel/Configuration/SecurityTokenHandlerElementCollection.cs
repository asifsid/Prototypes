using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Configuration
{
	[ConfigurationCollection(typeof(CustomTypeElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[ComVisible(true)]
	public class SecurityTokenHandlerElementCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
			set
			{
				base["name"] = value;
			}
		}

		[ConfigurationProperty("securityTokenHandlerConfiguration", IsRequired = false)]
		public SecurityTokenHandlerConfigurationElement HandlerConfiguration
		{
			get
			{
				return (SecurityTokenHandlerConfigurationElement)base["securityTokenHandlerConfiguration"];
			}
			set
			{
				base["securityTokenHandlerConfiguration"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (string.IsNullOrEmpty(Name) && !HandlerConfiguration.IsConfigured)
				{
					return base.Count > 0;
				}
				return true;
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new CustomTypeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((CustomTypeElement)element).TypeName;
		}

		protected override void Init()
		{
			BaseAdd(new CustomTypeElement(typeof(Saml11SecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(Saml2SecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(WindowsUserNameSecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(X509SecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(KerberosSecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(RsaSecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(SessionSecurityTokenHandler)));
			BaseAdd(new CustomTypeElement(typeof(EncryptedSecurityTokenHandler)));
		}
	}
}
