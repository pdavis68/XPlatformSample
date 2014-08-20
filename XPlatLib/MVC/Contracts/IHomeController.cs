using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlatLib.MVC.Contracts
{
    public interface IHomeController : IController
    {
        void GetBusy();
        void RunPage2();

        string RunPage2ButtonText { get; }
        string GetBusyButtonText { get; }
    }
}
