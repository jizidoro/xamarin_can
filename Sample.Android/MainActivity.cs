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
        Button btnCriar;
        Button btnLogin;
        int LoginId;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            createDatabase();


            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnCriar = FindViewById<Button>(Resource.Id.btnRegistrar);
            txtUsuario = FindViewById<EditText>(Resource.Id.txtUsuario);
            txtSenha = FindViewById<EditText>(Resource.Id.txtSenha);

            btnLogin.Click += BtnLogin_Click;
            btnCriar.Click += BtnCriar_Click;
            

        }

        private void BtnCriar_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegistrarActivity));
        }

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);

            //string requestBody = string.Format("{{\"grant_type\":\"{0}\",\"username\":\"{1}\",\"password\":\"{2}\"}}", "password", txtUsuario.Text, txtSenha.Text);
            string requestBody = string.Format("grant_type=password&username={0}&password={1}", "admin", "admin@123");
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
                try
                {
                    Token teste = JsonConvert.DeserializeObject<Token>(result);
                    teste.LoginId = LoginId;
                    var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Usuario2.db3"));
                    connection.InsertAsync(teste);
                    Toast.MakeText(this, teste.access_token.ToString(), ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Usuario2.db3");
                var db = new SQLiteConnection(dbPath);

                var dados = db.Table<Login>();
                var login = dados.Where(x => x.usuario == txtUsuario.Text && x.senha == txtSenha.Text).FirstOrDefault();

                var dadosToken = db.Table<Token>();
                var TokenAtual = dadosToken.Where(x => x.LoginId == login.id).FirstOrDefault();
                LoginId = login.id;
                //teste
                if (LoginId > 0 && TokenAtual.Id == 0)
                {
                    System.Uri myUri = new System.Uri("http://192.168.17.102:13359/Token");
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);
                    
                }

                if (login != null)
                {
                    Toast.MakeText(this, "Login realizado com sucesso", ToastLength.Short).Show();
                    var atividade2 = new Intent(this, typeof(LoginActivity));
                    //pega os dados digitados em txtUsuario
                    atividade2.PutExtra("nome", FindViewById<EditText>(Resource.Id.txtUsuario).Text);
                    StartActivity(atividade2);
                }
                else
                {
                    Toast.MakeText(this, "Nome do usuário e/ou Senha inválida(os)", ToastLength.Short).Show();
                }
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
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Usuario2.db3"));
                connection.CreateTableAsync<Login>();
                connection.CreateTableAsync<Token>();
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