namespace MockWorker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Web.Handlers;

    internal static class AppDomainEventHandlers
    {
        private static readonly Dictionary<string, Version> _assemblyRedirects = new Dictionary<string, Version>()
        {
            //{ "Microsoft.Xrm.Sdk", new Version(9, 0, 0, 0) },
            //{ "Microsoft.Xrm.Sdk.Workflow", new Version(9, 0, 0, 0) },
            //{ "Microsoft.Xrm.Sdk.Data", new Version(9, 0, 0, 0) },
            //{ "Microsoft.Crm.Sdk", new Version(9, 0, 0, 0) },
            //{ "Microsoft.Crm.Sdk.Proxy", new Version(9, 0, 0, 0) },
            //{ "Microsoft.IdentityModel.Clients.ActiveDirectory", new Version(5, 1, 0, 0) }
        };

        public static void Init()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyRedirects;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ReflectionOnlyAssemblyResolveHandler;
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            using (ColorContext.Forground(ConsoleColor.Red))
            {
                Console.WriteLine($"Encountered unhandled exception.");

                if (e.ExceptionObject is Exception exception)
                {
                    Console.WriteLine($"Exception:{exception}");
                }

                Console.WriteLine($"IsTerminating: {e.IsTerminating}");
            }
        }

        private static Assembly AssemblyRedirects(object sender, ResolveEventArgs args)
        {
            Console.WriteLine(args.ToString());

            using (ColorContext.Forground(ConsoleColor.DarkGray))
            {
                Assembly assembly = null;

                Console.WriteLine($"Assembly Resolver called.");
                Console.WriteLine($"Requesting Assembly ==> {args.RequestingAssembly?.GetName().Name ?? "(UNKNOWN)"}");
                Console.WriteLine($"Requesting Asm Path ==> {args.RequestingAssembly?.Location}");
                Console.WriteLine($"Requested Assembly  <== {args.Name}");
                if (args != null)
                {
                    AssemblyName name = new AssemblyName(args.Name);
                    using (ColorContext.Forground(ConsoleColor.DarkCyan))
                    {
                        //   throw new Exception("Fake Exception");

                        if (_assemblyRedirects.TryGetValue(name.Name, out Version targetVersion))
                        {
                            name.Version = targetVersion;
                            Console.WriteLine($"Redirecting to predefined version: {name.FullName}.");

                            assembly = Assembly.Load(name);
                        }
                        else if (args.RequestingAssembly != null)
                        {
                            var dir = Directory.GetParent(args.RequestingAssembly.Location);
                            if (dir.Parent.Name == "Plugins")
                            {
                                // Extract assembly name from fullName.
                                var assemblyFile = Path.Combine(dir.FullName, name.Name + ".dll");
                                if (File.Exists(assemblyFile))
                                {
                                    Console.WriteLine($"Loading Assembly <== {assemblyFile}");
                                    assembly = Assembly.LoadFile(assemblyFile);
                                }
                                else
                                {
                                    throw new Exception($"Assembly not found : {assemblyFile}");   
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Non-plugin-path.");
                            }
                        }
                    }
                }

                return assembly;
            }
        }

        private static Assembly ReflectionOnlyAssemblyResolveHandler(object sender, ResolveEventArgs args)
        {
            if (args != null)
            {
                AssemblyName name = new AssemblyName(args.Name);
                if (_assemblyRedirects.TryGetValue(name.Name, out Version targetVersion))
                {
                    name.Version = targetVersion;
                }

                return Assembly.ReflectionOnlyLoad(name.FullName);
            }

            return null;
        }
    }
}
