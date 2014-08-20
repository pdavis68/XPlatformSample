using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlatLib.MVC.Contracts;
using XPlatXampLib;

namespace XPlatLib.MVC.Controllers
{
    public class HomeController : IHomeController
    {
        [NoIoCResolve]
        private IHomeView _view;

        private IHomeModel _model; 
        private IApplication _application;


        public void GetBusy()
        {
            _view.StartUpdate(10);
            Task.Factory.StartNew(() =>
            {
                for (int index = 0; index < 10; index++)
                {
                    _application.Sleep(300);
                    _view.UpdateProgress(index + 1);
                }
                _application.Sleep(300);
                _view.EndUpdate();
            });
        }

        public void RunPage2()
        {
            _view.RunPage2();
        }

        public void SetView(IView view)
        {
            _view = view as IHomeView;
        }


        public string RunPage2ButtonText
        {
            get 
            {
                return "Run Page 2";
            }
        }

        public string GetBusyButtonText
        {
            get
            {
                return "Get Busy";
            }
        }
    }
}
