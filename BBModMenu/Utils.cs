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
        
        public static bool IsHotkeyPressed(string combo)
        {
            if (string.IsNullOrEmpty(combo) || Keyboard.current == null)
                return false;

            bool ctrl = combo.Contains("Ctrl+");
            bool shift = combo.Contains("Shift+");
            bool alt = combo.Contains("Alt+");

            string keyName = combo.Replace("Ctrl+", "")
                .Replace("Shift+", "")
                .Replace("Alt+", "");

            if (!Enum.TryParse<Key>(keyName, out var key))
                return false;

            var keyboard = Keyboard.current;

            bool ctrlOk = !ctrl || (keyboard.leftCtrlKey.isPressed || keyboard.rightCtrlKey.isPressed);
            bool shiftOk = !shift || (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed);
            bool altOk = !alt || (keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed);

            bool keyOk = keyboard[key].wasPressedThisFrame;

            return ctrlOk && shiftOk && altOk && keyOk;
        }

    }
}