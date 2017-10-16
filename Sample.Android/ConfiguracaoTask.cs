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
    string usuario, senha;
    WebResponse myWebResponse = null;

    public ConfiguracaoTask(Context context)
    {
        _context = context;
    }

    protected override void OnPreExecute()
    {
        base.OnPreExecute();

        _progressDialog = ProgressDialog.Show(_context, "Configuração", "Por favor aguarde...");
    }

    protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
    {
        usuario = @params[0].ToString();
        senha = @params[1].ToString();

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        var db2 = new SQLiteConnection(dbPath);
        db2.Close();
        
        var db = new SQLiteConnection(dbPath);
        var dadosConfiguracao = db.Table<Configuracao>();

        var configuracao = dadosConfiguracao.FirstOrDefault();
        db.Close();

        try
        {
            string url = "http://" + configuracao.endereco + "/Api/GerenciamentoPatio/GetApiVersion";
            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myWebRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myWebRequest.Method = "GET";

            myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            responseStream.Close();
            myWebResponse.Close();
            


        }
        catch (WebException e)
        {
            if (e.Status == WebExceptionStatus.ProtocolError)
            {
                myWebResponse = (HttpWebResponse)e.Response;
                var erro = ("Errorcode: {0}", myWebResponse);
            }
            else
            {
                var erro = ("Error: {0}", e.Status);
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