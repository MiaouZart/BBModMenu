using System;
using System.Collections.Generic;
using MelonLoader;

namespace BBModMenu
{
    public static class BBSettings
    {

        public static void AddCategory(String categoryName) {
            MelonPreferences.CreateCategory(categoryName);
        }

        public static void AddEntry<T>(String categoryName,String entryName ,T defaultValue) {
           MelonPreferences.CreateCategory(categoryName).CreateEntry<T>(entryName,defaultValue);
        }

        public static T GetEntryValue<T>(String categoryName, String entryName) {
            return MelonPreferences.GetEntryValue<T>(categoryName, entryName);
        }

        public static void SetEntryValue<T>(String categoryName, String entryName, T newValue) {
            MelonPreferences.SetEntryValue<T>(categoryName, entryName, newValue);
        }

        public static void  SavePref() {
            MelonPreferences.Save();
        }

        public static bool HasEntry(String categoryName, String entryName) {
            return MelonPreferences.HasEntry(categoryName, entryName);
        }
    }
}