using System;
using UnityEngine;
using Vectorio.Utilities;

// Token: 0x0200007B RID: 123
[Serializable]
public class Accent
{
	// Token: 0x0600059C RID: 1436 RVA: 0x0001E660 File Offset: 0x0001C860
	public Accent(AccentData accentData)
	{
		this.primaryColor = Utilities.IntToColor(accentData.pc);
		this.secondaryColor = Utilities.IntToColor(accentData.sc);
		if (accentData.pm)
		{
			this.primaryMaterial = new Material(Library.GetMainMaterial());
			this.primaryMaterial.SetColor("_EmissionColor", Utilities.IntToColor(accentData.pme));
			this.primaryMaterial.SetColor("_Color", Utilities.IntToColor(accentData.pmt));
		}
		else
		{
			this.primaryMaterial = new Material(Library.GetDefaultMaterial());
		}
		if (accentData.sm)
		{
			this.secondaryMaterial = new Material(Library.GetMainMaterial());
			this.secondaryMaterial.SetColor("_EmissionColor", Utilities.IntToColor(accentData.sme));
			this.secondaryMaterial.SetColor("_Color", Utilities.IntToColor(accentData.smt));
			return;
		}
		this.secondaryMaterial = new Material(Library.GetDefaultMaterial());
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0001E754 File Offset: 0x0001C954
	public Accent(Color primaryColor, Color secondaryColor, Material primaryMaterial, Material secondaryMaterial)
	{
		this.primaryColor = primaryColor;
		this.secondaryColor = secondaryColor;
		this.primaryMaterial = primaryMaterial;
		this.secondaryMaterial = secondaryMaterial;
	}

	// Token: 0x0400032D RID: 813
	public const string EMISSION_COLOR = "_EmissionColor";

	// Token: 0x0400032E RID: 814
	public const string TINT_COLOR = "_Color";

	// Token: 0x0400032F RID: 815
	public Color primaryColor;

	// Token: 0x04000330 RID: 816
	public Color secondaryColor;

	// Token: 0x04000331 RID: 817
	public Material primaryMaterial;

	// Token: 0x04000332 RID: 818
	public Material secondaryMaterial;
}
