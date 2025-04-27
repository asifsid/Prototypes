using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class EntityExtensions
	{
		private static string[] UpdatableAttributes = new string[7] { "ownerid", "owneridtype", "statecode", "statuscode", "modifiedon", "modifiedby", "modifiedonbehalfby" };

		public static bool IsAttributeNull(this Entity entity, string attributeName)
		{
			bool result = true;
			object obj = default(object);
			if (((DataCollection<string, object>)(object)entity.get_Attributes()).TryGetValue(attributeName, ref obj))
			{
				result = string.IsNullOrEmpty(Convert.ToString(obj, CultureInfo.InvariantCulture));
			}
			return result;
		}

		public static void SetAttributeValue(this Entity entity, string attributeName, object value)
		{
			if (((DataCollection<string, object>)(object)entity.get_Attributes()).ContainsKey(attributeName))
			{
				((DataCollection<string, object>)(object)entity.get_Attributes()).set_Item(attributeName, value);
			}
			else
			{
				((DataCollection<string, object>)(object)entity.get_Attributes()).Add(attributeName, value);
			}
		}

		public static void ClearAttribute(this Entity entity, string attributeName)
		{
			if (((DataCollection<string, object>)(object)entity.get_Attributes()).ContainsKey(attributeName))
			{
				((DataCollection<string, object>)(object)entity.get_Attributes()).Remove(attributeName);
			}
		}

		public static Entity Clone(this Entity entity)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			Entity val = new Entity(entity.get_LogicalName());
			val.set_RowVersion(entity.get_RowVersion());
			foreach (KeyValuePair<string, object> item in (DataCollection<string, object>)(object)entity.get_Attributes())
			{
				val.set_Item(item.Key, item.Value);
			}
			val.set_EntityState(entity.get_EntityState());
			((DataCollection<Relationship, EntityCollection>)(object)val.get_RelatedEntities()).AddRange((IEnumerable<KeyValuePair<Relationship, EntityCollection>>)entity.get_RelatedEntities());
			return val;
		}

		public static void ResetAttributes(this Entity entity, string[] attributesToReset)
		{
			foreach (string attributeName in attributesToReset)
			{
				entity.ClearAttribute(attributeName);
			}
		}

		public static Entity Merge(this Entity value, Entity target)
		{
			Exceptions.ThrowIfNull(target, "target");
			if (value.get_LogicalName() != target.get_LogicalName())
			{
				throw new CrmException(string.Format(Labels.CannotDoOperation, "Merge", value.get_LogicalName(), target.get_LogicalName()));
			}
			Entity val = value.Clone();
			foreach (KeyValuePair<string, object> item in (DataCollection<string, object>)(object)target.get_Attributes())
			{
				val.set_Item(item.Key, item.Value);
			}
			return val;
		}

		public static T Merge<T>(this T value, T target) where T : Entity
		{
			Exceptions.ThrowIfNull(target, "target");
			if (typeof(T) == typeof(Entity))
			{
				Entity obj = ((Entity)(object)value).Merge((Entity)(object)target);
				return (T)(obj as T);
			}
			T val = (T)Activator.CreateInstance(typeof(T));
			((Entity)val).set_LogicalName(((Entity)value).get_LogicalName());
			((Entity)val).set_EntityState(((Entity)value).get_EntityState());
			((Entity)val).set_RowVersion(((Entity)value).get_RowVersion());
			((DataCollection<string, object>)(object)((Entity)val).get_Attributes()).AddRange((IEnumerable<KeyValuePair<string, object>>)((Entity)value).get_Attributes());
			((DataCollection<string, string>)(object)((Entity)val).get_FormattedValues()).AddRange((IEnumerable<KeyValuePair<string, string>>)((Entity)value).get_FormattedValues());
			((DataCollection<Relationship, EntityCollection>)(object)((Entity)val).get_RelatedEntities()).AddRange((IEnumerable<KeyValuePair<Relationship, EntityCollection>>)((Entity)value).get_RelatedEntities());
			((DataCollection<string, object>)(object)((Entity)val).get_KeyAttributes()).AddRange((IEnumerable<KeyValuePair<string, object>>)((Entity)value).get_KeyAttributes());
			((Entity)val).set_ExtensionData(((Entity)value).get_ExtensionData());
			foreach (KeyValuePair<string, object> item in (DataCollection<string, object>)(object)((Entity)target).get_Attributes())
			{
				((Entity)val).set_Item(item.Key, item.Value);
			}
			return val;
		}

		public static bool HasUpdatableAttributes(this Entity entity)
		{
			return ((DataCollection<string, object>)(object)entity.get_Attributes()).get_Count() <= UpdatableAttributes.Count() + 1 && ((IEnumerable<KeyValuePair<string, object>>)entity.get_Attributes()).Count((KeyValuePair<string, object> pair) => !UpdatableAttributes.Contains(pair.Key)) <= 1;
		}
	}
}
