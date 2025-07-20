using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class SelectedDictionary : MonoBehaviour
{
	// Token: 0x060005F6 RID: 1526 RVA: 0x0001F27C File Offset: 0x0001D47C
	public void SetFilter(Type type)
	{
		this._filter = type;
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0001F285 File Offset: 0x0001D485
	public void ClearFilter()
	{
		this._filter = null;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0001F290 File Offset: 0x0001D490
	public void Add(Entity entity)
	{
		if (this._filter != null && entity.GetType() != this._filter)
		{
			return;
		}
		ulong key = (ulong)entity.RuntimeID;
		if (!this._selectionTable.ContainsKey(key))
		{
			this._selectionTable.Add(key, entity);
			entity.AddComponent<SelectedEntity>();
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001F2E9 File Offset: 0x0001D4E9
	public void Remove(ulong id)
	{
		if (this._selectionTable.ContainsKey(id))
		{
			Component component = this._selectionTable[id];
			this._selectionTable.Remove(id);
			Object.Destroy(component.GetComponent<SelectedEntity>());
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001F31C File Offset: 0x0001D51C
	public void RemoveAll()
	{
		foreach (KeyValuePair<ulong, Entity> keyValuePair in this._selectionTable)
		{
			if (keyValuePair.Value != null)
			{
				Object.Destroy(keyValuePair.Value.GetComponent<SelectedEntity>());
			}
		}
		this._selectionTable = new Dictionary<ulong, Entity>();
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001F394 File Offset: 0x0001D594
	public List<Entity> GetList()
	{
		List<Entity> list = new List<Entity>();
		foreach (KeyValuePair<ulong, Entity> keyValuePair in this._selectionTable)
		{
			if (keyValuePair.Value != null)
			{
				list.Add(keyValuePair.Value);
			}
		}
		return list;
	}

	// Token: 0x04000368 RID: 872
	protected Dictionary<ulong, Entity> _selectionTable = new Dictionary<ulong, Entity>();

	// Token: 0x04000369 RID: 873
	protected Type _filter;
}
