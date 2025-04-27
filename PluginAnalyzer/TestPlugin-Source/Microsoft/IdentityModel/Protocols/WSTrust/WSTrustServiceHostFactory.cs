using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Compilation;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustServiceHostFactory : ServiceHostFactory
	{
		public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
		{
			if (string.IsNullOrEmpty(constructorString))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("constructorString");
			}
			SecurityTokenServiceConfiguration securityTokenServiceConfiguration = CreateSecurityTokenServiceConfiguration(constructorString);
			if (securityTokenServiceConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3096", constructorString)));
			}
			return new WSTrustServiceHost(securityTokenServiceConfiguration, baseAddresses);
		}

		protected virtual SecurityTokenServiceConfiguration CreateSecurityTokenServiceConfiguration(string constructorString)
		{
			if (string.IsNullOrEmpty(constructorString))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("constructorString");
			}
			Type type = BuildManager.GetType(constructorString, throwOnError: true);
			if (!type.IsSubclassOf(typeof(SecurityTokenServiceConfiguration)))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3087", type)));
			}
			object obj = Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
			return obj as SecurityTokenServiceConfiguration;
		}
	}
}
