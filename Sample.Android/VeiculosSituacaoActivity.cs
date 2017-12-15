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

    [Activity(Label = "Situações")]
    public class VeiculosSituacaoActivity : ListActivity
    {
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            activity = this;

            var db = new SQLiteAsyncConnection(dbPath);
            db.ExecuteAsync("DELETE FROM Cesv");
            db.ExecuteAsync("DELETE FROM Status");
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = await  dadosConfiguracao.FirstOrDefaultAsync();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();

            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/GetCesvByStatus";
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

            var teste = JsonConvert.DeserializeObject<List<ModelStatus>>(json);
            
            foreach (var item in teste.OrderBy(x => x.denominacao))
            {
                ListaStatus.Add(string.Concat(item.denominacao , " (", item.ListaCesv.Where(x => x.armazemId == TokenAtual.armazemId).Count(),")"));
                IdStatus.Add(Convert.ToInt32(item.statusId));

                foreach (var subitem in item.ListaCesv)
                {
                    subitem.id = Convert.ToInt32(subitem.cesvId);
                    subitem.statusInicioId = subitem.statusInicioId;
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
                    db.InsertOrReplaceAsync(subitem);
                }
                Status statusTemp = new Status();
                statusTemp.cor = item.cor;
                statusTemp.denominacao = item.denominacao;
                statusTemp.statusId = item.statusId;
                statusTemp.id = Convert.ToInt32(item.statusId);
                db.InsertOrReplaceAsync(statusTemp);
            }

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.VeiculosSituacao, ListaStatus);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                TokenAtual.statusId = IdStatus[args.Position].ToString();
                db.InsertOrReplaceAsync(TokenAtual);
                StartActivity(typeof(VeiculosSituacaoOprActivity));
            };
            
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        public static VeiculosSituacaoActivity GetInstace()
        {
            return activity;
        }

        static VeiculosSituacaoActivity activity;

        public List<string> ListaStatus = new List<string>();
        public List<int> IdStatus = new List<int>();
    }
}


