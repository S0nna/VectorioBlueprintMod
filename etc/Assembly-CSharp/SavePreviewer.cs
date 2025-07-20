using System;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vectorio.Formatting;
using Vectorio.Serialization;

// Token: 0x020001EE RID: 494
public class SavePreviewer : Singleton<SavePreviewer>
{
	// Token: 0x06000F38 RID: 3896 RVA: 0x00047360 File Offset: 0x00045560
	public void PreviewSave(SaveData save)
	{
		this.saveInformation.SetActive(true);
		this.noSaveSelected.SetActive(false);
		this._save = save;
		this.title.text = save.Name.ToUpper();
		if (save.GamemodeData != null)
		{
			if (save.GamemodeData.PresetID != null && save.GamemodeData.PresetID != "")
			{
				PresetData presetData = Library.RequestData<PresetData>(save.GamemodeData.PresetID);
				if (presetData == null)
				{
					this.preset.text = "<b>Preset:</b> Custom";
					Debug.Log("[SAVE PREVIEW] Preset data does not exist. This could or could not be a mistake.");
				}
				else
				{
					this.preset.text = "<b>Preset:</b> " + presetData.Name;
				}
			}
			else
			{
				this.preset.text = "<b>Preset:</b> Custom";
			}
			this._gamemode = Library.RequestData<GamemodeData>(save.GamemodeData.ID);
			if (this._gamemode == null)
			{
				Debug.Log("[SAVE PREVIEW] Gamemode on file is invalid or does not exist!");
			}
		}
		else
		{
			Debug.Log("[SAVE PREVIEW] Cannot display information for " + save.Name + " as gamemode data is null or invalid!");
			this.preset.text = "<b>Preset:</b> Unknown";
			this._gamemode = null;
		}
		SaveData.Region region = save.GetRegion(save.ActiveRegion);
		this.seed.text = "<b>Seed:</b> " + this._save.Seed.ToString();
		this.progress.text = "<b>Total Progress:</b> " + this._save.GetProgressText(null);
		TMP_Text tmp_Text = this.activeRegion;
		string str = "<b>Active Region:</b> ";
		RegionData regionData = Library.RequestData<RegionData>(region.ID);
		tmp_Text.text = str + ((regionData != null) ? regionData.Name : null);
		this.timePlayed.text = "<b>Time Played:</b> " + Formatter.FormatPlayTime(this._save.WorldTime);
		this.version.text = "<b>Version Created:</b> " + this._save.Version;
		if (region.preview != null)
		{
			this.noPreviewAvailable.SetActive(false);
			this.camera.gameObject.SetActive(true);
			this.camera.targetTexture = this.saveTexture;
			for (int i = 0; i < region.preview.GetLength(0); i++)
			{
				for (int j = 0; j < region.preview.GetLength(1); j++)
				{
					Color color;
					if (ColorUtility.TryParseHtmlString(region.preview[i, j], out color))
					{
						Vector3Int position = new Vector3Int(i + this.offset, j, 0);
						this.tilemap.SetTile(position, this.tileBase);
						this.tilemap.SetTileFlags(position, TileFlags.None);
						this.tilemap.SetColor(position, color);
					}
					else
					{
						Debug.Log(string.Concat(new string[]
						{
							"[SAVE] Invalid color hex provided (",
							i.ToString(),
							", ",
							j.ToString(),
							")"
						}));
					}
				}
			}
			this.camera.transform.position = new Vector2((float)region.preview.GetLength(0) * 5f / 2f + (float)this.offset * 5f, (float)region.preview.GetLength(0) * 5f / 2f);
			this.camera.orthographicSize = (float)region.preview.GetLength(0) * 1.4f;
			return;
		}
		this.camera.gameObject.SetActive(false);
		this.noPreviewAvailable.SetActive(true);
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00047700 File Offset: 0x00045900
	public void EditSave()
	{
		this.worldPanel.EditSave(this._save);
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00047713 File Offset: 0x00045913
	public void ExportSave()
	{
		FileOperations.OpenFile(this._gamemode, this._save.FileName);
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0004772C File Offset: 0x0004592C
	public void OpenDeleteSave()
	{
		if (this._gamemode == null)
		{
			Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.invalidSound);
			return;
		}
		this.panelGroup.alpha = 0f;
		this.panelGroup.interactable = false;
		this.panelGroup.blocksRaycasts = false;
		this.deleteWindow.SetActive(true);
		this.fileToDelete.text = this._save.Name.ToUpper();
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x000477A8 File Offset: 0x000459A8
	public void ConfirmDelete()
	{
		try
		{
			FileOperations.DeleteSave(this._gamemode, this._save.FileName);
		}
		catch
		{
			Debug.Log("[SAVE PREVIEW] Could not delete save!");
		}
		this.saveList.GenerateList(null);
		this.camera.gameObject.SetActive(false);
		this.noPreviewAvailable.SetActive(true);
		this.noSaveSelected.SetActive(true);
		this.CloseDeleteSave();
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00047828 File Offset: 0x00045A28
	public void CloseDeleteSave()
	{
		this.panelGroup.alpha = 1f;
		this.panelGroup.interactable = true;
		this.panelGroup.blocksRaycasts = true;
		this.deleteWindow.SetActive(false);
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x00047860 File Offset: 0x00045A60
	public void LoadSave()
	{
		Singleton<Events>.Instance.onDisableActionMap.Invoke();
		Singleton<EntityManager>.Instance.ClearAllEntities();
		SaveSystem.SaveData = this._save;
		if (!Authenticator.UserAuthenticated)
		{
			Singleton<Lobby>.Instance.CreateOfflineLobby();
			return;
		}
		if (Menu.IsOnline())
		{
			Singleton<Lobby>.Instance.CreateSteamLobby(ELobbyType.k_ELobbyTypePublic, 20);
			return;
		}
		Singleton<Lobby>.Instance.CreateLocalLobby(2);
	}

	// Token: 0x04000C71 RID: 3185
	[SerializeField]
	public List<SavePreviewer.VersionCheck> versionChecks;

	// Token: 0x04000C72 RID: 3186
	[TextArea]
	public string defaultVersionMismatchDisplay;

	// Token: 0x04000C73 RID: 3187
	public NewWorldPanel worldPanel;

	// Token: 0x04000C74 RID: 3188
	public SaveList saveList;

	// Token: 0x04000C75 RID: 3189
	public Camera camera;

	// Token: 0x04000C76 RID: 3190
	public Tilemap tilemap;

	// Token: 0x04000C77 RID: 3191
	public TileBase tileBase;

	// Token: 0x04000C78 RID: 3192
	public CanvasGroup panelGroup;

	// Token: 0x04000C79 RID: 3193
	public GameObject saveInformation;

	// Token: 0x04000C7A RID: 3194
	public GameObject noSaveSelected;

	// Token: 0x04000C7B RID: 3195
	public GameObject noPreviewAvailable;

	// Token: 0x04000C7C RID: 3196
	public AudioClip invalidSound;

	// Token: 0x04000C7D RID: 3197
	public RenderTexture saveTexture;

	// Token: 0x04000C7E RID: 3198
	public TextMeshProUGUI title;

	// Token: 0x04000C7F RID: 3199
	public TextMeshProUGUI preset;

	// Token: 0x04000C80 RID: 3200
	public TextMeshProUGUI seed;

	// Token: 0x04000C81 RID: 3201
	public TextMeshProUGUI progress;

	// Token: 0x04000C82 RID: 3202
	public TextMeshProUGUI activeRegion;

	// Token: 0x04000C83 RID: 3203
	public TextMeshProUGUI timePlayed;

	// Token: 0x04000C84 RID: 3204
	public TextMeshProUGUI version;

	// Token: 0x04000C85 RID: 3205
	public int offset;

	// Token: 0x04000C86 RID: 3206
	public int xSize = 250;

	// Token: 0x04000C87 RID: 3207
	public int ySize = 250;

	// Token: 0x04000C88 RID: 3208
	protected SaveData _save;

	// Token: 0x04000C89 RID: 3209
	protected GamemodeData _gamemode;

	// Token: 0x04000C8A RID: 3210
	public GameObject deleteWindow;

	// Token: 0x04000C8B RID: 3211
	public TextMeshProUGUI fileToDelete;

	// Token: 0x020001EF RID: 495
	[Serializable]
	public struct VersionCheck
	{
		// Token: 0x04000C8C RID: 3212
		public int majorNumber;

		// Token: 0x04000C8D RID: 3213
		public bool checkMinor;

		// Token: 0x04000C8E RID: 3214
		public int minorNumber;

		// Token: 0x04000C8F RID: 3215
		public bool checkPatch;

		// Token: 0x04000C90 RID: 3216
		public int patchNumber;

		// Token: 0x04000C91 RID: 3217
		[TextArea]
		public string display;
	}
}
