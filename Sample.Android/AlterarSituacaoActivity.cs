using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
//
namespace Sample.Android
{

    [Activity(Label = "AlterarSituacaoActivity")]
    public class AlterarSituacaoActivity : Activity
    {
        Button btnOpr, btnQr;
        EditText txtCesv;
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AlterarSituacao);

            var db = new SQLiteAsyncConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();


            txtCesv = FindViewById<EditText>(Resource.Id.txtCesv);

            btnOpr = FindViewById<Button>(Resource.Id.buttonOpr);
            btnOpr.Click += delegate (object sender, EventArgs e)
            {
                
                TokenAtual.numeroCesv = txtCesv.Text;
                db.InsertOrReplaceAsync(TokenAtual);


                StartActivity(typeof(AlterarSituacaoOprActivity));
            };

            btnQr = FindViewById<Button>(Resource.Id.buttonScan);
            btnQr.Click += btnQr_Click;
        }



        private void btnQr_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AlterarSituacaoQrCodeActivity));
        }

    }
}
