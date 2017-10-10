﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Sample.Android
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            webRequestTeste();


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


        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);

            string requestBody = string.Format("");
            //string requestBody = string.Format("grant_type=password&username={0}&password={1}", "admin", "admin@123");
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            webRequest.BeginGetResponse(new AsyncCallback(GetResponseStreamCallback), webRequest);
        }

        void GetResponseStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = httpWebStreamReader.ReadToEnd();
            }
        }

        public WebRequest webRequestTeste()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Usuario2.db3");
            var db = new SQLiteConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            db.Close();
            System.Uri myUri = new System.Uri("http://192.168.17.102:13359/Api/GerenciamentoPatio/GetUnidadesUsuario");
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + TokenAtual.access_token);
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();
            if (responseStream == null) return null;

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            responseStream.Close();
            myWebResponse.Close();

            return null;

                
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