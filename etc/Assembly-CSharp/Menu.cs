using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000155 RID: 341
[DefaultExecutionOrder(10)]
public class Menu : MonoBehaviour
{
	// Token: 0x06000B15 RID: 2837 RVA: 0x0002F598 File Offset: 0x0002D798
	public static bool IsOnline()
	{
		return Menu._isOnline;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0002F59F File Offset: 0x0002D79F
	public void ToggleOnline(bool toggle)
	{
		Menu._isOnline = toggle;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0002F5A8 File Offset: 0x0002D7A8
	public void Start()
	{
		Menu._isOnline = false;
		this._cameraBound = this.cameraSize - 0.05f;
		this._cameraStart = this.camera.orthographicSize;
		LeanTween.alphaCanvas(this.serversGroup, 1f, 1f);
		this._contactingServers = true;
		InputManager.OnBackActionPressed.AddListener(new UnityAction(this.CloseActiveWindow));
		InputManager.OnHudActionPressed.AddListener(new UnityAction(this.Toggle));
		Singleton<AuthEvents>.Instance.onAuthenticationSuccessful.AddListener(new UnityAction<string>(this.LoginFinished));
		Singleton<AuthEvents>.Instance.onAuthenticationFailed.AddListener(new UnityAction<string>(this.LoginFinished));
		Singleton<Events>.Instance.onMusicVolumeChanged.AddListener(new UnityAction(this.UpdateMusicVolume));
		this.camera.orthographicSize = 35f;
		Singleton<global::VolumeManager>.Instance.SetDepthOfField(0.6f);
		if (SaveSystem.IS_EXPERIMENTAL)
		{
			this.version.text = "Experimental v" + Application.version;
		}
		else
		{
			this.version.text = "Steam Version v" + Application.version;
		}
		Singleton<AudioPlayer>.Instance.SetSpatialBlending(1f);
		this.UpdateMusicVolume();
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0002F6EC File Offset: 0x0002D8EC
	public void Toggle()
	{
		this._toggle = !this._toggle;
		this.canvasGroup.alpha = (this._toggle ? 1f : 0f);
		this.canvasGroup.blocksRaycasts = this._toggle;
		this.canvasGroup.interactable = this._toggle;
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0002F749 File Offset: 0x0002D949
	public void UpdateMusicVolume()
	{
		this.musicSource.volume = Singleton<Settings>.Instance.GetMusicVolume();
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0002F760 File Offset: 0x0002D960
	public void Update()
	{
		if (this._contactingServers)
		{
			if (this._serverDelay <= 0f)
			{
				if (!this._loginStarted)
				{
					this._maxServerConnectionTime = 10f;
					this._loginStarted = true;
					Authenticator.Login();
				}
			}
			else
			{
				this._serverDelay -= Time.deltaTime;
			}
			if (this._loginStarted)
			{
				if (this._maxServerConnectionTime <= 0f)
				{
					this._maxServerConnectionTime = 10f;
					this.LoginFinished("TIMED OUT");
					return;
				}
				this._maxServerConnectionTime -= Time.deltaTime;
				return;
			}
		}
		else if (this._runAnim)
		{
			if (this._cameraTime < 1f)
			{
				this._cameraTime += Time.deltaTime * this.cameraSpeed;
				if (this.camera.orthographicSize <= this._cameraBound)
				{
					this.camera.orthographicSize = Mathf.SmoothStep(this._cameraStart, this.cameraSize, this._cameraTime);
				}
			}
			if (this._depthTime < 1f)
			{
				this._depthTime += Time.deltaTime * this.volumeSpeed;
				Singleton<global::VolumeManager>.Instance.SetDepthOfField(Mathf.Lerp(0.6f, 1f, this._depthTime));
			}
		}
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
	public void LoginFinished(string response)
	{
		LeanTween.alphaCanvas(this.serversGroup, 0f, 0.25f);
		this._contactingServers = false;
		if (Authenticator.UserAuthenticated)
		{
			if (this.displayEarlyAccessWarning)
			{
				LeanTween.alphaCanvas(this.earlyAccessWarningGroup, 1f, 0.25f).setDelay(0.25f);
				this.earlyAccessWarningGroup.interactable = true;
				this.earlyAccessWarningGroup.blocksRaycasts = true;
				return;
			}
			this.EnableMain();
			return;
		}
		else
		{
			if (!Authenticator.PiratedCopy)
			{
				this.responseText.text = response;
				LeanTween.alphaCanvas(this.loginGroup, 1f, 0.25f).setDelay(0.25f);
				this.loginGroup.interactable = true;
				this.loginGroup.blocksRaycasts = true;
				return;
			}
			LeanTween.alphaCanvas(this.piratedGroup, 1f, 0.25f).setDelay(0.25f);
			this.piratedGroup.interactable = true;
			this.piratedGroup.blocksRaycasts = true;
			return;
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0002F9A5 File Offset: 0x0002DBA5
	public void ConfirmBeta()
	{
		LeanTween.alphaCanvas(this.earlyAccessWarningGroup, 0f, 0.25f);
		this.earlyAccessWarningGroup.interactable = false;
		this.earlyAccessWarningGroup.blocksRaycasts = false;
		this.EnableMain();
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0002F9DB File Offset: 0x0002DBDB
	public void ConfirmOffline()
	{
		LeanTween.alphaCanvas(this.loginGroup, 0f, 0.25f);
		this.loginGroup.interactable = false;
		this.loginGroup.blocksRaycasts = false;
		this.EnableMain();
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0002FA11 File Offset: 0x0002DC11
	public void ConfirmPirated()
	{
		LeanTween.alphaCanvas(this.piratedGroup, 0f, 0.25f);
		this.piratedGroup.interactable = false;
		this.piratedGroup.blocksRaycasts = false;
		this.EnableMain();
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0002FA48 File Offset: 0x0002DC48
	public void EnableMain()
	{
		this.loadingIcon.SetActive(false);
		this.menuScene.startPlacing = true;
		LeanTween.alphaCanvas(this.loginGroup, 0f, this.mainButtonSpeed);
		this.loginGroup.interactable = false;
		this.loginGroup.blocksRaycasts = false;
		float num = this.mainButtonSpeed;
		foreach (MenuButton menuButton in this.mainButtons)
		{
			menuButton.group.GetComponent<RectTransform>().localPosition = menuButton.inPos;
			if (menuButton.sound != null)
			{
				Singleton<AudioPlayer>.Instance.PlayDelayedInterfaceSound(menuButton.sound, num);
			}
			menuButton.group.alpha = 0f;
			LeanTween.alphaCanvas(menuButton.group, 1f, this.mainButtonSpeed).setDelay(num);
			LeanTween.moveLocal(menuButton.group.gameObject, menuButton.normalPos, this.mainButtonSpeed).setEase(LeanTweenType.easeOutExpo).setDelay(num += this.mainButtonDelay);
		}
		this.mainGroup.alpha = 1f;
		this.mainGroup.interactable = true;
		this.mainGroup.blocksRaycasts = true;
		if (DevTools.FORCE_LOCAL_CONNECTION)
		{
			this.connectLocalButton.alpha = 1f;
			this.connectLocalButton.interactable = true;
			this.connectLocalButton.blocksRaycasts = true;
		}
		this._runAnim = true;
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0002FBE8 File Offset: 0x0002DDE8
	public void CloseActiveWindow()
	{
		if (this._settingsOpen)
		{
			this.CloseSettings();
			return;
		}
		if (this._newWorldOpen)
		{
			this.CloseNewWorldAndOpenSaves();
			return;
		}
		if (this._savesOpen)
		{
			this.CloseSavesAndOpenGamemodes();
			return;
		}
		if (this._gamemodesOpen)
		{
			this.CloseGamemodes();
			return;
		}
		if (this._lobbiesOpen)
		{
			this.CloseLobbiesAndOpenMultiplayer();
			return;
		}
		if (this._multiplayerOpen)
		{
			this.CloseMultiplayer();
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0002FC50 File Offset: 0x0002DE50
	public void OpenSettings()
	{
		this.settingsPanel.transform.localPosition = this.upPosition;
		this.ToggleGroup(this.settingsPanel, this.settingsBackground, true, 0f);
		LeanTween.alphaCanvas(this.settingsPanel, 1f, this.mainButtonSpeed);
		LeanTween.moveLocal(this.settingsPanel.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo();
		this._settingsOpen = true;
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0002FCD4 File Offset: 0x0002DED4
	public void CloseSettings()
	{
		this.ToggleGroup(this.settingsPanel, this.settingsBackground, false, 0f);
		this.settingsPanel.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.settingsPanel.gameObject, this.upPosition, this.mainButtonSpeed).setEaseInExpo();
		this._settingsOpen = false;
		Singleton<Settings>.Instance.UpdateSettings();
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0002FD4C File Offset: 0x0002DF4C
	public void OpenGamemodes()
	{
		this.gamemodeGroup.transform.localPosition = this.upPosition;
		this.ToggleGroup(this.gamemodeGroup, this.gamemodeBackground, true, 0f);
		LeanTween.alphaCanvas(this.gamemodeGroup, 1f, this.mainButtonSpeed);
		LeanTween.moveLocal(this.gamemodeGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo();
		this._gamemodesOpen = true;
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0002FDD0 File Offset: 0x0002DFD0
	public void OpenMultiplayer()
	{
		this.multiplayerGroup.transform.localPosition = this.upPosition;
		this.ToggleGroup(this.multiplayerGroup, this.multiplayerBackground, true, 0f);
		LeanTween.alphaCanvas(this.multiplayerGroup, 1f, this.mainButtonSpeed);
		LeanTween.moveLocal(this.multiplayerGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo();
		this._multiplayerOpen = true;
		Menu._isOnline = true;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0002FE5C File Offset: 0x0002E05C
	public void CloseGamemodesAndOpenSaves(string gamemodeID)
	{
		this.ToggleGroup(this.gamemodeGroup, this.gamemodeBackground, false, 0f);
		this.gamemodeGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.gamemodeGroup.gameObject, this.downPosition, this.mainButtonSpeed).setEaseInExpo();
		this._gamemodesOpen = false;
		this.ToggleGroup(this.savesGroup, this.savesBackground, true, this.windowDelay);
		this.savesGroup.transform.localPosition = this.upPosition;
		LeanTween.moveLocal(this.savesGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._savesOpen = true;
		this._gamemodeData = Library.RequestData<GamemodeData>(gamemodeID);
		if (this._gamemodeData != null)
		{
			this.savesList.GenerateList(this._gamemodeData);
			return;
		}
		Debug.Log("[MENU] A gamemode with ID " + gamemodeID + " could not be found!");
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0002FF78 File Offset: 0x0002E178
	public void CloseSavesAndOpenNewWorld()
	{
		this.ToggleGroup(this.savesGroup, this.savesBackground, false, 0f);
		this.savesGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.savesGroup.gameObject, this.downPosition, this.mainButtonSpeed).setEaseInExpo();
		this._savesOpen = false;
		this.ToggleGroup(this.newWorldGroup, this.newWorldBackground, true, this.windowDelay);
		this.newWorldGroup.transform.localPosition = this.upPosition;
		LeanTween.moveLocal(this.newWorldGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._newWorldOpen = true;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x00030054 File Offset: 0x0002E254
	public void CloseNewWorldAndOpenSaves()
	{
		this.ToggleGroup(this.newWorldGroup, this.newWorldBackground, false, 0f);
		this.savesGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.savesGroup.gameObject, this.upPosition, this.mainButtonSpeed).setEaseInExpo();
		this._newWorldOpen = false;
		this.ToggleGroup(this.savesGroup, this.savesBackground, true, this.windowDelay);
		this.savesGroup.transform.localPosition = this.downPosition;
		LeanTween.moveLocal(this.savesGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._savesOpen = true;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00030130 File Offset: 0x0002E330
	public void CloseSavesAndOpenGamemodes()
	{
		this.ToggleGroup(this.savesGroup, this.savesBackground, false, 0f);
		this.savesGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.savesGroup.gameObject, this.upPosition, this.mainButtonSpeed).setEaseInExpo();
		this._newWorldOpen = false;
		this.ToggleGroup(this.gamemodeGroup, this.gamemodeBackground, true, this.windowDelay);
		this.gamemodeGroup.transform.localPosition = this.downPosition;
		LeanTween.moveLocal(this.gamemodeGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._gamemodesOpen = true;
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0003020C File Offset: 0x0002E40C
	public void CloseGamemodes()
	{
		this.ToggleGroup(this.gamemodeGroup, this.gamemodeBackground, false, 0f);
		this.gamemodeGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.gamemodeGroup.gameObject, this.upPosition, this.mainButtonSpeed).setEaseInExpo();
		this._gamemodesOpen = false;
		if (Menu._isOnline)
		{
			this.ToggleGroup(this.multiplayerGroup, this.multiplayerBackground, true, this.windowDelay);
			this.multiplayerGroup.transform.localPosition = this.downPosition;
			LeanTween.moveLocal(this.multiplayerGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
			this._multiplayerOpen = true;
		}
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x000302EC File Offset: 0x0002E4EC
	public void CloseMultiplayerAndOpenGamemodes()
	{
		this.ToggleGroup(this.multiplayerGroup, this.multiplayerBackground, false, 0f);
		this.multiplayerGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.multiplayerGroup.gameObject, this.downPosition, this.mainButtonSpeed).setEaseInExpo();
		this._multiplayerOpen = false;
		this.ToggleGroup(this.gamemodeGroup, this.gamemodeBackground, true, this.windowDelay);
		this.gamemodeGroup.transform.localPosition = this.upPosition;
		LeanTween.moveLocal(this.gamemodeGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._gamemodesOpen = true;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x000303C8 File Offset: 0x0002E5C8
	public void CloseMultiplayerAndOpenLobbies()
	{
		this.ToggleGroup(this.multiplayerGroup, this.multiplayerBackground, false, 0f);
		this.multiplayerGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.multiplayerGroup.gameObject, this.downPosition, this.mainButtonSpeed).setEaseInExpo();
		this._multiplayerOpen = false;
		this.ToggleGroup(this.lobbyGroup, this.lobbyBackground, true, this.windowDelay);
		this.lobbyGroup.transform.localPosition = this.upPosition;
		LeanTween.moveLocal(this.lobbyGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._lobbiesOpen = true;
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x000304A4 File Offset: 0x0002E6A4
	public void CloseLobbiesAndOpenMultiplayer()
	{
		this.ToggleGroup(this.lobbyGroup, this.lobbyBackground, false, 0f);
		this.lobbyGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.lobbyGroup.gameObject, this.upPosition, this.mainButtonSpeed).setEaseInExpo();
		this._lobbiesOpen = false;
		this.ToggleGroup(this.multiplayerGroup, this.multiplayerBackground, true, this.windowDelay);
		this.multiplayerGroup.transform.localPosition = this.downPosition;
		LeanTween.moveLocal(this.multiplayerGroup.gameObject, Vector2.zero, this.mainButtonSpeed).setEaseOutExpo().setDelay(this.windowDelay);
		this._multiplayerOpen = true;
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00030580 File Offset: 0x0002E780
	public void CloseMultiplayer()
	{
		this.ToggleGroup(this.multiplayerGroup, this.multiplayerBackground, false, 0f);
		this.multiplayerGroup.transform.localPosition = Vector2.zero;
		LeanTween.moveLocal(this.multiplayerGroup.gameObject, this.upPosition, this.mainButtonSpeed).setEaseInExpo();
		this._multiplayerOpen = false;
		Menu._isOnline = false;
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x000305F3 File Offset: 0x0002E7F3
	public void OpenLink(string link)
	{
		Application.OpenURL(link);
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x000305FC File Offset: 0x0002E7FC
	public void ToggleCredits(bool toggle)
	{
		if (toggle)
		{
			LeanTween.alphaCanvas(this.mainGroup, 0f, this.mainButtonSpeed);
			this.mainGroup.interactable = false;
			this.mainGroup.blocksRaycasts = false;
			LeanTween.alphaCanvas(this.creditsGroup, 1f, this.mainButtonSpeed).setDelay(this.mainButtonSpeed);
			this.creditsGroup.interactable = true;
			this.creditsGroup.blocksRaycasts = true;
			return;
		}
		LeanTween.alphaCanvas(this.creditsGroup, 0f, this.mainButtonSpeed);
		this.creditsGroup.interactable = false;
		this.creditsGroup.blocksRaycasts = false;
		LeanTween.alphaCanvas(this.mainGroup, 1f, this.mainButtonSpeed).setDelay(this.mainButtonSpeed);
		this.mainGroup.interactable = true;
		this.mainGroup.blocksRaycasts = true;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x000306E0 File Offset: 0x0002E8E0
	private void ToggleGroup(CanvasGroup panel, CanvasGroup background, bool toggle, float delay = 0f)
	{
		background.interactable = toggle;
		background.blocksRaycasts = toggle;
		panel.interactable = toggle;
		panel.blocksRaycasts = toggle;
		if (toggle)
		{
			if (delay > 0f)
			{
				LeanTween.alphaCanvas(background, 1f, this.mainButtonSpeed);
				LeanTween.alphaCanvas(panel, 1f, this.mainButtonSpeed).setDelay(delay);
				return;
			}
			LeanTween.alphaCanvas(background, 1f, this.mainButtonSpeed);
			LeanTween.alphaCanvas(panel, 1f, this.mainButtonSpeed);
			return;
		}
		else
		{
			if (delay > 0f)
			{
				LeanTween.alphaCanvas(background, 0f, this.mainButtonSpeed);
				LeanTween.alphaCanvas(panel, 0f, this.mainButtonSpeed).setDelay(delay);
				return;
			}
			LeanTween.alphaCanvas(background, 0f, this.mainButtonSpeed);
			LeanTween.alphaCanvas(panel, 0f, this.mainButtonSpeed);
			return;
		}
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x000307BF File Offset: 0x0002E9BF
	public void QuitGame()
	{
		Application.Quit();
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x000307C8 File Offset: 0x0002E9C8
	public void EnableMainScreen()
	{
		foreach (MenuButton menuButton in this.mainButtons)
		{
			menuButton.group.alpha = 1f;
			menuButton.group.transform.localPosition = menuButton.normalPos;
		}
		this.loginGroup.alpha = 0f;
		this.loginGroup.interactable = false;
		this.loginGroup.blocksRaycasts = false;
		this.mainGroup.alpha = 1f;
		this.mainGroup.interactable = true;
		this.mainGroup.blocksRaycasts = true;
		this.camera.orthographicSize = this.cameraSize;
		Singleton<global::VolumeManager>.Instance.SetDepthOfField(1f);
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x000308B0 File Offset: 0x0002EAB0
	public void DisableMainScreen()
	{
		foreach (MenuButton menuButton in this.mainButtons)
		{
			menuButton.group.alpha = 0f;
			menuButton.group.transform.localPosition = menuButton.inPos;
		}
		this.loginGroup.alpha = 0f;
		this.loginGroup.interactable = false;
		this.loginGroup.blocksRaycasts = false;
		this.mainGroup.alpha = 1f;
		this.mainGroup.interactable = false;
		this.mainGroup.blocksRaycasts = false;
		this.camera.orthographicSize = 30f;
		Singleton<global::VolumeManager>.Instance.SetDepthOfField(0.6f);
	}

	// Token: 0x04000762 RID: 1890
	public CanvasGroup canvasGroup;

	// Token: 0x04000763 RID: 1891
	public CanvasGroup serversGroup;

	// Token: 0x04000764 RID: 1892
	public CanvasGroup loginGroup;

	// Token: 0x04000765 RID: 1893
	public CanvasGroup piratedGroup;

	// Token: 0x04000766 RID: 1894
	public CanvasGroup earlyAccessWarningGroup;

	// Token: 0x04000767 RID: 1895
	public TextMeshProUGUI responseText;

	// Token: 0x04000768 RID: 1896
	protected float _serverDelay = 1f;

	// Token: 0x04000769 RID: 1897
	protected float _maxServerConnectionTime = 10f;

	// Token: 0x0400076A RID: 1898
	protected bool _loginStarted;

	// Token: 0x0400076B RID: 1899
	protected bool _contactingServers = true;

	// Token: 0x0400076C RID: 1900
	public GameObject loadingIcon;

	// Token: 0x0400076D RID: 1901
	public bool displayEarlyAccessWarning;

	// Token: 0x0400076E RID: 1902
	public MenuScene menuScene;

	// Token: 0x0400076F RID: 1903
	public List<MenuButton> mainButtons;

	// Token: 0x04000770 RID: 1904
	public CanvasGroup mainGroup;

	// Token: 0x04000771 RID: 1905
	public CanvasGroup creditsGroup;

	// Token: 0x04000772 RID: 1906
	public CanvasGroup connectLocalButton;

	// Token: 0x04000773 RID: 1907
	public TextMeshProUGUI version;

	// Token: 0x04000774 RID: 1908
	public float mainButtonSpeed;

	// Token: 0x04000775 RID: 1909
	public float mainButtonDelay;

	// Token: 0x04000776 RID: 1910
	public float windowDelay = 0.5f;

	// Token: 0x04000777 RID: 1911
	public VolumeProfile volume;

	// Token: 0x04000778 RID: 1912
	public Camera camera;

	// Token: 0x04000779 RID: 1913
	public float volumeSpeed;

	// Token: 0x0400077A RID: 1914
	public float cameraSpeed;

	// Token: 0x0400077B RID: 1915
	public Vector2 upPosition;

	// Token: 0x0400077C RID: 1916
	public Vector2 downPosition;

	// Token: 0x0400077D RID: 1917
	protected float _dotValue = 0.6f;

	// Token: 0x0400077E RID: 1918
	protected float _cameraTime;

	// Token: 0x0400077F RID: 1919
	protected float _depthTime;

	// Token: 0x04000780 RID: 1920
	protected bool _runAnim;

	// Token: 0x04000781 RID: 1921
	public float cameraSize = 45f;

	// Token: 0x04000782 RID: 1922
	protected float _cameraBound;

	// Token: 0x04000783 RID: 1923
	protected float _cameraStart;

	// Token: 0x04000784 RID: 1924
	public CanvasGroup gamemodeGroup;

	// Token: 0x04000785 RID: 1925
	public CanvasGroup gamemodeBackground;

	// Token: 0x04000786 RID: 1926
	protected bool _gamemodesOpen;

	// Token: 0x04000787 RID: 1927
	public DemoMenu demoMenu;

	// Token: 0x04000788 RID: 1928
	public CanvasGroup multiplayerGroup;

	// Token: 0x04000789 RID: 1929
	public CanvasGroup multiplayerBackground;

	// Token: 0x0400078A RID: 1930
	protected bool _multiplayerOpen;

	// Token: 0x0400078B RID: 1931
	protected static bool _isOnline;

	// Token: 0x0400078C RID: 1932
	public CanvasGroup lobbyGroup;

	// Token: 0x0400078D RID: 1933
	public CanvasGroup lobbyBackground;

	// Token: 0x0400078E RID: 1934
	protected bool _lobbiesOpen;

	// Token: 0x0400078F RID: 1935
	public CanvasGroup savesGroup;

	// Token: 0x04000790 RID: 1936
	public CanvasGroup savesBackground;

	// Token: 0x04000791 RID: 1937
	public SaveList savesList;

	// Token: 0x04000792 RID: 1938
	public GameObject experimentalSavesObj;

	// Token: 0x04000793 RID: 1939
	protected bool _savesOpen;

	// Token: 0x04000794 RID: 1940
	public CanvasGroup newWorldGroup;

	// Token: 0x04000795 RID: 1941
	public CanvasGroup newWorldBackground;

	// Token: 0x04000796 RID: 1942
	public NewWorldPanel newWorldPanel;

	// Token: 0x04000797 RID: 1943
	protected bool _newWorldOpen;

	// Token: 0x04000798 RID: 1944
	public float newWorldDelay = 0.25f;

	// Token: 0x04000799 RID: 1945
	public AudioSource musicSource;

	// Token: 0x0400079A RID: 1946
	private GamemodeData _gamemodeData;

	// Token: 0x0400079B RID: 1947
	private bool _toggle = true;

	// Token: 0x0400079C RID: 1948
	public CanvasGroup settingsPanel;

	// Token: 0x0400079D RID: 1949
	public CanvasGroup settingsBackground;

	// Token: 0x0400079E RID: 1950
	private bool _settingsOpen;

	// Token: 0x02000156 RID: 342
	public enum ReleaseMode
	{
		// Token: 0x040007A0 RID: 1952
		None,
		// Token: 0x040007A1 RID: 1953
		EarlyAccess,
		// Token: 0x040007A2 RID: 1954
		Experimental,
		// Token: 0x040007A3 RID: 1955
		Offline,
		// Token: 0x040007A4 RID: 1956
		Pirated
	}
}
