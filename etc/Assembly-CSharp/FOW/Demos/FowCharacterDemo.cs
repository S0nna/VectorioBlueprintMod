using System;
using UnityEngine;

namespace FOW.Demos
{
	// Token: 0x0200038F RID: 911
	public class FowCharacterDemo : MonoBehaviour
	{
		// Token: 0x060017A5 RID: 6053 RVA: 0x00072E98 File Offset: 0x00071098
		private void Awake()
		{
			this.cc = base.GetComponent<CharacterController>();
			this.CursorLocked = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00072EBC File Offset: 0x000710BC
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.CursorLocked = !this.CursorLocked;
				if (this.CursorLocked)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
			}
			if (this.CursorLocked)
			{
				base.transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f);
				this.yRot -= Input.GetAxis("Mouse Y");
			}
			this.yRot = Mathf.Clamp(this.yRot, -80f, 80f);
			this.setInput();
			this.move();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00072F68 File Offset: 0x00071168
		public void setInput()
		{
			bool[] array = new bool[]
			{
				Input.GetKey(KeyCode.W),
				Input.GetKey(KeyCode.A),
				Input.GetKey(KeyCode.S),
				Input.GetKey(KeyCode.D),
				Input.GetKey(KeyCode.LeftShift)
			};
			this.speedTarget = 0f;
			this.inputDirection = Vector2.zero;
			if (array[0])
			{
				this.inputDirection.y = this.inputDirection.y + 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[1])
			{
				this.inputDirection.x = this.inputDirection.x - 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[2])
			{
				this.inputDirection.y = this.inputDirection.y - 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[3])
			{
				this.inputDirection.x = this.inputDirection.x + 1f;
				this.speedTarget = this.WalkingSpeed;
			}
			if (array[4])
			{
				this.speedTarget *= this.RunningMultiplier;
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00073074 File Offset: 0x00071274
		private void move()
		{
			if (this.cc.isGrounded)
			{
				this.velocity.y = 0f;
			}
			Vector2 a = new Vector2(base.transform.forward.x, base.transform.forward.z);
			Vector2 vector = Vector3.Normalize(new Vector2(base.transform.right.x, base.transform.right.z) * this.inputDirection.x + a * this.inputDirection.y);
			this.velocityXZ = Vector2.MoveTowards(this.velocityXZ, vector.normalized * this.speedTarget, Time.deltaTime * this.Acceleration);
			this.velocity.x = this.velocityXZ.x * Time.deltaTime;
			this.velocity.z = this.velocityXZ.y * Time.deltaTime;
			this.velocity.y = this.velocity.y + -9.81f * Time.deltaTime * Time.deltaTime;
			this.cc.enabled = true;
			this.cc.Move(this.velocity);
			this.cc.enabled = false;
		}

		// Token: 0x04001748 RID: 5960
		public float WalkingSpeed = 5f;

		// Token: 0x04001749 RID: 5961
		public float RunningMultiplier = 1.65f;

		// Token: 0x0400174A RID: 5962
		public float Acceleration = 25f;

		// Token: 0x0400174B RID: 5963
		private float yRot;

		// Token: 0x0400174C RID: 5964
		private CharacterController cc;

		// Token: 0x0400174D RID: 5965
		private bool CursorLocked;

		// Token: 0x0400174E RID: 5966
		private Vector2 inputDirection = Vector2.zero;

		// Token: 0x0400174F RID: 5967
		private Vector2 velocityXZ = Vector2.zero;

		// Token: 0x04001750 RID: 5968
		private Vector3 velocity = Vector3.zero;

		// Token: 0x04001751 RID: 5969
		private float speedTarget;
	}
}
