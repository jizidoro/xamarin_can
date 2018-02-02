using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Sample.Android;
using Sample.Android.Resources.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

[Activity(Label = "Dados da Cesv", Icon = "@drawable/icon")]
public class VeiculosSituacaoRelatorioActivity : ListActivity
{
    string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "bancoB3.db3");
    protected override async void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        var db = new SQLiteAsyncConnection(dbPath);
        var dadosToken = db.Table<Token>();
        var dadosCesv = db.Table<Cesv>();
        var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();
        var DadosRelatorioCesv = await dadosCesv.Where(x => x.armazemId == TokenAtual.armazemId && x.cesvId == TokenAtual.cesvId).FirstOrDefaultAsync();

        RelatorioCesv.Add(string.Concat("Numero: ", DadosRelatorioCesv.numero));
        RelatorioCesv.Add(string.Concat("Placa: ", DadosRelatorioCesv.placa.ToUpper()));
        RelatorioCesv.Add(string.Concat("Status: ", DadosRelatorioCesv.statusInicio));
        RelatorioCesv.Add(string.Concat("Motorista: ", DadosRelatorioCesv.nome));
        RelatorioCesv.Add(string.Concat("Telefone: ", DadosRelatorioCesv.telefone));
        RelatorioCesv.Add(string.Concat("Cliente: ", DadosRelatorioCesv.nomeCliente));
        RelatorioCesv.Add(string.Concat("Transportadora: ", DadosRelatorioCesv.nomeTransportadora));
        RelatorioCesv.Add(string.Concat("Tipo do veículo: ", DadosRelatorioCesv.tipoVeiculo));
        RelatorioCesv.Add(string.Concat("Hora início: ", DadosRelatorioCesv.DataInicioAgendamentoPatio));
        RelatorioCesv.Add(string.Concat("Hora fim: ", DadosRelatorioCesv.DataFimAgendamentoPatio));

        ListAdapter = new HomeScreenAdapter(this, RelatorioCesv);
        //ListView.SetBackgroundColor(Color.ParseColor("#ff0000"));
        ListView.TextFilterEnabled = true;


        ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
        {
            if (args.Position == 2)
            {
                TokenAtual.cesvId = DadosRelatorioCesv.cesvId.ToString();
                TokenAtual.numeroCesv = HttpUtility.UrlEncode(GeraQrCriptografado(DadosRelatorioCesv.numero.ToString()));
                db.InsertOrReplaceAsync(TokenAtual);
                //Toast.MakeText(Application, OprCesv[args.Position], ToastLength.Short).Show();
                StartActivity(typeof(AlterarSituacaoOprActivity));
                Finish();
                VeiculosSituacaoActivity.GetInstace().Finish();
                VeiculosSituacaoOprActivity.GetInstace().Finish();
            }
        };

        ListAdapter = new HomeScreenAdapter(this, RelatorioCesv);
        ListView.FastScrollEnabled = true;
    }

    protected override void OnListItemClick(ListView l, View v, int position, long id)
    {
        var t = RelatorioCesv[position];
        Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
    }

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("clkajsdlkjoizdfoi**9kldkjroijlk=");
    private static readonly byte[] Iv = { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18, 0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };


    public static string GeraQrCriptografado(string dados)
    {
        var bytes = Encoding.UTF8.GetBytes(dados);
        using (var aes = Aes.Create())
        using (MemoryStream mStream = new MemoryStream())
        using (CryptoStream encryptor = new CryptoStream(
            mStream,
            aes.CreateEncryptor(Key, Iv),
            CryptoStreamMode.Write))
        {
            encryptor.Write(bytes, 0, bytes.Length);
            encryptor.FlushFinalBlock();
            var sb = new StringBuilder();
            return Convert.ToBase64String(mStream.ToArray());
        }
    }

    public class HomeScreenAdapter : BaseAdapter<string>
    {
        List<string> items;
        Activity context;
        public HomeScreenAdapter(Activity context, List<string> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is supplied
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            // set view properties to reflect data for the given row
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
            // return the view, populated with data, for display
            if (position == 2)
            {
                view.SetBackgroundColor(Color.ParseColor("#d3d3d3"));
            }
            return view;
        }

    }


    public List<string> RelatorioCesv = new List<string>();
    public List<int> IdCesv = new List<int>();
    public Activity _context;

}