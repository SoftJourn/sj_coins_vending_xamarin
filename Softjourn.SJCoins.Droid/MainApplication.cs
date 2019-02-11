using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Softjourn.SJCoins.Droid.Bootstrapping;
using Plugin.CurrentActivity;
using Softjourn.SJCoins.Droid.Utils;

namespace Softjourn.SJCoins.Droid
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            HockeyAppUtils.CheckForCrashes(ApplicationContext);

            InitializeIoC();
            //removed ssl errors validation
            //ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        private void InitializeIoC()
        {
            new Bootstrapper().Init();

        }

        public override void OnLowMemory()
        {
            GC.Collect(GC.MaxGeneration);

            base.OnLowMemory();
        }
    }
}