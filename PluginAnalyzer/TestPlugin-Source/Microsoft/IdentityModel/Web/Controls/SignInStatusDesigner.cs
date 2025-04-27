using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.Design.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
	internal class SignInStatusDesigner : CompositeControlDesigner
	{
		private class LoginStatusDesignerActionList : DesignerActionList
		{
			private class SignInStatusViewTypeConverter : TypeConverter
			{
				public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					return new StandardValuesCollection(new string[2]
					{
						SR.GetString("SignInStatus_SignedOutView"),
						SR.GetString("SignInStatus_SignedInView")
					});
				}

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}

			private SignInStatusDesigner _designer;

			[TypeConverter(typeof(SignInStatusViewTypeConverter))]
			public string View
			{
				get
				{
					if (_designer._loggedIn)
					{
						return SR.GetString("SignInStatus_SignedInView");
					}
					return SR.GetString("SignInStatus_SignedOutView");
				}
				set
				{
					if (string.Compare(value, SR.GetString("SignInStatus_SignedInView"), StringComparison.Ordinal) == 0)
					{
						_designer._loggedIn = true;
					}
					else if (string.Compare(value, SR.GetString("SignInStatus_SignedOutView"), StringComparison.Ordinal) == 0)
					{
						_designer._loggedIn = false;
					}
					_designer.UpdateDesignTimeHtml();
				}
			}

			public LoginStatusDesignerActionList(SignInStatusDesigner designer)
				: base(designer.Component)
			{
				_designer = designer;
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				designerActionItemCollection.Add(new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), string.Empty, SR.GetString("WebControls_ViewsDescription")));
				return designerActionItemCollection;
			}
		}

		private bool _loggedIn;

		private FederatedPassiveSignInStatus _loginStatus;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new LoginStatusDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		protected override bool UsePreviewControl => true;

		public override string GetDesignTimeHtml()
		{
			IDictionary dictionary = new HybridDictionary(2);
			dictionary["LoggedIn"] = _loggedIn;
			FederatedPassiveSignInStatus federatedPassiveSignInStatus = (FederatedPassiveSignInStatus)base.ViewControl;
			((IControlDesignerAccessor)federatedPassiveSignInStatus).SetDesignModeState(dictionary);
			if (_loggedIn)
			{
				string signOutText = federatedPassiveSignInStatus.SignOutText;
				if (signOutText == null || signOutText.Length == 0 || signOutText == " ")
				{
					federatedPassiveSignInStatus.SignOutText = "[" + federatedPassiveSignInStatus.ID + "]";
				}
			}
			else
			{
				string signOutText = federatedPassiveSignInStatus.SignInText;
				if (signOutText == null || signOutText.Length == 0 || signOutText == " ")
				{
					federatedPassiveSignInStatus.SignInText = "[" + federatedPassiveSignInStatus.ID + "]";
				}
			}
			return base.GetDesignTimeHtml();
		}

		public override void Initialize(IComponent component)
		{
			_loginStatus = (FederatedPassiveSignInStatus)component;
			base.Initialize((IComponent)_loginStatus);
		}
	}
}
