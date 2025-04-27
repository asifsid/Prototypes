namespace TestConsole
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using static System.Console;

    static class OverrideDirectoryTest
    {
        public static void Run()
        {
            const string configRoot = @"..\..\ConfigFiles";

            foreach (var file in GetConfigFiles(configRoot))
            {
                WriteLine(file);
            }
        }

        static IEnumerable<string> GetConfigFiles(string root)
        {
            var files = Directory.EnumerateFiles(root, "*.ini").Select(path => Path.GetFileName(path));

            var includeFile = Path.Combine(root, "include.txt");

            if (File.Exists(includeFile))
            {
                foreach (var dir in File.ReadAllLines(includeFile))
                {
                    if (Directory.Exists(dir))
                    {
                        files = files.Concat(GetConfigFiles(dir));
                    }
                }
            }

            return files;
        }
    }
}
