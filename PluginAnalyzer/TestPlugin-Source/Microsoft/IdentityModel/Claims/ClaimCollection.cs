using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public class ClaimCollection : IList<Claim>, ICollection<Claim>, IEnumerable<Claim>, IEnumerable
	{
		private List<Claim> _claims = new List<Claim>();

		private IClaimsIdentity _subject;

		public Claim this[int index]
		{
			get
			{
				return _claims[index];
			}
			set
			{
				if (index < 0 || index >= _claims.Count)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("index");
				}
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				if (!object.ReferenceEquals(value.Subject, _subject))
				{
					if (value.Subject != null && value.Subject.Claims != null)
					{
						value.Subject.Claims.Remove(value);
					}
					_claims[index] = value;
					value.SetSubject(_subject);
				}
			}
		}

		public int Count => _claims.Count;

		public bool IsReadOnly => false;

		public ClaimCollection(IClaimsIdentity subject)
		{
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			_subject = subject;
		}

		public void AddRange(IEnumerable<Claim> collection)
		{
			if (collection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("collection");
			}
			List<Claim> list = new List<Claim>();
			foreach (Claim item in collection)
			{
				list.Add(item);
			}
			foreach (Claim item2 in list)
			{
				Add(item2);
			}
		}

		public ClaimCollection CopyWithSubject(IClaimsIdentity subject)
		{
			ClaimCollection claimCollection = new ClaimCollection(subject);
			foreach (Claim claim in _claims)
			{
				claimCollection.Add(claim.Copy());
			}
			return claimCollection;
		}

		public void CopyRange(IEnumerable<Claim> collection)
		{
			if (collection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("collection");
			}
			AddRange(collection.Select((Claim claim) => new Claim(claim.ClaimType, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer)));
		}

		public bool Exists(Predicate<Claim> match)
		{
			return _claims.Exists(match);
		}

		public ICollection<Claim> FindAll(Predicate<Claim> match)
		{
			return _claims.FindAll(match);
		}

		public int IndexOf(Claim item)
		{
			return _claims.IndexOf(item);
		}

		public void Insert(int index, Claim item)
		{
			if (index < 0 || index >= _claims.Count)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("index");
			}
			if (item == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("item");
			}
			if (!object.ReferenceEquals(item.Subject, _subject))
			{
				if (item.Subject != null && item.Subject.Claims != null)
				{
					item.Subject.Claims.Remove(item);
				}
				_claims.Insert(index, item);
				item.SetSubject(_subject);
			}
		}

		public void RemoveAt(int index)
		{
			Claim item = _claims[index];
			Remove(item);
		}

		public void Add(Claim item)
		{
			if (item == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("item");
			}
			if (!object.ReferenceEquals(item.Subject, _subject))
			{
				if (item.Subject != null && item.Subject.Claims != null)
				{
					item.Subject.Claims.Remove(item);
				}
				_claims.Add(item);
				item.SetSubject(_subject);
			}
		}

		public void Clear()
		{
			_claims.ForEach(delegate(Claim claim)
			{
				claim.SetSubject(null);
			});
			_claims.Clear();
		}

		public bool Contains(Claim item)
		{
			return _claims.Contains(item);
		}

		public void CopyTo(Claim[] array, int arrayIndex)
		{
			_claims.CopyTo(array, arrayIndex);
		}

		public bool Remove(Claim item)
		{
			if (item == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("item");
			}
			if (item.Subject != _subject)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4028")));
			}
			if (!_claims.Remove(item))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4029")));
			}
			item.SetSubject(null);
			return true;
		}

		public IEnumerator<Claim> GetEnumerator()
		{
			return _claims.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_claims).GetEnumerator();
		}
	}
}
