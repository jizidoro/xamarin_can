using Android.App;
using Android.Content;
using Android.OS;
using Newtonsoft.Json;
using Sample.Android;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.IO;
using System.Net;
using System.Text;

public class ConfiguracaoTask : AsyncTask
{
    private ProgressDialog _progressDialog;
    private Context _context;
    string txtEndereco;
    WebResponse myWebResponse = null;

    public ConfiguracaoTask(Context context) => _context = context;

    protected override void OnPreExecute()
    {
        base.OnPreExecute();

        _progressDialog = ProgressDialog.Show(_context, "Configuração", "Por favor aguarde...");
    }

    protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
    {
        txtEndereco = @params[0].ToString();

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "bancoB3.db3");
        var db2 = new SQLiteConnection(dbPath);
        db2.Close();
        


        try
        {
            string url = "http://" + txtEndereco + "/Api/GerenciamentoPatio/GetApiVersion";
            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myWebRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myWebRequest.Method = "GET";
            myWebRequest.Timeout = 13000;
            myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            responseStream.Close();
            myWebResponse.Close();


            var db = new SQLiteConnection(dbPath);
            db.Execute("DELETE FROM Configuracao");
            var dadosConfiguracao = db.Table<Configuracao>();

            Configuracao NovaConfiguracao = new Configuracao();
            NovaConfiguracao.endereco = txtEndereco;
            db.InsertOrReplace(NovaConfiguracao);

            db.Close();

            
            _context.StartActivity(typeof(MainActivity));
            ((Activity)_context).Finish();

        }
        catch (WebException e)
        {
            if (e.Status == WebExceptionStatus.ProtocolError)
            {
                myWebResponse = (HttpWebResponse)e.Response;
                //var erro = ("Errorcode: {0}", myWebResponse);
            }
            else
            {
                //var erro = ("Error: {0}", e.Status);
            }
        }


        return true;
    }



    protected override void OnPostExecute(Java.Lang.Object result)
    {
        base.OnPostExecute(result);
        
        _progressDialog.Hide();

    }
}