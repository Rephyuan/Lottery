using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Add
/// </summary>
namespace Model.Member
{
    public class Edit
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);

        public Edit()
        {
            
        }

        public class AgentStruct
        {
            public int AgentId;
            public string Password;
            public string Title;
            public string Nickname;
            public string Stauts;
            public int? UpdateUserId;
            public JToken BetSetting;
            public JToken BetRate;
            public JToken DefaultBetSetting;
        }

        public string AgentStructHandle(AgentStruct agentStruct)
        {
            string update_str = "update [lottery].[dbo].[member]  ";
            string set_str = "";
            string where_str = "";

            if (agentStruct.Password != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " password='" + agentStruct.Password + "'";
            }

            if (agentStruct.Nickname != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " nickname = '" + agentStruct.Password + "'";
            }

            if (agentStruct.Stauts != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " status = " + agentStruct.Stauts;
            }

            if (agentStruct.BetSetting != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " betSetting = '" + JsonConvert.SerializeObject(agentStruct.BetSetting) + "'";
                ModuleName(agentStruct.BetSetting)
            }

            where_str += " id = " + agentStruct.AgentId.ToString();

            if (set_str != "")
            {
                if (agentStruct.UpdateUserId.HasValue)
                {
                    if (set_str != "") set_str += ",";
                    set_str += " updateUserId = " + agentStruct.UpdateUserId;
                }

                set_str += ",";
                set_str += " updateDateTime='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";
                conn.Execute(update_str + " set " + set_str + " where " + where_str);
            }

            update_str = "update [lottery].[dbo].[company]  ";
            set_str = "";
            where_str = "";

            if (agentStruct.Title != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " title = '" + agentStruct.Title + "'";
            }

            if (agentStruct.BetRate != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " betRate = '" + JsonConvert.SerializeObject(agentStruct.BetRate) + "'";
            }

            if (agentStruct.DefaultBetSetting != null)
            {
                if (set_str != "") set_str += ",";
                set_str += " defaultBetSetting = '" + JsonConvert.SerializeObject(agentStruct.DefaultBetSetting) + "'";
            }

            where_str += "  principalId = " + agentStruct.AgentId.ToString();

            if (set_str != "")
            {
                if (agentStruct.UpdateUserId.HasValue)
                {
                    if (set_str != "") set_str += ",";
                    set_str += " updateUserId = " + agentStruct.UpdateUserId;
                }

                set_str += ",";
                set_str += " updateDateTime = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";

                conn.Execute(update_str + " set " + set_str + " where " + where_str);
            }

            return set_str;
        }

        void ModuleName(JToken etSetting,int levelId, int memberId)
        {
            JToken j1 = FFF();
            string s2 = JsonConvert.SerializeObject(j1);
            if (s1 != s2)
            {
                ModuleName(etSetting, levelId, memberId);

            }
        }
        public class AgentLoginStruct
        {
            public int AgentId;
            public string LoginIP;
        }

        public string AgentLoginStructHandle(AgentLoginStruct loginStruct)
        {
            string update_str = "update [lottery].[dbo].[member]  ";
            string set_str = "";
            string where_str = "";

            if (set_str != "") set_str += ",";
            set_str += " loginIP = '" + loginStruct.LoginIP + "'";

            if (set_str != "") set_str += ",";
            set_str += " LoginDateTime='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";

            where_str += " id = " + loginStruct.AgentId.ToString();

            conn.Execute(update_str + " set " + set_str + " where " + where_str);

            string select_str = "select username, parentId , levelId , companyId from [lottery].[dbo].[member] with(nolock)";
            where_str = " Where id = " + loginStruct.AgentId.ToString();

            var q = conn.Query<member>(select_str + where_str, new { id = loginStruct.AgentId }).FirstOrDefault();

            HttpContext.Current.Session["id"] = loginStruct.AgentId;
            HttpContext.Current.Session["username"] = q.username;
            HttpContext.Current.Session["levelId"] = q.levelId;
            HttpContext.Current.Session["parentId"] = q.parentId;
            HttpContext.Current.Session["companyId"] = q.companyId;

            return set_str;
        }

        public class MemberLoginStruct
        {
            public int MemberId;
            public string LoginIP;
        }

        public string MemberLoginStructHandle(MemberLoginStruct loginStruct)
        {
            string update_str = "update [lottery].[dbo].[member] ";
            string set_str = "";
            string where_str = "";

            if (set_str != "") set_str += ",";
            set_str += " loginIP = '" + loginStruct.LoginIP + "'";

            if (set_str != "") set_str += ",";
            set_str += " loginGuid = '" + Guid.NewGuid().ToString("N") + "'";

            if (set_str != "") set_str += ",";
            set_str += " LoginDateTime ='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";

            where_str += " id = " + loginStruct.MemberId.ToString();

            conn.Execute(update_str + " set " + set_str + " where " + where_str);

            string select_str = "select externalId, parentId , levelId , companyId , walletAmount from [lottery].[dbo].[member] with(nolock)";
            where_str = " id = " + loginStruct.MemberId.ToString();

            var q = conn.Query<member>(select_str + " where " + where_str).FirstOrDefault();

            HttpContext.Current.Session["id"] = loginStruct.MemberId;
            HttpContext.Current.Session["externalId"] = q.externalId;
            HttpContext.Current.Session["parentId"] = q.parentId;
            HttpContext.Current.Session["levelId"] = q.levelId;
            HttpContext.Current.Session["companyId"] = q.companyId;
            HttpContext.Current.Session["walletAmount"] = q.walletAmount;

            return set_str;
        }

        public class BalanceTransferStruct
        {
            public int MemberId;
            public decimal WalletAmount;
        }
        public string BalanceTransferStructHandle(BalanceTransferStruct balanceTransferStruct)
        {
            string update_str = "update [lottery].[dbo].[member]  ";
            string set_str = "";
            string where_str = "";

            set_str += " WalletAmount = " + balanceTransferStruct.WalletAmount;

            where_str += " id = " + balanceTransferStruct.MemberId.ToString();

            if (set_str != "") set_str += ",";
            set_str += " updateDateTime='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";

            conn.Execute(update_str + " set " + set_str + " where " + where_str);

            return set_str;
        }

        public class ForwardGameStruct
        {
            public int MemberId;
            public string Guid;
        }

        public string ForwardGameStructHandle(ForwardGameStruct fg)
        {
            string update_str = "update [lottery].[dbo].[member]  ";
            string set_str = "";
            string where_str = "";

            set_str = " loginGuid = '" + fg.Guid + "'";

            where_str += " id = " + fg.MemberId.ToString();

            conn.Execute(update_str + " set " + set_str + " where " + where_str);

            return set_str;
        }
    }
}