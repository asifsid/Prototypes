using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class ExpirationCacheItem<T>
	{
		public T Value { get; private set; }

		public DateTime AddedOn { get; private set; }

		public TimeSpan MaxCacheAge { get; private set; }

		public ExpirationCacheItem(T value, TimeSpan maxCacheAge)
		{
			Value = value;
			AddedOn = DateTime.UtcNow;
			MaxCacheAge = maxCacheAge;
		}

		public bool IsValid()
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = utcNow - AddedOn;
			return timeSpan <= MaxCacheAge;
		}
	}
}
