using System;
using TMPro;
using UnityEngine;
using Vectorio.Formatting;

// Token: 0x0200020C RID: 524
public class DamageNumber : MonoBehaviour
{
	// Token: 0x06000FA7 RID: 4007 RVA: 0x00049E6D File Offset: 0x0004806D
	public void Set(float dmg, Color color)
	{
		this.Set(Formatter.Round(dmg, 1), color);
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x00049E80 File Offset: 0x00048080
	public void Set(string text, Color color)
	{
		this.lastUpdate = DateTime.UtcNow;
		this.time = 1f;
		this.isActive = true;
		this.amount.text = text;
		this.amount.color = color;
		base.transform.localScale = new Vector2(1f, 1f);
		this.increaseScale = new Vector3(2f, 2f, 2f);
		this.decreaseScale = new Vector3(-4f, -4f, -4f);
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x00049F18 File Offset: 0x00048118
	public void SetText(string name, Color color)
	{
		this.time = 1f;
		this.isActive = true;
		this.amount.text = name;
		this.amount.color = color;
		base.transform.localScale = new Vector2(1f, 1f);
		this.increaseScale = new Vector3(2f, 2f, 2f);
		this.decreaseScale = new Vector3(-4f, -4f, -4f);
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x00049FA4 File Offset: 0x000481A4
	public void Move()
	{
		float customTime = this.GetCustomTime();
		base.transform.position += base.transform.up * this.speed * customTime;
		this.time -= customTime;
		if (this.time > 0.5f)
		{
			base.transform.localScale += this.increaseScale * customTime;
			return;
		}
		if (this.time > 0f)
		{
			base.transform.localScale += this.decreaseScale * customTime;
			return;
		}
		this.Disable();
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x0004A05E File Offset: 0x0004825E
	public void Disable()
	{
		base.gameObject.SetActive(false);
		this.isActive = false;
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0004A074 File Offset: 0x00048274
	protected float GetCustomTime()
	{
		TimeSpan timeSpan = DateTime.UtcNow - this.lastUpdate;
		this.lastUpdate = DateTime.UtcNow;
		return (float)timeSpan.TotalSeconds;
	}

	// Token: 0x04000D66 RID: 3430
	public TextMeshPro amount;

	// Token: 0x04000D67 RID: 3431
	[HideInInspector]
	public float time;

	// Token: 0x04000D68 RID: 3432
	public float speed;

	// Token: 0x04000D69 RID: 3433
	[HideInInspector]
	public bool isActive;

	// Token: 0x04000D6A RID: 3434
	private Vector3 increaseScale;

	// Token: 0x04000D6B RID: 3435
	private Vector3 decreaseScale;

	// Token: 0x04000D6C RID: 3436
	private DateTime lastUpdate;
}
