using System.Runtime.InteropServices;

namespace Microsoft.Crm.Common.ObjectModel
{
	[ComVisible(true)]
	public enum ParticipationType
	{
		Sender = 1,
		Recipient,
		CCRecipient,
		BccRecipient,
		RequiredAttendee,
		OptionalAttendee,
		Organizer,
		Regarding,
		Owner,
		Resource,
		Customer,
		Partner
	}
}
