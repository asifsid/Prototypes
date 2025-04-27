using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Web.Configuration;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public static class FederatedAuthentication
	{
		public const int DefaultMaxArrayLength = 2097152;

		public const int DefaultMaxStringContentLength = 2097152;

		internal static readonly string ModulesKey = typeof(FederatedAuthentication).AssemblyQualifiedName;

		[ThreadStatic]
		internal static IDictionary _currentItemsOverride;

		internal static ServiceConfiguration _serviceConfiguration;

		internal static object _serviceConfigurationLock = new object();

		public static ClaimsAuthorizationModule ClaimsAuthorizationModule => GetHttpModule<ClaimsAuthorizationModule>();

		public static ClaimsPrincipalHttpModule ClaimsPrincipalHttpModule => GetHttpModule<ClaimsPrincipalHttpModule>();

		public static ServiceConfiguration ServiceConfiguration
		{
			get
			{
				lock (_serviceConfigurationLock)
				{
					if (_serviceConfiguration == null)
					{
						_serviceConfiguration = new ServiceConfiguration();
						ServiceConfigurationCreatedEventArgs serviceConfigurationCreatedEventArgs = new ServiceConfigurationCreatedEventArgs(_serviceConfiguration);
						FederatedAuthentication.ServiceConfigurationCreated?.Invoke(null, serviceConfigurationCreatedEventArgs);
						_serviceConfiguration = serviceConfigurationCreatedEventArgs.ServiceConfiguration;
						if (!_serviceConfiguration.IsInitialized)
						{
							_serviceConfiguration.Initialize();
						}
					}
					return _serviceConfiguration;
				}
			}
		}

		public static SessionAuthenticationModule SessionAuthenticationModule => GetHttpModule<SessionAuthenticationModule>();

		public static WSFederationAuthenticationModule WSFederationAuthenticationModule => GetHttpModule<WSFederationAuthenticationModule>();

		public static event EventHandler<ServiceConfigurationCreatedEventArgs> ServiceConfigurationCreated;

		private static IDictionary GetCurrentContextItems()
		{
			if (_currentItemsOverride == null)
			{
				if (HttpContext.Current == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1061"));
				}
				return HttpContext.Current.Items;
			}
			return _currentItemsOverride;
		}

		private static T GetHttpModule<T>() where T : class, IHttpModule
		{
			Dictionary<Type, IHttpModule> httpContextModuleMap = GetHttpContextModuleMap();
			IHttpModule value = null;
			if (!httpContextModuleMap.TryGetValue(typeof(T), out value))
			{
				value = GetHttpContextModule<T>();
				httpContextModuleMap.Add(typeof(T), value);
			}
			return (T)value;
		}

		private static Dictionary<Type, IHttpModule> GetHttpContextModuleMap()
		{
			IDictionary currentContextItems = GetCurrentContextItems();
			Dictionary<Type, IHttpModule> dictionary = currentContextItems[ModulesKey] as Dictionary<Type, IHttpModule>;
			if (dictionary == null)
			{
				dictionary = new Dictionary<Type, IHttpModule>();
				currentContextItems[ModulesKey] = dictionary;
			}
			return dictionary;
		}

		private static T GetHttpContextModule<T>() where T : class, IHttpModule
		{
			T val = null;
			HttpModuleCollection modules = HttpContext.Current.ApplicationInstance.Modules;
			for (int i = 0; i < modules.Count; i++)
			{
				val = modules[i] as T;
				if (val != null)
				{
					break;
				}
			}
			return val;
		}
	}
}
