using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common;

namespace Microsoft.Dynamics.LinkedInIntegration.Common.Localization
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[ComVisible(true)]
	public class Labels
	{
		private static PluginResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static PluginResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					PluginResourceManager pluginResourceManager = (resourceMan = new PluginResourceManager("Microsoft.Dynamics.LinkedInIntegration.Common.Localization.Labels", typeof(Labels).Assembly));
					resourceMan.SolutionName = "LinkedInIntegration";
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		public static string DefaultList => ResourceManager.GetString("DefaultList", resourceCulture);

		public static string PDLinkedinCardDescription => ResourceManager.GetString("PDLinkedinCardDescription", resourceCulture);

		public static string PDLinkedinCardTitle => ResourceManager.GetString("PDLinkedinCardTitle", resourceCulture);

		internal Labels()
		{
		}
	}
}
