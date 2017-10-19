using Android.App;
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

    [Activity(NoHistory = true ,Label = "AlterarSituacaoOprActivity")]
    public class AlterarSituacaoOprActivity : Activity
    {
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AlterarSituacaoOpr);


            var db = new SQLiteAsyncConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = await dadosConfiguracao.FirstOrDefaultAsync();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();

            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/GetCesvNumero?Numero="+ TokenAtual.numeroCesv + "&ArmazemId="+ TokenAtual.armazemId;
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
            
            var DadosRelatorioCesv = JsonConvert.DeserializeObject<ModelCesv>(json);
            if (string.IsNullOrEmpty(DadosRelatorioCesv.msg))
            { 
                RelatorioCesv.Add(string.Concat("Numero: ", DadosRelatorioCesv.numero));
                RelatorioCesv.Add(string.Concat("Placa: ", DadosRelatorioCesv.placa));
                RelatorioCesv.Add(string.Concat("Status: ", DadosRelatorioCesv.statusInicio));
                RelatorioCesv.Add(string.Concat("Motorista: ", DadosRelatorioCesv.nome));
                RelatorioCesv.Add(string.Concat("Telefone: ", DadosRelatorioCesv.telefone));
                RelatorioCesv.Add(string.Concat("Cliente: ", DadosRelatorioCesv.nomeCliente));
                RelatorioCesv.Add(string.Concat("Transportadora: ", DadosRelatorioCesv.nomeTransportadora));
                RelatorioCesv.Add(string.Concat("Tipo do veículo: ", DadosRelatorioCesv.tipoVeiculo));
                RelatorioCesv.Add(string.Concat("Data do agendamento: ", DadosRelatorioCesv.dataAgendamentoEntrada));



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



                for (int n = 0; n < DadosRelatorioCesv.ListaDestinos.Count -1; n++)
                {
                    var aButton = new Button(this);
                    aButton.Id = Convert.ToInt32(DadosRelatorioCesv.ListaDestinos[n].statusId);
                    aButton.Text = DadosRelatorioCesv.ListaDestinos[n].denominacao;
                    aButton.Click += delegate (object sender, EventArgs e)
                    {
                        string indice = (sender as Button).Id.ToString();

                        string urlPost = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/PostCesvAlteracaoStatus?CesvId=" + DadosRelatorioCesv.cesvId + "&StatusOrigemId=" + DadosRelatorioCesv.statusInicioId + "&StatusDestinoId=" + indice;
                        System.Uri myUriPost = new System.Uri(urlPost);
                        HttpWebRequest myWebRequestPost = (HttpWebRequest)HttpWebRequest.Create(myUriPost);
                        
                        var myHttpWebRequestPost = (HttpWebRequest)myWebRequestPost;
                        myHttpWebRequestPost.PreAuthenticate = true;
                        myHttpWebRequestPost.Method = "POST";
                        myHttpWebRequestPost.ContentLength = 0;
                        myHttpWebRequestPost.Headers.Add("Authorization", "Bearer " + TokenAtual.access_token);
                        myHttpWebRequestPost.Accept = "application/json";

                        var myWebResponsePost = myWebRequestPost.GetResponse();
                        var responseStreamPost = myWebResponsePost.GetResponseStream();

                        var myStreamReaderPost = new StreamReader(responseStreamPost, Encoding.Default);
                        var jsonPost = myStreamReaderPost.ReadToEnd();

                        responseStreamPost.Close();
                        myWebResponsePost.Close();
                        Toast.MakeText(Application, "Cesv alterada com sucesso!", ToastLength.Long).Show();
                        StartActivity(typeof(LoginActivity));
                        Finish();
                    };

                    mainLayout.AddView(aButton);
                }
            }
            else
            {
                Toast.MakeText(Application, DadosRelatorioCesv.msg , ToastLength.Long).Show();
                Finish();

            }

        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }




        public List<string> RelatorioCesv = new List<string>();
        public List<int> IdCesv = new List<int>();
    }
}