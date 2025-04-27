namespace Microsoft.IdentityModel
{
	internal enum KERB_LOGON_SUBMIT_TYPE
	{
		KerbInteractiveLogon = 2,
		KerbSmartCardLogon = 6,
		KerbWorkstationUnlockLogon = 7,
		KerbSmartCardUnlockLogon = 8,
		KerbProxyLogon = 9,
		KerbTicketLogon = 10,
		KerbTicketUnlockLogon = 11,
		KerbS4ULogon = 12,
		KerbCertificateLogon = 13,
		KerbCertificateS4ULogon = 14,
		KerbCertificateUnlockLogon = 0xF
	}
}
