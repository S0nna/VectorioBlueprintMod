using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class LogisticsManager : Singleton<LogisticsManager>
{
	// Token: 0x06000E6A RID: 3690 RVA: 0x00040C00 File Offset: 0x0003EE00
	protected void Update()
	{
		if (this._resourceXrayEnabled)
		{
			this.UpdateXrayTargets();
		}
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00040C10 File Offset: 0x0003EE10
	public LogisticsManager.Xray RegisterContainerXray(Transform parent, ResourceContainer container)
	{
		LogisticsManager.XrayObj xrayObj = new LogisticsManager.XrayObj(parent, container);
		this._resourceXrayList.Add(xrayObj);
		return xrayObj;
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x00040C34 File Offset: 0x0003EE34
	public LogisticsManager.Xray RegisterDroneXray(Transform parent, CargoDrone drone)
	{
		LogisticsManager.XrayDrone xrayDrone = new LogisticsManager.XrayDrone(parent, drone);
		this._resourceXrayList.Add(xrayDrone);
		return xrayDrone;
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x00040C58 File Offset: 0x0003EE58
	public void RemoveXrayTarget(LogisticsManager.Xray xrayObj)
	{
		if (this._resourceXrayList.Contains(xrayObj))
		{
			foreach (SpriteRenderer spriteRenderer in xrayObj.icons)
			{
				if (spriteRenderer != null)
				{
					spriteRenderer.gameObject.SetActive(false);
					this._pooledIcons.Push(spriteRenderer);
				}
			}
			xrayObj.icons.Clear();
			this._resourceXrayList.Remove(xrayObj);
			return;
		}
		Debug.Log("[LOGISTICS MANAGER] Couldn't remove container that doesn't exist!");
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00040CF8 File Offset: 0x0003EEF8
	public void ToggleXray()
	{
		this.ToggleXray(!this._resourceXrayEnabled);
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00040D0C File Offset: 0x0003EF0C
	public void ToggleXray(bool toggle)
	{
		this._resourceXrayEnabled = toggle;
		if (toggle)
		{
			this.mainCamera.cullingMask |= 2097152;
			this.mainCamera.cullingMask &= -4194305;
			return;
		}
		this.mainCamera.cullingMask |= 4194304;
		this.mainCamera.cullingMask &= -2097153;
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00040D80 File Offset: 0x0003EF80
	public void ToggleAutoEdit(bool toggle)
	{
		this._autoEditEnabled = toggle;
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00040D89 File Offset: 0x0003EF89
	public bool AudioEdit
	{
		get
		{
			return this._autoEditEnabled;
		}
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x00040D91 File Offset: 0x0003EF91
	public void ToggleRoutePreview(bool toggle)
	{
		this._routePreview = toggle;
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00040D9A File Offset: 0x0003EF9A
	public bool RoutePreview
	{
		get
		{
			return this._routePreview;
		}
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x00040DA4 File Offset: 0x0003EFA4
	private void UpdateXrayTargets()
	{
		int xrayIndexTracker = this._xrayIndexTracker;
		for (int i = 0; i < 20; i++)
		{
			this._xrayIndexTracker++;
			if (this._xrayIndexTracker >= this._resourceXrayList.Count)
			{
				this._xrayIndexTracker = 0;
			}
			if (this._xrayIndexTracker == xrayIndexTracker)
			{
				break;
			}
			object resourceXrayListLock = this._resourceXrayListLock;
			LogisticsManager.Xray xrayObj;
			lock (resourceXrayListLock)
			{
				if (this._xrayIndexTracker >= this._resourceXrayList.Count)
				{
					break;
				}
				xrayObj = this._resourceXrayList[this._xrayIndexTracker];
			}
			this.UpdateXrayTarget(xrayObj);
		}
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x00040E5C File Offset: 0x0003F05C
	private void UpdateXrayTarget(LogisticsManager.Xray xrayObj)
	{
		if (xrayObj != null && xrayObj.parent != null)
		{
			List<ResourceData> resources = xrayObj.GetResources();
			this.UpdateXraySpritePositions(xrayObj.parent, ref xrayObj.icons, resources.Count);
			for (int i = 0; i < resources.Count; i++)
			{
				xrayObj.icons[i].sprite = resources[i].IconSprite;
			}
			return;
		}
		this.RemoveXrayTarget(xrayObj);
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x00040ED0 File Offset: 0x0003F0D0
	private void UpdateXraySpritePositions(Transform parent, ref List<SpriteRenderer> sprites, int amount)
	{
		if (parent == null)
		{
			Debug.LogError("Parent transform is null.");
			return;
		}
		if (this._pooledIcons == null)
		{
			Debug.LogError("_pooledIcons stack is null.");
			return;
		}
		if (this.xrayPrefab == null)
		{
			Debug.LogError("xrayPrefab is null.");
			return;
		}
		if (sprites == null)
		{
			Debug.LogError("Sprites list is null.");
			sprites = new List<SpriteRenderer>();
		}
		if (sprites.Count < amount)
		{
			while (sprites.Count < amount)
			{
				SpriteRenderer spriteRenderer;
				if (this._pooledIcons.TryPop(out spriteRenderer))
				{
					if (spriteRenderer == null)
					{
						continue;
					}
					spriteRenderer.gameObject.SetActive(true);
				}
				else
				{
					spriteRenderer = Object.Instantiate<SpriteRenderer>(this.xrayPrefab);
				}
				spriteRenderer.transform.parent = parent;
				sprites.Add(spriteRenderer);
			}
		}
		else
		{
			while (sprites.Count > amount)
			{
				int index = sprites.Count - 1;
				this._spriteTarget = sprites[index];
				if (this._spriteTarget == null)
				{
					Debug.LogError("Sprite target is null.");
				}
				else
				{
					this._spriteTarget.gameObject.SetActive(false);
					this._spriteTarget.transform.parent = base.transform;
					this._spriteTarget.transform.localPosition = Vector2.zero;
					this._pooledIcons.Push(this._spriteTarget);
					sprites.RemoveAt(index);
				}
			}
		}
		if (amount > 0)
		{
			Vector2 vector = this.oneSpritePosition;
			Vector2 vector2 = this.twoSpritePositionA;
			Vector2 vector3 = this.twoSpritePositionB;
			Vector2 vector4 = this.threeSpritePositionA;
			Vector2 vector5 = this.threeSpritePositionB;
			Vector2 vector6 = this.threeSpritePositionC;
			Vector2 vector7 = this.fourSpritePositionA;
			Vector2 vector8 = this.fourSpritePositionB;
			Vector2 vector9 = this.fourSpritePositionC;
			Vector2 vector10 = this.fourSpritePositionD;
			switch (amount)
			{
			case 1:
				sprites[0].transform.localPosition = this.oneSpritePosition;
				return;
			case 2:
				sprites[0].transform.localPosition = this.twoSpritePositionA;
				sprites[1].transform.localPosition = this.twoSpritePositionB;
				return;
			case 3:
				sprites[0].transform.localPosition = this.threeSpritePositionA;
				sprites[1].transform.localPosition = this.threeSpritePositionB;
				sprites[2].transform.localPosition = this.threeSpritePositionC;
				return;
			case 4:
				sprites[0].transform.localPosition = this.fourSpritePositionA;
				sprites[1].transform.localPosition = this.fourSpritePositionB;
				sprites[2].transform.localPosition = this.fourSpritePositionC;
				sprites[3].transform.localPosition = this.fourSpritePositionD;
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x04000B43 RID: 2883
	protected const int MAX_XRAY_TARGETS = 20;

	// Token: 0x04000B44 RID: 2884
	public Camera mainCamera;

	// Token: 0x04000B45 RID: 2885
	public SpriteRenderer xrayPrefab;

	// Token: 0x04000B46 RID: 2886
	public SpriteRenderer shadowPrefab;

	// Token: 0x04000B47 RID: 2887
	private SpriteRenderer _spriteTarget;

	// Token: 0x04000B48 RID: 2888
	public Vector2 oneSpritePosition;

	// Token: 0x04000B49 RID: 2889
	public Vector2 twoSpritePositionA;

	// Token: 0x04000B4A RID: 2890
	public Vector2 twoSpritePositionB;

	// Token: 0x04000B4B RID: 2891
	public Vector2 threeSpritePositionA;

	// Token: 0x04000B4C RID: 2892
	public Vector2 threeSpritePositionB;

	// Token: 0x04000B4D RID: 2893
	public Vector2 threeSpritePositionC;

	// Token: 0x04000B4E RID: 2894
	public Vector2 fourSpritePositionA;

	// Token: 0x04000B4F RID: 2895
	public Vector2 fourSpritePositionB;

	// Token: 0x04000B50 RID: 2896
	public Vector2 fourSpritePositionC;

	// Token: 0x04000B51 RID: 2897
	public Vector2 fourSpritePositionD;

	// Token: 0x04000B52 RID: 2898
	protected ConcurrentStack<SpriteRenderer> _pooledIcons = new ConcurrentStack<SpriteRenderer>();

	// Token: 0x04000B53 RID: 2899
	protected List<LogisticsManager.Xray> _resourceXrayList = new List<LogisticsManager.Xray>();

	// Token: 0x04000B54 RID: 2900
	private readonly object _resourceXrayListLock = new object();

	// Token: 0x04000B55 RID: 2901
	protected bool _resourceXrayEnabled;

	// Token: 0x04000B56 RID: 2902
	protected int _xrayIndexTracker;

	// Token: 0x04000B57 RID: 2903
	protected bool _autoEditEnabled;

	// Token: 0x04000B58 RID: 2904
	protected bool _routePreview;

	// Token: 0x020001C9 RID: 457
	public abstract class Xray
	{
		// Token: 0x06000E78 RID: 3704
		public abstract List<ResourceData> GetResources();

		// Token: 0x04000B59 RID: 2905
		public Transform parent;

		// Token: 0x04000B5A RID: 2906
		public List<SpriteRenderer> icons;
	}

	// Token: 0x020001CA RID: 458
	public class XrayObj : LogisticsManager.Xray
	{
		// Token: 0x06000E7A RID: 3706 RVA: 0x000411E3 File Offset: 0x0003F3E3
		public XrayObj(Transform parent, ResourceContainer target)
		{
			this.parent = parent;
			this.target = target;
			this.icons = new List<SpriteRenderer>();
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00041204 File Offset: 0x0003F404
		public override List<ResourceData> GetResources()
		{
			return this.target.GetMostStored();
		}

		// Token: 0x04000B5B RID: 2907
		public ResourceContainer target;
	}

	// Token: 0x020001CB RID: 459
	public class XrayDrone : LogisticsManager.Xray
	{
		// Token: 0x06000E7C RID: 3708 RVA: 0x00041211 File Offset: 0x0003F411
		public XrayDrone(Transform parent, CargoDrone target)
		{
			this.parent = parent;
			this.target = target;
			this.icons = new List<SpriteRenderer>();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00041232 File Offset: 0x0003F432
		public override List<ResourceData> GetResources()
		{
			return new List<ResourceData>
			{
				this.target.Filter
			};
		}

		// Token: 0x04000B5C RID: 2908
		public CargoDrone target;
	}
}
