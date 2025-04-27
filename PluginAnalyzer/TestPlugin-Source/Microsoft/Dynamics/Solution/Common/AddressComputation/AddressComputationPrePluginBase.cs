using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.Dynamics.CRM.Common;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common.AddressComputation
{
	[ComVisible(true)]
	public class AddressComputationPrePluginBase : PluginBase
	{
		private const string FetchXml = "fetch/entity/attribute[@name='";

		public AddressComputationPrePluginBase(Type pluginType)
			: base(pluginType)
		{
		}

		protected sealed override void ExecuteCrmPlugin(LocalPluginContext context)
		{
			Exceptions.ThrowIfNull(context, Labels.LocalContextNotSpecified);
			if (context.PluginExecutionContext != null)
			{
				RetrieveColumnSetBasedOnMessage(context.PluginExecutionContext);
			}
		}

		private void RetrieveColumnSetBasedOnMessage(IPluginExecutionContext context)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			XrmTelemetryContext.AddCustomProperty("AddressComputationSdkMessage", ((IExecutionContext)context).get_MessageName());
			if (((IExecutionContext)context).get_MessageName() == "Retrieve")
			{
				ColumnSet columnSet = (ColumnSet)((DataCollection<string, object>)(object)((IExecutionContext)context).get_InputParameters()).get_Item("ColumnSet");
				AddAttributestoColumnSet(columnSet);
			}
			else if (((IExecutionContext)context).get_MessageName() == "RetrieveMultiple")
			{
				QueryBase val = (QueryBase)((DataCollection<string, object>)(object)((IExecutionContext)context).get_InputParameters()).get_Item("Query");
				if (val is QueryExpression)
				{
					QueryExpression val2 = (QueryExpression)val;
					ColumnSet columnSet = val2.get_ColumnSet();
					AddAttributestoColumnSet(columnSet);
					XrmTelemetryContext.AddCustomProperty("AddressComputationQueryType", "QueryExpression");
				}
				else if (val is QueryByAttribute)
				{
					QueryByAttribute val3 = (QueryByAttribute)val;
					ColumnSet columnSet = val3.get_ColumnSet();
					AddAttributestoColumnSet(columnSet);
					XrmTelemetryContext.AddCustomProperty("AddressComputationQueryType", "QueryByAttribute");
				}
				else
				{
					FetchExpression val4 = (FetchExpression)val;
					string query = val4.get_Query();
					query = UpdateFetchXml(query);
					val4.set_Query(query);
					XrmTelemetryContext.AddCustomProperty("AddressComputationQueryType", "FetchExpression");
				}
			}
		}

		private void AddAttributestoColumnSet(ColumnSet columnSet)
		{
			if (columnSet != null)
			{
				if (((Collection<string>)(object)columnSet.get_Columns()).Contains(InternalAddress.AttributeAddress1_Composite))
				{
					AddColumnsToColumnSet(InternalAddress.Address1AttributeList, columnSet);
				}
				if (((Collection<string>)(object)columnSet.get_Columns()).Contains(InternalAddress.AttributeAddress2_Composite))
				{
					AddColumnsToColumnSet(InternalAddress.Address2AttributeList, columnSet);
				}
				if (((Collection<string>)(object)columnSet.get_Columns()).Contains(InternalAddress.AttributeAddress3_Composite))
				{
					AddColumnsToColumnSet(InternalAddress.Address3AttributeList, columnSet);
				}
				if (((Collection<string>)(object)columnSet.get_Columns()).Contains(InternalAddress.AttributeBillTo_Composite))
				{
					AddColumnsToColumnSet(InternalAddress.BillToAttributeList, columnSet);
				}
				if (((Collection<string>)(object)columnSet.get_Columns()).Contains(InternalAddress.AttributeShipTo_Composite))
				{
					AddColumnsToColumnSet(InternalAddress.ShipToAttributeList, columnSet);
				}
			}
		}

		private void AddColumnsToColumnSet(IList<string> attributeList, ColumnSet columnSet)
		{
			foreach (string attribute in attributeList)
			{
				if (!((Collection<string>)(object)columnSet.get_Columns()).Contains(attribute))
				{
					columnSet.AddColumn(attribute);
				}
			}
		}

		private string UpdateFetchXml(string query)
		{
			XmlDocument xmlDocument = XmlUtility.CreateXmlDocument(query);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("fetch/entity/attribute[@name='" + InternalAddress.AttributeAddress1_Composite + "']");
			if (xmlNode != null)
			{
				xmlDocument = AddAttributesToFetchXml(xmlDocument, InternalAddress.Address1AttributeList);
			}
			xmlNode = xmlDocument.SelectSingleNode("fetch/entity/attribute[@name='" + InternalAddress.AttributeAddress2_Composite + "']");
			if (xmlNode != null)
			{
				xmlDocument = AddAttributesToFetchXml(xmlDocument, InternalAddress.Address2AttributeList);
			}
			xmlNode = xmlDocument.SelectSingleNode("fetch/entity/attribute[@name='" + InternalAddress.AttributeAddress3_Composite + "']");
			if (xmlNode != null)
			{
				xmlDocument = AddAttributesToFetchXml(xmlDocument, InternalAddress.Address3AttributeList);
			}
			xmlNode = xmlDocument.SelectSingleNode("fetch/entity/attribute[@name='" + InternalAddress.AttributeBillTo_Composite + "']");
			if (xmlNode != null)
			{
				xmlDocument = AddAttributesToFetchXml(xmlDocument, InternalAddress.BillToAttributeList);
			}
			xmlNode = xmlDocument.SelectSingleNode("fetch/entity/attribute[@name='" + InternalAddress.AttributeShipTo_Composite + "']");
			if (xmlNode != null)
			{
				xmlDocument = AddAttributesToFetchXml(xmlDocument, InternalAddress.ShipToAttributeList);
			}
			return xmlDocument.OuterXml;
		}

		private XmlDocument AddAttributesToFetchXml(XmlDocument xmlDoc, IList<string> attributeList)
		{
			XmlNode xmlNode = xmlDoc.SelectSingleNode("fetch/entity");
			foreach (string attribute in attributeList)
			{
				XmlNode xmlNode2 = xmlDoc.SelectSingleNode("fetch/entity/attribute[@name='" + attribute + "']");
				if (xmlNode2 == null)
				{
					XmlNode xmlNode3 = xmlDoc.CreateNode(XmlNodeType.Element, "attribute", string.Empty);
					XmlNode xmlNode4 = xmlDoc.CreateNode(XmlNodeType.Attribute, "name", string.Empty);
					xmlNode4.Value = attribute;
					xmlNode3.Attributes.SetNamedItem(xmlNode4);
					xmlNode.AppendChild(xmlNode3);
				}
			}
			return xmlDoc;
		}
	}
}
