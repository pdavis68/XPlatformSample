using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XPlatLib.MVC.Contracts;
using XPlatXampLib;
using XPlatXampLib.MVC;

namespace XPlatformSample
{
    [Activity(Label = "PageActivity")]
    public class Page2Activity : ViewBase, IPage2View
    {
        [NoIoCResolve]
        IPage2Controller _controller;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Page2);
            _controller = _controller ?? MVCMaster.GetController(typeof(IPage2View)) as IPage2Controller;
            _controller.SetView(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            FindViewById<CheckedTextView>(Resource.Id.txtConnected).Checked = _controller.IsConnected;
            FindViewById<CheckedTextView>(Resource.Id.txtWiFi).Checked = _controller.IsWiFiConnected;
            FindViewById<CheckedTextView>(Resource.Id.txtMobile).Checked = _controller.IsMobileDataConnected;
        }

    }
}