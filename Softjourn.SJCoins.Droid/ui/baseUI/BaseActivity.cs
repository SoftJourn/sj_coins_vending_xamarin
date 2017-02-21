using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Presenters.IPresenters;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Droid.ui.baseUI
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity<TPresenter> : AppCompatActivity, IBaseView
        where TPresenter : class, IBasePresenter
    {
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

            var vmPolicy = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(
                vmPolicy
                    .DetectActivityLeaks()
                    .DetectLeakedClosableObjects()
                    .DetectLeakedSqlLiteObjects()
                    .DetectLeakedRegistrationObjects()
                    .PenaltyLog()
                    .Build());
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
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
            _progressDialog?.Dismiss();
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

        public void ShowToast(string text)
        {
        }

        public virtual void AttachEvents()
        {
            throw new NotImplementedException();
        }

        public virtual void DetachEvents()
        {
            throw new NotImplementedException();
        }
    }
}