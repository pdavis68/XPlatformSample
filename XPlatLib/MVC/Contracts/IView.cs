using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlatLib.MVC.Contracts
{
    public interface IView
    {
        void StartUpdate(int total);
        void UpdateProgress(int completed);
        void EndUpdate();
        void ShowError(string error);
    }
}
