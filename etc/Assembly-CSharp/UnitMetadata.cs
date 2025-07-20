using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
[MetadataFor(typeof(Unit))]
[Serializable]
public class UnitMetadata : ComponentMetadata<Unit>
{
	// Token: 0x06000784 RID: 1924 RVA: 0x00021C90 File Offset: 0x0001FE90
	public override void GetValues(Unit unit, MetadataContext context)
	{
		if (unit.Target != null)
		{
			this.TargetID = new uint?(unit.Target.RuntimeID);
		}
		else
		{
			this.TargetID = null;
		}
		this.Rotation = unit.transform.eulerAngles.z;
		if (unit.Rigidbody2D != null)
		{
			Vector2 velocity = unit.Rigidbody2D.velocity;
			this.VelocityX = velocity.x;
			this.VelocityY = velocity.y;
			this.HasVelocity = true;
		}
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00021D24 File Offset: 0x0001FF24
	public override void SetValues(Unit component, bool asPipette, MetadataContext context)
	{
		if (asPipette)
		{
			return;
		}
		Entity target;
		if (this.TargetID != null && Singleton<EntityManager>.Instance.TryGetEntity(this.TargetID.Value, out target))
		{
			component.OnHitDetectorCheckFinished(target);
		}
		else
		{
			component.SetTargetPosition(Singleton<WorldGenerator>.Instance.CenterWorldPos);
		}
		component.transform.rotation = Quaternion.Euler(0f, 0f, this.Rotation);
		if (this.HasVelocity && component.Rigidbody2D != null)
		{
			Vector2 velocity = new Vector2(this.VelocityX, this.VelocityY);
			component.Rigidbody2D.velocity = velocity;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000786 RID: 1926 RVA: 0x00021DCF File Offset: 0x0001FFCF
	// (set) Token: 0x06000787 RID: 1927 RVA: 0x00021DD7 File Offset: 0x0001FFD7
	public uint? TargetID { get; set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000788 RID: 1928 RVA: 0x00021DE0 File Offset: 0x0001FFE0
	// (set) Token: 0x06000789 RID: 1929 RVA: 0x00021DE8 File Offset: 0x0001FFE8
	public float Rotation { get; set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600078A RID: 1930 RVA: 0x00021DF1 File Offset: 0x0001FFF1
	// (set) Token: 0x0600078B RID: 1931 RVA: 0x00021DF9 File Offset: 0x0001FFF9
	public bool HasVelocity { get; set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x00021E02 File Offset: 0x00020002
	// (set) Token: 0x0600078D RID: 1933 RVA: 0x00021E0A File Offset: 0x0002000A
	public float VelocityX { get; set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00021E13 File Offset: 0x00020013
	// (set) Token: 0x0600078F RID: 1935 RVA: 0x00021E1B File Offset: 0x0002001B
	public float VelocityY { get; set; }
}
