using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class EkuPolicy
	{
		private Collection<Oid> _oids = new Collection<Oid>();

		public Collection<Oid> Oids => _oids;

		public EkuPolicy()
		{
		}

		public EkuPolicy(IEnumerable<Oid> oids)
		{
			if (oids == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("oids");
			}
			foreach (Oid oid in oids)
			{
				if (oid == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("oids", SR.GetString("ID3283"));
				}
				_oids.Add(oid);
			}
		}
	}
}
