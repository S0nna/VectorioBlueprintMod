using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200037D RID: 893
	public abstract class FogOfWarRevealer : MonoBehaviour
	{
		// Token: 0x06001739 RID: 5945 RVA: 0x0006EE7F File Offset: 0x0006D07F
		private void OnEnable()
		{
			this.RegisterRevealer();
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0006EE87 File Offset: 0x0006D087
		private void OnDisable()
		{
			this.DeregisterRevealer();
			this.Cleanup();
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0006EE95 File Offset: 0x0006D095
		private void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x0006EEA0 File Offset: 0x0006D0A0
		public void RegisterRevealer()
		{
			this.CachedTransform = base.transform;
			if (this.StartRevealerAsStatic)
			{
				this.SetRevealerAsStatic(true);
			}
			else
			{
				this.SetRevealerAsStatic(false);
			}
			this.NumberOfPoints = 0;
			if (FogOfWarWorld.instance == null)
			{
				if (!FogOfWarWorld.RevealersToRegister.Contains(this))
				{
					FogOfWarWorld.RevealersToRegister.Add(this);
				}
				return;
			}
			if (this.IsRegistered)
			{
				Debug.Log("Tried to double register revealer");
				return;
			}
			this.ViewPoints = new FogOfWarRevealer.SightSegment[FogOfWarWorld.instance.MaxPossibleSegmentsPerRevealer];
			this.EdgeAngles = new float[FogOfWarWorld.instance.MaxPossibleSegmentsPerRevealer];
			this.EdgeNormals = new float2[FogOfWarWorld.instance.MaxPossibleSegmentsPerRevealer];
			this.Angles = new float[this.ViewPoints.Length];
			this.Radii = new float[this.ViewPoints.Length];
			this.AreHits = new bool[this.ViewPoints.Length];
			this.IsRegistered = true;
			this.FogOfWarID = FogOfWarWorld.instance.RegisterRevealer(this);
			this.CircleStruct = default(FogOfWarWorld.RevealerStruct);
			this.LineOfSightPhase1();
			this.LineOfSightPhase2();
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x0006EFBC File Offset: 0x0006D1BC
		public void DeregisterRevealer()
		{
			if (FogOfWarWorld.instance == null)
			{
				if (FogOfWarWorld.RevealersToRegister.Contains(this))
				{
					FogOfWarWorld.RevealersToRegister.Remove(this);
				}
				return;
			}
			if (!this.IsRegistered)
			{
				return;
			}
			foreach (FogOfWarHider fogOfWarHider in this.HidersSeen)
			{
				if (fogOfWarHider != null)
				{
					fogOfWarHider.RemoveObserver(this);
				}
			}
			this.HidersSeen.Clear();
			this.IsRegistered = false;
			FogOfWarWorld.instance.DeRegisterRevealer(this);
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x0006F060 File Offset: 0x0006D260
		public void SetRevealerAsStatic(bool IsStatic)
		{
			if (this.IsRegistered)
			{
				if (this.StaticRevealer && !IsStatic)
				{
					FogOfWarWorld.instance.numDynamicRevealers++;
				}
				else if (!this.StaticRevealer && IsStatic)
				{
					FogOfWarWorld.instance.numDynamicRevealers--;
				}
			}
			this.StaticRevealer = IsStatic;
		}

		// Token: 0x0600173F RID: 5951
		protected abstract void _RevealHiders();

		// Token: 0x06001740 RID: 5952 RVA: 0x0006F0BA File Offset: 0x0006D2BA
		public void RevealHiders()
		{
			this._RevealHiders();
		}

		// Token: 0x06001741 RID: 5953
		protected abstract bool _TestPoint(Vector3 point);

		// Token: 0x06001742 RID: 5954 RVA: 0x0006F0C2 File Offset: 0x0006D2C2
		public bool TestPoint(Vector3 point)
		{
			return this._TestPoint(point);
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x0006F0CC File Offset: 0x0006D2CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void AddViewPoint(bool hit, float distance, float angle, float step, float2 normal, float2 point, float2 dir)
		{
			if (this.NumberOfPoints == this.ViewPoints.Length)
			{
				Debug.LogError("Sight Segment buffer is full! Increase Maximum Segments per Revealer on Fog Of War World!");
				return;
			}
			this.ViewPoints[this.NumberOfPoints].DidHit = hit;
			this.ViewPoints[this.NumberOfPoints].Radius = distance;
			this.ViewPoints[this.NumberOfPoints].Angle = angle;
			this.ViewPoints[this.NumberOfPoints].Point = point;
			this.ViewPoints[this.NumberOfPoints].Direction = dir;
			this.EdgeAngles[this.NumberOfPoints] = -step;
			this.EdgeNormals[this.NumberOfPoints] = normal;
			this.NumberOfPoints++;
		}

		// Token: 0x06001744 RID: 5956
		protected abstract void SetCenterAndHeight();

		// Token: 0x06001745 RID: 5957 RVA: 0x0006F19C File Offset: 0x0006D39C
		private void ApplyData()
		{
			for (int i = 0; i < this.NumberOfPoints; i++)
			{
				this.Angles[i] = this.ViewPoints[i].Angle;
				this.AreHits[i] = this.ViewPoints[i].DidHit;
				if (!this.AreHits[i])
				{
					this.ViewPoints[i].Radius = Mathf.Min(this.ViewPoints[i].Radius, this.ViewRadius);
				}
				this.Radii[i] = this.ViewPoints[i].Radius;
				if (i == this.NumberOfPoints - 1 && this.CircleIsComplete)
				{
					this.Angles[i] += 360f;
				}
			}
			this.SetCenterAndHeight();
			this.CircleStruct.CircleOrigin = this.center;
			this.CircleStruct.NumSegments = this.NumberOfPoints;
			this.CircleStruct.UnobscuredRadius = this.UnobscuredRadius;
			this.CircleStruct.CircleHeight = this.heightPos + this.ShaderEyeOffset;
			this.CircleStruct.CircleRadius = this.ViewRadius;
			this.CircleStruct.CircleFade = this.SoftenDistance;
			this.CircleStruct.VisionHeight = this.VisionHeight;
			this.CircleStruct.HeightFade = this.VisionHeightSoftenDistance;
			this.CircleStruct.Opacity = this.Opacity;
			FogOfWarWorld.instance.UpdateRevealerData(this.FogOfWarID, this.CircleStruct, this.NumberOfPoints, this.Angles, this.Radii, this.AreHits);
		}

		// Token: 0x06001746 RID: 5958
		protected abstract float GetEuler();

		// Token: 0x06001747 RID: 5959
		public abstract Vector3 GetEyePosition();

		// Token: 0x06001748 RID: 5960
		public abstract Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal);

		// Token: 0x06001749 RID: 5961
		protected abstract float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2);

		// Token: 0x0600174A RID: 5962 RVA: 0x0006F33E File Offset: 0x0006D53E
		public float GetRayDistance()
		{
			return this.RayDistance;
		}

		// Token: 0x0600174B RID: 5963
		protected abstract void _InitRevealer(int StepCount);

		// Token: 0x0600174C RID: 5964 RVA: 0x0006F348 File Offset: 0x0006D548
		private void InitRevealer(int StepCount, float AngleStep)
		{
			if (this.FirstIteration != null && this.FirstIteration.Distances.IsCreated)
			{
				this.Cleanup();
			}
			for (int i = 0; i < this.ViewPoints.Length; i++)
			{
				this.ViewPoints[i] = default(FogOfWarRevealer.SightSegment);
			}
			this.FirstIterationStepCount = StepCount;
			this.FirstIteration = new SightIteration();
			this.FirstIteration.InitializeStruct(StepCount);
			this.IterationRayCount = this.NumExtraRaysOnIteration + 2;
			this.PointsJob = new FogOfWarRevealer.CalculateNextPoints
			{
				UpVector = FogOfWarWorld.UpVector,
				AngleStep = AngleStep,
				Distances = this.FirstIteration.Distances,
				Points = this.FirstIteration.Points,
				Directions = this.FirstIteration.Directions,
				Normals = this.FirstIteration.Normals,
				ExpectedNextPoints = this.FirstIteration.NextPoints
			};
			this.FirstIterationConditions = new NativeArray<bool>(this.NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.FirstIterationConditionsJob = new FogOfWarRevealer.ConditionCalculations
			{
				Points = this.FirstIteration.Points,
				NextPoints = this.FirstIteration.NextPoints,
				Normals = this.FirstIteration.Normals,
				Hits = this.FirstIteration.Hits,
				IterateConditions = this.FirstIterationConditions
			};
			this.EdgeJob = default(FogOfWarRevealer.FindEdgeJob);
			this.Initialized = true;
			this._InitRevealer(StepCount);
		}

		// Token: 0x0600174D RID: 5965
		protected abstract void CleanupRevealer();

		// Token: 0x0600174E RID: 5966 RVA: 0x0006F4D8 File Offset: 0x0006D6D8
		private void Cleanup()
		{
			this.Initialized = false;
			if (this.FirstIteration == null || !this.FirstIteration.Distances.IsCreated)
			{
				return;
			}
			this.FirstIteration.DisposeStruct();
			this.FirstIterationConditions.Dispose();
			foreach (SightIteration sightIteration in this.SubIterations)
			{
				sightIteration.DisposeStruct();
			}
			this.SubIterations.Clear();
			this.CleanupRevealer();
		}

		// Token: 0x0600174F RID: 5967
		protected abstract void IterationOne(int NumSteps, float firstAngle, float angleStep);

		// Token: 0x06001750 RID: 5968
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract void RayCast(float angle, ref FogOfWarRevealer.SightRay ray);

		// Token: 0x06001751 RID: 5969 RVA: 0x0006F574 File Offset: 0x0006D774
		public void LineOfSightPhase1()
		{
			this.EdgeDstThreshold = Mathf.Max(0.001f, this.EdgeDstThreshold);
			this.CircleIsComplete = Mathf.Approximately(this.ViewAngle, 360f);
			this.CommandsPerJob = 32;
			this.EyePosition = this.GetEyePosition();
			this.RayDistance = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				this.RayDistance += this.SoftenDistance;
			}
			this.NumberOfPoints = 0;
			this.NumSteps = Mathf.Max(2, Mathf.CeilToInt(this.ViewAngle * this.RaycastResolution));
			this.AngleStep = this.ViewAngle / (float)(this.NumSteps - 1);
			if (this.Initialized && this.FirstIteration != null)
			{
				NativeArray<float> rayAngles = this.FirstIteration.RayAngles;
				if (this.FirstIteration.RayAngles.Length == this.NumSteps)
				{
					goto IL_EC;
				}
			}
			this.InitRevealer(this.NumSteps, this.AngleStep);
			IL_EC:
			float firstAngle = (-this.GetEuler() + 360f + 90f) % 360f - this.ViewAngle / 2f;
			this.IterationOne(this.NumSteps, firstAngle, this.AngleStep);
			this.FirstIterationConditionsJob.DoubleHitMaxAngleDelta = this.DoubleHitMaxAngleDelta;
			this.FirstIterationConditionsJob.EdgeDstThreshold = this.EdgeDstThreshold;
			this.FirstIterationConditionsJob.AddCorners = this.AddCorners;
			this.FirstIterationConditionsJobHandle = this.FirstIterationConditionsJob.Schedule(this.NumSteps, this.CommandsPerJob, this.PointsJobHandle);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0006F700 File Offset: 0x0006D900
		public void LineOfSightPhase2()
		{
			this.FirstIterationConditionsJobHandle.Complete();
			this.AddViewPoint(this.FirstIteration.Hits[0], this.FirstIteration.Distances[0], this.FirstIteration.RayAngles[0], 0f, this.FirstIteration.Normals[0], this.FirstIteration.Points[0], this.FirstIteration.Directions[0]);
			this.SortData(ref this.FirstIteration, this.AngleStep, this.NumSteps, 0, true);
			while (this.InUseIterations.Count > 0)
			{
				this.SubIterations.Push(this.InUseIterations.Pop());
			}
			if (this.NumberOfPoints == 1 && !this.ViewPoints[0].DidHit && !this.ViewPoints[1].DidHit)
			{
				this.AddViewPoint(false, this.ViewPoints[0].Radius, this.ViewPoints[0].Angle + this.ViewAngle / 2f, -this.EdgeAngles[0], new float2(0f, 0f), new float2(0f, 0f), new float2(0f, 0f));
			}
			if (this.CircleIsComplete)
			{
				if ((this.FirstIteration.Hits[this.NumSteps - 1] || this.FirstIteration.Hits[0]) && Vector2.Distance(this.FirstIteration.NextPoints[this.NumSteps - 1], this.FirstIteration.Points[0]) > 0.05f)
				{
					this.AddViewPoint(this.FirstIteration.Hits[this.NumSteps - 1], this.FirstIteration.Distances[this.NumSteps - 1], this.FirstIteration.RayAngles[this.NumSteps - 1], 0f, this.FirstIteration.Normals[this.NumSteps - 1], this.FirstIteration.Points[this.NumSteps - 1], this.FirstIteration.Directions[this.NumSteps - 1]);
				}
				this.AddViewPoint(this.FirstIteration.Hits[0], this.FirstIteration.Distances[0], this.FirstIteration.RayAngles[0], 0f, this.FirstIteration.Normals[0], this.FirstIteration.Points[0], this.FirstIteration.Directions[0]);
			}
			else
			{
				int index = this.NumSteps - 1;
				this.AddViewPoint(this.FirstIteration.Hits[index], this.FirstIteration.Distances[index], this.FirstIteration.RayAngles[index], 0f, this.FirstIteration.Normals[index], this.FirstIteration.Points[index], this.FirstIteration.Directions[index]);
			}
			if (this.ResolveEdge)
			{
				this.FindEdges();
			}
			this.ApplyData();
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0006FA80 File Offset: 0x0006DC80
		private void SortData(ref SightIteration iteration, float angleStep, int iterationSteps, int iterationNumber, bool FirstIteration = false)
		{
			float angleStep2 = angleStep / (float)(this.IterationRayCount - 1);
			for (int i = 1; i < iterationSteps; i++)
			{
				bool flag4;
				if (!FirstIteration)
				{
					float num = this.AngleBetweenVector2(iteration.Normals[i], iteration.Normals[i - 1]);
					bool flag = math.abs(num) > this.DoubleHitMaxAngleDelta;
					bool flag2 = !this.Vector2Aprox(iteration.Points[i], iteration.NextPoints[i - 1]);
					bool flag3 = ((iteration.Hits[i - 1] || iteration.Hits[i]) && (flag2 || flag)) || iteration.Hits[i - 1] != iteration.Hits[i];
					if (!this.AddCorners && flag && num > 0f && !flag2)
					{
						flag3 = false;
					}
					flag4 = flag3;
				}
				else
				{
					flag4 = this.FirstIterationConditions[i];
				}
				if (flag4)
				{
					if (iterationNumber == this.NumExtraIterations)
					{
						this.AddViewPoint(iteration.Hits[i - 1], iteration.Distances[i - 1], iteration.RayAngles[i - 1], -angleStep, iteration.Normals[i - 1], iteration.Points[i - 1], iteration.Directions[i - 1]);
						this.AddViewPoint(iteration.Hits[i], iteration.Distances[i], iteration.RayAngles[i], angleStep, iteration.Normals[i], iteration.Points[i], iteration.Directions[i]);
					}
					else
					{
						float initialAngle = iteration.RayAngles[i - 1];
						SightIteration sightIteration = this.Iterate(iterationNumber + 1, initialAngle, angleStep2, ref iteration, i - 1);
						this.SortData(ref sightIteration, angleStep2, this.IterationRayCount, iterationNumber + 1, false);
					}
				}
			}
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0006FC8C File Offset: 0x0006DE8C
		private SightIteration GetSubIteration()
		{
			if (this.SubIterations.Count > 0)
			{
				return this.SubIterations.Pop();
			}
			SightIteration sightIteration = new SightIteration();
			sightIteration.InitializeStruct(this.IterationRayCount);
			return sightIteration;
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0006FCBC File Offset: 0x0006DEBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private SightIteration Iterate(int iterNumber, float initialAngle, float AngleStep, ref SightIteration PreviousIteration, int PrevIterStartIndex)
		{
			SightIteration subIteration = this.GetSubIteration();
			this.InUseIterations.Push(subIteration);
			subIteration.RayAngles[0] = PreviousIteration.RayAngles[PrevIterStartIndex];
			subIteration.Hits[0] = PreviousIteration.Hits[PrevIterStartIndex];
			subIteration.Distances[0] = PreviousIteration.Distances[PrevIterStartIndex];
			subIteration.Points[0] = PreviousIteration.Points[PrevIterStartIndex];
			subIteration.Directions[0] = PreviousIteration.Directions[PrevIterStartIndex];
			subIteration.Normals[0] = PreviousIteration.Normals[PrevIterStartIndex];
			float2 @float = new float2(-subIteration.Normals[0].y, subIteration.Normals[0].x);
			float x = 180f - (this.AngleBetweenVector2(@float, -subIteration.Directions[0]) + AngleStep);
			float rhs = subIteration.Distances[0] * math.sin(math.radians(AngleStep)) / Mathf.Sin(math.radians(x));
			subIteration.NextPoints[0] = subIteration.Points[0] + @float * rhs;
			for (int i = 1; i < this.IterationRayCount - 1; i++)
			{
				this.RayCast(initialAngle + AngleStep * (float)i, ref this.currentRay);
				subIteration.RayAngles[i] = this.currentRay.angle;
				subIteration.Hits[i] = this.currentRay.hit;
				subIteration.Distances[i] = this.currentRay.distance;
				subIteration.Points[i] = this.currentRay.point;
				subIteration.Directions[i] = this.currentRay.direction;
				subIteration.Normals[i] = this.currentRay.normal;
				@float = new float2(-subIteration.Normals[i].y, subIteration.Normals[i].x);
				x = 180f - (this.AngleBetweenVector2(@float, -subIteration.Directions[i]) + AngleStep);
				rhs = subIteration.Distances[i] * math.sin(math.radians(AngleStep)) / Mathf.Sin(math.radians(x));
				subIteration.NextPoints[i] = subIteration.Points[i] + @float * rhs;
			}
			subIteration.RayAngles[this.IterationRayCount - 1] = PreviousIteration.RayAngles[PrevIterStartIndex + 1];
			subIteration.Hits[this.IterationRayCount - 1] = PreviousIteration.Hits[PrevIterStartIndex + 1];
			subIteration.Distances[this.IterationRayCount - 1] = PreviousIteration.Distances[PrevIterStartIndex + 1];
			subIteration.Points[this.IterationRayCount - 1] = PreviousIteration.Points[PrevIterStartIndex + 1];
			subIteration.Directions[this.IterationRayCount - 1] = PreviousIteration.Directions[PrevIterStartIndex + 1];
			subIteration.Normals[this.IterationRayCount - 1] = PreviousIteration.Normals[PrevIterStartIndex + 1];
			subIteration.NextPoints[this.IterationRayCount - 1] = PreviousIteration.NextPoints[PrevIterStartIndex + 1];
			return subIteration;
		}

		// Token: 0x06001756 RID: 5974
		protected abstract void _FindEdge();

		// Token: 0x06001757 RID: 5975 RVA: 0x00070069 File Offset: 0x0006E269
		private void FindEdgesJobs()
		{
			this._FindEdge();
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x00070074 File Offset: 0x0006E274
		private void FindEdges()
		{
			for (int i = 0; i < this.NumberOfPoints; i++)
			{
				float num = this.ViewPoints[i].Angle;
				float num2 = this.EdgeAngles[i];
				num2 /= 2f;
				num += num2;
				for (int j = 0; j < this.MaxEdgeResolveIterations; j++)
				{
					this.RayCast(num, ref this.currentRay);
					this.RotatedNormal.x = -this.EdgeNormals[i].y;
					this.RotatedNormal.y = this.EdgeNormals[i].x;
					float num3 = num - this.ViewPoints[i].Angle;
					float x = 180f - (this.AngleBetweenVector2(this.RotatedNormal, -this.ViewPoints[i].Direction) + num3);
					float rhs = this.ViewPoints[i].Radius * math.sin(math.radians(num3)) / math.sin(math.radians(x));
					float2 v = this.ViewPoints[i].Point + this.RotatedNormal * rhs;
					float num4;
					if (this.ViewPoints[i].DidHit != this.currentRay.hit || Vector2.Angle(this.EdgeNormals[i], this.currentRay.normal) > this.DoubleHitMaxAngleDelta || !this.Vector2Aprox(v, this.currentRay.point))
					{
						num4 = -1f;
					}
					else
					{
						num4 = 1f;
						this.ViewPoints[i].Angle = num;
						this.ViewPoints[i].Radius = this.currentRay.distance;
						this.EdgeNormals[i] = this.currentRay.normal;
						this.ViewPoints[i].Point = this.currentRay.point;
					}
					num2 /= 2f;
					if (math.abs(num2) < this.MaxAcceptableEdgeAngleDifference)
					{
						break;
					}
					num += num2 * num4;
				}
			}
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000702A8 File Offset: 0x0006E4A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float AngleBetweenVector2(float2 vec1, float2 vec2)
		{
			this.vec1Rotated90.x = -vec1.y;
			this.vec1Rotated90.y = vec1.x;
			float num = (math.dot(this.vec1Rotated90, vec2) < 0f) ? -1f : 1f;
			return Vector2.Angle(vec1, vec2) * num;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x0007030B File Offset: 0x0006E50B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool Vector2Aprox(float2 v1, float2 v2)
		{
			return math.distancesq(v1, v2) < this.EdgeDstThreshold;
		}

		// Token: 0x0600175B RID: 5979
		protected abstract Vector3 _Get3Dfrom2D(Vector2 twoD);

		// Token: 0x0600175C RID: 5980 RVA: 0x0007031C File Offset: 0x0006E51C
		private Vector3 Get3Dfrom2D(Vector2 twoD)
		{
			return this._Get3Dfrom2D(twoD);
		}

		// Token: 0x04001698 RID: 5784
		[Header("Customization Variables")]
		[SerializeField]
		public float ViewRadius = 15f;

		// Token: 0x04001699 RID: 5785
		[SerializeField]
		public float SoftenDistance = 3f;

		// Token: 0x0400169A RID: 5786
		[Range(1f, 360f)]
		[SerializeField]
		public float ViewAngle = 360f;

		// Token: 0x0400169B RID: 5787
		[SerializeField]
		public float UnobscuredRadius = 1f;

		// Token: 0x0400169C RID: 5788
		[Range(0f, 1f)]
		[SerializeField]
		public float Opacity = 1f;

		// Token: 0x0400169D RID: 5789
		[SerializeField]
		protected bool AddCorners = true;

		// Token: 0x0400169E RID: 5790
		[Range(0f, 1f)]
		[SerializeField]
		public float RevealHiderInFadeOutZonePercentage = 0.5f;

		// Token: 0x0400169F RID: 5791
		[Tooltip("how high above this object should the sight be calculated from")]
		[SerializeField]
		public float EyeOffset;

		// Token: 0x040016A0 RID: 5792
		[Tooltip("An offset used only in the shader, to determine how high above the revealer vision height should be calculated at")]
		[SerializeField]
		public float ShaderEyeOffset;

		// Token: 0x040016A1 RID: 5793
		[SerializeField]
		public float VisionHeight = 3f;

		// Token: 0x040016A2 RID: 5794
		[SerializeField]
		public float VisionHeightSoftenDistance = 1.5f;

		// Token: 0x040016A3 RID: 5795
		[SerializeField]
		public bool SampleHidersAtRevealerHeight = true;

		// Token: 0x040016A4 RID: 5796
		[SerializeField]
		protected LayerMask ObstacleMask;

		// Token: 0x040016A5 RID: 5797
		[Header("Quality Variables")]
		[SerializeField]
		public float RaycastResolution = 0.5f;

		// Token: 0x040016A6 RID: 5798
		public bool ResolveEdge = true;

		// Token: 0x040016A7 RID: 5799
		[Range(1f, 30f)]
		[Tooltip("Higher values will lead to more accurate edge detection, especially at higher distances. however, this will also result in more raycasts.")]
		[SerializeField]
		protected int MaxEdgeResolveIterations = 10;

		// Token: 0x040016A8 RID: 5800
		[Range(0f, 10f)]
		[SerializeField]
		protected int NumExtraIterations = 4;

		// Token: 0x040016A9 RID: 5801
		[Range(1f, 5f)]
		[SerializeField]
		protected int NumExtraRaysOnIteration = 3;

		// Token: 0x040016AA RID: 5802
		protected int IterationRayCount;

		// Token: 0x040016AB RID: 5803
		[SerializeField]
		protected int MaxHidersSampledPerFrame = 50;

		// Token: 0x040016AC RID: 5804
		[Header("Technical Variables")]
		[Range(0.001f, 1f)]
		[Tooltip("Lower values will lead to more accurate edge detection, especially at higher distances. however, this will also result in more raycasts.")]
		[SerializeField]
		protected float MaxAcceptableEdgeAngleDifference = 0.005f;

		// Token: 0x040016AD RID: 5805
		[Range(0.001f, 1f)]
		[SerializeField]
		protected float EdgeDstThreshold = 0.1f;

		// Token: 0x040016AE RID: 5806
		[SerializeField]
		protected float DoubleHitMaxAngleDelta = 15f;

		// Token: 0x040016AF RID: 5807
		[Tooltip("Static revealers are revealers that need the sight function to be called manually, similar to the 'Called Elsewhere' option on FOW world. To change this at runtime, use the SetRevealerAsStatic(bool IsStatic) Method.")]
		[SerializeField]
		public bool StartRevealerAsStatic;

		// Token: 0x040016B0 RID: 5808
		[HideInInspector]
		public bool StaticRevealer;

		// Token: 0x040016B1 RID: 5809
		[HideInInspector]
		public int FogOfWarID;

		// Token: 0x040016B2 RID: 5810
		[HideInInspector]
		public int IndexID;

		// Token: 0x040016B3 RID: 5811
		protected FogOfWarWorld.RevealerStruct CircleStruct;

		// Token: 0x040016B4 RID: 5812
		protected bool IsRegistered;

		// Token: 0x040016B5 RID: 5813
		public FogOfWarRevealer.SightSegment[] ViewPoints;

		// Token: 0x040016B6 RID: 5814
		protected float[] EdgeAngles;

		// Token: 0x040016B7 RID: 5815
		protected float2[] EdgeNormals;

		// Token: 0x040016B8 RID: 5816
		[HideInInspector]
		public int NumberOfPoints;

		// Token: 0x040016B9 RID: 5817
		[HideInInspector]
		public float[] Angles;

		// Token: 0x040016BA RID: 5818
		[HideInInspector]
		public float[] Radii;

		// Token: 0x040016BB RID: 5819
		[HideInInspector]
		public bool[] AreHits;

		// Token: 0x040016BC RID: 5820
		[Header("debug, you shouldnt have to mess with this")]
		public HashSet<FogOfWarHider> HidersSeen = new HashSet<FogOfWarHider>();

		// Token: 0x040016BD RID: 5821
		protected Transform CachedTransform;

		// Token: 0x040016BE RID: 5822
		protected int _lastHiderIndex;

		// Token: 0x040016BF RID: 5823
		protected float heightPos;

		// Token: 0x040016C0 RID: 5824
		protected Vector2 center;

		// Token: 0x040016C1 RID: 5825
		protected bool CircleIsComplete;

		// Token: 0x040016C2 RID: 5826
		protected bool Initialized;

		// Token: 0x040016C3 RID: 5827
		protected Vector3 EyePosition;

		// Token: 0x040016C4 RID: 5828
		protected int FirstIterationStepCount;

		// Token: 0x040016C5 RID: 5829
		protected SightIteration FirstIteration;

		// Token: 0x040016C6 RID: 5830
		protected int CommandsPerJob;

		// Token: 0x040016C7 RID: 5831
		protected FogOfWarRevealer.CalculateNextPoints PointsJob;

		// Token: 0x040016C8 RID: 5832
		protected JobHandle PointsJobHandle;

		// Token: 0x040016C9 RID: 5833
		public NativeArray<bool> FirstIterationConditions;

		// Token: 0x040016CA RID: 5834
		public FogOfWarRevealer.ConditionCalculations FirstIterationConditionsJob;

		// Token: 0x040016CB RID: 5835
		public JobHandle FirstIterationConditionsJobHandle;

		// Token: 0x040016CC RID: 5836
		protected float RayDistance;

		// Token: 0x040016CD RID: 5837
		protected FogOfWarRevealer.SightRay currentRay;

		// Token: 0x040016CE RID: 5838
		private int NumSteps;

		// Token: 0x040016CF RID: 5839
		private float AngleStep;

		// Token: 0x040016D0 RID: 5840
		private float2 RotatedNormal;

		// Token: 0x040016D1 RID: 5841
		private Stack<SightIteration> SubIterations = new Stack<SightIteration>();

		// Token: 0x040016D2 RID: 5842
		private Stack<SightIteration> InUseIterations = new Stack<SightIteration>();

		// Token: 0x040016D3 RID: 5843
		private bool ProfileExtraIterations;

		// Token: 0x040016D4 RID: 5844
		protected FogOfWarRevealer.FindEdgeJob EdgeJob;

		// Token: 0x040016D5 RID: 5845
		protected JobHandle EdgeJobHandle;

		// Token: 0x040016D6 RID: 5846
		private float2 vec1Rotated90;

		// Token: 0x0200037E RID: 894
		public enum RevealerMode
		{
			// Token: 0x040016D8 RID: 5848
			ConstantDensity,
			// Token: 0x040016D9 RID: 5849
			EdgeDetect
		}

		// Token: 0x0200037F RID: 895
		public struct SightRay
		{
			// Token: 0x0600175E RID: 5982 RVA: 0x00070413 File Offset: 0x0006E613
			public void SetData(bool _hit, Vector2 _point, float _distance, Vector2 _normal, Vector2 _direction)
			{
				this.hit = _hit;
				this.point = _point;
				this.distance = _distance;
				this.normal = _normal;
				this.direction = _direction;
			}

			// Token: 0x040016DA RID: 5850
			public bool hit;

			// Token: 0x040016DB RID: 5851
			public float2 point;

			// Token: 0x040016DC RID: 5852
			public float distance;

			// Token: 0x040016DD RID: 5853
			public float angle;

			// Token: 0x040016DE RID: 5854
			public float2 normal;

			// Token: 0x040016DF RID: 5855
			public float2 direction;
		}

		// Token: 0x02000380 RID: 896
		public struct SightSegment
		{
			// Token: 0x0600175F RID: 5983 RVA: 0x00070449 File Offset: 0x0006E649
			public SightSegment(float rad, float ang, bool hit, float2 point, float2 dir)
			{
				this.Radius = rad;
				this.Angle = ang;
				this.DidHit = hit;
				this.Point = point;
				this.Direction = dir;
			}

			// Token: 0x040016E0 RID: 5856
			public float Radius;

			// Token: 0x040016E1 RID: 5857
			public float Angle;

			// Token: 0x040016E2 RID: 5858
			public bool DidHit;

			// Token: 0x040016E3 RID: 5859
			public float2 Point;

			// Token: 0x040016E4 RID: 5860
			public float2 Direction;
		}

		// Token: 0x02000381 RID: 897
		[BurstCompile]
		public struct ConditionCalculations : IJobParallelFor
		{
			// Token: 0x06001760 RID: 5984 RVA: 0x00070470 File Offset: 0x0006E670
			public void Execute(int id)
			{
				if (id == 0)
				{
					return;
				}
				float num = this.AngleBetweenVector2(this.Normals[id], this.Normals[id - 1]);
				bool flag = math.abs(num) > this.DoubleHitMaxAngleDelta;
				bool flag2 = !this.Vector2Aprox(this.Points[id], this.NextPoints[id - 1]);
				bool value = ((this.Hits[id - 1] || this.Hits[id]) && (flag2 || flag)) || this.Hits[id - 1] != this.Hits[id];
				if (!this.AddCorners && flag && num > 0f && !flag2)
				{
					value = false;
				}
				this.IterateConditions[id] = value;
			}

			// Token: 0x06001761 RID: 5985 RVA: 0x00070544 File Offset: 0x0006E744
			private float AngleBetweenVector2(float2 vec1, float2 vec2)
			{
				float num = (math.dot(new float2
				{
					x = -vec1.y,
					y = vec1.x
				}, vec2) < 0f) ? -1f : 1f;
				return Vector2.Angle(vec1, vec2) * num;
			}

			// Token: 0x06001762 RID: 5986 RVA: 0x000705A2 File Offset: 0x0006E7A2
			private bool Vector2Aprox(float2 v1, float2 v2)
			{
				return math.distancesq(v1, v2) < this.EdgeDstThreshold;
			}

			// Token: 0x040016E5 RID: 5861
			public float DoubleHitMaxAngleDelta;

			// Token: 0x040016E6 RID: 5862
			public float EdgeDstThreshold;

			// Token: 0x040016E7 RID: 5863
			public bool AddCorners;

			// Token: 0x040016E8 RID: 5864
			[ReadOnly]
			public NativeArray<float2> Points;

			// Token: 0x040016E9 RID: 5865
			[ReadOnly]
			public NativeArray<float2> NextPoints;

			// Token: 0x040016EA RID: 5866
			[ReadOnly]
			public NativeArray<float2> Normals;

			// Token: 0x040016EB RID: 5867
			[ReadOnly]
			public NativeArray<bool> Hits;

			// Token: 0x040016EC RID: 5868
			[WriteOnly]
			public NativeArray<bool> IterateConditions;
		}

		// Token: 0x02000382 RID: 898
		public struct EdgeResolveData
		{
			// Token: 0x040016ED RID: 5869
			public float CurrentAngle;

			// Token: 0x040016EE RID: 5870
			public float AngleAdd;

			// Token: 0x040016EF RID: 5871
			public float Sign;

			// Token: 0x040016F0 RID: 5872
			public bool Break;
		}

		// Token: 0x02000383 RID: 899
		[BurstCompile]
		protected struct FindEdgeJob : IJobParallelFor
		{
			// Token: 0x06001763 RID: 5987 RVA: 0x000705B4 File Offset: 0x0006E7B4
			public void Execute(int index)
			{
				FogOfWarRevealer.EdgeResolveData edgeResolveData = this.EdgeData[index];
				if (edgeResolveData.Break)
				{
					return;
				}
				FogOfWarRevealer.SightSegment sightSegment = this.SightSegments[index];
				FogOfWarRevealer.SightRay sightRay = this.SightRays[index];
				float2 @float = this.EdgeNormals[index];
				float num = edgeResolveData.CurrentAngle - sightSegment.Angle;
				float2 float2 = new float2(-@float.y, @float.x);
				float x = 180f - (this.AngleBetweenVector2(float2, -sightSegment.Direction) + num);
				float rhs = sightSegment.Radius * math.sin(math.radians(num)) / Mathf.Sin(math.radians(x));
				float2 v = sightSegment.Point + float2 * rhs;
				if (sightSegment.DidHit != sightRay.hit || Vector2.Angle(@float, sightRay.normal) > this.DoubleHitMaxAngleDelta || !this.Vector2Aprox(v, sightRay.point))
				{
					edgeResolveData.Sign = -1f;
				}
				else
				{
					edgeResolveData.Sign = 1f;
					sightSegment.Angle = edgeResolveData.CurrentAngle;
					sightSegment.Radius = sightRay.distance;
					this.EdgeNormals[index] = sightRay.normal;
					sightSegment.Point = sightRay.point;
				}
				this.SightSegments[index] = sightSegment;
				edgeResolveData.AngleAdd /= 2f;
				if (math.abs(edgeResolveData.AngleAdd) < this.MaxAcceptableEdgeAngleDifference)
				{
					edgeResolveData.Break = true;
				}
				edgeResolveData.CurrentAngle += edgeResolveData.AngleAdd * edgeResolveData.Sign;
				this.EdgeData[index] = edgeResolveData;
			}

			// Token: 0x06001764 RID: 5988 RVA: 0x00070764 File Offset: 0x0006E964
			private float AngleBetweenVector2(float2 vec1, float2 vec2)
			{
				float num = (math.dot(new float2(-vec1.y, vec1.x), vec2) < 0f) ? -1f : 1f;
				return Vector2.Angle(vec1, vec2) * num;
			}

			// Token: 0x06001765 RID: 5989 RVA: 0x000707B0 File Offset: 0x0006E9B0
			private bool Vector2Aprox(float2 v1, float2 v2)
			{
				return math.distancesq(v1, v2) < this.EdgeDstThreshold;
			}

			// Token: 0x040016F1 RID: 5873
			public float MaxAcceptableEdgeAngleDifference;

			// Token: 0x040016F2 RID: 5874
			public float DoubleHitMaxAngleDelta;

			// Token: 0x040016F3 RID: 5875
			public float EdgeDstThreshold;

			// Token: 0x040016F4 RID: 5876
			[ReadOnly]
			public NativeArray<FogOfWarRevealer.SightRay> SightRays;

			// Token: 0x040016F5 RID: 5877
			public NativeArray<FogOfWarRevealer.SightSegment> SightSegments;

			// Token: 0x040016F6 RID: 5878
			public NativeArray<float2> EdgeNormals;

			// Token: 0x040016F7 RID: 5879
			public NativeArray<FogOfWarRevealer.EdgeResolveData> EdgeData;
		}

		// Token: 0x02000384 RID: 900
		[BurstCompile]
		public struct CalculateNextPoints : IJobParallelFor
		{
			// Token: 0x06001766 RID: 5990 RVA: 0x000707C4 File Offset: 0x0006E9C4
			public void Execute(int id)
			{
				float2 @float = this.Normals[id];
				float2 float2 = new float2(-@float.y, @float.x);
				float x = 180f - (this.AngleBetweenVector2(float2, -this.Directions[id]) + this.AngleStep);
				float rhs = this.Distances[id] * math.sin(math.radians(this.AngleStep)) / Mathf.Sin(math.radians(x));
				this.ExpectedNextPoints[id] = this.Points[id] + float2 * rhs;
			}

			// Token: 0x06001767 RID: 5991 RVA: 0x00070868 File Offset: 0x0006EA68
			private float AngleBetweenVector2(float2 vec1, float2 vec2)
			{
				float num = (math.dot(new float2(-vec1.y, vec1.x), vec2) < 0f) ? -1f : 1f;
				return Vector2.Angle(vec1, vec2) * num;
			}

			// Token: 0x040016F8 RID: 5880
			public Vector3 UpVector;

			// Token: 0x040016F9 RID: 5881
			public float AngleStep;

			// Token: 0x040016FA RID: 5882
			[ReadOnly]
			public NativeArray<float> Distances;

			// Token: 0x040016FB RID: 5883
			[ReadOnly]
			public NativeArray<float2> Points;

			// Token: 0x040016FC RID: 5884
			[ReadOnly]
			public NativeArray<float2> Normals;

			// Token: 0x040016FD RID: 5885
			[ReadOnly]
			public NativeArray<float2> Directions;

			// Token: 0x040016FE RID: 5886
			[WriteOnly]
			public NativeArray<float2> ExpectedNextPoints;
		}
	}
}
