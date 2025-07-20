using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vectorio.Formatting;

// Token: 0x020001EB RID: 491
public class ResourceTooltip : MonoBehaviour
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000F2D RID: 3885 RVA: 0x00046E29 File Offset: 0x00045029
	public bool IsEnabled
	{
		get
		{
			return this._isEnabled;
		}
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00046E31 File Offset: 0x00045031
	public void Setup(Canvas canvas, EntityInfo entityInfo)
	{
		this._canvas = canvas;
		this._entityInfo = entityInfo;
		base.GetComponent<RectTransform>().localScale = new Vector2(0.5f, 0.5f);
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00046E70 File Offset: 0x00045070
	public void CustomUpdate(ResourceData data)
	{
		Vector2 v;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas.transform as RectTransform, Input.mousePosition, this._canvas.worldCamera, out v))
		{
			base.transform.position = this._canvas.transform.TransformPoint(v);
		}
		if (!(data == null))
		{
			if (LeanTween.isTweening(this._entityInfo.gameObject))
			{
				LeanTween.cancelAll(this._entityInfo.gameObject);
				this._entityInfo.canvasGroup.alpha = 0f;
			}
			if (!this._isEnabled)
			{
				this._resource = data;
				this.title.text = data.Name.ToUpper();
				this.description.text = data.Description;
				this.powerText.text = "<b>POWER VALUE:</b> " + data.Power.ToString() + "w";
				this.timeText.text = "<b>BURN TIME:</b> " + Formatter.Time(data.BurnTime, false) + "s";
				this.tier.sprite = data.IconSprite;
				Color primaryColor = data.Accent.primaryColor;
				this.border.color = primaryColor;
				Color color = new Color(primaryColor.r * 0.2f, primaryColor.g * 0.2f, primaryColor.b * 0.2f);
				this.tier.color = color;
				this.background.color = color;
				this.title.color = color;
				this.banner.color = new Color(primaryColor.r * 0.1f, primaryColor.g * 0.1f, primaryColor.b * 0.1f);
				if (this.canvasGroup.alpha < 1f)
				{
					LeanTween.cancel(base.gameObject);
					LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.25f);
					LeanTween.scale(base.gameObject, Vector2.one, 0.25f);
				}
				this._isEnabled = true;
			}
			return;
		}
		if (this._isEnabled)
		{
			this._isEnabled = false;
		}
		if (this._resource == null)
		{
			return;
		}
		this._resource = null;
		if (this.canvasGroup.alpha > 0f)
		{
			LeanTween.cancel(base.gameObject);
			LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.25f);
			LeanTween.scale(base.gameObject, new Vector2(0.5f, 0.5f), 0.25f);
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00047118 File Offset: 0x00045318
	public void AnimateBanner()
	{
		if (LeanTween.isTweening(base.gameObject))
		{
			LeanTween.cancel(base.gameObject);
		}
		LeanTween.scale(base.gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.25f);
		LeanTween.scale(base.gameObject, Vector3.one, 0.25f).setDelay(0.25f);
	}

	// Token: 0x04000C59 RID: 3161
	public CanvasGroup canvasGroup;

	// Token: 0x04000C5A RID: 3162
	public RectTransform rectTransform;

	// Token: 0x04000C5B RID: 3163
	public TextMeshProUGUI title;

	// Token: 0x04000C5C RID: 3164
	public TextMeshProUGUI description;

	// Token: 0x04000C5D RID: 3165
	public TextMeshProUGUI powerText;

	// Token: 0x04000C5E RID: 3166
	public TextMeshProUGUI timeText;

	// Token: 0x04000C5F RID: 3167
	public Image tier;

	// Token: 0x04000C60 RID: 3168
	public Image border;

	// Token: 0x04000C61 RID: 3169
	public Image banner;

	// Token: 0x04000C62 RID: 3170
	public Image background;

	// Token: 0x04000C63 RID: 3171
	protected Canvas _canvas;

	// Token: 0x04000C64 RID: 3172
	protected EntityInfo _entityInfo;

	// Token: 0x04000C65 RID: 3173
	protected ResourceData _resource;

	// Token: 0x04000C66 RID: 3174
	protected bool _isEnabled;
}
