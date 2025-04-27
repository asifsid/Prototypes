using System;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public static class ActionCardTokenUtility
	{
		private const string titleSection = "title";

		private const string bodySection = "body";

		private const string minicardTextSection = "minicardtext";

		public static string GetReferenceTokensForMissedCloseDateCard(string name, int entityTypeCode, EntityReference regarding, DateTime closingDate)
		{
			Parameter item = new Parameter
			{
				Id = regarding.get_Id().ToString(),
				Name = name,
				TypeCode = entityTypeCode,
				Type = ParamType.EntityReference
			};
			Parameter item2 = new Parameter
			{
				Id = null,
				Name = closingDate.ToString("MMMM d, yyyy"),
				TypeCode = 0,
				Type = ParamType.DateTime
			};
			Token token = new Token();
			token.Params.Add(item);
			token.ResourceId = "ActionCard.MissedCloseDate.Title";
			Token token2 = new Token();
			token2.Params.Add(item2);
			token2.ResourceId = "ActionCard.MissedCloseDate.Body";
			Token token3 = new Token();
			token3.Params.Add(item);
			token3.ResourceId = "ActionCard.MissedCloseDate.MiniCardText";
			ReferenceTokens referenceTokens = new ReferenceTokens();
			referenceTokens.Tokens.Add("title", token);
			referenceTokens.Tokens.Add("body", token2);
			referenceTokens.Tokens.Add("minicardtext", token3);
			return JsonConvert.SerializeObject(referenceTokens);
		}

		public static string GetReferenceTokensForCloseDateComingSoonCard(string name, int entityTypeCode, EntityReference regarding, DateTime closingDate)
		{
			Parameter item = new Parameter
			{
				Id = regarding.get_Id().ToString(),
				Name = name,
				TypeCode = entityTypeCode,
				Type = ParamType.EntityReference
			};
			Parameter item2 = new Parameter
			{
				Id = null,
				Name = closingDate.ToString("MMMM dd, yyyy"),
				TypeCode = 0,
				Type = ParamType.DateTime
			};
			Token token = new Token();
			token.ResourceId = "ActionCard.CloseDateComingSoon.Title";
			token.Params.Add(item);
			Token token2 = new Token();
			token2.Params.Add(item2);
			token2.ResourceId = "ActionCard.CloseDateComingSoon.Body";
			Token token3 = new Token();
			token3.Params.Add(item);
			token3.Params.Add(item2);
			token3.ResourceId = "ActionCard.CloseDateComingSoon.MiniCardText";
			ReferenceTokens referenceTokens = new ReferenceTokens();
			referenceTokens.Tokens.Add("title", token);
			referenceTokens.Tokens.Add("body", token2);
			referenceTokens.Tokens.Add("minicardtext", token3);
			return JsonConvert.SerializeObject(referenceTokens);
		}

		public static string GetReferenceTokenForNonActivityCard(Entity entity, int typeCode, string entitySubject, string bodyResourceId)
		{
			Token token = new Token();
			Token token2 = new Token();
			Token token3 = new Token();
			Parameter item = new Parameter
			{
				Id = entity.get_Id().ToString(),
				Name = (entitySubject ?? string.Empty),
				TypeCode = typeCode,
				Type = ParamType.EntityReference
			};
			Parameter item2 = new Parameter
			{
				Id = null,
				Name = DateTime.UtcNow.ToString("s") + "Z",
				TypeCode = 0,
				Type = ParamType.DateTime
			};
			token.ResourceId = "ActionCard.NOActivity.Title";
			token.Params.Add(item);
			token2.ResourceId = bodyResourceId ?? string.Empty;
			token2.Params.Add(item2);
			token3.ResourceId = "ActionCard.NOActivity.MiniCardText";
			token3.Params.Add(item);
			token3.Params.Add(item2);
			ReferenceTokens referenceTokens = new ReferenceTokens();
			referenceTokens.Tokens.Add("title", token);
			referenceTokens.Tokens.Add("body", token2);
			referenceTokens.Tokens.Add("minicardtext", token3);
			return JsonConvert.SerializeObject(referenceTokens);
		}
	}
}
