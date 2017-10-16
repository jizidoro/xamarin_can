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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Configuracao);

            txtPorta = FindViewById<EditText>(Resource.Id.txtPorta);
            txtEndereco = FindViewById<EditText>(Resource.Id.txtEndereco);
            

            btnConfigurar = FindViewById<Button>(Resource.Id.btnConfigurar);
            btnConfigurar.Click += BtnConfigurar_Click;

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
            var db2 = new SQLiteConnection(dbPath);
            db2.Close();

            var db = new SQLiteConnection(dbPath);
            var dadosConfiguracao = db.Table<Configuracao>();

            var configuracao = dadosConfiguracao.FirstOrDefault();
            db.Close();

            if (configuracao != null)
            {
                var repartidor = configuracao.endereco.Split(':');
                txtEndereco.Text = repartidor[0];
                txtPorta.Text = repartidor[1];
            }

        }


        private void BtnConfigurar_Click(object sender, EventArgs e)
        {
            var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3"));
            connection.ExecuteAsync("DELETE FROM Configuracao");



            Configuracao configuracao = new Configuracao();
            configuracao.endereco = txtEndereco.Text+":"+ txtPorta.Text;
            connection.InsertOrReplaceAsync(configuracao);

            new ConfiguracaoTask(this).Execute("admin", "teste_configuracao_endereco");
            Finish();
        }
    }
}