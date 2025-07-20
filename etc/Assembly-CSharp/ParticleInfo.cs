using System;
using UnityEngine;

// Token: 0x02000223 RID: 547
public struct ParticleInfo
{
	// Token: 0x06001013 RID: 4115 RVA: 0x0004BEF0 File Offset: 0x0004A0F0
	public void Setup(ParticleSystem particle, ParticleInfo.ColoringMode coloringMode, Material defaultMaterial)
	{
		this._useParticle = true;
		this._particle = particle;
		this._coloringMode = coloringMode;
		this.SetColoring(defaultMaterial);
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0004BF10 File Offset: 0x0004A110
	public void SetColoring(Material material)
	{
		ParticleInfo.ColoringMode coloringMode = this._coloringMode;
		if (coloringMode == ParticleInfo.ColoringMode.Material)
		{
			this._particleMaterial = material;
			return;
		}
		if (coloringMode != ParticleInfo.ColoringMode.Color)
		{
			return;
		}
		this._particleColor = material.color;
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0004BF41 File Offset: 0x0004A141
	public void Disable()
	{
		this._useParticle = false;
		this._particle = null;
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06001016 RID: 4118 RVA: 0x0004BF51 File Offset: 0x0004A151
	public bool UseParticle
	{
		get
		{
			return this._useParticle;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06001017 RID: 4119 RVA: 0x0004BF59 File Offset: 0x0004A159
	public ParticleSystem Particle
	{
		get
		{
			return this._particle;
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x0004BF61 File Offset: 0x0004A161
	public ParticleInfo.ColoringMode GetColoringMode
	{
		get
		{
			return this._coloringMode;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06001019 RID: 4121 RVA: 0x0004BF69 File Offset: 0x0004A169
	public Material Material
	{
		get
		{
			return this._particleMaterial;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x0004BF71 File Offset: 0x0004A171
	public Color Color
	{
		get
		{
			return this._particleColor;
		}
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0004BF79 File Offset: 0x0004A179
	public void SetParticlePosition(Vector2 position)
	{
		this._useParticlePosition = true;
		this._particlePosition = position;
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0004BF89 File Offset: 0x0004A189
	public Vector2 GetParticlePosition(Transform transform)
	{
		if (!this._useParticlePosition)
		{
			return transform.position;
		}
		return this._particlePosition;
	}

	// Token: 0x04000E22 RID: 3618
	private bool _useParticle;

	// Token: 0x04000E23 RID: 3619
	private ParticleSystem _particle;

	// Token: 0x04000E24 RID: 3620
	private Material _particleMaterial;

	// Token: 0x04000E25 RID: 3621
	private Color _particleColor;

	// Token: 0x04000E26 RID: 3622
	private ParticleInfo.ColoringMode _coloringMode;

	// Token: 0x04000E27 RID: 3623
	private bool _useParticlePosition;

	// Token: 0x04000E28 RID: 3624
	private Vector2 _particlePosition;

	// Token: 0x02000224 RID: 548
	public enum ColoringMode
	{
		// Token: 0x04000E2A RID: 3626
		None,
		// Token: 0x04000E2B RID: 3627
		Material,
		// Token: 0x04000E2C RID: 3628
		Color
	}
}
