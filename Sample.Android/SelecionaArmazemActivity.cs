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

    [Activity(Label = "VeiculosSituacaoActivity")]
    public class SelecionaArmazemActivity : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            webRequestTeste();

            ListAdapter = new ArrayAdapter<string>(this,Resource.Layout.HistoricoAlertas, Armazens);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += Btn_ClickLista;
        }

        private void Btn_ClickLista(object sender, AdapterView.ItemClickEventArgs args)
        {

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha1.db3");
            var db = new SQLiteConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            TokenAtual.armazemId = IdArmazens[args.Position].ToString();
            db.InsertOrReplace(TokenAtual);
            db.Close();


            Toast.MakeText(Application, Armazens[args.Position] , ToastLength.Short).Show();
            StartActivity(typeof(LoginActivity));
        }

        public WebRequest webRequestTeste()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha1.db3");
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

            var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha1.db3"));

            foreach (var item in teste.ListaArmazens)
            {
                Armazens.Add(item.denominacao);
                IdArmazens.Add(Convert.ToInt32(item.armazemId));
                connection.InsertOrReplaceAsync(item);
                foreach (var subitem in item.ListaPermissoes)
                {
                    subitem.armazemId = item.armazemId;
                    connection.InsertOrReplaceAsync(subitem);
                }
            }
            
            connection.InsertOrReplaceAsync(teste);

            return myWebRequest;


        }

        public List<string> Armazens = new List<string>();
        public List<int> IdArmazens = new List<int>();
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
