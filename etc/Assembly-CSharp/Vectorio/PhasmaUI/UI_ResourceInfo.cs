using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Vectorio.Formatting;

namespace Vectorio.PhasmaUI
{
	// Token: 0x02000290 RID: 656
	public class UI_ResourceInfo : MonoBehaviour
	{
		// Token: 0x06001291 RID: 4753 RVA: 0x00055C38 File Offset: 0x00053E38
		public void Set(ResourceData resource, float xPos)
		{
			this.title.text = resource.Name;
			this.desc.text = resource.Description;
			this.tier.text = resource.Tier.ToString();
			if (resource.Power > 0)
			{
				this.power.text = resource.Power.ToString() + "w";
				this.time.text = Formatter.Round(resource.BurnTime, 1) + "s";
			}
			else
			{
				this.power.text = "~";
				this.time.text = "~";
			}
			Color color = new Color(resource.Accent.secondaryColor.r * 0.5f, resource.Accent.secondaryColor.g * 0.5f, resource.Accent.secondaryColor.b * 0.5f, 1f);
			this.proceduralImage.color = resource.Accent.primaryColor;
			this.background.color = resource.Accent.primaryColor;
			this.digital.color = color;
			this.icon.sprite = resource.IconSprite;
			if (!this._isActive)
			{
				if (LeanTween.isTweening(this.buttonInfo.group.gameObject))
				{
					LeanTween.cancel(this.buttonInfo.group.gameObject);
				}
				Vector2 v = new Vector2(xPos, this.buttonInfo.inPos.y);
				Vector2 v2 = new Vector2(xPos, this.buttonInfo.normalPos.y);
				base.transform.localPosition = v;
				if (this.buttonInfo.sound != null)
				{
					Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.buttonInfo.sound);
				}
				this.buttonInfo.group.alpha = 0f;
				LeanTween.alphaCanvas(this.buttonInfo.group, 1f, 0.25f);
				LeanTween.moveLocal(base.transform.gameObject, v2, 0.25f).setEase(LeanTweenType.easeOutExpo);
				this._isActive = true;
				return;
			}
			if (LeanTween.isTweening(this.buttonInfo.group.gameObject))
			{
				LeanTween.cancel(this.buttonInfo.group.gameObject);
			}
			this.buttonInfo.group.alpha = 1f;
			base.transform.position = new Vector2(xPos, this.buttonInfo.normalPos.y);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x00055EE8 File Offset: 0x000540E8
		public void Disable()
		{
			if (this._isActive)
			{
				this._isActive = false;
				if (LeanTween.isTweening(this.buttonInfo.group.gameObject))
				{
					LeanTween.cancel(this.buttonInfo.group.gameObject);
				}
				Vector2 v = new Vector2(base.transform.localPosition.x, this.buttonInfo.outPos.y);
				LeanTween.alphaCanvas(this.buttonInfo.group, 0f, 0.25f);
				LeanTween.moveLocal(base.transform.gameObject, v, 0.25f).setEase(LeanTweenType.easeOutExpo);
			}
		}

		// Token: 0x04001043 RID: 4163
		public TextMeshProUGUI title;

		// Token: 0x04001044 RID: 4164
		public TextMeshProUGUI desc;

		// Token: 0x04001045 RID: 4165
		public TextMeshProUGUI tier;

		// Token: 0x04001046 RID: 4166
		public TextMeshProUGUI power;

		// Token: 0x04001047 RID: 4167
		public TextMeshProUGUI time;

		// Token: 0x04001048 RID: 4168
		public Image background;

		// Token: 0x04001049 RID: 4169
		public Image digital;

		// Token: 0x0400104A RID: 4170
		public Image icon;

		// Token: 0x0400104B RID: 4171
		public ProceduralImage proceduralImage;

		// Token: 0x0400104C RID: 4172
		public MenuButton buttonInfo;

		// Token: 0x0400104D RID: 4173
		private bool _isActive;
	}
}
