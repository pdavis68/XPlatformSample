using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XPlatLib.MVC.Contracts;
using XPlatXampLib.MVC;

namespace XPlatformSample
{
    [Activity(Label = "XPlatformSample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ViewBase, IHomeView
    {
        private IHomeController _controller;

        public MainActivity()
        {
            XApplication.Init();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _controller = _controller ?? MVCMaster.GetController(typeof(IHomeView)) as IHomeController;
            _controller.SetView(this);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btnPage2);
            button.Text = _controller.RunPage2ButtonText;
            button.Click += new EventHandler((s, e) =>
            {
                _controller.RunPage2();
            });

            button = FindViewById<Button>(Resource.Id.btnBusy);
            button.Text = _controller.GetBusyButtonText;
            button.Click += new EventHandler((s, e) =>
            {
                _controller.GetBusy();
            });

        }

        public void RunPage2()
        {
            StartActivity(typeof(Page2Activity));
        }
    }
}

