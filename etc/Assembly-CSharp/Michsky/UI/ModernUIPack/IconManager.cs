using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
	// Token: 0x02000309 RID: 777
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Modern UI Pack/Image/Icon Manager")]
	[RequireComponent(typeof(Image))]
	public class IconManager : MonoBehaviour
	{
		// Token: 0x06001535 RID: 5429 RVA: 0x000615AC File Offset: 0x0005F7AC
		private void Awake()
		{
			try
			{
				if (this.iconLibrary == null)
				{
					this.iconLibrary = Resources.Load<IconLibrary>("Icon Library");
				}
				if (this.imageObject == null)
				{
					this.imageObject = base.gameObject.GetComponent<Image>();
				}
				base.enabled = true;
				this.UpdateElement();
			}
			catch
			{
				Debug.LogWarning("<b>Icon Library</b> is missing, but it should be assigned.", this);
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00061624 File Offset: 0x0005F824
		private void Update()
		{
			if (this.iconLibrary.alwaysUpdate)
			{
				this.UpdateElement();
			}
			if (Application.isPlaying && this.iconLibrary.optimizeUpdates)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00061654 File Offset: 0x0005F854
		public void UpdateElement()
		{
			if (this.iconLibrary == null)
			{
				base.enabled = false;
				return;
			}
			int i = 0;
			while (i < this.iconLibrary.icons.Count)
			{
				if (this.selectedIconID == this.iconLibrary.icons[i].iconTitle && base.gameObject.activeInHierarchy)
				{
					if (this.spriteSize == 0)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite32;
						break;
					}
					if (this.spriteSize == 1)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite64;
						break;
					}
					if (this.spriteSize == 2)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite128;
						break;
					}
					if (this.spriteSize == 3)
					{
						this.imageObject.sprite = this.iconLibrary.icons[i].iconSprite256;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (!this.iconLibrary.alwaysUpdate)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00061794 File Offset: 0x0005F994
		public void UpdateSpriteSize(int spriteIndex, int newSize)
		{
			if (newSize == 0)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite32;
				return;
			}
			if (newSize == 1)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite64;
				return;
			}
			if (newSize == 2)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite128;
				return;
			}
			if (newSize == 3)
			{
				this.imageObject.sprite = this.iconLibrary.icons[spriteIndex].iconSprite256;
			}
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x00061838 File Offset: 0x0005FA38
		public void ChangeIcon(string newSprite, int preferredSize)
		{
			int num = -1;
			for (int i = 0; i < this.iconLibrary.icons.Count; i++)
			{
				if (newSprite == this.iconLibrary.icons[i].iconTitle)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				this.UpdateSpriteSize(num, preferredSize);
				return;
			}
			Debug.Log("<b>[Icon Manager]</b> Cannot find an icon named '" + newSprite + "'");
		}

		// Token: 0x0400133B RID: 4923
		public IconLibrary iconLibrary;

		// Token: 0x0400133C RID: 4924
		public string selectedIconID;

		// Token: 0x0400133D RID: 4925
		public int selectedIconIndex;

		// Token: 0x0400133E RID: 4926
		[Range(0f, 3f)]
		public int spriteSize;

		// Token: 0x0400133F RID: 4927
		private Image imageObject;

		// Token: 0x04001340 RID: 4928
		[HideInInspector]
		public string currentSize;

		// Token: 0x04001341 RID: 4929
		[HideInInspector]
		public bool size32;

		// Token: 0x04001342 RID: 4930
		[HideInInspector]
		public bool size64;

		// Token: 0x04001343 RID: 4931
		[HideInInspector]
		public bool size128;

		// Token: 0x04001344 RID: 4932
		[HideInInspector]
		public bool size256;
	}
}
