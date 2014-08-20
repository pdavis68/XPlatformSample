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
using XPlatLib.Contracts;
using Android.Net;

namespace XPlatformSample.Core
{
    public class Reachability : IReachability
    {
        public bool IsConnected
        {
            get 
            {
                ConnectivityManager cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                return (cm != null && cm.ActiveNetworkInfo != null && cm.ActiveNetworkInfo.IsConnected);
            }
        }

        public bool WiFiConnected
        {
            get 
            {
                ConnectivityManager cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                return (cm != null && cm.GetNetworkInfo(ConnectivityType.Wifi).GetState() == NetworkInfo.State.Connected);
            }
        }

        public bool MobileConnected
        {
            get
            {
                ConnectivityManager cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                return (cm != null && cm.GetNetworkInfo(ConnectivityType.Mobile).GetState() == NetworkInfo.State.Connected);
            }
        }
    }
}