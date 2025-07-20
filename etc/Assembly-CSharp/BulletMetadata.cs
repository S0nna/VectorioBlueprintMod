using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
[MetadataFor(typeof(Bullet))]
[Serializable]
public class BulletMetadata : ComponentMetadata<Bullet>
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x0600071E RID: 1822 RVA: 0x00020D13 File Offset: 0x0001EF13
	// (set) Token: 0x0600071F RID: 1823 RVA: 0x00020D1B File Offset: 0x0001EF1B
	public E_ID TurretID { get; set; }

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000720 RID: 1824 RVA: 0x00020D24 File Offset: 0x0001EF24
	// (set) Token: 0x06000721 RID: 1825 RVA: 0x00020D2C File Offset: 0x0001EF2C
	public float Rotation { get; set; }

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000722 RID: 1826 RVA: 0x00020D35 File Offset: 0x0001EF35
	// (set) Token: 0x06000723 RID: 1827 RVA: 0x00020D3D File Offset: 0x0001EF3D
	public float Lifetime { get; set; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000724 RID: 1828 RVA: 0x00020D46 File Offset: 0x0001EF46
	// (set) Token: 0x06000725 RID: 1829 RVA: 0x00020D4E File Offset: 0x0001EF4E
	public int Pierces { get; set; }

	// Token: 0x06000726 RID: 1830 RVA: 0x00020D58 File Offset: 0x0001EF58
	public override void GetValues(Bullet component, MetadataContext context)
	{
		if (component.Turret != null && !component.Turret.Entity.Has_EFlag_IsDead)
		{
			this.TurretID = new E_ID(component.Turret.RuntimeID, context);
			this.Rotation = component.transform.eulerAngles.z;
			this.Lifetime = component.Lifetime;
			this.Pierces = component.Pierces;
		}
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00020DCC File Offset: 0x0001EFCC
	public override void SetValues(Bullet component, bool asPipette, MetadataContext context)
	{
		if (!asPipette)
		{
			Entity entity;
			if (this.TurretID != null && this.TurretID.TryGetEntity(out entity) && entity.Has_EComponent<Turret>())
			{
				Turret turret = entity.Get_EComponent<Turret>(false);
				component.LinkTurretValues(turret, true);
				component.Lifetime = this.Lifetime;
				component.Pierces = this.Pierces;
				component.transform.rotation = Quaternion.Euler(0f, 0f, this.Rotation);
				return;
			}
			Debug.Log("[BULLET] Terminating due to missing metadata.");
			component.Entity.DestroyEntity(true);
		}
	}
}
