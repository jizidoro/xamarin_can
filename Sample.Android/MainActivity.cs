using Android.App;
using Android.OS;
using Android.Widget;
using SQLite;
using Sample.Android.Resources.Model;
using System;
using System.IO;

namespace Sample.Android
{
    [Activity(Label = "App.Login_SQLite", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText txtUsuario;
        EditText txtSenha;
        Button btnLogin;
        Button btnConfigurar;
        
        private ProgressDialog _progressDialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            createDatabase();
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
            var db2 = new SQLiteConnection(dbPath); db2.Close();
            _progressDialog = new ProgressDialog(this) { Indeterminate = true };
            _progressDialog.SetTitle("Carregando");

            
            /*
            var db3 = new SQLiteAsyncConnection(dbPath);
            var dadosTokenAsync = db3.Table<Token>();
            var TokenAtualAsync = await dadosTokenAsync.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();
            */

            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            txtUsuario = FindViewById<EditText>(Resource.Id.txtUsuario);
            txtSenha = FindViewById<EditText>(Resource.Id.txtSenha);

            btnConfigurar = FindViewById<Button>(Resource.Id.btnConfigurar);
            btnConfigurar.Click += BtnConfigurar_Click;

            btnLogin.Click += BtnLogin_Click;
            
            

            var db = new SQLiteConnection(dbPath);
            var dadosToken = db.Table<Token>();
            Token TokenAtual = null;
            if (dadosToken.Count() > 0)
            {
                TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            }
            db.Close();
            if (TokenAtual != null)
            {
                if (!string.IsNullOrEmpty(TokenAtual.access_token))
                {
                    StartActivity(typeof(SelecionaArmazemActivity));
                    Finish();
                }
            }

        }


        private void BtnConfigurar_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ConfiguracaoActivity));
        }


        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                _progressDialog.Show();
                
                new LoginTask(this).Execute(txtUsuario.Text, txtSenha.Text);
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
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3"));
                connection.CreateTableAsync<Login>();
                connection.CreateTableAsync<Token>();
                connection.CreateTableAsync<Empresa>();
                connection.CreateTableAsync<Armazem>();
                connection.CreateTableAsync<Permissao>();
                connection.CreateTableAsync<Configuracao>();
                connection.CreateTableAsync<Cesv>();
                connection.CreateTableAsync<Status>();

                connection.ExecuteAsync("DELETE FROM Permissao");
                connection.ExecuteAsync("DELETE FROM Empresa");
                connection.ExecuteAsync("DELETE FROM Armazem");
                connection.ExecuteAsync("DELETE FROM Cesv");
                connection.ExecuteAsync("DELETE FROM Status");

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