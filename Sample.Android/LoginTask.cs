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

public class LoginTask : AsyncTask
{
    private ProgressDialog _progressDialog;
    private Context _context;
    string usuario, senha;
    WebResponse myWebResponse = null;

    public LoginTask(Context context)
    {
        _context = context;
    }

    protected override void OnPreExecute()
    {
        base.OnPreExecute();

        _progressDialog = ProgressDialog.Show(_context, "Logando no sistema", "Por favor aguarde...");
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

        if (configuracao == null)
        {
            _context.StartActivity(typeof(ConfiguracaoActivity));
        }

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
            if (responseStream == null) return null;

            var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            var json = myStreamReader.ReadToEnd();

            responseStream.Close();
            myWebResponse.Close();


            var teste = JsonConvert.DeserializeObject<Token>(json);
            teste.loginId = usuario;
            teste.data_att_token = DateTime.Now.AddSeconds(teste.expires_in);
            var connection = new SQLiteAsyncConnection(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3"));

            if (string.IsNullOrEmpty(teste.error))
            {
                connection.InsertOrReplaceAsync(teste);
            }
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
        
        var db = new SQLiteConnection(dbPath);
        var dadosToken = db.Table<Token>();

        var TokenAtual = dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefault();
        db.Close();
        if (TokenAtual != null)
        {
            if (!string.IsNullOrEmpty(TokenAtual.access_token))
            {
                _context.StartActivity(typeof(SelecionaArmazemActivity));
            }
        }
    }
}