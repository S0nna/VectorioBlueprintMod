using System;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x0200009E RID: 158
[Serializable]
public class AccentData
{
	// Token: 0x06000634 RID: 1588 RVA: 0x0001F61C File Offset: 0x0001D81C
	public AccentData(Accent a)
	{
		this.use = (a != null);
		if (this.use)
		{
			try
			{
				if (a.primaryMaterial.HasColor("_EmissionColor"))
				{
					this.pmt = Utilities.ColorToInt(a.primaryMaterial.GetColor("_Color"));
					this.pme = Utilities.ColorToInt(a.primaryMaterial.GetColor("_EmissionColor"));
					this.pm = true;
				}
				if (a.secondaryMaterial.HasColor("_EmissionColor"))
				{
					this.smt = Utilities.ColorToInt(a.secondaryMaterial.GetColor("_Color"));
					this.sme = Utilities.ColorToInt(a.secondaryMaterial.GetColor("_EmissionColor"));
					this.sm = true;
				}
				this.pc = Utilities.ColorToInt(a.primaryColor);
				this.sc = Utilities.ColorToInt(a.secondaryColor);
			}
			catch
			{
				Debug.Log("[ACCENT DATA] Ran into error while trying to parse material properties");
				this.use = false;
			}
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001F72C File Offset: 0x0001D92C
	public AccentData()
	{
		this.use = false;
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000636 RID: 1590 RVA: 0x0001F73B File Offset: 0x0001D93B
	// (set) Token: 0x06000637 RID: 1591 RVA: 0x0001F743 File Offset: 0x0001D943
	public bool use { get; set; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001F74C File Offset: 0x0001D94C
	// (set) Token: 0x06000639 RID: 1593 RVA: 0x0001F754 File Offset: 0x0001D954
	public bool pm { get; set; }

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001F75D File Offset: 0x0001D95D
	// (set) Token: 0x0600063B RID: 1595 RVA: 0x0001F765 File Offset: 0x0001D965
	public bool sm { get; set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600063C RID: 1596 RVA: 0x0001F76E File Offset: 0x0001D96E
	// (set) Token: 0x0600063D RID: 1597 RVA: 0x0001F776 File Offset: 0x0001D976
	public int pmt { get; set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001F77F File Offset: 0x0001D97F
	// (set) Token: 0x0600063F RID: 1599 RVA: 0x0001F787 File Offset: 0x0001D987
	public int pme { get; set; }

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001F790 File Offset: 0x0001D990
	// (set) Token: 0x06000641 RID: 1601 RVA: 0x0001F798 File Offset: 0x0001D998
	public int smt { get; set; }

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000642 RID: 1602 RVA: 0x0001F7A1 File Offset: 0x0001D9A1
	// (set) Token: 0x06000643 RID: 1603 RVA: 0x0001F7A9 File Offset: 0x0001D9A9
	public int sme { get; set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001F7B2 File Offset: 0x0001D9B2
	// (set) Token: 0x06000645 RID: 1605 RVA: 0x0001F7BA File Offset: 0x0001D9BA
	public int pc { get; set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001F7C3 File Offset: 0x0001D9C3
	// (set) Token: 0x06000647 RID: 1607 RVA: 0x0001F7CB File Offset: 0x0001D9CB
	public int sc { get; set; }
}
