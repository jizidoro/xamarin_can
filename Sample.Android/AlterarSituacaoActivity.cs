using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Sample.Android
{

    [Activity(Label = "AlterarSituacaoActivity")]
    public class AlterarSituacaoActivity : Activity
    {
        Button btnOpr, btnQr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AlterarSituacao);

            webRequestTeste();

            btnOpr = FindViewById<Button>(Resource.Id.buttonOpr);
            btnOpr.Click += btnOpr_Click;


            btnQr = FindViewById<Button>(Resource.Id.buttonScan);
            btnQr.Click += btnQr_Click;



        }

        public WebRequest webRequestTeste()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");

            var db2 = new SQLiteConnection(dbPath);
            db2.Close();

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


            var teste = JsonConvert.DeserializeObject<Empresa>(json);

            var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3"));
            

            foreach (var item in teste.ListaArmazens)
            {
                //faz alguma coisa()
            }
            

            return myWebRequest;


        }


        private void btnOpr_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Registro incluído com sucesso...,", ToastLength.Short).Show();
            StartActivity(typeof(AlterarSituacaoOprActivity));
        }

        private void btnQr_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Registro incluído com sucesso...,", ToastLength.Short).Show();
            StartActivity(typeof(AlterarSituacaoQrCodeActivity));
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
