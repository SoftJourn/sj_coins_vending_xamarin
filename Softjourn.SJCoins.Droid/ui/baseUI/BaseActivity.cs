using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;
using Softjourn.SJCoins.Droid.utils;
using String = System.String;

namespace Softjourn.SJCoins.Droid.ui.baseUI
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity<TPresenter> : AppCompatActivity, IBaseView
        where TPresenter : class, IBasePresenter
    {
        protected bool MProfileIsVisible = false;
        public bool MConfirmDialogIsVisible = false;

        private ProgressDialog _progressDialog;
        private ILifetimeScope _scope;

        public Dialog ConfirmDialog;

        protected TPresenter ViewPresenter { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);

            _scope = BaseBootstrapper.Container.BeginLifetimeScope();

            ViewPresenter = _scope.Resolve<TPresenter>();
            ViewPresenter.AttachView(this);
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewPresenter = null;
        }

        public virtual void HideProgress()
        {
            _progressDialog.Dismiss();
        }

        public virtual void ShowProgress(string message)
        {
            _progressDialog.SetMessage(message);
            _progressDialog.SetCancelable(false);
            _progressDialog.Show();
        }

        protected void OnNoInternetAvailable(string message)
        {
            ShowToast(message);
            HideProgress();
        }

        public void ShowToast(String text)
        {
            Utils.ShowErrorToast(this, text);
        }

        public abstract void ShowSnackBar(string message);

        public abstract void LogOut(IMenuItem item);

        public void AttachEvents()
        {
            throw new NotImplementedException();
        }

        public void DetachEvents()
        {
            throw new NotImplementedException();
        }

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

    }
}