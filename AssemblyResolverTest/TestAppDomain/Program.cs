namespace TestAppDomain
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Policy;
    using System.Text;
    using System.Threading.Tasks;

    //public interface IPlugin    
    //{   
    //    void Execute(IServiceProvider serviceProvider);
    //}
    class Program
    {
        /// <summary>
        /// Assembly cache file name format. 
        /// {0} is the pluginAssemblyid in "B" format and {1} is the plugin Assembly content hash code.
        /// </summary>
        public const string AssemblyCacheFileNameFormat = "{0}-{1}.dll";
        public const string AssemblyCachePathStatic = @"AssemblyCache";

        public const string LiveSkuDrive = @"D:\";
        public const string AppFolderSuffix = @"Microsoft\MSCRM\";
        public const string _containerReadWriteFolderPath = "";
        static void Main(string[] args)
        {
            #region Path is not legal
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyRedirects);
            //// First version assembly
            ////read content directly - used in prod
            //var firstassembly = Assembly.Load(ReadFileContents(@"D:\Amol\demo\mytestsecondplugin\bin\995480d9-6c01-4c94-ad02-c02ece456b93\Microsoft.Dynamics.ProjectServiceCoreSandbox.Plugins{f9035d48-4ee7-4f5c-b5e1-7f128dd243b6}.dll"));
            ////read assembly directly 
            ////var firstassembly = Assembly.Load(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Old\TestAssemblyLoad.dll");
            //// LoadFrom usage
            ////var firstassembly = Assembly.LoadFrom(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Old\TestAssemblyLoad.dll");
            //// LoadFile usage
            ////var firstassembly = Assembly.LoadFile(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Old\TestAssemblyLoad.dll");
            //var firstassemblytypes = firstassembly.GetTypes();
            //Console.WriteLine("Firstversion");
            //foreach (var t in firstassemblytypes)
            //{
            //    Console.WriteLine(t.FullName);
            //}

            //Console.WriteLine("Firstversion + resource read");
            //IPlugin plugin = GetPluginSandboxCodeUnit(firstassembly, "Microsoft.Dynamics.ProjectServiceCoreSandbox.Plugins.CreateProjectV1",
            //    "unsecure", "secure");
            ////IServiceProvider serviceProvider = new Moq
            //// plugin.Execute(new ServiceProvider());
            ////Type typeOfTest = firstassembly.GetType("Microsoft.Dynamics.ProjectServiceCoreSandbox.Plugins.CreateProjectV1");
            ////MethodInfo hello = typeOfTest.GetMethod("Execute");
            ////object interfaceObject = Activator.CreateInstance(typeOfTest);
            ////object test = hello.Invoke(interfaceObject, new object[] { "Bob" });
            //Console.WriteLine("\n");
            //Console.ReadKey();

            //Console.ReadKey();
            #endregion

            #region previous code
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyRedirects);
                //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
                // First version assembly
                //read content directly - used in prod
                var firstassembly = Assembly.Load(ReadFileContents(@"ResourceManagerTest.dll"));
                //read assembly directly 
                //var firstassembly = Assembly.Load(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Old\TestAssemblyLoad.dll");
                // LoadFrom usage
                //var firstassembly = Assembly.LoadFrom(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Old\TestAssemblyLoad.dll");
                // LoadFile usage
                //var firstassembly = Assembly.LoadFile(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Old\TestAssemblyLoad.dll");
                var firstassemblytypes = firstassembly.GetTypes();
                Console.WriteLine("Firstversion");
                foreach (var t in firstassemblytypes)
                {
                    Console.WriteLine(t.FullName);
                }

                Console.WriteLine("Firstversion + resource read");
                Type typeOfTest = firstassembly.GetType("ResourceManagerTest.ResourceManagerReadTest");
                MethodInfo hello = typeOfTest.GetMethod("Hello");
                object interfaceObject = Activator.CreateInstance(typeOfTest);
                object test = hello.Invoke(interfaceObject, new object[] { "Bob" });
                Console.WriteLine("\n");
                Console.ReadKey();

                //second version of assembly
                for (int i = 0; i < 1; i++)
                {
                    //read content directly
                    var secondassembly = Assembly.Load(ReadFileContents(@"ResourceManagerTest.dll"));
                    //read assembly directly 
                    //var secondassembly = Assembly.Load(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Debug\TestAssemblyLoad.dll");
                    // LoadFrom usage
                    //var secondassembly = Assembly.LoadFrom(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Debug\TestAssemblyLoad.dll");
                    // LoadFile usage
                    //var secondassembly = Assembly.LoadFile(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Debug\TestAssemblyLoad.dll");
                    var secondassemblytypes = secondassembly.GetTypes();
                    Console.WriteLine("SecondVersion");
                    foreach (var t in secondassemblytypes)
                    {
                        Console.WriteLine(t.FullName);
                    }
                    Console.WriteLine("\n");
                    //Console.ReadKey();

                    //Make an array for the list of assemblies.
                    Assembly[] assems = AppDomain.CurrentDomain.GetAssemblies();

                    //List the assemblies in the current application domain.
                    Console.WriteLine("List of assemblies loaded in current appdomain:");
                    foreach (Assembly assem in assems)
                        Console.WriteLine(assem.ToString());

                    Console.WriteLine("\n");


                    Console.WriteLine("SecondVersion + resource read");

                    Type secondtypeOfTest = secondassembly.GetType("ResourceManagerTest.ResourceManagerReadTest");
                    MethodInfo secondhello = secondtypeOfTest.GetMethod("Hello");
                    object secondinterfaceObject = Activator.CreateInstance(secondtypeOfTest);
                    object secondtest = secondhello.Invoke(secondinterfaceObject, new object[] { "Bob" });
                    Console.WriteLine("\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}{ex.StackTrace}");
            }
            #endregion

            Console.ReadKey();
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }

        private static readonly Dictionary<string, Version> _assemblyRedirects = new Dictionary<string, Version>()
        {
            { "Microsoft.Xrm.Sdk", new Version(9, 0, 0, 0) },
            { "Microsoft.Xrm.Sdk.Workflow", new Version(9, 0, 0, 0) },
            { "Microsoft.Xrm.Sdk.Data", new Version(9, 0, 0, 0) },
            { "Microsoft.Crm.Sdk", new Version(9, 0, 0, 0) },
            { "Microsoft.Crm.Sdk.Proxy", new Version(9, 0, 0, 0) },
            { "Microsoft.Xrm.Kernel.Contracts", new Version(9, 0, 0, 0) },
            { "System.Memory", new Version(4, 0, 1, 1) },//added for text.json
            { "System.Text.Encodings.Web", new Version(5,0,0,1)}, //added for text.json
            { "Microsoft.Bcl.AsyncInterfaces", new Version(5,0,0,0) }, //added for text.json
            { "System.Threading.Tasks.Extensions", new Version(4,2,0,1) }, //added for text.json            
            { "Microsoft.IdentityModel.Clients.ActiveDirectory", new Version(5, 1, 0, 0) }
        };


        public static IPlugin GetPluginSandboxCodeUnit(Assembly _assembly, string pluginTypeName, string pluginConfiguration, string pluginSecureConfig)
        {
            Type pluginType = _assembly.GetType(pluginTypeName);

            if (pluginType == null)
            {
                throw new InvalidOperationException($"The plug-in type could not be found in the plug-in assembly: {pluginTypeName}");
            }

            Type pluginTypeInterface;
            pluginTypeInterface = pluginType.GetInterface(nameof(IPlugin));
            if (pluginTypeInterface == null)
            {
                throw new ArgumentNullException($"The plug-in type does not implement Microsoft.Xrm.Sdk.IPlugin: {pluginTypeName}");
            }

            object[] args = null;
            ConstructorInfo constructorInfo;
            if ((constructorInfo = pluginType.GetConstructor(new Type[] { typeof(string), typeof(string) })) != null)
            {
                args = new object[] { pluginConfiguration, pluginSecureConfig };
            }
            else if ((constructorInfo = pluginType.GetConstructor(new Type[] { typeof(string) })) != null)
            {
                args = new object[] { pluginConfiguration };
            }

            IPlugin plugin = CreateSandboxCodeUnit(_assembly, constructorInfo, args, pluginTypeName);

            if (!(plugin is IPlugin pluginInterface))
            {
                throw new ArgumentNullException($"Could not create the plug-in interface from the plug-in assembly: {pluginTypeName}");
            }

            return pluginInterface;
        }

        /// <summary>
        /// Create an instance of the plugin
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <param name="args"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static IPlugin CreateSandboxCodeUnit(Assembly _assembly, ConstructorInfo constructorInfo, object[] args, string typeName)
        {
            IPlugin sandboxCodeUnit;
            if (constructorInfo != null)
            {
                sandboxCodeUnit = constructorInfo.Invoke(args) as IPlugin;
            }
            else
            {
                sandboxCodeUnit = _assembly.CreateInstance(typeName) as IPlugin;
            }

            return sandboxCodeUnit;
        }


        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);
            if (_assemblyRedirects.TryGetValue(name.Name, out Version targetVersion))
            {
                name.Version = targetVersion;
                // return Assembly.Load(name);
                // var name1 = args.Name.Split(',')[0];
                return Assembly.Load(ReadFileContents($"D:\\Amol\\demo\\PluginTest\\bin\\Debug\\net462\\{name.Name}.dll"));
            }

            //if (args.RequestingAssembly != null)
            //{
            //    return Assembly.LoadFile(@"D:\Amol\demo\mytestsecondplugin\TestAssemblyLoad\bin\Debug\TestAssemblyLoad.dll");
            //}
            return null;
        }


        /// <summary>
		/// Root files path for sandbox
		/// Return Local App Data if UseLocalAppDataAsRootFilesPath is in registry
		/// Return SandboxRootFilesPath if it is in registry
		/// Return LiveSkuDrive if it is available and sku is live
		/// Return Local App Data otherwise
		/// </summary>
		public static readonly Lazy<string> SandboxRootFilesPath = new Lazy<string>(() =>
        {
            return LiveSkuDrive;
        });


        /// <summary>
		/// Returns the Sandbox files path.
		/// </summary>
		public static string SandboxFilesPath()
        {
            string sandboxRootFilesPath = SandboxRootFilesPath.Value;
            return Path.Combine(sandboxRootFilesPath, AppFolderSuffix);
        }

        /// <summary>
		/// Returns the Sandbox assembly cache path.
		/// </summary>
		public static string AssemblyCachePath(string sandboxFilesPath = null)
        {
            sandboxFilesPath = string.IsNullOrEmpty(sandboxFilesPath) ? SandboxFilesPath() : sandboxFilesPath;

            var assemblyCachePath = Path.Combine(sandboxFilesPath, AssemblyCachePathStatic);

            return assemblyCachePath;
        }

        internal static Assembly AssemblyRedirects(object sender, ResolveEventArgs args)
        {
            //try
            //{
                if (args != null)
                {
                    AssemblyName name = new AssemblyName(args.Name);
                    if (_assemblyRedirects.TryGetValue(name.Name, out Version targetVersion))
                    {
                        name.Version = targetVersion;
                        return Assembly.Load(name);
                    }

                    if (args.RequestingAssembly != null)
                    {
                        var requestingAssembly = args.RequestingAssembly;
                        var folder = Path.GetDirectoryName(requestingAssembly.Location);
                        var _assemblyCachePath = AssemblyCachePath(_containerReadWriteFolderPath);
                        if (folder.StartsWith(_assemblyCachePath, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Extract assembly name from fullName.
                            var assemblyFile = Path.Combine(folder, $"{args.Name?.Split(',').First()}.dll");
                            if (File.Exists(assemblyFile))
                            {
                                return Assembly.LoadFile(assemblyFile);
                            }
                            return null;
                        }
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"assembly resolver {ex.ToString()}");
            //}
            return null;
        }

        /// <summary>
        /// Reads assembly from disk
        /// </summary>
        /// <param name="assemblyPath">Path to assembly file</param>
        /// <returns>Byte array of assembly</returns>
        private static byte[] ReadFileContents(string assemblyPath)
        {
            byte[] assemblyContents;
            using (FileStream fs = File.Open(assemblyPath, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int numBytesToRead = Convert.ToInt32(fs.Length);
                assemblyContents = new byte[(numBytesToRead)];

                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fs.Read(assemblyContents, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                    {
                        break;
                    }

                    numBytesRead += n;
                    numBytesToRead -= n;
                }

                if (numBytesRead != fs.Length)
                {
                    string unableToReadMessage = string.Format(CultureInfo.InvariantCulture, "SandboxAppDomain.LoadPluginAssembly: Failed to read all assembly bytes at {0}. Shutting down workerprocess.", assemblyPath);
                    throw new InvalidOperationException(unableToReadMessage);
                }
            }

            return assemblyContents;
        }


    }
}
