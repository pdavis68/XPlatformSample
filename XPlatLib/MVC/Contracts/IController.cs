﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPlatLib.MVC.Contracts
{
    public interface IController
    {
        void SetView(IView view);
    }
}
