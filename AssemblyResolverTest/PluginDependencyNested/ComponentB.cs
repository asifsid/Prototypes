namespace PLuginDependencyNested
{
    using PluginDependencyNested;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;

    public class ComponentB
    {
        public string GetVersion()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(this.GetType().Assembly.Location);

            var doc = JsonDocument.Parse(Content.VersionJson);
            var root = doc.RootElement;
            return $"{root.GetProperty("name").GetString()} : {root.GetProperty("version").GetString()}";
        }

        public string GetJsonTextVersion()
        {
            Console.WriteLine($"Loading from: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            return $"(Json.TextVersion: {typeof(JsonSerializer).Assembly.GetName().Version})";
        }


    }
}
