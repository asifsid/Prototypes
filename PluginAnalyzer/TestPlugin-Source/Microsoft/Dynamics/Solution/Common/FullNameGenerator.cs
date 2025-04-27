using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class FullNameGenerator
	{
		public static string GenerateFullName(FullNameConventionCode fullNameConventionCode, string firstName, string middleName, string lastName)
		{
			firstName = ConvertNullToEmptyString(firstName);
			middleName = ConvertNullToEmptyString(middleName);
			lastName = ConvertNullToEmptyString(lastName);
			string text = string.Empty;
			if (middleName.Length > 0)
			{
				text = middleName.Substring(0, 1);
			}
			StringBuilder stringBuilder = new StringBuilder();
			switch (fullNameConventionCode)
			{
			case FullNameConventionCode.LastFirst:
				if (lastName.Length > 0 && firstName.Length > 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}, {1}", lastName, firstName);
				}
				else
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}", lastName, firstName);
				}
				break;
			case FullNameConventionCode.FirstLast:
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}", firstName, lastName);
				break;
			case FullNameConventionCode.LastFirstMiddleInitial:
				Generate3PartNameCommaSeperated(firstName, lastName, text, stringBuilder);
				break;
			case FullNameConventionCode.FirstMiddleInitialLast:
				stringBuilder.Append(firstName);
				if (middleName.Length > 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}.", text);
				}
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", lastName);
				break;
			case FullNameConventionCode.LastFirstMiddle:
				if (lastName.Length > 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}", lastName);
				}
				if (lastName.Length > 0 && (firstName.Length > 0 || middleName.Length > 0))
				{
					stringBuilder = stringBuilder.Append(",");
				}
				if (firstName.Length > 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", firstName);
				}
				if (middleName.Length > 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", middleName);
				}
				break;
			case FullNameConventionCode.FirstMiddleLast:
				stringBuilder.Append(firstName);
				if (middleName.Length > 0)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", middleName);
				}
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", lastName);
				break;
			case FullNameConventionCode.LastSpaceFirst:
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}", lastName, firstName);
				break;
			case FullNameConventionCode.LastNoSpaceFirst:
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", lastName, firstName);
				break;
			default:
				throw new ArgumentOutOfRangeException("fullNameConventionCode");
			}
			return stringBuilder.ToString().Trim();
		}

		private static void Generate3PartNameCommaSeperated(string first, string last, string middle, StringBuilder fullName)
		{
			if (last.Length > 0)
			{
				fullName.AppendFormat(CultureInfo.InvariantCulture, "{0}", last);
			}
			if (last.Length > 0 && (first.Length > 0 || middle.Length > 0))
			{
				fullName = fullName.Append(",");
			}
			if (first.Length > 0)
			{
				fullName.AppendFormat(CultureInfo.InvariantCulture, " {0}", first);
			}
			if (middle.Length > 0)
			{
				fullName.AppendFormat(CultureInfo.InvariantCulture, " {0}.", middle);
			}
		}

		private static void Generate3PartNameCommaSeperatedSqlClause(string first, string last, string middle, StringBuilder fullName)
		{
			if (last.Length > 0)
			{
				fullName.AppendFormat(CultureInfo.InvariantCulture, "{0} ", last);
			}
			if (last.Length > 0 && (first.Length > 0 || middle.Length > 0))
			{
				fullName = fullName.Append("+ ', '");
			}
			if (first.Length > 0)
			{
				fullName.AppendFormat(CultureInfo.InvariantCulture, "+ {0}", first);
			}
			if (middle.Length > 0)
			{
				fullName.AppendFormat(CultureInfo.InvariantCulture, "+ ' ' + {0} + '.'", middle);
			}
		}

		private static string ConvertNullToEmptyString(string name)
		{
			if (name == null)
			{
				return string.Empty;
			}
			return name.Trim();
		}
	}
}
