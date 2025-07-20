using System;
using UnityEngine.Events;

// Token: 0x0200014C RID: 332
public class HeatUI : UI_Legacy_ResourceAmount
{
	// Token: 0x06000ADE RID: 2782 RVA: 0x0002DE8C File Offset: 0x0002C08C
	public override UI_Legacy_ResourceAmount Setup(ResourceData resource, int amount, int storage)
	{
		Singleton<Events>.Instance.onHeatLimitReached.AddListener(new UnityAction(this.OnMaxHeatReached));
		Singleton<Events>.Instance.onHeatThresholdUpdated.AddListener(new UnityAction<int, int>(this.OnHeatThresholdUpdated));
		return base.Setup(resource, amount, storage);
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0002DED8 File Offset: 0x0002C0D8
	public override void UpdateAmount(int newAmount)
	{
		this.amount.text = newAmount.ToString("n2") + " HEAT";
		this.progressBar.currentPercent = (float)newAmount;
		this.progressBar.UpdateUI();
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00003212 File Offset: 0x00001412
	public override void UpdateStorage(int newAmount)
	{
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0002DF13 File Offset: 0x0002C113
	public void OnMaxHeatReached()
	{
		this.title.text = "MAX HEAT";
		this.progressBar.minValue = 0f;
		this.progressBar.maxValue = 0f;
		this.progressBar.UpdateUI();
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0002DF50 File Offset: 0x0002C150
	public void OnHeatThresholdUpdated(int minHeat, int maxHeat)
	{
		this.title.text = maxHeat.ToString() + " NEW ENEMY";
		this.progressBar.minValue = (float)minHeat;
		this.progressBar.maxValue = (float)maxHeat;
		this.progressBar.UpdateUI();
	}
}
