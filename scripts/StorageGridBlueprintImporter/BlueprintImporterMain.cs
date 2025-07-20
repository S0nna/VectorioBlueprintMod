using BepInEx;
using UnityEngine;

[BepInPlugin("vectorio.blueprintimporter", "StorageGridBlueprintImporter", "1.0.0")]
public class BlueprintImporterMain : BaseUnityPlugin
{
    void Awake()
    {
        Logger.LogInfo("[BlueprintImporter] Loaded");
        var go = new GameObject("StorageGridBlueprintRunner");
        var runner = go.AddComponent<StorageGridBlueprintRunner>();
        runner.Logger = Logger;
        DontDestroyOnLoad(go);
    }
}
