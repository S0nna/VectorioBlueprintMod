using BepInEx.Logging;
using System;
using System.IO;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    public ManualLogSource Logger { get; set; }
    
    private bool consoleVisible = false;
    private string inputText = "";
    private string outputText = "";
    private Vector2 scrollPosition;
    private string inFilePath;
    
    private GUIStyle consoleStyle;
    private GUIStyle inputStyle;
    private GUIStyle outputStyle;
    
    void Start()
    {
        // Set up the in.txt file path in the BepInEx directory
        inFilePath = Path.Combine(UnityEngine.Application.dataPath, "..", "BepInEx", "in.txt");
        Logger.LogInfo($"[ConsoleMod] Console initialized. in.txt path: {inFilePath}");
        
        // Load initial content from in.txt if it exists
        LoadInFile();
    }
    
    void Update()
    {
        // Toggle console with tilde key (Shift + backtick)
        if (Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftShift))
        {
            consoleVisible = !consoleVisible;
            if (consoleVisible)
            {
                Logger.LogInfo("[ConsoleMod] Console opened with Shift + backtick");
                inputText = "";
                // Pause the game when console opens
                Time.timeScale = 0f;
                // Load and display in.txt content when opening
                string content = LoadInFile();
                if (!string.IsNullOrEmpty(content))
                {
                    outputText = $"[{DateTime.Now:HH:mm:ss}] {content}\n\n";
                }
            }
            else
            {
                Logger.LogInfo("[ConsoleMod] Console closed with Shift + backtick");
                // Resume the game when console closes
                Time.timeScale = 1f;
            }
        }
    }
    
    void OnGUI()
    {
        if (!consoleVisible) return;
        
        // Initialize GUI styles if not already done
        if (consoleStyle == null)
        {
            InitializeStyles();
        }
        
        // Handle input events in OnGUI (works even when paused)
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.Return)
            {
                ProcessCommand();
                e.Use();
            }
            else if (e.keyCode == KeyCode.Escape)
            {
                consoleVisible = false;
                Time.timeScale = 1f; // Resume the game
                Logger.LogInfo("[ConsoleMod] Console closed with Escape key");
                e.Use();
            }
        }
        
        // Console background
        GUI.Box(new Rect(10, 10, UnityEngine.Screen.width - 20, UnityEngine.Screen.height - 20), "", consoleStyle);
        
        // Output area
        GUILayout.BeginArea(new Rect(20, 20, UnityEngine.Screen.width - 40, UnityEngine.Screen.height - 80));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label(outputText, outputStyle);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        
        // Input area
        GUILayout.BeginArea(new Rect(20, UnityEngine.Screen.height - 60, UnityEngine.Screen.width - 40, 40));
        GUI.SetNextControlName("ConsoleInput");
        inputText = GUILayout.TextField(inputText, inputStyle);
        GUILayout.EndArea();
        
        // Focus on input field when console opens
        if (GUI.GetNameOfFocusedControl() == "")
        {
            GUI.FocusControl("ConsoleInput");
        }
        
        // Add a close button for easier access
        if (GUI.Button(new Rect(UnityEngine.Screen.width - 50, 15, 35, 25), "X"))
        {
            consoleVisible = false;
            Time.timeScale = 1f; // Resume the game
        }
    }
    
    void InitializeStyles()
    {
        consoleStyle = new GUIStyle();
        consoleStyle.normal.background = MakeTexture(1, 1, new Color(0, 0, 0, 0.8f));
        consoleStyle.padding = new RectOffset(10, 10, 10, 10);
        
        inputStyle = new GUIStyle(GUI.skin.textField);
        inputStyle.normal.textColor = Color.white;
        inputStyle.fontSize = 14;
        
        outputStyle = new GUIStyle(GUI.skin.label);
        outputStyle.normal.textColor = Color.white;
        outputStyle.fontSize = 12;
        outputStyle.wordWrap = true;
    }
    
    Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
    
    void ProcessCommand()
    {
        if (string.IsNullOrWhiteSpace(inputText)) return;
        
        string command = inputText.Trim().ToLower();
        string response = "";
        
        try
        {
            if (command == "copy")
            {
                response = CopyToClipboard();
            }
            else if (command == "paste")
            {
                response = PasteFromClipboard();
            }
            else if (command == "exit")
            {
                consoleVisible = false;
                Time.timeScale = 1f; // Resume the game
                Logger.LogInfo("[ConsoleMod] Console closed with exit command");
                inputText = "";
                return;
            }
            else if (command == "help")
            {
                response = "Available commands:\n" +
                          "copy - Copy in.txt contents to clipboard\n" +
                          "paste - Paste clipboard contents to in.txt\n" +
                          "exit - Close console\n" +
                          "help - Show this help message\n\n" +
                          "You can also paste blueprint data directly - it will be saved to in.txt and console will close.";
            }
            else
            {
                // Check if the input looks like blueprint data
                if (IsBlueprintFormat(inputText))
                {
                    response = SaveBlueprintAndClose(inputText);
                }
                else
                {
                    response = $"Unknown command: {command}\nType 'help' for available commands.";
                }
            }
        }
        catch (Exception ex)
        {
            response = $"Error: {ex.Message}";
            Logger.LogError($"[ConsoleMod] Error processing command '{command}': {ex}");
        }
        
        if (!string.IsNullOrEmpty(response))
        {
            outputText += $"[{DateTime.Now:HH:mm:ss}] {response}\n\n";
        }
        
        inputText = "";
    }
    
    string LoadInFile()
    {
        try
        {
            if (File.Exists(inFilePath))
            {
                string content = File.ReadAllText(inFilePath);
                if (string.IsNullOrWhiteSpace(content))
                {
                    return "in.txt exists but is empty.";
                }
                return $"in.txt contents:\n{content}";
            }
            else
            {
                return "in.txt does not exist. Use 'write <text>' to create it.";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"[ConsoleMod] Error reading in.txt: {ex}");
            return $"Error reading in.txt: {ex.Message}";
        }
    }
    
    string CopyToClipboard()
    {
        try
        {
            if (File.Exists(inFilePath))
            {
                string content = File.ReadAllText(inFilePath);
                if (string.IsNullOrWhiteSpace(content))
                {
                    return "in.txt is empty, nothing to copy.";
                }
                
                // Copy to system clipboard using Unity's built-in method
                GUIUtility.systemCopyBuffer = content;
                return $"Copied {content.Length} characters to system clipboard.";
            }
            else
            {
                return "in.txt does not exist.";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"[ConsoleMod] Error copying to clipboard: {ex}");
            return $"Error copying to clipboard: {ex.Message}";
        }
    }
    
    string PasteFromClipboard()
    {
        try
        {
            string clipboardText = GUIUtility.systemCopyBuffer;
            if (string.IsNullOrWhiteSpace(clipboardText))
            {
                return "System clipboard is empty.";
            }
            
            // Check if clipboard content looks like blueprint data
            if (IsBlueprintFormat(clipboardText))
            {
                return SaveBlueprintAndClose(clipboardText);
            }
            
            File.WriteAllText(inFilePath, clipboardText);
            return $"Pasted {clipboardText.Length} characters to in.txt.";
        }
        catch (Exception ex)
        {
            Logger.LogError($"[ConsoleMod] Error pasting from clipboard: {ex}");
            return $"Error pasting from clipboard: {ex.Message}";
        }
    }
    
    bool IsBlueprintFormat(string text)
    {
        try
        {
            // Split by both newlines and carriage returns, but don't remove empty entries
            // since blueprint data might have empty lines
            string[] lines = text.Split(new[] { '\n', '\r' });
            if (lines.Length < 2) return false;
            
            // Find the first non-empty line
            string firstLine = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    firstLine = trimmed;
                    break;
                }
            }
            
            if (string.IsNullOrEmpty(firstLine)) return false;
            
            // Check first line for dimensions (e.g., "5x3" or "10x8")
            firstLine = firstLine.TrimEnd(new[] { ';' });
            if (!firstLine.Contains("x")) return false;
            
            string[] dims = firstLine.Split(new[] { 'x' });
            if (dims.Length != 2) return false;
            
            if (!int.TryParse(dims[0], out int width) || !int.TryParse(dims[1], out int height))
                return false;
            
            if (width <= 0 || height <= 0 || width > 100 || height > 100) return false;
            
            // Count non-empty lines that look like grid data
            int gridLines = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;
                
                // Skip lines that look like metadata (containing : or =)
                if (line.Contains(":") || line.Contains("=")) continue;
                
                // Check if line looks like grid data (contains commas)
                if (line.Contains(","))
                {
                    string cleanLine = line.TrimEnd(new[] { ';' });
                    string[] cells = cleanLine.Split(new[] { ',' });
                    
                    // Check if the line has roughly the right number of cells
                    if (cells.Length == width)
                    {
                        gridLines++;
                        
                        // Check if cells look like entity IDs (not empty, reasonable length)
                        foreach (string cell in cells)
                        {
                            string trimmed = cell.Trim();
                            if (string.IsNullOrEmpty(trimmed) || trimmed.Length > 50) return false;
                        }
                    }
                }
            }
            
            // We should have at least some grid lines that match the dimensions
            return gridLines > 0 && gridLines <= height + 5; // Allow some extra lines for metadata
        }
        catch
        {
            return false;
        }
    }
    
    string SaveBlueprintAndClose(string blueprintData)
    {
        try
        {
            File.WriteAllText(inFilePath, blueprintData);
            consoleVisible = false;
            Time.timeScale = 1f; // Resume the game
            Logger.LogInfo("[ConsoleMod] Blueprint detected and saved, console closed");
            return $"Blueprint detected! Saved {blueprintData.Length} characters to in.txt and closed console.";
        }
        catch (Exception ex)
        {
            Logger.LogError($"[ConsoleMod] Error saving blueprint: {ex}");
            return $"Error saving blueprint: {ex.Message}";
        }
    }
} 