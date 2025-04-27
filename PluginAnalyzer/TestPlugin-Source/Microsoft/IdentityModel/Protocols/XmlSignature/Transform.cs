using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal abstract class Transform
	{
		public abstract string Algorithm { get; }

		public virtual bool NeedsInclusiveContext => false;

		public abstract object Process(object input);

		public abstract byte[] ProcessAndDigest(object input, string digestAlgorithm);

		public abstract void ReadFrom(XmlDictionaryReader reader);

		public abstract void WriteTo(XmlDictionaryWriter writer);
	}
}
