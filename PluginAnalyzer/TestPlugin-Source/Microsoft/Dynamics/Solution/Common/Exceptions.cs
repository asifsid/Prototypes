using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Localization;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class Exceptions
	{
		public static void ThrowIfNull(object parameter, string name, int? errorCode = null)
		{
			if (parameter == null)
			{
				throw errorCode.HasValue ? new CrmArgumentNullException(name, errorCode.Value) : new CrmArgumentNullException(name);
			}
		}

		public static void ThrowIfGuidEmpty(Guid parameter, string name, int errorCode)
		{
			if (parameter == Guid.Empty)
			{
				throw new CrmArgumentException(Labels.InvalidGuidParameter, name, errorCode);
			}
		}

		public static void ThrowIfGuidEmpty(Guid parameter, string name)
		{
			ThrowIfGuidEmpty(parameter, name, -2147220989);
		}

		public static void ThrowIfEmpty(string value, string parameterName, int? errorCode = null)
		{
			ThrowIfNull(value, parameterName, errorCode);
			if (value.Length == 0)
			{
				throw errorCode.HasValue ? new CrmArgumentException(Labels.StringIsEmpty, parameterName, errorCode.Value) : new CrmArgumentException(Labels.StringIsEmpty, parameterName);
			}
		}

		public static void ThrowIfCollectionEmpty<T>(ICollection<T> value, string parameterName)
		{
			ThrowIfNull(value, parameterName);
			if (value.Count == 0)
			{
				throw new CrmArgumentException(Labels.CollectionIsEmpty, parameterName);
			}
		}

		public static void ThrowIfNotEmpty(string value, string parameterName)
		{
			if (value != null && value.Length != 0)
			{
				throw new CrmArgumentException(Labels.StringIsNotEmpty, parameterName);
			}
		}

		public static void ThrowIfOutOfRange(int value, int min, int max, string parameterName)
		{
			if (value < min || value > max)
			{
				throw new CrmArgumentOutOfRangeException(parameterName, value, string.Format(CultureInfo.InvariantCulture, "Expected value between {0} and {1} inclusive.", new int[2] { min, max }));
			}
		}

		public static void ThrowIfNotDefined(Type enumType, object value, string parameterName)
		{
			if (!enumType.IsSubclassOf(typeof(Enum)))
			{
				throw new CrmInvalidOperationException("Unable to perform IsDefined operation on non-enum types.");
			}
			if (!Enum.IsDefined(enumType, value))
			{
				throw new CrmArgumentOutOfRangeException(parameterName, value, "Value for this parameter must be defined by the enum.");
			}
		}

		public static void ThrowIfInterfaceNotImplemented(Type type, Type interfaceInQuestion)
		{
			Type[] interfaces = type.GetInterfaces();
			Type[] array = interfaces;
			foreach (Type type2 in array)
			{
				if (type2 == interfaceInQuestion)
				{
					return;
				}
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] args = new string[2] { type.Name, interfaceInQuestion.Name };
			throw new CrmInvalidOperationException(string.Format(invariantCulture, "{0} must implement the required interface: {1}.", args));
		}

		public static void ThrowIfNotPositive(int value, string parameterName)
		{
			if (value <= 0)
			{
				throw new CrmArgumentOutOfRangeException(parameterName, value, "Value for this parameter must be greater than zero.");
			}
		}

		public static void ThrowIfNotPositive(long value, string parameterName)
		{
			if (value <= 0)
			{
				throw new CrmArgumentOutOfRangeException(parameterName, value, "Value for this parameter must be greater than zero.");
			}
		}

		public static void ThrowIfNegative(int value, string parameterName)
		{
			if (value < 0)
			{
				throw new CrmArgumentOutOfRangeException(parameterName, value, "Value for this parameter must be equal or greater than zero.");
			}
		}

		public static void ThrowIfNegative(TimeSpan value, string parameterName)
		{
			if (value < TimeSpan.Zero)
			{
				throw new CrmArgumentOutOfRangeException(parameterName, value, "Value for this parameter must be equal or greater than zero.");
			}
		}

		public static void ThrowIfNullOrEmpty(string parameter, string name)
		{
			ThrowIfNull(parameter, name);
			ThrowIfEmpty(parameter, name);
		}

		public static void ThrowIfNullOrWhiteSpace(string parameter, string name, int? errorCode = null)
		{
			ThrowIfNull(parameter, name, errorCode);
			ThrowIfEmpty(parameter.Trim(), name, errorCode);
		}

		public static void ThrowIfNoValue<T>(T? value, string message) where T : struct
		{
			if (!value.HasValue)
			{
				throw new CrmArgumentNullException(message);
			}
		}
	}
}
