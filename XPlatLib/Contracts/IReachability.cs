using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlatLib.Contracts
{
    public interface IReachability
    {
        bool IsConnected { get; }
        bool WiFiConnected { get; }
        bool MobileConnected { get; }
    }
}
