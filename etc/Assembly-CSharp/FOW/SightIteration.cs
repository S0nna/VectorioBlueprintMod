using System;
using Unity.Collections;
using Unity.Mathematics;

namespace FOW
{
	// Token: 0x02000385 RID: 901
	public class SightIteration
	{
		// Token: 0x06001768 RID: 5992 RVA: 0x000708B4 File Offset: 0x0006EAB4
		public void InitializeStruct(int NumSteps)
		{
			this.RayAngles = new NativeArray<float>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Hits = new NativeArray<bool>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Distances = new NativeArray<float>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Points = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Directions = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Normals = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.NextPoints = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00070924 File Offset: 0x0006EB24
		public void DisposeStruct()
		{
			this.RayAngles.Dispose();
			this.Distances.Dispose();
			this.Hits.Dispose();
			this.Points.Dispose();
			this.Directions.Dispose();
			this.Normals.Dispose();
			this.NextPoints.Dispose();
		}

		// Token: 0x040016FF RID: 5887
		public NativeArray<float> RayAngles;

		// Token: 0x04001700 RID: 5888
		public NativeArray<bool> Hits;

		// Token: 0x04001701 RID: 5889
		public NativeArray<float> Distances;

		// Token: 0x04001702 RID: 5890
		public NativeArray<float2> Points;

		// Token: 0x04001703 RID: 5891
		public NativeArray<float2> Directions;

		// Token: 0x04001704 RID: 5892
		public NativeArray<float2> Normals;

		// Token: 0x04001705 RID: 5893
		public NativeArray<float2> NextPoints;

		// Token: 0x04001706 RID: 5894
		public SightIteration[] NextIterations;
	}
}
