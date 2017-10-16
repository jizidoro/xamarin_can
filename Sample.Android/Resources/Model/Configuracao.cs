using SQLite;
using System;

namespace Sample.Android.Resources.Model
{


    public class Configuracao
    {
        [PrimaryKey, AutoIncrement]
        public int? id { get; set; }

        public string endereco { get; set; }

        public override string ToString()
        {
            return string.Format("[Configuracao: id={0}, endereco={1}]", id, endereco);
        }
    }

}