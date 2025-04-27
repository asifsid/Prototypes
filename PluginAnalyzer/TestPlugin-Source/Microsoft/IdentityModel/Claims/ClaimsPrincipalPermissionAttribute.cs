using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Claims
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	[ComVisible(true)]
	public class ClaimsPrincipalPermissionAttribute : CodeAccessSecurityAttribute
	{
		private string _resource;

		private string _operation;

		public string Operation
		{
			get
			{
				return _operation;
			}
			set
			{
				_operation = value;
			}
		}

		public string Resource
		{
			get
			{
				return _resource;
			}
			set
			{
				_resource = value;
			}
		}

		public ClaimsPrincipalPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		public override IPermission CreatePermission()
		{
			return new ClaimsPrincipalPermission(_resource, _operation);
		}
	}
}
