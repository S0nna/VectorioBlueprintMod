using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Vectorio;

// Token: 0x020001CF RID: 463
public class MenuController : MonoBehaviour
{
	// Token: 0x06000E89 RID: 3721 RVA: 0x00041446 File Offset: 0x0003F646
	public void Start()
	{
		Singleton<Events>.Instance.onUpdatedKeybinds.AddListener(new UnityAction<InputActions>(this.SetBindings));
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x00041463 File Offset: 0x0003F663
	private void OnEnable()
	{
		this.ToggleActionMap(true);
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0004146C File Offset: 0x0003F66C
	private void OnDisable()
	{
		this.ToggleActionMap(false);
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x00041478 File Offset: 0x0003F678
	public void SetBindings(InputActions inputActions)
	{
		this._action_move = inputActions.Player.MoveCamera;
		this._action_pan = inputActions.Player.PanCamera;
		this._action_mouse = inputActions.Player.Mouse;
		this._action_scroll = inputActions.Player.Zoom;
		this._action_primary = inputActions.Player.Primary;
		this._action_secondary = inputActions.Player.Secondary;
		this._action_pipette = inputActions.Player.Pipette;
		this._action_primary_modifier = inputActions.Player.PrimaryModifier;
		this._action_secondary_modifier = inputActions.Player.SecondaryModifier;
		this._action_fast_camera = inputActions.Player.FastCamera;
		this._action_slow_camera = inputActions.Player.SlowCamera;
		this._action_hotbar_number = inputActions.Player.HotbarNumber;
		this._action_hotbar_switch = inputActions.Player.HotbarSwitch;
		this._action_inventory = inputActions.Player.OpenInventory;
		this._action_research = inputActions.Player.OpenResearch;
		this._action_delete_mode = inputActions.Player.DeleteMode;
		this._action_command_mode = inputActions.Player.CommandMode;
		this._action_build_mode = inputActions.Player.BuildMode;
		this._action_edit_mode = inputActions.Player.EditMode;
		this._action_deselect_mode = inputActions.Player.DeselectMode;
		this._action_map = inputActions.Player.ToggleMap;
		this._action_console = inputActions.Player.DeveloperTools;
		this._action_back = inputActions.Player.CloseMenu;
		this._action_hud = inputActions.Player.ToggleHUD;
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00041668 File Offset: 0x0003F868
	public void ToggleActionMap(bool toggle)
	{
		if (toggle)
		{
			Debug.Log("[CONTROLLER] Enabling action mapping on " + base.transform.name);
			this.SetBindings(new InputActions());
			this._action_move.Enable();
			this._action_pan.Enable();
			this._action_mouse.Enable();
			this._action_scroll.Enable();
			this._action_primary.Enable();
			this._action_secondary.Enable();
			this._action_pipette.Enable();
			this._action_primary_modifier.Enable();
			this._action_secondary_modifier.Enable();
			this._action_fast_camera.Enable();
			this._action_slow_camera.Enable();
			this._action_hotbar_number.Enable();
			this._action_hotbar_switch.Enable();
			this._action_edit_mode.Enable();
			this._action_inventory.Enable();
			this._action_research.Enable();
			this._action_map.Enable();
			this._action_console.Enable();
			this._action_delete_mode.Enable();
			this._action_build_mode.Enable();
			this._action_command_mode.Enable();
			this._action_back.Enable();
			this._action_hud.Enable();
			this._action_deselect_mode.Enable();
			return;
		}
		Debug.Log("[CONTROLLER] Disabling action mapping on " + base.transform.name);
		this._action_move.Disable();
		this._action_pan.Disable();
		this._action_mouse.Disable();
		this._action_scroll.Disable();
		this._action_primary.Disable();
		this._action_secondary.Disable();
		this._action_pipette.Disable();
		this._action_primary_modifier.Disable();
		this._action_secondary_modifier.Disable();
		this._action_fast_camera.Disable();
		this._action_slow_camera.Disable();
		this._action_hotbar_number.Disable();
		this._action_hotbar_switch.Disable();
		this._action_edit_mode.Disable();
		this._action_inventory.Disable();
		this._action_research.Disable();
		this._action_map.Disable();
		this._action_console.Disable();
		this._action_delete_mode.Disable();
		this._action_build_mode.Disable();
		this._action_command_mode.Disable();
		this._action_back.Disable();
		this._action_hud.Disable();
		this._action_deselect_mode.Disable();
	}

	// Token: 0x04000B64 RID: 2916
	private InputAction _action_move;

	// Token: 0x04000B65 RID: 2917
	private InputAction _action_pan;

	// Token: 0x04000B66 RID: 2918
	private InputAction _action_mouse;

	// Token: 0x04000B67 RID: 2919
	private InputAction _action_scroll;

	// Token: 0x04000B68 RID: 2920
	private InputAction _action_primary;

	// Token: 0x04000B69 RID: 2921
	private InputAction _action_secondary;

	// Token: 0x04000B6A RID: 2922
	private InputAction _action_pipette;

	// Token: 0x04000B6B RID: 2923
	private InputAction _action_primary_modifier;

	// Token: 0x04000B6C RID: 2924
	private InputAction _action_secondary_modifier;

	// Token: 0x04000B6D RID: 2925
	private InputAction _action_fast_camera;

	// Token: 0x04000B6E RID: 2926
	private InputAction _action_hotbar_number;

	// Token: 0x04000B6F RID: 2927
	private InputAction _action_hotbar_switch;

	// Token: 0x04000B70 RID: 2928
	private InputAction _action_edit_mode;

	// Token: 0x04000B71 RID: 2929
	private InputAction _action_inventory;

	// Token: 0x04000B72 RID: 2930
	private InputAction _action_research;

	// Token: 0x04000B73 RID: 2931
	private InputAction _action_map;

	// Token: 0x04000B74 RID: 2932
	private InputAction _action_console;

	// Token: 0x04000B75 RID: 2933
	private InputAction _action_delete_mode;

	// Token: 0x04000B76 RID: 2934
	private InputAction _action_back;

	// Token: 0x04000B77 RID: 2935
	private InputAction _action_build_mode;

	// Token: 0x04000B78 RID: 2936
	private InputAction _action_hud;

	// Token: 0x04000B79 RID: 2937
	private InputAction _action_command_mode;

	// Token: 0x04000B7A RID: 2938
	private InputAction _action_deselect_mode;

	// Token: 0x04000B7B RID: 2939
	private InputAction _action_slow_camera;
}
