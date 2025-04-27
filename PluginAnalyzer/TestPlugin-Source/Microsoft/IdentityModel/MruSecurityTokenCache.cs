using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public class MruSecurityTokenCache : SecurityTokenCache
	{
		internal struct CacheEntry
		{
			public SecurityToken value;

			public LinkedListNode<object> node;
		}

		public const int DefaultTokenCacheSize = 10000;

		private Dictionary<object, CacheEntry> _items;

		private int _maximumSize;

		private CacheEntry _mruEntry;

		private LinkedList<object> _mruList;

		private int _sizeAfterPurge;

		private object _syncRoot = new object();

		public int MaximumSize => _maximumSize;

		internal Dictionary<object, CacheEntry> Items => _items;

		public MruSecurityTokenCache()
			: this(10000)
		{
		}

		public MruSecurityTokenCache(int maximumSize)
			: this(maximumSize, null)
		{
		}

		public MruSecurityTokenCache(int maximumSize, IEqualityComparer<object> comparer)
			: this(maximumSize / 5 * 4, maximumSize, comparer)
		{
		}

		public MruSecurityTokenCache(int sizeAfterPurge, int maximumSize)
			: this(sizeAfterPurge, maximumSize, null)
		{
		}

		public MruSecurityTokenCache(int sizeAfterPurge, int maximumSize, IEqualityComparer<object> comparer)
		{
			if (sizeAfterPurge < 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0008"), "sizeAfterPurge"));
			}
			if (sizeAfterPurge >= maximumSize)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0009"), "sizeAfterPurge"));
			}
			_items = new Dictionary<object, CacheEntry>(maximumSize, comparer);
			_maximumSize = maximumSize;
			_mruList = new LinkedList<object>();
			_sizeAfterPurge = sizeAfterPurge;
		}

		public override void ClearEntries()
		{
			lock (_syncRoot)
			{
				_mruList.Clear();
				_items.Clear();
				_mruEntry.value = null;
				_mruEntry.node = null;
			}
		}

		public override bool TryRemoveEntry(object key)
		{
			if (key == null)
			{
				return false;
			}
			lock (_syncRoot)
			{
				if (_items.TryGetValue(key, out var value))
				{
					_items.Remove(key);
					_mruList.Remove(value.node);
					if (object.ReferenceEquals(_mruEntry.node, value.node))
					{
						_mruEntry.value = null;
						_mruEntry.node = null;
					}
					return true;
				}
			}
			return false;
		}

		public override bool TryRemoveAllEntries(object key)
		{
			if (key == null)
			{
				return false;
			}
			Dictionary<object, CacheEntry> dictionary = new Dictionary<object, CacheEntry>();
			lock (_syncRoot)
			{
				foreach (object key2 in _items.Keys)
				{
					if (key2.Equals(key))
					{
						dictionary.Add(key2, _items[key2]);
					}
				}
				foreach (object key3 in dictionary.Keys)
				{
					_items.Remove(key3);
					CacheEntry cacheEntry = dictionary[key3];
					_mruList.Remove(cacheEntry);
					if (object.ReferenceEquals(_mruEntry.node, cacheEntry.node))
					{
						_mruEntry.value = null;
						_mruEntry.node = null;
					}
				}
			}
			return dictionary.Count > 0;
		}

		public override bool TryAddEntry(object key, SecurityToken value)
		{
			bool flag;
			lock (_syncRoot)
			{
				flag = TryGetEntry(key, out var _);
				if (!flag)
				{
					Add(key, value);
				}
			}
			return !flag;
		}

		public override bool TryGetEntry(object key, out SecurityToken value)
		{
			lock (_syncRoot)
			{
				if (_mruEntry.node != null && key != null && key.Equals(_mruEntry.node.Value))
				{
					value = _mruEntry.value;
					return true;
				}
				CacheEntry value2;
				bool flag = _items.TryGetValue(key, out value2);
				value = value2.value;
				if (flag)
				{
					if (_mruList.Count > 1)
					{
						if (!object.ReferenceEquals(_mruList.First, value2.node))
						{
							_mruList.Remove(value2.node);
							_mruList.AddFirst(value2.node);
							_mruEntry = value2;
							return flag;
						}
						return flag;
					}
					return flag;
				}
				return flag;
			}
		}

		public override bool TryGetAllEntries(object key, out IList<SecurityToken> tokens)
		{
			tokens = new List<SecurityToken>();
			lock (_syncRoot)
			{
				foreach (object key2 in _items.Keys)
				{
					if (key2.Equals(key))
					{
						CacheEntry mruEntry = _items[key2];
						if (_mruList.Count > 1 && !object.ReferenceEquals(_mruList.First, mruEntry.node))
						{
							_mruList.Remove(mruEntry.node);
							_mruList.AddFirst(mruEntry.node);
							_mruEntry = mruEntry;
						}
						tokens.Add(mruEntry.value);
					}
				}
				return tokens.Count > 0;
			}
		}

		public override bool TryReplaceEntry(object key, SecurityToken newValue)
		{
			lock (_syncRoot)
			{
				return TryRemoveEntry(key) && TryAddEntry(key, newValue);
			}
		}

		private void Add(object key, SecurityToken value)
		{
			if (key == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("key");
			}
			bool flag = false;
			lock (_syncRoot)
			{
				try
				{
					if (_items.Count == _maximumSize)
					{
						int num = _maximumSize - _sizeAfterPurge;
						for (int i = 0; i < num; i++)
						{
							object value2 = _mruList.Last.Value;
							_mruList.RemoveLast();
							_items.Remove(value2);
						}
						flag = true;
					}
					CacheEntry cacheEntry = default(CacheEntry);
					cacheEntry.node = _mruList.AddFirst(key);
					cacheEntry.value = value;
					_items.Add(key, cacheEntry);
					_mruEntry = cacheEntry;
				}
				catch
				{
					throw;
				}
			}
			if (flag && DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("ID8003", _maximumSize, _sizeAfterPurge));
			}
		}
	}
}
