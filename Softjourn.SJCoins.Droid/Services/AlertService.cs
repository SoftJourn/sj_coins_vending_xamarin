using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Softjourn.SJCoins.Core.API.Model.Machines;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using Softjourn.SJCoins.Droid.ui.activities;
using Softjourn.SJCoins.Droid.ui.adapters;

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

        //public void CreateMachineSelectorDialog(List<Machines> machines, MainActivity activity)
        //{
        //    var names = machines.Select(machine => machine.Name).ToList();

        //    Dialog dialog = new Dialog(activity);
        //    if (!dialog.IsShowing)
        //    {
        //        dialog.Window.RequestFeature(WindowFeatures.NoTitle);
        //        dialog.Window.RequestFeature(WindowFeatures.SwipeToDismiss);
        //        dialog.SetContentView(Resource.Layout.dialog_select_machine);
        //        var machinesList = dialog.FindViewById<ListView>(Resource.Id.lv);
        //        SelectMachineListAdapter adapter = new SelectMachineListAdapter(activity,
        //            Android.Resource.Layout.SimpleListItem1, names);
        //        machinesList.Adapter = adapter;
        //        dialog.Window.Attributes.WindowAnimations = Resource.Style.MachinesDialogAnimation;
        //        activity.HideProgress();
        //        dialog.Show();

        //        machinesList.ItemClick += (sender, e) =>
        //        {
        //            foreach (Machines machine in machines)
        //            {
        //                if (adapter.GetItem(e.Position).ToString() == machine.Name)
        //                {
        //                    //Preferences.StoreObject(Const.SelectedMachineId, Java.Lang.String.ValueOf(machine.Id));
        //                    //Preferences.StoreObject(Const.SelectedMachineName, machine.Name);
        //                    break;
        //                }
        //            }
        //            activity.ShowProgress(activity.GetString(Resource.String.progress_loading));
        //            if (activity.MConfirmDialogIsVisible)
        //            {
        //                activity.ConfirmDialog.Dismiss();
        //            }
        //            //LoadProductList();
        //            dialog.Dismiss();
        //        };
        //        dialog.CancelEvent += (sender, e) =>
        //        {
        //            //if (TextUtils.IsEmpty(Preferences.RetrieveStringObject(Const.SelectedMachineId)))
        //            //{
        //            //    ShowToastMessage(activity.GetString(Resource.String.machine_not_selected_toast));
        //            //}
        //        };
        //    }
        //}

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