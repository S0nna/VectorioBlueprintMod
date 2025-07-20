using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Token: 0x02000185 RID: 389
public class InputManager
{
	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000CF6 RID: 3318 RVA: 0x00038C2D File Offset: 0x00036E2D
	public static bool IsControlledConnected
	{
		get
		{
			return InputManager._isControllerConnected;
		}
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00038C34 File Offset: 0x00036E34
	public static void ConnectController(InputController controller)
	{
		if (InputManager._isControllerConnected)
		{
			Debug.Log("[INPUT MANAGER] Controller is already connected!");
			return;
		}
		InputManager._connectedController = controller2;
		InputManager._isControllerConnected = true;
		controller2.PrimaryAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryActionPressed.Invoke();
		};
		controller2.PrimaryAction.canceled += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryActionReleased.Invoke();
		};
		controller2.PanCameraAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPanCameraActionPressed.Invoke();
		};
		controller2.PanCameraAction.canceled += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPanCameraActionReleased.Invoke();
		};
		controller2.SecondaryAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryActionPressed.Invoke();
		};
		controller2.SecondaryAction.canceled += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryActionReleased.Invoke();
		};
		controller2.PrimaryModifierAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryModifierActionPressed.Invoke();
		};
		controller2.PrimaryModifierAction.canceled += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryModifierActionReleased.Invoke();
		};
		controller2.SecondaryModifierAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryModifierActionPressed.Invoke();
		};
		controller2.SecondaryModifierAction.canceled += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryModifierActionReleased.Invoke();
		};
		controller2.HotbarNumber.performed += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnHotbarNumberChanged.Invoke((int)context.ReadValue<float>());
		};
		controller2.HotbarSwitch.performed += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnFunctionNumberChanged.Invoke((int)context.ReadValue<float>());
		};
		controller2.EditMode.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnEditModeActionPressed.Invoke();
		};
		controller2.BuildMode.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnBuildModeActionPressed.Invoke();
		};
		controller2.CommandMode.started += delegate(InputAction.CallbackContext controller)
		{
			InputManager.OnCommandModePressed.Invoke();
		};
		controller2.DeleteMode.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnDeleteModeActionPressed.Invoke();
		};
		controller2.DeselectMode.started += delegate(InputAction.CallbackContext controller)
		{
			InputManager.OnDeselectModePressed.Invoke();
		};
		controller2.PipetteAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPipetteActionPressed.Invoke();
		};
		controller2.FastCameraAction.performed += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnFastCameraActionHeld.Invoke();
		};
		controller2.SlowCameraAction.performed += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSlowCameraActionHeld.Invoke();
		};
		controller2.InventoryAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnInventoryActionPressed.Invoke();
		};
		controller2.ResearchAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnResearchActionPressed.Invoke();
		};
		controller2.MapAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnMapActionPressed.Invoke();
		};
		controller2.ConsoleAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnConsoleActionPressed.Invoke();
		};
		controller2.BackAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnBackActionPressed.Invoke();
		};
		controller2.HudAction.started += delegate(InputAction.CallbackContext context)
		{
			InputManager.OnHudActionPressed.Invoke();
		};
		Debug.Log("[INPUT MANAGER] Connected to controller " + controller2.name + " successfully!");
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x000390C0 File Offset: 0x000372C0
	public static void DisconnectController(InputController controller)
	{
		if (!InputManager._isControllerConnected)
		{
			Debug.Log("[INPUT MANAGER] Controller is not connected!");
			return;
		}
		if (controller2 != InputManager._connectedController)
		{
			Debug.Log("[INPUT MANAGER] The connected controller does not match!");
			return;
		}
		InputManager._connectedController = null;
		InputManager._isControllerConnected = false;
		controller2.PrimaryAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryActionPressed.Invoke();
		};
		controller2.PrimaryAction.canceled -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryActionReleased.Invoke();
		};
		controller2.PanCameraAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPanCameraActionPressed.Invoke();
		};
		controller2.PanCameraAction.canceled -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPanCameraActionReleased.Invoke();
		};
		controller2.SecondaryAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryActionPressed.Invoke();
		};
		controller2.SecondaryAction.canceled -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryActionReleased.Invoke();
		};
		controller2.PrimaryModifierAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryModifierActionPressed.Invoke();
		};
		controller2.PrimaryModifierAction.canceled -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPrimaryModifierActionReleased.Invoke();
		};
		controller2.SecondaryModifierAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryModifierActionPressed.Invoke();
		};
		controller2.SecondaryModifierAction.canceled -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnSecondaryModifierActionReleased.Invoke();
		};
		controller2.HotbarNumber.performed -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnHotbarNumberChanged.Invoke(context.ReadValue<int>());
		};
		controller2.HotbarSwitch.performed -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnFunctionNumberChanged.Invoke(context.ReadValue<int>());
		};
		controller2.EditMode.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnEditModeActionPressed.Invoke();
		};
		controller2.BuildMode.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnBuildModeActionPressed.Invoke();
		};
		controller2.CommandMode.started -= delegate(InputAction.CallbackContext controller)
		{
			InputManager.OnCommandModePressed.Invoke();
		};
		controller2.DeleteMode.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnDeleteModeActionPressed.Invoke();
		};
		controller2.DeselectMode.started -= delegate(InputAction.CallbackContext controller)
		{
			InputManager.OnDeselectModePressed.Invoke();
		};
		controller2.PipetteAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnPipetteActionPressed.Invoke();
		};
		controller2.FastCameraAction.performed -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnFastCameraActionHeld.Invoke();
		};
		controller2.InventoryAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnInventoryActionPressed.Invoke();
		};
		controller2.ResearchAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnResearchActionPressed.Invoke();
		};
		controller2.MapAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnMapActionPressed.Invoke();
		};
		controller2.ConsoleAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnConsoleActionPressed.Invoke();
		};
		controller2.BackAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnBackActionPressed.Invoke();
		};
		controller2.HudAction.started -= delegate(InputAction.CallbackContext context)
		{
			InputManager.OnHudActionPressed.Invoke();
		};
		Debug.Log("[INPUT MANAGER] Disconnected from controller " + controller2.name + " successfully!");
	}

	// Token: 0x040008DE RID: 2270
	private static InputController _connectedController = null;

	// Token: 0x040008DF RID: 2271
	private static bool _isControllerConnected = false;

	// Token: 0x040008E0 RID: 2272
	public static UnityEvent<int> OnHotbarNumberChanged = new UnityEvent<int>();

	// Token: 0x040008E1 RID: 2273
	public static UnityEvent<int> OnFunctionNumberChanged = new UnityEvent<int>();

	// Token: 0x040008E2 RID: 2274
	public static UnityEvent OnPrimaryActionPressed = new UnityEvent();

	// Token: 0x040008E3 RID: 2275
	public static UnityEvent OnPrimaryActionReleased = new UnityEvent();

	// Token: 0x040008E4 RID: 2276
	public static UnityEvent OnSecondaryActionPressed = new UnityEvent();

	// Token: 0x040008E5 RID: 2277
	public static UnityEvent OnSecondaryActionReleased = new UnityEvent();

	// Token: 0x040008E6 RID: 2278
	public static UnityEvent OnPanCameraActionPressed = new UnityEvent();

	// Token: 0x040008E7 RID: 2279
	public static UnityEvent OnPanCameraActionReleased = new UnityEvent();

	// Token: 0x040008E8 RID: 2280
	public static UnityEvent OnPrimaryModifierActionPressed = new UnityEvent();

	// Token: 0x040008E9 RID: 2281
	public static UnityEvent OnPrimaryModifierActionReleased = new UnityEvent();

	// Token: 0x040008EA RID: 2282
	public static UnityEvent OnSecondaryModifierActionPressed = new UnityEvent();

	// Token: 0x040008EB RID: 2283
	public static UnityEvent OnSecondaryModifierActionReleased = new UnityEvent();

	// Token: 0x040008EC RID: 2284
	public static UnityEvent OnPipetteActionPressed = new UnityEvent();

	// Token: 0x040008ED RID: 2285
	public static UnityEvent OnFastCameraActionHeld = new UnityEvent();

	// Token: 0x040008EE RID: 2286
	public static UnityEvent OnSlowCameraActionHeld = new UnityEvent();

	// Token: 0x040008EF RID: 2287
	public static UnityEvent OnEditModeActionPressed = new UnityEvent();

	// Token: 0x040008F0 RID: 2288
	public static UnityEvent OnBuildModeActionPressed = new UnityEvent();

	// Token: 0x040008F1 RID: 2289
	public static UnityEvent OnDeleteModeActionPressed = new UnityEvent();

	// Token: 0x040008F2 RID: 2290
	public static UnityEvent OnCommandModePressed = new UnityEvent();

	// Token: 0x040008F3 RID: 2291
	public static UnityEvent OnDeselectModePressed = new UnityEvent();

	// Token: 0x040008F4 RID: 2292
	public static UnityEvent OnInventoryActionPressed = new UnityEvent();

	// Token: 0x040008F5 RID: 2293
	public static UnityEvent OnResearchActionPressed = new UnityEvent();

	// Token: 0x040008F6 RID: 2294
	public static UnityEvent OnMapActionPressed = new UnityEvent();

	// Token: 0x040008F7 RID: 2295
	public static UnityEvent OnConsoleActionPressed = new UnityEvent();

	// Token: 0x040008F8 RID: 2296
	public static UnityEvent OnBackActionPressed = new UnityEvent();

	// Token: 0x040008F9 RID: 2297
	public static UnityEvent OnHudActionPressed = new UnityEvent();
}
