# BBModMenu

A comprehensive UI library for creating and managing mod settings in Béton Brutal. BBModMenu automatically integrates into the game's interface and provides a full-featured settings menu with persistent configuration storage.

## Description

BBModMenu is a modding library built on Unity's UI Toolkit that enables mod developers to create professional, user-friendly settings interfaces. It handles all the complexity of UI creation, layout management, and persistent storage, allowing developers to focus on their mod's functionality.

The library provides a complete set of UI components including sliders, toggles, buttons, carousels, and hotkey bindings. All settings are automatically saved and loaded through BBSettings, eliminating the need for manual configuration management.

## Technologies

- **Unity UI Toolkit** - Modern declarative UI framework using VisualElements
- **MelonLoader** - Mod loader framework for Unity games
- **BBSettings** - Persistent configuration storage system
- **C# Reflection** - Dynamic access to game UI components

## Installation

### Prerequisites

- Béton Brutal game installed
- [MelonLoader](https://github.com/LavaGang/MelonLoader) installed and configured

### Steps

1. Download the latest release from the [releases page](https://github.com/MiaouZart/BBModMenu/releases)
2. Extract the DLL file to your `BETON BRUTAL\Mods` folder
3. Launch the game - BBModMenu will automatically initialize
4. Access the Mod Menu through the game's settings interface

### Configuration Storage

All mod settings are automatically saved to:
```
BETON BRUTAL\UserData\MelonPreferences.cfg
```

No manual configuration required - settings persist automatically between game sessions.

## Features

### Core Functionality

- **Automatic Integration** - Seamlessly adds a Mod Menu screen to the game's existing UI
- **Persistent Settings** - All configurations automatically saved and restored via BBSettings
- **Category Organization** - Group related settings into logical categories
- **Flexible Layout System** - Groups and wrappers for precise UI control
- **Multiple Input Types** - Comprehensive set of UI components for different input needs

### Available UI Components

- **Sliders** - Numeric value selection with optional integer-only mode
- **Toggles** - Boolean on/off switches
- **Buttons** - Custom action triggers
- **Carousels** - Multi-option selection with arrow navigation
- **HotKeys** - Keyboard shortcut binding with modifier key support (Ctrl, Shift, Alt)
- **Labels** - Text display for descriptions and section headers
- **Groups** - Container elements for organizing related controls
- **Wrappers** - Layout containers for individual control pairs

### Utility Functions

- **Hotkey Detection** - Check if key combinations are pressed or held
- **Automatic Persistence** - No manual save/load implementation required
- **Event Callbacks** - React to setting changes in real-time

## Usage Guide

### Basic Setup

First, obtain a reference to the ModMenu instance:
```csharp
GameObject gameUI = GameObject.Find("GameUI");
GameUI _gameUI = gameUI.GetComponent<GameUI>();
List<UIScreen> screens = typeof(GameUI)?.GetField("screens", BindingFlags.NonPublic | BindingFlags.Instance)
    ?.GetValue(_gameUI) as List<UIScreen>;

ModMenu _modMenu = screens?.FirstOrDefault(screen => screen is ModMenu) as ModMenu;
if (_modMenu is null)
{
    Debug.Log("ModMenu not found");
    return;
}
```

### Creating a Settings Category

Categories serve as namespaces for organizing settings in both the UI and config file:
```csharp
string categoryName = "MyMod";
var myModSettings = _modMenu.AddSetting(categoryName);
```

### Adding UI Components

#### Slider with Integer Values
```csharp
var intensitySlider = _modMenu.CreateSlider(categoryName, "Intensity", 0, 100, 50, true);
intensitySlider.RegisterValueChangedCallback(evt => {
    // Handle value change
    UpdateIntensity(evt.newValue);
});
```

#### Toggle Switch
```csharp
var enableToggle = _modMenu.CreateToggle(categoryName, "Enabled", true);
enableToggle.RegisterValueChangedCallback(evt => {
    // Handle toggle state
    SetFeatureEnabled(evt.newValue);
});
```

#### Action Button
```csharp
var resetButton = _modMenu.CreateButton("Reset to Defaults");
resetButton.clicked += () => {
    // Perform reset action
    ResetAllSettings();
};
```

#### Carousel Selection
```csharp
var qualityOptions = new List<string>() { "Low", "Medium", "High", "Ultra" };
var qualityCarousel = _modMenu.CreateCarousel(
    categoryName, 
    "Quality", 
    qualityOptions, 
    newValue => {
        // Handle selection change
        SetQuality(newValue);
    }, 
    "Medium"
);
```

#### Hotkey Binding
```csharp
var toggleKey = _modMenu.CreateHotKey(categoryName, "ToggleKey", KeyCode.F);
toggleKey.OnChanged += newKey => {
    Debug.Log($"Hotkey changed to: {newKey}");
};

// In Update loop:
if (Utils.IsHotkeyPressed(toggleKey.Value)) {
    ToggleFeature();
}
```

### Organizing Layout

Structure your settings interface using groups and wrappers:
```csharp
// Create a group for related settings
var visualGroup = _modMenu.CreateGroup("Visual Settings");

// Create wrapper for brightness slider
var brightnessWrapper = _modMenu.CreateWrapper();
brightnessWrapper.Add(_modMenu.CreateLabel("Brightness"));
brightnessWrapper.Add(brightnessSlider);

// Create wrapper for contrast slider
var contrastWrapper = _modMenu.CreateWrapper();
contrastWrapper.Add(_modMenu.CreateLabel("Contrast"));
contrastWrapper.Add(contrastSlider);

// Add wrappers to group
visualGroup.Add(brightnessWrapper);
visualGroup.Add(contrastWrapper);

// Add group to category
myModSettings.Add(visualGroup);
```

## API Reference

### ModMenu Methods

#### `VisualElement AddSetting(string setting)`
Creates a new category section in the Mod Menu. Returns a container for adding UI elements.

#### `VisualElement CreateGroup(string groupName)`
Creates a group container for organizing related settings.

#### `VisualElement CreateWrapper()`
Creates a styled wrapper for single control layouts (label + input pair).

#### `VisualElement CreateLabel(string text)`
Creates a standard text label.

#### `VisualElement CreateTitleLabel(string text)`
Creates a large, prominent title label for section headers.

#### `VisualElement CreateButton(string text)`
Creates a clickable button element.

#### `VisualElement CreateSlider(string category, string name, float min, float max, float defaultValue = 0, bool onlyInt = false)`
Creates a slider control with automatic persistence.

**Parameters:**
- `category` - Settings category for persistence
- `name` - Display name and config key
- `min` - Minimum value
- `max` - Maximum value
- `defaultValue` - Initial value
- `onlyInt` - Restrict to integer values only

#### `VisualElement CreateToggle(string category, string name, bool defaultValue = false)`
Creates a toggle checkbox with automatic persistence.

**Parameters:**
- `category` - Settings category for persistence
- `name` - Display name and config key
- `defaultValue` - Initial state

#### `CarouselEntry CreateCarousel(string category, string name, List<string> options, Action<string> onValueChanged = null, string defaultValue = "")`
Creates a carousel selector with arrow navigation.

**Parameters:**
- `category` - Settings category for persistence
- `name` - Display name and config key
- `options` - List of selectable values
- `onValueChanged` - Callback when selection changes
- `defaultValue` - Initially selected value

#### `HotKeyEntry CreateHotKey(string category, string name, KeyCode defaultKey)`
Creates a hotkey binding control with modifier support.

**Parameters:**
- `category` - Settings category for persistence
- `name` - Display name and config key
- `defaultKey` - Initial key binding

### Utils Class

Static utility class providing helper structures and functions.

#### `Utils.HotKeyEntry`

Represents a hotkey settings entry.

**Properties:**
- `VisualElement Root` - UI root element
- `string Value` - Current key combination string (e.g., "Ctrl+F")
- `Action<string> OnChanged` - Callback invoked on key change

#### `Utils.CarouselEntry`

Represents a carousel settings entry.

**Properties:**
- `VisualElement Root` - UI root element
- `string Value` - Currently selected value

#### `bool Utils.IsHotkeyPressed(string combo)`

Checks if a key combination was pressed this frame. Supports modifiers: "Ctrl+", "Shift+", "Alt+".

**Returns:** `true` if the combination is pressed, otherwise `false`

**Example:**
```csharp
if (Utils.IsHotkeyPressed("Ctrl+Shift+F")) {
    Debug.Log("Hotkey triggered");
}
```

#### `bool Utils.IsHotkeyHeld(string combo)`

Checks if a key combination is currently held down. Uses the same modifier syntax as `IsHotkeyPressed`.

## Complete Example

Here is a complete example implementing a flashlight mod with color customization and toggle hotkey:
```csharp
using static BBModMenu.Utils;

public class FlashlightMod : MelonMod {
    private ModMenu _modMenu;
    private HotKeyEntry _toggleKey;
    private Flashlight _flashlight;
    
    public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
        // Get ModMenu reference
        GameObject gameUI = GameObject.Find("GameUI");
        GameUI _gameUI = gameUI.GetComponent<GameUI>();
        List<UIScreen> screens = typeof(GameUI)?.GetField("screens", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(_gameUI) as List<UIScreen>;
        _modMenu = screens?.FirstOrDefault(screen => screen is ModMenu) as ModMenu;
        
        if (_modMenu == null) return;
        
        // Create settings category
        string categoryName = "Flashlight";
        var flashlightSettings = _modMenu.AddSetting(categoryName);
        
        // Create color sliders group
        var colorGroup = _modMenu.CreateGroup("Color Settings");
        
        var rSlider = _modMenu.CreateSlider(categoryName, "Red", 0, 255, 255, true);
        var gSlider = _modMenu.CreateSlider(categoryName, "Green", 0, 255, 255, true);
        var bSlider = _modMenu.CreateSlider(categoryName, "Blue", 0, 255, 255, true);
        
        rSlider.RegisterValueChangedCallback(evt => UpdateColor());
        gSlider.RegisterValueChangedCallback(evt => UpdateColor());
        bSlider.RegisterValueChangedCallback(evt => UpdateColor());
        
        // Layout color sliders
        var rWrapper = _modMenu.CreateWrapper();
        rWrapper.Add(_modMenu.CreateLabel("Red"));
        rWrapper.Add(rSlider);
        
        var gWrapper = _modMenu.CreateWrapper();
        gWrapper.Add(_modMenu.CreateLabel("Green"));
        gWrapper.Add(gSlider);
        
        var bWrapper = _modMenu.CreateWrapper();
        bWrapper.Add(_modMenu.CreateLabel("Blue"));
        bWrapper.Add(bSlider);
        
        colorGroup.Add(rWrapper);
        colorGroup.Add(gWrapper);
        colorGroup.Add(bWrapper);
        
        // Create toggle hotkey
        _toggleKey = _modMenu.CreateHotKey(categoryName, "ToggleKey", KeyCode.F);
        _toggleKey.OnChanged += newKey => {
            MelonLogger.Msg($"Toggle key changed to: {newKey}");
        };
        
        var keyWrapper = _modMenu.CreateWrapper();
        keyWrapper.Add(_modMenu.CreateLabel("Toggle Hotkey"));
        keyWrapper.Add(_toggleKey.Root);
        
        // Add all to settings
        flashlightSettings.Add(colorGroup);
        flashlightSettings.Add(keyWrapper);
    }
    
    public override void OnUpdate() {
        if (Utils.IsHotkeyPressed(_toggleKey.Value)) {
            _flashlight.enabled = !_flashlight.enabled;
            MelonLogger.Msg("Flashlight toggled");
        }
    }
    
    private void UpdateColor() {
        // Apply color changes to flashlight
    }
}
```

## Contributing

Contributions are welcome. Please submit issues and pull requests on the GitHub repository.

## License

This project is provided as-is for use with Béton Brutal mods. Please respect the game's terms of service when creating and distributing mods.
