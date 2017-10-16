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
    
    [Activity(Label = "VeiculosSituacaoActivity")]
    public class VeiculosSituacaoOprActivity : ListActivity
    {
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        string configuracaoEndereco = "";
        string AccessToken = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);



            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.VeiculosSituacao, Cesvs);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += Btn_ClickLista;
        }

        private void Btn_ClickLista(object sender, AdapterView.ItemClickEventArgs args)
        {

            Toast.MakeText(Application, Cesvs[args.Position], ToastLength.Short).Show();
            //StartActivity(typeof(VeiculosSituacaoOprActivity));
        }

        public void webRequestSituacoes()
        {
            /*
            foreach (var item in teste)
            {
                Cesvs.Add(item.denominacao);
                IdCesvs.Add(Convert.ToInt32(item.statusId));

                foreach (var subitem in item.ListaCesv)
                {
                    subitem.id = Convert.ToInt32(subitem.cesvId);
                    subitem.statusInicio = item.statusId;
                }
                item.id = Convert.ToInt32(item.statusId);
                connection.InsertOrReplaceAsync(item);
            }
            */


        }

        public List<string> Cesvs = new List<string>();
        public List<int> IdCesvs = new List<int>();
    }
}


