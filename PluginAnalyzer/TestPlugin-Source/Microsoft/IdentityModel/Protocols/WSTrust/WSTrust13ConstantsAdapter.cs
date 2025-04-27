namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	internal class WSTrust13ConstantsAdapter : WSTrustConstantsAdapter
	{
		internal class WSTrust13ElementNames : WSTrustElementNames
		{
			internal string KeyWrapAlgorithm = "KeyWrapAlgorithm";

			internal string SecondaryParameters = "SecondaryParameters";

			internal string RequestSecurityTokenResponseCollection = "RequestSecurityTokenResponseCollection";

			internal string ValidateTarget = "ValidateTarget";
		}

		internal class WSTrust13Actions : WSTrustActions
		{
			internal string CancelResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal";

			internal string IssueResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal";

			internal string RenewResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal";

			internal string ValidateResponseCollection = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal";

			internal WSTrust13Actions()
			{
				Cancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel";
				CancelResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel";
				Issue = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue";
				IssueResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue";
				Renew = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew";
				RenewResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew";
				RequestSecurityContextToken = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/SCT";
				RequestSecurityContextTokenCancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/SCT-Cancel";
				RequestSecurityContextTokenResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/SCT";
				RequestSecurityContextTokenResponseCancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/SCT-Cancel";
				Validate = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate";
				ValidateResponse = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate";
			}
		}

		internal class WSTrust13ComputedKeyAlgorithm : WSTrustComputedKeyAlgorithm
		{
			internal WSTrust13ComputedKeyAlgorithm()
			{
				Psha1 = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/CK/PSHA1";
			}
		}

		internal class WSTrust13KeyTypes : WSTrustKeyTypes
		{
			internal WSTrust13KeyTypes()
			{
				Asymmetric = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/PublicKey";
				Bearer = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer";
				Symmetric = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/SymmetricKey";
			}
		}

		internal class WSTrust13RequestTypes : WSTrustRequestTypes
		{
			internal WSTrust13RequestTypes()
			{
				Cancel = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Cancel";
				Issue = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Issue";
				Renew = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Renew";
				Validate = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Validate";
			}
		}

		private static WSTrust13ConstantsAdapter _instance;

		private static WSTrust13ElementNames _trust13ElementNames;

		private static WSTrust13Actions _trust13ActionNames;

		private static WSTrust13ComputedKeyAlgorithm _trust13ComputedKeyAlgorithm;

		private static WSTrust13KeyTypes _trust13KeyTypes;

		private static WSTrust13RequestTypes _trust13RequestTypes;

		internal static WSTrust13ConstantsAdapter Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new WSTrust13ConstantsAdapter();
				}
				return _instance;
			}
		}

		internal override WSTrustActions Actions
		{
			get
			{
				if (_trust13ActionNames == null)
				{
					_trust13ActionNames = new WSTrust13Actions();
				}
				return _trust13ActionNames;
			}
		}

		internal override WSTrustComputedKeyAlgorithm ComputedKeyAlgorithm
		{
			get
			{
				if (_trust13ComputedKeyAlgorithm == null)
				{
					_trust13ComputedKeyAlgorithm = new WSTrust13ComputedKeyAlgorithm();
				}
				return _trust13ComputedKeyAlgorithm;
			}
		}

		internal override WSTrustElementNames Elements
		{
			get
			{
				if (_trust13ElementNames == null)
				{
					_trust13ElementNames = new WSTrust13ElementNames();
				}
				return _trust13ElementNames;
			}
		}

		internal override WSTrustKeyTypes KeyTypes
		{
			get
			{
				if (_trust13KeyTypes == null)
				{
					_trust13KeyTypes = new WSTrust13KeyTypes();
				}
				return _trust13KeyTypes;
			}
		}

		internal override WSTrustRequestTypes RequestTypes
		{
			get
			{
				if (_trust13RequestTypes == null)
				{
					_trust13RequestTypes = new WSTrust13RequestTypes();
				}
				return _trust13RequestTypes;
			}
		}

		protected WSTrust13ConstantsAdapter()
		{
			NamespaceURI = "http://docs.oasis-open.org/ws-sx/ws-trust/200512";
			Prefix = "trust";
		}
	}
}
