using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Threading;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustChannel : IWSTrustChannelContract, IWSTrustContract, IChannel, ICommunicationObject
	{
		internal class WSTrustChannelAsyncResult : AsyncResult
		{
			public enum Operations
			{
				Cancel,
				Issue,
				Renew,
				Validate
			}

			private IWSTrustContract _client;

			private RequestSecurityToken _rst;

			private WSTrustSerializationContext _serializationContext;

			private Message _response;

			private Operations _operation;

			public IWSTrustContract Client
			{
				get
				{
					return _client;
				}
				set
				{
					_client = value;
				}
			}

			public RequestSecurityToken RequestSecurityToken
			{
				get
				{
					return _rst;
				}
				set
				{
					_rst = value;
				}
			}

			public Message Response
			{
				get
				{
					return _response;
				}
				set
				{
					_response = value;
				}
			}

			public WSTrustSerializationContext SerializationContext
			{
				get
				{
					return _serializationContext;
				}
				set
				{
					_serializationContext = value;
				}
			}

			public WSTrustChannelAsyncResult(IWSTrustContract client, Operations operation, RequestSecurityToken rst, WSTrustSerializationContext serializationContext, Message request, AsyncCallback callback, object state)
				: base(callback, state)
			{
				_client = client;
				_rst = rst;
				_serializationContext = serializationContext;
				_operation = operation;
				switch (_operation)
				{
				case Operations.Issue:
					client.BeginIssue(request, OnOperationCompleted, null);
					break;
				case Operations.Cancel:
					client.BeginCancel(request, OnOperationCompleted, null);
					break;
				case Operations.Renew:
					client.BeginRenew(request, OnOperationCompleted, null);
					break;
				case Operations.Validate:
					client.BeginValidate(request, OnOperationCompleted, null);
					break;
				default:
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3285", Enum.GetName(typeof(Operations), _operation)));
				}
			}

			public new static Message End(IAsyncResult iar)
			{
				AsyncResult.End(iar);
				WSTrustChannelAsyncResult wSTrustChannelAsyncResult = iar as WSTrustChannelAsyncResult;
				if (wSTrustChannelAsyncResult == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2004", typeof(WSTrustChannelAsyncResult), iar.GetType()));
				}
				return wSTrustChannelAsyncResult.Response;
			}

			private void OnOperationCompleted(IAsyncResult iar)
			{
				try
				{
					Response = EndOperation(iar);
					Complete(iar.CompletedSynchronously);
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

			private Message EndOperation(IAsyncResult iar)
			{
				return _operation switch
				{
					Operations.Cancel => Client.EndCancel(iar), 
					Operations.Issue => Client.EndIssue(iar), 
					Operations.Renew => Client.EndRenew(iar), 
					Operations.Validate => Client.EndValidate(iar), 
					_ => throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3285", _operation)), 
				};
			}
		}

		internal enum ProofKeyType
		{
			Unknown,
			Bearer,
			Symmetric,
			Asymmetric
		}

		private const int DefaultKeySizeInBits = 1024;

		private const int FaultMaxBufferSize = 20480;

		private WSTrustChannelFactory _factory;

		private IChannel _innerChannel;

		private IWSTrustChannelContract _innerContract;

		private MessageVersion _messageVersion;

		private TrustVersion _trustVersion;

		private WSTrustSerializationContext _context;

		private WSTrustRequestSerializer _wsTrustRequestSerializer;

		private WSTrustResponseSerializer _wsTrustResponseSerializer;

		public IChannel Channel
		{
			get
			{
				return _innerChannel;
			}
			protected set
			{
				_innerChannel = value;
			}
		}

		public WSTrustChannelFactory ChannelFactory
		{
			get
			{
				return _factory;
			}
			protected set
			{
				_factory = value;
			}
		}

		public IWSTrustChannelContract Contract
		{
			get
			{
				return _innerContract;
			}
			protected set
			{
				_innerContract = value;
			}
		}

		public TrustVersion TrustVersion
		{
			get
			{
				return _trustVersion;
			}
			protected set
			{
				if (value != null && value != TrustVersion.WSTrust13)
				{
					_ = TrustVersion.WSTrustFeb2005;
				}
				_trustVersion = value;
			}
		}

		public WSTrustSerializationContext WSTrustSerializationContext
		{
			get
			{
				return _context;
			}
			protected set
			{
				_context = value;
			}
		}

		public WSTrustRequestSerializer WSTrustRequestSerializer
		{
			get
			{
				return _wsTrustRequestSerializer;
			}
			protected set
			{
				_wsTrustRequestSerializer = value;
			}
		}

		public WSTrustResponseSerializer WSTrustResponseSerializer
		{
			get
			{
				return _wsTrustResponseSerializer;
			}
			protected set
			{
				_wsTrustResponseSerializer = value;
			}
		}

		public CommunicationState State => Channel.State;

		public event EventHandler Closed
		{
			add
			{
				Channel.Closed += value;
			}
			remove
			{
				Channel.Closed -= value;
			}
		}

		public event EventHandler Closing
		{
			add
			{
				Channel.Closing += value;
			}
			remove
			{
				Channel.Closing -= value;
			}
		}

		public event EventHandler Faulted
		{
			add
			{
				Channel.Faulted += value;
			}
			remove
			{
				Channel.Faulted -= value;
			}
		}

		public event EventHandler Opened
		{
			add
			{
				Channel.Opened += value;
			}
			remove
			{
				Channel.Opened -= value;
			}
		}

		public event EventHandler Opening
		{
			add
			{
				Channel.Opening += value;
			}
			remove
			{
				Channel.Opening -= value;
			}
		}

		public WSTrustChannel(WSTrustChannelFactory factory, IWSTrustChannelContract inner, TrustVersion trustVersion, WSTrustSerializationContext context, WSTrustRequestSerializer requestSerializer, WSTrustResponseSerializer responseSerializer)
		{
			if (factory == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("inner");
			}
			if (inner == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("inner");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			if (trustVersion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustVersion");
			}
			_innerChannel = inner as IChannel;
			if (_innerChannel == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3286"));
			}
			_innerContract = inner;
			_factory = factory;
			_context = context;
			_wsTrustRequestSerializer = requestSerializer;
			_wsTrustResponseSerializer = responseSerializer;
			_trustVersion = trustVersion;
			_messageVersion = MessageVersion.Default;
			if (_factory.Endpoint != null && _factory.Endpoint.Binding != null && _factory.Endpoint.Binding.MessageVersion != null)
			{
				_messageVersion = _factory.Endpoint.Binding.MessageVersion;
			}
		}

		protected virtual Message CreateRequest(RequestSecurityToken request, string requestType)
		{
			return Message.CreateMessage(_messageVersion, GetRequestAction(requestType, TrustVersion), new WSTrustRequestBodyWriter(request, WSTrustRequestSerializer, WSTrustSerializationContext));
		}

		protected virtual RequestSecurityTokenResponse ReadResponse(Message response)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (response.IsFault)
			{
				MessageFault messageFault = MessageFault.CreateFault(response, 20480);
				string action = null;
				if (response.Headers != null)
				{
					action = response.Headers.Action;
				}
				FaultException ex = FaultException.CreateFault(messageFault, action);
				throw ex;
			}
			return WSTrustResponseSerializer.ReadXml(response.GetReaderAtBodyContents(), WSTrustSerializationContext);
		}

		protected static string GetRequestAction(string requestType, TrustVersion trustVersion)
		{
			if (trustVersion != TrustVersion.WSTrust13 && trustVersion != TrustVersion.WSTrustFeb2005)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3137", trustVersion.ToString())));
			}
			switch (requestType)
			{
			case "http://schemas.microsoft.com/idfx/requesttype/cancel":
				if (trustVersion != TrustVersion.WSTrustFeb2005)
				{
					return "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Cancel";
				}
				return "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Cancel";
			case "http://schemas.microsoft.com/idfx/requesttype/issue":
				if (trustVersion != TrustVersion.WSTrustFeb2005)
				{
					return "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue";
				}
				return "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue";
			case "http://schemas.microsoft.com/idfx/requesttype/renew":
				if (trustVersion != TrustVersion.WSTrustFeb2005)
				{
					return "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Renew";
				}
				return "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Renew";
			case "http://schemas.microsoft.com/idfx/requesttype/validate":
				if (trustVersion != TrustVersion.WSTrustFeb2005)
				{
					return "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Validate";
				}
				return "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Validate";
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3141", requestType.ToString())));
			}
		}

		public virtual SecurityToken GetTokenFromResponse(RequestSecurityToken request, RequestSecurityTokenResponse response)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (!response.IsFinal)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID3270")));
			}
			if (response.RequestedSecurityToken == null)
			{
				return null;
			}
			SecurityToken securityToken = response.RequestedSecurityToken.SecurityToken;
			if (securityToken == null)
			{
				if (response.RequestedSecurityToken.SecurityTokenXml == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3138")));
				}
				SecurityToken proofKey = GetProofKey(request, response);
				DateTime? dateTime = null;
				DateTime? dateTime2 = null;
				if (response.Lifetime != null)
				{
					dateTime = response.Lifetime.Created;
					dateTime2 = response.Lifetime.Expires;
					if (!dateTime.HasValue)
					{
						dateTime = DateTime.UtcNow;
					}
					if (!dateTime2.HasValue)
					{
						dateTime2 = DateTime.UtcNow.AddHours(10.0);
					}
				}
				else
				{
					dateTime = DateTime.UtcNow;
					dateTime2 = DateTime.UtcNow.AddHours(10.0);
				}
				return new GenericXmlSecurityToken(response.RequestedSecurityToken.SecurityTokenXml, proofKey, dateTime.Value, dateTime2.Value, response.RequestedAttachedReference, response.RequestedUnattachedReference, new ReadOnlyCollection<IAuthorizationPolicy>(new List<IAuthorizationPolicy>()));
			}
			return securityToken;
		}

		internal static SecurityToken GetUseKeySecurityToken(UseKey useKey, string requestKeyType)
		{
			if (useKey != null && useKey.Token != null)
			{
				return useKey.Token;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3190", requestKeyType)));
		}

		internal static ProofKeyType GetKeyType(string keyType)
		{
			switch (keyType)
			{
			default:
				if (!string.IsNullOrEmpty(keyType))
				{
					switch (keyType)
					{
					case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/PublicKey":
					case "http://schemas.xmlsoap.org/ws/2005/02/trust/PublicKey":
					case "http://schemas.microsoft.com/idfx/keytype/asymmetric":
						return ProofKeyType.Asymmetric;
					case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer":
					case "http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey":
					case "http://schemas.microsoft.com/idfx/keytype/bearer":
						return ProofKeyType.Bearer;
					default:
						return ProofKeyType.Unknown;
					}
				}
				goto case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/SymmetricKey";
			case "http://docs.oasis-open.org/ws-sx/ws-trust/200512/SymmetricKey":
			case "http://schemas.xmlsoap.org/ws/2005/02/trust/SymmetricKey":
			case "http://schemas.microsoft.com/idfx/keytype/symmetric":
				return ProofKeyType.Symmetric;
			}
		}

		internal static bool IsPsha1(string algorithm)
		{
			if (!(algorithm == "http://docs.oasis-open.org/ws-sx/ws-trust/200512/CK/PSHA1") && !(algorithm == "http://schemas.xmlsoap.org/ws/2005/02/trust/CK/PSHA1"))
			{
				return algorithm == "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1";
			}
			return true;
		}

		internal static SecurityToken ComputeProofKey(RequestSecurityToken request, RequestSecurityTokenResponse response)
		{
			if (response.Entropy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3193")));
			}
			if (request.Entropy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3194")));
			}
			int keySizeInBits = request.KeySizeInBits ?? 1024;
			if (response.KeySizeInBits.HasValue)
			{
				keySizeInBits = response.KeySizeInBits.Value;
			}
			byte[] key = KeyGenerator.ComputeCombinedKey(request.Entropy.GetKeyBytes(), response.Entropy.GetKeyBytes(), keySizeInBits);
			return new BinarySecretSecurityToken(key);
		}

		internal static SecurityToken GetProofKey(RequestSecurityToken request, RequestSecurityTokenResponse response)
		{
			if (response.RequestedProofToken != null)
			{
				if (response.RequestedProofToken.ProtectedKey != null)
				{
					return new BinarySecretSecurityToken(response.RequestedProofToken.ProtectedKey.GetKeyBytes());
				}
				if (IsPsha1(response.RequestedProofToken.ComputedKeyAlgorithm))
				{
					return ComputeProofKey(request, response);
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3192", response.RequestedProofToken.ComputedKeyAlgorithm)));
			}
			switch (GetKeyType(request.KeyType))
			{
			case ProofKeyType.Asymmetric:
				return GetUseKeySecurityToken(request.UseKey, request.KeyType);
			case ProofKeyType.Symmetric:
				if (response.Entropy != null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3191")));
				}
				if (request.Entropy != null)
				{
					return new BinarySecretSecurityToken(request.Entropy.GetKeyBytes());
				}
				return null;
			case ProofKeyType.Bearer:
				return null;
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3139", request.KeyType)));
			}
		}

		public T GetProperty<T>() where T : class
		{
			return Channel.GetProperty<T>();
		}

		public void Abort()
		{
			Channel.Abort();
		}

		public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return Channel.BeginClose(timeout, callback, state);
		}

		public IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			return Channel.BeginClose(callback, state);
		}

		public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
		{
			return Channel.BeginOpen(timeout, callback, state);
		}

		public IAsyncResult BeginOpen(AsyncCallback callback, object state)
		{
			return Channel.BeginOpen(callback, state);
		}

		public void Close(TimeSpan timeout)
		{
			Channel.Close(timeout);
		}

		public void Close()
		{
			Channel.Close();
		}

		public void EndClose(IAsyncResult result)
		{
			Channel.EndClose(result);
		}

		public void EndOpen(IAsyncResult result)
		{
			Channel.EndOpen(result);
		}

		public void Open(TimeSpan timeout)
		{
			Channel.Open(timeout);
		}

		public void Open()
		{
			Channel.Open();
		}

		public virtual RequestSecurityTokenResponse Cancel(RequestSecurityToken rst)
		{
			return ReadResponse(Contract.Cancel(CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/cancel")));
		}

		public virtual SecurityToken Issue(RequestSecurityToken rst)
		{
			RequestSecurityTokenResponse rstr = null;
			return Issue(rst, out rstr);
		}

		public virtual SecurityToken Issue(RequestSecurityToken rst, out RequestSecurityTokenResponse rstr)
		{
			Message message = CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/issue");
			Message response = Contract.Issue(message);
			rstr = ReadResponse(response);
			return GetTokenFromResponse(rst, rstr);
		}

		public virtual RequestSecurityTokenResponse Renew(RequestSecurityToken rst)
		{
			return ReadResponse(Contract.Renew(CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/renew")));
		}

		public virtual RequestSecurityTokenResponse Validate(RequestSecurityToken rst)
		{
			return ReadResponse(Contract.Validate(CreateRequest(rst, "http://schemas.microsoft.com/idfx/requesttype/validate")));
		}

		private IAsyncResult BeginOperation(WSTrustChannelAsyncResult.Operations operation, string requestType, RequestSecurityToken rst, AsyncCallback callback, object state)
		{
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			Message request = CreateRequest(rst, requestType);
			WSTrustSerializationContext wSTrustSerializationContext = WSTrustSerializationContext;
			return new WSTrustChannelAsyncResult(this, operation, rst, wSTrustSerializationContext, request, callback, state);
		}

		private RequestSecurityTokenResponse EndOperation(IAsyncResult result, out WSTrustChannelAsyncResult tcar)
		{
			if (result == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
			}
			tcar = result as WSTrustChannelAsyncResult;
			if (tcar == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2004", typeof(WSTrustChannelAsyncResult), result.GetType()));
			}
			Message response = WSTrustChannelAsyncResult.End(result);
			return ReadResponse(response);
		}

		public IAsyncResult BeginCancel(RequestSecurityToken rst, AsyncCallback callback, object state)
		{
			return BeginOperation(WSTrustChannelAsyncResult.Operations.Cancel, "http://schemas.microsoft.com/idfx/requesttype/cancel", rst, callback, state);
		}

		public void EndCancel(IAsyncResult result, out RequestSecurityTokenResponse rstr)
		{
			rstr = EndOperation(result, out var _);
		}

		public IAsyncResult BeginIssue(RequestSecurityToken rst, AsyncCallback callback, object asyncState)
		{
			return BeginOperation(WSTrustChannelAsyncResult.Operations.Issue, "http://schemas.microsoft.com/idfx/requesttype/issue", rst, callback, asyncState);
		}

		public SecurityToken EndIssue(IAsyncResult result, out RequestSecurityTokenResponse rstr)
		{
			rstr = EndOperation(result, out var tcar);
			return GetTokenFromResponse(tcar.RequestSecurityToken, rstr);
		}

		public IAsyncResult BeginRenew(RequestSecurityToken rst, AsyncCallback callback, object state)
		{
			return BeginOperation(WSTrustChannelAsyncResult.Operations.Renew, "http://schemas.microsoft.com/idfx/requesttype/renew", rst, callback, state);
		}

		public void EndRenew(IAsyncResult result, out RequestSecurityTokenResponse rstr)
		{
			rstr = EndOperation(result, out var _);
		}

		public IAsyncResult BeginValidate(RequestSecurityToken rst, AsyncCallback callback, object state)
		{
			return BeginOperation(WSTrustChannelAsyncResult.Operations.Validate, "http://schemas.microsoft.com/idfx/requesttype/validate", rst, callback, state);
		}

		public void EndValidate(IAsyncResult result, out RequestSecurityTokenResponse rstr)
		{
			rstr = EndOperation(result, out var _);
		}

		public Message Cancel(Message message)
		{
			return Contract.Cancel(message);
		}

		public IAsyncResult BeginCancel(Message message, AsyncCallback callback, object asyncState)
		{
			return Contract.BeginCancel(message, callback, asyncState);
		}

		public Message EndCancel(IAsyncResult asyncResult)
		{
			return Contract.EndCancel(asyncResult);
		}

		public Message Issue(Message message)
		{
			return Contract.Issue(message);
		}

		public IAsyncResult BeginIssue(Message message, AsyncCallback callback, object asyncState)
		{
			return Contract.BeginIssue(message, callback, asyncState);
		}

		public Message EndIssue(IAsyncResult asyncResult)
		{
			return Contract.EndIssue(asyncResult);
		}

		public Message Renew(Message message)
		{
			return Contract.Renew(message);
		}

		public IAsyncResult BeginRenew(Message message, AsyncCallback callback, object asyncState)
		{
			return Contract.BeginRenew(message, callback, asyncState);
		}

		public Message EndRenew(IAsyncResult asyncResult)
		{
			return Contract.EndRenew(asyncResult);
		}

		public Message Validate(Message message)
		{
			return Contract.Validate(message);
		}

		public IAsyncResult BeginValidate(Message message, AsyncCallback callback, object asyncState)
		{
			return Contract.BeginValidate(message, callback, asyncState);
		}

		public Message EndValidate(IAsyncResult asyncResult)
		{
			return Contract.EndValidate(asyncResult);
		}
	}
}
