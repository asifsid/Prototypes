using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.IdentityModel.Threading
{
	[ComVisible(true)]
	public abstract class AsyncResult : IAsyncResult, IDisposable
	{
		private AsyncCallback _callback;

		private bool _completed;

		private bool _completedSync;

		private bool _disposed;

		private bool _endCalled;

		private Exception _exception;

		private ManualResetEvent _event;

		private object _state;

		private object _thisLock;

		public object AsyncState => _state;

		public virtual WaitHandle AsyncWaitHandle
		{
			get
			{
				if (_event == null)
				{
					bool completed = _completed;
					lock (_thisLock)
					{
						if (_event == null)
						{
							_event = new ManualResetEvent(_completed);
						}
					}
					if (!completed && _completed)
					{
						_event.Set();
					}
				}
				return _event;
			}
		}

		public bool CompletedSynchronously => _completedSync;

		public bool IsCompleted => _completed;

		public static void End(IAsyncResult result)
		{
			if (result == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
			}
			AsyncResult asyncResult = result as AsyncResult;
			if (asyncResult == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID4001"), "result"));
			}
			if (asyncResult._endCalled)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4002")));
			}
			asyncResult._endCalled = true;
			if (!asyncResult._completed)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			if (asyncResult._event != null)
			{
				((IDisposable)asyncResult._event).Dispose();
			}
			if (asyncResult._exception != null)
			{
				throw asyncResult._exception;
			}
		}

		protected AsyncResult()
			: this(null, null)
		{
		}

		protected AsyncResult(object state)
			: this(null, state)
		{
		}

		protected AsyncResult(AsyncCallback callback, object state)
		{
			_thisLock = new object();
			_callback = callback;
			_state = state;
		}

		~AsyncResult()
		{
			Dispose(isExplicitDispose: false);
		}

		protected void Complete(bool completedSynchronously)
		{
			Complete(completedSynchronously, null);
		}

		protected void Complete(bool completedSynchronously, Exception exception)
		{
			if (_completed)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AsynchronousOperationException(SR.GetString("ID4005")));
			}
			_completedSync = completedSynchronously;
			_exception = exception;
			if (completedSynchronously)
			{
				_completed = true;
			}
			else
			{
				lock (_thisLock)
				{
					_completed = true;
					if (_event != null)
					{
						_event.Set();
					}
				}
			}
			try
			{
				if (_callback != null)
				{
					_callback(this);
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (AsynchronousOperationException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AsynchronousOperationException(SR.GetString("ID4003"), innerException));
			}
		}

		protected virtual void Dispose(bool isExplicitDispose)
		{
			if (_disposed || !isExplicitDispose)
			{
				return;
			}
			lock (_thisLock)
			{
				if (!_disposed)
				{
					_disposed = true;
					if (_event != null)
					{
						_event.Close();
					}
				}
			}
		}

		public void Dispose()
		{
			Dispose(isExplicitDispose: true);
			GC.SuppressFinalize(this);
		}
	}
}
