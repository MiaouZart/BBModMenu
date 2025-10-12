# BBModMenu – Mod Settings UI Library

## Features

* Automatically integrates into the game’s `GameUI` and adds a new **Mod Menu** screen.
* Supports **persistent mod settings** via `BBSettings` (automatically saved/loaded).
* Provides helper methods to create:

    * Categories for organizing mod settings.
    * Wrappers & groups for layout control.
    * Labels & title labels for section headers.
    * Sliders (with optional integer-only mode).
    * Toggles (on/off checkboxes).
    * Buttons for custom actions.
    * Carousels and HotKeys.
* Built on Unity’s **UI Toolkit** (`VisualElement`, `Slider`, `Toggle`, etc.).

---

## Getting Started

### 1. Access the Mod Menu

First, locate the `ModMenu` instance from the game’s UI:

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

---

### 2. Define a Category

A **category** is used as a namespace for saving settings inside the config file:

```csharp
string categoryName = "Flashlight";
var flashlightSettings = _modMenu.AddSetting(categoryName);
```

This creates a new **settings section** in the menu and a persistent entry in the configuration.

---

### 3. Create UI Elements

#### Slider

```csharp
var rSlider = _modMenu.CreateSlider(categoryName, "Red", 0, 255, 255, true);
rSlider.RegisterValueChangedCallback(evt => updateColor());
```

Parameters:

1. `categoryName` – Category for saving the value.
2. `"Red"` – Display name of the slider.
3. `0` – Minimum value.
4. `255` – Maximum value.
5. `255` – Default value.
6. `true` – Force integer-only values.

#### Toggle

```csharp
var toggle = _modMenu.CreateToggle(categoryName, "On", true);

toggle.RegisterValueChangedCallback(delegate(ChangeEvent<bool> b) {
    _flashlight.flashlight.enabled = b.newValue;
});
```

Parameters:

1. `categoryName` – Category for saving the value.
2. `"On"` – Settings name for the toggle.
3. `false` - Default value.

#### Button

```csharp
var resetBtn = _modMenu.CreateButton("Reset");
resetBtn.clicked += ResetTimer; // function call when clicked
resetWrapper.Add(resetBtn);
```

Parameters:

1. `Reset` – Text display on the button.

#### Carousel

```csharp
var carouselData = new List<string>() {
    "Item1","Item3","Item4","Item5","Item6"
};
var carouselValue = "";

void OnChanged(string newValue) { // Function called when the value changes
    carouselValue = newValue;
    MelonLogger.Msg("OnChanged: " + newValue);
}
var Carousel = _modMenu.CreateCarousel(categoryName,"carou", carouselData,OnChanged,"item3");
var visualElemntOfCarousel = Carousel.Root;
carouselValue = Carousel.Value;
carWrapper.Add(visualElemntOfCarousel);
```

Parameters:

1. `categoryName` – Category for saving the value.
2. `"carou"` – Display name in the settings.
3. `carouselData` – List of string.
4. `OnChanged` – Function called when value changes.
5. `"item3"` – Default value.

---

#### HotKey

```csharp
var key = _modMenu.CreateHotKey(categoryName, "key", KeyCode.F);
var visualElemntOfKey = key.Root;
activateKey = key.Value;
key.OnChanged += newKey => {
    MelonLogger.Msg($"New key(s) : {newKey}");
    activateKey = newKey;
};
settingsGroup.Add(visualElemntOfKey);
```

Parameters:

1. `categoryName` – Category for saving the value.
2. `"key"` – Display name in the settings.
3. ` KeyCode.F` – Default key.

---

### 4. Organize Layout

Use **groups** and **wrappers** to structure the settings UI:

```csharp
// Create a group for sliders
var sliderGroup = _modMenu.CreateGroup("Sliders");

// Red channel
var rWrapper = _modMenu.CreateWrapper();
rWrapper.Add(_modMenu.CreateLabel("Red"));
rWrapper.Add(rSlider);

// Green channel
var gWrapper = _modMenu.CreateWrapper();
gWrapper.Add(_modMenu.CreateLabel("Green"));
gWrapper.Add(gSlider);

// Blue channel
var bWrapper = _modMenu.CreateWrapper();
bWrapper.Add(_modMenu.CreateLabel("Blue"));
bWrapper.Add(bSlider);

// Add to the group
sliderGroup.Add(rWrapper);
sliderGroup.Add(gWrapper);
sliderGroup.Add(bWrapper);
```

---

## 📖 API Reference

### `VisualElement AddSetting(string setting)`

Creates a new category/section in the Mod Menu.
Returns a `VisualElement` group container.

---

### `VisualElement CreateGroup(string groupName)`

Creates a container (`VisualElement`) for grouping related settings.

---

### `VisualElement CreateWrapper()`

Creates a styled container (`VisualElement`) for a single control (label + slider, etc.).

---

### `VisualElement CreateLabel(string text)`

Creates a simple text label.

---

### `VisualElement CreateTitleLabel(string text)`

Creates a large, styled title label.

---

### `VisualElement CreateButton(string text)`

Creates a clickable button.

---

###
`VisualElement CreateSlider(string category, string name, float min, float max, float defaultValue = 0, bool onlyInt = false)`

Creates a slider linked to a saved setting.

---

### `VisualElement CreateToggle(string category, string name, bool defaultValue = false)`

Creates a toggle checkbox linked to a saved setting.

---

###
`CarouselEntry CreateCarousel(string category, string name, List<string> card,  Action<string> onValueChanged = null, string defaultValue = "")`

Creates a carousel option with arrows to navigate between values in a list.

---

### `HotKeyEntry CreateHotKey(string category, string name, KeyCode defaultKey)`

Creates a hotkey setting that supports multi-key combos (`Ctrl+`, `Shift+`, `Alt+`).

---

## Utils Class

### `class BBModMenu.Utils`

A static helper class that provides utility structures and functions for working with hotkeys and carousels.

---

#### **Nested Classes**

##### `Utils.HotKeyEntry`

Represents a Mod Menu hotkey entry.

| Property    | Type             | Description                                                                   |
|-------------|------------------|-------------------------------------------------------------------------------|
| `Root`      | `VisualElement`  | The UI root element of the hotkey entry.                                      |
| `Value`     | `string`         | The string value representing the current key combination (e.g., `"Ctrl+F"`). |
| `OnChanged` | `Action<string>` | Callback invoked when the hotkey value is changed.                            |

---

##### `Utils.CarouselEntry`

Represents a Mod Menu carousel entry.

| Property | Type            | Description                                   |
|----------|-----------------|-----------------------------------------------|
| `Root`   | `VisualElement` | The UI root element of the carousel.          |
| `Value`  | `string`        | The currently selected value in the carousel. |

---

#### **Static Methods**

##### `bool Utils.IsHotkeyPressed(string combo)`



Checks if a given key combination string (e.g. `"Ctrl+Shift+F"`) was pressed during the current frame.


##### `bool Utils.IsHotkeyHeld(string combo)`

Checks if a given key combination string (e.g. `"Ctrl+Shift+F"`) was held.

**Parameters:**

* `combo`: The key combination string, supporting optional modifiers `"Ctrl+"`, `"Shift+"`, `"Alt+"`.

**Returns:**
`true` if the combination is pressed this frame, otherwise `false`.

**Example:**

```csharp
if (Utils.IsHotkeyPressed("Ctrl+Shift+F")) {
    Debug.Log("Hotkey pressed!");
}
```

---

## Persistence

All settings created with sliders, toggles, or other components are automatically saved and loaded using `BBSettings`.
No extra serialization logic is required — values are stored per **category + entry name**.

---

## 📌 Example: Flashlight Settings

```csharp
using static BBModMenu.Utils; // import for IsHotkeyPressed(string)
HotKeyEntry _hotKeyEntry;        
string categoryName = "Flashlight";
var flashlightSettings = _modMenu.AddSetting(categoryName);

var sliderGroup = _modMenu.CreateGroup("Sliders");

var rSlider = _modMenu.CreateSlider(categoryName, "Red", 0, 255, 255, true);
rSlider.RegisterValueChangedCallback(evt => updateColor());
 
_hotKeyEntry = _modMenu.CreateHotKey(categoryName, "key", KeyCode.F);
var visualElemntOfKey = _hotKeyEntry.Root;
activateKey = _hotKeyEntry.Value;
key.OnChanged += newKey =>
{
    MelonLogger.Msg($"New assigned key : {newKey}");
};

// Organize layout
var rWrapper = _modMenu.CreateWrapper();
rWrapper.Add(_modMenu.CreateLabel("Red"));
rWrapper.Add(rSlider);

sliderGroup.Add(rWrapper);
flashlightSettings.Add(sliderGroup);

// In the update loop:
private void Update() {
    if (IsHotkeyPressed(key.Value)) {
        MelonLogger.Msg("Activate Key pressed!");
        _flashlight.flashlight.enabled = !_flashlight.flashlight.enabled;
    } 
}
```
