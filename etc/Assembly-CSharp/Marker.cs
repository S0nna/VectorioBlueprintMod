using System;
using TMPro;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class Marker : MonoBehaviour
{
	// Token: 0x06000FE5 RID: 4069 RVA: 0x0004AC67 File Offset: 0x00048E67
	public void SetIcon(Sprite sprite, Vector2 scale)
	{
		this.icon.sprite = sprite;
		this.icon.transform.localScale = scale;
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x0004AC8B File Offset: 0x00048E8B
	public void SetTitle(string text)
	{
		this.title.text = text;
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x0004AC99 File Offset: 0x00048E99
	public void SetTitlePosition(Vector2 newPosition)
	{
		this.title.transform.localPosition = newPosition;
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x0004ACB4 File Offset: 0x00048EB4
	public void SetDesc(string text)
	{
		if (this.desc != null)
		{
			if (!this.desc.gameObject.activeSelf)
			{
				base.transform.localPosition = new Vector3(base.transform.position.x, base.transform.position.y + 5f, base.transform.position.z);
				this.desc.gameObject.SetActive(true);
			}
			this.desc.text = text;
		}
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0004AD44 File Offset: 0x00048F44
	public void Recycle()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000E06 RID: 3590
	public SpriteRenderer icon;

	// Token: 0x04000E07 RID: 3591
	public SpriteRenderer background;

	// Token: 0x04000E08 RID: 3592
	public TextMeshPro title;

	// Token: 0x04000E09 RID: 3593
	public TextMeshPro desc;
}
