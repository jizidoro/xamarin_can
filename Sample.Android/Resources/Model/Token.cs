using SQLite;
using System;

namespace Sample.Android.Resources.Model
{


    public class Token
    {
        [PrimaryKey, AutoIncrement]
        public int? id { get; set; }

        //public Login Login { get; set; }

        [MaxLength(15)]
        public string loginId { get; set; }

        //public Armazem Armazem { get; set; }

        [MaxLength(15)]
        public string armazemId { get; set; }

        //public Empresa Empresa { get; set; }

        [MaxLength(15)]
        public string empresaId { get; set; }

        [MaxLength(15)]
        public string access_token { get; set; }

        [MaxLength(25)]
        public string token_type { get; set; }

        public int expires_in { get; set; }

        //"error": "invalid_grant",
        public string error { get; set; }
        //"error_description": "The user name or password is incorrect."
        public string error_description { get; set; }

        public DateTime data_att_token { get; set; }

        public override string ToString()
        {
            return string.Format("[Token: id={0}, access_token={1}, token_type={2}, expires_in={3}, data_att_token={4}, loginId={5}, armazemId={6}, empresaId={7}]", id, access_token, token_type, expires_in, data_att_token, loginId, armazemId, empresaId);
        }
    }

}