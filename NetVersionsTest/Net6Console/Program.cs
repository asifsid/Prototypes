// See https://aka.ms/new-console-template for more information
Console.WriteLine("From a Net 6 Console ...");

NetStandardLib.NetStandardComponent.Invoke();
Net6Lib.Net6Component.Invoke();
FullFrameworkLib.FFComponent.Invoke();

Console.ReadKey();
