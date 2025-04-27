using System;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Web.Configuration
{
	[ComVisible(true)]
	public class ServiceConfigurationCreatedEventArgs : EventArgs
	{
		private ServiceConfiguration _serviceConfiguration;

		public ServiceConfiguration ServiceConfiguration
		{
			get
			{
				return _serviceConfiguration;
			}
			set
			{
				_serviceConfiguration = value;
			}
		}

		public ServiceConfigurationCreatedEventArgs(ServiceConfiguration config)
		{
			_serviceConfiguration = config;
		}
	}
}
