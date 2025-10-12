using MelonLoader;

namespace BBModMenu
{
    public static class BBSettings
    {
        public static void AddCategory(string categoryName) {
            MelonPreferences.CreateCategory(categoryName);
        }

        public static void AddEntry<T>(string categoryName, string entryName, T defaultValue) {
            MelonPreferences.CreateCategory(categoryName).CreateEntry(entryName, defaultValue);
        }

        public static T GetEntryValue<T>(string categoryName, string entryName) {
            return MelonPreferences.GetEntryValue<T>(categoryName, entryName);
        }

        public static void SetEntryValue<T>(string categoryName, string entryName, T newValue) {
            MelonPreferences.SetEntryValue(categoryName, entryName, newValue);
        }

        public static void SavePref() {
            MelonPreferences.Save();
        }

        public static bool HasEntry(string categoryName, string entryName) {
            return MelonPreferences.HasEntry(categoryName, entryName);
        }
    }
}