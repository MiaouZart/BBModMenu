using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Slider = UnityEngine.UIElements.Slider;
using Toggle = UnityEngine.UIElements.Toggle;

namespace BBModMenu
{
    public class BBModMenuMod : MelonMod
    {

        [HarmonyPatch(typeof(GameUI), "Awake")]
        private static class PatchGameUIAwake
        {
            [HarmonyPrefix]
            private static void Prefix(GameUI __instance)
            {
                __instance.gameObject.AddComponent<BBModMenuComponent>();
            }
        }
    }
    class BBModMenuComponent : MonoBehaviour
    {
    private GameUI _gameUI;
    private List<UIScreen> _screens;
    public ModMenu ModMenu;
    
    public void Start() {
        MelonPreferences.Save();
        GameObject gameUI = GameObject.Find("GameUI");
        _gameUI = gameUI.GetComponent<GameUI>();
        _screens = typeof(GameUI)?.GetField("screens", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(_gameUI) as List<UIScreen>;

        ModMenu = new ModMenu(_gameUI);
        _screens?.Add(ModMenu);
        

        MainScreen mainScreen = _gameUI.FindScreen<MainScreen>();
        var backingField = typeof(UIScreen).GetField($"<Screen>k__BackingField",
            BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingField != null)
        {
            var visualElementOfMainScreen = backingField.GetValue(mainScreen) as VisualElement;
            VisualElement menuVisualElement = visualElementOfMainScreen?[0];
            Button menuModButton = new Button{ text = "Mod Menu" };
            menuVisualElement?.Add(menuModButton);
            menuModButton.clicked += delegate(){ _gameUI.SwitchToScreen<ModMenu>(); };
        }
        
        CustomMapPauseScreen customMapPauseScreen = _gameUI.FindScreen<CustomMapPauseScreen>();
        if (backingField != null)
        {
            var visualElementOfMainScreen = backingField.GetValue(customMapPauseScreen) as VisualElement;
            VisualElement menuVisualElement = visualElementOfMainScreen?[0];
            Button menuModButton = new Button{ text = "Mod Menu" };
            menuVisualElement?.Add(menuModButton);
            menuModButton.clicked += delegate(){ _gameUI.SwitchToScreen<ModMenu>(); };
        }
        
    }


    private void Update() {
        //Melon<BBModMenuMod>.Logger.Msg(_gameUI.ActiveScreen);
    }
    }

public class ModMenu : UIScreen {
    
    private VisualElement _root;
    private ScrollView _menu;
    private VisualElement _settings;
    private VisualElement _menuItems;
    private ScrollView _scrollView;
    public static Color _BBgreen = new Color(106f / 255f, 144f / 255f, 43f / 255f);
    public static Color _BBBackGround = new Color(0.000f, 0.000f, 0.000f, 0.502f);
    private Button backButton;
    
    public ModMenu(GameUI ui) : base(ui, "Intro") {
        
        var propertyInfo = typeof(UIScreen).GetProperty("Screen", 
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        var backingField = typeof(UIScreen).GetField($"<Screen>k__BackingField", 
            BindingFlags.Instance | BindingFlags.NonPublic);
        _root = new VisualElement();
        _root.name = "SettingsMenu";
        if (backingField != null) {
            backingField.SetValue(this, _root);
        }
        else {
            propertyInfo?.GetSetMethod(true)?.Invoke(this, new object[] { _root });
        }
        _menu = new ScrollView();  
        _settings = new VisualElement();
        _scrollView = new ScrollView();

        _root.style.flexDirection = FlexDirection.Row;
        _root.style.height = new Length(100, LengthUnit.Percent);

        _menu.name = "Menu";
        _menu.style.flexDirection = FlexDirection.Column;
        _menu.style.flexBasis = new Length(20, LengthUnit.Percent);
        
        _settings.name = "Settings";
        _settings.style.flexDirection = FlexDirection.Column;
        _settings.style.flexGrow = 1;
        _settings.style.marginLeft = 100;

        _scrollView.style.height = new Length(100, LengthUnit.Percent);
        _settings.Add(_scrollView);

        _root.Add(_menu);
        _root.Add(_settings);
        VisualElement backButtonGroup = CreateGroup("BackButton");
        backButtonGroup.style.marginTop = 10;
        backButton = CreateButton("Back");
        backButton.clicked += () => base.UI.SwitchToPreviousScreen();
        backButtonGroup.Add(backButton);
        _menuItems = new VisualElement();
        _menu.Add(_menuItems);
        _menu.Add(backButtonGroup);
        Debug.Log("ModMenu Created");
            
        }


    public override void OnUpdate() {
        base.OnUpdate();
        if (PlayerInput.BackPressed) {
            base.UI.SwitchToPreviousScreen();
        }

    }
    
    
    private bool _added = false;
    public override void OnEnter() {
        if (!_added) {
            base.Root.Add(_root);
            _added = true;
        }
        base.OnEnter();
    }


    public VisualElement AddSetting(string setting) {
        VisualElement groupVisualElement = CreateGroup(setting);
        AddToScrollView(groupVisualElement);
        BBSettings.AddCategory(setting);
        
        Button groupButton = CreateButton(setting);
        groupButton.style.color = _BBgreen;
        groupButton.clicked += delegate(){
            _scrollView.scrollOffset = new Vector2(0f, groupVisualElement.layout.position.y);
        };
        
        groupVisualElement.Add(CreateTitleLabel(setting));
        
        AddToScrollView(groupVisualElement);
        AddToMenu(groupButton);
        
        return groupVisualElement;
    }

    public void AddToScrollView(VisualElement element) {
        _scrollView.Add(element);
    }

    public void AddToMenu(VisualElement element) {
        _menuItems.Add(element);
    }

    public VisualElement CreateGroup(string groupName) {
        VisualElement newgroup = new VisualElement();
        newgroup.name = groupName;
        
        return newgroup;
    }



    public VisualElement CreateWrapper() {
        VisualElement newwrapper = new VisualElement();
        newwrapper.style.marginBottom = 5;
        newwrapper.style.paddingBottom = 7;
        newwrapper.style.backgroundColor = _BBBackGround;
        
        return newwrapper;
    }
    
    
    

    public void AddToGroup(VisualElement group,VisualElement element) {
        group.Add(element);
    }


    public Label CreateLabel(string text) {
        Label label = new Label();
        label.text = text;
        label.style.fontSize = 20;
        
        return label;
    }

    public Label CreateTitleLabel(string text) {
        Label label = new Label();
        label.text = text; 
        label.style.backgroundColor = _BBBackGround;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.fontSize = 50;
        label.style.color = _BBgreen;
        
        return label; 
    }

    public Button CreateButton(string text) {
        Button newButton = new Button{text = text};
        newButton.style.alignSelf = Align.FlexStart;
        newButton.style.width = new StyleLength(StyleKeyword.Auto);
        newButton.style.unityTextAlign = TextAnchor.MiddleLeft;
        
        return newButton;
    }


    public Slider CreateSlider(string category,string name,float min, float max, float defaultValue = 0, bool onlyInt = false)
    {
        BBSettings.AddEntry<float>(category, name, defaultValue);
        Slider newSlider = new Slider(min, max)
        {
            value = BBSettings.GetEntryValue<float>(category, name),
            name = "Slider",
            showInputField = false
            
        };
        newSlider.name = name;

        TextField valueField = new TextField
        {
            value = onlyInt ? ((int)BBSettings.GetEntryValue<float>(category, name)).ToString() : BBSettings.GetEntryValue<float>(category, name).ToString("0.##"),
            focusable = false,
            isReadOnly = true
        };

        valueField.style.width = 8 * 4 + 10; 
        valueField.style.marginLeft = 10;
        newSlider.Add(valueField);

        newSlider.RegisterValueChangedCallback(evt => 
        {
            float newValue = onlyInt ? Mathf.Round(evt.newValue) : evt.newValue;
            newSlider.value = newValue; 
            valueField.value = onlyInt ? ((int)newValue).ToString() : newValue.ToString("0.##");
            BBSettings.SetEntryValue<float>(category, name, evt.newValue);
            BBSettings.SavePref();
        });
    
        return newSlider;
    }

    public Toggle CreateToggle(string category ,string name,bool defaultValue = false) {
        
        BBSettings.AddEntry<bool>(category, name, defaultValue);
        Toggle checkBox = new Toggle();
        checkBox.name = name;
        var checkBoxCheckMark = checkBox.Q("unity-checkmark");
        checkBox.value = BBSettings.GetEntryValue<bool>(category, name);
        checkBoxCheckMark.style.unityBackgroundImageTintColor = (checkBox.value ? Color.white : Color.clear);
        checkBox.RegisterValueChangedCallback(delegate(ChangeEvent<bool> b){
            checkBoxCheckMark.style.unityBackgroundImageTintColor = (b.newValue ? Color.white : Color.clear);
            BBSettings.SetEntryValue<bool>(category, name, b.newValue);
            BBSettings.SavePref();
        });
        
        return checkBox;
    }

    
    

}
    
    
}
