using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class TransformChain
	{
		private string _prefix = "ds";

		private IList<Transform> _transforms = new List<Transform>(2);

		public int TransformCount => _transforms.Count;

		public Transform this[int index] => _transforms[index];

		public bool NeedsInclusiveContext
		{
			get
			{
				for (int i = 0; i < TransformCount; i++)
				{
					if (this[i].NeedsInclusiveContext)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void Add(Transform transform)
		{
			_transforms.Add(transform);
		}

		public void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
		{
			reader.MoveToStartElement("Transforms", "http://www.w3.org/2000/09/xmldsig#");
			_prefix = reader.Prefix;
			reader.Read();
			while (reader.IsStartElement("Transform", "http://www.w3.org/2000/09/xmldsig#"))
			{
				string attribute = reader.GetAttribute("Algorithm", null);
				Transform transform = transformFactory.CreateTransform(attribute);
				transform.ReadFrom(reader);
				Add(transform);
			}
			reader.MoveToContent();
			reader.ReadEndElement();
			if (TransformCount == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6017")));
			}
		}

		public byte[] TransformToDigest(object data, string digestMethod)
		{
			for (int i = 0; i < TransformCount - 1; i++)
			{
				data = this[i].Process(data);
			}
			return this[TransformCount - 1].ProcessAndDigest(data, digestMethod);
		}

		public void WriteTo(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement(_prefix, "Transforms", "http://www.w3.org/2000/09/xmldsig#");
			for (int i = 0; i < TransformCount; i++)
			{
				this[i].WriteTo(writer);
			}
			writer.WriteEndElement();
		}
	}
}
