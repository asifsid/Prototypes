using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class SchedulerResponseData : IAzureSchedulerResponse
	{
		public string Name { get; set; }

		public string GetJobName()
		{
			if (!string.IsNullOrWhiteSpace(Name))
			{
				string[] array = Name.Split('/');
				if (array != null && array.Length > 1)
				{
					return array[1];
				}
			}
			return null;
		}
	}
}
