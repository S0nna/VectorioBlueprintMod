using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000112 RID: 274
public class Gateway : EntityComponent, IComponent<Gateway, GatewayData>, IMouseListener
{
	// Token: 0x06000929 RID: 2345 RVA: 0x00026FB9 File Offset: 0x000251B9
	public GatewayData GetData()
	{
		return this._gatewayData;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00026FC4 File Offset: 0x000251C4
	public void OnInitialize(GatewayData data)
	{
		this._gatewayData = data;
		if (this._particleSystem == null)
		{
			this._particleSystem = Object.Instantiate<ParticleSystem>(this._gatewayData.particle);
			this._particleSystem.transform.SetParent(base.transform);
			this._particleSystem.transform.localPosition = Vector2.zero;
		}
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0002702C File Offset: 0x0002522C
	public override void OnSpawn(bool fromSave)
	{
		Singleton<Events>.Instance.onPowerExceeded.AddListener(new UnityAction(this.OnPowerExceeded));
		Singleton<Events>.Instance.onPowerRecovered.AddListener(new UnityAction(this.OnPowerRecovered));
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00027064 File Offset: 0x00025264
	private void OnPowerExceeded()
	{
		this._particleSystem.Stop();
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00027071 File Offset: 0x00025271
	private void OnPowerRecovered()
	{
		this._particleSystem.Play();
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0002707E File Offset: 0x0002527E
	public void Animate(float time, float speed)
	{
		base.GetModel.transform.Rotate(Vector3.forward, time * speed);
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00027098 File Offset: 0x00025298
	public override void OnReset()
	{
		Singleton<Events>.Instance.onPowerExceeded.RemoveListener(new UnityAction(this.OnPowerExceeded));
		Singleton<Events>.Instance.onPowerRecovered.RemoveListener(new UnityAction(this.OnPowerRecovered));
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x000270D0 File Offset: 0x000252D0
	public void OnMouseClick()
	{
		Singleton<Events>.Instance.onGatewayClicked.Invoke(this);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00003212 File Offset: 0x00001412
	public void OnMouseHover(bool toggle)
	{
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x000270D0 File Offset: 0x000252D0
	public void OnQuickEdit()
	{
		Singleton<Events>.Instance.onGatewayClicked.Invoke(this);
	}

	// Token: 0x040005AE RID: 1454
	private GatewayData _gatewayData;

	// Token: 0x040005AF RID: 1455
	private ParticleSystem _particleSystem;
}
