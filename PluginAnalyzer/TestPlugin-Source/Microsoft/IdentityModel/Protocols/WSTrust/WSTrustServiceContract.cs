using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using System.Threading;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Threading;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ServiceBehavior(Name = "SecurityTokenService", Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/securitytokenservice", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ComVisible(true)]
	public class WSTrustServiceContract : IWSTrustFeb2005SyncContract, IWSTrust13SyncContract, IWSTrustFeb2005AsyncContract, IWSTrust13AsyncContract, IWsdlExportExtension, IContractBehavior
	{
		internal class ProcessCoreAsyncResult : AsyncResult
		{
			private WSTrustServiceContract _trustServiceContract;

			private DispatchContext _dispatchContext;

			private MessageVersion _messageVersion;

			private WSTrustResponseSerializer _responseSerializer;

			private WSTrustSerializationContext _serializationContext;

			public WSTrustServiceContract TrustServiceContract => _trustServiceContract;

			public DispatchContext DispatchContext => _dispatchContext;

			public MessageVersion MessageVersion => _messageVersion;

			public WSTrustResponseSerializer ResponseSerializer => _responseSerializer;

			public WSTrustSerializationContext SerializationContext => _serializationContext;

			public ProcessCoreAsyncResult(WSTrustServiceContract contract, DispatchContext dispatchContext, MessageVersion messageVersion, WSTrustResponseSerializer responseSerializer, WSTrustSerializationContext serializationContext, AsyncCallback asyncCallback, object asyncState)
				: base(asyncCallback, asyncState)
			{
				if (contract == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contract");
				}
				if (dispatchContext == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("dispatchContext");
				}
				if (responseSerializer == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
				}
				if (serializationContext == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serializationContext");
				}
				_trustServiceContract = contract;
				_dispatchContext = dispatchContext;
				_messageVersion = messageVersion;
				_responseSerializer = responseSerializer;
				_serializationContext = serializationContext;
				contract.BeginDispatchRequest(dispatchContext, OnDispatchRequestCompleted, null);
			}

			public new static System.ServiceModel.Channels.Message End(IAsyncResult ar)
			{
				AsyncResult.End(ar);
				ProcessCoreAsyncResult processCoreAsyncResult = ar as ProcessCoreAsyncResult;
				if (processCoreAsyncResult == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2004", typeof(ProcessCoreAsyncResult), ar.GetType()));
				}
				return System.ServiceModel.Channels.Message.CreateMessage(OperationContext.Current.RequestContext.RequestMessage.Version, processCoreAsyncResult.DispatchContext.ResponseAction, new WSTrustResponseBodyWriter(processCoreAsyncResult.DispatchContext.ResponseMessage, processCoreAsyncResult.ResponseSerializer, processCoreAsyncResult.SerializationContext));
			}

			private void OnDispatchRequestCompleted(IAsyncResult ar)
			{
				try
				{
					_dispatchContext = _trustServiceContract.EndDispatchRequest(ar);
					Complete(completedSynchronously: false);
				}
				catch (Exception exception)
				{
					if (DiagnosticUtil.ExceptionUtil.IsFatal(exception))
					{
						throw;
					}
					Complete(completedSynchronously: false, exception);
				}
			}
		}

		internal class DispatchRequestAsyncResult : AsyncResult
		{
			private DispatchContext _dispatchContext;

			public DispatchContext DispatchContext => _dispatchContext;

			public DispatchRequestAsyncResult(DispatchContext dispatchContext, AsyncCallback asyncCallback, object asyncState)
				: base(asyncCallback, asyncState)
			{
				_dispatchContext = dispatchContext;
				IClaimsPrincipal principal = dispatchContext.Principal;
				RequestSecurityToken requestSecurityToken = dispatchContext.RequestMessage as RequestSecurityToken;
				Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService securityTokenService = dispatchContext.SecurityTokenService;
				if (requestSecurityToken == null)
				{
					Complete(completedSynchronously: true, DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3023"))));
					return;
				}
				switch (requestSecurityToken.RequestType)
				{
				case "http://schemas.microsoft.com/idfx/requesttype/cancel":
					securityTokenService.BeginCancel(principal, requestSecurityToken, OnCancelComplete, null);
					break;
				case "http://schemas.microsoft.com/idfx/requesttype/issue":
					securityTokenService.BeginIssue(principal, requestSecurityToken, OnIssueComplete, null);
					break;
				case "http://schemas.microsoft.com/idfx/requesttype/renew":
					securityTokenService.BeginRenew(principal, requestSecurityToken, OnRenewComplete, null);
					break;
				case "http://schemas.microsoft.com/idfx/requesttype/validate":
					securityTokenService.BeginValidate(principal, requestSecurityToken, OnValidateComplete, null);
					break;
				default:
					Complete(completedSynchronously: true, DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3112", requestSecurityToken.RequestType))));
					break;
				}
			}

			public new static DispatchContext End(IAsyncResult ar)
			{
				AsyncResult.End(ar);
				DispatchRequestAsyncResult dispatchRequestAsyncResult = ar as DispatchRequestAsyncResult;
				if (dispatchRequestAsyncResult == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2004", typeof(DispatchRequestAsyncResult), ar.GetType()));
				}
				return dispatchRequestAsyncResult.DispatchContext;
			}

			private void OnCancelComplete(IAsyncResult ar)
			{
				try
				{
					_dispatchContext.ResponseMessage = _dispatchContext.SecurityTokenService.EndCancel(ar);
					Complete(completedSynchronously: false);
				}
				catch (Exception exception)
				{
					Complete(completedSynchronously: false, exception);
				}
			}

			private void OnIssueComplete(IAsyncResult ar)
			{
				try
				{
					_dispatchContext.ResponseMessage = _dispatchContext.SecurityTokenService.EndIssue(ar);
					Complete(completedSynchronously: false);
				}
				catch (Exception exception)
				{
					Complete(completedSynchronously: false, exception);
				}
			}

			private void OnRenewComplete(IAsyncResult ar)
			{
				try
				{
					_dispatchContext.ResponseMessage = _dispatchContext.SecurityTokenService.EndRenew(ar);
					Complete(completedSynchronously: false);
				}
				catch (Exception exception)
				{
					Complete(completedSynchronously: false, exception);
				}
			}

			private void OnValidateComplete(IAsyncResult ar)
			{
				try
				{
					_dispatchContext.ResponseMessage = _dispatchContext.SecurityTokenService.EndValidate(ar);
					Complete(completedSynchronously: false);
				}
				catch (Exception exception)
				{
					Complete(completedSynchronously: false, exception);
				}
			}
		}

		private const string soap11Namespace = "http://schemas.xmlsoap.org/soap/envelope/";

		private const string soap12Namespace = "http://www.w3.org/2003/05/soap-envelope";

		private SecurityTokenServiceConfiguration _securityTokenServiceConfiguration;

		public SecurityTokenServiceConfiguration SecurityTokenServiceConfiguration => _securityTokenServiceConfiguration;

		private event EventHandler<WSTrustRequestProcessingErrorEventArgs> _requestFailed;

		public event EventHandler<WSTrustRequestProcessingErrorEventArgs> RequestFailed
		{
			add
			{
				this._requestFailed = (EventHandler<WSTrustRequestProcessingErrorEventArgs>)Delegate.Combine(this._requestFailed, value);
			}
			remove
			{
				this._requestFailed = (EventHandler<WSTrustRequestProcessingErrorEventArgs>)Delegate.Remove(this._requestFailed, value);
			}
		}

		public WSTrustServiceContract(SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
		{
			if (securityTokenServiceConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceConfiguration");
			}
			_securityTokenServiceConfiguration = securityTokenServiceConfiguration;
		}

		protected virtual SecurityTokenResolver GetSecurityHeaderTokenResolver(RequestContext requestContext)
		{
			if (requestContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestContext");
			}
			List<SecurityToken> list = new List<SecurityToken>();
			if (requestContext.RequestMessage != null && requestContext.RequestMessage.Properties != null && requestContext.RequestMessage.Properties.Security != null)
			{
				SecurityMessageProperty security = requestContext.RequestMessage.Properties.Security;
				if (security.ProtectionToken != null)
				{
					list.Add(security.ProtectionToken.SecurityToken);
				}
				if (security.HasIncomingSupportingTokens)
				{
					foreach (SupportingTokenSpecification incomingSupportingToken in security.IncomingSupportingTokens)
					{
						if (incomingSupportingToken != null && (incomingSupportingToken.SecurityTokenAttachmentMode == SecurityTokenAttachmentMode.Endorsing || incomingSupportingToken.SecurityTokenAttachmentMode == SecurityTokenAttachmentMode.SignedEndorsing))
						{
							list.Add(incomingSupportingToken.SecurityToken);
						}
					}
				}
				if (security.InitiatorToken != null)
				{
					list.Add(security.InitiatorToken.SecurityToken);
				}
			}
			if (list.Count > 0)
			{
				return SecurityTokenResolver.CreateDefaultSecurityTokenResolver(list.AsReadOnly(), canMatchLocalId: true);
			}
			return EmptySecurityTokenResolver.Instance;
		}

		protected virtual SecurityTokenResolver GetRstSecurityTokenResolver()
		{
			SecurityTokenResolver securityTokenResolver = _securityTokenServiceConfiguration.CreateAggregateTokenResolver();
			if (_securityTokenServiceConfiguration != null && securityTokenResolver != null && !object.ReferenceEquals(securityTokenResolver, EmptySecurityTokenResolver.Instance))
			{
				return securityTokenResolver;
			}
			if (OperationContext.Current != null && OperationContext.Current.Host != null && OperationContext.Current.Host.Description != null)
			{
				ServiceCredentials serviceCredentials = OperationContext.Current.Host.Description.Behaviors.Find<ServiceCredentials>();
				if (serviceCredentials != null && serviceCredentials.ServiceCertificate != null && serviceCredentials.ServiceCertificate.Certificate != null)
				{
					List<SecurityToken> list = new List<SecurityToken>(1);
					list.Add(new X509SecurityToken(serviceCredentials.ServiceCertificate.Certificate));
					return SecurityTokenResolver.CreateDefaultSecurityTokenResolver(list.AsReadOnly(), canMatchLocalId: false);
				}
			}
			return EmptySecurityTokenResolver.Instance;
		}

		protected virtual WSTrustSerializationContext CreateSerializationContext()
		{
			return new WSTrustSerializationContext(_securityTokenServiceConfiguration.SecurityTokenHandlerCollectionManager, GetRstSecurityTokenResolver(), GetSecurityHeaderTokenResolver(OperationContext.Current.RequestContext));
		}

		protected virtual IAsyncResult BeginDispatchRequest(DispatchContext dispatchContext, AsyncCallback asyncCallback, object asyncState)
		{
			return new DispatchRequestAsyncResult(dispatchContext, asyncCallback, asyncState);
		}

		protected virtual DispatchContext EndDispatchRequest(IAsyncResult ar)
		{
			return DispatchRequestAsyncResult.End(ar);
		}

		protected virtual void DispatchRequest(DispatchContext dispatchContext)
		{
			RequestSecurityToken requestSecurityToken = dispatchContext.RequestMessage as RequestSecurityToken;
			Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService securityTokenService = dispatchContext.SecurityTokenService;
			IClaimsPrincipal principal = dispatchContext.Principal;
			if (requestSecurityToken != null)
			{
				switch (requestSecurityToken.RequestType)
				{
				case "http://schemas.microsoft.com/idfx/requesttype/cancel":
					dispatchContext.ResponseMessage = securityTokenService.Cancel(principal, requestSecurityToken);
					break;
				case "http://schemas.microsoft.com/idfx/requesttype/issue":
					dispatchContext.ResponseMessage = securityTokenService.Issue(principal, requestSecurityToken);
					break;
				case "http://schemas.microsoft.com/idfx/requesttype/renew":
					dispatchContext.ResponseMessage = securityTokenService.Renew(principal, requestSecurityToken);
					break;
				case "http://schemas.microsoft.com/idfx/requesttype/validate":
					dispatchContext.ResponseMessage = securityTokenService.Validate(principal, requestSecurityToken);
					break;
				default:
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3112", requestSecurityToken.RequestType)));
				}
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3022")));
		}

		protected virtual System.ServiceModel.Channels.Message ProcessCore(System.ServiceModel.Channels.Message requestMessage, WSTrustRequestSerializer requestSerializer, WSTrustResponseSerializer responseSerializer, string requestAction, string responseAction, string trustNamespace)
		{
			if (requestMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestMessage");
			}
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			if (string.IsNullOrEmpty(requestAction))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestAction");
			}
			if (string.IsNullOrEmpty(responseAction))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseAction");
			}
			if (string.IsNullOrEmpty(trustNamespace))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustNamespace");
			}
			System.ServiceModel.Channels.Message result = null;
			try
			{
				WSTrustSerializationContext wSTrustSerializationContext = CreateSerializationContext();
				DispatchContext dispatchContext = CreateDispatchContext(requestMessage, requestAction, responseAction, trustNamespace, requestSerializer, responseSerializer, wSTrustSerializationContext);
				ValidateDispatchContext(dispatchContext);
				DispatchRequest(dispatchContext);
				result = System.ServiceModel.Channels.Message.CreateMessage(OperationContext.Current.RequestContext.RequestMessage.Version, dispatchContext.ResponseAction, new WSTrustResponseBodyWriter(dispatchContext.ResponseMessage, responseSerializer, wSTrustSerializationContext));
				return result;
			}
			catch (Exception ex)
			{
				if (!HandleException(ex, trustNamespace, requestAction, requestMessage.Version.Envelope))
				{
					throw;
				}
				return result;
			}
		}

		protected virtual DispatchContext CreateDispatchContext(System.ServiceModel.Channels.Message requestMessage, string requestAction, string responseAction, string trustNamespace, WSTrustRequestSerializer requestSerializer, WSTrustResponseSerializer responseSerializer, WSTrustSerializationContext serializationContext)
		{
			DispatchContext dispatchContext = new DispatchContext();
			dispatchContext.Principal = Thread.CurrentPrincipal as IClaimsPrincipal;
			dispatchContext.RequestAction = requestAction;
			dispatchContext.ResponseAction = responseAction;
			dispatchContext.TrustNamespace = trustNamespace;
			DispatchContext dispatchContext2 = dispatchContext;
			XmlReader readerAtBodyContents = requestMessage.GetReaderAtBodyContents();
			if (requestSerializer.CanRead(readerAtBodyContents))
			{
				dispatchContext2.RequestMessage = requestSerializer.ReadXml(readerAtBodyContents, serializationContext);
			}
			else
			{
				if (!responseSerializer.CanRead(readerAtBodyContents))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3114")));
				}
				dispatchContext2.RequestMessage = responseSerializer.ReadXml(readerAtBodyContents, serializationContext);
			}
			dispatchContext2.SecurityTokenService = CreateSTS();
			return dispatchContext2;
		}

		protected virtual void ValidateDispatchContext(DispatchContext dispatchContext)
		{
			if (dispatchContext.RequestMessage is RequestSecurityToken && !IsValidRSTAction(dispatchContext))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3113", "RequestSecurityToken", dispatchContext.RequestAction)));
			}
			if (dispatchContext.RequestMessage is RequestSecurityTokenResponse && !IsValidRSTRAction(dispatchContext))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3113", "RequestSecurityTokenResponse", dispatchContext.RequestAction)));
			}
		}

		private static bool IsValidRSTAction(DispatchContext dispatchContext)
		{
			bool result = false;
			string requestAction = dispatchContext.RequestAction;
			if (dispatchContext.TrustNamespace == "http://docs.oasis-open.org/ws-sx/ws-trust/200512")
			{
				switch (requestAction)
				{
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate":
					result = true;
					break;
				}
			}
			if (dispatchContext.TrustNamespace == "http://schemas.xmlsoap.org/ws/2005/02/trust")
			{
				switch (requestAction)
				{
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel":
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue":
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew":
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate":
					result = true;
					break;
				}
			}
			return result;
		}

		private static bool IsValidRSTRAction(DispatchContext dispatchContext)
		{
			bool result = false;
			string requestAction = dispatchContext.RequestAction;
			if (dispatchContext.TrustNamespace == "http://docs.oasis-open.org/ws-sx/ws-trust/200512")
			{
				switch (requestAction)
				{
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal":
				case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate":
					result = true;
					break;
				}
			}
			if (dispatchContext.TrustNamespace == "http://schemas.xmlsoap.org/ws/2005/02/trust")
			{
				switch (requestAction)
				{
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel":
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue":
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew":
				case "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate":
					result = true;
					break;
				}
			}
			return result;
		}

		private Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService CreateSTS()
		{
			Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService securityTokenService = _securityTokenServiceConfiguration.CreateSecurityTokenService();
			if (securityTokenService == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3002")));
			}
			return securityTokenService;
		}

		protected virtual IAsyncResult BeginProcessCore(System.ServiceModel.Channels.Message requestMessage, WSTrustRequestSerializer requestSerializer, WSTrustResponseSerializer responseSerializer, string requestAction, string responseAction, string trustNamespace, AsyncCallback callback, object state)
		{
			if (requestMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			if (string.IsNullOrEmpty(requestAction))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestAction");
			}
			if (string.IsNullOrEmpty(responseAction))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseAction");
			}
			if (string.IsNullOrEmpty(trustNamespace))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustNamespace");
			}
			IAsyncResult result = null;
			try
			{
				WSTrustSerializationContext serializationContext = CreateSerializationContext();
				DispatchContext dispatchContext = CreateDispatchContext(requestMessage, requestAction, responseAction, trustNamespace, requestSerializer, responseSerializer, serializationContext);
				ValidateDispatchContext(dispatchContext);
				result = new ProcessCoreAsyncResult(this, dispatchContext, OperationContext.Current.RequestContext.RequestMessage.Version, responseSerializer, serializationContext, callback, state);
				return result;
			}
			catch (Exception ex)
			{
				if (!HandleException(ex, trustNamespace, requestAction, requestMessage.Version.Envelope))
				{
					throw;
				}
				return result;
			}
		}

		protected virtual System.ServiceModel.Channels.Message EndProcessCore(IAsyncResult ar, string requestAction, string responseAction, string trustNamespace)
		{
			if (ar == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("ar");
			}
			ProcessCoreAsyncResult processCoreAsyncResult = ar as ProcessCoreAsyncResult;
			if (processCoreAsyncResult == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID2004", typeof(ProcessCoreAsyncResult), ar.GetType()), "ar"));
			}
			System.ServiceModel.Channels.Message result = null;
			try
			{
				result = ProcessCoreAsyncResult.End(ar);
				return result;
			}
			catch (Exception ex)
			{
				if (!HandleException(ex, trustNamespace, requestAction, processCoreAsyncResult.MessageVersion.Envelope))
				{
					throw;
				}
				return result;
			}
		}

		protected virtual bool HandleException(Exception ex, string trustNamespace, string action, EnvelopeVersion requestEnvelopeVersion)
		{
			if (DiagnosticUtil.IsFatal(ex))
			{
				return false;
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, "RequestFailed: TrustNamespace={0}, Action={1}, Exception={2}", trustNamespace, action, ex);
			}
			if (this._requestFailed != null)
			{
				this._requestFailed(this, new WSTrustRequestProcessingErrorEventArgs(action, ex));
			}
			bool flag = false;
			ServiceDebugBehavior serviceDebugBehavior = OperationContext.Current.Host.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (serviceDebugBehavior != null)
			{
				flag = serviceDebugBehavior.IncludeExceptionDetailInFaults;
			}
			if (string.IsNullOrEmpty(trustNamespace) || string.IsNullOrEmpty(action) || flag || ex is FaultException)
			{
				return false;
			}
			FaultException ex2 = _securityTokenServiceConfiguration.ExceptionMapper.FromException(ex, (requestEnvelopeVersion == EnvelopeVersion.Soap11) ? "http://schemas.xmlsoap.org/soap/envelope/" : "http://www.w3.org/2003/05/soap-envelope", trustNamespace);
			if (ex2 != null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(ex2);
			}
			return false;
		}

		public System.ServiceModel.Channels.Message ProcessTrust13Cancel(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13Issue(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13Renew(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13Validate(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13CancelResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13IssueResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13RenewResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrust13ValidateResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005Cancel(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005Issue(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005Renew(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005Validate(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005CancelResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005IssueResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005RenewResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public System.ServiceModel.Channels.Message ProcessTrustFeb2005ValidateResponse(System.ServiceModel.Channels.Message message)
		{
			return ProcessCore(message, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrust13Cancel(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13Cancel(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13Issue(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13Issue(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13Renew(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13Renew(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13Validate(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13Validate(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13CancelResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13CancelResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Cancel", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/CancelFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13IssueResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13IssueResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Issue", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTRC/IssueFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13RenewResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13RenewResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Renew", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/RenewFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrust13ValidateResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrust13RequestSerializer, _securityTokenServiceConfiguration.WSTrust13ResponseSerializer, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrust13ValidateResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/Validate", "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RSTR/ValidateFinal", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
		}

		public IAsyncResult BeginTrustFeb2005Cancel(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005Cancel(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005Issue(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005Issue(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005Renew(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005Renew(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005Validate(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005Validate(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005CancelResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005CancelResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Cancel", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005IssueResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005IssueResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005RenewResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005RenewResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Renew", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public IAsyncResult BeginTrustFeb2005ValidateResponse(System.ServiceModel.Channels.Message request, AsyncCallback callback, object state)
		{
			return BeginProcessCore(request, _securityTokenServiceConfiguration.WSTrustFeb2005RequestSerializer, _securityTokenServiceConfiguration.WSTrustFeb2005ResponseSerializer, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust", callback, state);
		}

		public System.ServiceModel.Channels.Message EndTrustFeb2005ValidateResponse(IAsyncResult ar)
		{
			return EndProcessCore(ar, "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Validate", "http://schemas.xmlsoap.org/ws/2005/02/trust");
		}

		public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
		}

		public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
		{
		}

		public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
		{
		}

		public virtual void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
		{
		}

		public virtual void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
		{
			if (exporter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("exporter");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (context.WsdlPort == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3146"));
			}
			if (context.WsdlPort.Service == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3147"));
			}
			if (context.WsdlPort.Service.ServiceDescription == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3148"));
			}
			System.Web.Services.Description.ServiceDescription serviceDescription = context.WsdlPort.Service.ServiceDescription;
			foreach (PortType portType in serviceDescription.PortTypes)
			{
				if (StringComparer.Ordinal.Equals(portType.Name, "IWSTrustFeb2005Sync"))
				{
					IncludeNamespace(context, "t", "http://schemas.xmlsoap.org/ws/2005/02/trust");
					ImportSchema(exporter, context, "http://schemas.xmlsoap.org/ws/2005/02/trust");
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Cancel", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Issue", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Renew", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005Validate", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(portType.Name, "IWSTrust13Sync"))
				{
					IncludeNamespace(context, "trust", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
					ImportSchema(exporter, context, "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
					FixMessageElement(serviceDescription, portType, context, "Trust13Cancel", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
					FixMessageElement(serviceDescription, portType, context, "Trust13Issue", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
					FixMessageElement(serviceDescription, portType, context, "Trust13Renew", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
					FixMessageElement(serviceDescription, portType, context, "Trust13Validate", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(portType.Name, "IWSTrustFeb2005Async"))
				{
					IncludeNamespace(context, "t", "http://schemas.xmlsoap.org/ws/2005/02/trust");
					ImportSchema(exporter, context, "http://schemas.xmlsoap.org/ws/2005/02/trust");
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005CancelAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005IssueAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005RenewAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
					FixMessageElement(serviceDescription, portType, context, "TrustFeb2005ValidateAsync", new XmlQualifiedName("RequestSecurityToken", "http://schemas.xmlsoap.org/ws/2005/02/trust"), new XmlQualifiedName("RequestSecurityTokenResponse", "http://schemas.xmlsoap.org/ws/2005/02/trust"));
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(portType.Name, "IWSTrust13Async"))
				{
					IncludeNamespace(context, "trust", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
					ImportSchema(exporter, context, "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
					FixMessageElement(serviceDescription, portType, context, "Trust13CancelAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
					FixMessageElement(serviceDescription, portType, context, "Trust13IssueAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
					FixMessageElement(serviceDescription, portType, context, "Trust13RenewAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
					FixMessageElement(serviceDescription, portType, context, "Trust13ValidateAsync", new XmlQualifiedName("RequestSecurityToken", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"), new XmlQualifiedName("RequestSecurityTokenResponseCollection", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"));
				}
			}
		}

		protected virtual void IncludeNamespace(WsdlEndpointConversionContext context, string prefix, string ns)
		{
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (string.IsNullOrEmpty(prefix))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("prefix");
			}
			if (string.IsNullOrEmpty(ns))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("ns");
			}
			bool flag = false;
			XmlQualifiedName[] array = context.WsdlBinding.ServiceDescription.Namespaces.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (StringComparer.Ordinal.Equals(array[i].Namespace, ns))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				context.WsdlBinding.ServiceDescription.Namespaces.Add(prefix, ns);
			}
		}

		protected virtual void ImportSchema(WsdlExporter exporter, WsdlEndpointConversionContext context, string ns)
		{
			if (exporter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("exporter");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (string.IsNullOrEmpty(ns))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("ns");
			}
			foreach (XmlSchema schema in context.WsdlPort.Service.ServiceDescription.Types.Schemas)
			{
				foreach (XmlSchemaObject include in schema.Includes)
				{
					XmlSchemaImport xmlSchemaImport = include as XmlSchemaImport;
					if (xmlSchemaImport != null && StringComparer.Ordinal.Equals(xmlSchemaImport.Namespace, ns))
					{
						return;
					}
				}
			}
			XmlSchema xmlSchema2 = GetXmlSchema(exporter, ns);
			if (xmlSchema2 == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3004", ns));
			}
			XmlSchema xmlSchema3;
			if (context.WsdlPort.Service.ServiceDescription.Types.Schemas.Count == 0)
			{
				xmlSchema3 = new XmlSchema();
				context.WsdlPort.Service.ServiceDescription.Types.Schemas.Add(xmlSchema3);
			}
			else
			{
				xmlSchema3 = context.WsdlPort.Service.ServiceDescription.Types.Schemas[0];
			}
			XmlSchemaImport xmlSchemaImport2 = new XmlSchemaImport();
			xmlSchemaImport2.Namespace = ns;
			exporter.GeneratedXmlSchemas.Add(xmlSchema2);
			xmlSchema3.Includes.Add(xmlSchemaImport2);
		}

		private static XmlSchema GetXmlSchema(WsdlExporter exporter, string ns)
		{
			if (exporter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("exporter");
			}
			if (string.IsNullOrEmpty(ns))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("ns");
			}
			ICollection collection = exporter.GeneratedXmlSchemas.Schemas(ns);
			if (collection != null && collection.Count > 0)
			{
				{
					IEnumerator enumerator = collection.GetEnumerator();
					try
					{
						if (enumerator.MoveNext())
						{
							return (XmlSchema)enumerator.Current;
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			return XmlSchema.Read(new StringReader(ns switch
			{
				"http://schemas.xmlsoap.org/ws/2005/02/trust" => "<?xml version='1.0' encoding='utf-8'?>\r\n<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'\r\n           xmlns:wst='http://schemas.xmlsoap.org/ws/2005/02/trust'\r\n           targetNamespace='http://schemas.xmlsoap.org/ws/2005/02/trust'\r\n           elementFormDefault='qualified' >\r\n\r\n<xs:element name='RequestSecurityToken' type='wst:RequestSecurityTokenType' />\r\n  <xs:complexType name='RequestSecurityTokenType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n<xs:element name='RequestSecurityTokenResponse' type='wst:RequestSecurityTokenResponseType' />\r\n  <xs:complexType name='RequestSecurityTokenResponseType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n        </xs:schema>", 
				"http://docs.oasis-open.org/ws-sx/ws-trust/200512" => "<?xml version='1.0' encoding='utf-8'?>\r\n<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'\r\n           xmlns:trust='http://docs.oasis-open.org/ws-sx/ws-trust/200512'\r\n           targetNamespace='http://docs.oasis-open.org/ws-sx/ws-trust/200512'\r\n           elementFormDefault='qualified' >\r\n\r\n<xs:element name='RequestSecurityToken' type='trust:RequestSecurityTokenType' />\r\n  <xs:complexType name='RequestSecurityTokenType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n<xs:element name='RequestSecurityTokenResponse' type='trust:RequestSecurityTokenResponseType' />\r\n  <xs:complexType name='RequestSecurityTokenResponseType' >\r\n    <xs:choice minOccurs='0' maxOccurs='unbounded' >\r\n        <xs:any namespace='##any' processContents='lax' minOccurs='0' maxOccurs='unbounded' />\r\n    </xs:choice>\r\n    <xs:attribute name='Context' type='xs:anyURI' use='optional' />\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n  <xs:element name='RequestSecurityTokenResponseCollection' type='trust:RequestSecurityTokenResponseCollectionType' />\r\n  <xs:complexType name='RequestSecurityTokenResponseCollectionType' >\r\n    <xs:sequence>\r\n      <xs:element ref='trust:RequestSecurityTokenResponse' minOccurs='1' maxOccurs='unbounded' />\r\n    </xs:sequence>\r\n    <xs:anyAttribute namespace='##other' processContents='lax' />\r\n  </xs:complexType>\r\n\r\n        </xs:schema>", 
				_ => throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID5004", ns)), 
			}), null);
		}

		protected virtual void FixMessageElement(System.Web.Services.Description.ServiceDescription serviceDescription, PortType portType, WsdlEndpointConversionContext context, string operationName, XmlQualifiedName inputMessageElement, XmlQualifiedName outputMessageElement)
		{
			if (serviceDescription == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceDescription");
			}
			if (portType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("portType");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (string.IsNullOrEmpty(operationName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("operationName");
			}
			if (inputMessageElement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("inputMessageElement");
			}
			if (outputMessageElement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("outputMessageElement");
			}
			Operation operation = null;
			System.Web.Services.Description.Message message = null;
			System.Web.Services.Description.Message message2 = null;
			foreach (Operation operation2 in portType.Operations)
			{
				if (StringComparer.Ordinal.Equals(operation2.Name, operationName))
				{
					operation = operation2;
					foreach (System.Web.Services.Description.Message message3 in serviceDescription.Messages)
					{
						if (StringComparer.Ordinal.Equals(message3.Name, operation2.Messages.Input.Message.Name))
						{
							if (message3.Parts.Count != 1)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3144", portType.Name, operation2.Name, message3.Name, message3.Parts.Count));
							}
							message = message3;
						}
						else if (StringComparer.Ordinal.Equals(message3.Name, operation2.Messages.Output.Message.Name))
						{
							if (message3.Parts.Count != 1)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3144", portType.Name, operation2.Name, message3.Name, message3.Parts.Count));
							}
							message2 = message3;
						}
						if (message != null && message2 != null)
						{
							break;
						}
					}
				}
				if (operation != null)
				{
					break;
				}
			}
			if (operation != null)
			{
				if (message == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3149", portType.Name, portType.Namespaces, operationName));
				}
				if (message2 == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3150", portType.Name, portType.Namespaces, operationName));
				}
				message.Parts[0].Element = inputMessageElement;
				message2.Parts[0].Element = outputMessageElement;
				message.Parts[0].Type = null;
				message2.Parts[0].Type = null;
			}
		}
	}
}
