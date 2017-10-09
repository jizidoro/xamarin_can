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

    [Activity(Label = "VeiculosSituacaoActivity")]
    public class VeiculosSituacaoActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.HistoricoAlertas, countries);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += Btn_ClickLista;
        }

        private void Btn_ClickLista(object sender, AdapterView.ItemClickEventArgs args)
        {
            Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
            StartActivity(typeof(VeiculosSituacaoOprActivity));
        }

        static readonly string[] countries = new String[] {
            "Martinique","Mauritania","Mauritius","Mayotte","Mexico","Micronesia","Moldova",
            "Monaco","Mongolia","Montserrat","Morocco","Mozambique","Myanmar","Namibia",
            "Nauru","Nepal","Netherlands","Netherlands Antilles","New Caledonia","New Zealand",
            "Nicaragua","Niger","Nigeria","Niue","Norfolk Island","North Korea","Northern Marianas",
            "Norway","Oman","Pakistan","Palau","Panama","Papua New Guinea","Paraguay","Peru",
            "Philippines","Pitcairn Islands","Poland","Portugal","Puerto Rico","Qatar",
            "Reunion","Romania","Russia","Rwanda","Sqo Tome and Principe","Saint Helena",
            "Saint Kitts and Nevis","Saint Lucia","Saint Pierre and Miquelon",
            "Saint Vincent and the Grenadines","Samoa","San Marino","Saudi Arabia","Senegal",
            "Seychelles","Sierra Leone","Singapore","Slovakia","Slovenia","Solomon Islands",
            "Somalia","South Africa","South Georgia and the South Sandwich Islands","South Korea",
            "Spain","Sri Lanka","Sudan","Suriname","Svalbard and Jan Mayen","Swaziland","Sweden",
            "Switzerland","Syria","Taiwan","Tajikistan","Tanzania","Thailand","The Bahamas",
            "The Gambia","Togo","Tokelau","Tonga","Trinidad and Tobago","Tunisia","Turkey",
            "Turkmenistan","Turks and Caicos Islands","Tuvalu","Virgin Islands","Uganda",
            "Ukraine","United Arab Emirates","United Kingdom",
            "United States","United States Minor Outlying Islands","Uruguay","Uzbekistan",
            "Vanuatu","Vatican City","Venezuela","Vietnam","Wallis and Futuna","Western Sahara",
            "Yemen","Yugoslavia","Zambia","Zimbabwe"
          };

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
