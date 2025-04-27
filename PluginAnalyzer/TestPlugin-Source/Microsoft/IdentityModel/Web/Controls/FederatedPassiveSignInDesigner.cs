using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.UI.Design.WebControls;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Microsoft.IdentityModel.Web.Controls
{
	internal class FederatedPassiveSignInDesigner : CompositeControlDesigner
	{
		private class FederatedPassiveSignInActionList : DesignerActionList
		{
			private FederatedPassiveSignIn _signInControl;

			private FederatedPassiveSignInDesigner _designer;

			public FederatedPassiveSignInActionList(FederatedPassiveSignInDesigner designer)
				: base(designer.Component)
			{
				_designer = designer;
				_signInControl = (FederatedPassiveSignIn)designer.Component;
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "InvokeFedUtil", SR.GetString("SignIn_InvokeFedUtil"), string.Empty, SR.GetString("SignIn_InvokeFedUtil"), includeAsDesignerVerb: true));
				return designerActionItemCollection;
			}

			private void InvokeFedUtil()
			{
				if (!_signInControl.UseFederationPropertiesFromConfiguration)
				{
					MessageBox.Show(SR.GetString("SignIn_InvokeFedUtilErrorConfigProperty"), SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, GetMessageBoxOptions());
					return;
				}
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Identity Foundation SDK\\Setup\\v3.5");
				if (registryKey == null)
				{
					MessageBox.Show(SR.GetString("SignIn_InvokeFedUtilErrorGeneric"), SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, GetMessageBoxOptions());
					return;
				}
				string text = registryKey.GetValue("InstallPath") as string;
				if (string.IsNullOrEmpty(text))
				{
					MessageBox.Show(SR.GetString("SignIn_InvokeFedUtilErrorGeneric"), SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, GetMessageBoxOptions());
					return;
				}
				string text2 = Path.Combine(text, "FedUtil.exe");
				if (!File.Exists(text2))
				{
					MessageBox.Show(SR.GetString("SignIn_InvokeFedUtilErrorGeneric"), SR.GetString("SignIn_InvokeFedUtilError"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, GetMessageBoxOptions());
				}
				else
				{
					Process.Start(text2, "/c");
				}
			}

			private MessageBoxOptions GetMessageBoxOptions()
			{
				MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
				if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
				{
					messageBoxOptions |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
				}
				return messageBoxOptions;
			}
		}

		private const string _wifSDKRegistryPath = "SOFTWARE\\Microsoft\\Windows Identity Foundation SDK\\Setup\\v3.5";

		private const string _wifSDKRegistryKey = "InstallPath";

		private const string _fedUtilExe = "FedUtil.exe";

		private const string _fedUtilArgs = "/c";

		private bool _fedUtilInstalled;

		private DesignerActionListCollection _actionLists;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (_fedUtilInstalled)
				{
					if (_actionLists == null)
					{
						_actionLists = new DesignerActionListCollection();
						_actionLists.Add(new FederatedPassiveSignInActionList(this));
					}
					return _actionLists;
				}
				return base.ActionLists;
			}
		}

		public override void Initialize(IComponent component)
		{
			_fedUtilInstalled = IsFedUtilInstalled();
			base.Initialize(component);
		}

		private bool IsFedUtilInstalled()
		{
			bool result = false;
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows Identity Foundation SDK\\Setup\\v3.5");
			if (registryKey != null)
			{
				string text = registryKey.GetValue("InstallPath") as string;
				if (!string.IsNullOrEmpty(text))
				{
					string path = Path.Combine(text, "FedUtil.exe");
					if (File.Exists(path))
					{
						result = true;
					}
				}
			}
			return result;
		}
	}
}
