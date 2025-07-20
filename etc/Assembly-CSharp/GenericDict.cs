using System;
using System.Collections.Generic;

// Token: 0x02000212 RID: 530
public abstract class GenericDict
{
	// Token: 0x06000FBB RID: 4027 RVA: 0x0004A253 File Offset: 0x00048453
	public void Add<T>(string key, T value) where T : class
	{
		if (!this._dict.ContainsKey(key))
		{
			this._dict.Add(key, value);
			return;
		}
		this._dict[key] = value;
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x0004A288 File Offset: 0x00048488
	public T GetValue<T>(string key) where T : class
	{
		if (this._dict.ContainsKey(key))
		{
			return this._dict[key] as T;
		}
		return default(T);
	}

	// Token: 0x04000DC9 RID: 3529
	private Dictionary<string, object> _dict = new Dictionary<string, object>();
}
