using System.Collections.Generic;

namespace Sample.Android.Resources.Model
{

    public class ModelArmazem
    {
        public int? id { get; set; }

        public string armazemId { get; set; }
        
        public string denominacao { get; set; }
        
        public string latitude { get; set; }

        public string longitude { get; set; }

        public List<Permissao> ListaPermissoes { get; set; }
        
    }

}

