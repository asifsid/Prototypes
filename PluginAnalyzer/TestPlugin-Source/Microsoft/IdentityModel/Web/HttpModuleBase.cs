using System.Runtime.InteropServices;
using System.Web;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public abstract class HttpModuleBase : IHttpModule
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
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_serviceConfiguration = value;
			}
		}

		public virtual void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			_serviceConfiguration = FederatedAuthentication.ServiceConfiguration;
			InitializeModule(context);
		}

		protected abstract void InitializeModule(HttpApplication context);
	}
}
