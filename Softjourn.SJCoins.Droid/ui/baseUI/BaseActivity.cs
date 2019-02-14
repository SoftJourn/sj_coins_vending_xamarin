using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Autofac;
using Softjourn.SJCoins.Core.UI.Bootstrapper;
using Softjourn.SJCoins.Core.UI.Presenters.Interfaces;
using Softjourn.SJCoins.Core.UI.ViewInterfaces;

namespace Softjourn.SJCoins.Droid.ui.baseUI
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity<TPresenter> : AppCompatActivity, IBaseView
        where TPresenter : class, IBasePresenter
    {
        protected ProgressDialog ProgressDialog;
        private ILifetimeScope _scope;

        public Dialog ConfirmDialog;

        protected TPresenter ViewPresenter { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ProgressDialog = new ProgressDialog(this);
            ProgressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);

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
            Console.WriteLine("Destroyed Activity" + this.LocalClassName);
            ViewPresenter = null;
        }

        public virtual void HideProgress() => ProgressDialog?.Dismiss();

        public virtual void ShowProgress(string message)
        {
            ProgressDialog.SetMessage(message);
            ProgressDialog.SetCancelable(false);
            ProgressDialog.Show();
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