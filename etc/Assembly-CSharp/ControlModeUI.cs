using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001A9 RID: 425
public class ControlModeUI : MonoBehaviour
{
	// Token: 0x06000DD9 RID: 3545 RVA: 0x0003D708 File Offset: 0x0003B908
	public void Start()
	{
		Singleton<Events>.Instance.onBuildModeChanged.AddListener(new UnityAction<Hologram.BuildMode>(this.OnChangeMode));
		this._modeDict = new Dictionary<Hologram.BuildMode, ControlModeUI.ModeButton>();
		foreach (ControlModeUI.ModeButton modeButton in this.modes)
		{
			this._modeDict.Add(modeButton.mode, modeButton);
		}
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0003D78C File Offset: 0x0003B98C
	public void SetMode(int mode)
	{
		Singleton<Events>.Instance.onChangeBuildMode.Invoke((Hologram.BuildMode)mode);
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x0003D7A0 File Offset: 0x0003B9A0
	public void OnChangeMode(Hologram.BuildMode mode)
	{
		foreach (ControlModeUI.ModeButton modeButton in this.modes)
		{
			modeButton.Toggle(this, false);
		}
		if (this._modeDict.ContainsKey(mode))
		{
			this._modeDict[mode].Toggle(this, true);
		}
	}

	// Token: 0x04000A1C RID: 2588
	public List<ControlModeUI.ModeButton> modes;

	// Token: 0x04000A1D RID: 2589
	public float enabledModeTextSize;

	// Token: 0x04000A1E RID: 2590
	public float disabledModeTextSize;

	// Token: 0x04000A1F RID: 2591
	public Image border;

	// Token: 0x04000A20 RID: 2592
	public Image dividerOne;

	// Token: 0x04000A21 RID: 2593
	public Image dividerTwo;

	// Token: 0x04000A22 RID: 2594
	public Color enabledModeTextColor;

	// Token: 0x04000A23 RID: 2595
	public Color disabledModeTextColor;

	// Token: 0x04000A24 RID: 2596
	private Dictionary<Hologram.BuildMode, ControlModeUI.ModeButton> _modeDict;

	// Token: 0x020001AA RID: 426
	[Serializable]
	public class ModeButton
	{
		// Token: 0x06000DDD RID: 3549 RVA: 0x0003D814 File Offset: 0x0003BA14
		public void Toggle(ControlModeUI ui, bool toggle)
		{
			if (toggle)
			{
				this.modeText.fontSize = ui.enabledModeTextSize;
				this.modeText.color = ui.enabledModeTextColor;
				this.modeBackground.color = this.enabledColor;
				if (this.modeSound != null)
				{
					Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.modeSound);
					return;
				}
			}
			else
			{
				this.modeText.fontSize = ui.disabledModeTextSize;
				this.modeText.color = ui.disabledModeTextColor;
				this.modeBackground.color = this.disabledColor;
			}
		}

		// Token: 0x04000A25 RID: 2597
		public Hologram.BuildMode mode;

		// Token: 0x04000A26 RID: 2598
		public TextMeshProUGUI modeText;

		// Token: 0x04000A27 RID: 2599
		public Image modeBackground;

		// Token: 0x04000A28 RID: 2600
		public AudioClip modeSound;

		// Token: 0x04000A29 RID: 2601
		public Color enabledColor;

		// Token: 0x04000A2A RID: 2602
		public Color disabledColor;
	}
}
