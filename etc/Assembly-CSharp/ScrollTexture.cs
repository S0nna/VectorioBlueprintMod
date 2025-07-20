using System;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class ScrollTexture : MonoBehaviour
{
	// Token: 0x06000F45 RID: 3909 RVA: 0x00047B14 File Offset: 0x00045D14
	public void Awake()
	{
		this._renderTexture = base.GetComponent<Renderer>();
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00047B22 File Offset: 0x00045D22
	public void Update()
	{
		this._renderTexture.material.mainTextureOffset = new Vector2(this.xSpeed * Time.deltaTime, this.ySpeed * Time.deltaTime);
	}

	// Token: 0x04000C94 RID: 3220
	[SerializeField]
	protected float xSpeed = 0.5f;

	// Token: 0x04000C95 RID: 3221
	[SerializeField]
	protected float ySpeed = 0.5f;

	// Token: 0x04000C96 RID: 3222
	protected Renderer _renderTexture;
}
