using SQLite;
using System.Collections.Generic;

namespace Sample.Android.Resources.Model
{

    public class Empresa
    {
        [PrimaryKey]
        public int? id { get; set; }

        public string empresaId { get; set; }
        
        public string denominacao { get; set; }
        
        public override string ToString()
        {
            return string.Format("[Empresa:id={0}, empresaId={1}, denominacao={2}]", id, empresaId, denominacao);
        }
    }

}

/*
{ EmpresaId = 1, Denominacao = "SEARA"  }: [
        {
            "ArmazemId": 1034,
            "Denominacao": "SEARA - PLANTA 01",
            "Latitude": "-24.419415",
            "Longitude": "-53.828311"
        }
    ]

*/
