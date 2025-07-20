using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Vectorio;
using Vectorio.PhasmaUI;

// Token: 0x02000184 RID: 388
public class InputController : MonoBehaviour
{
	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0003805C File Offset: 0x0003625C
	public InputAction PrimaryAction
	{
		get
		{
			return this._action_primary;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00038064 File Offset: 0x00036264
	public InputAction SecondaryAction
	{
		get
		{
			return this._action_secondary;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003806C File Offset: 0x0003626C
	public InputAction PipetteAction
	{
		get
		{
			return this._action_pipette;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00038074 File Offset: 0x00036274
	public InputAction PanCameraAction
	{
		get
		{
			return this._action_pan;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0003807C File Offset: 0x0003627C
	public InputAction PrimaryModifierAction
	{
		get
		{
			return this._action_primary_modifier;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00038084 File Offset: 0x00036284
	public InputAction SecondaryModifierAction
	{
		get
		{
			return this._action_secondary_modifier;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0003808C File Offset: 0x0003628C
	public InputAction FastCameraAction
	{
		get
		{
			return this._action_fast_camera;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00038094 File Offset: 0x00036294
	public InputAction SlowCameraAction
	{
		get
		{
			return this._action_slow_camera;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0003809C File Offset: 0x0003629C
	public InputAction HotbarNumber
	{
		get
		{
			return this._action_hotbar_number;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x000380A4 File Offset: 0x000362A4
	public InputAction HotbarSwitch
	{
		get
		{
			return this._action_hotbar_switch;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x000380AC File Offset: 0x000362AC
	public InputAction InventoryAction
	{
		get
		{
			return this._action_inventory;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x000380B4 File Offset: 0x000362B4
	public InputAction ResearchAction
	{
		get
		{
			return this._action_research;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x000380BC File Offset: 0x000362BC
	public InputAction MapAction
	{
		get
		{
			return this._action_map;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x000380C4 File Offset: 0x000362C4
	public InputAction ConsoleAction
	{
		get
		{
			return this._action_console;
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000380CC File Offset: 0x000362CC
	public InputAction BackAction
	{
		get
		{
			return this._action_back;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000380D4 File Offset: 0x000362D4
	public InputAction HudAction
	{
		get
		{
			return this._action_hud;
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000380DC File Offset: 0x000362DC
	public InputAction EditMode
	{
		get
		{
			return this._action_edit_mode;
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000CDD RID: 3293 RVA: 0x000380E4 File Offset: 0x000362E4
	public InputAction BuildMode
	{
		get
		{
			return this._action_build_mode;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000CDE RID: 3294 RVA: 0x000380EC File Offset: 0x000362EC
	public InputAction DeleteMode
	{
		get
		{
			return this._action_delete_mode;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000CDF RID: 3295 RVA: 0x000380F4 File Offset: 0x000362F4
	public InputAction CommandMode
	{
		get
		{
			return this._action_command_mode;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x000380FC File Offset: 0x000362FC
	public InputAction DeselectMode
	{
		get
		{
			return this._action_deselect_mode;
		}
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x00038104 File Offset: 0x00036304
	private void Awake()
	{
		this._camera = Camera.main;
		this._targetZoom = this._camera.orthographicSize;
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00038124 File Offset: 0x00036324
	public void Start()
	{
		Singleton<Events>.Instance.onControllerReady.Invoke(this);
		Singleton<Events>.Instance.onUpdatedKeybinds.AddListener(new UnityAction<InputActions>(this.SetBindings));
		Singleton<Events>.Instance.onStartDraggingCamera.AddListener(new UnityAction(this.StartDragging));
		Singleton<Events>.Instance.onStopDraggingCamera.AddListener(new UnityAction(this.StopDragging));
		Singleton<Events>.Instance.onJumpSequenceStarted.AddListener(new UnityAction<Gateway>(this.OnJumpSequenceStarted));
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000381AD File Offset: 0x000363AD
	private Vector3 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000381CD File Offset: 0x000363CD
	private void StartDragging()
	{
		this._isDragging = true;
		this._dragOrigin = this.GetMousePosition();
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x000381E2 File Offset: 0x000363E2
	private void StopDragging()
	{
		this._isDragging = false;
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x000381EB File Offset: 0x000363EB
	private void Update()
	{
		if (!Singleton<Gamemode>.Instance.AllowClientMovement)
		{
			return;
		}
		this.CalculateZoom();
		if (!this._isDragging)
		{
			this.CalculateMovement();
			if (Singleton<Settings>.Instance.UseScreenPanning)
			{
				this.CalculatePanning();
			}
			this.ClampPosition();
		}
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x00038226 File Offset: 0x00036426
	private void LateUpdate()
	{
		if (!Singleton<Gamemode>.Instance.AllowClientMovement)
		{
			return;
		}
		if (this._isDragging)
		{
			this.CalculateDrag();
			this.ClampPosition();
		}
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0003824C File Offset: 0x0003644C
	private void CalculateMovement()
	{
		this._movement = this._action_move.ReadValue<Vector2>();
		this._moveSpeed = (this._action_fast_camera.IsPressed() ? this.fastSpeed : (this._action_slow_camera.IsPressed() ? this.slowSpeed : this.normalSpeed));
		Vector3 translation = new Vector3(this._movement.x, this._movement.y, 0f) * this._moveSpeed * Singleton<Settings>.Instance.MovementMultiplier * Time.deltaTime;
		base.transform.Translate(translation, Space.World);
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x000382F4 File Offset: 0x000364F4
	private void CalculatePanning()
	{
		if (!Singleton<Settings>.Instance.UseScreenPanning)
		{
			return;
		}
		Vector3 mousePosition = Input.mousePosition;
		Vector3 vector = Vector3.zero;
		if (mousePosition.x <= this.edgeThreshold)
		{
			vector += Vector3.left;
		}
		else if (mousePosition.x >= (float)Screen.width - this.edgeThreshold)
		{
			vector += Vector3.right;
		}
		if (mousePosition.y <= this.edgeThreshold)
		{
			vector += Vector3.down;
		}
		else if (mousePosition.y >= (float)Screen.height - this.edgeThreshold)
		{
			vector += Vector3.up;
		}
		if (vector != Vector3.zero)
		{
			base.transform.Translate(vector.normalized * this._moveSpeed * Time.deltaTime, Space.World);
		}
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000383CC File Offset: 0x000365CC
	private void ClampPosition()
	{
		Vector3 position = base.transform.position;
		position.x = Mathf.Clamp(position.x, Singleton<WorldGenerator>.Instance.RegionMinBoundaryX, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryX);
		position.y = Mathf.Clamp(position.y, Singleton<WorldGenerator>.Instance.RegionMinBoundaryY, Singleton<WorldGenerator>.Instance.RegionMaxBoundaryY);
		base.transform.position = position;
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x00038440 File Offset: 0x00036640
	private void CalculateZoom()
	{
		if (Singleton<Interface>.Instance.IsMouseOverUI)
		{
			return;
		}
		float num = this._gridToggle ? 2f : 1f;
		if (this._action_fast_camera.IsPressed())
		{
			num *= 2f;
		}
		float num2 = Mathf.Abs(this._action_scroll.ReadValue<float>());
		if (num2 > 1f)
		{
			if (num2 > this._mouseAdjust)
			{
				this._mouseAdjust = Mathf.Min(num2, this.maxAdjustment);
			}
			this._targetZoom -= this._action_scroll.ReadValue<float>() / this._mouseAdjust * this.zoomFactor * num;
		}
		else
		{
			this._targetZoom -= this._action_scroll.ReadValue<float>() * this.zoomFactor * num;
		}
		this._targetZoom = Mathf.Clamp(this._targetZoom, this.minZoom, this.maxZoom);
		float num3 = Mathf.Lerp(this._camera.orthographicSize, this._targetZoom, Time.deltaTime * this.zoomSpeed * num);
		this.UpdateAudioSpatialBlending(num3);
		this._camera.orthographicSize = num3;
		if (!this._gridToggle && this._camera.orthographicSize > this.maxGridZoom)
		{
			Singleton<Events>.Instance.onToggleGrid.Invoke(false);
			this._gridToggle = true;
			return;
		}
		if (this._gridToggle && this._camera.orthographicSize < this.maxGridZoom)
		{
			Singleton<Events>.Instance.onToggleGrid.Invoke(true);
			this._gridToggle = false;
		}
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x000385C0 File Offset: 0x000367C0
	private void UpdateAudioSpatialBlending(float newZoom)
	{
		if (AudioPlayer.ALLOW_INFLUENCE_ON_SPATIAL_BLENDING && newZoom != this._camera.orthographicSize)
		{
			float t = Mathf.InverseLerp(this.minZoom, this.maxGridZoom, newZoom);
			float spatialBlending = Mathf.Lerp(0.5f, 1f, t);
			Singleton<AudioPlayer>.Instance.SetSpatialBlending(spatialBlending);
		}
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x00038611 File Offset: 0x00036811
	private void CalculateDrag()
	{
		this._dragDifference = this.GetMousePosition() - this._camera.transform.position;
		base.transform.position = this._dragOrigin - this._dragDifference;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00038650 File Offset: 0x00036850
	private void OnJumpSequenceStarted(Gateway gateway)
	{
		base.transform.position = gateway.transform.position;
		Singleton<Events>.Instance.onChangeBuildMode.Invoke(Hologram.BuildMode.Default);
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00038678 File Offset: 0x00036878
	public void SetBindings(InputActions inputActions)
	{
		Debug.Log("[CONTROLLER] Received new bindings, updating controller.");
		if (this._areBindingsSetup)
		{
			this.DisableBindings();
		}
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
		this._areBindingsSetup = true;
		this.EnableBindings();
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0003888A File Offset: 0x00036A8A
	private void OnEnable()
	{
		this.ToggleActionMap(true);
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00038893 File Offset: 0x00036A93
	private void OnDisable()
	{
		this.ToggleActionMap(false);
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0003889C File Offset: 0x00036A9C
	public void ToggleActionMap(bool toggle)
	{
		if (!toggle)
		{
			this.DisableBindings();
			return;
		}
		if (Singleton<Settings>.Instance != null && Singleton<Settings>.Instance.GetInputActions != null)
		{
			this.SetBindings(Singleton<Settings>.Instance.GetInputActions);
			return;
		}
		this.SetBindings(new InputActions());
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x000388E8 File Offset: 0x00036AE8
	private void EnableBindings()
	{
		if (!this._areBindingsSetup)
		{
			Debug.Log("[INPUT CONTROLLER] Could not enable bindings as bindings have not been set!");
			return;
		}
		if (!this._bindingsEnabled)
		{
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
			InputManager.ConnectController(this);
		}
		else
		{
			Debug.Log("[INPUT CONTROLLER] Cannot enable bindings, as bindings are already active!");
		}
		this._bindingsEnabled = true;
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x00038A34 File Offset: 0x00036C34
	private void DisableBindings()
	{
		if (!this._areBindingsSetup)
		{
			Debug.Log("[INPUT CONTROLLER] Could not disable bindings as bindings have not been set!");
			return;
		}
		if (this._bindingsEnabled)
		{
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
			InputManager.DisconnectController(this);
		}
		else
		{
			Debug.Log("[INPUT CONTROLLER] Cannot disable bindings, as bindings are already disabled!");
		}
		this._bindingsEnabled = false;
	}

	// Token: 0x040008AE RID: 2222
	private InputAction _action_move;

	// Token: 0x040008AF RID: 2223
	private InputAction _action_pan;

	// Token: 0x040008B0 RID: 2224
	private InputAction _action_mouse;

	// Token: 0x040008B1 RID: 2225
	private InputAction _action_scroll;

	// Token: 0x040008B2 RID: 2226
	private InputAction _action_primary;

	// Token: 0x040008B3 RID: 2227
	private InputAction _action_secondary;

	// Token: 0x040008B4 RID: 2228
	private InputAction _action_pipette;

	// Token: 0x040008B5 RID: 2229
	private InputAction _action_primary_modifier;

	// Token: 0x040008B6 RID: 2230
	private InputAction _action_secondary_modifier;

	// Token: 0x040008B7 RID: 2231
	private InputAction _action_fast_camera;

	// Token: 0x040008B8 RID: 2232
	private InputAction _action_hotbar_number;

	// Token: 0x040008B9 RID: 2233
	private InputAction _action_hotbar_switch;

	// Token: 0x040008BA RID: 2234
	private InputAction _action_edit_mode;

	// Token: 0x040008BB RID: 2235
	private InputAction _action_inventory;

	// Token: 0x040008BC RID: 2236
	private InputAction _action_research;

	// Token: 0x040008BD RID: 2237
	private InputAction _action_map;

	// Token: 0x040008BE RID: 2238
	private InputAction _action_console;

	// Token: 0x040008BF RID: 2239
	private InputAction _action_delete_mode;

	// Token: 0x040008C0 RID: 2240
	private InputAction _action_back;

	// Token: 0x040008C1 RID: 2241
	private InputAction _action_build_mode;

	// Token: 0x040008C2 RID: 2242
	private InputAction _action_hud;

	// Token: 0x040008C3 RID: 2243
	private InputAction _action_command_mode;

	// Token: 0x040008C4 RID: 2244
	private InputAction _action_deselect_mode;

	// Token: 0x040008C5 RID: 2245
	private InputAction _action_slow_camera;

	// Token: 0x040008C6 RID: 2246
	[Header("Movement Options")]
	public float normalSpeed = 150f;

	// Token: 0x040008C7 RID: 2247
	public float fastSpeed = 500f;

	// Token: 0x040008C8 RID: 2248
	public float slowSpeed = 25f;

	// Token: 0x040008C9 RID: 2249
	public float edgeThreshold = 10f;

	// Token: 0x040008CA RID: 2250
	public float edgeSpeed = 50f;

	// Token: 0x040008CB RID: 2251
	[Header("Zoom Options")]
	public float zoomFactor = 25f;

	// Token: 0x040008CC RID: 2252
	public float zoomSpeed = 20f;

	// Token: 0x040008CD RID: 2253
	public float lerpSpeed = 2f;

	// Token: 0x040008CE RID: 2254
	public float fastZoomSpeed = 150f;

	// Token: 0x040008CF RID: 2255
	[Header("Zoom Options (Grid)")]
	public float minZoom = 10f;

	// Token: 0x040008D0 RID: 2256
	public float maxGridZoom = 200f;

	// Token: 0x040008D1 RID: 2257
	public float maxZoom = 500f;

	// Token: 0x040008D2 RID: 2258
	public float maxAdjustment = 300f;

	// Token: 0x040008D3 RID: 2259
	protected Camera _camera;

	// Token: 0x040008D4 RID: 2260
	protected Vector2 _movement;

	// Token: 0x040008D5 RID: 2261
	protected float _moveSpeed;

	// Token: 0x040008D6 RID: 2262
	protected float _targetZoom;

	// Token: 0x040008D7 RID: 2263
	protected bool _gridToggle;

	// Token: 0x040008D8 RID: 2264
	private Vector3 _dragOrigin;

	// Token: 0x040008D9 RID: 2265
	private Vector3 _dragDifference;

	// Token: 0x040008DA RID: 2266
	private bool _isDragging;

	// Token: 0x040008DB RID: 2267
	private bool _bindingsEnabled;

	// Token: 0x040008DC RID: 2268
	private bool _areBindingsSetup;

	// Token: 0x040008DD RID: 2269
	private float _mouseAdjust = 10f;
}
