using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class TokenService
	{
		private TokenServiceEndpoint _endpoint;

		private IUserCredential _userCredential;

		private static IUserCredential _defaultUserCredential = new KerberosCredential();

		public static IUserCredential DefaultUserCredential => _defaultUserCredential;

		public static UserCredentialType DefaultUserCredentialType => UserCredentialType.KerberosV5Credential;

		internal X509Certificate2Collection Certificates => _endpoint.Certificates;

		public string DisplayCredentialHint => _endpoint.DisplayCredentialHint;

		public EndpointAddress Address => _endpoint.Address;

		public MetadataSet MetadataSet => _endpoint.MetadataSet;

		public MetadataReference MetadataReference => _endpoint.MetadataReference;

		public IUserCredential UserCredential => _userCredential;

		public TokenService(TokenServiceEndpoint endpoint)
			: this(endpoint, DefaultUserCredential)
		{
		}

		public TokenService(EndpointAddress endpointAddress)
			: this(endpointAddress, DefaultUserCredential)
		{
		}

		public TokenService(EndpointAddress endpointAddress, IUserCredential userCredential)
			: this(new TokenServiceEndpoint(endpointAddress, userCredential?.CredentialType ?? DefaultUserCredentialType), userCredential)
		{
		}

		public TokenService(EndpointAddress endpointAddress, IUserCredential userCredential, string displayCredentialHint)
			: this(new TokenServiceEndpoint(endpointAddress, userCredential?.CredentialType ?? DefaultUserCredentialType, displayCredentialHint), userCredential)
		{
		}

		public TokenService(TokenServiceEndpoint endpoint, IUserCredential userCredential)
		{
			if (endpoint == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpoint");
			}
			if (userCredential == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("userCredential");
			}
			if (endpoint.UserCredentialType != userCredential.CredentialType)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2048", endpoint.UserCredentialType, userCredential.CredentialType));
			}
			if (endpoint.MetadataSet == null || endpoint.MetadataSet.MetadataSections == null || endpoint.MetadataSet.MetadataSections.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2049"));
			}
			for (int i = 0; i < endpoint.MetadataSet.MetadataSections.Count; i++)
			{
				MetadataSection metadataSection = endpoint.MetadataSet.MetadataSections[i];
				if (metadataSection.Metadata == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2049"));
				}
				MetadataReference metadataReference = metadataSection.Metadata as MetadataReference;
				if (metadataReference != null)
				{
					if (metadataReference.Address == null || metadataReference.Address.Uri == null || !metadataReference.Address.Uri.IsAbsoluteUri)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2030"));
					}
					if (!StringComparer.OrdinalIgnoreCase.Equals(metadataReference.Address.Uri.Scheme, Uri.UriSchemeHttps))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2029"));
					}
				}
			}
			_endpoint = endpoint;
			_userCredential = userCredential;
		}
	}
}
