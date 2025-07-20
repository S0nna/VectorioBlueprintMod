# Vectorio Community Blueprints

This branch is dedicated to sharing blueprints created by the Vectorio community!

## How to Share Your Blueprints

### 1. Create a Blueprint File
- Use the in-game blueprint copy tool (press `B` to start selection, then `Ctrl+C` to copy)
- Your blueprint will be saved to `in.txt` in the BepInEx folder
- The file will contain your blueprint data in the format:
  ```
  5x6;
  none, none, none, none, none;
  none, vec_collector, vec_collector, vec_collector, none;
  none, vec_collector, vec_collector, vec_collector, none;
  none, vec_collector, vec_collector, vec_collector, none;
  none, vec_collector, vec_collector, vec_collector, none;
  none, none, none, none, none;
  ```

### 2. Submit Your Blueprint
1. **Fork this repository**
2. **Create a new file** in the `blueprints/` folder with a descriptive name
3. **Copy your blueprint data** from `in.txt` into the new file
4. **Add a description** at the top of the file explaining what the blueprint does
5. **Submit a pull request** to this branch

### 3. File Naming Convention
Use descriptive names like:
- `collector_array_5x6.txt`
- `defense_turret_ring.txt`
- `resource_processing_chain.txt`
- `drone_logistics_hub.txt`

### 4. Blueprint Description Format
Start each blueprint file with a description:
```
# Collector Array 5x6
# Description: A simple 5x6 collector array with drone routing
# Author: YourName
# Date: 2024-01-15
# Tags: collectors, drones, basic

5x6;
none, none, none, none, none;
none, vec_collector, vec_collector, vec_collector, none;
...
```

## How to Use Community Blueprints

### Method 1: Console Mod (Recommended)
1. Install the BlueprintMod
2. Open the in-game console with `Shift + ~`
3. Copy the blueprint data from a file in this repository
4. Paste it directly into the console - it will auto-detect and save to `in.txt`
5. The blueprint will be automatically imported into your game

### Method 2: Manual Copy
1. Copy the blueprint data from a file in this repository
2. Paste it into `in.txt` in your BepInEx folder
3. The blueprint will be imported on the next game tick

## Blueprint Categories

### 🏭 Production
- Resource collectors and processors
- Manufacturing chains
- Power generation setups

### 🛡️ Defense
- Turret arrays
- Shield configurations
- Defensive walls and barriers

### 🚁 Logistics
- Drone routing networks
- Storage and distribution systems
- Transport hubs

### 🏗️ Infrastructure
- Base layouts
- Research facilities
- Utility buildings

## Contributing Guidelines

- **Test your blueprints** before sharing
- **Include clear descriptions** of what the blueprint does
- **Use appropriate tags** to help others find your blueprints
- **Keep file sizes reasonable** - split large blueprints if needed
- **Respect the community** - no offensive or inappropriate content

## Getting Help

- Join the Vectorio Discord for blueprint discussions
- Check the main branch for mod installation instructions
- Report issues with specific blueprints in the repository issues

---

**Happy blueprinting! 🎮** 