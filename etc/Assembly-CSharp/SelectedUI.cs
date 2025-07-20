using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vectorio.PhasmaUI;

// Token: 0x020001F4 RID: 500
public class SelectedUI : Vectorio.PhasmaUI.Button
{
	// Token: 0x06000F4A RID: 3914 RVA: 0x00047B9C File Offset: 0x00045D9C
	public override void Start()
	{
		Singleton<Events>.Instance.onEntityEquipped.AddListener(new UnityAction<EntityData>(this.SetEntity));
		Singleton<Events>.Instance.onEntityUnequip.AddListener(new UnityAction(this.Clear));
		Singleton<Events>.Instance.onInventoryOpen.AddListener(new UnityAction(this.CheckIfClose));
		Singleton<Events>.Instance.onInventoryClose.AddListener(new UnityAction(this.CheckIfOpen));
		base.Start();
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00047C1C File Offset: 0x00045E1C
	public void SetEntity(EntityData entity)
	{
		this._entityData = entity;
		if (entity != null)
		{
			this.title.text = entity.Name.ToUpper();
			this.shortDesc.text = entity.ShortDescription;
			if (entity.UseNormalCost && entity.NormalCost != null)
			{
				this.normalResourceAmount.gameObject.SetActive(true);
				this.normalResourceIcon.gameObject.SetActive(true);
				this.normalResourceAmount.text = entity.NormalCost.amount.ToString();
				this.normalResourceIcon.sprite = entity.NormalCost.resource.IconSprite;
				this.divider.SetActive(true);
			}
			else
			{
				this.normalResourceAmount.gameObject.SetActive(false);
				this.normalResourceIcon.gameObject.SetActive(false);
				this.divider.SetActive(false);
			}
			if (entity.UseSpecialCost && entity.SpecialCost != null)
			{
				this.specialResourceAmount.gameObject.SetActive(true);
				this.specialResourceIcon.gameObject.SetActive(true);
				this.specialResourceAmount.text = entity.SpecialCost.amount.ToString();
				Sprite iconSprite = entity.SpecialCost.resource.IconSprite;
				if (entity.SpecialCost.resource == this.power)
				{
					iconSprite = this.powerIcon;
				}
				else if (entity.SpecialCost.resource == this.heat)
				{
					iconSprite = this.heatIcon;
				}
				this.specialResourceIcon.sprite = iconSprite;
			}
			else
			{
				this.specialResourceAmount.gameObject.SetActive(false);
				this.specialResourceIcon.gameObject.SetActive(false);
			}
			Color accentColor = entity.model.GetAccentColor(AccentType.PrimaryColor);
			this.digital.color = new Color(accentColor.r * 0.3f, accentColor.g * 0.3f, accentColor.b * 0.3f, 1f);
			this.background.color = accentColor;
			this.border.color = accentColor;
			if (this._model != null)
			{
				Object.Destroy(this._model);
			}
			this._model = Singleton<ModelConstructor>.Instance.BuildInterfaceModel(entity.model, this.modelSize, this.modelParent, null);
			this._model.transform.localScale = Vector2.one;
			if (!Inventory.IsOpen)
			{
				this.Open();
				return;
			}
		}
		else
		{
			this.Close();
		}
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x00047EA4 File Offset: 0x000460A4
	public void Open()
	{
		LeanTween.alphaCanvas(this.menuButton.group, 1f, 0.2f);
		LeanTween.moveLocal(this.menuButton.group.gameObject, this.menuButton.normalPos, 0.2f).setEase(LeanTweenType.easeOutExpo);
		this.menuButton.group.interactable = true;
		this.menuButton.group.blocksRaycasts = true;
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00047F20 File Offset: 0x00046120
	public void Close()
	{
		LeanTween.alphaCanvas(this.menuButton.group, 0f, 0.2f);
		LeanTween.moveLocal(this.menuButton.group.gameObject, this.menuButton.outPos, 0.2f).setEase(LeanTweenType.easeInExpo);
		this.menuButton.group.interactable = false;
		this.menuButton.group.blocksRaycasts = false;
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00047F9C File Offset: 0x0004619C
	public void Clear()
	{
		this.Close();
		this._entityData = null;
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00047FAB File Offset: 0x000461AB
	public void CheckIfOpen()
	{
		if (this._entityData != null)
		{
			this.SetEntity(this._entityData);
		}
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00047FC7 File Offset: 0x000461C7
	public void CheckIfClose()
	{
		if (this._entityData != null)
		{
			this.Close();
		}
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00047FDD File Offset: 0x000461DD
	public override void OnClick()
	{
		Singleton<Events>.Instance.onChangeBuildMode.Invoke(Hologram.BuildMode.Default);
	}

	// Token: 0x04000C99 RID: 3225
	public MenuButton menuButton;

	// Token: 0x04000C9A RID: 3226
	public TextMeshProUGUI title;

	// Token: 0x04000C9B RID: 3227
	public TextMeshProUGUI shortDesc;

	// Token: 0x04000C9C RID: 3228
	public TextMeshProUGUI normalResourceAmount;

	// Token: 0x04000C9D RID: 3229
	public TextMeshProUGUI specialResourceAmount;

	// Token: 0x04000C9E RID: 3230
	public Image border;

	// Token: 0x04000C9F RID: 3231
	public Image background;

	// Token: 0x04000CA0 RID: 3232
	public Image digital;

	// Token: 0x04000CA1 RID: 3233
	public Image normalResourceIcon;

	// Token: 0x04000CA2 RID: 3234
	public Image specialResourceIcon;

	// Token: 0x04000CA3 RID: 3235
	public GameObject divider;

	// Token: 0x04000CA4 RID: 3236
	public ResourceData power;

	// Token: 0x04000CA5 RID: 3237
	public ResourceData heat;

	// Token: 0x04000CA6 RID: 3238
	public Sprite powerIcon;

	// Token: 0x04000CA7 RID: 3239
	public Sprite heatIcon;

	// Token: 0x04000CA8 RID: 3240
	protected GameObject _model;

	// Token: 0x04000CA9 RID: 3241
	public Transform modelParent;

	// Token: 0x04000CAA RID: 3242
	public Vector2 modelSize;

	// Token: 0x04000CAB RID: 3243
	protected EntityData _entityData;
}
