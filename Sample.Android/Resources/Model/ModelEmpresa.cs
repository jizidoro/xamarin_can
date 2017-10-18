using System.Collections.Generic;

namespace Sample.Android.Resources.Model
{

    public class ModelEmpresa
    {
        public int? id { get; set; }

        public string empresaId { get; set; }
        
        public string denominacao { get; set; }

        public List<ModelArmazem> ListaArmazens { get; set; }

    }

}
