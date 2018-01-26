using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Sample.Android
{

    [Activity(Label = "Configuração")]
    public class ConfiguracaoActivity : Activity
    {
        Button btnConfigurar;
        EditText txtEndereco;
        string TokenUsuario { get; set; }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Configuracao);
            
            txtEndereco = FindViewById<EditText>(Resource.Id.txtEndereco);

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "bancoB3.db3");
            var db = new SQLiteAsyncConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            
            btnConfigurar = FindViewById<Button>(Resource.Id.btnConfigurar);
            btnConfigurar.Click += delegate (object sender, EventArgs e)
            {
                if (!string.IsNullOrEmpty(txtEndereco.Text))
                {
                    new ConfiguracaoTask(this).Execute(txtEndereco.Text);
                }
                
            };

            var configuracao = await dadosConfiguracao.FirstOrDefaultAsync();

            if (configuracao != null)
            {
                txtEndereco.Text = configuracao.endereco;
            }
        }

        
        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}