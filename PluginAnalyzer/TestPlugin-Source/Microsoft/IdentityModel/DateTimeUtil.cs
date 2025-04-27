using System;

namespace Microsoft.IdentityModel
{
	internal static class DateTimeUtil
	{
		public static DateTime Add(DateTime time, TimeSpan timespan)
		{
			if (timespan >= TimeSpan.Zero && DateTime.MaxValue - time <= timespan)
			{
				return GetMaxValue(time.Kind);
			}
			if (timespan <= TimeSpan.Zero && DateTime.MinValue - time >= timespan)
			{
				return GetMinValue(time.Kind);
			}
			return time + timespan;
		}

		public static DateTime AddNonNegative(DateTime time, TimeSpan timespan)
		{
			if (timespan <= TimeSpan.Zero)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2082")));
			}
			return Add(time, timespan);
		}

		public static DateTime GetMaxValue(DateTimeKind kind)
		{
			return new DateTime(DateTime.MaxValue.Ticks, kind);
		}

		public static DateTime GetMinValue(DateTimeKind kind)
		{
			return new DateTime(DateTime.MinValue.Ticks, kind);
		}

		public static DateTime? ToUniversalTime(DateTime? value)
		{
			if (!value.HasValue || value.Value.Kind == DateTimeKind.Utc)
			{
				return value;
			}
			return ToUniversalTime(value.Value);
		}

		public static DateTime ToUniversalTime(DateTime value)
		{
			if (value.Kind == DateTimeKind.Utc)
			{
				return value;
			}
			return value.ToUniversalTime();
		}
	}
}
