using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class LeanSmooth
{
	// Token: 0x060000F7 RID: 247 RVA: 0x00008F3C File Offset: 0x0000713C
	public static float damp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		smoothTime = Mathf.Max(0.0001f, smoothTime);
		float num = 2f / smoothTime;
		float num2 = num * deltaTime;
		float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
		float num4 = current - target;
		float num5 = target;
		if (maxSpeed > 0f)
		{
			float num6 = maxSpeed * smoothTime;
			num4 = Mathf.Clamp(num4, -num6, num6);
		}
		target = current - num4;
		float num7 = (currentVelocity + num * num4) * deltaTime;
		currentVelocity = (currentVelocity - num * num7) * num3;
		float num8 = target + (num4 + num7) * num3;
		if (num5 - current > 0f == num8 > num5)
		{
			num8 = num5;
			currentVelocity = (num8 - num5) / deltaTime;
		}
		return num8;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00009004 File Offset: 0x00007204
	public static Vector3 damp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		float x = LeanSmooth.damp(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime);
		float y = LeanSmooth.damp(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime);
		float z = LeanSmooth.damp(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime);
		return new Vector3(x, y, z);
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00009070 File Offset: 0x00007270
	public static Color damp(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f)
	{
		float r = LeanSmooth.damp(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime);
		float g = LeanSmooth.damp(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime);
		float b = LeanSmooth.damp(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime);
		float a = LeanSmooth.damp(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime);
		return new Color(r, g, b, a);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000090F8 File Offset: 0x000072F8
	public static float spring(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		float num = target - current;
		currentVelocity += deltaTime / smoothTime * accelRate * num;
		currentVelocity *= 1f - deltaTime * friction;
		if (maxSpeed > 0f && maxSpeed < Mathf.Abs(currentVelocity))
		{
			currentVelocity = maxSpeed * Mathf.Sign(currentVelocity);
		}
		return current + currentVelocity;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0000915C File Offset: 0x0000735C
	public static Vector3 spring(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		float x = LeanSmooth.spring(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float y = LeanSmooth.spring(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float z = LeanSmooth.spring(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		return new Vector3(x, y, z);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x000091D4 File Offset: 0x000073D4
	public static Color spring(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f)
	{
		float r = LeanSmooth.spring(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float g = LeanSmooth.spring(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float b = LeanSmooth.spring(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		float a = LeanSmooth.spring(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime, friction, accelRate);
		return new Color(r, g, b, a);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x0000926C File Offset: 0x0000746C
	public static float linear(float current, float target, float moveSpeed, float deltaTime = -1f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		bool flag = target > current;
		float num = deltaTime * moveSpeed * (flag ? 1f : -1f);
		float num2 = current + num;
		float num3 = num2 - target;
		if ((flag && num3 > 0f) || (!flag && num3 < 0f))
		{
			return target;
		}
		return num2;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000092C4 File Offset: 0x000074C4
	public static Vector3 linear(Vector3 current, Vector3 target, float moveSpeed, float deltaTime = -1f)
	{
		float x = LeanSmooth.linear(current.x, target.x, moveSpeed, deltaTime);
		float y = LeanSmooth.linear(current.y, target.y, moveSpeed, deltaTime);
		float z = LeanSmooth.linear(current.z, target.z, moveSpeed, deltaTime);
		return new Vector3(x, y, z);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00009314 File Offset: 0x00007514
	public static Color linear(Color current, Color target, float moveSpeed)
	{
		float r = LeanSmooth.linear(current.r, target.r, moveSpeed, -1f);
		float g = LeanSmooth.linear(current.g, target.g, moveSpeed, -1f);
		float b = LeanSmooth.linear(current.b, target.b, moveSpeed, -1f);
		float a = LeanSmooth.linear(current.a, target.a, moveSpeed, -1f);
		return new Color(r, g, b, a);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00009388 File Offset: 0x00007588
	public static float bounceOut(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		if (deltaTime < 0f)
		{
			deltaTime = Time.deltaTime;
		}
		float num = target - current;
		currentVelocity += deltaTime / smoothTime * accelRate * num;
		currentVelocity *= 1f - deltaTime * friction;
		if (maxSpeed > 0f && maxSpeed < Mathf.Abs(currentVelocity))
		{
			currentVelocity = maxSpeed * Mathf.Sign(currentVelocity);
		}
		float num2 = current + currentVelocity;
		bool flag = target > current;
		float num3 = num2 - target;
		if ((flag && num3 > 0f) || (!flag && num3 < 0f))
		{
			currentVelocity = -currentVelocity * hitDamping;
			num2 = current + currentVelocity;
		}
		return num2;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x0000941C File Offset: 0x0000761C
	public static Vector3 bounceOut(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		float x = LeanSmooth.bounceOut(current.x, target.x, ref currentVelocity.x, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float y = LeanSmooth.bounceOut(current.y, target.y, ref currentVelocity.y, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float z = LeanSmooth.bounceOut(current.z, target.z, ref currentVelocity.z, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		return new Vector3(x, y, z);
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00009498 File Offset: 0x00007698
	public static Color bounceOut(Color current, Color target, ref Color currentVelocity, float smoothTime, float maxSpeed = -1f, float deltaTime = -1f, float friction = 2f, float accelRate = 0.5f, float hitDamping = 0.9f)
	{
		float r = LeanSmooth.bounceOut(current.r, target.r, ref currentVelocity.r, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float g = LeanSmooth.bounceOut(current.g, target.g, ref currentVelocity.g, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float b = LeanSmooth.bounceOut(current.b, target.b, ref currentVelocity.b, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		float a = LeanSmooth.bounceOut(current.a, target.a, ref currentVelocity.a, smoothTime, maxSpeed, deltaTime, friction, accelRate, hitDamping);
		return new Color(r, g, b, a);
	}
}
