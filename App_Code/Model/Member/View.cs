using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Model.Member
{
    /// <summary>
    /// Summary description for View
    /// </summary>
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