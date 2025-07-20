using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vectorio.Generation
{
	// Token: 0x02000280 RID: 640
	[Serializable]
	public class BiomeData
	{
		// Token: 0x06001221 RID: 4641 RVA: 0x00052B9E File Offset: 0x00050D9E
		public bool UseNoiseSettings()
		{
			return this.noiseType == BiomeData.NoiseType.Island || this.noiseType == BiomeData.NoiseType.Directional;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00052BB4 File Offset: 0x00050DB4
		public bool UseFalloffSettings()
		{
			return this.noiseType == BiomeData.NoiseType.Island;
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00052BBF File Offset: 0x00050DBF
		public bool UseDirectionalSettings()
		{
			return this.noiseType == BiomeData.NoiseType.Directional;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00052BCA File Offset: 0x00050DCA
		public bool HideFalloffSettings()
		{
			return !this.useAdvancedFalloff;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00052BD5 File Offset: 0x00050DD5
		public void PreviewNoise()
		{
			this._seed = Random.Range(0, 999999);
			this.PreviewNoiseWithSeed();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00052BF0 File Offset: 0x00050DF0
		private void PreviewNoiseWithSeed()
		{
			BiomeData.NoiseType noiseType = this.noiseType;
			float[,] heightMap;
			if (noiseType != BiomeData.NoiseType.Island)
			{
				if (noiseType != BiomeData.NoiseType.Directional)
				{
					return;
				}
				this.direction = (CardinalDirection)Random.Range(0, 4);
				heightMap = Noise.GenerateDirectionalBiomeMap(this._seed, this, 500, this.direction);
			}
			else
			{
				heightMap = Noise.GenerateBiomeMap(this._seed, this);
			}
			this._noiseTexture = Noise.TextureFromHeightMap(heightMap);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00052C4D File Offset: 0x00050E4D
		public void PreviewFalloff()
		{
			this._seed = Random.Range(0, 999999);
			this.PreviewFalloffWithSeed();
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00052C68 File Offset: 0x00050E68
		private void PreviewFalloffWithSeed()
		{
			float[,] heightMap;
			if (!this.useAdvancedFalloff)
			{
				heightMap = Noise.GenerateFalloffMap(this.noiseSize, this.falloffCurve);
			}
			else
			{
				heightMap = Noise.GenerateFalloffMap(this._seed, this);
			}
			this._noiseTexture = Noise.TextureFromHeightMap(heightMap);
		}

		// Token: 0x04000FCA RID: 4042
		[SerializeField]
		private Texture2D _noiseTexture;

		// Token: 0x04000FCB RID: 4043
		public Color color;

		// Token: 0x04000FCC RID: 4044
		public int biomeSize;

		// Token: 0x04000FCD RID: 4045
		public int minSpawnDistance;

		// Token: 0x04000FCE RID: 4046
		public int maxSpawnDistance;

		// Token: 0x04000FCF RID: 4047
		[Range(0f, 1f)]
		public float threshold = 0.5f;

		// Token: 0x04000FD0 RID: 4048
		public CardinalDirection direction;

		// Token: 0x04000FD1 RID: 4049
		public List<WorldFeature> features = new List<WorldFeature>();

		// Token: 0x04000FD2 RID: 4050
		public BiomeData.NoiseType noiseType;

		// Token: 0x04000FD3 RID: 4051
		public float biomeCeiling = 0.8f;

		// Token: 0x04000FD4 RID: 4052
		public float biomeFloor = 0.2f;

		// Token: 0x04000FD5 RID: 4053
		[Range(0f, 1f)]
		public float biomeCoverage = 0.5f;

		// Token: 0x04000FD6 RID: 4054
		public int fadeLength;

		// Token: 0x04000FD7 RID: 4055
		public int maxOffset = 10;

		// Token: 0x04000FD8 RID: 4056
		public int noiseSize;

		// Token: 0x04000FD9 RID: 4057
		public float noiseScale;

		// Token: 0x04000FDA RID: 4058
		[Range(1f, 5f)]
		public int octaves = 3;

		// Token: 0x04000FDB RID: 4059
		[Range(0f, 1f)]
		public float persistence = 0.5f;

		// Token: 0x04000FDC RID: 4060
		[Range(1f, 10f)]
		public float lacunarity = 10f;

		// Token: 0x04000FDD RID: 4061
		public bool useAdvancedFalloff;

		// Token: 0x04000FDE RID: 4062
		public int falloffCurve = 4;

		// Token: 0x04000FDF RID: 4063
		public float falloffScale = 25f;

		// Token: 0x04000FE0 RID: 4064
		[Range(1f, 5f)]
		public int falloffOctaves = 2;

		// Token: 0x04000FE1 RID: 4065
		[Range(0f, 1f)]
		public float falloffPersistence = 0.5f;

		// Token: 0x04000FE2 RID: 4066
		[Range(1f, 10f)]
		public float falloffLacunarity = 10f;

		// Token: 0x04000FE3 RID: 4067
		[Range(0f, 1f)]
		public float falloffMixFactor = 0.5f;

		// Token: 0x04000FE4 RID: 4068
		private int _seed;

		// Token: 0x02000281 RID: 641
		public enum NoiseType
		{
			// Token: 0x04000FE6 RID: 4070
			None,
			// Token: 0x04000FE7 RID: 4071
			Island,
			// Token: 0x04000FE8 RID: 4072
			Directional
		}
	}
}
