using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public class ExceptionMapper
	{
		internal const string SoapSenderFaultCode = "Sender";

		private Dictionary<Type, ConstructorInfo> _exceptionMap = new Dictionary<Type, ConstructorInfo>();

		public Dictionary<Type, ConstructorInfo> ExceptionMap => _exceptionMap;

		public ExceptionMapper()
		{
			_exceptionMap.Add(typeof(AuthenticationBadElementsException), typeof(AuthenticationBadElementsFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(BadRequestException), typeof(BadRequestFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(Microsoft.IdentityModel.SecurityTokenService.FailedAuthenticationException), typeof(Microsoft.IdentityModel.Protocols.WSTrust.FailedAuthenticationFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(ExpiredDataException), typeof(ExpiredDataFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(InvalidRequestException), typeof(InvalidRequestFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(InvalidScopeException), typeof(InvalidScopeFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(Microsoft.IdentityModel.SecurityTokenService.InvalidSecurityTokenException), typeof(Microsoft.IdentityModel.Protocols.WSTrust.InvalidSecurityTokenFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(InvalidTimeRangeException), typeof(InvalidTimeRangeFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(RenewNeededException), typeof(RenewNeededFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(RequestFailedException), typeof(RequestFailedFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(UnableToRenewException), typeof(UnableToRenewFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(UnsupportedTokenTypeBadRequestException), typeof(BadRequestFaultException).GetConstructor(new Type[2]
			{
				typeof(string),
				typeof(string)
			}));
			_exceptionMap.Add(typeof(AlreadySignedInException), typeof(AlreadySignedInFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(IssuerNameNotSupportedException), typeof(IssuerNameNotSupportedFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(NeedFresherCredentialsException), typeof(NeedFresherCredentialsFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(NoMatchInScopeException), typeof(NoMatchInScopeFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(NoPseudonymInScopeException), typeof(NoPseudonymInScopeFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(NotSignedInException), typeof(NotSignedInFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(RstParameterNotAcceptedException), typeof(RstParameterNotAcceptedFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(UnsupportedClaimsDialectException), typeof(UnsupportedClaimsDialectFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(UnsupportedEncodingException), typeof(UnsupportedEncodingFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(FailedRequiredClaimsException), typeof(FailedRequiredClaimsFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(InformationCardRefreshRequiredException), typeof(InformationCardRefreshRequiredFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(InternalErrorException), typeof(InternalErrorFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(InvalidInputException), typeof(InvalidInputFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(InvalidProofKeyException), typeof(InvalidProofKeyFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(MissingAppliesToException), typeof(MissingAppliesToFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(UnauthorizedRequestException), typeof(UnauthorizedRequestFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(UnknownInformationCardReferenceException), typeof(UnknownInformationCardReferenceFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(UnsupportedSignatureFormatException), typeof(UnsupportedSignatureFormatFaultException).GetConstructor(new Type[1] { typeof(string) }));
			_exceptionMap.Add(typeof(Microsoft.IdentityModel.Tokens.FailedAuthenticationException), typeof(Microsoft.IdentityModel.Tokens.FailedAuthenticationFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(FailedCheckException), typeof(FailedCheckFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(InvalidSecurityException), typeof(InvalidSecurityFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException), typeof(Microsoft.IdentityModel.Tokens.InvalidSecurityTokenFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(MessageExpiredException), typeof(MessageExpiredFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(SecurityTokenUnavailableException), typeof(SecurityTokenUnavailableFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(SecurityTokenValidationException), typeof(Microsoft.IdentityModel.Tokens.FailedAuthenticationFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(UnsupportedSecurityTokenException), typeof(UnsupportedSecurityTokenFaultException).GetConstructor(Type.EmptyTypes));
			_exceptionMap.Add(typeof(UnsupportedAlgorithmException), typeof(UnsupportedAlgorithmFaultException).GetConstructor(Type.EmptyTypes));
		}

		public virtual FaultException FromException(Exception ex)
		{
			return FromException(ex, string.Empty, string.Empty);
		}

		public virtual FaultException FromException(Exception ex, string soapNamespace, string trustNamespace)
		{
			FaultException ex2 = ex as FaultException;
			if (ex2 != null)
			{
				return ex2;
			}
			try
			{
				ConstructorInfo constructorInfo = null;
				foreach (Type key in _exceptionMap.Keys)
				{
					if (key.IsAssignableFrom(ex.GetType()))
					{
						constructorInfo = _exceptionMap[key];
						break;
					}
				}
				if ((object)constructorInfo != null)
				{
					if (constructorInfo.GetParameters().GetLength(0) == 2)
					{
						WSTrustConstantsAdapter constantsAdapter = WSTrustConstantsAdapter.GetConstantsAdapter(trustNamespace);
						if (constantsAdapter == null)
						{
							return null;
						}
						return constructorInfo.Invoke(new object[2] { soapNamespace, trustNamespace }) as FaultException;
					}
					if (constructorInfo.GetParameters().GetLength(0) == 1)
					{
						if (string.IsNullOrEmpty(soapNamespace))
						{
							return null;
						}
						return constructorInfo.Invoke(new object[1] { soapNamespace }) as FaultException;
					}
					return constructorInfo.Invoke(null) as FaultException;
				}
				return ex2;
			}
			catch
			{
				return null;
			}
		}

		public virtual bool HandleSecurityTokenProcessingException(Exception ex)
		{
			if (DiagnosticUtil.IsFatal(ex))
			{
				return false;
			}
			if (ex is FaultException)
			{
				return false;
			}
			FaultException ex2 = FromException(ex);
			if (ex2 != null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(ex2);
			}
			return false;
		}
	}
}
