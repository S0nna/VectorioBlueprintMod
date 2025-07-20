using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class PortDoors : MonoBehaviour
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00028AC5 File Offset: 0x00026CC5
	// (set) Token: 0x060009A6 RID: 2470 RVA: 0x00028ACD File Offset: 0x00026CCD
	public float ElapsedTime
	{
		get
		{
			return this._elapsedTime;
		}
		set
		{
			this._elapsedTime = value;
		}
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00028AD6 File Offset: 0x00026CD6
	public void Setup(Port port, Drone drone, Transform leftDoor, Transform rightDoor)
	{
		this._port = port;
		this._portData = port.GetData();
		this._drone = drone;
		this._leftDoor = leftDoor;
		this._rightDoor = rightDoor;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00028B04 File Offset: 0x00026D04
	public void ResetDoors()
	{
		this._leftDoor.localPosition = this._portData.doorClosedPosition;
		this._rightDoor.localPosition = this._portData.doorClosedPosition;
		this._leftDoor.localScale = this._portData.doorClosedScale;
		this._rightDoor.localScale = this._portData.doorClosedScale;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00028B80 File Offset: 0x00026D80
	public bool OpenDoors(float timeSinceLastUpdate)
	{
		this._elapsedTime += timeSinceLastUpdate;
		float num = Mathf.Clamp01(this._elapsedTime / this._port.DoorOpenTime.Value);
		this._leftDoor.localPosition = Vector3.Lerp(-this._portData.doorClosedPosition, -this._portData.doorOpenPosition, num);
		this._rightDoor.localPosition = Vector3.Lerp(this._portData.doorClosedPosition, this._portData.doorOpenPosition, num);
		this._leftDoor.localScale = Vector3.Lerp(this._portData.doorClosedScale, this._portData.doorOpenScale, num);
		this._rightDoor.localScale = Vector3.Lerp(this._portData.doorClosedScale, this._portData.doorOpenScale, num);
		if (num >= 1f)
		{
			this._elapsedTime = 0f;
			return true;
		}
		return false;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00028C9C File Offset: 0x00026E9C
	public bool CloseDoors(float timeSinceLastUpdate)
	{
		this._elapsedTime += timeSinceLastUpdate;
		float num = Mathf.Clamp01(this._elapsedTime / this._port.DoorOpenTime.Value);
		this._leftDoor.localPosition = Vector3.Lerp(-this._portData.doorOpenPosition, -this._portData.doorClosedPosition, num);
		this._rightDoor.localPosition = Vector3.Lerp(this._portData.doorOpenPosition, this._portData.doorClosedPosition, num);
		this._leftDoor.localScale = Vector3.Lerp(this._portData.doorOpenScale, this._portData.doorClosedScale, num);
		this._rightDoor.localScale = Vector3.Lerp(this._portData.doorOpenScale, this._portData.doorClosedScale, num);
		if (num >= 1f)
		{
			this._elapsedTime = 0f;
			this.ResetDoors();
			return true;
		}
		return false;
	}

	// Token: 0x040005EF RID: 1519
	protected Port _port;

	// Token: 0x040005F0 RID: 1520
	protected PortData _portData;

	// Token: 0x040005F1 RID: 1521
	protected Drone _drone;

	// Token: 0x040005F2 RID: 1522
	protected Transform _leftDoor;

	// Token: 0x040005F3 RID: 1523
	protected Transform _rightDoor;

	// Token: 0x040005F4 RID: 1524
	protected bool _isOnScreen;

	// Token: 0x040005F5 RID: 1525
	protected float _elapsedTime;
}
