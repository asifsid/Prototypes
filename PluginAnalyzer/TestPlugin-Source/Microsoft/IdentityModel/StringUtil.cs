namespace Microsoft.IdentityModel
{
	internal static class StringUtil
	{
		public static string OptimizeString(string value)
		{
			if (value != null)
			{
				string text = string.IsInterned(value);
				if (text != null)
				{
					return text;
				}
			}
			return value;
		}
	}
}
