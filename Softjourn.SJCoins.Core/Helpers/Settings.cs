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
          set { AppSettings.AddOrUpdateValue(IsFirstLaunch, IsFirstLaunchDefault); }  
      }

  }
}