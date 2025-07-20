using System;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class Wall : BuildingComponent, IComponent<Wall, WallData>
{
	// Token: 0x06000A78 RID: 2680 RVA: 0x0002BF59 File Offset: 0x0002A159
	public WallData GetData()
	{
		return this._wallData;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0002BF61 File Offset: 0x0002A161
	public void OnInitialize(WallData data)
	{
		this._wallData = data;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0002BF6A File Offset: 0x0002A16A
	public override void OnSpawn(bool fromSave)
	{
		this.wallPiece = this._wallData.RequestWallPiece(base.Entity.GetModel.ID);
		this.CheckForConnections();
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0002BF94 File Offset: 0x0002A194
	public void CheckForConnections()
	{
		Vector2Int vector2Int = base.Building.Cells[0];
		this.northWall = Singleton<TileGrid>.Instance.IsCellOccupiedByType<Wall>(new Vector2Int(vector2Int.x, vector2Int.y + 1));
		if (this.northWall != null)
		{
			this.northWall.southWall = this;
			this.northWall.UpdateConnection();
		}
		this.eastWall = Singleton<TileGrid>.Instance.IsCellOccupiedByType<Wall>(new Vector2Int(vector2Int.x + 1, vector2Int.y));
		if (this.eastWall != null)
		{
			this.eastWall.westWall = this;
			this.eastWall.UpdateConnection();
		}
		this.southWall = Singleton<TileGrid>.Instance.IsCellOccupiedByType<Wall>(new Vector2Int(vector2Int.x, vector2Int.y - 1));
		if (this.southWall != null)
		{
			this.southWall.northWall = this;
			this.southWall.UpdateConnection();
		}
		this.westWall = Singleton<TileGrid>.Instance.IsCellOccupiedByType<Wall>(new Vector2Int(vector2Int.x - 1, vector2Int.y));
		if (this.westWall != null)
		{
			this.westWall.eastWall = this;
			this.westWall.UpdateConnection();
		}
		this.UpdateConnection();
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
	public void UpdateConnection()
	{
		int num = (this.northWall != null) ? 1 : 0;
		int num2 = (this.southWall != null) ? 1 : 0;
		int num3 = (this.westWall != null) ? 1 : 0;
		int num4 = (this.eastWall != null) ? 1 : 0;
		int num5 = num + num4 + num2 + num3;
		bool flag = false;
		Vector3 zero = Vector3.zero;
		switch (num5)
		{
		case 0:
			if (this.wallType != WallPiece.Type.NoConnections)
			{
				this.wallType = WallPiece.Type.NoConnections;
				flag = true;
			}
			break;
		case 1:
			if (this.wallType != WallPiece.Type.OneConnection)
			{
				this.wallType = WallPiece.Type.OneConnection;
				flag = true;
			}
			if (num == 1)
			{
				zero = new Vector3(0f, 0f, 90f);
			}
			else if (num3 == 1)
			{
				zero = new Vector3(0f, 0f, 180f);
			}
			else if (num2 == 1)
			{
				zero = new Vector3(0f, 0f, 270f);
			}
			break;
		case 2:
			if ((num == 1 && num2 == 1) || (num3 == 1 && num4 == 1))
			{
				if (this.wallType != WallPiece.Type.TwoConnectionsStraight)
				{
					this.wallType = WallPiece.Type.TwoConnectionsStraight;
					flag = true;
				}
				if (num == 1 && num2 == 1)
				{
					zero = new Vector3(0f, 0f, 90f);
				}
			}
			else
			{
				if (this.wallType != WallPiece.Type.TwoConnectionsCorner)
				{
					this.wallType = WallPiece.Type.TwoConnectionsCorner;
					flag = true;
				}
				if (num == 1 && num3 == 1)
				{
					zero = new Vector3(0f, 0f, 90f);
				}
				else if (num3 == 1 && num2 == 1)
				{
					zero = new Vector3(0f, 0f, 180f);
				}
				else if (num2 == 1 && num4 == 1)
				{
					zero = new Vector3(0f, 0f, 270f);
				}
			}
			break;
		case 3:
			if (this.wallType != WallPiece.Type.ThreeConnections)
			{
				this.wallType = WallPiece.Type.ThreeConnections;
				flag = true;
			}
			if (num == 1 && num3 == 1 && num2 == 1)
			{
				zero = new Vector3(0f, 0f, 90f);
			}
			else if (num3 == 1 && num2 == 1 && num4 == 1)
			{
				zero = new Vector3(0f, 0f, 180f);
			}
			else if (num2 == 1 && num4 == 1 && num == 1)
			{
				zero = new Vector3(0f, 0f, 270f);
			}
			break;
		case 4:
			if (this.wallType != WallPiece.Type.FourConnections)
			{
				this.wallType = WallPiece.Type.FourConnections;
				flag = true;
			}
			break;
		}
		if (flag)
		{
			if (base.Entity.Has_EComponent<FOW_Cloak>())
			{
				foreach (ModelObject.Layer layer in base.GetModel.Layers)
				{
					base.Entity.Cloak.RemoveLayer(layer.spriteRenderer);
				}
			}
			Accent accent = null;
			if (base.Entity.GetModel != null)
			{
				base.Entity.GetModel.DisconnectRenderer();
				accent = base.Entity.GetModel.GetAccent();
				if (base.Entity.GetModel.gameObject != null)
				{
					Object.Destroy(base.Entity.GetModel.gameObject);
				}
				else
				{
					Debug.Log("[WALL] Could not remove model object as it does not exist!");
				}
			}
			ModelObject model = Singleton<ModelConstructor>.Instance.GetModel(this.wallPiece.RequestWallPiece(this.wallType), base.Entity, Layers.SORTING_BUILDING_LAYER, false);
			model.transform.eulerAngles = zero;
			model.transform.localScale = Vector2.one;
			model.ConnectRenderer(base.Entity);
			if (accent != null)
			{
				base.Entity.GetModel.ApplyAccent(accent);
			}
			base.Entity.GetModel = model;
			if (base.Entity.Has_EComponent<FOW_Cloak>())
			{
				foreach (ModelObject.Layer layer2 in model.Layers)
				{
					base.Entity.Cloak.AddLayer(layer2.spriteRenderer);
				}
			}
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0002C520 File Offset: 0x0002A720
	public override void OnReset()
	{
		if (this.northWall != null && this.northWall.southWall == this)
		{
			this.northWall.southWall = null;
			this.northWall.UpdateConnection();
		}
		if (this.eastWall != null && this.eastWall.westWall == this)
		{
			this.eastWall.westWall = null;
			this.eastWall.UpdateConnection();
		}
		if (this.southWall != null && this.southWall.northWall == this)
		{
			this.southWall.northWall = null;
			this.southWall.UpdateConnection();
		}
		if (this.westWall != null && this.westWall.eastWall == this)
		{
			this.westWall.eastWall = null;
			this.westWall.UpdateConnection();
		}
		if (base.Entity.GetModel != null)
		{
			Object.Destroy(base.Entity.GetModel.gameObject);
		}
		this.wallType = WallPiece.Type.NoConnections;
		this.wallPiece = null;
	}

	// Token: 0x0400067F RID: 1663
	private WallData _wallData;

	// Token: 0x04000680 RID: 1664
	protected WallPiece.Type wallType;

	// Token: 0x04000681 RID: 1665
	protected WallPiece wallPiece;

	// Token: 0x04000682 RID: 1666
	public Wall northWall;

	// Token: 0x04000683 RID: 1667
	public Wall eastWall;

	// Token: 0x04000684 RID: 1668
	public Wall southWall;

	// Token: 0x04000685 RID: 1669
	public Wall westWall;
}
