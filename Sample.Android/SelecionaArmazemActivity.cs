using Android.App;
using Android.OS;
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
    [Activity(Label = "Selecione o armazém")]
    public class SelecionaArmazemActivity : ListActivity
    {
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");

            var db = new SQLiteAsyncConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = await dadosConfiguracao.FirstOrDefaultAsync();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();

            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/GetUnidadesUsuario";
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


            var teste = JsonConvert.DeserializeObject<ModelEmpresa>(json);
            
            foreach (var item in teste.ListaArmazens)
            {
                ListaArmazens.Add(item.denominacao);
                IdArmazens.Add(Convert.ToInt32(item.armazemId));

                Armazem armazemTemp = new Armazem();
                armazemTemp.armazemId = item.armazemId;
                armazemTemp.denominacao = item.denominacao;
                armazemTemp.id = Convert.ToInt32(item.armazemId);
                armazemTemp.latitude = item.latitude;
                armazemTemp.longitude = item.longitude;
                db.InsertOrReplaceAsync(armazemTemp);

                foreach (var subitem in item.ListaPermissoes)
                {
                    subitem.armazemId = item.armazemId;
                    db.InsertOrReplaceAsync(subitem);
                }
            }
            Empresa empresaTemp = new Empresa();

            empresaTemp.id = Convert.ToInt32(teste.empresaId);
            empresaTemp.denominacao = teste.denominacao;
            empresaTemp.empresaId = teste.empresaId;

            db.InsertOrReplaceAsync(empresaTemp);

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.HistoricoAlertas, ListaArmazens);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                TokenAtual.armazemId = IdArmazens[args.Position].ToString();
                db.InsertOrReplaceAsync(TokenAtual);
                
                Toast.MakeText(Application, ListaArmazens[args.Position], ToastLength.Short).Show();
                StartActivity(typeof(LoginActivity));

            };
        }



        public List<string> ListaArmazens = new List<string>();
        public List<int> IdArmazens = new List<int>();
    }
}

