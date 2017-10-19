using Android.App;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Sample.Android
{
    
    [Activity(Label = "Veículos")]
    public class VeiculosSituacaoOprActivity : ListActivity
    {
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            activity = this;

            var db = new SQLiteAsyncConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var dadosCesv = db.Table<Cesv>();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();

            var ListaCesv = await dadosCesv.Where(x => x.statusInicioId == TokenAtual.statusId && x.armazemId == TokenAtual.armazemId).OrderBy(x => x.placa).ToListAsync();
            
            foreach (var item in ListaCesv.OrderBy(x => x.placa))
            {
                OprCesv.Add(string.Format("{0} - {1}", item.placa, item.nome));
                OprIdCesv.Add(Convert.ToInt32(item.cesvId));
            }

            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.VeiculosSituacaoOpr, OprCesv);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                TokenAtual.cesvId = OprIdCesv[args.Position].ToString();
                db.InsertOrReplaceAsync(TokenAtual);
                //Toast.MakeText(Application, OprCesv[args.Position], ToastLength.Short).Show();
                StartActivity(typeof(VeiculosSituacaoRelatorioActivity));

            };

        }

        public static VeiculosSituacaoOprActivity GetInstace()
        {
            return activity;
        }

        static VeiculosSituacaoOprActivity activity;


        public List<string> OprCesv = new List<string>();
        public List<int> OprIdCesv = new List<int>();
    }
}


