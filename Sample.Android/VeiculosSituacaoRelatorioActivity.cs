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

[Activity(Label = "Dados da Cesv", Icon = "@drawable/icon")]
public class VeiculosSituacaoRelatorioActivity : ListActivity
{
    string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sapoha4.db3");
    protected override async void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);

        var db = new SQLiteAsyncConnection(dbPath);
        var dadosToken = db.Table<Token>();
        var dadosCesv = db.Table<Cesv>();
        var TokenAtual = await dadosToken.Where(x => x.data_att_token >= DateTime.Now).FirstOrDefaultAsync();
        var DadosRelatorioCesv = await dadosCesv.Where(x => x.armazemId == TokenAtual.armazemId && x.cesvId == TokenAtual.cesvId).FirstOrDefaultAsync();

        RelatorioCesv.Add(string.Concat("Numero: ", DadosRelatorioCesv.numero));
        RelatorioCesv.Add(string.Concat("Placa: ", DadosRelatorioCesv.placa));
        RelatorioCesv.Add(string.Concat("Status: ", DadosRelatorioCesv.statusInicio));
        RelatorioCesv.Add(string.Concat("Motorista: ", DadosRelatorioCesv.nome));
        RelatorioCesv.Add(string.Concat("Telefone: ", DadosRelatorioCesv.telefone));
        RelatorioCesv.Add(string.Concat("Cliente: ", DadosRelatorioCesv.nomeCliente));
        RelatorioCesv.Add(string.Concat("Transportadora: ", DadosRelatorioCesv.nomeTransportadora));
        RelatorioCesv.Add(string.Concat("Tipo do veículo: ", DadosRelatorioCesv.tipoVeiculo));
        RelatorioCesv.Add(string.Concat("Data do agendamento: ", DadosRelatorioCesv.dataAgendamentoEntrada));


        ListAdapter = new HomeScreenAdapter(this, RelatorioCesv);
        //ListView.SetBackgroundColor(Color.ParseColor("#ff0000"));
        ListView.TextFilterEnabled = true;


        ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
        {
            if (args.Position == 2)
            {
                TokenAtual.cesvId = DadosRelatorioCesv.cesvId.ToString();
                TokenAtual.numeroCesv = DadosRelatorioCesv.numero.ToString();
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

    public static VeiculosSituacaoRelatorioActivity GetInstace()
    {
        return activity;
    }

    static VeiculosSituacaoRelatorioActivity activity;

    public List<string> RelatorioCesv = new List<string>();
    public List<int> IdCesv = new List<int>();
    public Activity _context;

}