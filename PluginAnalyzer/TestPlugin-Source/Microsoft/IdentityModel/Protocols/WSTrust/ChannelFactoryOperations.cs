using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public static class ChannelFactoryOperations
	{
		public static T CreateChannelActingAs<T>(this ChannelFactory<T> factory, SecurityToken actAs)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.ActAs = actAs;
			return CreateChannelWithParameters(factory, federatedClientCredentialsParameters);
		}

		public static T CreateChannelActingAs<T>(this ChannelFactory<T> factory, EndpointAddress address, SecurityToken actAs)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.ActAs = actAs;
			return CreateChannelWithParameters(factory, address, federatedClientCredentialsParameters);
		}

		public static T CreateChannelActingAs<T>(this ChannelFactory<T> factory, EndpointAddress address, Uri via, SecurityToken actAs)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.ActAs = actAs;
			return CreateChannelWithParameters(factory, address, via, federatedClientCredentialsParameters);
		}

		public static T CreateChannelOnBehalfOf<T>(this ChannelFactory<T> factory, SecurityToken onBehalfOf)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.OnBehalfOf = onBehalfOf;
			return CreateChannelWithParameters(factory, federatedClientCredentialsParameters);
		}

		public static T CreateChannelOnBehalfOf<T>(this ChannelFactory<T> factory, EndpointAddress address, SecurityToken onBehalfOf)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.OnBehalfOf = onBehalfOf;
			return CreateChannelWithParameters(factory, address, federatedClientCredentialsParameters);
		}

		public static T CreateChannelOnBehalfOf<T>(this ChannelFactory<T> factory, EndpointAddress address, Uri via, SecurityToken onBehalfOf)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.OnBehalfOf = onBehalfOf;
			return CreateChannelWithParameters(factory, address, via, federatedClientCredentialsParameters);
		}

		public static T CreateChannelWithIssuedToken<T>(this ChannelFactory<T> factory, SecurityToken issuedToken)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.IssuedSecurityToken = issuedToken;
			return CreateChannelWithParameters(factory, federatedClientCredentialsParameters);
		}

		public static T CreateChannelWithIssuedToken<T>(this ChannelFactory<T> factory, EndpointAddress address, SecurityToken issuedToken)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.IssuedSecurityToken = issuedToken;
			return CreateChannelWithParameters(factory, address, federatedClientCredentialsParameters);
		}

		public static T CreateChannelWithIssuedToken<T>(this ChannelFactory<T> factory, EndpointAddress address, Uri via, SecurityToken issuedToken)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			federatedClientCredentialsParameters.IssuedSecurityToken = issuedToken;
			return CreateChannelWithParameters(factory, address, via, federatedClientCredentialsParameters);
		}

		internal static void VerifyChannelFactory<T>(ChannelFactory<T> channelFactory)
		{
			if (channelFactory == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("channelFactory");
			}
			if (channelFactory.Endpoint == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3195"));
			}
			if (channelFactory.Endpoint.Behaviors == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3195"));
			}
			FederatedClientCredentials federatedClientCredentials = channelFactory.Endpoint.Behaviors.Find<ClientCredentials>() as FederatedClientCredentials;
			if (federatedClientCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3196"));
			}
		}

		public static T CreateChannelWithParameters<T>(ChannelFactory<T> factory, FederatedClientCredentialsParameters parameters)
		{
			VerifyChannelFactory(factory);
			T val = factory.CreateChannel();
			((IChannel)(object)val).GetProperty<ChannelParameterCollection>().Add(parameters);
			return val;
		}

		public static T CreateChannelWithParameters<T>(ChannelFactory<T> factory, EndpointAddress address, FederatedClientCredentialsParameters parameters)
		{
			VerifyChannelFactory(factory);
			T val = factory.CreateChannel(address);
			((IChannel)(object)val).GetProperty<ChannelParameterCollection>().Add(parameters);
			return val;
		}

		public static T CreateChannelWithParameters<T>(ChannelFactory<T> factory, EndpointAddress address, Uri via, FederatedClientCredentialsParameters parameters)
		{
			VerifyChannelFactory(factory);
			T val = factory.CreateChannel(address, via);
			((IChannel)(object)val).GetProperty<ChannelParameterCollection>().Add(parameters);
			return val;
		}

		public static void ConfigureChannelFactory<T>(this ChannelFactory<T> channelFactory)
		{
			if (channelFactory == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("channelFactory");
			}
			if (channelFactory.State != 0 && channelFactory.State != CommunicationState.Opening)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3197"));
			}
			ClientCredentials clientCredentials = channelFactory.Endpoint.Behaviors.Find<ClientCredentials>();
			if (clientCredentials != null)
			{
				channelFactory.Endpoint.Behaviors.Remove(clientCredentials.GetType());
				FederatedClientCredentials item = new FederatedClientCredentials(clientCredentials);
				channelFactory.Endpoint.Behaviors.Add(item);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3212"));
		}
	}
}
