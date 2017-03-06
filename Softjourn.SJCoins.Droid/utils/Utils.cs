
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.Utils;
using String = System.String;

namespace Softjourn.SJCoins.Droid.utils
{
    public class Utils
    {

        public static void ShowErrorToast(Context context, String text)
        {
            Toast toast = Toast.MakeText(context, text, ToastLength.Short);
            toast.SetGravity(GravityFlags.Center, 0, 0);
            if (toast.View.WindowVisibility != ViewStates.Visible)
            {
                toast.Show();
            }
        }

        public static void ShowSnackBar(View view, String message)
        {
            Snackbar snackbar = Snackbar
                    .Make(view, message, Snackbar.LengthLong);
            snackbar.Show();
        }

        public static void ClearUsersData()
        {
            Preferences.ClearStringObject(Const.AccessToken);
            Preferences.ClearStringObject(Const.RefreshToken);
            Preferences.ClearStringObject(Const.SelectedMachineName);
            Preferences.ClearStringObject(Const.SelectedMachineId);
            Preferences.ClearStringObject(Const.UserBalancePreferencesKey);
            Preferences.ClearStringObject(Const.UserNamePreferencesKey);
        }
    }
}