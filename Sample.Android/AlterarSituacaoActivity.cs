using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using System;
using SQLite;
using Sample.Android.Resources.Model;
using System.Collections.Generic;
using Android.Views;
using ZXing;
using ZXing.Mobile;
//
namespace Sample.Android
{

    [Activity(Label = "Alterar Situação", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class AlterarSituacaoActivity : Activity
    {
        Button btnOpr;
        Button buttonScanCustomView;
        EditText txtCesv;
        string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        MobileBarcodeScanner scanner;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Initialize the scanner first so we can track the current context
            MobileBarcodeScanner.Initialize(Application);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.AlterarSituacaoQrCode);

            //Create a new instance of our Scanner
            scanner = new MobileBarcodeScanner();

            SetContentView(Resource.Layout.AlterarSituacao);

            var db = new SQLiteAsyncConnection(dbPath);
            var dadosToken = db.Table<Token>();
            var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();


            txtCesv = FindViewById<EditText>(Resource.Id.txtCesv);

            Button flashButton;
            View zxingOverlay;

            btnOpr = FindViewById<Button>(Resource.Id.buttonOpr);
            btnOpr.Click += delegate (object sender, EventArgs e)
            {
                
                TokenAtual.numeroCesv = txtCesv.Text;
                db.InsertOrReplaceAsync(TokenAtual);


                StartActivity(typeof(AlterarSituacaoOprActivity));
            };

            buttonScanCustomView = this.FindViewById<Button>(Resource.Id.buttonScanCustomView);
            buttonScanCustomView.Click += async delegate {

                //Tell our scanner we want to use a custom overlay instead of the default
                scanner.UseCustomOverlay = true;

                //Inflate our custom overlay from a resource layout
                zxingOverlay = LayoutInflater.FromContext(this).Inflate(Resource.Layout.ZxingOverlay, null);

                //Find the button from our resource layout and wire up the click event
                flashButton = zxingOverlay.FindViewById<Button>(Resource.Id.buttonZxingFlash);
                flashButton.Click += (sender, e) => scanner.ToggleTorch();

                //Set our custom overlay
                scanner.CustomOverlay = zxingOverlay;

                //Start scanning!
                var result = await scanner.Scan();

                HandleScanResult(result);
            };
        }



        private void btnQr_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AlterarSituacaoQrCodeActivity));
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (ZXing.Net.Mobile.Android.PermissionsHandler.NeedsPermissionRequest(this))
                ZXing.Net.Mobile.Android.PermissionsHandler.RequestPermissionsAsync(this);
        }

        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
            {
                msg = "Found Barcode: " + result.Text;
                string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");

                var db2 = new SQLiteConnection(dbPath);
                db2.Close();

                var db = new SQLiteConnection(dbPath);
                var dadosToken = db.Table<Token>();
                var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
                TokenAtual.numeroCesv = result.Text;
                db.InsertOrReplace(TokenAtual);
                db.Close();
            }
            else
                msg = "Scanning Canceled!";
            StartActivity(typeof(AlterarSituacaoOprActivity));
            Finish();
            //this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [Java.Interop.Export("UITestBackdoorScan")]
        public Java.Lang.String UITestBackdoorScan(string param)
        {
            var expectedFormat = BarcodeFormat.QR_CODE;
            Enum.TryParse(param, out expectedFormat);
            var opts = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat> { expectedFormat }
            };
            var barcodeScanner = new MobileBarcodeScanner();

            Console.WriteLine("Scanning " + expectedFormat);

            //Start scanning
            barcodeScanner.Scan(opts).ContinueWith(t => {

                var result = t.Result;

                var format = result?.BarcodeFormat.ToString() ?? string.Empty;
                var value = result?.Text ?? string.Empty;

                RunOnUiThread(() => {

                    AlertDialog dialog = null;
                    dialog = new AlertDialog.Builder(this)
                                    .SetTitle("Barcode Result")
                                    .SetMessage(format + "|" + value)
                                    .SetNeutralButton("OK", (sender, e) => {
                                        dialog.Cancel();
                                    }).Create();
                    dialog.Show();
                });
            });

            return new Java.Lang.String();
        }
    }
}
