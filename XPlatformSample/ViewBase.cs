using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RadialProgress;

namespace XPlatformSample
{
    public class ViewBase : Activity
    {
        private RelativeLayout _rl = null;
        private RadialProgressView _rpv = null;
        private int _total = -1;

        public void StartUpdate(int total)
        {
            _total = total;
            RunOnUiThread(() =>
            {
                if (_rpv == null)
                {
                    _rl = new RelativeLayout(this);
                    _rl.LayoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    _rpv = new RadialProgressView(this);
                    RelativeLayout.LayoutParams rpvParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    rpvParams.AddRule(LayoutRules.CenterInParent);
                    _rpv.LayoutParameters = rpvParams;
                    _rpv.Visibility = ViewStates.Visible;
                    _rl.AddView(_rpv, new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent));
                    AddContentView(_rl, new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent));
                }
                else
                {
                    _rpv.Value = 0;
                    _rpv.Visibility = ViewStates.Visible;
                }
            });
        }

        public void UpdateProgress(int completed)
        {
            RunOnUiThread(() =>
            {
                _rpv.Value = ((float)completed / (float)_total);
            });
        }

        public void EndUpdate()
        {
            RunOnUiThread(() =>
            {
                if (_rpv != null)
                {
                    _rpv.Visibility = ViewStates.Gone;
                }
            });
        }


        public void ShowError(string error)
        {
            using (AlertDialog.Builder adb = new AlertDialog.Builder(this))
            {
                AlertDialog ad = adb.SetTitle("Error").SetMessage(error).Create();
                ad.Show();
            }
        }
    }
}