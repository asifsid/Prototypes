using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Dynamics.Solution.Localization
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Labels
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					ResourceManager resourceManager = (resourceMan = new ResourceManager("Microsoft.Dynamics.Solution.Common.Localization.Labels", typeof(Labels).Assembly));
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string AccessTokenExpired => ResourceManager.GetString("AccessTokenExpired", resourceCulture);

		internal static string AzureOperationResponseTimedOut => ResourceManager.GetString("AzureOperationResponseTimedOut", resourceCulture);

		internal static string AzureServiceConnectionTypeNotSupported => ResourceManager.GetString("AzureServiceConnectionTypeNotSupported", resourceCulture);

		internal static string BadRequest => ResourceManager.GetString("BadRequest", resourceCulture);

		internal static string CannotConvertAttributes => ResourceManager.GetString("CannotConvertAttributes", resourceCulture);

		internal static string CannotDoOperation => ResourceManager.GetString("CannotDoOperation", resourceCulture);

		internal static string CannotSetInexistentParameter => ResourceManager.GetString("CannotSetInexistentParameter", resourceCulture);

		internal static string CollectionIsEmpty => ResourceManager.GetString("CollectionIsEmpty", resourceCulture);

		internal static string ContextOutputParameterMissingOrNull => ResourceManager.GetString("ContextOutputParameterMissingOrNull", resourceCulture);

		internal static string ContextTargetNotPresentOrNotEntityType => ResourceManager.GetString("ContextTargetNotPresentOrNotEntityType", resourceCulture);

		internal static string Forbidden => ResourceManager.GetString("Forbidden", resourceCulture);

		internal static string HierarchyPathEllipsis => ResourceManager.GetString("HierarchyPathEllipsis", resourceCulture);

		internal static string InvalidArgument => ResourceManager.GetString("InvalidArgument", resourceCulture);

		internal static string InvalidGuidParameter => ResourceManager.GetString("InvalidGuidParameter", resourceCulture);

		internal static string InvalidOperation => ResourceManager.GetString("InvalidOperation", resourceCulture);

		internal static string InvalidParameter => ResourceManager.GetString("InvalidParameter", resourceCulture);

		internal static string InvalidPointer => ResourceManager.GetString("InvalidPointer", resourceCulture);

		internal static string InvalidProduct => ResourceManager.GetString("InvalidProduct", resourceCulture);

		internal static string InvalidRecurrenceRule => ResourceManager.GetString("InvalidRecurrenceRule", resourceCulture);

		internal static string InvalidStateCodeStatusCode => ResourceManager.GetString("InvalidStateCodeStatusCode", resourceCulture);

		internal static string InvalidStatusCodeForStateCode => ResourceManager.GetString("InvalidStatusCodeForStateCode", resourceCulture);

		internal static string LocalContextNotSpecified => ResourceManager.GetString("LocalContextNotSpecified", resourceCulture);

		internal static string LoopDetectionFailed => ResourceManager.GetString("LoopDetectionFailed", resourceCulture);

		internal static string MissingUomScheduleId => ResourceManager.GetString("MissingUomScheduleId", resourceCulture);

		internal static string NetworkIssue => ResourceManager.GetString("NetworkIssue", resourceCulture);

		internal static string NoConversionWillHappenForEntityWithoutAttributes => ResourceManager.GetString("NoConversionWillHappenForEntityWithoutAttributes", resourceCulture);

		internal static string NoIdentityMap => ResourceManager.GetString("NoIdentityMap", resourceCulture);

		internal static string ObjectDoesNotExist => ResourceManager.GetString("ObjectDoesNotExist", resourceCulture);

		internal static string ProductDoesNotExist => ResourceManager.GetString("ProductDoesNotExist", resourceCulture);

		internal static string ProductKitLoopBeingCreated => ResourceManager.GetString("ProductKitLoopBeingCreated", resourceCulture);

		internal static string ProductKitLoopExists => ResourceManager.GetString("ProductKitLoopExists", resourceCulture);

		internal static string StringIsEmpty => ResourceManager.GetString("StringIsEmpty", resourceCulture);

		internal static string StringIsNotEmpty => ResourceManager.GetString("StringIsNotEmpty", resourceCulture);

		internal static string TextAnalyticsAzureSchedulerError => ResourceManager.GetString("TextAnalyticsAzureSchedulerError", resourceCulture);

		internal static string TextAnalyticsFeatureNotEnabled => ResourceManager.GetString("TextAnalyticsFeatureNotEnabled", resourceCulture);

		internal static string unManagedidsdataaccessunexpected => ResourceManager.GetString("unManagedidsdataaccessunexpected", resourceCulture);

		internal static string UpdatePricingErrorCodeMessage => ResourceManager.GetString("UpdatePricingErrorCodeMessage", resourceCulture);

		internal static string InvalidUnitForProduct => ResourceManager.GetString("InvalidUnitForProduct", resourceCulture);

		internal Labels()
		{
		}
	}
}
