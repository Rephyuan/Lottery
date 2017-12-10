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
    /// Summary description for List
    /// </summary>
    public class List
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
        GlobalFunc glbf = new GlobalFunc();

        public List()
        {

        }

        public class AgentListStruct
        {
            public int? L8;
            public int? L7;
            public int? LevelId;
            public string status;
            public DateTime? CreateBeginDateTime;
            public DateTime? CreateEndDateTime;
        }

        /// <summary>
        /// 取得列表
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns>a:username; b:levelId; c:externalId; d:loginDateTime; e:loginIP; f:createDateTime; g:updatedatetime; h:updateUserId</returns>
        public JArray AgentListStructHandle(AgentListStruct br)
        {
            JArray ja = new JArray();

            Define define = new Define();
            string select_str = "select * from [lottery].[dbo].[member] with(nolock) ";
            string where_str = "";

            where_str += glbf.GetSQLSameLine();

            if (br.L8 != null)
            {
                if (where_str != "") where_str += " and ";
                where_str += " l8 = " + br.L8.ToString();
            }

            if (br.L7 != null)
            {
                if (where_str != "") where_str += " and ";
                where_str += " l7 = " + br.L7.ToString();
            }

            if (br.LevelId != null)
            {
                if (where_str != "") where_str += " and ";
                where_str += " levelId = " + br.LevelId.ToString();
            }

            if (br.CreateBeginDateTime.HasValue)
            {
                if (where_str != "") where_str += " and ";
                where_str += " createDateTime >= '" + ((DateTime)br.CreateBeginDateTime).ToString("yyyy/MM/dd HH:mm:ss") + "'";
            }

            if (br.CreateEndDateTime.HasValue)
            {
                if (where_str != "") where_str += " and ";
                where_str += " createDateTime <= '" + ((DateTime)br.CreateEndDateTime).ToString("yyyy/MM/dd HH:mm:ss") + "'";
            }

            if (br.status != null)
            {
                if (where_str != "") where_str += " and ";
                where_str += " status = '" + br.status + "'";
            }
            List<member> agentList = new List<member>();

            if (where_str != "")
                agentList.AddRange(conn.Query<member>(select_str + " where " + where_str).ToList());
            else
                agentList.AddRange(conn.Query<member>(select_str).ToList());

            foreach (var m in agentList)
            {
                JToken jt = new JObject();
                jt["a"] = m.username;
                jt["b"] = m.levelId;
                jt["c"] = m.companyId;

                if (m.loginDateTime.HasValue == false)
                {
                    jt["d"] = "";
                }
                else
                {
                    jt["d"] = m.loginDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                }
                if (m.loginIP == null)
                {
                    jt["e"] = "";
                }
                else
                {
                    jt["e"] = m.loginIP;
                }
                jt["f"] = m.createDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss");

                if (m.updateDateTime.HasValue == false)
                {
                    jt["g"] = "";
                }
                else
                {
                    jt["g"] = m.updateDateTime.Value.ToString("yyyy/MM/dd HH:mm:ss");
                }
                if (m.updateUserId.HasValue)
                {
                    jt["h"] = "";
                }
                else
                {
                    jt["h"] = m.updateUserId;
                }
                jt["i"] = m.id;

                jt["j"] = m.l9;
                jt["k"] = m.l8;
                jt["l"] = m.l7;
                jt["m"] = m.l1;
                jt["n"] = m.status;
                jt["o"] = m.nickname;

                ja.Add(jt);
            }
            return ja;
        }
    }
}