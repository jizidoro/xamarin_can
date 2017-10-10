﻿using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;
using System;

namespace Sample.Android.Resources.Model
{

    public class Token
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Login { get; set; }

        public string LoginId { get; set; }

        public string Armazem { get; set; }

        public string ArmazemId { get; set; }
        
        public string access_token { get; set; }
        
        public string token_type { get; set; }

        public int expires_in { get; set; }

        public DateTime data_att_token { get; set; }

        public override string ToString()
        {
            return string.Format("[Token: id={0}, access_token={1}, token_type={2},expires_in={3},data_att_token={4}]", Id, access_token, token_type, expires_in, data_att_token);
        }
    }

}