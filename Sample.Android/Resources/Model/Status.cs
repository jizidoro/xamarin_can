using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Status
    {
        [PrimaryKey]
        public int? id { get; set; }

        public string statusId { get; set; }
        
        public string denominacao { get; set; }
        
        public string cor { get; set; }

        public override string ToString() => string.Format("[Status:id={0}, statusId={1}, denominacao={2}, cor={3}]", id, statusId, denominacao, cor);
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
