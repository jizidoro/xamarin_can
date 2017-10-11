using SQLite;
using System.Collections.Generic;

namespace Sample.Android.Resources.Model
{

    public class Empresa
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public string EmpresaId { get; set; }
        
        public string Denominacao { get; set; }

        public List<Armazem> ListaArmazens { get; set; }

        public override string ToString()
        {
            return string.Format("[Empresa:Id={0}, EmpresaId={1}, Denominacao={2}]", Id, EmpresaId, Denominacao);
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
