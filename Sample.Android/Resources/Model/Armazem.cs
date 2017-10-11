using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Armazem
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public string ArmazemId { get; set; }
        
        public string Denominacao { get; set; }
        
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public override string ToString()
        {
            return string.Format("[Armazem:Id[0], ArmazemId={1}, Denominacao={2}, Latitude={3}, Longitude={4}]", Id, ArmazemId, Denominacao, Latitude, Longitude);
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
