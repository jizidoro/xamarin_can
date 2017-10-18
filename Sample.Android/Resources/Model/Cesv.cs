using System.Collections.Generic;
using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Cesv
    {
        [PrimaryKey]
        public int? id {get; set; }

        public string cesvId {get; set; }
        
        public string numero {get; set; }
        
        public string statusInicio {get; set; }

        public string statusInicioId { get; set; }

        public string statusDestino {get; set; }

        public string placa {get; set; }

        public string nome {get; set; }

        public string telefone {get; set; }

        public string nomeCliente {get; set; }

        public string nomeTransportadora {get; set; }

        public string tipoVeiculo {get; set; }

        public string dataAgendamentoEntrada {get; set; }

        public string armazemId { get; set; }

        //public List<Status> ListaDestinos { get; set; }

        public override string ToString() => string.Format("[Cesv:id={0}, cesvId={1}, numero={2}, statusInicio={3}, statusDestino={4} ,placa ={5} , nome ={6} , telefone ={7} , nomeCliente ={8} , nomeTransportadora ={9} , tipoVeiculo ={10} , dataAgendamentoEntrada ={11} ,armazemId ={12} statusInicioId ={13}]", id, cesvId, numero, statusInicio, statusDestino, placa, nome, telefone, nomeCliente, nomeTransportadora, tipoVeiculo, dataAgendamentoEntrada, armazemId, statusInicioId);
    }

}

//raw {"{EmpresaId = 1, Denominacao = SEARA  }":[{"ArmazemId":1034,"Denominacao":"SEARA - PLANTA 01","Latitude":"-24.419415","Longitude":"-53.828311"}]}
/*
"{EmpresaId = 1, Denominacao = SEARA  }": 
    [
        {
            "ArmazemId": 1034,
            "Denominacao": "SEARA - PLANTA 01",
            "Latitude": "-24.419415",
            "Longitude": "-53.828311"
        }
    ]

*/
