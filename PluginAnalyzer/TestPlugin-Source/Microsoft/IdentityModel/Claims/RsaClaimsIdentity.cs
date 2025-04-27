using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public class RsaClaimsIdentity : ClaimsIdentity, ISerializable
	{
		public RsaClaimsIdentity()
		{
		}

		public RsaClaimsIdentity(IEnumerable<Claim> claims, string authenticationType)
			: base(claims, authenticationType)
		{
		}

		protected RsaClaimsIdentity(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
}
