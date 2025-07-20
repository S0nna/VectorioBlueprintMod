using System;
using UnityEngine;

// Token: 0x0200022A RID: 554
public class Rotator : MonoBehaviour
{
	// Token: 0x0600103C RID: 4156 RVA: 0x0004C665 File Offset: 0x0004A865
	public void SetSpeed(float speed)
	{
		this._speed = speed;
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0004C66E File Offset: 0x0004A86E
	public void ToggleAlignmentTracking(bool toggle)
	{
		this._trackAlignment = toggle;
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x0004C677 File Offset: 0x0004A877
	public bool IsAlignedToTarget()
	{
		return this._isAlignedToTarget;
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x0004C67F File Offset: 0x0004A87F
	public void ResetRotation()
	{
		base.transform.rotation = Quaternion.identity;
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0004C691 File Offset: 0x0004A891
	public void Rotate(float time)
	{
		base.transform.Rotate(Vector3.forward, this._speed * time);
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x0004C6AC File Offset: 0x0004A8AC
	public void RotateTowards(Transform target, float time)
	{
		Vector3 vector = target.transform.position - base.transform.position;
		Quaternion quaternion = Quaternion.AngleAxis(Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f, Vector3.forward);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this._speed * time);
		if (this._trackAlignment)
		{
			this._isAlignedToTarget = this.IsPointedAtTarget(quaternion);
		}
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0004C738 File Offset: 0x0004A938
	public void PredicativelyRotateTowards(Transform target, float targetSpeed, float movementSpeed, float time)
	{
		float f = (target.eulerAngles.z + 90f) * 0.017453292f;
		Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f) * targetSpeed;
		Vector3 vector2 = target.position - base.transform.position;
		vector2.z = 0f;
		vector.z = 0f;
		float num = Vector3.Dot(vector, vector) - movementSpeed * movementSpeed;
		float num2 = 2f * Vector3.Dot(vector, vector2);
		float num3 = Vector3.Dot(vector2, vector2);
		float num4 = num2 * num2 - 4f * num * num3;
		if (num4 >= 0f)
		{
			float num5 = (-num2 - Mathf.Sqrt(num4)) / (2f * num);
			float num6 = (-num2 + Mathf.Sqrt(num4)) / (2f * num);
			float num7 = 0f;
			if (num5 > 0f && num6 > 0f)
			{
				num7 = Mathf.Min(num5, num6);
			}
			else if (num5 > 0f)
			{
				num7 = num5;
			}
			else if (num6 > 0f)
			{
				num7 = num6;
			}
			if (num7 > 0f)
			{
				Vector3 vector3 = target.position + vector * num7 - base.transform.position;
				vector3.z = 0f;
				float z = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f - 90f;
				Quaternion quaternion = Quaternion.Euler(0f, 0f, z);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this._speed * time);
				if (this._trackAlignment)
				{
					this._isAlignedToTarget = this.IsPointedAtTarget(quaternion);
				}
			}
		}
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0004C904 File Offset: 0x0004AB04
	public void FaceTowards(Transform target)
	{
		Vector3 vector = target.transform.position - base.transform.position;
		float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f;
		base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		if (this._trackAlignment)
		{
			this._isAlignedToTarget = true;
		}
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0004C970 File Offset: 0x0004AB70
	protected bool IsPointedAtTarget(Quaternion targetRotation)
	{
		return Quaternion.Angle(base.transform.rotation, targetRotation) < 2f;
	}

	// Token: 0x04000E44 RID: 3652
	protected float _speed;

	// Token: 0x04000E45 RID: 3653
	protected bool _trackAlignment;

	// Token: 0x04000E46 RID: 3654
	protected bool _isAlignedToTarget;
}
