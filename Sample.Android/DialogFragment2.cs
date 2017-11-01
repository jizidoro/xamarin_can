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

namespace DialogConfirmacao
{
    public class DialogFragment2 : DialogFragment
    {
        public static DialogFragment2 NewInstance(Bundle bundle)
        {
            DialogFragment2 fragment = new DialogFragment2();
            fragment.Arguments = bundle;
            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
            alert.SetTitle("Confirma a alteração da situação da CESV ?");
            alert.SetMessage("");
            alert.SetPositiveButton("Confirma", (senderAlert, args) => {
                //Toast.MakeText(Activity, "Confirmado!", ToastLength.Short).Show();
            });

            alert.SetNegativeButton("Cancela", (senderAlert, args) => {
                //Toast.MakeText(Activity, "Cancelado!", ToastLength.Short).Show();
            });
            return alert.Create();
        }
    }
}