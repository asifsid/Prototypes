using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal class TransformFactory
	{
		private static TransformFactory _instance = new TransformFactory();

		internal static TransformFactory Instance => _instance;

		protected TransformFactory()
		{
		}

		public virtual Transform CreateTransform(string transformAlgorithmUri)
		{
			if (transformAlgorithmUri == "http://www.w3.org/2001/10/xml-exc-c14n#")
			{
				return new ExclusiveCanonicalizationTransform();
			}
			if (transformAlgorithmUri == "http://www.w3.org/2000/09/xmldsig#enveloped-signature")
			{
				return new EnvelopedSignatureTransform();
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6021")));
		}
	}
}
