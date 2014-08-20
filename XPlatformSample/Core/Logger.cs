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
using Android.Util;
using XPlatLib.MVC.Contracts;

namespace XPlatformSample.Core
{
    public class Logger : ILogger
    {
        private IApplication _application;

        public void Info(string message)
        {
            Log.Info(_application.Name, message);
        }

        public void Warn(string message)
        {
            Log.Warn(_application.Name, message);
        }

        public void Error(string message)
        {
            Log.Error(_application.Name, message);
        }
    }
}