using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Xrm.Kernel.Contracts.ExternalIntegration;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class StringEncryptionHelper
	{
		private const string _secretsKey = "AzureMLSecretsPrimary";

		public static string Encrypt(IPluginContext context, string plaintext)
		{
			if (string.IsNullOrEmpty(plaintext) || AdapterUtility.IsOnPremTestHookEnabled(context))
			{
				return string.Empty;
			}
			using SymmetricAlgorithm symmetricAlgorithm = GetEncryptionProvider(context);
			using ICryptoTransform transform = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);
			using MemoryStream memoryStream = new MemoryStream();
			using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				streamWriter.Write(plaintext);
			}
			return Convert.ToBase64String(memoryStream.ToArray());
		}

		public static byte[] GetKeyHash(IPluginContext context)
		{
			if (AdapterUtility.IsOnPremTestHookEnabled(context))
			{
				return Encoding.Unicode.GetBytes("TestKeyHash");
			}
			using SymmetricAlgorithm symmetricAlgorithm = GetEncryptionProvider(context);
			byte[] array = new byte[symmetricAlgorithm.Key.Length + symmetricAlgorithm.IV.Length];
			Buffer.BlockCopy(symmetricAlgorithm.Key, 0, array, 0, symmetricAlgorithm.Key.Length);
			Buffer.BlockCopy(symmetricAlgorithm.IV, 0, array, symmetricAlgorithm.Key.Length, symmetricAlgorithm.IV.Length);
			using SHA256CryptoServiceProvider sHA256CryptoServiceProvider = new SHA256CryptoServiceProvider();
			sHA256CryptoServiceProvider.Initialize();
			return sHA256CryptoServiceProvider.ComputeHash(array);
		}

		private static SymmetricAlgorithm GetEncryptionProvider(IPluginContext context)
		{
			IExternalIntegrationConfigSettings val = AdapterUtility.RetrievePropertiesFromGeoDB("AzureMLSecretsPrimary", context);
			if (val == null)
			{
			}
			string applicationSecret = val.get_ApplicationSecret();
			string applicationSecretSecondary = val.get_ApplicationSecretSecondary();
			string applicationInfo = val.get_ApplicationInfo();
			if (string.Compare(applicationInfo, "AES", StringComparison.InvariantCultureIgnoreCase) != 0)
			{
			}
			return new AesCryptoServiceProvider
			{
				Key = Convert.FromBase64String(applicationSecret),
				IV = Convert.FromBase64String(applicationSecretSecondary)
			};
		}
	}
}
