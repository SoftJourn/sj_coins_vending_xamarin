// Helpers/Settings.cs


using Xamarin.Essentials;

namespace Softjourn.SJCoins.Core.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string IsFirstLaunch = "is_first_launch";
        private static readonly bool IsFirstLaunchDefault = true;

        private const string AccessTokenKey = "access_token_key";
        private static readonly string AccessTokenDefault = string.Empty;

        private const string RefreshTokenKey = "refresh_token_key";
        private static readonly string RefreshTokenDefault = string.Empty;

        private const string SelectedMachineIdKey = "selected_machine_id_key";
        private static readonly string SelectedMachineIdDefault = null;
        private const string SelectedMachineNameKey = "selected_machine_name_key";
        private static readonly string SelectedMachineNameDefault = null;

        #endregion


        public static string GeneralSettings
        {
            get => Preferences.Get(SettingsKey, SettingsDefault);
            set => Preferences.Set(SettingsKey, value);
        }

        public static bool FirstLaunch
        {
            get => Preferences.Get(IsFirstLaunch, IsFirstLaunchDefault);
            set => Preferences.Set(IsFirstLaunch, value);
        }

        public static string AccessToken
        {
            get => Preferences.Get(AccessTokenKey, AccessTokenDefault);
            set => Preferences.Set(AccessTokenKey, value);
        }

        public static string RefreshToken
        {
            get => Preferences.Get(RefreshTokenKey, RefreshTokenDefault);
            set => Preferences.Set(RefreshTokenKey, value);
        }

        public static string SelectedMachineId
        {
            get => Preferences.Get(SelectedMachineIdKey, SelectedMachineIdDefault);
            set => Preferences.Set(SelectedMachineIdKey, value);
        }

        public static string SelectedMachineName
        {
            get => Preferences.Get(SelectedMachineNameKey, SelectedMachineNameDefault);
            set => Preferences.Set(SelectedMachineNameKey, value);
        }

        public static bool OnlyOneVendingMachine { get; set; } = false;

        public static void ClearUserData()
        {
            AccessToken = AccessTokenDefault;
            RefreshToken = RefreshTokenDefault;
            SelectedMachineId = SelectedMachineIdDefault;
            SelectedMachineName = SelectedMachineNameDefault;

        }

    }
}