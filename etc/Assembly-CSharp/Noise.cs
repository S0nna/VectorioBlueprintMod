using System;
using UnityEngine;
using Vectorio.Generation;

// Token: 0x02000222 RID: 546
public static class Noise
{
	// Token: 0x06001009 RID: 4105 RVA: 0x0004B624 File Offset: 0x00049824
	public static float[,] GenerateBiomeMap(int seed, BiomeData biome)
	{
		int noiseSize = biome.noiseSize;
		float noiseScale = biome.noiseScale;
		int falloffCurve = biome.falloffCurve;
		int octaves = biome.octaves;
		float persistence = biome.persistence;
		float lacunarity = biome.lacunarity;
		float[,] array = Noise.GeneratePerlinNoiseMap(seed, noiseSize, noiseScale, octaves, persistence, lacunarity);
		float[,] array2;
		if (biome.useAdvancedFalloff)
		{
			array2 = Noise.GenerateFalloffMap(seed, biome);
		}
		else
		{
			array2 = Noise.GenerateFalloffMap(noiseSize, falloffCurve);
		}
		float[,] array3 = new float[noiseSize, noiseSize];
		for (int i = 0; i < noiseSize; i++)
		{
			for (int j = 0; j < noiseSize; j++)
			{
				float num = Mathf.Clamp01(array[j, i] - array2[j, i]);
				if (num < biome.biomeFloor)
				{
					num = 0f;
				}
				else if (num > biome.biomeCeiling)
				{
					num = 1f;
				}
				else
				{
					num = (num - biome.biomeFloor) / (biome.biomeCeiling - biome.biomeFloor);
				}
				array3[j, i] = num;
			}
		}
		return Noise.AdjustAndNormalizeMap(array3, noiseSize, noiseSize);
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0004B728 File Offset: 0x00049928
	public static float[,] GenerateDirectionalBiomeMap(int seed, BiomeData biome, int mapSize, CardinalDirection direction)
	{
		int num = mapSize;
		int num2 = mapSize;
		int num3 = Mathf.Max(0, biome.maxOffset - biome.fadeLength);
		if (direction == CardinalDirection.East || direction == CardinalDirection.West)
		{
			num = Mathf.CeilToInt((float)mapSize * biome.biomeCoverage) + num3;
		}
		else
		{
			num2 = Mathf.CeilToInt((float)mapSize * biome.biomeCoverage) + num3;
		}
		float[,] array = new float[num, num2];
		float[] array2 = Noise.GeneratePerlinNoise(seed, mapSize, biome.noiseScale);
		int num4 = 0;
		switch (direction)
		{
		case CardinalDirection.North:
			num4 = biome.fadeLength + num3;
			break;
		case CardinalDirection.East:
			num4 = biome.fadeLength + num3;
			break;
		case CardinalDirection.South:
			num4 = num2 - biome.fadeLength - num3;
			break;
		case CardinalDirection.West:
			num4 = num - biome.fadeLength - num3;
			break;
		}
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				switch (direction)
				{
				case CardinalDirection.North:
					if (j < num4)
					{
						int num5 = (int)((float)biome.maxOffset * array2[i]);
						int num6 = j + num5;
						if (num6 < num2)
						{
							array[i, num6] = (float)j / (float)biome.fadeLength;
						}
					}
					else if (array[i, j] == 0f)
					{
						array[i, j] = 1f;
					}
					break;
				case CardinalDirection.East:
					if (i < num4)
					{
						int num5 = (int)((float)biome.maxOffset * array2[j]);
						int num7 = i + num5;
						if (num7 < num)
						{
							array[num7, j] = (float)i / (float)biome.fadeLength;
						}
					}
					else if (array[i, j] == 0f)
					{
						array[i, j] = 1f;
					}
					break;
				case CardinalDirection.South:
					if (j > num4)
					{
						int num5 = (int)((float)biome.maxOffset * array2[i]);
						int num8 = j - num5;
						if (num8 >= 0)
						{
							array[i, num8] = (float)(num2 - j) / (float)biome.fadeLength;
						}
					}
					else if (array[i, j] == 0f)
					{
						array[i, j] = 1f;
					}
					break;
				case CardinalDirection.West:
					if (i > num4)
					{
						int num5 = (int)((float)biome.maxOffset * array2[j]);
						int num9 = i - num5;
						if (num9 >= 0)
						{
							array[num9, j] = (float)(num - i) / (float)biome.fadeLength;
						}
					}
					else if (array[i, j] == 0f)
					{
						array[i, j] = 1f;
					}
					break;
				}
			}
		}
		return array;
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0004B9B0 File Offset: 0x00049BB0
	public static float[,] GenerateFalloffMap(int size, int curve)
	{
		float[,] array = new float[size, size];
		int num = size / 2;
		int num2 = size / 2;
		float num3 = Mathf.Sqrt((float)(num * num + num2 * num2));
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				float num4 = (float)(j - num);
				float num5 = (float)(i - num2);
				float num6 = Mathf.Pow(Noise.SmoothEdgeTransition(Mathf.Clamp01(Mathf.Sqrt(num4 * num4 + num5 * num5) / num3)), (float)curve);
				array[j, i] = num6;
			}
		}
		return array;
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0004BA38 File Offset: 0x00049C38
	public static float[,] GenerateFalloffMap(int seed, BiomeData biomeData)
	{
		int noiseSize = biomeData.noiseSize;
		int falloffCurve = biomeData.falloffCurve;
		float falloffScale = biomeData.falloffScale;
		int falloffOctaves = biomeData.falloffOctaves;
		float falloffPersistence = biomeData.falloffPersistence;
		float falloffLacunarity = biomeData.falloffLacunarity;
		float falloffMixFactor = biomeData.falloffMixFactor;
		float[,] array = new float[noiseSize, noiseSize];
		float[,] array2 = Noise.GeneratePerlinNoiseMap(seed, noiseSize, falloffScale, falloffOctaves, falloffPersistence, falloffLacunarity);
		int num = noiseSize / 2;
		int num2 = noiseSize / 2;
		float num3 = Mathf.Sqrt((float)(num * num + num2 * num2));
		for (int i = 0; i < noiseSize; i++)
		{
			for (int j = 0; j < noiseSize; j++)
			{
				float num4 = (float)(j - num);
				float num5 = (float)(i - num2);
				float num6 = Mathf.Pow(Noise.SmoothEdgeTransition(Mathf.Clamp01(Mathf.Sqrt(num4 * num4 + num5 * num5) / num3)), (float)falloffCurve);
				float b = array2[j, i];
				num6 = Mathf.Lerp(num6, b, falloffMixFactor);
				array[j, i] = num6;
			}
		}
		return array;
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0004BB28 File Offset: 0x00049D28
	private static float[] GeneratePerlinNoise(int seed, int length, float scale)
	{
		float num = (float)new Random(seed).Next(0, 100000);
		float[] array = new float[length];
		for (int i = 0; i < length; i++)
		{
			float x = (float)i / scale + num;
			array[i] = Mathf.PerlinNoise(x, 0f);
		}
		return array;
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0004BB74 File Offset: 0x00049D74
	private static float[,] GeneratePerlinNoiseMap(int seed, int width, int height, float scale)
	{
		float[,] array = new float[width, height];
		Random.InitState(seed);
		Vector2 vector = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				float x = (float)j / scale + vector.x;
				float y = (float)i / scale + vector.y;
				float num = Mathf.PerlinNoise(x, y);
				array[j, i] = num;
			}
		}
		return array;
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0004BBF8 File Offset: 0x00049DF8
	private static float[,] GeneratePerlinNoiseMap(int seed, int size, float scale, int octaves, float persistence, float lacunarity)
	{
		float[,] array = new float[size, size];
		Random.InitState(seed);
		Vector2[] array2 = new Vector2[octaves];
		for (int i = 0; i < octaves; i++)
		{
			float x = Random.Range(0f, 9999f);
			float y = Random.Range(0f, 9999f);
			array2[i] = new Vector2(x, y);
		}
		if (scale <= 0f)
		{
			scale = 0.0001f;
		}
		float num = 0f;
		for (int j = 0; j < size; j++)
		{
			for (int k = 0; k < size; k++)
			{
				float num2 = 1f;
				float num3 = 1f;
				float num4 = 0f;
				for (int l = 0; l < octaves; l++)
				{
					float x2 = ((float)k + array2[l].x) / scale * num3;
					float y2 = ((float)j + array2[l].y) / scale * num3;
					float num5 = Mathf.PerlinNoise(x2, y2) * 2f - 1f;
					num4 += num5 * num2;
					num2 *= persistence;
					num3 *= lacunarity;
				}
				if (num4 > num)
				{
					num = num4;
				}
				array[k, j] = num4;
			}
		}
		for (int m = 0; m < size; m++)
		{
			for (int n = 0; n < size; n++)
			{
				float value = (array[n, m] + 1f) / (num / 0.9f);
				array[n, m] = Mathf.Clamp(value, 0f, 1f);
			}
		}
		return array;
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0004BD91 File Offset: 0x00049F91
	private static float SmoothEdgeTransition(float value)
	{
		return 6f * Mathf.Pow(value, 5f) - 15f * Mathf.Pow(value, 4f) + 10f * Mathf.Pow(value, 3f);
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0004BDC8 File Offset: 0x00049FC8
	private static float[,] AdjustAndNormalizeMap(float[,] map, int width, int height)
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (map[j, i] < num)
				{
					num = map[j, i];
				}
				if (map[j, i] > num2)
				{
					num2 = map[j, i];
				}
			}
		}
		for (int k = 0; k < height; k++)
		{
			for (int l = 0; l < width; l++)
			{
				map[l, k] = (map[l, k] - num) / (num2 - num);
			}
		}
		return map;
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0004BE5C File Offset: 0x0004A05C
	public static Texture2D TextureFromHeightMap(float[,] heightMap)
	{
		int length = heightMap.GetLength(0);
		int length2 = heightMap.GetLength(1);
		Texture2D texture2D = new Texture2D(length, length2)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};
		Color[] array = new Color[length * length2];
		for (int i = 0; i < length2; i++)
		{
			for (int j = 0; j < length; j++)
			{
				float t = heightMap[j, i];
				array[i * length + j] = Color.Lerp(Color.black, Color.white, t);
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}
}
