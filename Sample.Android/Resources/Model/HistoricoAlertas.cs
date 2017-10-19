using System.Collections.Generic;
using SQLite;

namespace Sample.Android.Resources.Model
{
    public class HistoricoAlertas
    {
        [PrimaryKey]
        public int? id { get; set; }

        public string armazemId { get; set; }
        
        public string texto { get; set; }

        public string tipo { get; set; }

        public string dataCriacao { get; set; }

        public string historicoAlertasId { get; set; }

        public override string ToString()
        {
            return string.Format("[HistoricoAlertas:id={0}, armazemId={1}, texto={2}, tipo={3}, dataCriacao={3}, historicoAlertasId ={4}]", id, armazemId, texto, tipo, dataCriacao , historicoAlertasId);
        }
    }
}

