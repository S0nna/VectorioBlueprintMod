using System;
using UnityEngine;
using Vectorio.Stats;

// Token: 0x0200010B RID: 267
public class EntityDetector : EntityComponent, IMouseListener
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060008FA RID: 2298 RVA: 0x00026788 File Offset: 0x00024988
	// (remove) Token: 0x060008FB RID: 2299 RVA: 0x000267C0 File Offset: 0x000249C0
	public event EntityDetector.DetectorEvent OnHitDetectorCheckFinished = delegate(Entity <p0>)
	{
	};

	// Token: 0x060008FC RID: 2300 RVA: 0x000267F5 File Offset: 0x000249F5
	public void SetTargetMode(int mode)
	{
		this._hitDetector.SetMode(mode);
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x060008FD RID: 2301 RVA: 0x00026803 File Offset: 0x00024A03
	public int GetTargetMode
	{
		get
		{
			return (int)this._hitDetector.GetMode;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x00026810 File Offset: 0x00024A10
	public bool WaitingForQuery
	{
		get
		{
			return this._hitDetector.WaitingForQuery;
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0002681D File Offset: 0x00024A1D
	public void SyncPosition()
	{
		this._hitDetector.transform.position = base.transform.position;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0002683C File Offset: 0x00024A3C
	public void Setup(ref StatInt range, string layer, bool staticDetector)
	{
		if (this._hitDetector == null)
		{
			this._hitDetector = new GameObject().AddComponent<HitDetector>();
			this._hitDetector.transform.position = base.transform.position;
			this._hitDetector.transform.name = base.transform.name + " detector";
		}
		else
		{
			this._hitDetector.transform.SetParent(null);
			this.SyncPosition();
		}
		this._hitDetector.Setup(this, ref range, layer, staticDetector);
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x000268D0 File Offset: 0x00024AD0
	public void Check()
	{
		this._hitDetector.SoftCheck();
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x00003212 File Offset: 0x00001412
	public void OnQuickEdit()
	{
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00003212 File Offset: 0x00001412
	public void OnMouseClick()
	{
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000268DD File Offset: 0x00024ADD
	public void OnMouseHover(bool toggle)
	{
		this._hitDetector.ToggleVisibility(toggle);
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x000268EB File Offset: 0x00024AEB
	public void EntityDetected(Entity entity)
	{
		EntityDetector.DetectorEvent onHitDetectorCheckFinished = this.OnHitDetectorCheckFinished;
		if (onHitDetectorCheckFinished == null)
		{
			return;
		}
		onHitDetectorCheckFinished(entity);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x000268FE File Offset: 0x00024AFE
	public override void OnReset()
	{
		this._hitDetector.ToggleCollisions(false);
		this._hitDetector.transform.SetParent(base.transform);
	}

	// Token: 0x0400059D RID: 1437
	private HitDetector _hitDetector;

	// Token: 0x0200010C RID: 268
	// (Invoke) Token: 0x06000909 RID: 2313
	public delegate void DetectorEvent(Entity entity);
}
