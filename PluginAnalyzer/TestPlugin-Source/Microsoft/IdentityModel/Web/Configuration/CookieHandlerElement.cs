using System;
using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Web.Configuration
{
	[ComVisible(true)]
	public class CookieHandlerElement : ConfigurationElement
	{
		private const bool DefaultHideFromScript = true;

		private const bool DefaultRequireSsl = true;

		private const string MaxPersistentSessionLifetimeString = "365.0:0:0";

		private const string TimeSpanZeroString = "0:0:0";

		[ConfigurationProperty("mode", IsRequired = false, DefaultValue = CookieHandlerMode.Default)]
		public CookieHandlerMode Mode
		{
			get
			{
				return (CookieHandlerMode)base["mode"];
			}
			set
			{
				base["mode"] = value;
			}
		}

		[ConfigurationProperty("chunkedCookieHandler", IsRequired = false)]
		public ChunkedCookieHandlerElement ChunkedCookieHandler
		{
			get
			{
				return (ChunkedCookieHandlerElement)base["chunkedCookieHandler"];
			}
			set
			{
				base["chunkedCookieHandler"] = value;
			}
		}

		[ConfigurationProperty("customCookieHandler", IsRequired = false)]
		public CustomTypeElement CustomCookieHandler
		{
			get
			{
				return (CustomTypeElement)base["customCookieHandler"];
			}
			set
			{
				base["customCookieHandler"] = value;
			}
		}

		[ConfigurationProperty("domain", IsRequired = false)]
		public string Domain
		{
			get
			{
				return (string)base["domain"];
			}
			set
			{
				base["domain"] = value;
			}
		}

		[ConfigurationProperty("hideFromScript", IsRequired = false, DefaultValue = true)]
		public bool HideFromScript
		{
			get
			{
				return (bool)base["hideFromScript"];
			}
			set
			{
				base["hideFromScript"] = value;
			}
		}

		[ConfigurationProperty("name", IsRequired = false)]
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

		[ConfigurationProperty("path", IsRequired = false)]
		public string Path
		{
			get
			{
				return (string)base["path"];
			}
			set
			{
				base["path"] = value;
			}
		}

		[ConfigurationProperty("persistentSessionLifetime", DefaultValue = "0:0:0", IsRequired = false)]
		[TimeSpanValidator(MinValueString = "0:0:0", MaxValueString = "365.0:0:0")]
		public TimeSpan PersistentSessionLifetime
		{
			get
			{
				return (TimeSpan)base["persistentSessionLifetime"];
			}
			set
			{
				base["persistentSessionLifetime"] = value;
			}
		}

		[ConfigurationProperty("requireSsl", IsRequired = false, DefaultValue = true)]
		public bool RequireSsl
		{
			get
			{
				return (bool)base["requireSsl"];
			}
			set
			{
				base["requireSsl"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (Mode == CookieHandlerMode.Default && !ChunkedCookieHandler.IsConfigured && !CustomCookieHandler.IsConfigured && string.IsNullOrEmpty(Domain) && HideFromScript && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Path) && PersistentSessionLifetime.Equals(TimeSpan.Zero))
				{
					return !RequireSsl;
				}
				return true;
			}
		}

		internal CookieHandler GetConfiguredCookieHandler()
		{
			CookieHandlerMode cookieHandlerMode = Mode;
			if (cookieHandlerMode == CookieHandlerMode.Default)
			{
				cookieHandlerMode = CookieHandlerMode.Chunked;
			}
			if (ChunkedCookieHandler.IsConfigured && CookieHandlerMode.Chunked != cookieHandlerMode)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "mode", SR.GetString("ID1027", "chunkedCookieHandler", Mode));
			}
			if (CustomCookieHandler.IsConfigured && CookieHandlerMode.Custom != cookieHandlerMode)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "mode", SR.GetString("ID1027", "customCookieHandler", Mode));
			}
			CookieHandler handler = new ChunkedCookieHandler();
			switch (cookieHandlerMode)
			{
			case CookieHandlerMode.Chunked:
				ApplyChunked(ref handler);
				break;
			case CookieHandlerMode.Custom:
				ApplyCustom(ref handler);
				break;
			}
			handler.HideFromClientScript = HideFromScript;
			handler.RequireSsl = RequireSsl;
			if (!string.IsNullOrEmpty(Domain))
			{
				handler.Domain = Domain;
			}
			if (!string.IsNullOrEmpty(Name))
			{
				handler.Name = Name;
			}
			if (!string.IsNullOrEmpty(Path))
			{
				handler.Path = Path;
			}
			if (PersistentSessionLifetime > TimeSpan.Zero)
			{
				handler.PersistentSessionLifetime = PersistentSessionLifetime;
			}
			return handler;
		}

		private void ApplyChunked(ref CookieHandler handler)
		{
			if (ChunkedCookieHandler.IsConfigured)
			{
				try
				{
					handler = new ChunkedCookieHandler(ChunkedCookieHandler.ChunkSize);
				}
				catch (ArgumentException inner)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(ChunkedCookieHandler, "chunkSize", inner);
				}
			}
		}

		private void ApplyCustom(ref CookieHandler handler)
		{
			if (!CustomCookieHandler.IsConfigured)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "mode", SR.GetString("ID1028", "customCookieHandler", Mode));
			}
			handler = CustomTypeElement.Resolve<CookieHandler>(CustomCookieHandler, new object[0]);
		}
	}
}
