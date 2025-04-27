using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	internal class WrappedSessionSecurityTokenAuthenticator : SecurityTokenAuthenticator, IIssuanceSecurityTokenAuthenticator, ICommunicationObject
	{
		private SessionSecurityTokenHandler _sessionTokenHandler;

		private IIssuanceSecurityTokenAuthenticator _issuanceSecurityTokenAuthenticator;

		private ICommunicationObject _communicationObject;

		private SctClaimsHandler _sctClaimsHandler;

		private ExceptionMapper _exceptionMapper;

		public IssuedSecurityTokenHandler IssuedSecurityTokenHandler
		{
			get
			{
				return _issuanceSecurityTokenAuthenticator.IssuedSecurityTokenHandler;
			}
			set
			{
				_issuanceSecurityTokenAuthenticator.IssuedSecurityTokenHandler = value;
			}
		}

		public RenewedSecurityTokenHandler RenewedSecurityTokenHandler
		{
			get
			{
				return _issuanceSecurityTokenAuthenticator.RenewedSecurityTokenHandler;
			}
			set
			{
				_issuanceSecurityTokenAuthenticator.RenewedSecurityTokenHandler = value;
			}
		}

		public CommunicationState State => _communicationObject.State;

		public event EventHandler Closed
		{
			add
			{
				_communicationObject.Closed += value;
			}
			remove
			{
				_communicationObject.Closed -= value;
			}
		}

		public event EventHandler Closing
		{
			add
			{
				_communicationObject.Closing += value;
			}
			remove
			{
				_communicationObject.Closing -= value;
			}
		}

		public event EventHandler Faulted
		{
			add
			{
				_communicationObject.Faulted += value;
			}
			remove
			{
				_communicationObject.Faulted -= value;
			}
		}

		public event EventHandler Opened
		{
			add
			{
				_communicationObject.Opened += value;
			}
			remove
			{
				_communicationObject.Opened -= value;
			}
		}

		public event EventHandler Opening
		{
			add
			{
				_communicationObject.Opening += value;
			}
			remove
			{
				_communicationObject.Opening -= value;
			}
		}

		public WrappedSessionSecurityTokenAuthenticator(SessionSecurityTokenHandler sessionTokenHandler, SecurityTokenAuthenticator wcfSessionAuthenticator, SctClaimsHandler sctClaimsHandler, ExceptionMapper exceptionMapper)
		{
			if (sessionTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionTokenHandler");
			}
			if (wcfSessionAuthenticator == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wcfSessionAuthenticator");
			}
			if (sctClaimsHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sctClaimsHandler");
			}
			if (exceptionMapper == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("exceptionMapper");
			}
			_issuanceSecurityTokenAuthenticator = wcfSessionAuthenticator as IIssuanceSecurityTokenAuthenticator;
			if (_issuanceSecurityTokenAuthenticator == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4244"));
			}
			_communicationObject = wcfSessionAuthenticator as ICommunicationObject;
			if (_communicationObject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4245"));
			}
			_sessionTokenHandler = sessionTokenHandler;
			_sctClaimsHandler = sctClaimsHandler;
			_exceptionMapper = exceptionMapper;
		}

		protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			SecurityContextSecurityToken securityContextToken = token as SecurityContextSecurityToken;
			SessionSecurityToken token2 = new SessionSecurityToken(securityContextToken, SessionSecurityTokenHandler.DefaultSecureConversationVersion);
			ClaimsIdentityCollection identityCollection = null;
			try
			{
				identityCollection = _sessionTokenHandler.ValidateToken(token2, _sctClaimsHandler.EndpointId);
			}
			catch (Exception ex)
			{
				if (!_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
			return new List<IAuthorizationPolicy>(new AuthorizationPolicy[1]
			{
				new AuthorizationPolicy(identityCollection)
			}).AsReadOnly();
		}

		protected override bool CanValidateTokenCore(SecurityToken token)
		{
			return token is SecurityContextSecurityToken;
		}

		public void Abort()
		{
			_communicationObject.Abort();
		}

		public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return _communicationObject.BeginClose(timeout, callback, state);
		}

		public IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			return _communicationObject.BeginClose(callback, state);
		}

		public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return _communicationObject.BeginOpen(timeout, callback, state);
		}

		public IAsyncResult BeginOpen(AsyncCallback callback, object state)
		{
			return _communicationObject.BeginOpen(callback, state);
		}

		public void Close(TimeSpan timeout)
		{
			_communicationObject.Close(timeout);
		}

		public void Close()
		{
			_communicationObject.Close();
		}

		public void EndClose(IAsyncResult result)
		{
			_communicationObject.EndClose(result);
		}

		public void EndOpen(IAsyncResult result)
		{
			_communicationObject.EndOpen(result);
		}

		public void Open(TimeSpan timeout)
		{
			_communicationObject.Open(timeout);
		}

		public void Open()
		{
			_communicationObject.Open();
		}
	}
}
