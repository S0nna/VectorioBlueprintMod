using System;
using TMPro;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class BorderZone : MonoBehaviour
{
	// Token: 0x06000DC5 RID: 3525 RVA: 0x0003D2E8 File Offset: 0x0003B4E8
	public void Set(string newText, string oldText, Color color, int size, Vector2Int position)
	{
		this.newSector.text = newText;
		this.oldSector.text = oldText;
		this.worldBorder.color = color;
		this.mapBorder.color = color;
		base.transform.position = new Vector3((float)(position.x * 5), (float)(position.y * 5), 1f);
		this.worldBorder.size = new Vector2((float)size, 1f);
		this.mapBorder.size = new Vector2((float)size, 1f);
	}

	// Token: 0x04000A07 RID: 2567
	public TextMeshPro newSector;

	// Token: 0x04000A08 RID: 2568
	public TextMeshPro oldSector;

	// Token: 0x04000A09 RID: 2569
	public SpriteRenderer worldBorder;

	// Token: 0x04000A0A RID: 2570
	public SpriteRenderer mapBorder;
}
