using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Sample.Android;

namespace DialogConfirmacao

{

    public class DialogFragment1 : DialogFragment
    {
        public static DialogFragment1 NewInstance(Bundle bundle)
        {
            DialogFragment1 fragment = new DialogFragment1();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.DialogFragment1Layout, container, false);
            Button button = view.FindViewById<Button>(Resource.Id.CloseButton);
            button.Click += delegate {
                Dismiss();
                Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
            };

            return view;
        }
    }
}