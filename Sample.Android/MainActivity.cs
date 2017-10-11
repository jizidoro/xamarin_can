using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
using Sample.Android.Resources.Model;
using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sample.Android
{
    [Activity(Label = "App.Login_SQLite", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText txtUsuario;
        EditText txtSenha;
        Button btnLogin;
        string TokenUsuario = "";
        private Object thisLock = new Object();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            createDatabase();


            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            txtUsuario = FindViewById<EditText>(Resource.Id.txtUsuario);
            txtSenha = FindViewById<EditText>(Resource.Id.txtSenha);

            btnLogin.Click += BtnLogin_Click;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha3.db3");
            var db = new SQLiteConnection(dbPath);
            var dadosToken = db.Table<Token>();
            
            var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            db.Close();
            if (TokenAtual != null)
            {
                if (!string.IsNullOrEmpty(TokenAtual.access_token))
                {
                    StartActivity(typeof(SelecionaArmazemActivity));
                }
            }

        }
        /*
        private void BtnCriar_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
        }
        */
        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);

            string requestBody = string.Format("grant_type=password&username={0}&password={1}", txtUsuario.Text, txtSenha.Text);
            //string requestBody = string.Format("grant_type=password&username={0}&password={1}", "admin", "admin@123");
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            var myWebResponse = webRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            if (!string.IsNullOrEmpty(TokenUsuario))
            {
                Token teste = JsonConvert.DeserializeObject<Token>(json);
                teste.loginId = txtUsuario.Text;
                teste.data_att_token = DateTime.Now.AddSeconds(teste.expires_in);
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha3.db3"));
                connection.InsertOrReplaceAsync(teste);
                TokenUsuario = teste.access_token;
                if (!string.IsNullOrEmpty(TokenUsuario))
                {
                    StartActivity(typeof(SelecionaArmazemActivity));
                }
                else
                {
                    Toast.MakeText(this, "Nome do usuário e/ou Senha inválida(os)", ToastLength.Short).Show();
                }
            }
            else
            {
                Token teste = JsonConvert.DeserializeObject<Token>(json);
                teste.loginId = txtUsuario.Text;
                teste.data_att_token = DateTime.Now.AddSeconds(teste.expires_in);
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha3.db3"));
                connection.InsertOrReplaceAsync(teste);
                TokenUsuario = teste.access_token;
                if (!string.IsNullOrEmpty(TokenUsuario))
                {
                    StartActivity(typeof(SelecionaArmazemActivity));
                }
                else
                {
                    Toast.MakeText(this, "Nome do usuário e/ou Senha inválida(os)", ToastLength.Short).Show();
                }
            }

        }

       

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                System.Uri myUri = new System.Uri("http://192.168.17.102:13359/Token");
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);
                

                
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        private string createDatabase()
        {
            try
            {
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha3.db3"));
                connection.CreateTableAsync<Login>();
                connection.CreateTableAsync<Token>();
                connection.CreateTableAsync<Empresa>();
                connection.CreateTableAsync<Armazem>();
                connection.CreateTableAsync<Permissao>();
                //Toast.MakeText(this, "Database created", ToastLength.Short).Show();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
    }
}