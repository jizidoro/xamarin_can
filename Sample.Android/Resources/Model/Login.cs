using SQLite;

namespace Sample.Android.Resources.Model
{

    public class Login
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(25),Unique]
        public string usuario { get; set; }

        [MaxLength(15)]
        public string senha { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: id={0}, usuario={1}, senha={2}]", id, usuario, senha);
        }
    }

}
