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
using System.Linq;
using System.Net;
using System.Text;
using Android.Util;
using Newtonsoft.Json;

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
            await db.InsertOrReplaceAsync(TokenAtual);

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
                        aButton.SetTextSize(ComplexUnitType.Sp, 25);
                        mainLayout.AddView(aButton);
                        break;
                    case "AppPatioVeiculoSituacao":
                        aButton.Text = "Veiculos por Situação";
                        aButton.Click += BtnCriar_Click2;
                        aButton.SetTextSize(ComplexUnitType.Sp, 25);
                        mainLayout.AddView(aButton);
                        break;
                    case "AppPatioHistoricoAlertas":
                        aButton.Text = "Historico de Alertas";
                        aButton.Click += BtnCriar_Click3;
                        aButton.SetTextSize(ComplexUnitType.Sp, 25);
                        mainLayout.AddView(aButton);
                        break;
                }
            }

            if (permissoes.Where(x => x.denominacao.Equals("AppPatioEntradaAbreCancela")).Any())
            {

                var entradaButton = new Button(this);
                entradaButton.LayoutParameters = linearLayoutParams;
                entradaButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
                entradaButton.Text = "Abre Cancela Entrada";
                entradaButton.Click += BtnCriar_Click4;
                entradaButton.SetTextSize(ComplexUnitType.Sp, 25);
                mainLayout.AddView(entradaButton);
            }



            if (permissoes.Where(x => x.denominacao.Equals("AppPatioSaidaAbreCancela")).Any())
            {
                var saidaButton = new Button(this);
                saidaButton.LayoutParameters = linearLayoutParams;
                saidaButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
                saidaButton.Text = "Abre Cancela Saida";
                saidaButton.Click += BtnCriar_Click5;
                saidaButton.SetTextSize(ComplexUnitType.Sp, 25);
                mainLayout.AddView(saidaButton);
            }

            var configButton = new Button(this);
            configButton.LayoutParameters = linearLayoutParams;
            configButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
            configButton.Text = "Configuração";
            configButton.Click += BtnCriar_Click6;
            configButton.SetTextSize(ComplexUnitType.Sp, 25);
            mainLayout.AddView(configButton);




            var logoutButton = new Button(this);
            logoutButton.LayoutParameters = linearLayoutParams;
            logoutButton.SetTextColor(new r.ColorStateList(new int[][] { new int[] { } }, new int[] { Color.White.ToArgb() }));
            logoutButton.Text = "Sair";
            logoutButton.Click += BtnCriar_Click7;
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

        private void BtnCriar_Click6(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "ConfiguracaoActivity...,", ToastLength.Short).Show();
            StartActivity(typeof(ConfiguracaoActivity));
            Finish();
        }

        private void BtnCriar_Click4(object sender, EventArgs e)
        {
            var db2 = new SQLiteConnection(dbPath);
            db2.Close();

            var db = new SQLiteConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = dadosConfiguracao.FirstOrDefault();
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            db.Close();

            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/PostAbreCancelaEntrada?UsuarioCod=" + TokenAtual.loginId;
            System.Uri myUriPost = new System.Uri(url);
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

            var teste = JsonConvert.DeserializeObject<string>(jsonPost);

            Toast.MakeText(this, teste, ToastLength.Short).Show();
        }

        private void BtnCriar_Click5(object sender, EventArgs e)
        {

            var db2 = new SQLiteConnection(dbPath);
            db2.Close();

            var db = new SQLiteConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            var dadosToken = db.Table<Token>();
            var configuracao = dadosConfiguracao.FirstOrDefault();
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            db.Close();
            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/PostAbreCancelaSaida?UsuarioCod=" + TokenAtual.loginId;
            System.Uri myUriPost = new System.Uri(url);
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

            var teste = JsonConvert.DeserializeObject<string>(jsonPost);

            Toast.MakeText(this, teste, ToastLength.Short).Show();
        }

        private void BtnCriar_Click7(object sender, EventArgs e)
        {
            var db = new SQLiteAsyncConnection(dbPath);
            db.ExecuteAsync("DELETE FROM Token");
            
            StartActivity(typeof(MainActivity));
            FinishAffinity();
        }
    }
}