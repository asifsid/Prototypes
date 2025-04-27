using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class StatusCodeValidator
	{
		private const int DefaultStatusCodeForStateCode = -1;

		private const string AttributeStateCode = "statecode";

		private const string AttributeStatusCode = "statuscode";

		public static void ValidateForCreate(Entity entity, EntityMetadata entityMetadata)
		{
			AttributeMetadata val = entityMetadata.get_Attributes().FirstOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statecode"));
			AttributeMetadata val2 = entityMetadata.get_Attributes().FirstOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statuscode"));
			if (val != null && val2 != null && !entity.IsAttributeNull("statuscode"))
			{
				StateAttributeMetadata val3 = val as StateAttributeMetadata;
				int stateCode = (((EnumAttributeMetadata)val3).get_DefaultFormValue().HasValue ? ((EnumAttributeMetadata)val3).get_DefaultFormValue().Value : 0);
				if (!entity.IsAttributeNull("statecode"))
				{
					stateCode = (int)entity.get_Item("statecode");
				}
				Validate(entityMetadata, stateCode, (int)entity.get_Item("statuscode"), entity.get_Id());
			}
		}

		public static void ValidateForUpdate(Entity entity, EntityMetadata entityMetadata, IOrganizationService orgService)
		{
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			AttributeMetadata val = entityMetadata.get_Attributes().FirstOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statecode"));
			AttributeMetadata val2 = entityMetadata.get_Attributes().FirstOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statuscode"));
			if (val == null || val2 == null || entity.IsAttributeNull("statuscode"))
			{
				return;
			}
			int stateCode;
			if (!entity.IsAttributeNull("statecode"))
			{
				stateCode = (int)entity.get_Item("statecode");
			}
			else
			{
				Entity val3 = orgService.Retrieve(entity.get_LogicalName(), entity.get_Id(), new ColumnSet(new string[2] { "statecode", "statuscode" }));
				stateCode = (int)val3.get_Item("statecode");
			}
			if ((int)entity.get_Item("statuscode") == -1)
			{
				StateAttributeMetadata val4 = val as StateAttributeMetadata;
				OptionMetadata obj = ((IEnumerable<OptionMetadata>)((EnumAttributeMetadata)val4).get_OptionSet().get_Options()).FirstOrDefault((OptionMetadata x) => x.get_Value() == stateCode);
				StateOptionMetadata val5 = obj as StateOptionMetadata;
				entity.set_Item("statuscode", (object)val5.get_DefaultStatus());
			}
			else
			{
				Validate(entityMetadata, stateCode, (int)entity.get_Item("statuscode"), entity.get_Id());
			}
		}

		public static void Validate(EntityMetadata entityMetadata, int stateCode, int statusCode, Guid businessEntityId)
		{
			if (entityMetadata.get_Attributes().Any((AttributeMetadata x) => x.get_LogicalName().Equals("statecode")) && !Validate(entityMetadata, stateCode))
			{
				throw new CrmArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} is not a valid state code on {1} with Id {2}.", stateCode, entityMetadata.get_LogicalName(), businessEntityId), "statecode", -2147187704);
			}
			if (statusCode == -1)
			{
				return;
			}
			AttributeMetadata val = entityMetadata.get_Attributes().FirstOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statuscode"));
			if (val == null && statusCode != -1)
			{
				throw new CrmArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} is not a valid status code on {1} with Id: {2}.", statusCode, entityMetadata.get_LogicalName(), businessEntityId), "statuscode", -2147187704);
			}
			StatusAttributeMetadata val2 = val as StatusAttributeMetadata;
			OptionMetadata obj = ((IEnumerable<OptionMetadata>)((EnumAttributeMetadata)val2).get_OptionSet().get_Options()).FirstOrDefault((OptionMetadata x) => x.get_Value() == statusCode);
			StatusOptionMetadata val3 = obj as StatusOptionMetadata;
			if (val3 == null)
			{
				throw new CrmArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} is not a valid status code on {1} with Id {2}.", statusCode, entityMetadata.get_LogicalName(), businessEntityId), "statuscode", -2147187704);
			}
			if (val3.get_State() == stateCode)
			{
				return;
			}
			AttributeMetadata obj2 = entityMetadata.get_Attributes().FirstOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statecode"));
			StateAttributeMetadata val4 = obj2 as StateAttributeMetadata;
			OptionMetadata val5 = ((IEnumerable<OptionMetadata>)((EnumAttributeMetadata)val4).get_OptionSet().get_Options()).FirstOrDefault((OptionMetadata x) => x.get_Value() == stateCode);
			throw new CrmArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} is not a valid status code for state code {1}State.{2} on {3} with Id {4}.", statusCode, entityMetadata.get_SchemaName(), ((IEnumerable<LocalizedLabel>)val5.get_Label().get_LocalizedLabels()).First().get_Label(), entityMetadata.get_LogicalName(), businessEntityId), "statuscode", -2147187704);
		}

		public static bool Validate(EntityMetadata entityMetadata, int stateCode)
		{
			AttributeMetadata val = entityMetadata.get_Attributes().SingleOrDefault((AttributeMetadata x) => x.get_LogicalName().Equals("statecode"));
			if (val != null)
			{
				StateAttributeMetadata val2 = val as StateAttributeMetadata;
				if (val2 != null && !((IEnumerable<OptionMetadata>)((EnumAttributeMetadata)val2).get_OptionSet().get_Options()).Any((OptionMetadata x) => x.get_Value() == stateCode))
				{
					return false;
				}
			}
			return true;
		}
	}
}
