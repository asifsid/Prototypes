using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public sealed class ClaimsPrincipalPermission : IPermission, ISecurityEncodable, IUnrestrictedPermission
	{
		private class ResourceAction
		{
			public string Action;

			public string Resource;

			public ResourceAction(string resource, string action)
			{
				if (string.IsNullOrEmpty(resource))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("resource");
				}
				Resource = resource;
				Action = action;
			}

			public override bool Equals(object obj)
			{
				ResourceAction resourceAction = obj as ResourceAction;
				if (resourceAction != null)
				{
					if (string.CompareOrdinal(resourceAction.Resource, Resource) == 0)
					{
						return string.CompareOrdinal(resourceAction.Action, Action) == 0;
					}
					return false;
				}
				return base.Equals(obj);
			}

			public override int GetHashCode()
			{
				return Resource.GetHashCode() ^ Action.GetHashCode();
			}
		}

		private List<ResourceAction> _resourceActions = new List<ResourceAction>();

		public static void CheckAccess(string resource, string action)
		{
			ClaimsPrincipalPermission claimsPrincipalPermission = new ClaimsPrincipalPermission(resource, action);
			claimsPrincipalPermission.Demand();
		}

		public ClaimsPrincipalPermission(string resource, string action)
		{
			_resourceActions.Add(new ResourceAction(resource, action));
		}

		private ClaimsPrincipalPermission(IEnumerable<ResourceAction> resourceActions)
		{
			foreach (ResourceAction resourceAction in resourceActions)
			{
				_resourceActions.Add(new ResourceAction(resourceAction.Resource, resourceAction.Action));
			}
		}

		private void ThrowSecurityException()
		{
			AssemblyName assemblyName = null;
			Evidence evidence = null;
			PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
			permissionSet.Assert();
			try
			{
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				assemblyName = callingAssembly.GetName();
				if ((object)callingAssembly != Assembly.GetExecutingAssembly())
				{
					evidence = callingAssembly.Evidence;
				}
			}
			catch
			{
			}
			PermissionSet.RevertAssert();
			throw DiagnosticUtil.ExceptionUtil.ThrowHelper(new SecurityException("ID4266", assemblyName, null, null, null, SecurityAction.Demand, this, this, evidence), TraceEventType.Error);
		}

		public IPermission Copy()
		{
			return new ClaimsPrincipalPermission(_resourceActions);
		}

		public void Demand()
		{
			ServiceConfiguration current = ServiceConfiguration.GetCurrent();
			ClaimsAuthorizationManager claimsAuthorizationManager = current.ClaimsAuthorizationManager;
			foreach (ResourceAction resourceAction in _resourceActions)
			{
				IClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as IClaimsPrincipal;
				if (claimsPrincipal == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4284"));
				}
				AuthorizationContext context = new AuthorizationContext(claimsPrincipal, resourceAction.Resource, resourceAction.Action);
				if (!claimsAuthorizationManager.CheckAccess(context))
				{
					ThrowSecurityException();
				}
			}
		}

		public IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			ClaimsPrincipalPermission claimsPrincipalPermission = target as ClaimsPrincipalPermission;
			if (claimsPrincipalPermission == null)
			{
				return null;
			}
			List<ResourceAction> list = new List<ResourceAction>();
			foreach (ResourceAction resourceAction in claimsPrincipalPermission._resourceActions)
			{
				if (_resourceActions.Contains(resourceAction))
				{
					list.Add(resourceAction);
				}
			}
			return new ClaimsPrincipalPermission(list);
		}

		public bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return false;
			}
			ClaimsPrincipalPermission claimsPrincipalPermission = target as ClaimsPrincipalPermission;
			if (claimsPrincipalPermission == null)
			{
				return false;
			}
			foreach (ResourceAction resourceAction in _resourceActions)
			{
				if (!claimsPrincipalPermission._resourceActions.Contains(resourceAction))
				{
					return false;
				}
			}
			return true;
		}

		public IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			ClaimsPrincipalPermission claimsPrincipalPermission = target as ClaimsPrincipalPermission;
			if (claimsPrincipalPermission == null)
			{
				return null;
			}
			List<ResourceAction> list = new List<ResourceAction>();
			list.AddRange(claimsPrincipalPermission._resourceActions);
			foreach (ResourceAction resourceAction in _resourceActions)
			{
				if (!list.Contains(resourceAction))
				{
					list.Add(resourceAction);
				}
			}
			return new ClaimsPrincipalPermission(list);
		}

		public void FromXml(SecurityElement e)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelper(new NotImplementedException(), TraceEventType.Error);
		}

		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			Type type = GetType();
			StringBuilder stringBuilder = new StringBuilder(type.Assembly.ToString());
			stringBuilder.Replace('"', '\'');
			securityElement.AddAttribute("class", type.FullName + ", " + stringBuilder);
			securityElement.AddAttribute("version", "1");
			foreach (ResourceAction resourceAction in _resourceActions)
			{
				SecurityElement securityElement2 = new SecurityElement("ResourceAction");
				securityElement2.AddAttribute("resource", resourceAction.Resource);
				securityElement2.AddAttribute("action", resourceAction.Action);
				securityElement.AddChild(securityElement2);
			}
			return securityElement;
		}

		public bool IsUnrestricted()
		{
			return true;
		}
	}
}
