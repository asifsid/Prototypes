using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Threading
{
	[ComVisible(true)]
	public class TypedAsyncResult<T> : AsyncResult
	{
		private T _result;

		public T Result => _result;

		public TypedAsyncResult(object state)
			: base(state)
		{
		}

		public TypedAsyncResult(AsyncCallback callback, object state)
			: base(callback, state)
		{
		}

		public void Complete(T result, bool completedSynchronously)
		{
			_result = result;
			Complete(completedSynchronously);
		}

		public void Complete(T result, bool completedSynchronously, Exception exception)
		{
			_result = result;
			Complete(completedSynchronously, exception);
		}

		public new static T End(IAsyncResult result)
		{
			if (result == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
			}
			TypedAsyncResult<T> typedAsyncResult = result as TypedAsyncResult<T>;
			if (typedAsyncResult == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("result", SR.GetString("ID2004", typeof(TypedAsyncResult<T>), result.GetType()));
			}
			AsyncResult.End(typedAsyncResult);
			return typedAsyncResult.Result;
		}
	}
}
