using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class OrganizationServiceExtensions
	{
		private const int MaxNumberOfRecordsPerResultPage = 5000;

		public static EntityCollection RetrieveMultipleWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, QueryBase query)
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName, stage))
			{
				return organizationService.RetrieveMultiple(query);
			}
		}

		public static EntityCollection RetrieveMultipleWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, QueryBase query)
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName))
			{
				return organizationService.RetrieveMultiple(query);
			}
		}

		public static EntityCollection RetrieveMultiplePagedWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, QueryExpression query)
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName))
			{
				return organizationService.RetrieveMultiplePaged(query);
			}
		}

		public static EntityCollection RetrieveMultiplePagedWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, QueryExpression query)
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName, stage))
			{
				return organizationService.RetrieveMultiplePaged(query);
			}
		}

		public static EntityCollection RetrieveMultiplePagedWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, FetchExpression expression)
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName))
			{
				return organizationService.RetrieveMultiplePaged(expression);
			}
		}

		public static EntityCollection RetrieveMultiplePagedWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, FetchExpression expression)
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName, stage))
			{
				return organizationService.RetrieveMultiplePaged(expression);
			}
		}

		public static IEnumerable<T> RetrieveMultiplePagedWithSkipPluginsSet<T>(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, QueryExpression query) where T : Entity
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName))
			{
				return organizationService.RetrieveMultiplePaged<T>(query);
			}
		}

		public static IEnumerable<T> RetrieveMultiplePagedWithSkipPluginsSet<T>(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, QueryExpression query) where T : Entity
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName, stage))
			{
				return organizationService.RetrieveMultiplePaged<T>(query);
			}
		}

		public static IEnumerable<T> RetrieveMultipleWithSkipPluginsSet<T>(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, QueryExpression query) where T : Entity
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName))
			{
				return organizationService.RetrieveMultiple<T>((QueryBase)(object)query);
			}
		}

		public static IEnumerable<T> RetrieveMultipleWithSkipPluginsSet<T>(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, QueryExpression query) where T : Entity
		{
			using (pluginContext.SetSkipRetrieveMultiplePlugins(entityName, stage))
			{
				return organizationService.RetrieveMultiple<T>((QueryBase)(object)query);
			}
		}

		public static T RetrieveWithSkipPluginsSet<T>(this IOrganizationService organizationService, IPluginContext pluginContext, Guid entityId, ColumnSet columns) where T : Entity
		{
			object obj = typeof(T).GetCustomAttributes(typeof(EntityLogicalNameAttribute), inherit: true).FirstOrDefault();
			string logicalName = (obj as EntityLogicalNameAttribute).get_LogicalName();
			using (pluginContext.SetSkipRetrievePlugins(logicalName))
			{
				return organizationService.Retrieve<T>(entityId, columns);
			}
		}

		public static T RetrieveWithSkipPluginsSet<T>(this IOrganizationService organizationService, IPluginContext pluginContext, int? stage, Guid entityId, ColumnSet columns) where T : Entity
		{
			object obj = typeof(T).GetCustomAttributes(typeof(EntityLogicalNameAttribute), inherit: true).FirstOrDefault();
			string logicalName = (obj as EntityLogicalNameAttribute).get_LogicalName();
			using (pluginContext.SetSkipRetrievePlugins(logicalName, stage))
			{
				return organizationService.Retrieve<T>(entityId, columns);
			}
		}

		public static Entity RetrieveWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, Guid entityId, ColumnSet columns)
		{
			using (pluginContext.SetSkipRetrievePlugins(entityName))
			{
				return organizationService.Retrieve(entityName, entityId, columns);
			}
		}

		public static Entity RetrieveWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, int? stage, string entityName, Guid entityId, ColumnSet columns)
		{
			using (pluginContext.SetSkipRetrievePlugins(entityName, stage))
			{
				return organizationService.Retrieve(entityName, entityId, columns);
			}
		}

		public static Guid CreateWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, Entity entity)
		{
			using (pluginContext.SetSkipCreatePlugins(entityName))
			{
				return organizationService.Create(entity);
			}
		}

		public static Guid CreateWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, Entity entity)
		{
			using (pluginContext.SetSkipCreatePlugins(entityName, stage))
			{
				return organizationService.Create(entity);
			}
		}

		public static void DeleteWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, Guid id)
		{
			using (pluginContext.SetSkipDeletePlugins(entityName))
			{
				organizationService.Delete(entityName, id);
			}
		}

		public static void DeleteWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityName, int? stage, Guid id)
		{
			using (pluginContext.SetSkipDeletePlugins(entityName, stage))
			{
				organizationService.Delete(entityName, id);
			}
		}

		public static OrganizationResponse ExecuteWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string messageName, string entityname, OrganizationRequest request)
		{
			using (pluginContext.SetSkipPlugins(messageName, entityname))
			{
				return organizationService.Execute(request);
			}
		}

		public static OrganizationResponse ExecuteWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string messageName, string entityname, int? stage, OrganizationRequest request)
		{
			using (pluginContext.SetSkipPlugins(messageName, entityname, stage))
			{
				return organizationService.Execute(request);
			}
		}

		public static void UpdateWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityname, Entity entity)
		{
			using (pluginContext.SetSkipUpdatePlugins(entityname))
			{
				organizationService.Update(entity);
			}
		}

		public static void UpdateWithSkipPluginsSet(this IOrganizationService organizationService, IPluginContext pluginContext, string entityname, int? stage, Entity entity)
		{
			using (pluginContext.SetSkipUpdatePlugins(entityname, stage))
			{
				organizationService.Update(entity);
			}
		}

		public static T Retrieve<T>(this IOrganizationService organizationService, Guid entityId, ColumnSet columns) where T : Entity
		{
			object obj = typeof(T).GetCustomAttributes(typeof(EntityLogicalNameAttribute), inherit: true).FirstOrDefault();
			string logicalName = (obj as EntityLogicalNameAttribute).get_LogicalName();
			Entity val = organizationService.Retrieve(logicalName, entityId, columns);
			return val.ToEntity<T>();
		}

		public static bool HasResults(this IOrganizationService organizationService, QueryBase query)
		{
			EntityCollection val = organizationService.RetrieveMultiple(query);
			return ((Collection<Entity>)(object)val.get_Entities()).Count > 0;
		}

		public static IEnumerable<T> RetrieveMultiple<T>(this IOrganizationService organizationService, QueryBase query) where T : Entity
		{
			EntityCollection val = organizationService.RetrieveMultiple(query);
			return ((IEnumerable<Entity>)val.get_Entities()).Select((Entity entity) => entity.ToEntity<T>());
		}

		public static IEnumerable<T> RetrieveMultiplePaged<T>(this IOrganizationService organizationService, QueryExpression query) where T : Entity
		{
			query.set_PageInfo(CreateFirstPagePagingInfo(5000));
			EntityCollection returnCollection;
			do
			{
				returnCollection = organizationService.RetrieveMultiple((QueryBase)(object)query);
				if (returnCollection.get_Entities() != null)
				{
					foreach (Entity entity in (Collection<Entity>)(object)returnCollection.get_Entities())
					{
						yield return entity.ToEntity<T>();
					}
				}
				if (returnCollection.get_MoreRecords())
				{
					SetPagingInfoForNextPageRetrieval(query.get_PageInfo(), returnCollection.get_PagingCookie());
				}
			}
			while (returnCollection.get_MoreRecords());
		}

		public static EntityCollection RetrieveMultiplePaged(this IOrganizationService organizationService, QueryExpression query)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			query.set_PageInfo(CreateFirstPagePagingInfo(5000));
			EntityCollection val = new EntityCollection();
			EntityCollection val2;
			do
			{
				val2 = organizationService.RetrieveMultiple((QueryBase)(object)query);
				if (val2.get_Entities() != null)
				{
					val.get_Entities().AddRange((IEnumerable<Entity>)val2.get_Entities());
				}
				if (val2.get_MoreRecords())
				{
					SetPagingInfoForNextPageRetrieval(query.get_PageInfo(), val2.get_PagingCookie());
				}
			}
			while (val2.get_MoreRecords());
			return val;
		}

		public static IEnumerable<T> RetrieveMultiplePaged<T>(this IOrganizationService organizationService, FetchExpression expression) where T : Entity
		{
			EntityCollection returnCollection = organizationService.RetrieveMultiplePaged(expression);
			if (returnCollection.get_Entities() == null)
			{
				yield break;
			}
			foreach (Entity entity in (Collection<Entity>)(object)returnCollection.get_Entities())
			{
				yield return entity.ToEntity<T>();
			}
		}

		public static EntityCollection RetrieveMultiplePaged(this IOrganizationService organizationService, FetchExpression expression)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			int num = 1;
			string text = CreatePagingFetchXml(expression.get_Query(), num);
			EntityCollection val = new EntityCollection();
			EntityCollection val2;
			do
			{
				val2 = organizationService.RetrieveMultiple((QueryBase)new FetchExpression(text));
				if (val2.get_Entities() != null)
				{
					val.get_Entities().AddRange((IEnumerable<Entity>)val2.get_Entities());
				}
				if (val2.get_MoreRecords())
				{
					text = CreatePagingFetchXml(expression.get_Query(), ++num, val2.get_PagingCookie());
				}
			}
			while (val2.get_MoreRecords());
			return val;
		}

		private static PagingInfo CreateFirstPagePagingInfo(int numberOfRecordsPerResultPage)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			PagingInfo val = new PagingInfo();
			val.set_Count(numberOfRecordsPerResultPage);
			val.set_PageNumber(1);
			val.set_PagingCookie((string)null);
			return val;
		}

		private static void SetPagingInfoForNextPageRetrieval(PagingInfo pagingInfo, string pagingCookie)
		{
			int pageNumber = pagingInfo.get_PageNumber();
			pagingInfo.set_PageNumber(pageNumber + 1);
			pagingInfo.set_PagingCookie(pagingCookie);
		}

		private static string CreatePagingFetchXml(string xml, int page, string cookie = "")
		{
			StringReader input = new StringReader(xml);
			XmlTextReader reader = new XmlTextReader(input);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
			return CreatePagingFetchXml(xmlDocument, cookie, page, 5000);
		}

		private static string CreatePagingFetchXml(XmlDocument doc, string cookie, int page, int count)
		{
			XmlAttributeCollection attributes = doc.DocumentElement.Attributes;
			if (cookie != null)
			{
				XmlAttribute xmlAttribute = doc.CreateAttribute("paging-cookie");
				xmlAttribute.Value = cookie;
				attributes.Append(xmlAttribute);
			}
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("page");
			xmlAttribute2.Value = Convert.ToString(page);
			attributes.Append(xmlAttribute2);
			XmlAttribute xmlAttribute3 = doc.CreateAttribute("count");
			xmlAttribute3.Value = Convert.ToString(count);
			attributes.Append(xmlAttribute3);
			StringBuilder stringBuilder = new StringBuilder(1024);
			StringWriter w = new StringWriter(stringBuilder);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			doc.WriteTo(xmlTextWriter);
			xmlTextWriter.Close();
			return stringBuilder.ToString();
		}
	}
}
