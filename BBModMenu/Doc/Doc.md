# BBModMenu – Mod Settings UI Library

BBModMenu is a Unity UI Toolkit–based library that extends the game’s **Mod Menu** to allow developers to easily add, display, and persist settings for their mods.  
---

## Features

- Automatically integrates into the game’s `GameUI` and adds a new **Mod Menu** screen.
- Supports **persistent mod settings** via `BBSettings` (automatically saved/loaded).
- Provides helper methods to create:
  - Categories for organizing mod settings.
  - Wrappers & groups for layout control.
  - Labels & title labels for section headers.
  - Sliders (with optional integer-only mode).
  - Toggles (on/off checkboxes).
  - Buttons for custom actions.
- Built on Unity’s **UI Toolkit** (`VisualElement`, `Slider`, `Toggle`, etc.).

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

####  Toggle
```csharp
var toggle = _modMenu.CreateToggle(categoryName, "On", true);

toggle.RegisterValueChangedCallback(delegate(ChangeEvent<bool> b) {
    _flashlight.flashlight.enabled = b.newValue;
});
```

#### Button
```csharp
var resetBtn = _modMenu.CreateButton("Reset");
resetWrapper.Add(resetBtn);
```



#### Carousel 
```csharp
Dictionary<string, string> difficultyOptions = new Dictionary<string, string>
{
    { "easy", "Easy" },
    { "medium", "Medium" },
    { "hard", "Hard" }
};

var carousel = _modMenu.CreateCarousel("Gameplay", difficultyOptions, "Difficulty", (val) =>
{
    Debug.Log("Difficulty changed to " + val);
}, "Medium");

settingsGroup.Add(carousel);

```

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

### `AddSetting(string setting)`
Creates a new category/section in the Mod Menu.  
Returns a `VisualElement` group container.

---

### `CreateGroup(string groupName)`
Creates a container (`VisualElement`) for grouping related settings.

---

### `CreateWrapper()`
Creates a styled container (`VisualElement`) for a single control (label + slider, etc.).

---

### `CreateLabel(string text)`
Creates a simple text label.

---

### `CreateTitleLabel(string text)`
Creates a large, styled title label.

---

### `CreateButton(string text)`
Creates a clickable button.

---

### `CreateSlider(string category, string name, float min, float max, float defaultValue = 0, bool onlyInt = false)`
Creates a slider linked to a saved setting.

---

### `CreateToggle(string category, string name, bool defaultValue = false)`
Creates a toggle checkbox linked to a saved setting.

---

##  Persistence

All settings created with sliders, toggles, or other components are automatically saved and loaded using `BBSettings`.  
No extra serialization logic is required — values are stored per **category + entry name**.

---

## 📌 Example: Flashlight Settings

```csharp
string categoryName = "Flashlight";
var flashlightSettings = _modMenu.AddSetting(categoryName);

var sliderGroup = _modMenu.CreateGroup("Sliders");

var rSlider = _modMenu.CreateSlider(categoryName, "Red", 0, 255, 255, true);
rSlider.RegisterValueChangedCallback(evt => updateColor());


// Organize layout
var rWrapper = _modMenu.CreateWrapper();
rWrapper.Add(_modMenu.CreateLabel("Red"));
rWrapper.Add(rSlider);

sliderGroup.Add(rWrapper);
flashlightSettings.Add(sliderGroup);
```
