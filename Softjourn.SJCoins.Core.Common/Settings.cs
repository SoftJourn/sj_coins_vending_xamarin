using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Softjourn.SJCoins.Core.Common
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

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
            get => AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(SettingsKey, value);
        }

        public static bool FirstLaunch
        {
            get => AppSettings.GetValueOrDefault(IsFirstLaunch, IsFirstLaunchDefault);
            set => AppSettings.AddOrUpdateValue(IsFirstLaunch, value);
        }

        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefault);
            set => AppSettings.AddOrUpdateValue(AccessTokenKey, value);
        }

        public static string RefreshToken
        {
            get => AppSettings.GetValueOrDefault(RefreshTokenKey, RefreshTokenDefault);
            set => AppSettings.AddOrUpdateValue(RefreshTokenKey, value);
        }

        public static string SelectedMachineId
        {
            get => AppSettings.GetValueOrDefault(SelectedMachineIdKey, SelectedMachineIdDefault);
            set => AppSettings.AddOrUpdateValue(SelectedMachineIdKey, value);
        }

        public static string SelectedMachineName
        {
            get => AppSettings.GetValueOrDefault(SelectedMachineNameKey, SelectedMachineNameDefault);
            set => AppSettings.AddOrUpdateValue(SelectedMachineNameKey, value);
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