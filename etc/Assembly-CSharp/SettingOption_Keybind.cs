using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000193 RID: 403
public class SettingOption_Keybind : SettingOption_Base
{
	// Token: 0x06000D77 RID: 3447 RVA: 0x0003B06B File Offset: 0x0003926B
	public void Setup(InputAction action, int index, Color color)
	{
		this.keybindButton.Setup(action, index);
		base.SetInfo(action.name, color);
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x00003212 File Offset: 0x00001412
	public void Reset()
	{
	}

	// Token: 0x0400098D RID: 2445
	public KeybindButton keybindButton;
}
