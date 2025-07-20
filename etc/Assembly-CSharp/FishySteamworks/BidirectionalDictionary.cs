using System;
using System.Collections;
using System.Collections.Generic;

namespace FishySteamworks
{
	// Token: 0x020002B5 RID: 693
	public class BidirectionalDictionary<T1, T2> : IEnumerable
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x00058AF0 File Offset: 0x00056CF0
		public IEnumerable<T1> FirstTypes
		{
			get
			{
				return this.t1ToT2Dict.Keys;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x00058AFD File Offset: 0x00056CFD
		public IEnumerable<T2> SecondTypes
		{
			get
			{
				return this.t2ToT1Dict.Keys;
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00058B0A File Offset: 0x00056D0A
		public IEnumerator GetEnumerator()
		{
			return this.t1ToT2Dict.GetEnumerator();
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00058B1C File Offset: 0x00056D1C
		public int Count
		{
			get
			{
				return this.t1ToT2Dict.Count;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x00058B29 File Offset: 0x00056D29
		public Dictionary<T1, T2> First
		{
			get
			{
				return this.t1ToT2Dict;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x00058B31 File Offset: 0x00056D31
		public Dictionary<T2, T1> Second
		{
			get
			{
				return this.t2ToT1Dict;
			}
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00058B39 File Offset: 0x00056D39
		public void Add(T1 key, T2 value)
		{
			if (this.t1ToT2Dict.ContainsKey(key))
			{
				this.Remove(key);
			}
			this.t1ToT2Dict[key] = value;
			this.t2ToT1Dict[value] = key;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00058B6A File Offset: 0x00056D6A
		public void Add(T2 key, T1 value)
		{
			if (this.t2ToT1Dict.ContainsKey(key))
			{
				this.Remove(key);
			}
			this.t2ToT1Dict[key] = value;
			this.t1ToT2Dict[value] = key;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00058B9B File Offset: 0x00056D9B
		public T2 Get(T1 key)
		{
			return this.t1ToT2Dict[key];
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00058BA9 File Offset: 0x00056DA9
		public T1 Get(T2 key)
		{
			return this.t2ToT1Dict[key];
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00058BB7 File Offset: 0x00056DB7
		public bool TryGetValue(T1 key, out T2 value)
		{
			return this.t1ToT2Dict.TryGetValue(key, out value);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00058BC6 File Offset: 0x00056DC6
		public bool TryGetValue(T2 key, out T1 value)
		{
			return this.t2ToT1Dict.TryGetValue(key, out value);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00058BD5 File Offset: 0x00056DD5
		public bool Contains(T1 key)
		{
			return this.t1ToT2Dict.ContainsKey(key);
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00058BE3 File Offset: 0x00056DE3
		public bool Contains(T2 key)
		{
			return this.t2ToT1Dict.ContainsKey(key);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00058BF4 File Offset: 0x00056DF4
		public void Remove(T1 key)
		{
			if (this.Contains(key))
			{
				T2 key2 = this.t1ToT2Dict[key];
				this.t1ToT2Dict.Remove(key);
				this.t2ToT1Dict.Remove(key2);
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00058C34 File Offset: 0x00056E34
		public void Remove(T2 key)
		{
			if (this.Contains(key))
			{
				T1 key2 = this.t2ToT1Dict[key];
				this.t1ToT2Dict.Remove(key2);
				this.t2ToT1Dict.Remove(key);
			}
		}

		// Token: 0x17000277 RID: 631
		public T1 this[T2 key]
		{
			get
			{
				return this.t2ToT1Dict[key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x17000278 RID: 632
		public T2 this[T1 key]
		{
			get
			{
				return this.t1ToT2Dict[key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x040010E9 RID: 4329
		private Dictionary<T1, T2> t1ToT2Dict = new Dictionary<T1, T2>();

		// Token: 0x040010EA RID: 4330
		private Dictionary<T2, T1> t2ToT1Dict = new Dictionary<T2, T1>();
	}
}
