using Android.App;
using r = Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Newtonsoft.Json;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Android.Util;
using DialogConfirmacao;

namespace Sample.Android
{

    [Activity(NoHistory = true ,Label = "Situações")]
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
                    Orientation = Orientation.Vertical, WeightSum = 2,
                    LayoutParameters =
                               new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)

                };

                scrollView.AddView(mainLayout);

                LinearLayout.LayoutParams LayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
                //LayoutParams.Height = LinearLayout.LayoutParams.MatchParent;
                //LayoutParams.Width = LinearLayout.LayoutParams.MatchParent;
                LayoutParams.Weight = 1;
                LayoutParams.SetMargins(10, 10, 10, 10);
                //LinearLayoutParams.Weight = Convert.ToSingle(0.5);

                var texto1 = new TextView(this);
                texto1.Text = string.Concat("Numero: ", DadosRelatorioCesv.numero);
                texto1.SetTextSize(ComplexUnitType.Px, 50);
                mainLayout.AddView(texto1);

                var texto2 = new TextView(this);
                texto2.Text = string.Concat("Placa: ", DadosRelatorioCesv.placa);
                texto2.SetTextSize(ComplexUnitType.Px, 50);
                mainLayout.AddView(texto2);

                var texto3 = new TextView(this);
                texto3.Text = string.Concat("Cliente: ", DadosRelatorioCesv.nomeCliente);
                texto3.SetTextSize(ComplexUnitType.Px, 50);
                mainLayout.AddView(texto3);

                var texto4 = new TextView(this);
                texto4.Text = string.Concat("Transportadora: ", DadosRelatorioCesv.nomeTransportadora);
                texto4.SetTextSize(ComplexUnitType.Px, 50);
                mainLayout.AddView(texto4);

                var texto5 = new TextView(this);
                texto5.Text = string.Concat("Motorista: ", DadosRelatorioCesv.nome);
                texto5.SetTextSize(ComplexUnitType.Px, 50);
                mainLayout.AddView(texto5);

                var texto6 = new TextView(this);
                texto6.Text = string.Concat("Status: ", DadosRelatorioCesv.statusInicio);
                texto6.SetTextSize(ComplexUnitType.Px, 50);
                mainLayout.AddView(texto6);

                


                
                for (int n = 0; n < DadosRelatorioCesv.ListaDestinos.Count; n++)
                {
                    var aButton = new Button(this);
                    aButton.LayoutParameters = LayoutParams;
                    //aButton.SetBackgroundResource(Color.Rgb(10 ,10 ,10));
                    //aButton.DrawingCacheBackgroundColor = Color.ParseColor("#FF6A00");
                    aButton.SetBackgroundColor(Color.ParseColor(DadosRelatorioCesv.ListaDestinos[n].cor));
                    aButton.SetTextColor(Color.ParseColor("#ffffff"));
                    
                    //aButton.;
                    //aButton.SetTextColor(GetColor(Resource.Color.blue_agend));
                    //aButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
                    aButton.Id = Convert.ToInt32(DadosRelatorioCesv.ListaDestinos[n].statusId);
                    aButton.Text = DadosRelatorioCesv.ListaDestinos[n].denominacao;
                    aButton.SetTextSize(ComplexUnitType.Px, 100);
                    aButton.Click += delegate (object sender, EventArgs e)
                    {

                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Confirma a alteração da situação da CESV ?");
                        alert.SetPositiveButton("Confirma", (senderAlert, args) => {
                            string indice = (sender as Button).Id.ToString();

                            string urlPost = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/PostCesvAlteracaoStatus?CesvId=" + DadosRelatorioCesv.cesvId + "&StatusOrigemId=" + DadosRelatorioCesv.statusInicioId + "&StatusDestinoId=" + indice + "&UsuarioCod=" + TokenAtual.loginId;
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
                        });

                        alert.SetNegativeButton("Cancela", (senderAlert, args) => {
                            //Toast.MakeText(Activity, "Cancelado!", ToastLength.Short).Show();
                        });

                        Dialog dialog = alert.Create();
                        dialog.Show();

                        
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