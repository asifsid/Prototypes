using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class ProtocolEndpoint
	{
		private Uri _binding;

		private Uri _location;

		private Uri _responseLocation;

		public Uri Binding
		{
			get
			{
				return _binding;
			}
			set
			{
				_binding = value;
			}
		}

		public Uri Location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		public Uri ResponseLocation
		{
			get
			{
				return _responseLocation;
			}
			set
			{
				_responseLocation = value;
			}
		}

		public ProtocolEndpoint()
			: this(null, null)
		{
		}

		public ProtocolEndpoint(Uri binding, Uri location)
		{
			Binding = binding;
			Location = location;
		}
	}
}
