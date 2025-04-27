
*******************************
To test the current behavior where the dependencies are loaded in "Default" context:

Open project properties for "DependentAssemblyTest" project and define the following in the "Conditional Compilation Symbols":

Default_LoadContext


This will load the HostProcess project as a direct reference and will load the AssemblyC in the default load context. 

 - When AssemblyA and AssemblyB is trying to load AssemblyC, it does not fire the AssemblyResolve event and will try to use V3 of AssemblyC. 
   This will fail to find the methods either Assembly A or Assmbly B is looking for.


*******************************

To test the new behavior where the dependencies are loaded in "LoadFrom" context:

Open project properties for "DependentAssemblyTest" project and remove "Conditional Compilation Symbols"

This will not have a direct reference to HostProcess and will load the HostProcess dll with LoadFrom.
 
 - Means the Host Process and subsequent dependency of AssemblyC will be loaded LoadFrom context.

 - When AssemblyA and AssemblyB is trying to load AssemblyC, AsseblyC is not found in Default Load Context 
 - It will fire the AssemblyResolve event and we will be able to provide the right version of the assembly. 
 - This will be able to find the methods that Assembly A and Assembly B are looking for, including the extension methods in V1 and V2 of AssemblyC.

 *******************************

 Layout:

 AssemblyC
 ---------
	Uses the conditional compilation symbols to define the version of the assembly. V1, V2, V3
	Depending on the version, it will copy the dll to (solution-root-folder)\C_V1, (solution-root-folder)\C_V2, (solution-root-folder)\C_V3
	The ClassC has different methods in each version of the assembly.
 
AssemblyA
---------
	References AssemblyC from SolutionRoot\C_V1
	Calls the V1 methods of AssemblyC

AssemblyB
---------
	References AssemblyC from SolutionRoot\C_V2
	Calls the V2 methods of AssemblyC	

 HostProcess
 -----------
	References AssemblyC from SolutionRoot\C_V3


DependentAssemblyTest
---------------------
	Uses the conditional compilation symbol "Default_LoadContext" to define the load behavior of the HostProcess.
	Removing the conditional compilation symbol will load the HostProcess in LoadFrom context.