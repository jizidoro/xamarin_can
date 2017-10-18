using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using ZXing;
using ZXing.Mobile;
using System;
using SQLite;
using Sample.Android.Resources.Model;

namespace Sample.Android
{
    [Activity(Label = "AlterarSituacaoQrCodeActivity", Theme = "@android:style/Theme.Holo.Light", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class AlterarSituacaoQrCodeActivity : Activity
    {
        Button buttonScanCustomView;
        Button buttonScanDefaultView;
        Button buttonContinuousScan;
        Button buttonFragmentScanner;
        Button buttonGenerate;

        MobileBarcodeScanner scanner;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Initialize the scanner first so we can track the current context
            MobileBarcodeScanner.Initialize(Application);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.AlterarSituacaoQrCode);

            //Create a new instance of our Scanner
            scanner = new MobileBarcodeScanner();

            buttonScanDefaultView = this.FindViewById<Button>(Resource.Id.buttonScanDefaultView);
            buttonScanDefaultView.Click += async delegate {

                //Tell our scanner to use the default overlay
                scanner.UseCustomOverlay = false;

                //We can customize the top and bottom text of the default overlay
                scanner.TopText = "Para evitar erros na leitura.\n Segure o dispositivo a 15 centimetros do código de barras.";
                scanner.BottomText = "Coloque um código de barras no visor da câmera para digitalizá-lo.";

                //Start scanning
                var result = await scanner.Scan();

                HandleScanResult(result);
            };

            buttonContinuousScan = FindViewById<Button>(Resource.Id.buttonScanContinuous);
            buttonContinuousScan.Click += delegate {

                scanner.UseCustomOverlay = false;

                //We can customize the top and bottom text of the default overlay
                scanner.TopText = "Para evitar erros na leitura.\n Segure o dispositivo a 15 centimetros do código de barras.";
                scanner.BottomText = "Coloque um código de barras no visor da câmera para digitalizá-lo.";

                var opt = new MobileBarcodeScanningOptions();
                opt.DelayBetweenContinuousScans = 3000;

                //Start scanning
                scanner.ScanContinuously(opt, HandleScanResult);
            };

            Button flashButton;
            View zxingOverlay;

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

            buttonFragmentScanner = FindViewById<Button>(Resource.Id.buttonFragment);
            buttonFragmentScanner.Click += delegate {
                StartActivity(typeof(FragmentActivity));
            };

            buttonGenerate = FindViewById<Button>(Resource.Id.buttonGenerate);
            buttonGenerate.Click += delegate {
                StartActivity(typeof(ImageActivity));
            };
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
                var db = new SQLiteConnection(dbPath);
                var dadosToken = db.Table<Token>();
                var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
                TokenAtual.numeroCesv = result.Text;
                db.InsertOrReplace(TokenAtual);
                db.Close();
                StartActivity(typeof(AlterarSituacaoOprActivity));
            }
            else
                msg = "Scanning Canceled!";

            this.RunOnUiThread(() => Toast.MakeText(this, msg, ToastLength.Short).Show());
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


