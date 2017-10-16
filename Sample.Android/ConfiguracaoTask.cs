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
    bool ConexaoOk = false;
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
            string url = "http://" + configuracao.endereco + "/Token";
            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myWebRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myWebRequest.Method = "POST";
            myWebRequest.ContentType = "application/x-www-form-urlencoded";

            Stream postStream = myWebRequest.GetRequestStream();

            string requestBody = string.Format("grant_type=password&username={0}&password={1}", usuario, senha);
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);

            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            responseStream.Close();
            myWebResponse.Close();

            ConexaoOk = true;


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

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
        var db2 = new SQLiteConnection(dbPath);
        db2.Close();
        
        var db1 = new SQLiteConnection(dbPath);
        var dadosConfiguracao = db1.Table<Configuracao>();
        var configuracao = dadosConfiguracao.FirstOrDefault();
        db1.Close();

        if (ConexaoOk)
        {
            new AlertDialog.Builder(_context)
            .SetTitle("Falha na autenticação.")
            .SetMessage("Suas configurações de endereço estão incorretas ou o servidor esta indisponivel.")
            .Show();
            _context.StartActivity(typeof(ConfiguracaoActivity));

        }
        else
        {
            new AlertDialog.Builder(_context)
            .SetTitle("Configuração concluida")
            .SetMessage("Redirecionando para o Configuracao!")
            .Show();
            _context.StartActivity(typeof(MainActivity));
        }
    }
}