using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens
{
	internal class SamlAttributeKeyComparer : IEqualityComparer<SamlAttributeKeyComparer.AttributeKey>
	{
		public class AttributeKey
		{
			private int _hashCode;

			private string _name;

			private string _valueType;

			private string _originalIssuer;

			internal string Name => _name;

			internal string ValueType => _valueType;

			internal string OriginalIssuer => _originalIssuer;

			public AttributeKey(Saml11Attribute attribute)
			{
				if (attribute == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("attribute");
				}
				_name = attribute.Name;
				_valueType = attribute.AttributeValueXsiType;
				_originalIssuer = attribute.OriginalIssuer ?? string.Empty;
				ComputeHashCode();
			}

			public AttributeKey(Saml2Attribute attribute)
			{
				if (attribute == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("attribute");
				}
				_name = attribute.Name;
				_valueType = attribute.AttributeValueXsiType;
				_originalIssuer = attribute.OriginalIssuer ?? string.Empty;
				ComputeHashCode();
			}

			public override int GetHashCode()
			{
				return _hashCode;
			}

			private void ComputeHashCode()
			{
				_hashCode = _name.GetHashCode();
				_hashCode ^= _valueType.GetHashCode();
				_hashCode ^= _originalIssuer.GetHashCode();
			}
		}

		public bool Equals(AttributeKey x, AttributeKey y)
		{
			if (x.Name.Equals(y.Name, StringComparison.Ordinal) && x.ValueType.Equals(y.ValueType, StringComparison.Ordinal))
			{
				return x.OriginalIssuer.Equals(y.OriginalIssuer, StringComparison.Ordinal);
			}
			return false;
		}

		public int GetHashCode(AttributeKey obj)
		{
			return obj.GetHashCode();
		}
	}
}
