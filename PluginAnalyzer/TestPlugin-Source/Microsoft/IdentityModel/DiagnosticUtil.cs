#define TRACE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel
{
	internal static class DiagnosticUtil
	{
		public static class ExceptionUtil
		{
			public static Exception ThrowHelperArgumentNull(string arg)
			{
				return ThrowHelperError(new ArgumentNullException(arg));
			}

			public static Exception ThrowHelperArgumentNullOrEmptyString(string arg)
			{
				return ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), arg));
			}

			public static Exception ThrowHelperArgumentOutOfRange(string arg)
			{
				return ThrowHelperError(new ArgumentOutOfRangeException(arg));
			}

			public static Exception ThrowHelperArgumentOutOfRange(string arg, string message)
			{
				return ThrowHelperError(new ArgumentOutOfRangeException(arg, message));
			}

			public static Exception ThrowHelperArgumentOutOfRange(string arg, object actualValue, string message)
			{
				return ThrowHelperError(new ArgumentOutOfRangeException(arg, actualValue, message));
			}

			public static Exception ThrowHelperArgument(string arg, string message)
			{
				return ThrowHelperError(new ArgumentException(message, arg));
			}

			public static Exception ThrowHelperConfigurationError(ConfigurationElement configElement, string propertyName, Exception inner)
			{
				if (inner == null)
				{
					throw ThrowHelperArgumentNull("inner");
				}
				if (configElement == null)
				{
					throw ThrowHelperArgumentNull("configElement");
				}
				if (propertyName == null)
				{
					throw ThrowHelperArgumentNull("propertyName");
				}
				if (configElement.ElementInformation == null)
				{
					throw ThrowHelperArgument("configElement", SR.GetString("ID0003", "configElement.ElementInformation"));
				}
				if (configElement.ElementInformation.Properties == null)
				{
					throw ThrowHelperArgument("configElement", SR.GetString("ID0003", "configElement.ElementInformation.Properties"));
				}
				if (configElement.ElementInformation.Properties[propertyName] == null)
				{
					throw ThrowHelperArgument("configElement", SR.GetString("ID0005", "configElement.ElementInformation.Properties", propertyName));
				}
				return ThrowHelperError(new ConfigurationErrorsException(SR.GetString("ID1024", propertyName, inner.Message), inner, configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber));
			}

			public static Exception ThrowHelperConfigurationError(ConfigurationElement configElement, string propertyName, string message)
			{
				if (configElement == null)
				{
					throw ThrowHelperArgumentNull("configElement");
				}
				if (propertyName == null)
				{
					throw ThrowHelperArgumentNull("propertyName");
				}
				if (configElement.ElementInformation == null)
				{
					throw ThrowHelperArgument("configElement", SR.GetString("ID0003", "configElement.ElementInformation"));
				}
				if (configElement.ElementInformation.Properties == null)
				{
					throw ThrowHelperArgument("configElement", SR.GetString("ID0003", "configElement.ElementInformation.Properties"));
				}
				if (configElement.ElementInformation.Properties[propertyName] == null)
				{
					throw ThrowHelperArgument("configElement", SR.GetString("ID0005", "configElement.ElementInformation.Properties", propertyName));
				}
				return ThrowHelperError(new ConfigurationErrorsException(message, configElement.ElementInformation.Properties[propertyName].Source, configElement.ElementInformation.Properties[propertyName].LineNumber));
			}

			public static Exception ThrowHelperInvalidOperation(string message)
			{
				return ThrowHelperError(new InvalidOperationException(message));
			}

			public static Exception ThrowHelperError(Exception exception)
			{
				return ThrowHelper(exception, TraceEventType.Error);
			}

			public static Exception ThrowHelperError(Exception exception, string description)
			{
				return ThrowHelper(exception, TraceEventType.Error);
			}

			public static Exception ThrowHelper(Exception exception, TraceEventType eventType)
			{
				return ThrowHelper(exception, eventType, SR.GetString("TraceHandledException"));
			}

			public static Exception ThrowHelper(Exception exception, TraceEventType eventType, string description)
			{
				return ThrowHelper(exception, eventType, description, null);
			}

			public static Exception ThrowHelper(Exception exception, TraceEventType eventType, string description, TraceRecord tr)
			{
				TraceUtil.Trace(eventType, TraceCode.HandledException, description, tr, exception);
				return exception;
			}

			public static Exception ThrowHelperXml(XmlReader reader, string message)
			{
				return ThrowHelperXml(reader, message, null);
			}

			public static Exception ThrowHelperXml(XmlReader reader, string message, Exception inner)
			{
				IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
				return ThrowHelperError(new XmlException(message, inner, xmlLineInfo?.LineNumber ?? 0, xmlLineInfo?.LinePosition ?? 0));
			}

			public static bool IsFatal(Exception exception)
			{
				for (Exception ex = exception; ex != null; ex = ex.InnerException)
				{
					if ((ex is OutOfMemoryException && !(ex is InsufficientMemoryException)) || ex is ThreadAbortException || ex is AccessViolationException || ex is SEHException)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static class TraceUtil
		{
			private const string _critical = "Critical";

			private const string _error = "Error";

			private const string _warning = "Warning";

			private const string _information = "Information";

			private const string _verbose = "Verbose";

			private const string _start = "Start";

			private const string _stop = "Stop";

			private const string _suspend = "Suspend";

			private const string _transfer = "Transfer";

			private const string TraceSourceName = "Microsoft.IdentityModel";

			private const string EventSourceName = "Microsoft.IdentityModel 3.5.0.0";

			private static bool _calledShutdown = false;

			private static string _appDomainFriendlyName = AppDomain.CurrentDomain.FriendlyName;

			private static TraceSource _traceSource = new TraceSource("Microsoft.IdentityModel");

			private static bool _traceEnabled = InitializeTraceEnabled();

			private static object _lock = new object();

			private static DateTime _lastFailure = DateTime.MinValue;

			private static TimeSpan _failureBlackout = TimeSpan.FromMinutes(10.0);

			public static string ProcessName
			{
				get
				{
					using Process process = Process.GetCurrentProcess();
					return process.ProcessName;
				}
			}

			public static int ProcessId
			{
				get
				{
					using Process process = Process.GetCurrentProcess();
					return process.Id;
				}
			}

			private static void AddDomainEventHandlersForCleanup(TraceSource traceSource)
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.UnhandledException += UnhandledExceptionHandler;
				currentDomain.DomainUnload += ExitOrUnloadEventHandler;
				currentDomain.ProcessExit += ExitOrUnloadEventHandler;
			}

			private static void AddExceptionToTraceString(XmlWriter xmlWriter, Exception exception)
			{
				xmlWriter.WriteElementString("ExceptionType", XmlEncode(exception.GetType().AssemblyQualifiedName));
				xmlWriter.WriteElementString("Message", XmlEncode(exception.Message));
				xmlWriter.WriteElementString("StackTrace", XmlEncode(StackTraceString(exception)));
				xmlWriter.WriteElementString("ExceptionString", XmlEncode(exception.ToString()));
				Win32Exception ex = exception as Win32Exception;
				if (ex != null)
				{
					xmlWriter.WriteElementString("NativeErrorCode", ex.NativeErrorCode.ToString("X", CultureInfo.InvariantCulture));
				}
				if (exception.Data != null && exception.Data.Count > 0)
				{
					xmlWriter.WriteStartElement("DataItems");
					foreach (object key in exception.Data.Keys)
					{
						xmlWriter.WriteStartElement("Data");
						xmlWriter.WriteElementString("Key", XmlEncode(key.ToString()));
						xmlWriter.WriteElementString("Value", XmlEncode(exception.Data[key].ToString()));
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
				}
				if (exception.InnerException != null)
				{
					xmlWriter.WriteStartElement("InnerException");
					AddExceptionToTraceString(xmlWriter, exception.InnerException);
					xmlWriter.WriteEndElement();
				}
			}

			private static void ExitOrUnloadEventHandler(object sender, EventArgs e)
			{
				ShutdownTracing();
			}

			private static void LogTraceFailure(string traceString, Exception e)
			{
				try
				{
					lock (_lock)
					{
						if (DateTime.UtcNow.Subtract(_lastFailure) >= _failureBlackout)
						{
							_lastFailure = DateTime.UtcNow;
							EventLog eventLog = new EventLog();
							eventLog.Source = "Microsoft.IdentityModel 3.5.0.0";
							StackTrace stackTrace = new StackTrace();
							if (e == null)
							{
								eventLog.WriteEntry(SR.GetString("EventLogTraceFailedWithExceptionMessage", e.ToString(), stackTrace.ToString()), EventLogEntryType.Error);
							}
							else
							{
								eventLog.WriteEntry(SR.GetString("EventLogTraceFailedWithoutExceptionMessage", stackTrace.ToString()), EventLogEntryType.Error);
							}
						}
					}
				}
				catch (Exception exception)
				{
					if (IsFatal(exception))
					{
						throw;
					}
				}
			}

			private static string LookupSeverity(TraceEventType type)
			{
				return type switch
				{
					TraceEventType.Critical => "Critical", 
					TraceEventType.Error => "Error", 
					TraceEventType.Warning => "Warning", 
					TraceEventType.Information => "Information", 
					TraceEventType.Verbose => "Verbose", 
					TraceEventType.Start => "Start", 
					TraceEventType.Stop => "Stop", 
					TraceEventType.Suspend => "Suspend", 
					TraceEventType.Transfer => "Transfer", 
					_ => type.ToString(), 
				};
			}

			private static bool InitializeTraceEnabled()
			{
				AddDomainEventHandlersForCleanup(_traceSource);
				if (_traceSource.Switch.Level != 0 && _traceSource.Listeners != null)
				{
					return _traceSource.Listeners.Count > 0;
				}
				return false;
			}

			public static bool ShouldTrace(TraceEventType eventType)
			{
				if (_traceEnabled)
				{
					return _traceSource.Switch.ShouldTrace(eventType);
				}
				return false;
			}

			private static void ShutdownTracing()
			{
				if (_traceSource == null || _calledShutdown)
				{
					return;
				}
				try
				{
					if (_traceEnabled && _traceSource.Switch.Level != 0)
					{
						_calledShutdown = true;
						if (ShouldTrace(TraceEventType.Information))
						{
							Dictionary<string, string> dictionary = new Dictionary<string, string>(3);
							dictionary["AppDomain.FriendlyName"] = AppDomain.CurrentDomain.FriendlyName;
							dictionary["ProcessName"] = ProcessName;
							dictionary["ProcessId"] = ProcessId.ToString(CultureInfo.CurrentCulture);
							Trace(TraceEventType.Information, TraceCode.AppDomainUnload, SR.GetString("TraceAppDomainUnload"), new DictionaryTraceRecord(dictionary), null);
						}
						_traceSource.Flush();
					}
				}
				catch (Exception ex)
				{
					if (IsFatal(ex))
					{
						throw;
					}
					LogTraceFailure(null, ex);
				}
			}

			private static string StackTraceString(Exception exception)
			{
				string text = exception.StackTrace;
				if (string.IsNullOrEmpty(text))
				{
					StackTrace stackTrace = new StackTrace();
					StackFrame[] frames = stackTrace.GetFrames();
					int num = 0;
					bool flag = false;
					StackFrame[] array = frames;
					foreach (StackFrame stackFrame in array)
					{
						string name = stackFrame.GetMethod().Name;
						switch (name)
						{
						case "AddExceptionToTraceString":
						case "HandleSecurityTokenProcessingException":
						case "StackTraceString":
						case "Trace":
						case "TraceString":
							num++;
							break;
						default:
							if (name.StartsWith("ThrowHelper", StringComparison.Ordinal))
							{
								num++;
							}
							else
							{
								flag = true;
							}
							break;
						}
						if (flag)
						{
							break;
						}
					}
					stackTrace = new StackTrace(num, fNeedFileInfo: false);
					text = stackTrace.ToString();
				}
				return text;
			}

			public static void Trace(TraceEventType type, TraceCode tc, string description, TraceRecord traceRecord, Exception exception)
			{
				try
				{
					PlainXmlWriter plainXmlWriter = new PlainXmlWriter();
					TraceXPathNavigator navigator = plainXmlWriter.Navigator;
					plainXmlWriter.WriteStartElement("TraceRecord");
					plainXmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/2009/10/IdentityModel/TraceRecord");
					plainXmlWriter.WriteAttributeString("Severity", LookupSeverity(type));
					if (string.IsNullOrEmpty(description))
					{
						plainXmlWriter.WriteElementString("Description", "Microsoft.IdentityModel Diagnostic Trace");
					}
					else
					{
						plainXmlWriter.WriteElementString("Description", description);
					}
					plainXmlWriter.WriteElementString("AppDomain", _appDomainFriendlyName);
					traceRecord?.WriteTo(plainXmlWriter);
					if (exception != null)
					{
						plainXmlWriter.WriteStartElement("Exception");
						AddExceptionToTraceString(plainXmlWriter, exception);
						plainXmlWriter.WriteEndElement();
					}
					_traceSource.TraceData(type, (int)tc, navigator);
					if (_calledShutdown)
					{
						_traceSource.Flush();
					}
					_lastFailure = DateTime.MinValue;
				}
				catch (Exception exception2)
				{
					if (ExceptionUtil.IsFatal(exception2))
					{
						throw;
					}
				}
			}

			public static void TraceString(TraceEventType eventType, string formatString, params object[] args)
			{
				if (ShouldTrace(eventType))
				{
					if (args != null && args.Length > 0)
					{
						Trace(eventType, TraceCode.Diagnostics, string.Format(CultureInfo.InvariantCulture, formatString, args), null, null);
					}
					else
					{
						Trace(eventType, TraceCode.Diagnostics, formatString, null, null);
					}
				}
			}

			public static string XmlEncode(string text)
			{
				if (string.IsNullOrEmpty(text))
				{
					return text;
				}
				int length = text.Length;
				StringBuilder stringBuilder = new StringBuilder(length + 8);
				for (int i = 0; i < length; i++)
				{
					char c = text[i];
					switch (c)
					{
					case '<':
						stringBuilder.Append("&lt;");
						break;
					case '>':
						stringBuilder.Append("&gt;");
						break;
					case '&':
						stringBuilder.Append("&amp;");
						break;
					default:
						stringBuilder.Append(c);
						break;
					}
				}
				return stringBuilder.ToString();
			}

			private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
			{
				Exception exception = (Exception)args.ExceptionObject;
				Trace(TraceEventType.Critical, TraceCode.UnhandledException, SR.GetString("TraceUnhandledException"), null, exception);
				ShutdownTracing();
			}
		}

		public static bool IsFatal(Exception exception)
		{
			return ExceptionUtil.IsFatal(exception);
		}
	}
}
