using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class AnimationManager : Singleton<AnimationManager>
{
	// Token: 0x06000450 RID: 1104 RVA: 0x000168DB File Offset: 0x00014ADB
	public void RegisterAnimation(IAnimateable animateable)
	{
		if (animateable.IsAnimating)
		{
			return;
		}
		this._animateables.Add(animateable);
		animateable.IsAnimating = true;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x000168FC File Offset: 0x00014AFC
	public void Update()
	{
		this.UpdatePlacementAnimations();
		if (this.pipetteSquare != null && this.pipetteSquare.color.a > 0f)
		{
			if (this._copySquare)
			{
				this.pipetteSquare.transform.localScale = new Vector2(this.pipetteSquare.transform.localScale.x + this.pipetteSizeSpeed * Time.deltaTime, this.pipetteSquare.transform.localScale.y + this.pipetteSizeSpeed * Time.deltaTime);
			}
			else
			{
				this.pipetteSquare.transform.localScale = new Vector2(this.pipetteSquare.transform.localScale.x - this.pipetteSizeSpeed * Time.deltaTime, this.pipetteSquare.transform.localScale.y - this.pipetteSizeSpeed * Time.deltaTime);
			}
			this.pipetteSquare.color = new Color(1f, 1f, 1f, this.pipetteSquare.color.a - this.pipetteAlphaSpeed * Time.deltaTime);
		}
		for (int i = 0; i < this._animateables.Count; i++)
		{
			if (this._animateables[i].Animate(Time.deltaTime))
			{
				this._animateables[i].ResetAnimation();
				this._animateables[i].IsAnimating = false;
				this._animateables.RemoveAt(i);
				i--;
			}
		}
		this._numbersTracked = 0;
		for (int j = this._lastNumberIndex; j < this.activeNumbers.Count; j++)
		{
			if (++this._numbersTracked > 50)
			{
				this._lastNumberIndex = j;
				return;
			}
			if (!this.activeNumbers[j].isActive)
			{
				this.inactiveNumbers.Add(this.activeNumbers[j]);
				this.activeNumbers.RemoveAt(j);
				j--;
			}
			else
			{
				this.activeNumbers[j].Move();
			}
		}
		this._lastNumberIndex = 0;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00016B30 File Offset: 0x00014D30
	private void UpdatePlacementAnimations()
	{
		for (int i = 0; i < this._placeAnimations.Count; i++)
		{
			if (this._placeAnimations[i].obj == null)
			{
				this._placeAnimations.RemoveAt(i);
				i--;
			}
			else
			{
				this._placeAnimations[i].time += Time.deltaTime;
				float num = (Mathf.Sin(this._placeAnimations[i].time * 30f + this._placeAnimations[i].offset) + 1f) / 10f + 1f;
				this._placeAnimations[i].obj.localScale = new Vector3(num, num, 1f);
				if (this._placeAnimations[i].time >= this._placeAnimations[i].end)
				{
					this._placeAnimations[i].obj.localScale = this._placeAnimations[i].original;
					this._placeAnimations.RemoveAt(i);
					i--;
				}
			}
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00016C64 File Offset: 0x00014E64
	public void CreateDamageNumber(Vector2 position, float amount)
	{
		if (!this.useDamageNumbers || !Singleton<Settings>.Instance.UseDamageNumbers)
		{
			return;
		}
		if (this.inactiveNumbers.Count > 0)
		{
			this.inactiveNumbers[0].transform.position = position;
			this.inactiveNumbers[0].gameObject.SetActive(true);
			this.inactiveNumbers[0].Set(amount, Color.white);
			this.activeNumbers.Add(this.inactiveNumbers[0]);
			this.inactiveNumbers.RemoveAt(0);
			return;
		}
		DamageNumber damageNumber = Object.Instantiate<DamageNumber>(this.damageNumber, position, Quaternion.identity);
		damageNumber.Set(amount, Color.white);
		this.activeNumbers.Add(damageNumber);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00016D34 File Offset: 0x00014F34
	public void CreateTemporaryText(Vector2 position, string text, Color color)
	{
		if (this.inactiveNumbers.Count > 0)
		{
			this.inactiveNumbers[0].transform.position = position;
			this.inactiveNumbers[0].gameObject.SetActive(true);
			this.inactiveNumbers[0].Set(text, color);
			this.activeNumbers.Add(this.inactiveNumbers[0]);
			this.inactiveNumbers.RemoveAt(0);
			return;
		}
		DamageNumber damageNumber = Object.Instantiate<DamageNumber>(this.damageNumber, position, Quaternion.identity);
		damageNumber.Set(text, color);
		this.activeNumbers.Add(damageNumber);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00016DE4 File Offset: 0x00014FE4
	public void CreateCopySquare(Vector2 position, Vector2 size)
	{
		this.pipetteSquare.transform.position = position;
		this.pipetteSquare.transform.localScale = size;
		this.pipetteSquare.color = Color.white;
		this._copySquare = true;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00016E34 File Offset: 0x00015034
	public void CreatePasteSquare(Vector2 position, Vector2 size)
	{
		this.pipetteSquare.transform.position = position;
		this.pipetteSquare.transform.localScale = size;
		this.pipetteSquare.color = Color.white;
		this._copySquare = false;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00016E84 File Offset: 0x00015084
	public void RegisterPlacementAnimation(Transform building, bool full = false)
	{
		if (Singleton<Gamemode>.Instance.UseEntityAnimations)
		{
			AnimationManager.PlacementAnimation item = new AnimationManager.PlacementAnimation(building, full);
			this._placeAnimations.Add(item);
		}
	}

	// Token: 0x0400022C RID: 556
	public bool useDamageNumbers = true;

	// Token: 0x0400022D RID: 557
	protected const int MAX_NUMBER_UPDATES = 50;

	// Token: 0x0400022E RID: 558
	protected int _numbersTracked;

	// Token: 0x0400022F RID: 559
	protected int _lastNumberIndex;

	// Token: 0x04000230 RID: 560
	protected bool _copySquare;

	// Token: 0x04000231 RID: 561
	public DamageNumber damageNumber;

	// Token: 0x04000232 RID: 562
	protected List<DamageNumber> activeNumbers = new List<DamageNumber>();

	// Token: 0x04000233 RID: 563
	protected List<DamageNumber> inactiveNumbers = new List<DamageNumber>();

	// Token: 0x04000234 RID: 564
	protected List<AnimationManager.PlacementAnimation> _placeAnimations = new List<AnimationManager.PlacementAnimation>();

	// Token: 0x04000235 RID: 565
	protected List<IAnimateable> _animateables = new List<IAnimateable>();

	// Token: 0x04000236 RID: 566
	public float pipetteSizeSpeed;

	// Token: 0x04000237 RID: 567
	public float pipetteAlphaSpeed;

	// Token: 0x04000238 RID: 568
	public SpriteRenderer pipetteSquare;

	// Token: 0x02000054 RID: 84
	public class PlacementAnimation
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x00016EEC File Offset: 0x000150EC
		public PlacementAnimation(Transform obj, bool full)
		{
			this.obj = obj;
			this.offset = (full ? -1.57f : 1.57f);
			this.end = (full ? 0.209f : 0.105f);
			this.original = Vector3.one;
		}

		// Token: 0x04000239 RID: 569
		public Transform obj;

		// Token: 0x0400023A RID: 570
		public float time;

		// Token: 0x0400023B RID: 571
		public float end;

		// Token: 0x0400023C RID: 572
		public float offset;

		// Token: 0x0400023D RID: 573
		public Vector3 original;
	}
}
