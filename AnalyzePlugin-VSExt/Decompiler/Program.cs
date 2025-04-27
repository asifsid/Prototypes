namespace Decompiler
{
    using ICSharpCode.Decompiler;
    using ICSharpCode.Decompiler.CSharp;
    using ICSharpCode.Decompiler.Metadata;
    using Microsoft.Build.Locator;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.MSBuild;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        static void DecompileAsProject(string assemblyFileName, string outputPath)
        {
            var decompiler = new WholeProjectDecompiler() { Settings = new DecompilerSettings(LanguageVersion.Latest)
            {
                ThrowOnAssemblyResolveErrors = false,
                RemoveDeadCode = true,
                RemoveDeadStores = true
            } };

            var module = new PEFile(assemblyFileName);
            var resolver = new UniversalAssemblyResolver(assemblyFileName, false, module.Reader.DetectTargetFrameworkId());
            
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }

            Directory.CreateDirectory(outputPath);

            decompiler.AssemblyResolver = resolver;
            decompiler.DecompileProject(module, outputPath);
        }

        private static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
        {
            Console.WriteLine("Multiple installs of MSBuild detected please select one:");
            for (int i = 0; i < visualStudioInstances.Length; i++)
            {
                Console.WriteLine($"Instance {i + 1}");
                Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
                Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
                Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
            }

            while (true)
            {
                var userResponse = Console.ReadLine();
                if (int.TryParse(userResponse, out int instanceNumber) &&
                    instanceNumber > 0 &&
                    instanceNumber <= visualStudioInstances.Length)
                {
                    return visualStudioInstances[instanceNumber - 1];
                }
                Console.WriteLine("Input not accepted, try again.");
            }
        }

        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine($"Missing assembly name");
                    return;
                }

                const string dllExt = ".dll";
                
                var assemblyName = args[0].EndsWith(dllExt) ? Path.GetFileNameWithoutExtension(args[0]) : args[0];
                var assemblyFile = assemblyName + dllExt;
                
                var outputPath = $"{assemblyName}-Source";
                var projectPath = $"{outputPath}/{assemblyName}.csproj";

                DecompileAsProject(assemblyFile, outputPath);

                var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                var instance = visualStudioInstances.Length == 1
                    // If there is only one instance of MSBuild on this machine, set that as the one to use.
                    ? visualStudioInstances[0]
                    // Handle selecting the version of MSBuild you want to use.
                    : SelectVisualStudioInstance(visualStudioInstances);

                Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

                // NOTE: Be sure to register an instance with the MSBuildLocator 
                //       before calling MSBuildWorkspace.Create()
                //       otherwise, MSBuildWorkspace won't MEF compose.
                MSBuildLocator.RegisterInstance(instance);

                using var workspace = MSBuildWorkspace.Create();

                // Print message for WorkspaceFailed event to help diagnosing project load failures.
                workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

                Console.WriteLine($"Loading project '{projectPath}'");

                // Attach progress reporter so we print projects as they are loaded.
                var project = await workspace.OpenProjectAsync(projectPath);

                foreach (var doc in project.Documents)
                {
                    var tree = await doc.GetSyntaxTreeAsync();
                    foreach (var node in tree.GetRoot().DescendantNodes())
                    {
                        if (node.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.LockStatement))
                        {
                            Console.WriteLine(node.ToString());
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void Foo(IServiceProvider services)
        {
            services.
        }
    }

    public interface IServiceProvider
    {

    }

    public interface ILogger
    {
        void Log(string s);
    }

    class Logger : ILogger
    {
        void ILogger.Log(string s)
        {
            throw new NotImplementedException();
        }
    }
    public static class Extensions
    {
        public static ILogger GetLogger(this IServiceProvider provider)
        {
            return new Logger();
        }
    }

}

