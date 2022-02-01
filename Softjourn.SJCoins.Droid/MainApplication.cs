using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Bumptech.Glide;
using Softjourn.SJCoins.Droid.Bootstrapping;
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
            Xamarin.Essentials.Platform.Init(this);
            Glide.Init(this, new GlideBuilder());
            InitializeIoC();
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            Xamarin.Essentials.Platform.OnResume(activity);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
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