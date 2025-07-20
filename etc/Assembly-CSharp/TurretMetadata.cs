using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
[MetadataFor(typeof(Turret))]
[Serializable]
public class TurretMetadata : ComponentMetadata<Turret>
{
	// Token: 0x0600077B RID: 1915 RVA: 0x00021BA9 File Offset: 0x0001FDA9
	public override void GetValues(Turret turret, MetadataContext context)
	{
		this.BarrelRotation = turret.GetBarrelRotation();
		this.Cooldown = turret.Timer;
		this.TargetMode = turret.GetTargetMode();
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00021BD0 File Offset: 0x0001FDD0
	public override void SetValues(Turret component, bool asPipette, MetadataContext context)
	{
		component.SetTargetMode(this.TargetMode);
		if (this.BarrelRotation != 0f)
		{
			component.RotatorPivot.transform.eulerAngles = new Vector3(component.RotatorPivot.transform.eulerAngles.x, component.RotatorPivot.transform.eulerAngles.y, this.BarrelRotation);
		}
		if (this.Cooldown != 0f)
		{
			component.Timer = this.Cooldown;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600077D RID: 1917 RVA: 0x00021C54 File Offset: 0x0001FE54
	// (set) Token: 0x0600077E RID: 1918 RVA: 0x00021C5C File Offset: 0x0001FE5C
	public float BarrelRotation { get; set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x00021C65 File Offset: 0x0001FE65
	// (set) Token: 0x06000780 RID: 1920 RVA: 0x00021C6D File Offset: 0x0001FE6D
	public float Cooldown { get; set; }

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000781 RID: 1921 RVA: 0x00021C76 File Offset: 0x0001FE76
	// (set) Token: 0x06000782 RID: 1922 RVA: 0x00021C7E File Offset: 0x0001FE7E
	public int TargetMode { get; set; }
}
