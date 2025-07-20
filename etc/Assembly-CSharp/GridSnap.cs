using System;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class GridSnap : MonoBehaviour
{
	// Token: 0x06000C8C RID: 3212 RVA: 0x0003647F File Offset: 0x0003467F
	private void Start()
	{
		this.previousTargetPosition = this.target.transform.position;
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00036498 File Offset: 0x00034698
	private void Update()
	{
		if (this.target.position != this.previousTargetPosition)
		{
			this.previousTargetPosition = this.target.position;
			Vector2 v = new Vector2(Mathf.Round(this.target.position.x / this.snap) * this.snap, Mathf.Round(this.target.position.y / this.snap) * this.snap);
			base.transform.position = v;
		}
	}

	// Token: 0x0400086E RID: 2158
	public Transform target;

	// Token: 0x0400086F RID: 2159
	public float snap;

	// Token: 0x04000870 RID: 2160
	private Vector3 previousTargetPosition;
}
