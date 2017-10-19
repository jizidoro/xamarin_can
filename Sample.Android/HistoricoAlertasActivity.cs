using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Sample.Android
{
    
    [Activity(Label = "HistoricoAlertasActivity")]
    public class HistoricoAlertasActivity : ListActivity
    {
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var db = new SQLiteAsyncConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = await dadosConfiguracao.FirstOrDefaultAsync();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();

            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/GetAlertasByArmazem?ArmazemId=" + TokenAtual.armazemId;
            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myWebRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);


            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + TokenAtual.access_token);
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            responseStream.Close();
            myWebResponse.Close();

            var teste = JsonConvert.DeserializeObject<List<HistoricoAlertas>>(json);
            
            foreach (var item in teste)
            {
                ListaHistoricoAlertas.Add(item.texto);
                IdHistoricoAlertas.Add(Convert.ToInt32(item.historicoAlertasId));
                item.id = Convert.ToInt32(item.historicoAlertasId);
                db.InsertOrReplaceAsync(item);

            }

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.VeiculosSituacao, ListaHistoricoAlertas);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                TokenAtual.statusId = ListaHistoricoAlertas[args.Position].ToString();
                db.InsertOrReplaceAsync(TokenAtual);
                //StartActivity(typeof(HistoricoAlertasRelatorioActivity));
            };

        }

        public List<string> ListaHistoricoAlertas = new List<string>();
        public List<int> IdHistoricoAlertas = new List<int>();
    }
}

