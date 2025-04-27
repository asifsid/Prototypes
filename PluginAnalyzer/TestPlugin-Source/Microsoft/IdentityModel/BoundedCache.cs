using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace Microsoft.IdentityModel
{
	internal class BoundedCache<T> where T : class
	{
		protected class ExpirableItem<ET>
		{
			private DateTime _expirationTime;

			private ET _item;

			public ET Item => _item;

			public ExpirableItem(ET item, DateTime expirationTime)
			{
				_item = item;
				if (expirationTime.Kind != DateTimeKind.Utc)
				{
					_expirationTime = DateTimeUtil.ToUniversalTime(expirationTime);
				}
				else
				{
					_expirationTime = expirationTime;
				}
			}

			public bool IsExpired()
			{
				return _expirationTime <= DateTime.UtcNow;
			}
		}

		[Flags]
		internal enum CachingMode
		{
			Time = 0x0,
			MRU = 0x1,
			FIFO = 0x2
		}

		private Dictionary<string, ExpirableItem<T>> _items;

		private int _capacity;

		private TimeSpan _purgeInterval;

		private ReaderWriterLock _readWriteLock;

		private DateTime _lastPurgeTime = DateTime.UtcNow;

		protected ReaderWriterLock CacheLock => _readWriteLock;

		public virtual int Capacity
		{
			get
			{
				return _capacity;
			}
			set
			{
				if (value <= 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", value, SR.GetString("ID0002"));
				}
				_capacity = value;
			}
		}

		protected Dictionary<string, ExpirableItem<T>> Items => _items;

		public TimeSpan PurgeInterval
		{
			get
			{
				return _purgeInterval;
			}
			set
			{
				if (value <= TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", value, SR.GetString("ID0016"));
				}
				_purgeInterval = value;
			}
		}

		public BoundedCache(int capacity, TimeSpan purgeInterval)
			: this(capacity, purgeInterval, (IEqualityComparer<string>)StringComparer.Ordinal)
		{
		}

		public BoundedCache(int capacity, TimeSpan purgeInterval, IEqualityComparer<string> keyComparer)
		{
			if (capacity <= 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("capacity", capacity, SR.GetString("ID0002"));
			}
			if (purgeInterval <= TimeSpan.Zero)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("purgeInterval", purgeInterval, SR.GetString("ID0016"));
			}
			if (keyComparer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyComparer");
			}
			_capacity = capacity;
			_purgeInterval = purgeInterval;
			_items = new Dictionary<string, ExpirableItem<T>>(keyComparer);
			_readWriteLock = new ReaderWriterLock();
		}

		public virtual void Clear()
		{
			_readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
			try
			{
				_items.Clear();
			}
			finally
			{
				_readWriteLock.ReleaseWriterLock();
			}
		}

		private void EnforceQuota()
		{
			if (_capacity == int.MaxValue || _items.Count < _capacity)
			{
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new QuotaExceededException(SR.GetString("ID0021", _capacity)));
		}

		public virtual int IncreaseCapacity(int size)
		{
			if (size <= 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("size", size, SR.GetString("ID0002"));
			}
			_readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
			try
			{
				if (int.MaxValue - size <= _capacity)
				{
					_capacity = int.MaxValue;
				}
				else
				{
					_capacity += size;
				}
				return _capacity;
			}
			finally
			{
				_readWriteLock.ReleaseWriterLock();
			}
		}

		private void Purge()
		{
			if (DateTime.UtcNow < DateTimeUtil.Add(_lastPurgeTime, _purgeInterval))
			{
				return;
			}
			_lastPurgeTime = DateTime.UtcNow;
			_readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
			try
			{
				List<string> list = new List<string>();
				foreach (string key in _items.Keys)
				{
					if (_items[key].IsExpired())
					{
						list.Add(key);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					_items.Remove(list[i]);
				}
			}
			finally
			{
				_readWriteLock.ReleaseWriterLock();
			}
		}

		public virtual bool TryAdd(string key, T item, DateTime expirationTime)
		{
			Purge();
			_readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
			EnforceQuota();
			try
			{
				if (_items.ContainsKey(key))
				{
					return false;
				}
				_items[key] = new ExpirableItem<T>(item, expirationTime);
				return true;
			}
			finally
			{
				_readWriteLock.ReleaseWriterLock();
			}
		}

		public virtual bool TryFind(string key)
		{
			Purge();
			_readWriteLock.AcquireReaderLock(TimeSpan.FromMilliseconds(-1.0));
			try
			{
				if (_items.ContainsKey(key))
				{
					return true;
				}
				return false;
			}
			finally
			{
				_readWriteLock.ReleaseReaderLock();
			}
		}

		public virtual bool TryGet(string key, out T item)
		{
			Purge();
			item = null;
			_readWriteLock.AcquireReaderLock(TimeSpan.FromMilliseconds(-1.0));
			try
			{
				if (!_items.ContainsKey(key))
				{
					return false;
				}
				item = _items[key].Item;
				return true;
			}
			finally
			{
				_readWriteLock.ReleaseReaderLock();
			}
		}

		public virtual bool TryRemove(string key)
		{
			Purge();
			_readWriteLock.AcquireWriterLock(TimeSpan.FromMilliseconds(-1.0));
			try
			{
				if (!_items.ContainsKey(key))
				{
					return false;
				}
				_items.Remove(key);
				return true;
			}
			finally
			{
				_readWriteLock.ReleaseWriterLock();
			}
		}
	}
}
