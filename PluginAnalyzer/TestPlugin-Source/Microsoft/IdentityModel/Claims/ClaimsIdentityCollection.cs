using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public class ClaimsIdentityCollection : IList<IClaimsIdentity>, ICollection<IClaimsIdentity>, IEnumerable<IClaimsIdentity>, IEnumerable
	{
		private List<IClaimsIdentity> _collection = new List<IClaimsIdentity>();

		public IClaimsIdentity this[int index]
		{
			get
			{
				return _collection[index];
			}
			set
			{
				_collection[index] = value;
			}
		}

		public int Count => _collection.Count;

		public bool IsReadOnly => false;

		public ClaimsIdentityCollection()
		{
		}

		public ClaimsIdentityCollection(IEnumerable<IClaimsIdentity> subjects)
		{
			if (subjects != null)
			{
				_collection.AddRange(subjects);
			}
		}

		public ClaimsIdentityCollection Copy()
		{
			ClaimsIdentityCollection claimsIdentityCollection = new ClaimsIdentityCollection();
			using IEnumerator<IClaimsIdentity> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				IClaimsIdentity current = enumerator.Current;
				claimsIdentityCollection.Add(current.Copy());
			}
			return claimsIdentityCollection;
		}

		public void AddRange(IEnumerable<IClaimsIdentity> collection)
		{
			if (collection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("collection");
			}
			_collection.AddRange(collection);
		}

		public int IndexOf(IClaimsIdentity item)
		{
			return _collection.IndexOf(item);
		}

		public void Insert(int index, IClaimsIdentity item)
		{
			if (item == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("item");
			}
			_collection.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_collection.RemoveAt(index);
		}

		public void Add(IClaimsIdentity item)
		{
			if (item == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("item");
			}
			_collection.Add(item);
		}

		public void Clear()
		{
			_collection.Clear();
		}

		public bool Contains(IClaimsIdentity item)
		{
			return _collection.Contains(item);
		}

		public void CopyTo(IClaimsIdentity[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("array");
			}
			_collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(IClaimsIdentity item)
		{
			return _collection.Remove(item);
		}

		public IEnumerator<IClaimsIdentity> GetEnumerator()
		{
			return _collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_collection).GetEnumerator();
		}
	}
}
