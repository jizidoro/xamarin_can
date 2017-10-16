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

    [Activity(Label = "VeiculosSituacaoActivity")]
    public class VeiculosSituacaoActivity : ListActivity
    {
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var db2 = new SQLiteConnection(dbPath);
            db2.Close();

            webRequestSituacoes();

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.HistoricoAlertas, Status);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += Btn_ClickLista;
        }

        private void Btn_ClickLista(object sender, AdapterView.ItemClickEventArgs args)
        {

            var db = new SQLiteConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            TokenAtual.statusId = IdStatus[args.Position].ToString();
            db.InsertOrReplace(TokenAtual);
            db.Close();


            Toast.MakeText(Application, Status[args.Position], ToastLength.Short).Show();
            StartActivity(typeof(VeiculosSituacaoOprActivity));
        }

        public WebRequest webRequestSituacoes()
        {

            var db = new SQLiteConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = dadosConfiguracao.FirstOrDefault();
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            db.Close();
            db.Dispose();
            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/GetCesvByStatus";
            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myWebRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);


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
            
            var teste = JsonConvert.DeserializeObject<List<Status>>(json);

            var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3"));

            foreach (var item in teste)
            {
                Status.Add(item.denominacao);
                IdStatus.Add(Convert.ToInt32(item.statusId));

                foreach (var subitem in item.ListaCesv)
                {
                    subitem.id = Convert.ToInt32(subitem.cesvId);
                    /*
                    item.cesvId = item.cesvId;
                    item.dataAgendamentoEntrada = item.dataAgendamentoEntrada;
                    item.nome = item.nome;
                    item.nomeCliente = item.nomeCliente;
                    item.nomeTransportadora = item.nomeTransportadora;
                    item.numero = item.numero;
                    item.placa = item.placa;
                    item.statusDestino = item.statusDestino;
                    item.statusInicio = item.statusInicio;
                    item.telefone = item.telefone;
                    item.tipoVeiculo = item.tipoVeiculo;
                    */
                    connection.InsertOrReplaceAsync(subitem);
                }
                item.id = Convert.ToInt32(item.statusId);
                connection.InsertOrReplaceAsync(item);
            }

            

            return myWebRequest;


        }

        public List<string> Status = new List<string>();
        public List<int> IdStatus = new List<int>();
    }
}


