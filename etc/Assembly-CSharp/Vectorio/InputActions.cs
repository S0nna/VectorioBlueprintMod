using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Vectorio
{
	// Token: 0x02000256 RID: 598
	public class InputActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x0004F515 File Offset: 0x0004D715
		public InputActionAsset asset { get; }

		// Token: 0x06001131 RID: 4401 RVA: 0x0004F520 File Offset: 0x0004D720
		public InputActions()
		{
			this.asset = InputActionAsset.FromJson("{\n    \"name\": \"Player\",\n    \"maps\": [\n        {\n            \"name\": \"Player\",\n            \"id\": \"2ceaf91d-0b2f-4be9-b4ff-87f006d70a74\",\n            \"actions\": [\n                {\n                    \"name\": \"Move Camera\",\n                    \"type\": \"Value\",\n                    \"id\": \"dd8ed8c5-e2a5-4574-8cdf-34bddc60410f\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Pan Camera\",\n                    \"type\": \"Button\",\n                    \"id\": \"e95f659c-f6f9-43dc-b50d-3f37c15f602c\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Zoom\",\n                    \"type\": \"Value\",\n                    \"id\": \"cc0714cf-9281-469a-81a1-3eb0adacef7e\",\n                    \"expectedControlType\": \"Axis\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Primary\",\n                    \"type\": \"Button\",\n                    \"id\": \"fae08b21-3b2d-4e5b-bfcf-b85c66711e51\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Secondary\",\n                    \"type\": \"Button\",\n                    \"id\": \"6703741b-7435-4d2f-b3f1-fc34b1a60f70\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Pipette\",\n                    \"type\": \"Button\",\n                    \"id\": \"ef059d73-c89a-4a58-b8b0-f21a9a3ada10\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Mouse\",\n                    \"type\": \"Value\",\n                    \"id\": \"529f0592-14b8-485f-a2ab-156d5a7ce313\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Primary Modifier\",\n                    \"type\": \"Button\",\n                    \"id\": \"f79383dc-794b-4f34-80e4-533953571b17\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Secondary Modifier\",\n                    \"type\": \"Button\",\n                    \"id\": \"b6489f15-73c7-45e8-b44e-81ff64c4d609\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Hotbar Number\",\n                    \"type\": \"Button\",\n                    \"id\": \"5f6f58f1-1895-477c-a8fb-924e31bbb9bb\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Hotbar Switch\",\n                    \"type\": \"Button\",\n                    \"id\": \"dadeef4c-5f9b-4e55-b583-22a1fab99802\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Fast Camera\",\n                    \"type\": \"Button\",\n                    \"id\": \"128bf608-de28-45f6-ac06-389b5c80c6d5\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Slow Camera\",\n                    \"type\": \"Button\",\n                    \"id\": \"edec7f50-ccf6-4632-adbe-ca513a79e3e3\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Open Inventory\",\n                    \"type\": \"Button\",\n                    \"id\": \"c210d2ae-6fe4-45ca-b6b5-c2a264917d3a\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Open Research\",\n                    \"type\": \"Button\",\n                    \"id\": \"55a4b775-0b19-4017-94fe-592961ff0ee8\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Toggle Map\",\n                    \"type\": \"Button\",\n                    \"id\": \"98b084a7-e128-42c5-966b-1a3a49dd5a8c\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Toggle HUD\",\n                    \"type\": \"Button\",\n                    \"id\": \"ebc06a04-cba2-44ea-8677-95d2f17dc5ab\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Build Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"35dfd745-8d46-479c-8fea-af0223463a33\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Edit Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"5af9a48f-00e7-41b9-bf1d-d575ad48c079\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Command Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"65603e04-4433-4ec7-ac11-e64b461aa6ed\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Delete Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"9b8316ea-a3cb-43fc-80d4-7efed7640bc9\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Deselect Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"445d520d-6903-4e47-a872-d66eac591bfe\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Close Menu\",\n                    \"type\": \"Button\",\n                    \"id\": \"4ed6350c-ff7a-44d0-8b03-69dbbc241397\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Developer Tools\",\n                    \"type\": \"Button\",\n                    \"id\": \"eedbf405-0805-4b60-9b91-84d186d10e4a\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"978bfe49-cc26-4a3d-ab7b-7d7a29327403\",\n                    \"path\": \"<Gamepad>/leftStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Move Camera\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"WASD\",\n                    \"id\": \"00ca640b-d935-4593-8157-c05846ea39b3\",\n                    \"path\": \"Dpad\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move Camera\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"e2062cb9-1b15-46a2-838c-2f8d72a0bdd9\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Move Camera\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"320bffee-a40b-4347-ac70-c210eb8bc73a\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Move Camera\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"d2581a9b-1d11-4566-b27d-b92aff5fabbc\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Move Camera\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"fcfe95b8-67b9-4526-84b5-5d0bc98d6400\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Move Camera\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"143bb1cd-cc10-4eca-a2f0-a3664166fe91\",\n                    \"path\": \"<Gamepad>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Gamepad\",\n                    \"action\": \"Primary\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"05f6913d-c316-48b2-a6bb-e225f14c7960\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \";Keyboard&Mouse\",\n                    \"action\": \"Primary\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"66a38a5a-9c3d-4c96-8145-593e309d6e7f\",\n                    \"path\": \"<Gamepad>/leftTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Gamepad\",\n                    \"action\": \"Secondary\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6f7d1e69-4eeb-4a86-88ce-76388a2e4656\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Secondary\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"fedb79b3-4a20-4d0f-90b1-86320d898cae\",\n                    \"path\": \"<Keyboard>/leftShift\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Fast Camera\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"af88cf63-5f7d-45fe-a0d1-76394120225f\",\n                    \"path\": \"<Keyboard>/e\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Open Inventory\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"2855d3a1-d3da-43f4-b39b-0101245f117b\",\n                    \"path\": \"<Keyboard>/r\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Open Research\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"7235ec6c-6c19-43f5-ab54-0ac6cdad7cab\",\n                    \"path\": \"<Keyboard>/m\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Toggle Map\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"de76caa2-82e9-400b-8565-f54b36003552\",\n                    \"path\": \"<Keyboard>/x\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Delete Mode\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f489aa61-c8d7-4402-b39c-df52cefc6c76\",\n                    \"path\": \"<Keyboard>/leftCtrl\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Primary Modifier\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6e3be93d-b751-4524-b678-c5a6691fa6ba\",\n                    \"path\": \"<Keyboard>/leftAlt\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Secondary Modifier\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"80fbeb6b-3f97-4672-ba4c-99184d33d7f6\",\n                    \"path\": \"<Keyboard>/1\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"9f56e0f0-90b5-4e1d-99cd-8bef0ad41651\",\n                    \"path\": \"<Keyboard>/2\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=2)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"bef207bf-4bb8-40f4-98e9-42e3c270b224\",\n                    \"path\": \"<Keyboard>/3\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=3)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"8ff7ea70-adfb-463f-aead-9666262a69bd\",\n                    \"path\": \"<Keyboard>/4\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=4)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"3f969f5e-508b-464d-ada8-ab9b40573f2f\",\n                    \"path\": \"<Keyboard>/5\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=5)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d51e00df-bfb6-4166-9362-9c2cc7d1d664\",\n                    \"path\": \"<Keyboard>/6\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=6)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"a95975be-c94b-49e8-88e2-728526920ccd\",\n                    \"path\": \"<Keyboard>/7\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=7)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"aee7b3d3-d542-496a-863d-4974703ba704\",\n                    \"path\": \"<Keyboard>/8\",\n                    \"interactions\": \"Press\",\n                    \"processors\": \"Scale(factor=8)\",\n                    \"groups\": \"Keyboard&Mouse\",\n                    \"action\": \"Hotbar Number\",\n                    \"isComposite\": false,\n      [...string is too long...]");
			this.m_Player = this.asset.FindActionMap("Player", true);
			this.m_Player_MoveCamera = this.m_Player.FindAction("Move Camera", true);
			this.m_Player_PanCamera = this.m_Player.FindAction("Pan Camera", true);
			this.m_Player_Zoom = this.m_Player.FindAction("Zoom", true);
			this.m_Player_Primary = this.m_Player.FindAction("Primary", true);
			this.m_Player_Secondary = this.m_Player.FindAction("Secondary", true);
			this.m_Player_Pipette = this.m_Player.FindAction("Pipette", true);
			this.m_Player_Mouse = this.m_Player.FindAction("Mouse", true);
			this.m_Player_PrimaryModifier = this.m_Player.FindAction("Primary Modifier", true);
			this.m_Player_SecondaryModifier = this.m_Player.FindAction("Secondary Modifier", true);
			this.m_Player_HotbarNumber = this.m_Player.FindAction("Hotbar Number", true);
			this.m_Player_HotbarSwitch = this.m_Player.FindAction("Hotbar Switch", true);
			this.m_Player_FastCamera = this.m_Player.FindAction("Fast Camera", true);
			this.m_Player_SlowCamera = this.m_Player.FindAction("Slow Camera", true);
			this.m_Player_OpenInventory = this.m_Player.FindAction("Open Inventory", true);
			this.m_Player_OpenResearch = this.m_Player.FindAction("Open Research", true);
			this.m_Player_ToggleMap = this.m_Player.FindAction("Toggle Map", true);
			this.m_Player_ToggleHUD = this.m_Player.FindAction("Toggle HUD", true);
			this.m_Player_BuildMode = this.m_Player.FindAction("Build Mode", true);
			this.m_Player_EditMode = this.m_Player.FindAction("Edit Mode", true);
			this.m_Player_CommandMode = this.m_Player.FindAction("Command Mode", true);
			this.m_Player_DeleteMode = this.m_Player.FindAction("Delete Mode", true);
			this.m_Player_DeselectMode = this.m_Player.FindAction("Deselect Mode", true);
			this.m_Player_CloseMenu = this.m_Player.FindAction("Close Menu", true);
			this.m_Player_DeveloperTools = this.m_Player.FindAction("Developer Tools", true);
			this.m_UI = this.asset.FindActionMap("UI", true);
			this.m_UI_Navigate = this.m_UI.FindAction("Navigate", true);
			this.m_UI_Submit = this.m_UI.FindAction("Submit", true);
			this.m_UI_Cancel = this.m_UI.FindAction("Cancel", true);
			this.m_UI_Point = this.m_UI.FindAction("Point", true);
			this.m_UI_Click = this.m_UI.FindAction("Click", true);
			this.m_UI_ScrollWheel = this.m_UI.FindAction("ScrollWheel", true);
			this.m_UI_MiddleClick = this.m_UI.FindAction("MiddleClick", true);
			this.m_UI_RightClick = this.m_UI.FindAction("RightClick", true);
			this.m_UI_TrackedDevicePosition = this.m_UI.FindAction("TrackedDevicePosition", true);
			this.m_UI_TrackedDeviceOrientation = this.m_UI.FindAction("TrackedDeviceOrientation", true);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0004F8B8 File Offset: 0x0004DAB8
		public void Dispose()
		{
			Object.Destroy(this.asset);
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x0004F8C5 File Offset: 0x0004DAC5
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x0004F8D2 File Offset: 0x0004DAD2
		public InputBinding? bindingMask
		{
			get
			{
				return this.asset.bindingMask;
			}
			set
			{
				this.asset.bindingMask = value;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x0004F8E0 File Offset: 0x0004DAE0
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x0004F8ED File Offset: 0x0004DAED
		public ReadOnlyArray<InputDevice>? devices
		{
			get
			{
				return this.asset.devices;
			}
			set
			{
				this.asset.devices = value;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x0004F8FB File Offset: 0x0004DAFB
		public ReadOnlyArray<InputControlScheme> controlSchemes
		{
			get
			{
				return this.asset.controlSchemes;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0004F908 File Offset: 0x0004DB08
		public bool Contains(InputAction action)
		{
			return this.asset.Contains(action);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0004F916 File Offset: 0x0004DB16
		public IEnumerator<InputAction> GetEnumerator()
		{
			return this.asset.GetEnumerator();
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0004F923 File Offset: 0x0004DB23
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0004F92B File Offset: 0x0004DB2B
		public void Enable()
		{
			this.asset.Enable();
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0004F938 File Offset: 0x0004DB38
		public void Disable()
		{
			this.asset.Disable();
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x0004F945 File Offset: 0x0004DB45
		public IEnumerable<InputBinding> bindings
		{
			get
			{
				return this.asset.bindings;
			}
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0004F952 File Offset: 0x0004DB52
		public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
		{
			return this.asset.FindAction(actionNameOrId, throwIfNotFound);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0004F961 File Offset: 0x0004DB61
		public int FindBinding(InputBinding bindingMask, out InputAction action)
		{
			return this.asset.FindBinding(bindingMask, out action);
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0004F970 File Offset: 0x0004DB70
		public InputActions.PlayerActions Player
		{
			get
			{
				return new InputActions.PlayerActions(this);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x0004F978 File Offset: 0x0004DB78
		public InputActions.UIActions UI
		{
			get
			{
				return new InputActions.UIActions(this);
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0004F980 File Offset: 0x0004DB80
		public InputControlScheme KeyboardMouseScheme
		{
			get
			{
				if (this.m_KeyboardMouseSchemeIndex == -1)
				{
					this.m_KeyboardMouseSchemeIndex = this.asset.FindControlSchemeIndex("Keyboard&Mouse");
				}
				return this.asset.controlSchemes[this.m_KeyboardMouseSchemeIndex];
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x0004F9C8 File Offset: 0x0004DBC8
		public InputControlScheme GamepadScheme
		{
			get
			{
				if (this.m_GamepadSchemeIndex == -1)
				{
					this.m_GamepadSchemeIndex = this.asset.FindControlSchemeIndex("Gamepad");
				}
				return this.asset.controlSchemes[this.m_GamepadSchemeIndex];
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0004FA10 File Offset: 0x0004DC10
		public InputControlScheme TouchScheme
		{
			get
			{
				if (this.m_TouchSchemeIndex == -1)
				{
					this.m_TouchSchemeIndex = this.asset.FindControlSchemeIndex("Touch");
				}
				return this.asset.controlSchemes[this.m_TouchSchemeIndex];
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x0004FA58 File Offset: 0x0004DC58
		public InputControlScheme JoystickScheme
		{
			get
			{
				if (this.m_JoystickSchemeIndex == -1)
				{
					this.m_JoystickSchemeIndex = this.asset.FindControlSchemeIndex("Joystick");
				}
				return this.asset.controlSchemes[this.m_JoystickSchemeIndex];
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0004FAA0 File Offset: 0x0004DCA0
		public InputControlScheme XRScheme
		{
			get
			{
				if (this.m_XRSchemeIndex == -1)
				{
					this.m_XRSchemeIndex = this.asset.FindControlSchemeIndex("XR");
				}
				return this.asset.controlSchemes[this.m_XRSchemeIndex];
			}
		}

		// Token: 0x04000EDE RID: 3806
		private readonly InputActionMap m_Player;

		// Token: 0x04000EDF RID: 3807
		private List<InputActions.IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<InputActions.IPlayerActions>();

		// Token: 0x04000EE0 RID: 3808
		private readonly InputAction m_Player_MoveCamera;

		// Token: 0x04000EE1 RID: 3809
		private readonly InputAction m_Player_PanCamera;

		// Token: 0x04000EE2 RID: 3810
		private readonly InputAction m_Player_Zoom;

		// Token: 0x04000EE3 RID: 3811
		private readonly InputAction m_Player_Primary;

		// Token: 0x04000EE4 RID: 3812
		private readonly InputAction m_Player_Secondary;

		// Token: 0x04000EE5 RID: 3813
		private readonly InputAction m_Player_Pipette;

		// Token: 0x04000EE6 RID: 3814
		private readonly InputAction m_Player_Mouse;

		// Token: 0x04000EE7 RID: 3815
		private readonly InputAction m_Player_PrimaryModifier;

		// Token: 0x04000EE8 RID: 3816
		private readonly InputAction m_Player_SecondaryModifier;

		// Token: 0x04000EE9 RID: 3817
		private readonly InputAction m_Player_HotbarNumber;

		// Token: 0x04000EEA RID: 3818
		private readonly InputAction m_Player_HotbarSwitch;

		// Token: 0x04000EEB RID: 3819
		private readonly InputAction m_Player_FastCamera;

		// Token: 0x04000EEC RID: 3820
		private readonly InputAction m_Player_SlowCamera;

		// Token: 0x04000EED RID: 3821
		private readonly InputAction m_Player_OpenInventory;

		// Token: 0x04000EEE RID: 3822
		private readonly InputAction m_Player_OpenResearch;

		// Token: 0x04000EEF RID: 3823
		private readonly InputAction m_Player_ToggleMap;

		// Token: 0x04000EF0 RID: 3824
		private readonly InputAction m_Player_ToggleHUD;

		// Token: 0x04000EF1 RID: 3825
		private readonly InputAction m_Player_BuildMode;

		// Token: 0x04000EF2 RID: 3826
		private readonly InputAction m_Player_EditMode;

		// Token: 0x04000EF3 RID: 3827
		private readonly InputAction m_Player_CommandMode;

		// Token: 0x04000EF4 RID: 3828
		private readonly InputAction m_Player_DeleteMode;

		// Token: 0x04000EF5 RID: 3829
		private readonly InputAction m_Player_DeselectMode;

		// Token: 0x04000EF6 RID: 3830
		private readonly InputAction m_Player_CloseMenu;

		// Token: 0x04000EF7 RID: 3831
		private readonly InputAction m_Player_DeveloperTools;

		// Token: 0x04000EF8 RID: 3832
		private readonly InputActionMap m_UI;

		// Token: 0x04000EF9 RID: 3833
		private List<InputActions.IUIActions> m_UIActionsCallbackInterfaces = new List<InputActions.IUIActions>();

		// Token: 0x04000EFA RID: 3834
		private readonly InputAction m_UI_Navigate;

		// Token: 0x04000EFB RID: 3835
		private readonly InputAction m_UI_Submit;

		// Token: 0x04000EFC RID: 3836
		private readonly InputAction m_UI_Cancel;

		// Token: 0x04000EFD RID: 3837
		private readonly InputAction m_UI_Point;

		// Token: 0x04000EFE RID: 3838
		private readonly InputAction m_UI_Click;

		// Token: 0x04000EFF RID: 3839
		private readonly InputAction m_UI_ScrollWheel;

		// Token: 0x04000F00 RID: 3840
		private readonly InputAction m_UI_MiddleClick;

		// Token: 0x04000F01 RID: 3841
		private readonly InputAction m_UI_RightClick;

		// Token: 0x04000F02 RID: 3842
		private readonly InputAction m_UI_TrackedDevicePosition;

		// Token: 0x04000F03 RID: 3843
		private readonly InputAction m_UI_TrackedDeviceOrientation;

		// Token: 0x04000F04 RID: 3844
		private int m_KeyboardMouseSchemeIndex = -1;

		// Token: 0x04000F05 RID: 3845
		private int m_GamepadSchemeIndex = -1;

		// Token: 0x04000F06 RID: 3846
		private int m_TouchSchemeIndex = -1;

		// Token: 0x04000F07 RID: 3847
		private int m_JoystickSchemeIndex = -1;

		// Token: 0x04000F08 RID: 3848
		private int m_XRSchemeIndex = -1;

		// Token: 0x02000257 RID: 599
		public struct PlayerActions
		{
			// Token: 0x06001147 RID: 4423 RVA: 0x0004FAE5 File Offset: 0x0004DCE5
			public PlayerActions(InputActions wrapper)
			{
				this.m_Wrapper = wrapper;
			}

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x06001148 RID: 4424 RVA: 0x0004FAEE File Offset: 0x0004DCEE
			public InputAction MoveCamera
			{
				get
				{
					return this.m_Wrapper.m_Player_MoveCamera;
				}
			}

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x06001149 RID: 4425 RVA: 0x0004FAFB File Offset: 0x0004DCFB
			public InputAction PanCamera
			{
				get
				{
					return this.m_Wrapper.m_Player_PanCamera;
				}
			}

			// Token: 0x17000213 RID: 531
			// (get) Token: 0x0600114A RID: 4426 RVA: 0x0004FB08 File Offset: 0x0004DD08
			public InputAction Zoom
			{
				get
				{
					return this.m_Wrapper.m_Player_Zoom;
				}
			}

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x0600114B RID: 4427 RVA: 0x0004FB15 File Offset: 0x0004DD15
			public InputAction Primary
			{
				get
				{
					return this.m_Wrapper.m_Player_Primary;
				}
			}

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x0600114C RID: 4428 RVA: 0x0004FB22 File Offset: 0x0004DD22
			public InputAction Secondary
			{
				get
				{
					return this.m_Wrapper.m_Player_Secondary;
				}
			}

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x0600114D RID: 4429 RVA: 0x0004FB2F File Offset: 0x0004DD2F
			public InputAction Pipette
			{
				get
				{
					return this.m_Wrapper.m_Player_Pipette;
				}
			}

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x0600114E RID: 4430 RVA: 0x0004FB3C File Offset: 0x0004DD3C
			public InputAction Mouse
			{
				get
				{
					return this.m_Wrapper.m_Player_Mouse;
				}
			}

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x0600114F RID: 4431 RVA: 0x0004FB49 File Offset: 0x0004DD49
			public InputAction PrimaryModifier
			{
				get
				{
					return this.m_Wrapper.m_Player_PrimaryModifier;
				}
			}

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x06001150 RID: 4432 RVA: 0x0004FB56 File Offset: 0x0004DD56
			public InputAction SecondaryModifier
			{
				get
				{
					return this.m_Wrapper.m_Player_SecondaryModifier;
				}
			}

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06001151 RID: 4433 RVA: 0x0004FB63 File Offset: 0x0004DD63
			public InputAction HotbarNumber
			{
				get
				{
					return this.m_Wrapper.m_Player_HotbarNumber;
				}
			}

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06001152 RID: 4434 RVA: 0x0004FB70 File Offset: 0x0004DD70
			public InputAction HotbarSwitch
			{
				get
				{
					return this.m_Wrapper.m_Player_HotbarSwitch;
				}
			}

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x06001153 RID: 4435 RVA: 0x0004FB7D File Offset: 0x0004DD7D
			public InputAction FastCamera
			{
				get
				{
					return this.m_Wrapper.m_Player_FastCamera;
				}
			}

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x06001154 RID: 4436 RVA: 0x0004FB8A File Offset: 0x0004DD8A
			public InputAction SlowCamera
			{
				get
				{
					return this.m_Wrapper.m_Player_SlowCamera;
				}
			}

			// Token: 0x1700021E RID: 542
			// (get) Token: 0x06001155 RID: 4437 RVA: 0x0004FB97 File Offset: 0x0004DD97
			public InputAction OpenInventory
			{
				get
				{
					return this.m_Wrapper.m_Player_OpenInventory;
				}
			}

			// Token: 0x1700021F RID: 543
			// (get) Token: 0x06001156 RID: 4438 RVA: 0x0004FBA4 File Offset: 0x0004DDA4
			public InputAction OpenResearch
			{
				get
				{
					return this.m_Wrapper.m_Player_OpenResearch;
				}
			}

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x06001157 RID: 4439 RVA: 0x0004FBB1 File Offset: 0x0004DDB1
			public InputAction ToggleMap
			{
				get
				{
					return this.m_Wrapper.m_Player_ToggleMap;
				}
			}

			// Token: 0x17000221 RID: 545
			// (get) Token: 0x06001158 RID: 4440 RVA: 0x0004FBBE File Offset: 0x0004DDBE
			public InputAction ToggleHUD
			{
				get
				{
					return this.m_Wrapper.m_Player_ToggleHUD;
				}
			}

			// Token: 0x17000222 RID: 546
			// (get) Token: 0x06001159 RID: 4441 RVA: 0x0004FBCB File Offset: 0x0004DDCB
			public InputAction BuildMode
			{
				get
				{
					return this.m_Wrapper.m_Player_BuildMode;
				}
			}

			// Token: 0x17000223 RID: 547
			// (get) Token: 0x0600115A RID: 4442 RVA: 0x0004FBD8 File Offset: 0x0004DDD8
			public InputAction EditMode
			{
				get
				{
					return this.m_Wrapper.m_Player_EditMode;
				}
			}

			// Token: 0x17000224 RID: 548
			// (get) Token: 0x0600115B RID: 4443 RVA: 0x0004FBE5 File Offset: 0x0004DDE5
			public InputAction CommandMode
			{
				get
				{
					return this.m_Wrapper.m_Player_CommandMode;
				}
			}

			// Token: 0x17000225 RID: 549
			// (get) Token: 0x0600115C RID: 4444 RVA: 0x0004FBF2 File Offset: 0x0004DDF2
			public InputAction DeleteMode
			{
				get
				{
					return this.m_Wrapper.m_Player_DeleteMode;
				}
			}

			// Token: 0x17000226 RID: 550
			// (get) Token: 0x0600115D RID: 4445 RVA: 0x0004FBFF File Offset: 0x0004DDFF
			public InputAction DeselectMode
			{
				get
				{
					return this.m_Wrapper.m_Player_DeselectMode;
				}
			}

			// Token: 0x17000227 RID: 551
			// (get) Token: 0x0600115E RID: 4446 RVA: 0x0004FC0C File Offset: 0x0004DE0C
			public InputAction CloseMenu
			{
				get
				{
					return this.m_Wrapper.m_Player_CloseMenu;
				}
			}

			// Token: 0x17000228 RID: 552
			// (get) Token: 0x0600115F RID: 4447 RVA: 0x0004FC19 File Offset: 0x0004DE19
			public InputAction DeveloperTools
			{
				get
				{
					return this.m_Wrapper.m_Player_DeveloperTools;
				}
			}

			// Token: 0x06001160 RID: 4448 RVA: 0x0004FC26 File Offset: 0x0004DE26
			public InputActionMap Get()
			{
				return this.m_Wrapper.m_Player;
			}

			// Token: 0x06001161 RID: 4449 RVA: 0x0004FC33 File Offset: 0x0004DE33
			public void Enable()
			{
				this.Get().Enable();
			}

			// Token: 0x06001162 RID: 4450 RVA: 0x0004FC40 File Offset: 0x0004DE40
			public void Disable()
			{
				this.Get().Disable();
			}

			// Token: 0x17000229 RID: 553
			// (get) Token: 0x06001163 RID: 4451 RVA: 0x0004FC4D File Offset: 0x0004DE4D
			public bool enabled
			{
				get
				{
					return this.Get().enabled;
				}
			}

			// Token: 0x06001164 RID: 4452 RVA: 0x0004FC5A File Offset: 0x0004DE5A
			public static implicit operator InputActionMap(InputActions.PlayerActions set)
			{
				return set.Get();
			}

			// Token: 0x06001165 RID: 4453 RVA: 0x0004FC64 File Offset: 0x0004DE64
			public void AddCallbacks(InputActions.IPlayerActions instance)
			{
				if (instance == null || this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance))
				{
					return;
				}
				this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
				this.MoveCamera.started += instance.OnMoveCamera;
				this.MoveCamera.performed += instance.OnMoveCamera;
				this.MoveCamera.canceled += instance.OnMoveCamera;
				this.PanCamera.started += instance.OnPanCamera;
				this.PanCamera.performed += instance.OnPanCamera;
				this.PanCamera.canceled += instance.OnPanCamera;
				this.Zoom.started += instance.OnZoom;
				this.Zoom.performed += instance.OnZoom;
				this.Zoom.canceled += instance.OnZoom;
				this.Primary.started += instance.OnPrimary;
				this.Primary.performed += instance.OnPrimary;
				this.Primary.canceled += instance.OnPrimary;
				this.Secondary.started += instance.OnSecondary;
				this.Secondary.performed += instance.OnSecondary;
				this.Secondary.canceled += instance.OnSecondary;
				this.Pipette.started += instance.OnPipette;
				this.Pipette.performed += instance.OnPipette;
				this.Pipette.canceled += instance.OnPipette;
				this.Mouse.started += instance.OnMouse;
				this.Mouse.performed += instance.OnMouse;
				this.Mouse.canceled += instance.OnMouse;
				this.PrimaryModifier.started += instance.OnPrimaryModifier;
				this.PrimaryModifier.performed += instance.OnPrimaryModifier;
				this.PrimaryModifier.canceled += instance.OnPrimaryModifier;
				this.SecondaryModifier.started += instance.OnSecondaryModifier;
				this.SecondaryModifier.performed += instance.OnSecondaryModifier;
				this.SecondaryModifier.canceled += instance.OnSecondaryModifier;
				this.HotbarNumber.started += instance.OnHotbarNumber;
				this.HotbarNumber.performed += instance.OnHotbarNumber;
				this.HotbarNumber.canceled += instance.OnHotbarNumber;
				this.HotbarSwitch.started += instance.OnHotbarSwitch;
				this.HotbarSwitch.performed += instance.OnHotbarSwitch;
				this.HotbarSwitch.canceled += instance.OnHotbarSwitch;
				this.FastCamera.started += instance.OnFastCamera;
				this.FastCamera.performed += instance.OnFastCamera;
				this.FastCamera.canceled += instance.OnFastCamera;
				this.SlowCamera.started += instance.OnSlowCamera;
				this.SlowCamera.performed += instance.OnSlowCamera;
				this.SlowCamera.canceled += instance.OnSlowCamera;
				this.OpenInventory.started += instance.OnOpenInventory;
				this.OpenInventory.performed += instance.OnOpenInventory;
				this.OpenInventory.canceled += instance.OnOpenInventory;
				this.OpenResearch.started += instance.OnOpenResearch;
				this.OpenResearch.performed += instance.OnOpenResearch;
				this.OpenResearch.canceled += instance.OnOpenResearch;
				this.ToggleMap.started += instance.OnToggleMap;
				this.ToggleMap.performed += instance.OnToggleMap;
				this.ToggleMap.canceled += instance.OnToggleMap;
				this.ToggleHUD.started += instance.OnToggleHUD;
				this.ToggleHUD.performed += instance.OnToggleHUD;
				this.ToggleHUD.canceled += instance.OnToggleHUD;
				this.BuildMode.started += instance.OnBuildMode;
				this.BuildMode.performed += instance.OnBuildMode;
				this.BuildMode.canceled += instance.OnBuildMode;
				this.EditMode.started += instance.OnEditMode;
				this.EditMode.performed += instance.OnEditMode;
				this.EditMode.canceled += instance.OnEditMode;
				this.CommandMode.started += instance.OnCommandMode;
				this.CommandMode.performed += instance.OnCommandMode;
				this.CommandMode.canceled += instance.OnCommandMode;
				this.DeleteMode.started += instance.OnDeleteMode;
				this.DeleteMode.performed += instance.OnDeleteMode;
				this.DeleteMode.canceled += instance.OnDeleteMode;
				this.DeselectMode.started += instance.OnDeselectMode;
				this.DeselectMode.performed += instance.OnDeselectMode;
				this.DeselectMode.canceled += instance.OnDeselectMode;
				this.CloseMenu.started += instance.OnCloseMenu;
				this.CloseMenu.performed += instance.OnCloseMenu;
				this.CloseMenu.canceled += instance.OnCloseMenu;
				this.DeveloperTools.started += instance.OnDeveloperTools;
				this.DeveloperTools.performed += instance.OnDeveloperTools;
				this.DeveloperTools.canceled += instance.OnDeveloperTools;
			}

			// Token: 0x06001166 RID: 4454 RVA: 0x0005035C File Offset: 0x0004E55C
			private void UnregisterCallbacks(InputActions.IPlayerActions instance)
			{
				this.MoveCamera.started -= instance.OnMoveCamera;
				this.MoveCamera.performed -= instance.OnMoveCamera;
				this.MoveCamera.canceled -= instance.OnMoveCamera;
				this.PanCamera.started -= instance.OnPanCamera;
				this.PanCamera.performed -= instance.OnPanCamera;
				this.PanCamera.canceled -= instance.OnPanCamera;
				this.Zoom.started -= instance.OnZoom;
				this.Zoom.performed -= instance.OnZoom;
				this.Zoom.canceled -= instance.OnZoom;
				this.Primary.started -= instance.OnPrimary;
				this.Primary.performed -= instance.OnPrimary;
				this.Primary.canceled -= instance.OnPrimary;
				this.Secondary.started -= instance.OnSecondary;
				this.Secondary.performed -= instance.OnSecondary;
				this.Secondary.canceled -= instance.OnSecondary;
				this.Pipette.started -= instance.OnPipette;
				this.Pipette.performed -= instance.OnPipette;
				this.Pipette.canceled -= instance.OnPipette;
				this.Mouse.started -= instance.OnMouse;
				this.Mouse.performed -= instance.OnMouse;
				this.Mouse.canceled -= instance.OnMouse;
				this.PrimaryModifier.started -= instance.OnPrimaryModifier;
				this.PrimaryModifier.performed -= instance.OnPrimaryModifier;
				this.PrimaryModifier.canceled -= instance.OnPrimaryModifier;
				this.SecondaryModifier.started -= instance.OnSecondaryModifier;
				this.SecondaryModifier.performed -= instance.OnSecondaryModifier;
				this.SecondaryModifier.canceled -= instance.OnSecondaryModifier;
				this.HotbarNumber.started -= instance.OnHotbarNumber;
				this.HotbarNumber.performed -= instance.OnHotbarNumber;
				this.HotbarNumber.canceled -= instance.OnHotbarNumber;
				this.HotbarSwitch.started -= instance.OnHotbarSwitch;
				this.HotbarSwitch.performed -= instance.OnHotbarSwitch;
				this.HotbarSwitch.canceled -= instance.OnHotbarSwitch;
				this.FastCamera.started -= instance.OnFastCamera;
				this.FastCamera.performed -= instance.OnFastCamera;
				this.FastCamera.canceled -= instance.OnFastCamera;
				this.SlowCamera.started -= instance.OnSlowCamera;
				this.SlowCamera.performed -= instance.OnSlowCamera;
				this.SlowCamera.canceled -= instance.OnSlowCamera;
				this.OpenInventory.started -= instance.OnOpenInventory;
				this.OpenInventory.performed -= instance.OnOpenInventory;
				this.OpenInventory.canceled -= instance.OnOpenInventory;
				this.OpenResearch.started -= instance.OnOpenResearch;
				this.OpenResearch.performed -= instance.OnOpenResearch;
				this.OpenResearch.canceled -= instance.OnOpenResearch;
				this.ToggleMap.started -= instance.OnToggleMap;
				this.ToggleMap.performed -= instance.OnToggleMap;
				this.ToggleMap.canceled -= instance.OnToggleMap;
				this.ToggleHUD.started -= instance.OnToggleHUD;
				this.ToggleHUD.performed -= instance.OnToggleHUD;
				this.ToggleHUD.canceled -= instance.OnToggleHUD;
				this.BuildMode.started -= instance.OnBuildMode;
				this.BuildMode.performed -= instance.OnBuildMode;
				this.BuildMode.canceled -= instance.OnBuildMode;
				this.EditMode.started -= instance.OnEditMode;
				this.EditMode.performed -= instance.OnEditMode;
				this.EditMode.canceled -= instance.OnEditMode;
				this.CommandMode.started -= instance.OnCommandMode;
				this.CommandMode.performed -= instance.OnCommandMode;
				this.CommandMode.canceled -= instance.OnCommandMode;
				this.DeleteMode.started -= instance.OnDeleteMode;
				this.DeleteMode.performed -= instance.OnDeleteMode;
				this.DeleteMode.canceled -= instance.OnDeleteMode;
				this.DeselectMode.started -= instance.OnDeselectMode;
				this.DeselectMode.performed -= instance.OnDeselectMode;
				this.DeselectMode.canceled -= instance.OnDeselectMode;
				this.CloseMenu.started -= instance.OnCloseMenu;
				this.CloseMenu.performed -= instance.OnCloseMenu;
				this.CloseMenu.canceled -= instance.OnCloseMenu;
				this.DeveloperTools.started -= instance.OnDeveloperTools;
				this.DeveloperTools.performed -= instance.OnDeveloperTools;
				this.DeveloperTools.canceled -= instance.OnDeveloperTools;
			}

			// Token: 0x06001167 RID: 4455 RVA: 0x00050A29 File Offset: 0x0004EC29
			public void RemoveCallbacks(InputActions.IPlayerActions instance)
			{
				if (this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
				{
					this.UnregisterCallbacks(instance);
				}
			}

			// Token: 0x06001168 RID: 4456 RVA: 0x00050A48 File Offset: 0x0004EC48
			public void SetCallbacks(InputActions.IPlayerActions instance)
			{
				foreach (InputActions.IPlayerActions instance2 in this.m_Wrapper.m_PlayerActionsCallbackInterfaces)
				{
					this.UnregisterCallbacks(instance2);
				}
				this.m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
				this.AddCallbacks(instance);
			}

			// Token: 0x04000F09 RID: 3849
			private InputActions m_Wrapper;
		}

		// Token: 0x02000258 RID: 600
		public struct UIActions
		{
			// Token: 0x06001169 RID: 4457 RVA: 0x00050AB8 File Offset: 0x0004ECB8
			public UIActions(InputActions wrapper)
			{
				this.m_Wrapper = wrapper;
			}

			// Token: 0x1700022A RID: 554
			// (get) Token: 0x0600116A RID: 4458 RVA: 0x00050AC1 File Offset: 0x0004ECC1
			public InputAction Navigate
			{
				get
				{
					return this.m_Wrapper.m_UI_Navigate;
				}
			}

			// Token: 0x1700022B RID: 555
			// (get) Token: 0x0600116B RID: 4459 RVA: 0x00050ACE File Offset: 0x0004ECCE
			public InputAction Submit
			{
				get
				{
					return this.m_Wrapper.m_UI_Submit;
				}
			}

			// Token: 0x1700022C RID: 556
			// (get) Token: 0x0600116C RID: 4460 RVA: 0x00050ADB File Offset: 0x0004ECDB
			public InputAction Cancel
			{
				get
				{
					return this.m_Wrapper.m_UI_Cancel;
				}
			}

			// Token: 0x1700022D RID: 557
			// (get) Token: 0x0600116D RID: 4461 RVA: 0x00050AE8 File Offset: 0x0004ECE8
			public InputAction Point
			{
				get
				{
					return this.m_Wrapper.m_UI_Point;
				}
			}

			// Token: 0x1700022E RID: 558
			// (get) Token: 0x0600116E RID: 4462 RVA: 0x00050AF5 File Offset: 0x0004ECF5
			public InputAction Click
			{
				get
				{
					return this.m_Wrapper.m_UI_Click;
				}
			}

			// Token: 0x1700022F RID: 559
			// (get) Token: 0x0600116F RID: 4463 RVA: 0x00050B02 File Offset: 0x0004ED02
			public InputAction ScrollWheel
			{
				get
				{
					return this.m_Wrapper.m_UI_ScrollWheel;
				}
			}

			// Token: 0x17000230 RID: 560
			// (get) Token: 0x06001170 RID: 4464 RVA: 0x00050B0F File Offset: 0x0004ED0F
			public InputAction MiddleClick
			{
				get
				{
					return this.m_Wrapper.m_UI_MiddleClick;
				}
			}

			// Token: 0x17000231 RID: 561
			// (get) Token: 0x06001171 RID: 4465 RVA: 0x00050B1C File Offset: 0x0004ED1C
			public InputAction RightClick
			{
				get
				{
					return this.m_Wrapper.m_UI_RightClick;
				}
			}

			// Token: 0x17000232 RID: 562
			// (get) Token: 0x06001172 RID: 4466 RVA: 0x00050B29 File Offset: 0x0004ED29
			public InputAction TrackedDevicePosition
			{
				get
				{
					return this.m_Wrapper.m_UI_TrackedDevicePosition;
				}
			}

			// Token: 0x17000233 RID: 563
			// (get) Token: 0x06001173 RID: 4467 RVA: 0x00050B36 File Offset: 0x0004ED36
			public InputAction TrackedDeviceOrientation
			{
				get
				{
					return this.m_Wrapper.m_UI_TrackedDeviceOrientation;
				}
			}

			// Token: 0x06001174 RID: 4468 RVA: 0x00050B43 File Offset: 0x0004ED43
			public InputActionMap Get()
			{
				return this.m_Wrapper.m_UI;
			}

			// Token: 0x06001175 RID: 4469 RVA: 0x00050B50 File Offset: 0x0004ED50
			public void Enable()
			{
				this.Get().Enable();
			}

			// Token: 0x06001176 RID: 4470 RVA: 0x00050B5D File Offset: 0x0004ED5D
			public void Disable()
			{
				this.Get().Disable();
			}

			// Token: 0x17000234 RID: 564
			// (get) Token: 0x06001177 RID: 4471 RVA: 0x00050B6A File Offset: 0x0004ED6A
			public bool enabled
			{
				get
				{
					return this.Get().enabled;
				}
			}

			// Token: 0x06001178 RID: 4472 RVA: 0x00050B77 File Offset: 0x0004ED77
			public static implicit operator InputActionMap(InputActions.UIActions set)
			{
				return set.Get();
			}

			// Token: 0x06001179 RID: 4473 RVA: 0x00050B80 File Offset: 0x0004ED80
			public void AddCallbacks(InputActions.IUIActions instance)
			{
				if (instance == null || this.m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance))
				{
					return;
				}
				this.m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
				this.Navigate.started += instance.OnNavigate;
				this.Navigate.performed += instance.OnNavigate;
				this.Navigate.canceled += instance.OnNavigate;
				this.Submit.started += instance.OnSubmit;
				this.Submit.performed += instance.OnSubmit;
				this.Submit.canceled += instance.OnSubmit;
				this.Cancel.started += instance.OnCancel;
				this.Cancel.performed += instance.OnCancel;
				this.Cancel.canceled += instance.OnCancel;
				this.Point.started += instance.OnPoint;
				this.Point.performed += instance.OnPoint;
				this.Point.canceled += instance.OnPoint;
				this.Click.started += instance.OnClick;
				this.Click.performed += instance.OnClick;
				this.Click.canceled += instance.OnClick;
				this.ScrollWheel.started += instance.OnScrollWheel;
				this.ScrollWheel.performed += instance.OnScrollWheel;
				this.ScrollWheel.canceled += instance.OnScrollWheel;
				this.MiddleClick.started += instance.OnMiddleClick;
				this.MiddleClick.performed += instance.OnMiddleClick;
				this.MiddleClick.canceled += instance.OnMiddleClick;
				this.RightClick.started += instance.OnRightClick;
				this.RightClick.performed += instance.OnRightClick;
				this.RightClick.canceled += instance.OnRightClick;
				this.TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
				this.TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
				this.TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
				this.TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
			}

			// Token: 0x0600117A RID: 4474 RVA: 0x00050E88 File Offset: 0x0004F088
			private void UnregisterCallbacks(InputActions.IUIActions instance)
			{
				this.Navigate.started -= instance.OnNavigate;
				this.Navigate.performed -= instance.OnNavigate;
				this.Navigate.canceled -= instance.OnNavigate;
				this.Submit.started -= instance.OnSubmit;
				this.Submit.performed -= instance.OnSubmit;
				this.Submit.canceled -= instance.OnSubmit;
				this.Cancel.started -= instance.OnCancel;
				this.Cancel.performed -= instance.OnCancel;
				this.Cancel.canceled -= instance.OnCancel;
				this.Point.started -= instance.OnPoint;
				this.Point.performed -= instance.OnPoint;
				this.Point.canceled -= instance.OnPoint;
				this.Click.started -= instance.OnClick;
				this.Click.performed -= instance.OnClick;
				this.Click.canceled -= instance.OnClick;
				this.ScrollWheel.started -= instance.OnScrollWheel;
				this.ScrollWheel.performed -= instance.OnScrollWheel;
				this.ScrollWheel.canceled -= instance.OnScrollWheel;
				this.MiddleClick.started -= instance.OnMiddleClick;
				this.MiddleClick.performed -= instance.OnMiddleClick;
				this.MiddleClick.canceled -= instance.OnMiddleClick;
				this.RightClick.started -= instance.OnRightClick;
				this.RightClick.performed -= instance.OnRightClick;
				this.RightClick.canceled -= instance.OnRightClick;
				this.TrackedDevicePosition.started -= instance.OnTrackedDevicePosition;
				this.TrackedDevicePosition.performed -= instance.OnTrackedDevicePosition;
				this.TrackedDevicePosition.canceled -= instance.OnTrackedDevicePosition;
				this.TrackedDeviceOrientation.started -= instance.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.performed -= instance.OnTrackedDeviceOrientation;
				this.TrackedDeviceOrientation.canceled -= instance.OnTrackedDeviceOrientation;
			}

			// Token: 0x0600117B RID: 4475 RVA: 0x00051165 File Offset: 0x0004F365
			public void RemoveCallbacks(InputActions.IUIActions instance)
			{
				if (this.m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
				{
					this.UnregisterCallbacks(instance);
				}
			}

			// Token: 0x0600117C RID: 4476 RVA: 0x00051184 File Offset: 0x0004F384
			public void SetCallbacks(InputActions.IUIActions instance)
			{
				foreach (InputActions.IUIActions instance2 in this.m_Wrapper.m_UIActionsCallbackInterfaces)
				{
					this.UnregisterCallbacks(instance2);
				}
				this.m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
				this.AddCallbacks(instance);
			}

			// Token: 0x04000F0A RID: 3850
			private InputActions m_Wrapper;
		}

		// Token: 0x02000259 RID: 601
		public interface IPlayerActions
		{
			// Token: 0x0600117D RID: 4477
			void OnMoveCamera(InputAction.CallbackContext context);

			// Token: 0x0600117E RID: 4478
			void OnPanCamera(InputAction.CallbackContext context);

			// Token: 0x0600117F RID: 4479
			void OnZoom(InputAction.CallbackContext context);

			// Token: 0x06001180 RID: 4480
			void OnPrimary(InputAction.CallbackContext context);

			// Token: 0x06001181 RID: 4481
			void OnSecondary(InputAction.CallbackContext context);

			// Token: 0x06001182 RID: 4482
			void OnPipette(InputAction.CallbackContext context);

			// Token: 0x06001183 RID: 4483
			void OnMouse(InputAction.CallbackContext context);

			// Token: 0x06001184 RID: 4484
			void OnPrimaryModifier(InputAction.CallbackContext context);

			// Token: 0x06001185 RID: 4485
			void OnSecondaryModifier(InputAction.CallbackContext context);

			// Token: 0x06001186 RID: 4486
			void OnHotbarNumber(InputAction.CallbackContext context);

			// Token: 0x06001187 RID: 4487
			void OnHotbarSwitch(InputAction.CallbackContext context);

			// Token: 0x06001188 RID: 4488
			void OnFastCamera(InputAction.CallbackContext context);

			// Token: 0x06001189 RID: 4489
			void OnSlowCamera(InputAction.CallbackContext context);

			// Token: 0x0600118A RID: 4490
			void OnOpenInventory(InputAction.CallbackContext context);

			// Token: 0x0600118B RID: 4491
			void OnOpenResearch(InputAction.CallbackContext context);

			// Token: 0x0600118C RID: 4492
			void OnToggleMap(InputAction.CallbackContext context);

			// Token: 0x0600118D RID: 4493
			void OnToggleHUD(InputAction.CallbackContext context);

			// Token: 0x0600118E RID: 4494
			void OnBuildMode(InputAction.CallbackContext context);

			// Token: 0x0600118F RID: 4495
			void OnEditMode(InputAction.CallbackContext context);

			// Token: 0x06001190 RID: 4496
			void OnCommandMode(InputAction.CallbackContext context);

			// Token: 0x06001191 RID: 4497
			void OnDeleteMode(InputAction.CallbackContext context);

			// Token: 0x06001192 RID: 4498
			void OnDeselectMode(InputAction.CallbackContext context);

			// Token: 0x06001193 RID: 4499
			void OnCloseMenu(InputAction.CallbackContext context);

			// Token: 0x06001194 RID: 4500
			void OnDeveloperTools(InputAction.CallbackContext context);
		}

		// Token: 0x0200025A RID: 602
		public interface IUIActions
		{
			// Token: 0x06001195 RID: 4501
			void OnNavigate(InputAction.CallbackContext context);

			// Token: 0x06001196 RID: 4502
			void OnSubmit(InputAction.CallbackContext context);

			// Token: 0x06001197 RID: 4503
			void OnCancel(InputAction.CallbackContext context);

			// Token: 0x06001198 RID: 4504
			void OnPoint(InputAction.CallbackContext context);

			// Token: 0x06001199 RID: 4505
			void OnClick(InputAction.CallbackContext context);

			// Token: 0x0600119A RID: 4506
			void OnScrollWheel(InputAction.CallbackContext context);

			// Token: 0x0600119B RID: 4507
			void OnMiddleClick(InputAction.CallbackContext context);

			// Token: 0x0600119C RID: 4508
			void OnRightClick(InputAction.CallbackContext context);

			// Token: 0x0600119D RID: 4509
			void OnTrackedDevicePosition(InputAction.CallbackContext context);

			// Token: 0x0600119E RID: 4510
			void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
		}
	}
}
