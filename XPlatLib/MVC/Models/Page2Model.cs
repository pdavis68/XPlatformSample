using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPlatLib.Contracts;
using XPlatLib.MVC.Contracts;

namespace XPlatLib.MVC.Models
{
    public class Page2Model : IPage2Model
    {
        private IReachability _reachability;

        public bool IsConnected
        {
            get { return _reachability.IsConnected; }
        }

        public bool IsWiFiConnected
        {
            get { return _reachability.WiFiConnected; }
        }

        public bool IsMobileDataConnected
        {
            get { return _reachability.MobileConnected; }
        }
    }
}
