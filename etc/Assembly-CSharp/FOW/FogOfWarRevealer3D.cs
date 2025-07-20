using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000387 RID: 903
	public class FogOfWarRevealer3D : FogOfWarRevealer
	{
		// Token: 0x0600177E RID: 6014 RVA: 0x000712E8 File Offset: 0x0006F4E8
		protected override void _InitRevealer(int StepCount)
		{
			if (this.RaycastCommandsNative.IsCreated)
			{
				this.CleanupRevealer();
			}
			this.RaycastCommandsNative = new NativeArray<RaycastCommand>(StepCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.RaycastHits = new NativeArray<RaycastHit>(StepCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Vector3Directions = new NativeArray<float3>(StepCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.RayQueryParameters = new QueryParameters(this.ObstacleMask, false, QueryTriggerInteraction.UseGlobal, false);
			this.SetupJob = new FogOfWarRevealer3D.Phase1SetupJob
			{
				GamePlane = (int)FogOfWarWorld.instance.gamePlane,
				RayAngles = this.FirstIteration.RayAngles,
				Vector3Directions = this.Vector3Directions,
				RaycastCommandsNative = this.RaycastCommandsNative
			};
			this.DataJob = new FogOfWarRevealer3D.GetVector2Data
			{
				GamePlane = (int)FogOfWarWorld.instance.gamePlane,
				RaycastHits = this.RaycastHits,
				Hits = this.FirstIteration.Hits,
				Distances = this.FirstIteration.Distances,
				InDirections = this.Vector3Directions,
				OutPoints = this.FirstIteration.Points,
				OutDirections = this.FirstIteration.Directions,
				OutNormals = this.FirstIteration.Normals
			};
			for (int i = 0; i < StepCount; i++)
			{
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00071437 File Offset: 0x0006F637
		protected override void CleanupRevealer()
		{
			if (!this.RaycastCommandsNative.IsCreated)
			{
				return;
			}
			this.RaycastCommandsNative.Dispose();
			this.RaycastHits.Dispose();
			this.Vector3Directions.Dispose();
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00071468 File Offset: 0x0006F668
		protected override void IterationOne(int NumSteps, float firstAngle, float angleStep)
		{
			this.SetupJob.FirstAngle = firstAngle;
			this.SetupJob.AngleStep = angleStep;
			this.SetupJob.RayDistance = this.RayDistance;
			this.SetupJob.EyePosition = this.EyePosition;
			this.RayQueryParameters.layerMask = this.ObstacleMask;
			this.SetupJob.Parameters = this.RayQueryParameters;
			this.SetupJobJobHandle = this.SetupJob.Schedule(NumSteps, this.CommandsPerJob, default(JobHandle));
			this.IterationOneJobHandle = RaycastCommand.ScheduleBatch(this.RaycastCommandsNative, this.RaycastHits, this.CommandsPerJob, this.SetupJobJobHandle);
			this.DataJob.RayDistance = this.RayDistance;
			this.DataJob.EyePosition = this.EyePosition;
			this.Vector2NormalJobHandle = this.DataJob.Schedule(NumSteps, this.CommandsPerJob, this.IterationOneJobHandle);
			this.PointsJobHandle = this.PointsJob.Schedule(NumSteps, this.CommandsPerJob, this.Vector2NormalJobHandle);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00071580 File Offset: 0x0006F780
		protected override void _FindEdge()
		{
			NativeArray<RaycastCommand> commands = new NativeArray<RaycastCommand>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<float3> inDirections = new NativeArray<float3>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<FogOfWarRevealer.SightRay> sightRays = new NativeArray<FogOfWarRevealer.SightRay>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<FogOfWarRevealer.SightSegment> sightSegments = new NativeArray<FogOfWarRevealer.SightSegment>(this.ViewPoints, Allocator.TempJob);
			NativeArray<FogOfWarRevealer.EdgeResolveData> edgeData = new NativeArray<FogOfWarRevealer.EdgeResolveData>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			this.EdgeJob.SightRays = sightRays;
			this.EdgeJob.SightSegments = sightSegments;
			this.EdgeJob.EdgeData = edgeData;
			this.EdgeJob.EdgeNormals = new NativeArray<float2>(this.EdgeNormals, Allocator.TempJob);
			this.EdgeJob.MaxAcceptableEdgeAngleDifference = this.MaxAcceptableEdgeAngleDifference;
			this.EdgeJob.DoubleHitMaxAngleDelta = this.DoubleHitMaxAngleDelta;
			this.EdgeJob.EdgeDstThreshold = this.EdgeDstThreshold;
			for (int i = 0; i < this.NumberOfPoints; i++)
			{
				FogOfWarRevealer.EdgeResolveData edgeResolveData = default(FogOfWarRevealer.EdgeResolveData);
				edgeResolveData.CurrentAngle = this.ViewPoints[i].Angle;
				edgeResolveData.AngleAdd = this.EdgeAngles[i];
				edgeResolveData.Sign = 1f;
				edgeResolveData.AngleAdd /= 2f;
				edgeResolveData.CurrentAngle += edgeResolveData.AngleAdd;
				edgeResolveData.Break = false;
				edgeData[i] = edgeResolveData;
			}
			for (int j = 0; j < this.MaxEdgeResolveIterations; j++)
			{
				for (int k = 0; k < this.NumberOfPoints; k++)
				{
					inDirections[k] = this.DirFromAngle(edgeData[k].CurrentAngle, true);
					commands[k] = new RaycastCommand(this.EyePosition, inDirections[k], this.RayQueryParameters, this.RayDistance);
				}
				JobHandle dependsOn = RaycastCommand.ScheduleBatch(commands, this.RaycastHits, this.CommandsPerJob, default(JobHandle));
				JobHandle dependsOn2 = new FogOfWarRevealer3D.SightRayFromRaycastHit
				{
					GamePlane = (int)FogOfWarWorld.instance.gamePlane,
					RayDistance = this.RayDistance,
					EyePosition = this.EyePosition,
					RaycastHits = this.RaycastHits,
					SightRays = sightRays,
					InDirections = inDirections
				}.Schedule(this.NumberOfPoints, this.CommandsPerJob, dependsOn);
				this.EdgeJobHandle = this.EdgeJob.Schedule(this.NumberOfPoints, this.CommandsPerJob, dependsOn2);
				this.EdgeJobHandle.Complete();
			}
			this.ViewPoints = sightSegments.ToArray();
			commands.Dispose();
			inDirections.Dispose();
			sightRays.Dispose();
			sightSegments.Dispose();
			edgeData.Dispose();
			this.EdgeJob.EdgeNormals.Dispose();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00071848 File Offset: 0x0006FA48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void RayCast(float angle, ref FogOfWarRevealer.SightRay ray)
		{
			Vector3 vector = this.DirFromAngle(angle, true);
			ray.angle = angle;
			ray.direction = this.GetVector2D(vector);
			if (Physics.Raycast(this.EyePosition, vector, out this.RayHit, this.RayDistance, this.ObstacleMask))
			{
				ray.hit = true;
				ray.normal = this.GetVector2D(this.RayHit.normal);
				ray.distance = this.RayHit.distance;
				ray.point = this.GetVector2D(this.RayHit.point);
				return;
			}
			ray.hit = false;
			ray.normal = -ray.direction;
			ray.distance = this.RayDistance;
			ray.point = this.GetVector2D(this.CachedTransform.position) + ray.direction * this.RayDistance;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00071930 File Offset: 0x0006FB30
		private float2 GetVector2D(Vector3 vector)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.vec2d.x = vector.x;
				this.vec2d.y = vector.z;
				return this.vec2d;
			case FogOfWarWorld.GamePlane.XY:
				this.vec2d.x = vector.x;
				this.vec2d.y = vector.y;
				return this.vec2d;
			case FogOfWarWorld.GamePlane.ZY:
				this.vec2d.x = vector.z;
				this.vec2d.y = vector.y;
				return this.vec2d;
			default:
				this.vec2d.x = vector.x;
				this.vec2d.y = vector.z;
				return this.vec2d;
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00071A00 File Offset: 0x0006FC00
		protected override float GetEuler()
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return this.CachedTransform.eulerAngles.y;
			case FogOfWarWorld.GamePlane.XY:
			{
				Vector3 up = this.CachedTransform.up;
				up.z = 0f;
				up.Normalize();
				return -Vector3.SignedAngle(up, Vector3.up, FogOfWarWorld.UpVector);
			}
			case FogOfWarWorld.GamePlane.ZY:
			{
				Vector3 up2 = this.CachedTransform.up;
				up2.x = 0f;
				up2.Normalize();
				return -Vector3.SignedAngle(up2, Vector3.up, FogOfWarWorld.UpVector);
			}
			default:
				return this.CachedTransform.eulerAngles.y;
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00071AB0 File Offset: 0x0006FCB0
		public override Vector3 GetEyePosition()
		{
			Vector3 vector = this.CachedTransform.position + FogOfWarWorld.UpVector * this.EyeOffset;
			if (FogOfWarWorld.instance.PixelateFog && FogOfWarWorld.instance.RoundRevealerPosition)
			{
				vector *= FogOfWarWorld.instance.PixelDensity;
				Vector3 b = new Vector3(FogOfWarWorld.instance.PixelGridOffset.x, 0f, FogOfWarWorld.instance.PixelGridOffset.y);
				vector -= b;
				vector = Vector3Int.RoundToInt(vector);
				vector += b;
				vector /= FogOfWarWorld.instance.PixelDensity;
			}
			return vector;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00071B60 File Offset: 0x0006FD60
		protected override void _RevealHiders()
		{
			this.EyePosition = this.GetEyePosition();
			this.ForwardVectorCached = this.GetForward();
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			num = Mathf.Max(num, this.UnobscuredRadius);
			for (int i = 0; i < Mathf.Min(this.MaxHidersSampledPerFrame, FogOfWarWorld.NumHiders); i++)
			{
				this._lastHiderIndex = (this._lastHiderIndex + 1) % FogOfWarWorld.NumHiders;
				FogOfWarHider fogOfWarHider = FogOfWarWorld.HidersList[this._lastHiderIndex];
				float num2 = this.DistBetweenVectors(fogOfWarHider.CachedTransform.position, this.EyePosition) - fogOfWarHider.MaxDistBetweenSamplePoints;
				bool flag = this.CanSeeHider(fogOfWarHider, num, num2);
				if (this.UnobscuredRadius < 0f && num2 + 1.5f * fogOfWarHider.MaxDistBetweenSamplePoints < -this.UnobscuredRadius)
				{
					flag = false;
				}
				if (flag)
				{
					if (!this.HidersSeen.Contains(fogOfWarHider))
					{
						this.HidersSeen.Add(fogOfWarHider);
						fogOfWarHider.AddObserver(this);
					}
				}
				else if (this.HidersSeen.Contains(fogOfWarHider))
				{
					this.HidersSeen.Remove(fogOfWarHider);
					fogOfWarHider.RemoveObserver(this);
				}
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00071C98 File Offset: 0x0006FE98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CanSeeHider(FogOfWarHider hiderInQuestion, float sightDist, float minDistToHider)
		{
			if (minDistToHider > sightDist)
			{
				return false;
			}
			for (int i = 0; i < hiderInQuestion.SamplePoints.Length; i++)
			{
				if (this.CanSeeHiderSamplePoint(hiderInQuestion.SamplePoints[i], sightDist))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00071CD4 File Offset: 0x0006FED4
		private bool CanSeeHiderSamplePoint(Transform samplePoint, float sightDist)
		{
			float maxDistance = this.DistBetweenVectors(samplePoint.position, this.EyePosition);
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.heightDist = Mathf.Abs(this.EyePosition.y - samplePoint.position.y);
				break;
			case FogOfWarWorld.GamePlane.XY:
				this.heightDist = Mathf.Abs(this.EyePosition.z - samplePoint.position.z);
				break;
			case FogOfWarWorld.GamePlane.ZY:
				this.heightDist = Mathf.Abs(this.EyePosition.x - samplePoint.position.x);
				break;
			}
			if (this.heightDist > this.VisionHeight)
			{
				return false;
			}
			if (Mathf.Abs(this.AngleBetweenVector2(samplePoint.position - this.EyePosition, this.ForwardVectorCached)) <= this.ViewAngle / 2f)
			{
				if (this.SampleHidersAtRevealerHeight)
				{
					this.SetHiderPosition(samplePoint.position, this.EyePosition);
				}
				else
				{
					this.hiderPosition = samplePoint.position;
				}
				if (!Physics.Raycast(this.EyePosition, this.hiderPosition - this.EyePosition, maxDistance, this.ObstacleMask))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00071E14 File Offset: 0x00070014
		private void SetHiderPosition(Vector3 point, Vector3 eyePosition)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.hiderPosition.x = point.x;
				this.hiderPosition.y = eyePosition.y;
				this.hiderPosition.z = point.z;
				return;
			case FogOfWarWorld.GamePlane.XY:
				this.hiderPosition.x = point.x;
				this.hiderPosition.y = point.y;
				this.hiderPosition.z = eyePosition.z;
				return;
			case FogOfWarWorld.GamePlane.ZY:
				this.hiderPosition.x = eyePosition.x;
				this.hiderPosition.y = point.y;
				this.hiderPosition.z = point.z;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00071EDC File Offset: 0x000700DC
		protected override bool _TestPoint(Vector3 point)
		{
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			this.EyePosition = this.GetEyePosition();
			this.ForwardVectorCached = this.GetForward();
			float num2 = this.DistBetweenVectors(point, this.EyePosition);
			if (num2 < this.UnobscuredRadius || (num2 < num && Mathf.Abs(this.AngleBetweenVector2(point - this.EyePosition, this.ForwardVectorCached)) < this.ViewAngle / 2f))
			{
				this.SetHiderPosition(point, this.EyePosition);
				if (!Physics.Raycast(this.EyePosition, this.hiderPosition - this.CachedTransform.position, num2, this.ObstacleMask))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x00071FAC File Offset: 0x000701AC
		protected override void SetCenterAndHeight()
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.center.x = this.EyePosition.x;
				this.center.y = this.EyePosition.z;
				this.heightPos = this.EyePosition.y;
				return;
			case FogOfWarWorld.GamePlane.XY:
				this.center.x = this.EyePosition.x;
				this.center.y = this.EyePosition.y;
				this.heightPos = this.EyePosition.z;
				return;
			case FogOfWarWorld.GamePlane.ZY:
				this.center.x = this.EyePosition.z;
				this.center.y = this.EyePosition.y;
				this.heightPos = this.EyePosition.x;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00072090 File Offset: 0x00070290
		protected override float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.vec1.x = _vec1.x;
				this.vec1.y = _vec1.z;
				this.vec2.x = _vec2.x;
				this.vec2.y = _vec2.z;
				break;
			case FogOfWarWorld.GamePlane.XY:
				this.vec1.x = _vec1.x;
				this.vec1.y = _vec1.y;
				this.vec2.x = _vec2.x;
				this.vec2.y = _vec2.y;
				break;
			case FogOfWarWorld.GamePlane.ZY:
				this.vec1.x = _vec1.z;
				this.vec1.y = _vec1.y;
				this.vec2.x = _vec2.z;
				this.vec2.y = _vec2.y;
				break;
			}
			this._vec1Rotated90.x = -this.vec1.y;
			this._vec1Rotated90.y = this.vec1.x;
			float num = (Vector2.Dot(this._vec1Rotated90, this.vec2) < 0f) ? -1f : 1f;
			return Vector2.Angle(this.vec1, this.vec2) * num;
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000721F8 File Offset: 0x000703F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float DistBetweenVectors(Vector3 _vec1, Vector3 _vec2)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.vec1.x = _vec1.x;
				this.vec1.y = _vec1.z;
				this.vec2.x = _vec2.x;
				this.vec2.y = _vec2.z;
				break;
			case FogOfWarWorld.GamePlane.ZY:
				this.vec1.x = _vec1.z;
				this.vec1.y = _vec1.y;
				this.vec2.x = _vec2.z;
				this.vec2.y = _vec2.y;
				break;
			}
			return Vector2.Distance(this.vec1, this.vec2);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000722C4 File Offset: 0x000704C4
		private Vector3 GetForward()
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return this.CachedTransform.forward;
			case FogOfWarWorld.GamePlane.XY:
				return this.CachedTransform.up;
			case FogOfWarWorld.GamePlane.ZY:
				return this.CachedTransform.up;
			default:
				return this.CachedTransform.forward;
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00072320 File Offset: 0x00070520
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				if (!angleIsGlobal)
				{
					angleInDegrees += this.CachedTransform.eulerAngles.y;
				}
				this.direction.x = Mathf.Cos(angleInDegrees * 0.017453292f);
				this.direction.z = Mathf.Sin(angleInDegrees * 0.017453292f);
				return this.direction;
			case FogOfWarWorld.GamePlane.XY:
				if (!angleIsGlobal)
				{
					angleInDegrees += this.CachedTransform.eulerAngles.z;
				}
				this.direction.x = Mathf.Cos(angleInDegrees * 0.017453292f);
				this.direction.y = Mathf.Sin(angleInDegrees * 0.017453292f);
				return this.direction;
			}
			if (!angleIsGlobal)
			{
				angleInDegrees += this.CachedTransform.eulerAngles.x;
			}
			this.direction.z = Mathf.Cos(angleInDegrees * 0.017453292f);
			this.direction.y = Mathf.Sin(angleInDegrees * 0.017453292f);
			return this.direction;
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00072434 File Offset: 0x00070634
		protected override Vector3 _Get3Dfrom2D(Vector2 pos)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return new Vector3(pos.x, this.CachedTransform.position.y, pos.y);
			case FogOfWarWorld.GamePlane.XY:
				return new Vector3(pos.x, pos.y, this.CachedTransform.position.z);
			case FogOfWarWorld.GamePlane.ZY:
				return new Vector3(this.CachedTransform.position.x, pos.y, pos.x);
			default:
				return new Vector3(pos.x, this.CachedTransform.position.y, pos.y);
			}
		}

		// Token: 0x04001712 RID: 5906
		private NativeArray<RaycastCommand> RaycastCommandsNative;

		// Token: 0x04001713 RID: 5907
		private NativeArray<RaycastHit> RaycastHits;

		// Token: 0x04001714 RID: 5908
		private NativeArray<float3> Vector3Directions;

		// Token: 0x04001715 RID: 5909
		private JobHandle IterationOneJobHandle;

		// Token: 0x04001716 RID: 5910
		private FogOfWarRevealer3D.Phase1SetupJob SetupJob;

		// Token: 0x04001717 RID: 5911
		private JobHandle SetupJobJobHandle;

		// Token: 0x04001718 RID: 5912
		private FogOfWarRevealer3D.GetVector2Data DataJob;

		// Token: 0x04001719 RID: 5913
		private JobHandle Vector2NormalJobHandle;

		// Token: 0x0400171A RID: 5914
		public QueryParameters RayQueryParameters;

		// Token: 0x0400171B RID: 5915
		private RaycastHit RayHit;

		// Token: 0x0400171C RID: 5916
		private float2 vec2d;

		// Token: 0x0400171D RID: 5917
		private Vector3 hiderPosition;

		// Token: 0x0400171E RID: 5918
		private float heightDist;

		// Token: 0x0400171F RID: 5919
		private Vector2 vec1;

		// Token: 0x04001720 RID: 5920
		private Vector2 vec2;

		// Token: 0x04001721 RID: 5921
		private Vector2 _vec1Rotated90;

		// Token: 0x04001722 RID: 5922
		private Vector3 ForwardVectorCached;

		// Token: 0x04001723 RID: 5923
		private Vector3 direction = Vector3.zero;

		// Token: 0x02000388 RID: 904
		[BurstCompile]
		private struct Phase1SetupJob : IJobParallelFor
		{
			// Token: 0x06001792 RID: 6034 RVA: 0x000724FC File Offset: 0x000706FC
			public void Execute(int id)
			{
				float num = this.FirstAngle + this.AngleStep * (float)id;
				this.RayAngles[id] = num;
				float3 @float = this.DirFromAngle(num);
				this.Vector3Directions[id] = @float;
				this.RaycastCommandsNative[id] = new RaycastCommand(this.EyePosition, @float, this.Parameters, this.RayDistance);
			}

			// Token: 0x06001793 RID: 6035 RVA: 0x00072568 File Offset: 0x00070768
			private float3 DirFromAngle(float angleInDegrees)
			{
				float3 result = default(float3);
				switch (this.GamePlane)
				{
				case 0:
					result.x = Mathf.Cos(angleInDegrees * 0.017453292f);
					result.z = Mathf.Sin(angleInDegrees * 0.017453292f);
					return result;
				case 1:
					result.x = Mathf.Cos(angleInDegrees * 0.017453292f);
					result.y = Mathf.Sin(angleInDegrees * 0.017453292f);
					return result;
				}
				result.z = Mathf.Cos(angleInDegrees * 0.017453292f);
				result.y = Mathf.Sin(angleInDegrees * 0.017453292f);
				return result;
			}

			// Token: 0x04001724 RID: 5924
			public int GamePlane;

			// Token: 0x04001725 RID: 5925
			public float FirstAngle;

			// Token: 0x04001726 RID: 5926
			public float AngleStep;

			// Token: 0x04001727 RID: 5927
			public float RayDistance;

			// Token: 0x04001728 RID: 5928
			public Vector3 EyePosition;

			// Token: 0x04001729 RID: 5929
			public QueryParameters Parameters;

			// Token: 0x0400172A RID: 5930
			[WriteOnly]
			public NativeArray<float> RayAngles;

			// Token: 0x0400172B RID: 5931
			[WriteOnly]
			public NativeArray<float3> Vector3Directions;

			// Token: 0x0400172C RID: 5932
			[WriteOnly]
			public NativeArray<RaycastCommand> RaycastCommandsNative;
		}

		// Token: 0x02000389 RID: 905
		[BurstCompile]
		private struct GetVector2Data : IJobParallelFor
		{
			// Token: 0x06001794 RID: 6036 RVA: 0x00072610 File Offset: 0x00070810
			public void Execute(int id)
			{
				float3 @float;
				float3 float2;
				if (!this.approximately(this.RaycastHits[id].distance, 0f))
				{
					this.Hits[id] = true;
					this.Distances[id] = this.RaycastHits[id].distance;
					@float = this.RaycastHits[id].point;
					float2 = this.RaycastHits[id].normal;
				}
				else
				{
					this.Hits[id] = false;
					this.Distances[id] = this.RayDistance;
					@float = this.EyePosition + this.InDirections[id] * this.RayDistance;
					float2 = -this.InDirections[id];
				}
				float2 value = default(float2);
				float2 x = default(float2);
				float2 x2 = default(float2);
				switch (this.GamePlane)
				{
				case 0:
					value.x = @float.x;
					value.y = @float.z;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].z;
					x2.x = float2.x;
					x2.y = float2.z;
					break;
				case 1:
					value.x = @float.x;
					value.y = @float.y;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].y;
					x2.x = float2.x;
					x2.y = float2.y;
					break;
				case 2:
					value.x = @float.z;
					value.y = @float.y;
					x.x = this.InDirections[id].z;
					x.y = this.InDirections[id].y;
					x2.x = float2.z;
					x2.y = float2.y;
					break;
				}
				this.OutPoints[id] = value;
				this.OutDirections[id] = math.normalize(x);
				this.OutNormals[id] = math.normalize(x2);
			}

			// Token: 0x06001795 RID: 6037 RVA: 0x00072896 File Offset: 0x00070A96
			private bool approximately(float a, float b)
			{
				return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), 9.536743E-07f);
			}

			// Token: 0x0400172D RID: 5933
			public int GamePlane;

			// Token: 0x0400172E RID: 5934
			public float RayDistance;

			// Token: 0x0400172F RID: 5935
			public float3 EyePosition;

			// Token: 0x04001730 RID: 5936
			[ReadOnly]
			public NativeArray<RaycastHit> RaycastHits;

			// Token: 0x04001731 RID: 5937
			[WriteOnly]
			public NativeArray<bool> Hits;

			// Token: 0x04001732 RID: 5938
			[WriteOnly]
			public NativeArray<float> Distances;

			// Token: 0x04001733 RID: 5939
			[ReadOnly]
			public NativeArray<float3> InDirections;

			// Token: 0x04001734 RID: 5940
			[WriteOnly]
			public NativeArray<float2> OutPoints;

			// Token: 0x04001735 RID: 5941
			[WriteOnly]
			public NativeArray<float2> OutDirections;

			// Token: 0x04001736 RID: 5942
			[WriteOnly]
			public NativeArray<float2> OutNormals;
		}

		// Token: 0x0200038A RID: 906
		[BurstCompile]
		private struct SightRayFromRaycastHit : IJobParallelFor
		{
			// Token: 0x06001796 RID: 6038 RVA: 0x000728C4 File Offset: 0x00070AC4
			public void Execute(int id)
			{
				FogOfWarRevealer.SightRay value = default(FogOfWarRevealer.SightRay);
				float3 @float;
				float3 float2;
				if (!this.approximately(this.RaycastHits[id].distance, 0f))
				{
					value.hit = true;
					value.distance = this.RaycastHits[id].distance;
					@float = this.RaycastHits[id].point;
					float2 = this.RaycastHits[id].normal;
				}
				else
				{
					value.hit = false;
					value.distance = this.RayDistance;
					@float = this.EyePosition + this.InDirections[id] * this.RayDistance;
					float2 = -this.InDirections[id];
				}
				float2 point = default(float2);
				float2 x = default(float2);
				float2 x2 = default(float2);
				switch (this.GamePlane)
				{
				case 0:
					point.x = @float.x;
					point.y = @float.z;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].z;
					x2.x = float2.x;
					x2.y = float2.z;
					break;
				case 1:
					point.x = @float.x;
					point.y = @float.y;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].y;
					x2.x = float2.x;
					x2.y = float2.y;
					break;
				case 2:
					point.x = @float.z;
					point.y = @float.y;
					x.x = this.InDirections[id].z;
					x.y = this.InDirections[id].y;
					x2.x = float2.z;
					x2.y = float2.y;
					break;
				}
				value.point = point;
				value.direction = math.normalize(x);
				value.normal = math.normalize(x2);
				this.SightRays[id] = value;
			}

			// Token: 0x06001797 RID: 6039 RVA: 0x00072896 File Offset: 0x00070A96
			private bool approximately(float a, float b)
			{
				return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), 9.536743E-07f);
			}

			// Token: 0x04001737 RID: 5943
			public int GamePlane;

			// Token: 0x04001738 RID: 5944
			public float RayDistance;

			// Token: 0x04001739 RID: 5945
			public float3 EyePosition;

			// Token: 0x0400173A RID: 5946
			[ReadOnly]
			public NativeArray<RaycastHit> RaycastHits;

			// Token: 0x0400173B RID: 5947
			[ReadOnly]
			public NativeArray<float3> InDirections;

			// Token: 0x0400173C RID: 5948
			[WriteOnly]
			public NativeArray<FogOfWarRevealer.SightRay> SightRays;
		}
	}
}
