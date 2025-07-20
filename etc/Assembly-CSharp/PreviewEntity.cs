using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vectorio.Utilities;

// Token: 0x020001DF RID: 479
public class PreviewEntity : MonoBehaviour
{
	// Token: 0x06000EDD RID: 3805 RVA: 0x000446EE File Offset: 0x000428EE
	public void TogglePreviewCamera(bool toggle)
	{
		this._previewCameraActive = toggle;
		this._previewCamera.enabled = toggle;
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00044704 File Offset: 0x00042904
	public void Start()
	{
		this._modelParent = new GameObject("Preview Model").GetComponent<Transform>();
		this._modelParent.position = new Vector2((float)this.previewPosition.x * 5f, (float)this.previewPosition.y * 5f);
		this._previewCamera.transform.position = this._modelParent.position;
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0004477C File Offset: 0x0004297C
	public void Update()
	{
		if (this._previewCameraActive && this._previewCamera.orthographicSize > this._previewCameraTargetSize)
		{
			this._previewCameraTime += Time.deltaTime;
			this._previewCamera.orthographicSize = Mathf.SmoothStep(this._previewCameraStartSize, this._previewCameraTargetSize, this._previewCameraTime * this.previewCameraZoomSpeed);
		}
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x000447E0 File Offset: 0x000429E0
	public void SetEntity(EntityData data)
	{
		if (this._modelObj != null)
		{
			Object.Destroy(this._modelObj.gameObject);
		}
		this._modelObj = Singleton<ModelConstructor>.Instance.GetModel(data.model, null, data.sortingLayer, true);
		this._modelObj.transform.position = Utilities.ConvertCellPositionToWorld(this.previewPosition);
		Singleton<AnimationManager>.Instance.RegisterPlacementAnimation(this._modelObj.transform, false);
		float previewSize = data.previewSize;
		this._previewCameraStartSize = 20f + previewSize;
		this._previewCameraTargetSize = previewSize;
		this._previewCamera.orthographicSize = this._previewCameraStartSize;
		this._previewCamera.targetTexture = this.cameraRenderTexture;
		this._previewCamera.transform.position = this._modelParent.position;
		this._previewCameraTime = 0f;
		List<Color> getReclaimerTileColors = Singleton<TileGrid>.Instance.GetReclaimerTileColors;
		for (int i = this.previewPosition.x - 15; i <= this.previewPosition.x + 15; i++)
		{
			for (int j = this.previewPosition.y - 15; j <= this.previewPosition.y + 15; j++)
			{
				Vector3Int position = new Vector3Int(i, j, 0);
				this._gridTilemap.SetTile(position, this.previewTile);
				this._gridTilemap.SetTileFlags(position, TileFlags.None);
				this._gridTilemap.SetColor(position, getReclaimerTileColors[Random.Range(0, getReclaimerTileColors.Count)]);
			}
		}
		this.TogglePreviewCamera(true);
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00044970 File Offset: 0x00042B70
	public void SetResource(ResourceData data)
	{
		if (this._modelObj != null)
		{
			Object.Destroy(this._modelObj.gameObject);
		}
		this._previewCameraStartSize = 25f;
		this._previewCameraTargetSize = 18f;
		this._previewCamera.orthographicSize = this._previewCameraStartSize;
		this._previewCamera.targetTexture = this.cameraRenderTexture;
		this._previewCamera.transform.position = this._modelParent.position;
		this._previewCameraTime = 0f;
		for (int i = this.previewPosition.x - 15; i <= this.previewPosition.x + 15; i++)
		{
			for (int j = this.previewPosition.y - 15; j <= this.previewPosition.y + 15; j++)
			{
				Vector3Int position = new Vector3Int(i, j, 0);
				this._gridTilemap.SetTile(position, this.previewTile);
				this._gridTilemap.SetTileFlags(position, TileFlags.None);
				this._gridTilemap.SetColor(position, Singleton<TileGrid>.Instance.GetRandomClaimColor());
			}
		}
		Vector3Int vector3Int = new Vector3Int(this.previewPosition.x, this.previewPosition.y, 0);
		foreach (object obj in this.resourceNodes[Random.Range(0, this.resourceNodes.Count)].transform)
		{
			Transform transform = (Transform)obj;
			Vector3Int position2 = new Vector3Int(vector3Int.x + (int)transform.localPosition.x, vector3Int.y + (int)transform.localPosition.y, 0);
			this._gridTilemap.SetColor(position2, Color.white);
			this._gridTilemap.SetTile(position2, Singleton<TileGrid>.Instance.FetchTile(data.Tile));
		}
		this.TogglePreviewCamera(true);
	}

	// Token: 0x04000BE6 RID: 3046
	protected const float START_SIZE = 20f;

	// Token: 0x04000BE7 RID: 3047
	public CollectorData collectorData;

	// Token: 0x04000BE8 RID: 3048
	[SerializeField]
	protected Camera _previewCamera;

	// Token: 0x04000BE9 RID: 3049
	[SerializeField]
	protected Tilemap _gridTilemap;

	// Token: 0x04000BEA RID: 3050
	public Vector2Int previewPosition;

	// Token: 0x04000BEB RID: 3051
	public float previewCameraZoomSpeed = 1f;

	// Token: 0x04000BEC RID: 3052
	public TileBase previewTile;

	// Token: 0x04000BED RID: 3053
	public RenderTexture cameraRenderTexture;

	// Token: 0x04000BEE RID: 3054
	public List<GameObject> resourceNodes;

	// Token: 0x04000BEF RID: 3055
	protected float _previewCameraTime;

	// Token: 0x04000BF0 RID: 3056
	protected float _previewCameraTargetSize = 25f;

	// Token: 0x04000BF1 RID: 3057
	protected float _previewCameraStartSize = 35f;

	// Token: 0x04000BF2 RID: 3058
	protected Transform _modelParent;

	// Token: 0x04000BF3 RID: 3059
	protected ModelObject _modelObj;

	// Token: 0x04000BF4 RID: 3060
	protected bool _previewCameraActive;
}
