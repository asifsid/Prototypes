using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel
{
	internal static class EmptyReadOnlyCollection<T>
	{
		public static readonly ReadOnlyCollection<T> Instance = new ReadOnlyCollection<T>(new T[0]);
	}
}
