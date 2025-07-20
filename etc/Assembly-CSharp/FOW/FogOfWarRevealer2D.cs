using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000386 RID: 902
	public class FogOfWarRevealer2D : FogOfWarRevealer
	{
		// Token: 0x0600176B RID: 5995 RVA: 0x0007097E File Offset: 0x0006EB7E
		protected override void _InitRevealer(int StepCount)
		{
			this.InitialRayResults = new RaycastHit2D[StepCount];
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00003212 File Offset: 0x00001412
		protected override void CleanupRevealer()
		{
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0007098C File Offset: 0x0006EB8C
		protected override void IterationOne(int NumSteps, float firstAngle, float angleStep)
		{
			for (int i = 0; i < NumSteps; i++)
			{
				this.FirstIteration.RayAngles[i] = firstAngle + angleStep * (float)i;
				this.FirstIteration.Directions[i] = this.DirectionFromAngle(this.FirstIteration.RayAngles[i], true);
				this.RayHit = Physics2D.Raycast(this.EyePosition, this.FirstIteration.Directions[i], this.RayDistance, this.ObstacleMask);
				if (this.RayHit.collider != null)
				{
					this.FirstIteration.Hits[i] = true;
					this.FirstIteration.Normals[i] = this.RayHit.normal;
					this.FirstIteration.Distances[i] = this.RayHit.distance;
					this.FirstIteration.Points[i] = this.RayHit.point;
				}
				else
				{
					this.FirstIteration.Hits[i] = false;
					this.FirstIteration.Normals[i] = -this.FirstIteration.Directions[i];
					this.FirstIteration.Distances[i] = this.RayDistance;
					this.FirstIteration.Points[i] = this.GetPositionxy(this.EyePosition) + this.FirstIteration.Directions[i] * this.RayDistance;
				}
			}
			this.PointsJobHandle = this.PointsJob.Schedule(NumSteps, this.CommandsPerJob, default(JobHandle));
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00070B64 File Offset: 0x0006ED64
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void RayCast(float angle, ref FogOfWarRevealer.SightRay ray)
		{
			Vector2 vector = this.DirectionFromAngle(angle, true);
			ray.angle = angle;
			ray.direction = vector;
			this.RayHit = Physics2D.Raycast(this.EyePosition, vector, this.RayDistance, this.ObstacleMask);
			if (this.RayHit.collider != null)
			{
				ray.hit = true;
				ray.normal = this.RayHit.normal;
				ray.distance = this.RayHit.distance;
				ray.point = this.RayHit.point;
				return;
			}
			ray.hit = false;
			ray.normal = -vector;
			ray.distance = this.RayDistance;
			ray.point = this.GetPositionxy(this.EyePosition) + ray.direction * this.RayDistance;
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00070C59 File Offset: 0x0006EE59
		private float2 GetPositionxy(Vector3 pos)
		{
			this.pos2d.x = pos.x;
			this.pos2d.y = pos.y;
			return this.pos2d;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00003212 File Offset: 0x00001412
		protected override void _FindEdge()
		{
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00070C84 File Offset: 0x0006EE84
		protected override float GetEuler()
		{
			Vector3 up = base.transform.up;
			up.z = 0f;
			up.Normalize();
			return -Vector3.SignedAngle(up, Vector3.up, -Vector3.forward);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00070CC8 File Offset: 0x0006EEC8
		public override Vector3 GetEyePosition()
		{
			Vector3 vector = base.transform.position;
			if (FogOfWarWorld.instance.PixelateFog && FogOfWarWorld.instance.RoundRevealerPosition)
			{
				vector *= FogOfWarWorld.instance.PixelDensity;
				Vector3 b = new Vector3(FogOfWarWorld.instance.PixelGridOffset.x, FogOfWarWorld.instance.PixelGridOffset.y, 0f);
				vector -= b;
				vector = Vector3Int.RoundToInt(vector);
				vector += b;
				vector /= FogOfWarWorld.instance.PixelDensity;
			}
			return vector;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00070D64 File Offset: 0x0006EF64
		protected override void _RevealHiders()
		{
			this.EyePosition = this.GetEyePosition();
			this.ForwardVectorCached = this.GetForward();
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			for (int i = 0; i < Mathf.Min(this.MaxHidersSampledPerFrame, FogOfWarWorld.NumHiders); i++)
			{
				this._lastHiderIndex = (this._lastHiderIndex + 1) % FogOfWarWorld.NumHiders;
				FogOfWarHider fogOfWarHider = FogOfWarWorld.HidersList[this._lastHiderIndex];
				bool flag = false;
				float num2 = this.distBetweenVectors(fogOfWarHider.transform.position, this.EyePosition) - fogOfWarHider.MaxDistBetweenSamplePoints;
				if (num2 < this.UnobscuredRadius || num2 < num)
				{
					for (int j = 0; j < fogOfWarHider.SamplePoints.Length; j++)
					{
						Transform transform = fogOfWarHider.SamplePoints[j];
						float num3 = this.distBetweenVectors(transform.position, this.EyePosition);
						if (num3 < this.UnobscuredRadius || (num3 < num && Mathf.Abs(this.AngleBetweenVector2(transform.position - this.EyePosition, this.ForwardVectorCached)) < this.ViewAngle / 2f))
						{
							this.SetHiderPosition(transform.position);
							if (!Physics2D.Raycast(this.EyePosition, this.hiderPosition - this.EyePosition, num3, this.ObstacleMask))
							{
								flag = true;
								break;
							}
						}
					}
				}
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

		// Token: 0x06001774 RID: 6004 RVA: 0x00070F5E File Offset: 0x0006F15E
		private void SetHiderPosition(Vector3 point)
		{
			this.hiderPosition.x = point.x;
			this.hiderPosition.y = point.y;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00070F84 File Offset: 0x0006F184
		protected override bool _TestPoint(Vector3 point)
		{
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			this.EyePosition = this.GetEyePosition();
			float num2 = this.distBetweenVectors(point, this.EyePosition);
			if (num2 < this.UnobscuredRadius || (num2 < num && Mathf.Abs(this.AngleBetweenVector2(point - this.EyePosition, this.GetForward())) < this.ViewAngle / 2f))
			{
				this.SetHiderPosition(point);
				if (!Physics2D.Raycast(this.EyePosition, this.hiderPosition - base.transform.position, num2, this.ObstacleMask))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00071050 File Offset: 0x0006F250
		protected override void SetCenterAndHeight()
		{
			this.center.x = this.EyePosition.x;
			this.center.y = this.EyePosition.y;
			this.heightPos = base.transform.position.z;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000710A0 File Offset: 0x0006F2A0
		protected override float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2)
		{
			this.vec1.x = _vec1.x;
			this.vec1.y = _vec1.y;
			this.vec2.x = _vec2.x;
			this.vec2.y = _vec2.y;
			this._vec1Rotated90.x = -this.vec1.y;
			this._vec1Rotated90.y = this.vec1.x;
			float num = (Vector2.Dot(this._vec1Rotated90, this.vec2) < 0f) ? -1f : 1f;
			return Vector2.Angle(this.vec1, this.vec2) * num;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00071158 File Offset: 0x0006F358
		private float distBetweenVectors(Vector3 _vec1, Vector3 _vec2)
		{
			this.vec1.x = _vec1.x;
			this.vec1.y = _vec1.y;
			this.vec2.x = _vec2.x;
			this.vec2.y = _vec2.y;
			return Vector2.Distance(this.vec1, this.vec2);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000711BC File Offset: 0x0006F3BC
		private Vector3 GetForward()
		{
			return new Vector3(base.transform.up.x, base.transform.up.y, 0f).normalized;
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000711FC File Offset: 0x0006F3FC
		private Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
			{
				angleInDegrees += base.transform.eulerAngles.z;
			}
			this.direction2d.x = Mathf.Cos(angleInDegrees * 0.017453292f);
			this.direction2d.y = Mathf.Sin(angleInDegrees * 0.017453292f);
			return this.direction2d;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00071254 File Offset: 0x0006F454
		public override Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
			{
				angleInDegrees += base.transform.eulerAngles.z;
			}
			this.direction.x = Mathf.Cos(angleInDegrees * 0.017453292f);
			this.direction.y = Mathf.Sin(angleInDegrees * 0.017453292f);
			return this.direction;
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000712AC File Offset: 0x0006F4AC
		protected override Vector3 _Get3Dfrom2D(Vector2 pos)
		{
			return new Vector3(pos.x, pos.y, 0f);
		}

		// Token: 0x04001707 RID: 5895
		private RaycastHit2D[] InitialRayResults;

		// Token: 0x04001708 RID: 5896
		private RaycastHit2D RayHit;

		// Token: 0x04001709 RID: 5897
		private float2 pos2d;

		// Token: 0x0400170A RID: 5898
		private Vector3 hiderPosition;

		// Token: 0x0400170B RID: 5899
		private Vector2 vec1;

		// Token: 0x0400170C RID: 5900
		private Vector2 vec2;

		// Token: 0x0400170D RID: 5901
		private Vector2 _vec1Rotated90;

		// Token: 0x0400170E RID: 5902
		private Vector3 ForwardVectorCached;

		// Token: 0x0400170F RID: 5903
		private RaycastHit2D rayHit;

		// Token: 0x04001710 RID: 5904
		private Vector2 direction2d = Vector3.zero;

		// Token: 0x04001711 RID: 5905
		private Vector3 direction = Vector3.zero;
	}
}
