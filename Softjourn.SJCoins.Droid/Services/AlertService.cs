using System;
using Android.App;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Softjourn.SJCoins.Core.UI.Services.Alert;

namespace Softjourn.SJCoins.Droid.Services
{
    public class AlertService : IAlertService
    {
        public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked)
        {
            CreateAlertDialog(title,msg, btnClicked,btnName);
        }

        public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
        {
            CreateAlertDialog(title, msg, btnCancelClicked);
        }

        public void ShowToastMessage(string msg)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            activity.RunOnUiThread(() =>
            {
                var toast = Toast.MakeText(activity, msg, ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                if (toast.View.WindowVisibility != ViewStates.Visible)
                {
                    toast.Show();
                }
            });
        }

        public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            activity.RunOnUiThread(() =>
            {
                var snackbar = Snackbar
                    .Make(activity.FindViewById(Resource.Id.layout_root), msg,
                        Snackbar.LengthLong);
                snackbar.Show();
            });
        }

        private void CreateAlertDialog(string title, string msg, Action btnClicked, string btnName = null)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            activity.RunOnUiThread(() =>
            {
                var dialog = new Dialog(activity);
                dialog.Window.RequestFeature(WindowFeatures.NoTitle);
                dialog.SetContentView(Resource.Layout.dialog_error);

                // set the custom dialog components
                var text = dialog.FindViewById<TextView>(Resource.Id.text);
                text.Text = msg;

                var titleText = dialog.FindViewById<TextView>(Resource.Id.textTitle);
                titleText.Text = title;

                var cancelButton = dialog.FindViewById<Button>(Resource.Id.dialogButtonCancel);
                if (btnName != null)
                {
                    cancelButton.Text = btnName;
                }
                cancelButton.Click += (sender, e) =>
                {
                    btnClicked?.Invoke();
                    dialog.Dismiss();

                };

                if (dialog.IsShowing) return;
                dialog.Window.Attributes.WindowAnimations = Resource.Style.ConfirmDialogAnimation;
                dialog.Show();
            });
        }
    }
}