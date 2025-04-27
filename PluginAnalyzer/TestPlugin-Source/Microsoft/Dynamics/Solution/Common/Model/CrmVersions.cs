using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Model
{
	[ComVisible(true)]
	public static class CrmVersions
	{
		public static readonly Version CrmVersion10 = new Version(1, 0);

		public static readonly Version CrmVersion12 = new Version(1, 2);

		public static readonly Version CrmVersion30 = new Version(3, 0);

		public static readonly Version CurrentVersion = CrmVersion30;

		public static readonly Version LeoVersion = new Version("6.1.0.0");

		public static readonly Version OrionVersion = new Version("6.0.0.0");

		public static readonly Version VegaVersion = new Version("7.0.0.0");

		public static readonly Version CarinaVersion = new Version("7.1.0.0");

		public static readonly Version AraVersion = new Version("8.0.0.0");

		public static readonly Version AraUR1Version = new Version("8.0.1");

		public static readonly Version NaosVersion = new Version("8.1.0.0");

		public static readonly Version CentaurusVersion = new Version("8.2.0.0");

		public static readonly Version PotassiumVersion = new Version("9.0.0.0");
	}
}
