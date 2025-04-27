using System;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class EnumExtensions
	{
		public static OptionSetValue ToOptionSetValue(this Enum enumeration)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return new OptionSetValue(enumeration.ToInt32());
		}

		public static int ToInt32(this Enum enumeration)
		{
			return Convert.ToInt32(enumeration);
		}
	}
}
