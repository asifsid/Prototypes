using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JNav
{
    class Root
    {
        private JNavNode _root;

        [JsonProperty("models")]
        public Model[] Models { get; set; }
    }

    class Model
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("children")]
        public Child[] Children { get; set; }
    }

    class Child
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
