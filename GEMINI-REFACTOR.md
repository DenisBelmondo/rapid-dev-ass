# Gemini Refactor: Project Folder Structure Analysis

This document provides an analysis of the current project folder structure and a recommended new structure to improve organization, scalability, and clarity.

## Analysis of Current Structure

The current folder structure has a good foundation but suffers from a few common organizational issues that can lead to confusion and difficulty in locating assets.

### What's Good

*   **Top-level directories:** The use of distinct top-level folders like `Assets`, `Scripts`, `Prefabs`, and `Scenes` is a good start.
*   **Script organization:** The `Assets/Scripts` folder is well-organized by feature (`Player`, `UI`, `Managers`), which is an excellent practice.

### Areas for Improvement

1.  **Redundant Folders:** There are multiple `Prefabs` folders (`Assets/Prefabs` and `Assets/Resources/Prefabs`). This creates ambiguity about where to save and find prefabs.
2.  **Overuse of the `Resources` Folder:** The `Resources` folder is used to store many assets (prefabs, materials, textures, tiles). The `Resources` folder is a special Unity folder that bundles any assets inside it directly into the final build, whether they are used or not. Assets in this folder are loaded by a string path (`Resources.Load("Path/To/Asset")`), which is slow and error-prone. The modern and recommended approach is to use direct references by assigning assets in the Inspector.
3.  **Root Asset Clutter:** Important assets like the `InputSystem_Actions` are located in the root `Assets` folder, where they can get lost among other folders.
4.  **Lack of Project Separation:** All project assets are mixed directly in the `Assets` folder alongside third-party packages (like TextMesh Pro). This can make it difficult to manage and upgrade packages without accidentally modifying your own game's assets.

## Recommended Folder Structure

Here is a more robust and scalable folder structure. The primary goal is to separate the game's own assets from third-party assets and to group files by feature and type in a more logical way.

```
Assets/
├── _Project/                  # All your game-specific assets go here.
│   ├── Art/
│   │   ├── Materials/
│   │   ├── Sprites/
│   │   └── VFX/
│   ├── Audio/
│   │   ├── Music/
│   │   └── SFX/
│   ├── Input/
│   │   └── InputSystem_Actions.inputactions
│   ├── Prefabs/
│   │   ├── Managers/
│   │   ├── Player/
│   │   ├── Pylon/
│   │   └── UI/
│   ├── Scenes/
│   │   ├── Main.unity
│   │   └── MainMenu.unity
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Game/
│   │   ├── Managers/
│   │   ├── Player/
│   │   ├── UI/
│   │   └── World/                 # For Pylon.cs, FogOfWarManager.cs, etc.
│   ├── Settings/
│   │   ├── Render/
│   │   └── SceneTemplates/
│   └── Tilemaps/
│       ├── Palettes/
│       └── Tiles/
│
├── Editor/                      # For custom editor scripts.
│
├── Resources/                   # Should be empty or used very sparingly.
│
└── ThirdParty/
    └── TextMesh Pro/
```

### Justification for Changes

*   **`_Project` Folder:** Placing all your game's assets in a single parent folder (prefixed with `_` to keep it at the top of the list) is a powerful organizational pattern. It instantly separates your code and assets from imported packages, preventing confusion and making package updates safer.

*   **Consolidated `Art`, `Prefabs`, `Tilemaps`:** All assets of a certain type now have a single, clear home. The `Prefabs` folder is subdivided by feature, mirroring the `Scripts` folder, which makes it intuitive to find the prefab for a specific script.

*   **Dedicated `Input` Folder:** The input action assets are moved out of the root and into a logical, easy-to-find location.

*   **Emptying the `Resources` Folder:** By moving assets like prefabs, tiles, and materials out of `Resources`, you are encouraged to use direct Inspector references. This is more efficient at runtime, safer (no magic strings), and ensures that only assets actually used in your scenes are included in the build.

*   **`ThirdParty` Folder:** Moving external packages like TextMesh Pro here cleans up the root `Assets` directory and clearly delineates what is external code versus your own.

### Action Plan

1.  Create the new folder structure within the `Assets` directory.
2.  Carefully move existing assets from their old locations to the new ones.
3.  Unity will likely lose some script and asset references on prefabs and scene objects during the move. You will need to go through your prefabs (`Player`, `Pylon`, `UI Canvas`, etc.) and scenes to re-assign any missing scripts or material references.
4.  Update any code that uses `Resources.Load()` to use direct references instead.