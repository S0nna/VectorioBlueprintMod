using System;
using System.Collections.Generic;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x020001F9 RID: 505
public class StatList : MonoBehaviour
{
	// Token: 0x06000F5D RID: 3933 RVA: 0x000489A9 File Offset: 0x00046BA9
	public void Set(Entity entity)
	{
		this.Set(entity.Stats);
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x000489B7 File Offset: 0x00046BB7
	public void Set(EntityData entity)
	{
		this.Set(entity.RetrieveStats());
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x000489C8 File Offset: 0x00046BC8
	public void Set(List<Stat> stats)
	{
		this.Clear();
		this.colorAlternate = false;
		for (int i = 0; i < stats.Count; i++)
		{
			Stat stat = stats[i];
			if (stat.Code != 0 && stat.Code != 1)
			{
				StatInfo statInfo = StatLibrary.FetchInfo(stat.Code);
				if (statInfo == null)
				{
					Debug.Log("[STAT UI] Cannot display info for stat with ID " + stat.Code.ToString());
				}
				else
				{
					StatUI statUI;
					if (i < this._stats.Count)
					{
						statUI = this._stats[i];
						statUI.gameObject.SetActive(true);
					}
					else
					{
						statUI = Object.Instantiate<StatUI>(this.statUIPrefab);
						statUI.transform.SetParent(this.list);
						statUI.GetComponent<RectTransform>().localScale = Vector2.one;
						this._stats.Add(statUI);
					}
					statUI.Set(stat, statInfo, this.colorAlternate ? this.colorOne : this.colorTwo);
					this.colorAlternate = !this.colorAlternate;
				}
			}
		}
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00048AE0 File Offset: 0x00046CE0
	private void Clear()
	{
		foreach (StatUI statUI in this._stats)
		{
			statUI.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000CEC RID: 3308
	public Transform list;

	// Token: 0x04000CED RID: 3309
	public StatUI statUIPrefab;

	// Token: 0x04000CEE RID: 3310
	public Color colorOne;

	// Token: 0x04000CEF RID: 3311
	public Color colorTwo;

	// Token: 0x04000CF0 RID: 3312
	private bool colorAlternate;

	// Token: 0x04000CF1 RID: 3313
	private List<StatUI> _stats = new List<StatUI>();

	// Token: 0x04000CF2 RID: 3314
	private StatUI failedStat;
}
