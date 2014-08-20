using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPlatLib.MVC.Contracts;
using XPlatLib.MVC.Controllers;
using XPlatLib.MVC.Models;
using XPlatXampLib;

namespace XPlatLib
{
    public static class Lib
    {
        public static void Init()
        {
            RegisterServices();
            RegisterMVC();
        }

        public static void RegisterServices()
        {
        }

        public static void RegisterMVC()
        {
            IoC.Container.RegisterServiceType<IHomeController, HomeController>();
            IoC.Container.RegisterServiceType<IHomeModel, HomeModel>();
            IoC.Container.RegisterServiceType<IPage2Controller, Page2Controller>();
            IoC.Container.RegisterServiceType<IPage2Model, Page2Model>();
        }
    }
}
