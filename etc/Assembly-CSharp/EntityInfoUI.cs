using System;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vectorio.PhasmaUI;
using Vectorio.Utilities;

// Token: 0x0200014A RID: 330
[DefaultExecutionOrder(1)]
public class EntityInfoUI : UI_Window
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0002D609 File Offset: 0x0002B809
	public Entity GetEntity
	{
		get
		{
			return this._entity;
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0002D611 File Offset: 0x0002B811
	public void Start()
	{
		this._canvasGroup = base.GetComponent<CanvasGroup>();
		Singleton<Events>.Instance.onEntityClicked.AddListener(new UnityAction<Entity>(this.Set));
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0002D63C File Offset: 0x0002B83C
	public void Update()
	{
		if (this._entity != null)
		{
			this.UpdateHealth(this._entity);
			this.entityOptions.CustomUpdate();
			if (this.canvasGroup.alpha == 1f)
			{
				this.previewCamera.transform.position = this._entity.transform.position;
			}
		}
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0002D6A0 File Offset: 0x0002B8A0
	private void UpdateHealth(Entity entity)
	{
		if (!this._usesHealth)
		{
			return;
		}
		this.health.text = "HEALTH: <size=6>" + this._damageReceiver.Health.ToString() + "/" + this._damageReceiver.MaxHealth.Value.ToString();
		this.healthBar.currentPercent = this._damageReceiver.Health;
		this.healthBar.maxValue = this._damageReceiver.MaxHealth.Value;
		this.healthBar.UpdateUI();
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0002D738 File Offset: 0x0002B938
	public void Set(Entity entity)
	{
		if (base.IsOpen)
		{
			return;
		}
		EntityData data = entity.GetData();
		this._entity = entity;
		this._usesHealth = entity.Has_EComponent<HealthComponent>();
		if (this._usesHealth)
		{
			this._damageReceiver = entity.Get_EComponent<HealthComponent>(false);
		}
		this.previewCamera.orthographicSize = data.previewSize;
		this.previewCamera.transform.position = entity.transform.position;
		this.previewCamera.targetTexture = this.cameraRenderTexture;
		if (!this.previewCamera.gameObject.activeSelf)
		{
			this.previewCamera.gameObject.SetActive(true);
		}
		this.title.text = data.Name.ToUpper();
		this.description.text = data.Description;
		this.UpdateHealth(entity);
		Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(entity.transform.position);
		this.coords.text = "X: " + vector2Int.x.ToString() + " / Y: " + vector2Int.y.ToString();
		if (this._stats != null && this._stats.Count > 0)
		{
			foreach (StatUI statUI in this._stats)
			{
				statUI.gameObject.SetActive(false);
			}
		}
		this.entityOptions.Set(this._entity);
		this.statList.Set(this._entity);
		Singleton<Interface>.Instance.CloseCurrentlyOpen();
		this.Open();
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0002D8EC File Offset: 0x0002BAEC
	public override void Open()
	{
		if (!Singleton<Interface>.Instance.CanOpenUI)
		{
			return;
		}
		base.SetIsOpen(true);
		Singleton<Interface>.Instance.SetCurrentlyOpen(this);
		if (this.canvasGroup.interactable)
		{
			return;
		}
		float num = 0f;
		foreach (MenuButton menuButton in this.menuButtons)
		{
			menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
			menuButton.group.alpha = 0f;
			LeanTween.alphaCanvas(menuButton.group, 1f, this.buttonAnimationSpeed).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, this.buttonAnimationSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.buttonAlphaCooldown);
		}
		this.previewCamera.enabled = true;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		if (this.openSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.openSound);
		}
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0002DA34 File Offset: 0x0002BC34
	public override void ForceOpen()
	{
		base.SetIsOpen(true);
		foreach (MenuButton menuButton in this.menuButtons)
		{
			menuButton.group.alpha = 1f;
			menuButton.group.transform.localPosition = menuButton.normalPos;
		}
		this.previewCamera.enabled = true;
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0002DAD8 File Offset: 0x0002BCD8
	public override void Close()
	{
		base.SetIsOpen(false);
		this.entityOptions.Clear();
		Singleton<Interface>.Instance.SetCurrentlyOpen(null);
		if (!this.canvasGroup.interactable)
		{
			return;
		}
		float num = 0f;
		foreach (MenuButton menuButton in this.menuButtons)
		{
			LeanTween.alphaCanvas(menuButton.group, 0f, this.buttonAnimationSpeed).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.outPos, this.buttonAnimationSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.buttonAlphaCooldown);
		}
		this.previewCamera.enabled = false;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		if (this.closeSound != null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.closeSound);
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0002DBEC File Offset: 0x0002BDEC
	public override void ForceClose()
	{
		base.SetIsOpen(false);
		this.entityOptions.Clear();
		foreach (MenuButton menuButton in this.menuButtons)
		{
			if (LeanTween.isTweening(menuButton.group.gameObject))
			{
				LeanTween.cancel(menuButton.group.gameObject);
			}
			menuButton.group.alpha = 0f;
			menuButton.group.transform.localPosition = menuButton.outPos;
		}
		this.previewCamera.enabled = false;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
	}

	// Token: 0x040006F3 RID: 1779
	protected Entity _entity;

	// Token: 0x040006F4 RID: 1780
	public EntityOptions entityOptions;

	// Token: 0x040006F5 RID: 1781
	public List<MenuButton> menuButtons;

	// Token: 0x040006F6 RID: 1782
	public float buttonAnimationSpeed;

	// Token: 0x040006F7 RID: 1783
	public float buttonAlphaCooldown;

	// Token: 0x040006F8 RID: 1784
	public TextMeshProUGUI title;

	// Token: 0x040006F9 RID: 1785
	public TextMeshProUGUI coords;

	// Token: 0x040006FA RID: 1786
	public TextMeshProUGUI description;

	// Token: 0x040006FB RID: 1787
	public TextMeshProUGUI health;

	// Token: 0x040006FC RID: 1788
	public RenderTexture cameraRenderTexture;

	// Token: 0x040006FD RID: 1789
	public ProgressBar healthBar;

	// Token: 0x040006FE RID: 1790
	public Camera previewCamera;

	// Token: 0x040006FF RID: 1791
	public StatList statList;

	// Token: 0x04000700 RID: 1792
	protected CanvasGroup _canvasGroup;

	// Token: 0x04000701 RID: 1793
	protected GameObject _model;

	// Token: 0x04000702 RID: 1794
	private HealthComponent _damageReceiver;

	// Token: 0x04000703 RID: 1795
	private bool _usesHealth;

	// Token: 0x04000704 RID: 1796
	public StatUI statPrefab;

	// Token: 0x04000705 RID: 1797
	public Transform statParent;

	// Token: 0x04000706 RID: 1798
	protected List<StatUI> _stats;
}
