using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustRequestProcessingErrorEventArgs : EventArgs
	{
		private Exception _exception;

		private string _requestType;

		public Exception Exception => _exception;

		public string RequestType => _requestType;

		public WSTrustRequestProcessingErrorEventArgs(string requestType, Exception exception)
		{
			_exception = exception;
			_requestType = requestType;
		}
	}
}
