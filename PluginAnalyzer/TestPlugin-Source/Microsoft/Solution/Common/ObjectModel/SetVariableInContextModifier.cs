using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common;
using Microsoft.Xrm.Kernel.Contracts;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Solution.Common.ObjectModel
{
	[ComVisible(true)]
	public class SetVariableInContextModifier : IDisposable
	{
		private IPluginContext _context;

		private string _key;

		private Dictionary<string, object> _variables;

		private bool _isPipelineControlVariable = false;

		public SetVariableInContextModifier(IPluginContext context, string key, object value)
		{
			Exceptions.ThrowIfNull(context, "context");
			_context = context;
			IPluginExecutionContext pluginExecutionContext = context.PluginExecutionContext;
			IPluginExecutionContext val = ((pluginExecutionContext != null) ? pluginExecutionContext.get_ParentContext() : null) ?? context.PluginExecutionContext;
			if (val != null && !((DataCollection<string, object>)(object)((IExecutionContext)val).get_SharedVariables()).ContainsKey(key))
			{
				((DataCollection<string, object>)(object)((IExecutionContext)val).get_SharedVariables()).Add(key, value);
				_key = key;
			}
		}

		public SetVariableInContextModifier(IPluginContext context, Dictionary<string, object> variables)
		{
			Exceptions.ThrowIfNull(context, "context");
			if (variables == null || variables.Count == 0)
			{
				throw new ArgumentException("Variables were not provided.");
			}
			_context = context;
			_variables = variables;
			IPluginExecutionContext val = context.PluginExecutionContext.get_ParentContext() ?? context.PluginExecutionContext;
			if (val == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> variable in _variables)
			{
				if (!((DataCollection<string, object>)(object)((IExecutionContext)val).get_SharedVariables()).ContainsKey(variable.Key))
				{
					((DataCollection<string, object>)(object)((IExecutionContext)val).get_SharedVariables()).Add(variable.Key, variable.Value);
				}
			}
		}

		public SetVariableInContextModifier(IPluginContext context, string key, object value, Scope scope, Lifetime lifetime)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			Exceptions.ThrowIfNull(context, "context");
			_context = context;
			if (context.SharedVariablesService != null)
			{
				_context.SharedVariablesService.Set(key, value, scope, lifetime);
			}
		}

		private SetVariableInContextModifier(IPluginContext context, string key)
		{
			Exceptions.ThrowIfNull(context, "context");
			_context = context;
			if (context.SharedVariablesService != null)
			{
				SetPipelineControlVariable(key, value: true);
				_isPipelineControlVariable = true;
				_key = key;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_isPipelineControlVariable && !string.IsNullOrEmpty(_key))
				{
					SetPipelineControlVariable(_key, value: false);
				}
				else
				{
					DisposeNormalVariable();
				}
			}
		}

		private void SetPipelineControlVariable(string key, bool value)
		{
			_context.SharedVariablesService.Set(key, (object)value, (Scope)2, (Lifetime)1);
		}

		private void DisposeNormalVariable()
		{
			IPluginExecutionContext pluginExecutionContext = _context.PluginExecutionContext;
			if (pluginExecutionContext == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(_key))
			{
				((DataCollection<string, object>)(object)((IExecutionContext)pluginExecutionContext).get_SharedVariables()).Remove(_key);
			}
			if (_variables == null || _variables.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<string, object> variable in _variables)
			{
				if (!string.IsNullOrEmpty(variable.Key))
				{
					((DataCollection<string, object>)(object)((IExecutionContext)pluginExecutionContext).get_SharedVariables()).Remove(variable.Key);
				}
			}
		}

		public static SetVariableInContextModifier GetForPipelineControlVariable(IPluginContext context, string key)
		{
			return new SetVariableInContextModifier(context, key);
		}
	}
}
