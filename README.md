# BlueprintMod

BlueprintMod is a collection of mods for Vectorio Remake, built using BepInEx and Unity. It provides enhanced blueprinting tools for players, making it easier to copy, place, and manage blueprints in-game.

## Features

### BlueprintCopyTool
- Copy and save blueprints from the game world
- Efficient blueprint placement system
- In-game UI integration for blueprint management

### StorageGridBlueprintImporter
- Import and manage storage grid blueprints
- Drone route blueprint data handling
- Advanced blueprint import functionality

## Project Structure
```
BlueprintMod/
├── scripts/
│   ├── StorageGridBlueprintCopyTool/
│   │   ├── BlueprintCopyRunner.cs
│   │   └── BlueprintCopyToolMain.cs
│   └── StorageGridBlueprintImporter/
│       ├── BlueprintImporterMain.cs
│       ├── DroneRouteBlueprintData.cs
│       └── StorageGridBlueprintRunner.cs
├── BlueprintMod.csproj
├── BlueprintMod.sln
├── .gitignore
├── LICENSE
└── README.md
```

## Installation

### Prerequisites
- Vectorio Remake (Steam)
- BepInEx mod loader installed
- .NET Framework 4.8

### Steps
1. Build the project using Visual Studio or your preferred C# IDE
2. Copy the generated `BlueprintMod.dll` from `bin/Release/` or `bin/Debug/`
3. Place the DLL into your `BepInEx/plugins` folder in your Vectorio Remake installation directory
4. Launch the game. The mods will load automatically

## Development

### Requirements
- Visual Studio 2019 or later
- .NET Framework 4.8
- Vectorio Remake game files for references
- BepInEx framework

### Building
1. Clone the repository
2. Open `BlueprintMod.sln` in Visual Studio
3. Ensure the project references point to your Vectorio Remake installation
4. Build the solution (Debug or Release configuration)

### Dependencies
- **Assembly-CSharp.dll** - Vectorio Remake game assembly
- **BepInEx.dll** - Mod loader framework
- **UnityEngine.dll** - Unity engine core
- Various Unity modules (Audio, Grid, IMGUI, Input, Tilemap)

## Usage

### BlueprintCopyTool
- Use in-game hotkeys or UI to copy blueprints
- Access blueprint management through the mod's interface

### StorageGridBlueprintImporter
- Import storage grid configurations
- Manage drone route blueprints
- Configure import settings through the mod interface

## Configuration

### Mod Configuration
Both mods support configuration through BepInEx's config system. Check the `BepInEx/config/` folder for generated configuration files.

## Troubleshooting

### Mod Issues
- Ensure BepInEx is properly installed and configured
- Check the BepInEx log file for any error messages
- Verify all required Unity modules are available
- Make sure the mod DLL is placed in the correct plugins folder

## Security Notes
- Don't commit sensitive configuration files to version control
- The `.gitignore` file is configured to exclude sensitive files and build artifacts

## License
This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Contributing
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly with Vectorio Remake
5. Submit a pull request

## Version History
- **1.0.0** - Initial release with BlueprintCopyTool and StorageGridBlueprintImporter 