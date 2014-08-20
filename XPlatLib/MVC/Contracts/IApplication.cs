using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlatLib.MVC.Contracts
{
    public interface IApplication
    {
        string Name { get; }
        void Sleep(int ms);
    }
}
