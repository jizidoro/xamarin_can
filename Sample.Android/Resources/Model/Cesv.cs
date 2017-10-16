using System.Collections.Generic;
using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Cesv
    {
        [PrimaryKey]
        public int? id { get; set; }

        public string cesvId { get; set; }
        
        public string numero { get; set; }
        
        public string statusInicio { get; set; }

        public string statusDestino { get; set; }

        public override string ToString()
        {
            return string.Format("[Cesv:id={0}, cesvId={1}, numero={2}, statusInicio={3}, statusDestino={4}]", id, cesvId, numero, statusInicio, statusDestino);
        }
    }

}

//raw {"{ EmpresaId = 1, Denominacao = SEARA  }":[{"ArmazemId":1034,"Denominacao":"SEARA - PLANTA 01","Latitude":"-24.419415","Longitude":"-53.828311"}]}
/*
"{ EmpresaId = 1, Denominacao = SEARA  }": 
    [
        {
            "ArmazemId": 1034,
            "Denominacao": "SEARA - PLANTA 01",
            "Latitude": "-24.419415",
            "Longitude": "-53.828311"
        }
    ]

*/
