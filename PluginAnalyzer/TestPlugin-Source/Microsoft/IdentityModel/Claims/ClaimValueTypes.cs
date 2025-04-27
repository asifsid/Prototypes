using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public static class ClaimValueTypes
	{
		private const string XmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

		public const string Base64Binary = "http://www.w3.org/2001/XMLSchema#base64Binary";

		public const string Boolean = "http://www.w3.org/2001/XMLSchema#boolean";

		public const string Date = "http://www.w3.org/2001/XMLSchema#date";

		public const string Datetime = "http://www.w3.org/2001/XMLSchema#dateTime";

		public const string Double = "http://www.w3.org/2001/XMLSchema#double";

		public const string HexBinary = "http://www.w3.org/2001/XMLSchema#hexBinary";

		public const string Integer = "http://www.w3.org/2001/XMLSchema#integer";

		public const string String = "http://www.w3.org/2001/XMLSchema#string";

		public const string Time = "http://www.w3.org/2001/XMLSchema#time";

		private const string XmlSignatureConstantsNamespace = "http://www.w3.org/2000/09/xmldsig#";

		public const string DsaKeyValue = "http://www.w3.org/2000/09/xmldsig#DSAKeyValue";

		public const string KeyInfo = "http://www.w3.org/2000/09/xmldsig#KeyInfo";

		public const string RsaKeyValue = "http://www.w3.org/2000/09/xmldsig#RSAKeyValue";

		private const string XQueryOperatorsNameSpace = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816";

		public const string DaytimeDuration = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816#dayTimeDuration";

		public const string YearMonthDuration = "http://www.w3.org/TR/2002/WD-xquery-operators-20020816#yearMonthDuration";

		private const string Xacml10Namespace = "urn:oasis:names:tc:xacml:1.0";

		public const string Rfc822Name = "urn:oasis:names:tc:xacml:1.0:data-type:rfc822Name";

		public const string X500Name = "urn:oasis:names:tc:xacml:1.0:data-type:x500Name";
	}
}
