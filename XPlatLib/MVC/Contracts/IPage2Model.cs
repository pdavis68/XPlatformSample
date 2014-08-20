using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlatLib.MVC.Contracts
{
    public interface IPage2Model
    {
        bool IsConnected { get; }
        bool IsWiFiConnected { get; }
        bool IsMobileDataConnected { get; }
    }
}
