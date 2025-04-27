namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	internal class WSTrustFeb2005ConstantsAdapter : WSTrustConstantsAdapter
	{
		internal class WSTrustFeb2005Actions : WSTrustActions
		{
			internal WSTrustFeb2005Actions()
			{
				Cancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel";
				CancelResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel";
				Issue = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue";
				IssueResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue";
				Renew = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew";
				RenewResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew";
				RequestSecurityContextToken = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT";
				RequestSecurityContextTokenCancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/SCT-Cancel";
				RequestSecurityContextTokenResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT";
				RequestSecurityContextTokenResponseCancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/SCT-Cancel";
				Validate = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate";
				ValidateResponse = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate";
			}
		}

		internal class WSTrustFeb2005ComputedKeyAlgorithm : WSTrustComputedKeyAlgorithm
		{
			internal WSTrustFeb2005ComputedKeyAlgorithm()
			{
				Psha1 = "http://schemas.xmlsoap.org/ws/2005/02/trust/CK/PSHA1";
			}
		}

		internal class WSTrustFeb2005KeyTypes : WSTrustKeyTypes
		{
			internal WSTrustFeb2005KeyTypes()
			{
				Asymmetric = "http://schemas.xmlsoap.org/ws/2005/02/trust/PublicKey";
				Bearer = "http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey";
				Symmetric = "http://schemas.xmlsoap.org/ws/2005/02/trust/SymmetricKey";
			}
		}

		internal class WSTrustFeb2005RequestTypes : WSTrustRequestTypes
		{
			internal WSTrustFeb2005RequestTypes()
			{
				Cancel = "http://schemas.xmlsoap.org/ws/2005/02/trust/Cancel";
				Issue = "http://schemas.xmlsoap.org/ws/2005/02/trust/Issue";
				Renew = "http://schemas.xmlsoap.org/ws/2005/02/trust/Renew";
				Validate = "http://schemas.xmlsoap.org/ws/2005/02/trust/Validate";
			}
		}

		private static WSTrustFeb2005ConstantsAdapter _instance;

		private static WSTrustFeb2005Actions _trustFeb2005Actions;

		private static WSTrustFeb2005ComputedKeyAlgorithm _trustFeb2005ComputedKeyAlgorithm;

		private static WSTrustFeb2005KeyTypes _trustFeb2005KeyTypes;

		private static WSTrustFeb2005RequestTypes _trustFeb2005RequestTypes;

		internal static WSTrustFeb2005ConstantsAdapter Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new WSTrustFeb2005ConstantsAdapter();
				}
				return _instance;
			}
		}

		internal override WSTrustActions Actions
		{
			get
			{
				if (_trustFeb2005Actions == null)
				{
					_trustFeb2005Actions = new WSTrustFeb2005Actions();
				}
				return _trustFeb2005Actions;
			}
		}

		internal override WSTrustComputedKeyAlgorithm ComputedKeyAlgorithm
		{
			get
			{
				if (_trustFeb2005ComputedKeyAlgorithm == null)
				{
					_trustFeb2005ComputedKeyAlgorithm = new WSTrustFeb2005ComputedKeyAlgorithm();
				}
				return _trustFeb2005ComputedKeyAlgorithm;
			}
		}

		internal override WSTrustKeyTypes KeyTypes
		{
			get
			{
				if (_trustFeb2005KeyTypes == null)
				{
					_trustFeb2005KeyTypes = new WSTrustFeb2005KeyTypes();
				}
				return _trustFeb2005KeyTypes;
			}
		}

		internal override WSTrustRequestTypes RequestTypes
		{
			get
			{
				if (_trustFeb2005RequestTypes == null)
				{
					_trustFeb2005RequestTypes = new WSTrustFeb2005RequestTypes();
				}
				return _trustFeb2005RequestTypes;
			}
		}

		protected WSTrustFeb2005ConstantsAdapter()
		{
			NamespaceURI = "http://schemas.xmlsoap.org/ws/2005/02/trust";
			Prefix = "t";
		}
	}
}
