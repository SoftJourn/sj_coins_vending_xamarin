// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Softjourn.SJCoins.Core.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

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

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static bool FirstLaunch
        {
            get { return AppSettings.GetValueOrDefault<bool>(IsFirstLaunch, IsFirstLaunchDefault); }
            set { AppSettings.AddOrUpdateValue(IsFirstLaunch, value); }
        }

        public static string AccessToken
        {
            get { return AppSettings.GetValueOrDefault<string>(AccessTokenKey, AccessTokenDefault); }
            set { AppSettings.AddOrUpdateValue(AccessTokenKey, value); }
        }

        public static string RefreshToken
        {
            get { return AppSettings.GetValueOrDefault<string>(RefreshTokenKey, RefreshTokenDefault); }
            set { AppSettings.AddOrUpdateValue(RefreshTokenKey, value); }
        }

        public static string SelectedMachineId
        {
            get { return AppSettings.GetValueOrDefault<string>(SelectedMachineIdKey, SelectedMachineIdDefault); }
            set { AppSettings.AddOrUpdateValue(SelectedMachineIdKey, value); }
        }

        public static void ClearUserData()
        {
            AccessToken = AccessTokenDefault;
            RefreshToken = RefreshTokenDefault;
            SelectedMachineId = SelectedMachineIdDefault;
        }

    }
}