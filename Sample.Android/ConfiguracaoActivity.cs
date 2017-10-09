using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace Sample.Android
{
    
    [Activity(Label = "ConfiguracaoActivity")]
    public class ConfiguracaoActivity : Activity
    {
        Button btnCriar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Configuracao);

            btnCriar = FindViewById<Button>(Resource.Id.btnRegistrar);
            btnCriar.Click += BtnCriar_Click;

        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Registro incluído com sucesso...,", ToastLength.Short).Show();
            StartActivity(typeof(ConfiguracaoActivity));
        }

    }
}

/*
var layout = new LinearLayout(this);
layout.Orientation = Orientation.Vertical;

            var aLabel = new TextView(this);
aLabel.Text = "Hello, World!!!";

            var aButton = new Button(this);
aButton.Text = "Say Hello!";

aButton.Click += (sender, e) =>
{ aLabel.Text = "Hello Android!"; };

layout.AddView(aLabel);
layout.AddView(aButton);
SetContentView(layout);
*/