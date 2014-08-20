using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPlatLib.MVC.Contracts;
using XPlatXampLib;

namespace XPlatLib.MVC.Controllers
{
    public class Page2Controller : IPage2Controller
    {
        [NoIoCResolve]
        private IPage2View _view;
        private IPage2Model _model;

        public bool IsConnected
        {
            get { return _model.IsConnected; }
        }

        public bool IsWiFiConnected
        {
            get { return _model.IsWiFiConnected; }
        }

        public bool IsMobileDataConnected
        {
            get { return _model.IsMobileDataConnected; }
        }

        public void SetView(IView view)
        {
            _view = view as IPage2View;
        }
    }
}
