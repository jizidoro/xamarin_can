using Android.App;
using r = Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Android.Util;

namespace Sample.Android
{
    [Activity(Label = "Ações")]
    public class LoginActivity : Activity
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "bancoB3.db3");
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            var db = new SQLiteAsyncConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var dadosPermissao = db.Table<Permissao>();

            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();
            var permissoes = await dadosPermissao.Where(x => x.armazemId == TokenAtual.armazemId).OrderBy(x => x.denominacao).ToListAsync();

            TokenAtual.numeroCesv = "";
            TokenAtual.cesvId = "";
            db.InsertOrReplaceAsync(TokenAtual);

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

            LinearLayout.LayoutParams linearLayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            linearLayoutParams.SetMargins(10, 0, 10, 0);
            //linearLayoutParams.Weight = Convert.ToSingle(0.5);

            for (int n = 0; n < permissoes.Count ; n++)
            {
                var aButton = new Button(this);
                
                aButton.LayoutParameters = linearLayoutParams;
                aButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));

                switch (permissoes[n].denominacao)
                {
                    case "AppPatioAlterarSitucao":
                        aButton.Text = "Alterar Situação";
                        aButton.Click += BtnCriar_Click1;
                        break;
                    case "AppPatioVeiculoSituacao":
                        aButton.Text = "Veiculos por Situação";
                        aButton.Click += BtnCriar_Click2;
                        break;
                    case "AppPatioHistoricoAlertas":
                        aButton.Text = "Historico de Alertas";
                        aButton.Click += BtnCriar_Click3;
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                aButton.SetTextSize(ComplexUnitType.Sp, 25);
                mainLayout.AddView(aButton);
            }

            var configButton = new Button(this);
            configButton.LayoutParameters = linearLayoutParams;
            configButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
            configButton.Text = "Configuração";
            configButton.Click += BtnCriar_Click4;
            configButton.SetTextSize(ComplexUnitType.Sp, 25);
            mainLayout.AddView(configButton);

            var logoutButton = new Button(this);
            logoutButton.LayoutParameters = linearLayoutParams;
            logoutButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
            logoutButton.Text = "Sair";
            logoutButton.Click += BtnCriar_Click5;
            logoutButton.SetTextSize(ComplexUnitType.Sp, 25);
            mainLayout.AddView(logoutButton);
        }


        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);

            string requestBody = string.Format("");
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

        public async void webRequestTeste()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "bancoB3.db3");

            var db = new SQLiteAsyncConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var dadosConfiguracao = db.Table<Configuracao>();
            var configuracao = await dadosConfiguracao.FirstOrDefaultAsync();
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
            
        }

        private void BtnCriar_Click1(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Alterar Situacao...,", ToastLength.Short).Show();
            StartActivity(typeof(AlterarSituacaoActivity));
            Finish();
        }

        private void BtnCriar_Click2(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "VeiculosSituacaoActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(VeiculosSituacaoActivity));
            Finish();
        }

        private void BtnCriar_Click3(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "HistoricoAlertasActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(HistoricoAlertasActivity));
            Finish();
        }

        private void BtnCriar_Click4(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "ConfiguracaoActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(ConfiguracaoActivity));
            Finish();
        }

        private void BtnCriar_Click5(object sender, EventArgs e)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            db.ExecuteAsync("DELETE FROM Token");
            
            StartActivity(typeof(MainActivity));
            FinishAffinity();
        }
    }
}