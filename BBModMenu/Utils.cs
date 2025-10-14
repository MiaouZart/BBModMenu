using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace BBModMenu
{
    public static class Utils
    {
        public class HotKeyEntry
        {
            public VisualElement Root;
            public string Value;
            public System.Action<string> OnChanged;
        }

        public class CarouselEntry
        {
            public VisualElement Root;
            public string Value;
        }

        public static bool IsHotkeyPressed(string combo) {
            return CheckHotkey(combo, pressedThisFrame: true);
        }

        public static bool IsHotkeyHeld(string combo) {
            return CheckHotkey(combo, pressedThisFrame: false);
        }

        private static bool CheckHotkey(string combo, bool pressedThisFrame) {
            if (string.IsNullOrEmpty(combo) || Keyboard.current == null)
                return false;

            var keyboard = Keyboard.current;
            var mouse = Mouse.current;

            bool ctrlRequired = combo.Contains("Ctrl+");
            bool shiftRequired = combo.Contains("Shift+");
            bool altRequired = combo.Contains("Alt+");

            string keyName = combo.Replace("Ctrl+", "")
                .Replace("Shift+", "")
                .Replace("Alt+", "")
                .Trim();

            bool ctrlOk = !ctrlRequired || (keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed);
            bool shiftOk = !shiftRequired || (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);
            bool altOk = !altRequired || (keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed);

            bool keyOk = false;

            if (keyName.StartsWith("Mouse"))
            {
                switch (keyName)
                {
                    case "Mouse0":
                        keyOk = pressedThisFrame ? mouse.leftButton.wasPressedThisFrame : mouse.leftButton.isPressed;
                        break;
                    case "Mouse1":
                        keyOk = pressedThisFrame ? mouse.rightButton.wasPressedThisFrame : mouse.rightButton.isPressed;
                        break;
                    case "Mouse2":
                        keyOk = pressedThisFrame
                            ? mouse.middleButton.wasPressedThisFrame
                            : mouse.middleButton.isPressed;
                        break;
                    case "Mouse3": 
    					keyOk = pressedThisFrame
        				? (mouse.backButton?.wasPressedThisFrame ?? false)
        				: (mouse.backButton?.isPressed ?? false);
    				break;
					case "Mouse4":
    					keyOk = pressedThisFrame
        				? (mouse.forwardButton?.wasPressedThisFrame ?? false)
        				: (mouse.forwardButton?.isPressed ?? false);
   					 break;	
		
                    default:
                        keyOk = false;
                        break;
                }
            }
            else
            {
                Key key = Key.None;

                var foundKey = keyboard.FindKeyOnCurrentKeyboardLayout(keyName.ToLower());
                if (foundKey != null)
                    key = foundKey.keyCode;
                else
                {
                    Enum.TryParse<Key>(keyName, true, out key);
                }

                if (key != Key.None)
                {
                    keyOk = pressedThisFrame ? keyboard[key].wasPressedThisFrame : keyboard[key].isPressed;
                }
            }

            return ctrlOk && shiftOk && altOk && keyOk;
        }
    }
}