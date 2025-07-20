using System;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B5 RID: 437
public class EntityInfo : MonoBehaviour
{
	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0003E731 File Offset: 0x0003C931
	public bool IsEnabled
	{
		get
		{
			return this._isEnabled;
		}
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0003E739 File Offset: 0x0003C939
	public void Setup(Canvas canvas, ResourceTooltip resourceInfo)
	{
		this._canvas = canvas;
		this._resourceInfo = resourceInfo;
		base.GetComponent<RectTransform>().localScale = new Vector2(0.5f, 0.5f);
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0003E778 File Offset: 0x0003C978
	public void CustomUpdate(Entity entity)
	{
		Vector2 v;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas.transform as RectTransform, Input.mousePosition, this._canvas.worldCamera, out v))
		{
			base.transform.position = this._canvas.transform.TransformPoint(v);
		}
		if (entity == null)
		{
			this.Hide();
			return;
		}
		if (!this._isEnabled)
		{
			this._isEnabled = true;
			if (LeanTween.isTweening(base.gameObject))
			{
				LeanTween.cancel(base.gameObject);
			}
			LeanTween.alphaCanvas(this.canvasGroup, 1f, 0.25f);
			LeanTween.scale(base.gameObject, Vector2.one, 0.25f);
		}
		this.title.text = entity.GetData().Name;
		this.runtimeID.text = entity.RuntimeID.ToString();
		this.desc.text = entity.GetData().Description;
		Color color = entity.GetModel.GetColor(AccentType.PrimaryColor);
		this.border.color = color;
		Color color2 = new Color(color.r * 0.2f, color.g * 0.2f, color.b * 0.2f);
		this.title.color = color2;
		this.background.color = color2;
		this.banner.color = new Color(color.r * 0.1f, color.g * 0.1f, color.b * 0.1f);
		if (entity.Has_EComponent<HealthComponent>())
		{
			HealthComponent healthComponent = entity.Get_EComponent<HealthComponent>(false);
			this.health.text = healthComponent.Health.ToString() + "/" + healthComponent.MaxHealth.Value.ToString();
			this.progressBar.maxValue = healthComponent.MaxHealth.Value;
			this.progressBar.currentPercent = healthComponent.MaxHealth.Value;
			this.progressBar.UpdateUI();
		}
		if (!this.noRepairObject.activeSelf)
		{
			this.repairObject.SetActive(false);
			this.noRepairObject.SetActive(true);
			this.repairEntity.gameObject.SetActive(false);
		}
		bool flag = entity.GetComponentValues().Count > 0;
		this.settingsObject.SetActive(flag);
		this.lineTransform.sizeDelta = new Vector2(this.lineTransform.sizeDelta.x, this.normalLineSize);
		float y = flag ? this.settingsPanelSize : this.normalPanelSize;
		this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, y);
		this.border.sprite = (flag ? this.settingsBorder : this.normalBorder);
		if (!true)
		{
			flag = (entity.GetComponentValues().Count > 0);
			this.rectTransform.sizeDelta = new Vector2(this.rectTransform.sizeDelta.x, flag ? this.settingsPanelSize : this.normalPanelSize);
			this.border.sprite = (flag ? this.settingsBorder : this.normalBorder);
			this.settingsObject.SetActive(flag);
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x0003EACC File Offset: 0x0003CCCC
	private void Hide()
	{
		if (this._isEnabled)
		{
			this._isEnabled = false;
			if (LeanTween.isTweening(base.gameObject))
			{
				LeanTween.cancel(base.gameObject);
			}
			LeanTween.alphaCanvas(this.canvasGroup, 0f, 0.25f);
			LeanTween.scale(base.gameObject, new Vector2(0.5f, 0.5f), 0.25f);
		}
	}

	// Token: 0x04000AA9 RID: 2729
	public CanvasGroup canvasGroup;

	// Token: 0x04000AAA RID: 2730
	public GameObject settingsObject;

	// Token: 0x04000AAB RID: 2731
	public GameObject repairObject;

	// Token: 0x04000AAC RID: 2732
	public GameObject noRepairObject;

	// Token: 0x04000AAD RID: 2733
	public Image border;

	// Token: 0x04000AAE RID: 2734
	public Image background;

	// Token: 0x04000AAF RID: 2735
	public Image banner;

	// Token: 0x04000AB0 RID: 2736
	public Image repairIcon;

	// Token: 0x04000AB1 RID: 2737
	public TextMeshProUGUI title;

	// Token: 0x04000AB2 RID: 2738
	public TextMeshProUGUI runtimeID;

	// Token: 0x04000AB3 RID: 2739
	public TextMeshProUGUI desc;

	// Token: 0x04000AB4 RID: 2740
	public TextMeshProUGUI viewInfo;

	// Token: 0x04000AB5 RID: 2741
	public TextMeshProUGUI copySettings;

	// Token: 0x04000AB6 RID: 2742
	public TextMeshProUGUI repairEntity;

	// Token: 0x04000AB7 RID: 2743
	public TextMeshProUGUI settingsTitle;

	// Token: 0x04000AB8 RID: 2744
	public TextMeshProUGUI settingsDesc;

	// Token: 0x04000AB9 RID: 2745
	public TextMeshProUGUI health;

	// Token: 0x04000ABA RID: 2746
	public TextMeshProUGUI repairCost;

	// Token: 0x04000ABB RID: 2747
	public float normalPanelSize;

	// Token: 0x04000ABC RID: 2748
	public float settingsPanelSize;

	// Token: 0x04000ABD RID: 2749
	public float repairNormalPanelSize;

	// Token: 0x04000ABE RID: 2750
	public float repairSettingsPanelSize;

	// Token: 0x04000ABF RID: 2751
	public float normalLineSize;

	// Token: 0x04000AC0 RID: 2752
	public float repairLineSize;

	// Token: 0x04000AC1 RID: 2753
	public Sprite normalBorder;

	// Token: 0x04000AC2 RID: 2754
	public Sprite settingsBorder;

	// Token: 0x04000AC3 RID: 2755
	public Sprite repairNormalBorder;

	// Token: 0x04000AC4 RID: 2756
	public Sprite repairSettingsBorder;

	// Token: 0x04000AC5 RID: 2757
	public RectTransform rectTransform;

	// Token: 0x04000AC6 RID: 2758
	public RectTransform lineTransform;

	// Token: 0x04000AC7 RID: 2759
	public ProgressBar progressBar;

	// Token: 0x04000AC8 RID: 2760
	protected Canvas _canvas;

	// Token: 0x04000AC9 RID: 2761
	protected ResourceTooltip _resourceInfo;

	// Token: 0x04000ACA RID: 2762
	protected bool _isEnabled;
}
