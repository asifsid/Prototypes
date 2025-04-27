using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.Xrm.Kernel.Contracts;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public abstract class PluginBase : IPlugin
	{
		protected string ChildClassName { get; private set; }

		protected virtual bool SkipPluginLogicOnCascadeDelete => true;

		protected internal PluginBase(Type childClassName)
		{
			ChildClassName = childClassName.ToString();
		}

		public void Execute(IServiceProvider serviceProvider)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (serviceProvider == null)
			{
				throw new InvalidPluginExecutionException("serviceProvider");
			}
			LocalPluginContext localPluginContext = new LocalPluginContext(serviceProvider);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] args = new string[1] { ChildClassName };
			localPluginContext.TraceOnPlugInTraceLog(string.Format(invariantCulture, "Entered {0}.Execute()", args));
			InitializeExecutionContext(localPluginContext);
			try
			{
				if (IsSkipPluginSet(localPluginContext))
				{
					CultureInfo invariantCulture2 = CultureInfo.InvariantCulture;
					args = new string[1] { ChildClassName };
					localPluginContext.TraceOnPlugInTraceLog(string.Format(invariantCulture2, "Skip plugin execution flag is set. Skipping {0}.Execute()", args));
				}
				else if (SkipPluginLogicOnCascadeDelete && IsInCascadeDeleteOperationForChildEntity(localPluginContext))
				{
					CultureInfo invariantCulture3 = CultureInfo.InvariantCulture;
					args = new string[1] { ChildClassName };
					localPluginContext.TraceOnPlugInTraceLog(string.Format(invariantCulture3, "Plugin is invoked from CascadeDelete operation. Skipping {0}.Execute()", args));
				}
				else
				{
					ExecuteCrmPlugin(localPluginContext);
				}
			}
			catch (CrmException ex)
			{
				localPluginContext.TraceOnPlugInTraceLog(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", new object[1] { ex.ToString() }));
				throw;
			}
			catch (FaultException<OrganizationServiceFault> ex2)
			{
				CultureInfo invariantCulture4 = CultureInfo.InvariantCulture;
				args = new string[1] { ex2.ToString() };
				localPluginContext.TraceOnPlugInTraceLog(string.Format(invariantCulture4, "Exception: {0}", args));
				throw;
			}
			finally
			{
				FinalizeExecutionContext();
				CultureInfo invariantCulture5 = CultureInfo.InvariantCulture;
				args = new string[1] { ChildClassName };
				localPluginContext.TraceOnPlugInTraceLog(string.Format(invariantCulture5, "Exiting {0}.Execute()", args));
			}
		}

		protected virtual void InitializeExecutionContext(LocalPluginContext localContext)
		{
			PluginContextManager.InitiatingExecution(localContext);
		}

		protected virtual void FinalizeExecutionContext()
		{
			PluginContextManager.FinalizingExecution();
		}

		protected bool IsSkipPluginSet(IPluginContext context)
		{
			string key = SharedVariableNamesProvider.Get(context);
			ISharedVariablesService sharedVariablesService = context.SharedVariablesService;
			if (sharedVariablesService == null)
			{
				return false;
			}
			if (sharedVariablesService.GetOrDefault(key, (Scope)2, defaultValue: false))
			{
				return true;
			}
			string messageName = ((IExecutionContext)context.PluginExecutionContext).get_MessageName();
			string primaryEntityName = ((IExecutionContext)context.PluginExecutionContext).get_PrimaryEntityName();
			key = SharedVariableNamesProvider.Get(messageName, primaryEntityName);
			if (sharedVariablesService.GetOrDefault(key, (Scope)2, defaultValue: false))
			{
				return true;
			}
			return false;
		}

		protected bool IsInCascadeDeleteOperationForChildEntity(IPluginContext context)
		{
			if (context.SharedVariablesService == null)
			{
				return false;
			}
			return context.SharedVariablesService.GetOrDefault("Microsoft.Xrm.Kernel.Contracts.InCascadeDeleteOperationForChildEntity", defaultValue: false);
		}

		protected abstract void ExecuteCrmPlugin(LocalPluginContext localcontext);
	}
}
