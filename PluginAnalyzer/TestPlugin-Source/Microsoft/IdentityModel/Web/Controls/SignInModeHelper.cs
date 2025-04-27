namespace Microsoft.IdentityModel.Web.Controls
{
	internal static class SignInModeHelper
	{
		internal static bool IsDefined(SignInMode value)
		{
			if (value != 0)
			{
				return value == SignInMode.Single;
			}
			return true;
		}
	}
}
