using System;

namespace Newtonsoft.Json.Schema
{
	[Obsolete("JSON Schema validation has been moved to its own package. See http://www.newtonsoft.com/jsonschema for more details.")]
	public enum UndefinedSchemaIdHandling
	{
		None,
		UseTypeName,
		UseAssemblyQualifiedName
	}
}
