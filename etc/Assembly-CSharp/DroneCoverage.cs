using System;
using System.Collections.Generic;

// Token: 0x0200020D RID: 525
public class DroneCoverage
{
	// Token: 0x06000FAE RID: 4014 RVA: 0x0004A0A5 File Offset: 0x000482A5
	public DroneCoverage(Drone drone, CoverageType type)
	{
		this.drone = drone;
		this.type = type;
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0004A0C8 File Offset: 0x000482C8
	public void AddTarget(Entity target)
	{
		if (!this.targets.Contains(target) && this.drone.CheckTarget(target, this.type))
		{
			this.targets.Add(target);
			this.drone.OnAddTarget(target, this.type);
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0004A115 File Offset: 0x00048315
	public void RemoveTarget(Entity target)
	{
		if (this.targets.Contains(target))
		{
			this.targets.Remove(target);
			this.drone.OnRemoveTarget(target, this.type);
		}
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0004A144 File Offset: 0x00048344
	public void SetArea(CoverageArea newArea)
	{
		this.area = newArea;
		Singleton<TileGrid>.Instance.RegisterDroneCoverage(this);
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x0004A159 File Offset: 0x00048359
	public void ClearCoverage()
	{
		Singleton<TileGrid>.Instance.ClearDroneCoverage(this);
		this.targets.Clear();
	}

	// Token: 0x04000D6D RID: 3437
	public Drone drone;

	// Token: 0x04000D6E RID: 3438
	public CoverageType type;

	// Token: 0x04000D6F RID: 3439
	public List<Entity> targets = new List<Entity>();

	// Token: 0x04000D70 RID: 3440
	public CoverageArea area;
}
