using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class CustomTypeElement : ConfigurationElementInterceptor
	{
		public bool IsConfigured => (object)TypeName != null;

		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
		[TypeConverter(typeof(TypeNameConverter))]
		public Type TypeName
		{
			get
			{
				return (Type)base["type"];
			}
			set
			{
				base["type"] = value;
			}
		}

		public CustomTypeElement()
		{
		}

		internal CustomTypeElement(Type typeName)
		{
			TypeName = typeName;
		}

		public static T Resolve<T>(CustomTypeElement customTypeElement, params object[] arguments) where T : class
		{
			if (customTypeElement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("customTypeElement");
			}
			try
			{
				Type typeName = customTypeElement.TypeName;
				if (!typeof(T).IsAssignableFrom(typeName))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(customTypeElement, "type", SR.GetString("ID1029", typeName.AssemblyQualifiedName, typeof(T)));
				}
				object[] array = null;
				if (customTypeElement.ElementAsXml != null)
				{
					foreach (XmlNode childNode in customTypeElement.ElementAsXml.ChildNodes)
					{
						if (childNode.NodeType != XmlNodeType.Element)
						{
							customTypeElement.ElementAsXml.RemoveChild(childNode);
						}
					}
				}
				if (customTypeElement.ElementAsXml == null || customTypeElement.ElementAsXml.ChildNodes.Count == 0)
				{
					if (arguments != null && arguments.Length > 0)
					{
						array = arguments;
					}
				}
				else if (arguments != null && arguments.Length > 0)
				{
					array = new object[1] { customTypeElement.ElementAsXml.ChildNodes };
				}
				else
				{
					array = new object[1 + arguments.Length];
					array[0] = customTypeElement.ElementAsXml.ChildNodes;
					for (int i = 0; i < arguments.Length; i++)
					{
						array[i + 1] = arguments[i];
					}
				}
				return (T)Activator.CreateInstance(typeName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, array, null);
			}
			catch (Exception ex)
			{
				if (ex is ConfigurationErrorsException || DiagnosticUtil.ExceptionUtil.IsFatal(ex))
				{
					throw;
				}
				if (ex is TargetInvocationException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ConfigurationErrorsException(SR.GetString("ID0012", customTypeElement.TypeName.AssemblyQualifiedName), ex));
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(customTypeElement, "type", ex);
			}
		}
	}
}
