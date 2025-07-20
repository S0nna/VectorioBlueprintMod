using BepInEx;
using UnityEngine;

[BepInPlugin("vectorio.blueprintcopytool", "BlueprintCopyTool", "1.0.0")]
public class BlueprintCopyToolMain : BaseUnityPlugin
{
    void Awake()
    {
        Logger.LogInfo("[BlueprintCopyTool] Loaded and ready!");
        var go = new GameObject("BlueprintCopyRunner");
        var runner = go.AddComponent<BlueprintCopyRunner>();
        runner.Logger = this.Logger;
        DontDestroyOnLoad(go);
    }
}
