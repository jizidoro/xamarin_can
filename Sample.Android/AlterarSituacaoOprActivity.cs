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
    
    [Activity(Label = "AlterarSituacaoOprActivity")]
    public class AlterarSituacaoOprActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.AlterarSituacaoOpr);


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