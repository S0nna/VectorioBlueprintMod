using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000143 RID: 323
public class AttackWarning : MonoBehaviour
{
	// Token: 0x06000AB4 RID: 2740 RVA: 0x0002CF92 File Offset: 0x0002B192
	public void Start()
	{
		Singleton<Events>.Instance.onBuildingDamaged.AddListener(new UnityAction<Building, float>(this.OnBuildingDamaged));
		Singleton<Events>.Instance.onBuildingDestroyed.AddListener(new UnityAction<Building>(this.OnBuildingDestroyed));
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x0002CFCC File Offset: 0x0002B1CC
	public void Update()
	{
		if (this._isActivated)
		{
			this._cooldown -= Time.deltaTime;
			if (this._trigger)
			{
				this.softWarningGroup.alpha += Time.deltaTime * this._speed;
				this.hardWarningGroup.alpha = this.softWarningGroup.alpha;
				if (this.softWarningGroup.alpha >= 1f)
				{
					this._trigger = false;
				}
			}
			else
			{
				this.softWarningGroup.alpha -= Time.deltaTime * this._speed;
				this.hardWarningGroup.alpha = this.softWarningGroup.alpha;
				if (this.softWarningGroup.alpha <= 0f)
				{
					this._trigger = true;
				}
			}
			if (this._cooldown <= 0f)
			{
				LeanTween.moveLocal(this.iconButton.group.gameObject, this.iconButton.outPos, 0.5f).setEase(LeanTweenType.easeInExpo);
				this.position = Singleton<WorldGenerator>.Instance.CenterWorldPos;
				this._isActivated = false;
				this._isWarning = false;
			}
		}
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x00003212 File Offset: 0x00001412
	public void OnBuildingDamaged(Building building, float damage)
	{
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00003212 File Offset: 0x00001412
	public void OnBuildingDestroyed(Building building)
	{
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x0002D0F7 File Offset: 0x0002B2F7
	public void OnViewWarning()
	{
		Singleton<Events>.Instance.onMoveCameraToPosition.Invoke(this.position);
	}

	// Token: 0x040006BE RID: 1726
	protected Vector2 position;

	// Token: 0x040006BF RID: 1727
	protected GameObject _model;

	// Token: 0x040006C0 RID: 1728
	public MenuButton iconButton;

	// Token: 0x040006C1 RID: 1729
	public CanvasGroup canvasGroup;

	// Token: 0x040006C2 RID: 1730
	public GameObject softWarningIcon;

	// Token: 0x040006C3 RID: 1731
	public GameObject hardWarningIcon;

	// Token: 0x040006C4 RID: 1732
	public CanvasGroup softWarningGroup;

	// Token: 0x040006C5 RID: 1733
	public CanvasGroup hardWarningGroup;

	// Token: 0x040006C6 RID: 1734
	public Transform modelParent;

	// Token: 0x040006C7 RID: 1735
	public AudioClip warningSound;

	// Token: 0x040006C8 RID: 1736
	protected float _speed = 2f;

	// Token: 0x040006C9 RID: 1737
	protected float _cooldown = 8f;

	// Token: 0x040006CA RID: 1738
	protected bool _isWarning;

	// Token: 0x040006CB RID: 1739
	protected bool _isActivated;

	// Token: 0x040006CC RID: 1740
	protected bool _trigger;
}
