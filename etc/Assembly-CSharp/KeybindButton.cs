using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Vectorio.PhasmaUI;

// Token: 0x02000153 RID: 339
[DefaultExecutionOrder(20)]
public class KeybindButton : Button
{
	// Token: 0x06000B0A RID: 2826 RVA: 0x0002F368 File Offset: 0x0002D568
	public void Setup(InputAction action, int index)
	{
		this._inputAction = action;
		this._bindingIndex = index;
		this._defaultBinding = action.bindings[index].effectivePath;
		this.UpdateUI();
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0002F3A6 File Offset: 0x0002D5A6
	public override void OnClick()
	{
		this.StartRebinding();
		base.OnClick();
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0002F3B4 File Offset: 0x0002D5B4
	public void StartRebinding()
	{
		Singleton<Settings>.Instance.StartRebinding(this._inputAction.name, this._bindingIndex, this.inputText, new Action(this.OnRebindComplete), new Action(this.OnRebindCanceled));
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x0002F3F0 File Offset: 0x0002D5F0
	public void ResetToDefault()
	{
		if (this._inputAction != null)
		{
			this._inputAction.ApplyBindingOverride(this._bindingIndex, this._defaultBinding);
			Singleton<Settings>.Instance.UpdateBinding(this._inputAction.name, this._bindingIndex, this._defaultBinding);
			this.UpdateUI();
		}
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0002F443 File Offset: 0x0002D643
	private void OnRebindComplete()
	{
		this.UpdateUI();
		Singleton<AudioPlayer>.Instance.PlayInterfaceSound(this.bindingFinishSound);
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0002F45B File Offset: 0x0002D65B
	private void OnRebindCanceled()
	{
		this.UpdateUI();
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0002F464 File Offset: 0x0002D664
	private void UpdateUI()
	{
		if (this.inputText != null)
		{
			if (Application.isPlaying)
			{
				this.inputText.text = Singleton<Settings>.Instance.GetBindingName(this._inputAction.name, this._bindingIndex);
				return;
			}
			this.inputText.text = this._inputAction.GetBindingDisplayString(this._bindingIndex, (InputBinding.DisplayStringOptions)0);
		}
	}

	// Token: 0x04000752 RID: 1874
	[Header("Interface Fields")]
	public TextMeshProUGUI inputText;

	// Token: 0x04000753 RID: 1875
	public AudioClip bindingFinishSound;

	// Token: 0x04000754 RID: 1876
	private InputAction _inputAction;

	// Token: 0x04000755 RID: 1877
	private int _bindingIndex;

	// Token: 0x04000756 RID: 1878
	private string _defaultBinding;
}
