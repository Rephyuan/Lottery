using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Define
/// </summary>

namespace Model.Company
{
    public class Define
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);

        public Define()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private Dictionary<int, company> companies = new Dictionary<int, company>();

        private company GetCompany(int principalId)
        {
            company qcompany = new company();

            if (!companies.TryGetValue(principalId, out qcompany))
            {
                string select_str = "select id,title, betRate , defaultBetSetting from [lottery].[dbo].[company] with(nolock)";
                string where_str = " Where principalId = @principalId";
                qcompany = conn.Query<company>(select_str + where_str, new { principalId = principalId }).FirstOrDefault();

                if (qcompany != null)
                {
                    companies.Add(principalId, qcompany);
                }
            }

            return qcompany;
        }

        public int GetCompanyId(int principalId)
        {
            return GetCompany(principalId).Id;
        }

        public string GetCompanyTitle(int principalId)
        {
            return GetCompany(principalId).title;
        }

        public string GetCompanyBetRate(int principalId)
        {
            return GetCompany(principalId).betRate;
        }

        public string GetCompanyDefaultBetSetting(int principalId)
        {
            return GetCompany(principalId).defaultBetSetting;
        }
    }
}