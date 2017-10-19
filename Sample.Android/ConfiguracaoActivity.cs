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

    [Activity(Label = "ConfiguracaoActivity")]
    public class ConfiguracaoActivity : Activity
    {
        Button btnConfigurar;
        EditText txtEndereco;
        EditText txtPorta;
        string TokenUsuario { get; set; }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Configuracao);

            txtPorta = FindViewById<EditText>(Resource.Id.txtPorta);
            txtEndereco = FindViewById<EditText>(Resource.Id.txtEndereco);

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
            var db = new SQLiteAsyncConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();
            
            btnConfigurar = FindViewById<Button>(Resource.Id.btnConfigurar);
            btnConfigurar.Click += delegate (object sender, EventArgs e)
            {
                db.ExecuteAsync("DELETE FROM Configuracao");
                
                new ConfiguracaoTask(this).Execute(txtEndereco.Text, txtPorta.Text);
                
            };

            var configuracao = await dadosConfiguracao.FirstOrDefaultAsync();

            if (configuracao != null)
            {
                var repartidor = configuracao.endereco.Split(':');
                txtEndereco.Text = repartidor[0];
                txtPorta.Text = repartidor[1];
            }

        }

        
        public override void OnBackPressed()
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}