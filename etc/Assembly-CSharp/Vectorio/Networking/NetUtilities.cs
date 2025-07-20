using System;
using Steamworks;
using UnityEngine;

namespace Vectorio.Networking
{
	// Token: 0x02000275 RID: 629
	public class NetUtilities
	{
		// Token: 0x06001211 RID: 4625 RVA: 0x000526BC File Offset: 0x000508BC
		public static Texture2D GetSteamImageAsTexture2D(int iImage)
		{
			Texture2D texture2D = null;
			uint num;
			uint num2;
			if (SteamUtils.GetImageSize(iImage, out num, out num2))
			{
				byte[] array = new byte[num * num2 * 4U];
				if (SteamUtils.GetImageRGBA(iImage, array, (int)(num * num2 * 4U)))
				{
					texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false, true);
					texture2D.LoadRawTextureData(array);
					texture2D.Apply();
				}
			}
			return texture2D;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0005270C File Offset: 0x0005090C
		public static Sprite GetSteamImageAsSprite(int iImage)
		{
			Texture2D steamImageAsTexture2D = NetUtilities.GetSteamImageAsTexture2D(iImage);
			return Sprite.Create(steamImageAsTexture2D, new Rect(0f, (float)steamImageAsTexture2D.height, (float)steamImageAsTexture2D.width, (float)(-(float)steamImageAsTexture2D.height)), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x04000F81 RID: 3969
		public const uint GAME_ID = 2082350U;
	}
}
