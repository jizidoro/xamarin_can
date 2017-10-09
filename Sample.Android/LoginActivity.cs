using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace Sample.Android
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var scrollView = new ScrollView(this)
            {
                LayoutParameters =
                           new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };

            this.SetContentView(scrollView);

            var mainLayout = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
                LayoutParameters =
                           new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)

            };

            scrollView.AddView(mainLayout);

            for (int n = 1; n < 5; n++)
            {
                var aButton = new Button(this);


                switch (n)
                {
                    case 1:
                        aButton.Text = "Alterar Situação";
                        aButton.Click += BtnCriar_Click1;
                        break;
                    case 2:
                        aButton.Text = "Veiculos por Situação";
                        aButton.Click += BtnCriar_Click2;
                        break;
                    case 3:
                        aButton.Text = "Historico de Alertas";
                        aButton.Click += BtnCriar_Click3;
                        break;
                    case 4:
                        aButton.Text = "Configuração";
                        aButton.Click += BtnCriar_Click4;
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }


                mainLayout.AddView(aButton);
            }
        }

        private void BtnCriar_Click1(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Alterar Situacao...,", ToastLength.Short).Show();
            StartActivity(typeof(AlterarSituacaoActivity));
        }

        private void BtnCriar_Click2(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "VeiculosSituacaoActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(VeiculosSituacaoActivity));
        }

        private void BtnCriar_Click3(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "HistoricoAlertasActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(HistoricoAlertasActivity));
        }

        private void BtnCriar_Click4(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "ConfiguracaoActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(ConfiguracaoActivity));
        }
    }
}