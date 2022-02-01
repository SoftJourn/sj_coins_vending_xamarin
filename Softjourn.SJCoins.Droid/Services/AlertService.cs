using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Bumptech.Glide;
using Google.Android.Material.Snackbar;
using Softjourn.SJCoins.Core.UI.Services.Alert;
using Softjourn.SJCoins.Core.API.Model.Products;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid.Services
{
    public class AlertService : IAlertService
    {
        public void ShowInformationDialog(string title, string msg, string btnName, Action btnClicked)
        {
            CreateAlertDialog(title, msg, btnClicked, btnName);
        }

        public void ShowConfirmationDialog(string title, string msg, Action btnOkClicked, Action btnCancelClicked)
        {
            CreateAlertDialog(title, msg, btnCancelClicked);
        }

        public void ShowToastMessage(string msg)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            activity.RunOnUiThread(() =>
            {
                var toast = Toast.MakeText(activity.ApplicationContext, msg, ToastLength.Short);
                if (toast == null)
                    return;
                toast.Show();
            });
        }

        public void ShowMessageWithUserInteraction(string title, string msg, string btnName, Action btnClicked)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            activity.RunOnUiThread(() =>
            {
                var snackbar = Snackbar
                    .Make(activity.FindViewById(Resource.Id.layout_root), msg,
                        BaseTransientBottomBar.LengthIndefinite);
                snackbar.SetAction("Ok", (v) =>
                {
                    snackbar.Dismiss();
                });
                snackbar.Show();
            });
        }

        public void ShowPurchaseConfirmationDialod(Product product, Action<Product> onPurchaseProductAction)
        {
            CreateConfirmationDialog(product, onPurchaseProductAction);
        }

        /**
         * Method for prompting user pick Photo From Camera or from Gallery
         * Using from Profile Screen when clicking on Avatar image
         */
        public void ShowPhotoSelectorDialog(List<string> photoSource, Action fromCamera, Action fromGallery)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;

            var dialog = new Dialog(activity);
            if (!dialog.IsShowing)
            {
                dialog.Window.RequestFeature(WindowFeatures.NoTitle);
                dialog.Window.RequestFeature(WindowFeatures.SwipeToDismiss);
                dialog.SetContentView(Resource.Layout.dialog_select_photo);
                var sourceList = dialog.FindViewById<ListView>(Resource.Id.lv);
                var adapter = new ArrayAdapter(activity,
                    Android.Resource.Layout.SimpleListItem1, photoSource);
                sourceList.Adapter = adapter;
                dialog.Show();

                sourceList.ItemClick += (sender, e) =>
                {
                    if (Regex.IsMatch(adapter.GetItem(e.Position).ToString(), ".*Take.*"))
                    {
                        fromCamera?.Invoke();
                        dialog.Dismiss();
                    }
                    else
                    {
                        fromGallery?.Invoke();
                        dialog.Dismiss();
                    }
                };
            }
        }

        public void ShowQrSelectorDialog(List<string> optionsList, Action scanCode, Action generateCode)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;

            var dialog = new Dialog(activity);
            if (!dialog.IsShowing)
            {
                dialog.Window.RequestFeature(WindowFeatures.NoTitle);
                dialog.Window.RequestFeature(WindowFeatures.SwipeToDismiss);
                dialog.SetContentView(Resource.Layout.dialog_select_photo);
                var title = dialog.FindViewById<TextView>(Resource.Id.textTitle);
                title.Text = activity.GetString(Resource.String.chooseOption);
                var sourceList = dialog.FindViewById<ListView>(Resource.Id.lv);
                var adapter = new ArrayAdapter(activity,
                    Android.Resource.Layout.SimpleListItem1, optionsList);
                sourceList.Adapter = adapter;
                dialog.Show();

                sourceList.ItemClick += (sender, e) =>
                {
                    if (Regex.IsMatch(adapter.GetItem(e.Position).ToString(), ".*Scan.*"))
                    {
                        scanCode?.Invoke();
                        dialog.Dismiss();
                    }
                    else
                    {
                        generateCode?.Invoke();
                        dialog.Dismiss();
                    }
                };
            }
        }

        private void CreateAlertDialog(string title, string msg, Action btnClicked, string btnName = null)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
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
                dialog.Show();
            });
        }

        /**
         * Creates Confirmation Purchase Dialog
         */
        private void CreateConfirmationDialog(Product product, Action<Product> onPurchaseProductAction)
        {
            var context = Xamarin.Essentials.Platform.CurrentActivity;
            var confirmDialog = new Dialog(context);
            confirmDialog.Window.RequestFeature(WindowFeatures.NoTitle);
            confirmDialog.Window.RequestFeature(WindowFeatures.SwipeToDismiss);
            confirmDialog.SetContentView(Resource.Layout.confirm_dialog);

            // set the custom dialog components
            var text = confirmDialog.FindViewById<TextView>(Resource.Id.text);
            text.Text = string.Format(context.GetString(Resource.String.dialog_msg_confirm_buy_product, product.Name, product.IntPrice));
            var image = confirmDialog.FindViewById<ImageView>(Resource.Id.image);
            Glide.With(context).Load(Const.UrlVendingService + product.ImageUrl).Into(image);

            if (!confirmDialog.IsShowing)
            {
                confirmDialog.Show();
            }
            var okButton = confirmDialog.FindViewById<Button>(Resource.Id.dialogButtonOK);
            //if (product.Price > Int64.Parse(Settings.AccessToken))
            //{
            //    okButton.SetTextColor(new Color(ContextCompat.GetColor(context, Resource.Color.colorScreenBackground)));
            //}
            //else
            //{
            okButton.SetTextColor(new Color(ContextCompat.GetColor(context, Resource.Color.colorBlue)));
            //}
            okButton.Click += (sender, e) =>
            {
                onPurchaseProductAction.Invoke(product);
                confirmDialog.Dismiss();
            };

            var cancelButton = confirmDialog.FindViewById<Button>(Resource.Id.dialogButtonCancel);
            cancelButton.Click += (sender, e) =>
            {
                confirmDialog.Dismiss();
            };

            confirmDialog.SetOnDismissListener(new OnDismissListener(() =>
        {
            confirmDialog.Dismiss();
        }));
        }

        private sealed class OnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        {
            private readonly Action _action;

            public OnDismissListener(Action action)
            {
                _action = action;
            }

            public void OnDismiss(IDialogInterface dialog)
            {
                _action();
            }
        }

    }
}