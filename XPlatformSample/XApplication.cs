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
using XPlatLib.Contracts;
using XPlatformSample.Core;
using XPlatXampLib.MVC;
using XPlatLib;

namespace XPlatformSample
{
    public class XApplication : IApplication
    {
        public string Name
        {
            get 
            {
                return "Cross Platform Sample";
            }
        }

        public void Sleep(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        public static void Init()
        {
            Lib.Init();
            XApplication app = new XApplication();
            app.RegisterServices();
            app.RegisterViews();
        }

        public void RegisterServices()
        {
            IoC.Container.RegisterServiceInstance<IApplication>(this);
            IoC.Container.RegisterServiceType<ILogger, Logger>();
            IoC.Container.RegisterServiceType<IReachability, Reachability>();
        }

        public void RegisterViews()
        {
            IoC.Container.RegisterServiceType<IHomeView, MainActivity>();
            MVCMaster.RegisterForView(typeof(IHomeView), typeof(IHomeController));
            IoC.Container.RegisterServiceType<IPage2View, Page2Activity>();
            MVCMaster.RegisterForView(typeof(IPage2View), typeof(IPage2Controller));
        }
    }
}