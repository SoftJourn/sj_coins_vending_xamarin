using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Square.Picasso;
using TrololoWorld.utils;
using String = System.String;

namespace Softjourn.SJCoins.Droid.ui.baseUI
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity : AppCompatActivity
    {
        protected bool MProfileIsVisible = false;
        protected bool MConfirmDialogIsVisible = false;

        private ProgressDialog _progressDialog;

        protected Dialog ConfirmDialog;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void HideProgress()
        {
            _progressDialog.Dismiss();
        }

        public void ShowProgress(String message)
        {
            _progressDialog.SetMessage(message);
            _progressDialog.SetCancelable(false);
            _progressDialog.Show();
        }

        protected void OnNoInternetAvailable()
        {
            ShowToast(GetString(Resource.String.internet_turned_off));
            HideProgress();
        }

        public void ShowToast(String text)
        {
            Utils.ShowErrorToast(this, text);
        }

        public abstract void ShowSnackBar(string message);

        public abstract void LogOut(IMenuItem item);

        //protected void OnCreateConfirmDialog(Product product)//, PurchaseContract.Presenter presenter)
        //{

        //    ConfirmDialog = new Dialog(this);
        //    ConfirmDialog.Window.RequestFeature(WindowFeatures.NoTitle);
        //    ConfirmDialog.Window.RequestFeature(WindowFeatures.SwipeToDismiss);
        //    ConfirmDialog.SetContentView(Resource.Layout.confirm_dialog);

        //    // set the custom dialog components
        //    var text = ConfirmDialog.FindViewById<TextView>(Resource.Id.text);
        //    text.Text = String.Format(GetString(Resource.String.dialog_msg_confirm_buy_product), product.Name,
        //        product.Price);
        //    var image = ConfirmDialog.FindViewById<ImageView>(Resource.Id.image);
        //    Picasso.With(this).Load(Const.UrlVendingService + product.ImageUrl).Into(image);

        //    if (!ConfirmDialog.IsShowing)
        //    {
        //        ConfirmDialog.Window.Attributes.WindowAnimations = Resource.Style.ConfirmDialogAnimation;
        //        MConfirmDialogIsVisible = true;
        //        ConfirmDialog.Show();
        //    }
        //    var okButton = ConfirmDialog.FindViewById<Button>(Resource.Id.dialogButtonOK);
        //    if (product.Price > Integer.ParseInt(Preferences.RetrieveStringObject(Const.UserBalancePreferencesKey)))
        //    {
        //        okButton.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorScreenBackground)));
        //    }
        //    else
        //    {
        //        okButton.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorBlue)));
        //    }
        //    okButton.Click += (sender, e) =>
        //    {
        //        if (product.Price > Integer.ParseInt(Preferences.RetrieveStringObject(Const.UserBalancePreferencesKey)))
        //        {
        //            ShowSnackBar(GetString(Resource.String.server_error_40901));
        //        }
        //        else
        //        {
        //            //presenter.buyProduct(Java.Lang.String.ValueOf(product.Id), this);
        //            MConfirmDialogIsVisible = false;
        //            ConfirmDialog.Dismiss();
        //        }
        //    };

        //    var cancelButton = ConfirmDialog.FindViewById<Button>(Resource.Id.dialogButtonCancel);
        //    cancelButton.Click += (sender, e) =>
        //    {

        //        MConfirmDialogIsVisible = false;
        //        ConfirmDialog.Dismiss();
        //    };

        //    ConfirmDialog.SetOnDismissListener(new OnDismissListener(() =>
        //    {

        //        MConfirmDialogIsVisible = false;
        //    }));
        //}

        //private sealed class OnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        //{
        //    private readonly Action _action;

        //    public OnDismissListener(Action action)
        //    {
        //        this._action = action;
        //    }

        //    public void OnDismiss(IDialogInterface dialog)
        //    {
        //        this._action();
        //    }
        //}


        //public void OnCreateErrorDialog(string message)
        //{
        //    Dialog dialog = new Dialog(this);
        //    dialog.Window.RequestFeature(WindowFeatures.NoTitle);
        //    dialog.SetContentView(Resource.Layout.dialog_error);

        //    // set the custom dialog components
        //    var text = dialog.FindViewById<TextView>(Resource.Id.text);
        //    text.Text = message;

        //    var cancelButton = dialog.FindViewById<Button>(Resource.Id.dialogButtonCancel);
        //    cancelButton.Click += (sender, e) =>
        //    {
        //        dialog.Dismiss();
        //    };

        //    if (!dialog.IsShowing)
        //    {
        //        dialog.Window.Attributes.WindowAnimations = Resource.Style.ConfirmDialogAnimation;
        //        dialog.Show();
        //    }
        //}
    }
}