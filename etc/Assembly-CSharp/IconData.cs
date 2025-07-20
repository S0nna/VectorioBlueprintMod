using System;
using UnityEngine;

// Token: 0x020000BB RID: 187
[CreateAssetMenu(fileName = "New Icon", menuName = "Vectorio/Icon")]
public class IconData : BaseData
{
	// Token: 0x060006A6 RID: 1702 RVA: 0x0001FF9C File Offset: 0x0001E19C
	private void OnIconUpdated()
	{
		string name = this.sprite.name.Replace("icon_", "").ToLower();
		base.SetBaseData(name, "", "icon_");
	}

	// Token: 0x04000419 RID: 1049
	public Sprite sprite;
}
