using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vectorio.Serialization;

// Token: 0x020001ED RID: 493
public class SaveList : MonoBehaviour
{
	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000F35 RID: 3893 RVA: 0x000471FF File Offset: 0x000453FF
	public static GamemodeData Gamemode
	{
		get
		{
			return SaveList._gamemode;
		}
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00047208 File Offset: 0x00045408
	public void GenerateList(GamemodeData gamemode = null)
	{
		if (gamemode != null)
		{
			SaveList._gamemode = gamemode;
		}
		else if (SaveList._gamemode == null)
		{
			Debug.Log("[SAVE LIST] A gamemode must be specified before recalling generate list function!");
			return;
		}
		this.title.text = SaveList._gamemode.Name.ToUpper() + " SAVES";
		if (this._buttons != null)
		{
			foreach (SaveButton saveButton in this._buttons)
			{
				Object.Destroy(saveButton.gameObject);
			}
			this._buttons.Clear();
		}
		else
		{
			this._buttons = new List<SaveButton>();
		}
		string[] allSaves = FileOperations.GetAllSaves(SaveList._gamemode);
		for (int i = 0; i < allSaves.Length; i++)
		{
			SaveData saveData = DataProcessor.DeserializeDataFromFile<SaveData>(allSaves[i]);
			if (saveData != null)
			{
				SaveButton saveButton2 = Object.Instantiate<SaveButton>(this.buttonPrefab);
				saveButton2.transform.SetParent(this.buttonParent);
				saveButton2.GetComponent<RectTransform>().localScale = Vector2.one;
				saveButton2.Set(saveData);
				this._buttons.Add(saveButton2);
			}
			else
			{
				Debug.Log("[SAVE LIST] Could not load file at path " + allSaves[i]);
			}
		}
		this.newSaveButton.SetSiblingIndex(int.MaxValue);
	}

	// Token: 0x04000C6B RID: 3179
	private static GamemodeData _gamemode;

	// Token: 0x04000C6C RID: 3180
	public TextMeshProUGUI title;

	// Token: 0x04000C6D RID: 3181
	public SaveButton buttonPrefab;

	// Token: 0x04000C6E RID: 3182
	public Transform buttonParent;

	// Token: 0x04000C6F RID: 3183
	public Transform newSaveButton;

	// Token: 0x04000C70 RID: 3184
	protected List<SaveButton> _buttons;
}
