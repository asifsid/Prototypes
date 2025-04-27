using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace JNav
{
    class SchemaGenerator
    {
        public static void Generate(string path)
        {
            var schema = File.ReadAllText(path);

            var jschema = JSchema.Parse(schema);

            if (jschema.Type == JSchemaType.Object)
            {
                
            }
        }
    }
}
