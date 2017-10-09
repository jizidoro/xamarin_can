using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

namespace Sample.Android.Resources.Model
{

    public class Token
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Login))]     // Specify the foreign key
        public int LoginId { get; set; }
        
        public string access_token { get; set; }

        [MaxLength(25)]
        public string token_type { get; set; }

        [MaxLength(35)]
        public string expires_in { get; set; }

        public override string ToString()
        {
            return string.Format("[Token: id={0}, access_token={1}, token_type={2},expires_in={3}]", Id, access_token, token_type, expires_in);
        }
    }

}