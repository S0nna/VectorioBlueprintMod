using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Vectorio.Utilities
{
	// Token: 0x02000287 RID: 647
	public class Utilities
	{
		// Token: 0x0600123B RID: 4667 RVA: 0x00053A8E File Offset: 0x00051C8E
		public static string UnescapeJson(string json)
		{
			return json.Substring(1, json.Length - 2).Replace("\\\"", "\"");
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00053AAE File Offset: 0x00051CAE
		public static string GetVariableID(string id, Type component)
		{
			return "component".ToLower() + "_" + id.ToLower();
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00053ACA File Offset: 0x00051CCA
		public static string AddSpaces(string input)
		{
			return Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00053ADC File Offset: 0x00051CDC
		public static Vector2 CalculateBuildingPosition(Vector2 position, int width, int height)
		{
			return new Vector2(5f * Mathf.Round(position.x / 5f) + ((width % 2 == 0) ? -2.5f : 0f), 5f * Mathf.Round(position.y / 5f) + ((height % 2 == 0) ? 2.5f : 0f));
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00053B40 File Offset: 0x00051D40
		public static void CalculateBuildingCells(ref List<Vector2Int> cells, Vector2 position, int width, int height)
		{
			Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(position);
			int num = width / 2;
			int num2 = height / 2;
			int num3 = vector2Int.x - num;
			int num4 = vector2Int.y - num2;
			int num5 = vector2Int.x + num;
			int num6 = vector2Int.y + num2;
			if (width % 2 == 0)
			{
				num3++;
			}
			if (height % 2 == 0)
			{
				num4++;
			}
			for (int i = num4; i <= num6; i++)
			{
				for (int j = num3; j <= num5; j++)
				{
					cells.Add(new Vector2Int(j, i));
				}
			}
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00053BD0 File Offset: 0x00051DD0
		public static Vector2Int ConvertWorldPositionToCell(Vector2 position)
		{
			int x = Mathf.FloorToInt((position.x + 2f) / 5f);
			int y = Mathf.FloorToInt((position.y + 2f) / 5f);
			return new Vector2Int(x, y);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00053C12 File Offset: 0x00051E12
		public static Vector2 ConvertCellPositionToWorld(Vector2Int cellPosition)
		{
			return new Vector2((float)(cellPosition.x * 5), (float)(cellPosition.y * 5));
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00053C30 File Offset: 0x00051E30
		public static Vector2Int ConvertHashPositionToCell(Vector2Int hashPosition, int cellSize)
		{
			int x = hashPosition.x * cellSize + cellSize / 2;
			int y = hashPosition.y * cellSize + cellSize / 2;
			return new Vector2Int(x, y);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00053C60 File Offset: 0x00051E60
		public static Tuple<Vector2Int, Vector2Int> CalculateTileRange(Transform parent, int range, int buildingHeight, int buildingWidth)
		{
			if (buildingHeight % 2 == 0 && buildingWidth % 2 == 0)
			{
				Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(new Vector2(parent.position.x - 2.5f, parent.position.y - 2.5f));
				Vector2Int vector2Int2 = Utilities.ConvertWorldPositionToCell(new Vector2(parent.position.x + 2.5f, parent.position.y + 2.5f));
				if (range >= 1)
				{
					range--;
				}
				return new Tuple<Vector2Int, Vector2Int>(new Vector2Int(vector2Int.x - range, vector2Int.y - range), new Vector2Int(vector2Int2.x + range, vector2Int2.y + range));
			}
			Vector2Int vector2Int3 = Utilities.ConvertWorldPositionToCell(parent.position);
			return new Tuple<Vector2Int, Vector2Int>(new Vector2Int(vector2Int3.x - range, vector2Int3.y - range), new Vector2Int(vector2Int3.x + range, vector2Int3.y + range));
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00053D58 File Offset: 0x00051F58
		public static List<Vector2Int> CalculateTileRange(Vector2 position, int range)
		{
			Vector2Int vector2Int = Utilities.ConvertWorldPositionToCell(position);
			List<Vector2Int> list = new List<Vector2Int>();
			for (int i = vector2Int.y - range; i <= vector2Int.y + range; i++)
			{
				for (int j = vector2Int.x - range; j <= vector2Int.x + range; j++)
				{
					list.Add(new Vector2Int(j, i));
				}
			}
			return list;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00053DB8 File Offset: 0x00051FB8
		public static List<Vector2Int> CalculateBuildingTiles(Vector2 position, BuildingData data, TileLayer layer)
		{
			Vector2Int item = Utilities.ConvertWorldPositionToCell(position);
			List<Vector2Int> list = new List<Vector2Int>();
			if ((data.Width == 0 && data.Height == 0) || (data.Width == 1 && data.Height == 1))
			{
				list.Add(item);
				return list;
			}
			float num = ((float)data.Width % 2f == 0f) ? (-2.5f - (float)data.Width / 2f) : (0f - (float)data.Width / 2f);
			float num2 = ((float)data.Height % 2f == 0f) ? (2.5f - (float)data.Height / 2f) : (0f - (float)data.Height / 2f);
			for (int i = 0; i < data.Width; i++)
			{
				for (int j = 0; j < data.Height; j++)
				{
					Vector2 v = new Vector2((float)item.x + num + (float)j, (float)item.y + num2 + (float)i);
					list.Add(Vector2Int.RoundToInt(v));
				}
			}
			return list;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00053ED4 File Offset: 0x000520D4
		public static bool CheckInheritance(Type child, Type parent)
		{
			parent = Utilities.ResolveGenericTypeDefinition(parent);
			Type type = child.IsGenericType ? child.GetGenericTypeDefinition() : child;
			while (type != typeof(object))
			{
				if (parent == type || Utilities.HasAnyInterfaces(parent, type))
				{
					return true;
				}
				type = ((type.BaseType != null && type.BaseType.IsGenericType) ? type.BaseType.GetGenericTypeDefinition() : type.BaseType);
				if (type == null)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00053F60 File Offset: 0x00052160
		private static bool HasAnyInterfaces(Type parent, Type child)
		{
			return child.GetInterfaces().Any((Type childInterface) => (childInterface.IsGenericType ? childInterface.GetGenericTypeDefinition() : childInterface) == parent);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00053F94 File Offset: 0x00052194
		private static Type ResolveGenericTypeDefinition(Type parent)
		{
			bool flag = true;
			if (parent.IsGenericType && parent.GetGenericTypeDefinition() != parent)
			{
				flag = false;
			}
			if (parent.IsGenericType && flag)
			{
				parent = parent.GetGenericTypeDefinition();
			}
			return parent;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00053FCE File Offset: 0x000521CE
		public static List<EntityData> Sort(EntityData[] entities)
		{
			Utilities.QuickSortEntities(entities, 0, entities.Length - 1);
			return entities.ToList<EntityData>();
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00053FE4 File Offset: 0x000521E4
		private static void QuickSortEntities(EntityData[] entities, int left, int right)
		{
			if (left < right)
			{
				int num = Utilities.Partition(entities, left, right);
				if (num > 1)
				{
					Utilities.QuickSortEntities(entities, left, num - 1);
				}
				if (num + 1 < right)
				{
					Utilities.QuickSortEntities(entities, num + 1, right);
				}
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0005401C File Offset: 0x0005221C
		private static int Partition(EntityData[] entities, int left, int right)
		{
			int inventoryIndex = entities[left].inventoryIndex;
			for (;;)
			{
				if (entities[left].inventoryIndex >= inventoryIndex)
				{
					while (entities[right].inventoryIndex > inventoryIndex)
					{
						right--;
					}
					if (left >= right)
					{
						return right;
					}
					if (entities[left].inventoryIndex == entities[right].inventoryIndex)
					{
						break;
					}
					EntityData entityData = entities[left];
					entities[left] = entities[right];
					entities[right] = entityData;
				}
				else
				{
					left++;
				}
			}
			return right;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00054080 File Offset: 0x00052280
		public static Utilities.CompressedData PackData(byte[] data)
		{
			return new Utilities.CompressedData
			{
				Data = data
			};
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000540A0 File Offset: 0x000522A0
		public static int ColorToInt(Color color)
		{
			return (int)(color.r * 255f) << 24 | (int)(color.g * 255f) << 16 | (int)(color.b * 255f) << 8 | (int)(color.a * 255f);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000540EC File Offset: 0x000522EC
		public static Color IntToColor(int colorInt)
		{
			float r = (float)(colorInt >> 24 & 255) / 255f;
			float g = (float)(colorInt >> 16 & 255) / 255f;
			float b = (float)(colorInt >> 8 & 255) / 255f;
			float a = (float)(colorInt & 255) / 255f;
			return new Color(r, g, b, a);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00054144 File Offset: 0x00052344
		public static Texture2D MergeSpriteLayers(SpriteLayer[] layers)
		{
			int num = 0;
			for (int i = 0; i < layers.Length; i++)
			{
				Sprite sprite = layers[i].overrideUI ? layers[i].overrideSprite : layers[i].sprite;
				if (sprite.texture.width > num)
				{
					num = sprite.texture.width;
				}
				else if (sprite.texture.height > num)
				{
					num = sprite.texture.height;
				}
			}
			Texture2D texture2D = new Texture2D(num, num);
			int num2 = num / 2;
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					texture2D.SetPixel(j, k, new Color(1f, 1f, 1f, 0f));
				}
			}
			for (int l = 0; l < layers.Length; l++)
			{
				if (!layers[l].hideOnUI)
				{
					Texture2D texture2D2 = Utilities.DuplicateTexture((layers[l].overrideUI ? layers[l].overrideSprite : layers[l].sprite).texture);
					int num3 = num2 - texture2D2.width / 2;
					int num4 = num2 - texture2D2.height / 2;
					for (int m = 0; m < texture2D2.width; m++)
					{
						for (int n = 0; n < texture2D2.height; n++)
						{
							int x = m + num3;
							int y = n + num4;
							if (texture2D2.GetPixel(m, n).a != 0f)
							{
								Color pixel = texture2D2.GetPixel(m, n);
								texture2D.SetPixel(x, y, new Color(pixel.r * layers[l].color.r, pixel.g * layers[l].color.g, pixel.b * layers[l].color.b, 1f));
							}
						}
					}
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0005433C File Offset: 0x0005253C
		private static Texture2D DuplicateTexture(Texture2D source)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
			Graphics.Blit(source, temporary);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(source.width, source.height);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		// Token: 0x04001000 RID: 4096
		public const int GRID_SIZE = 5;

		// Token: 0x02000288 RID: 648
		public struct CompressedData
		{
			// Token: 0x04001001 RID: 4097
			public byte[] Data;
		}
	}
}
