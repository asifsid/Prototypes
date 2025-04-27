using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Controls
{
	[ComVisible(true)]
	public class ErrorEventArgs : CancelEventArgs
	{
		private Exception _exception;

		public Exception Exception => _exception;

		public ErrorEventArgs(Exception exception)
			: this(cancel: false, exception)
		{
		}

		public ErrorEventArgs(bool cancel, Exception exception)
			: base(cancel)
		{
			if (exception == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("exception");
			}
			_exception = exception;
		}
	}
}
