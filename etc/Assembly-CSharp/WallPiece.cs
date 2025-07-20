using System;
using UnityEngine;

// Token: 0x02000235 RID: 565
[Serializable]
public class WallPiece
{
	// Token: 0x06001070 RID: 4208 RVA: 0x0004CEC8 File Offset: 0x0004B0C8
	public Model RequestWallPiece(WallPiece.Type type)
	{
		switch (type)
		{
		case WallPiece.Type.OneConnection:
			return this.oneConnection;
		case WallPiece.Type.TwoConnectionsCorner:
			return this.twoConnectionsCorner;
		case WallPiece.Type.TwoConnectionsStraight:
			return this.twoConnectionsStraight;
		case WallPiece.Type.ThreeConnections:
			return this.threeConnections;
		case WallPiece.Type.FourConnections:
			return this.fourConnections;
		default:
			return this.noConnections;
		}
	}

	// Token: 0x04000E6F RID: 3695
	[SerializeField]
	public Model noConnections;

	// Token: 0x04000E70 RID: 3696
	[SerializeField]
	public Model oneConnection;

	// Token: 0x04000E71 RID: 3697
	[SerializeField]
	public Model twoConnectionsCorner;

	// Token: 0x04000E72 RID: 3698
	[SerializeField]
	public Model twoConnectionsStraight;

	// Token: 0x04000E73 RID: 3699
	[SerializeField]
	public Model threeConnections;

	// Token: 0x04000E74 RID: 3700
	[SerializeField]
	public Model fourConnections;

	// Token: 0x02000236 RID: 566
	public enum Type
	{
		// Token: 0x04000E76 RID: 3702
		NoConnections,
		// Token: 0x04000E77 RID: 3703
		OneConnection,
		// Token: 0x04000E78 RID: 3704
		TwoConnectionsCorner,
		// Token: 0x04000E79 RID: 3705
		TwoConnectionsStraight,
		// Token: 0x04000E7A RID: 3706
		ThreeConnections,
		// Token: 0x04000E7B RID: 3707
		FourConnections
	}
}
