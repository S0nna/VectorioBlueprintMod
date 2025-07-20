using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class MarkerHandler : Singleton<MarkerHandler>
{
	// Token: 0x06000E84 RID: 3716 RVA: 0x0004130C File Offset: 0x0003F50C
	public void CreateMarker(string id, Vector2 position, Vector2 markerScale, Vector2 iconScale, Sprite sprite, string title, string desc = "")
	{
		Vector3 position2 = new Vector3(position.x, position.y, 1f);
		Marker marker = Object.Instantiate<Marker>(this.markerPrefab, position2, Quaternion.identity);
		marker.transform.localScale = markerScale;
		marker.SetIcon(sprite, iconScale);
		marker.SetTitle(title);
		if (desc != "")
		{
			marker.SetDesc(desc);
		}
		this._markers.Add(id, marker);
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x00041389 File Offset: 0x0003F589
	public void DestroyMarker(string id)
	{
		if (this._markers.ContainsKey(id))
		{
			this._markers[id].Recycle();
			this._markers.Remove(id);
		}
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x000413B8 File Offset: 0x0003F5B8
	public Marker CreateAndReturnMarker(Vector2 position, Sprite sprite, string title, Vector2 size, string desc = "")
	{
		Vector3 position2 = new Vector3(position.x, position.y, 1f);
		Marker marker = Object.Instantiate<Marker>(this.markerPrefab, position2, Quaternion.identity);
		marker.transform.localScale = size;
		marker.SetIcon(sprite, Vector2.one);
		marker.SetTitle(title);
		if (desc != "")
		{
			marker.SetDesc(desc);
		}
		return marker;
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0004142B File Offset: 0x0003F62B
	public void DestroyMarker(Marker marker)
	{
		marker.Recycle();
	}

	// Token: 0x04000B62 RID: 2914
	public Marker markerPrefab;

	// Token: 0x04000B63 RID: 2915
	private Dictionary<string, Marker> _markers = new Dictionary<string, Marker>();
}
