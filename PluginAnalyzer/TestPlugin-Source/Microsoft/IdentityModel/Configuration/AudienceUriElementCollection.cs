using System.ComponentModel;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ConfigurationCollection(typeof(AudienceUriElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	[ComVisible(true)]
	public class AudienceUriElementCollection : ConfigurationElementCollection
	{
		private const AudienceUriMode DefaultAudienceUriMode = AudienceUriMode.Always;

		[TypeConverter(typeof(AudienceUriModeConverter))]
		[ConfigurationProperty("mode", IsRequired = false, DefaultValue = AudienceUriMode.Always)]
		public AudienceUriMode Mode
		{
			get
			{
				return (AudienceUriMode)base["mode"];
			}
			set
			{
				base["mode"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (Mode == AudienceUriMode.Always)
				{
					return base.Count > 0;
				}
				return true;
			}
		}

		protected override void Init()
		{
			base.Init();
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new AudienceUriElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AudienceUriElement)element).Value;
		}
	}
}
