namespace Microsoft.IdentityModel.Diagnostics
{
	internal enum TraceCode
	{
		Diagnostics = 1,
		AppDomainUnload = 2,
		EventLog = 4,
		HandledException = 8,
		UnhandledException = 0x10
	}
}
