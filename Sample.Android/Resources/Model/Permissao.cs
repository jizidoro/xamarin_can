using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Permissao
    {
        

        [PrimaryKey]
        public int? id { get; set; }

        public string rotinaId { get; set; }
        public string denominacao { get; set; }
        public string acesso { get; set; }
        public string armazemId { get; set; }

        public override string ToString()
        {
            return string.Format("[Permissao:id={0}, rotinaId={1}, denominacao={2}, acesso={3}, armazemId={4}]", id, rotinaId, denominacao, acesso, armazemId);
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
