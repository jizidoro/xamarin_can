using Android.App;
using Android.OS;
using Android.Widget;
using SQLite;
using Sample.Android.Resources.Model;
using System;
using System.IO;
using Android.Content;

namespace Sample.Android
{
    [Activity(NoHistory = true, Label = "Gerenciamento de Pátio", MainLauncher = true, Icon = "@drawable/ic_icon_b3_2")]
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

            
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
            

            Token TokenAtual = null;


            var db2 = new SQLiteConnection(dbPath);
            db2.CreateTable<Token>();
            db2.Close();

            /*
            var db3 = new SQLiteAsyncConnection(dbPath);
            var dadosTokenAsync = db3.Table<Token>();
            var TokenAtualAsync = await dadosTokenAsync.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();
            */

            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            txtUsuario = FindViewById<EditText>(Resource.Id.txtUsuario);
            txtSenha = FindViewById<EditText>(Resource.Id.txtSenha);

            btnConfigurar = FindViewById<Button>(Resource.Id.btnConfigurar);
            btnConfigurar.Click += delegate (object sender, EventArgs e)
            {
                StartActivity(typeof(ConfiguracaoActivity));
            };

            btnLogin.Click += delegate (object sender, EventArgs e)
            {
                try
                {
                    _progressDialog = new ProgressDialog(this) { Indeterminate = true };
                    _progressDialog.SetTitle("Carregando");

                    _progressDialog.Show();
                    new LoginTask(this).Execute(txtUsuario.Text, txtSenha.Text);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
            };

            
            
            var db = new SQLiteConnection(dbPath);
            var dadosToken = db.Table<Token>();
            
            if (dadosToken.Count() > 0)
            {
                TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
            }
            db.Close();

            string resposta = createDatabase();

            if (TokenAtual != null)
            {
                if (!string.IsNullOrEmpty(TokenAtual.access_token))
                {
                    StartActivity(typeof(SelecionaArmazemActivity));
                    Finish();
                }
            }

        }



        private string createDatabase()
        {
            try
            {
                var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3"));
                connection.CreateTableAsync<Empresa>();
                connection.CreateTableAsync<Armazem>();
                connection.CreateTableAsync<Permissao>();
                connection.CreateTableAsync<Configuracao>();
                connection.CreateTableAsync<Status>();
                connection.CreateTableAsync<Cesv>();
                

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