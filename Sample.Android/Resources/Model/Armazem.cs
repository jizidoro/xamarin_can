using System.Collections.Generic;
using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Armazem
    {
        [PrimaryKey]
        public int? id { get; set; }

        public string armazemId { get; set; }
        
        public string denominacao { get; set; }
        
        public string latitude { get; set; }

        public string longitude { get; set; }

        public override string ToString()
        {
            return string.Format("[Armazem:id={0}, armazemId={1}, denominacao={2}, latitude={3}, longitude={4}]", id, armazemId, denominacao, latitude, longitude);
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
