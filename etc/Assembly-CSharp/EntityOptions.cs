using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class EntityOptions : MonoBehaviour
{
	// Token: 0x06000E0E RID: 3598 RVA: 0x0003EB3C File Offset: 0x0003CD3C
	public void Set(Entity entity)
	{
		bool flag = false;
		foreach (EntityOptions.Settings settings in this.settings)
		{
			Type type = Type.GetType(settings.entityComponent);
			EntityComponent component;
			if (type != null && entity.TryGet_EComponent(type, out component))
			{
				if (flag)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.dividerPrefab);
					gameObject.transform.SetParent(this.settingsList);
					gameObject.transform.localScale = Vector2.one;
					this._dividers.Push(gameObject);
				}
				EntitySettings entitySettings = Object.Instantiate<EntitySettings>(settings.prefab);
				entitySettings.transform.SetParent(this.settingsList);
				entitySettings.transform.localScale = Vector2.one;
				entitySettings.Set(component);
				if (entitySettings.gameObject.activeSelf)
				{
					this._activeSettings.Push(entitySettings);
				}
				if (!flag)
				{
					flag = true;
				}
			}
		}
		this.noSettings.SetActive(this._activeSettings.Count == 0);
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0003EC74 File Offset: 0x0003CE74
	public void CustomUpdate()
	{
		foreach (EntitySettings entitySettings in this._activeSettings)
		{
			entitySettings.CustomUpdate();
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x0003ECC4 File Offset: 0x0003CEC4
	public void Clear()
	{
		while (this._activeSettings.Count > 0)
		{
			Object.Destroy(this._activeSettings.Pop().gameObject);
		}
		while (this._dividers.Count > 0)
		{
			Object.Destroy(this._dividers.Pop().gameObject);
		}
	}

	// Token: 0x04000ACB RID: 2763
	public GameObject noSettings;

	// Token: 0x04000ACC RID: 2764
	public GameObject dividerPrefab;

	// Token: 0x04000ACD RID: 2765
	public Transform settingsList;

	// Token: 0x04000ACE RID: 2766
	[SerializeField]
	public List<EntityOptions.Settings> settings;

	// Token: 0x04000ACF RID: 2767
	private Stack<EntitySettings> _activeSettings = new Stack<EntitySettings>();

	// Token: 0x04000AD0 RID: 2768
	private Stack<GameObject> _dividers = new Stack<GameObject>();

	// Token: 0x020001B7 RID: 439
	[Serializable]
	public class Settings
	{
		// Token: 0x04000AD1 RID: 2769
		public EntitySettings prefab;

		// Token: 0x04000AD2 RID: 2770
		public string entityComponent;
	}
}
