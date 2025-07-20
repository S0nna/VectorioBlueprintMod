using System;
using System.Collections.Generic;

// Token: 0x0200021A RID: 538
public class Map<T1, T2>
{
	// Token: 0x06000FDC RID: 4060 RVA: 0x0004ABBD File Offset: 0x00048DBD
	public Map()
	{
		this.Forward = new Map<T1, T2>.Indexer<T1, T2>(this._forward);
		this.Reverse = new Map<T1, T2>.Indexer<T2, T1>(this._reverse);
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0004ABFD File Offset: 0x00048DFD
	public void Add(T1 t1, T2 t2)
	{
		this._forward.Add(t1, t2);
		this._reverse.Add(t2, t1);
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0004AC19 File Offset: 0x00048E19
	// (set) Token: 0x06000FDF RID: 4063 RVA: 0x0004AC21 File Offset: 0x00048E21
	public Map<T1, T2>.Indexer<T1, T2> Forward { get; private set; }

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0004AC2A File Offset: 0x00048E2A
	// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0004AC32 File Offset: 0x00048E32
	public Map<T1, T2>.Indexer<T2, T1> Reverse { get; private set; }

	// Token: 0x04000E01 RID: 3585
	private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();

	// Token: 0x04000E02 RID: 3586
	private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

	// Token: 0x0200021B RID: 539
	public class Indexer<T3, T4>
	{
		// Token: 0x06000FE2 RID: 4066 RVA: 0x0004AC3B File Offset: 0x00048E3B
		public Indexer(Dictionary<T3, T4> dictionary)
		{
			this._dictionary = dictionary;
		}

		// Token: 0x170001C5 RID: 453
		public T4 this[T3 index]
		{
			get
			{
				return this._dictionary[index];
			}
			set
			{
				this._dictionary[index] = value;
			}
		}

		// Token: 0x04000E05 RID: 3589
		private Dictionary<T3, T4> _dictionary;
	}
}
