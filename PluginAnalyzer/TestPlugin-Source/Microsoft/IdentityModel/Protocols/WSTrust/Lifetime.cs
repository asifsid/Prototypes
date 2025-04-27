using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class Lifetime
	{
		private DateTime? _created;

		private DateTime? _expires;

		public DateTime? Created
		{
			get
			{
				return _created;
			}
			set
			{
				_created = value;
			}
		}

		public DateTime? Expires
		{
			get
			{
				return _expires;
			}
			set
			{
				_expires = value;
			}
		}

		public Lifetime(DateTime created, DateTime expires)
			: this((DateTime?)created, (DateTime?)expires)
		{
		}

		public Lifetime(DateTime? created, DateTime? expires)
		{
			if (created.HasValue && expires.HasValue && expires.Value <= created.Value)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID2000")));
			}
			_created = DateTimeUtil.ToUniversalTime(created);
			_expires = DateTimeUtil.ToUniversalTime(expires);
		}
	}
}
