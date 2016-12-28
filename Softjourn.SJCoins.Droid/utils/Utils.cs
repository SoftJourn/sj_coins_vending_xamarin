
using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using String = System.String;

namespace TrololoWorld.utils
{
    class Utils
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

        //public static void StoreSessionInfo(Session session)
        //{
        //    Preferences.StoreObject(Const.AccessToken, session.AccessToken);
        //    Preferences.StoreObject(Const.RefreshToken, session.RefreshToken);
        //    Preferences.StoreObject(Const.ExpirationDate, Java.Lang.String.ValueOf((new Date().Time / 1000) + Long.ParseLong(session.ExpreIn)));
        //}

        //public static void StoreConcreteMachineInfo(Machines machine)
        //{
        //    Preferences.StoreObject(Const.SelectedMachineId, Java.Lang.String.ValueOf(machine.Id));
        //    Preferences.StoreObject(Const.SelectedMachineName, machine.Name);
        //}

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