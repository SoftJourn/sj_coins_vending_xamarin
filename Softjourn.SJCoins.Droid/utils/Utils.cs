
using Android.Content;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Snackbar;
using String = System.String;

namespace Softjourn.SJCoins.Droid.utils
{
    public class Utils
    {

        public static void ShowErrorToast(Context context, String text)
        {
            var toast = Toast.MakeText(context, text, ToastLength.Short);
            toast.SetGravity(GravityFlags.Center, 0, 0);
            if (toast.View.WindowVisibility != ViewStates.Visible)
            {
                toast.Show();
            }
        }

        public static void ShowSnackBar(View view, String message)
        {
            var snackbar = Snackbar
                    .Make(view, message, Snackbar.LengthLong);
            snackbar.Show();
        }
    }
}