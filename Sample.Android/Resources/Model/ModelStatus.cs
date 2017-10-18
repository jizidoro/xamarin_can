using System.Collections.Generic;
using SQLite;

namespace Sample.Android.Resources.Model
{

    public class ModelStatus
    {
        public int? id { get; set; }

        public string statusId { get; set; }
        
        public string denominacao { get; set; }
        
        public string cor { get; set; }

        public List<Cesv> ListaCesv { get; set; }
        
    }

}

