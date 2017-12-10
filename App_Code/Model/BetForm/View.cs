using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Model.BetForm
{
    public class View
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);

        public View()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        
    }
}
