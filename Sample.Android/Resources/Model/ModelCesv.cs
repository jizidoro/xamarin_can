using System.Collections.Generic;

namespace Sample.Android.Resources.Model
{

    public class ModelCesv
    {
        public int? id {get; set; }

        public string cesvId {get; set; }
        
        public string numero {get; set; }
        
        public string statusInicio {get; set; }

        public string statusInicioId { get; set; }

        public string CorStatusInicio { get; set; }

        public string statusDestino {get; set; }

        public string statusDestinoId { get; set; }

        public string placa {get; set; }

        public string nome {get; set; }

        public string telefone {get; set; }

        public string nomeCliente {get; set; }

        public string nomeTransportadora {get; set; }

        public string tipoVeiculo {get; set; }

        public string dataAgendamentoEntrada {get; set; }

        public string armazemId { get; set; }

        public string msg { get; set; }

        public string DataFimAgendamentoPatio { get; set; }

        public string DataInicioAgendamentoPatio { get; set; }

        public List<Status> ListaDestinos { get; set; }
    }

}

