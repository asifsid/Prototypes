using System;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class ServiceHostEndpointConfiguration
	{
		private string _address;

		private Binding _binding;

		private Type _contractType;

		public string Address => _address;

		public Binding Binding => _binding;

		public Type Contract => _contractType;

		public ServiceHostEndpointConfiguration(Type contractType, Binding binding, string address)
		{
			if (binding == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("binding");
			}
			if ((object)contractType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contractType");
			}
			if (string.IsNullOrEmpty(address))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("address");
			}
			_address = address;
			_binding = binding;
			_contractType = contractType;
		}
	}
}
