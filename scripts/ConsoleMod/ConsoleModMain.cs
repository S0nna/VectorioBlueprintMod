using BepInEx;
using UnityEngine;

[BepInPlugin("vectorio.consolemod", "ConsoleMod", "1.0.0")]
public class ConsoleModMain : BaseUnityPlugin
{
    void Awake()
    {
        Logger.LogInfo("[ConsoleMod] Loaded and ready!");
        var go = new GameObject("ConsoleManager");
        var consoleManager = go.AddComponent<ConsoleManager>();
        consoleManager.Logger = this.Logger;
        DontDestroyOnLoad(go);
    }
} 