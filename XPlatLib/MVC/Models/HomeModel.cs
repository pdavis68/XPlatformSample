using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XPlatLib.MVC.Contracts;

namespace XPlatLib.MVC.Models
{
    public class HomeModel : IHomeModel
    {
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
