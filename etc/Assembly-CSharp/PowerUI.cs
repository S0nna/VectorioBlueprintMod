using System;

// Token: 0x02000158 RID: 344
public class PowerUI : UI_Legacy_ResourceAmount
{
	// Token: 0x06000B39 RID: 2873 RVA: 0x00030B56 File Offset: 0x0002ED56
	public override void UpdateAmount(int newAmount)
	{
		this.amount.text = newAmount.ToString("n2") + " POWER";
		this.progressBar.currentPercent = (float)newAmount;
		this.progressBar.UpdateUI();
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x00030B91 File Offset: 0x0002ED91
	public override void UpdateStorage(int newAmount)
	{
		this.title.text = newAmount.ToString("n2") + " CAPACITY";
		this.progressBar.maxValue = (float)newAmount;
		this.progressBar.UpdateUI();
	}
}
