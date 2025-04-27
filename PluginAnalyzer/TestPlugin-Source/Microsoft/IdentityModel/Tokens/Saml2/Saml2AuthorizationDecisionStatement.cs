using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2AuthorizationDecisionStatement : Saml2Statement
	{
		public static readonly Uri EmptyResource = new Uri("", UriKind.Relative);

		private Collection<Saml2Action> _actions = new Collection<Saml2Action>();

		private Saml2Evidence _evidence;

		private SamlAccessDecision _decision;

		private Uri _resource;

		public Collection<Saml2Action> Actions => _actions;

		public SamlAccessDecision Decision
		{
			get
			{
				return _decision;
			}
			set
			{
				if (value < SamlAccessDecision.Permit || value > SamlAccessDecision.Indeterminate)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("decision");
				}
				_decision = value;
			}
		}

		public Saml2Evidence Evidence
		{
			get
			{
				return _evidence;
			}
			set
			{
				_evidence = value;
			}
		}

		public Uri Resource
		{
			get
			{
				return _resource;
			}
			set
			{
				if (null == value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				if (!value.IsAbsoluteUri && !value.Equals(EmptyResource))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4121"));
				}
				_resource = value;
			}
		}

		public Saml2AuthorizationDecisionStatement(Uri resource, SamlAccessDecision decision)
			: this(resource, decision, null)
		{
		}

		public Saml2AuthorizationDecisionStatement(Uri resource, SamlAccessDecision decision, IEnumerable<Saml2Action> actions)
		{
			if (null == resource)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("resource");
			}
			if (!resource.IsAbsoluteUri && !resource.Equals(EmptyResource))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("resource", SR.GetString("ID4121"));
			}
			if (decision < SamlAccessDecision.Permit || decision > SamlAccessDecision.Indeterminate)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("decision");
			}
			_resource = resource;
			_decision = decision;
			if (actions == null)
			{
				return;
			}
			foreach (Saml2Action action in actions)
			{
				_actions.Add(action);
			}
		}
	}
}
