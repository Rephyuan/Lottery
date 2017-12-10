using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
namespace Model.Member
{
    /// <summary>
    /// Summary description for Define
    /// </summary>
    public class Define
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);

        public Define()
        {

        }

        public class MemberLevels
        {
            public const int SA = 9;
            public const int UpAgent = 8;
            public const int Agent = 7;
            public const int Member = 1;
        }

        public static Dictionary<int, string> MemberLevelsLangMap = new Dictionary<int, string>
        {
            { MemberLevels.SA , "MEMBERLEVELS_" + MemberLevels.SA },
            { MemberLevels.UpAgent , "MEMBERLEVELS_" + MemberLevels.UpAgent },
            { MemberLevels.Agent , "MEMBERLEVELS_" + MemberLevels.Agent },
            { MemberLevels.Member , "MEMBERLEVELS_" + MemberLevels.Member },
        };

        public class MemberStauts
        {
            public const string Enable = "1";
            public const string Disable = "2";
            public const string NeedToChangePassword = "3";
        }

        public static Dictionary<string, string> MemberStautsLangMap = new Dictionary<string, string>
        {
            { MemberStauts.Enable , "MEMBERSTATUS_" + MemberStauts.Enable },
            { MemberStauts.Disable , "MEMBERSTATUS_" + MemberStauts.Disable },
            { MemberStauts.NeedToChangePassword , "MEMBERSTATUS_" + MemberStauts.NeedToChangePassword }
        };

        public class TransactionType
        {
            public const string Deposit = "deposit";
            public const string Debits = "debits";
        }

        public static Dictionary<string, string> TransactionTypesLangMap = new Dictionary<string, string>
        {
            { TransactionType.Deposit , TransactionType.Deposit },
            { TransactionType.Debits , TransactionType.Debits }
        };

        public int GetNextLevelId(int levelId)
        {
            if (levelId == MemberLevels.SA)
            {
                return MemberLevels.UpAgent;
            }
            if (levelId == MemberLevels.UpAgent)
            {
                return MemberLevels.Agent;
            }
            if (levelId == MemberLevels.Agent)
            {
                return MemberLevels.Member;
            }
            return 0;
        }

        public int UsernameValidate(string username) //  2 字元錯誤 1 長度錯誤 0 確認OK
        {
            if (new System.Text.RegularExpressions.Regex("^[A-Za-z0-9]+$").IsMatch(username) == false)
                return 2;
            else if (new System.Text.RegularExpressions.Regex("^[A-Za-z0-9]{4,10}$").IsMatch(username) == false)
                return 1;
            else
                return 0;
        }

        public int PasswordValidate(string password)
        {
            if ((new System.Text.RegularExpressions.Regex("^[A-Za-z0-9]+$")).IsMatch(password) == false)
                return 2;
            else if (new System.Text.RegularExpressions.Regex("^[A-Za-z0-9]{4,10}$").IsMatch(password) == false)
                return 1;
            else
                return 0;
        }

        public int TitleValidate(string title)
        {
            if ((new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5\0-9A-Za-z]+$")).IsMatch(title) == false)
                return 2;
            else if (new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5\0-9A-Za-z]{1,15}$").IsMatch(title) == false)
                return 1;
            else
                return 0;
        }

        private Dictionary<int, member> members = new Dictionary<int, member>();

        private member GetMember(int id)
        {
            member qmember = new member();

            if (!members.TryGetValue(id, out qmember))
            {
                string select_str = "select id,username,password,levelId,parentId,companyId,walletAmount, loginGuid , betSetting , status,l9,l8,l7 from [lottery].[dbo].[member] with(nolock)";
                string where_str = " Where id = @id";
                qmember = conn.Query<member>(select_str + where_str, new { id = id }).FirstOrDefault();

                if (qmember != null)
                {
                    members.Add(id, qmember);
                }
            }

            return qmember;
        }

        public string GetMemberUsername(int id)
        {

            return GetMember(id).username;
        }

        public string GetMemberPassword(int id)
        {
            return GetMember(id).password;
        }

        public int GetMemberLevelId(int id)
        {
            return GetMember(id).levelId;
        }

        public int GetMemberParentId(int id)
        {
            return GetMember(id).parentId;
        }

        public int GetMemberCompanyId(int id)
        {
            return GetMember(id).companyId;
        }

        public decimal GetMemberWalletAmount(int id)
        {
            return GetMember(id).walletAmount;
        }

        public string GetMemberBetSetting(int id)
        {
            return GetMember(id).betSetting;
        }

        public int? GetMemberL9(int id)
        {
            return GetMember(id).l9;
        }

        public int? GetMemberL8(int id)
        {
            return GetMember(id).l8;
        }

        public int? GetMemberL7(int id)
        {
            return GetMember(id).l7;
        }

        public int? GetMemberL1(int id)
        {
            return GetMember(id).l1;
        }

        public int? GetMemberLN(int id, int levelId)
        {
            int? targetMemberLN = null;
            if (levelId == 9)
            {
                targetMemberLN = GetMemberL9(id);
            }
            else if (levelId == 8)
            {
                targetMemberLN = GetMemberL8(id);
            }
            else if (levelId == 7)
            {
                targetMemberLN = GetMemberL7(id);
            }

            return targetMemberLN;
        }

        public string GetMemberStatus(int id)
        {
            return GetMember(id).status;
        }

        public string GetMemberGuid(int id)
        {
            return GetMember(id).loginGuid;
        }

        public decimal GetMemberSingleBetMinValue(int id , string betType)
        {
            string betSetting = GetMemberBetSetting(id);

            var betSettingObj = JsonConvert.DeserializeObject<JObject>(betSetting);

            decimal singleBetMinValue = decimal.Parse(betSettingObj["lottery"]["betType"][betType]["a"].ToString());

            return singleBetMinValue;
        }

        public decimal GetMemberSingleBetMaxValue(int id, string betType)
        {
            string betSetting = GetMemberBetSetting(id);

            var betSettingObj = JsonConvert.DeserializeObject<JObject>(betSetting);

            decimal singleBetMaxValue = decimal.Parse(betSettingObj["lottery"]["betType"][betType]["b"].ToString());

            return singleBetMaxValue;
        }

        public decimal GetMemberBetMaxValueByBetType(int id, string betType)
        {
            string betSetting = GetMemberBetSetting(id);

            var betSettingObj = JsonConvert.DeserializeObject<JObject>(betSetting);

            decimal betMaxValueByBetType = decimal.Parse(betSettingObj["lottery"]["betType"][betType]["c"].ToString());

            return betMaxValueByBetType;
        }

        public decimal GetMemberAgentBetMaxValue(int id, string betType)
        {
            string betSetting = GetMemberBetSetting(id);

            var betSettingObj = JsonConvert.DeserializeObject<JObject>(betSetting);

            decimal agentBetMaxValue = decimal.Parse(betSettingObj["lottery"]["global"]["d"].ToString());

            return agentBetMaxValue;
        }

        public decimal GetMemberAgentPayProportion(int id)
        {
            string betSetting = GetMemberBetSetting(id);

            var betSettingObj = JsonConvert.DeserializeObject<JObject>(betSetting);

            decimal agentPayProportion = decimal.Parse(betSettingObj["lottery"]["global"]["r"].ToString());

            return agentPayProportion;
        }

        public int GetMemberId(string username)
        {
            string select_str = "select id from [lottery].[dbo].[member] with(nolock)";
            string where_str = " Where username = @username";
            var m = conn.Query<member>(select_str + where_str,
                new { username = username }).FirstOrDefault();
            if (m == null)
            {
                return 0;
            }
            return m.id;
        }

        public int GetMemberId(string externalId, int parentId)
        {
            string select_str = "select id from [lottery].[dbo].[member] with(nolock)";
            string where_str = " Where externalId = @externalId and parentId = @parentId";
            var m = conn.Query<member>(select_str + where_str,
                new { externalId = externalId, parentId = parentId }).FirstOrDefault();
            if (m == null)
            {
                return 0;
            }
            return m.id;
        }

        public bool CheckMemberUsernameExist(string username) // 變更 method name checkusernameexist
        {
            string select_str = "select id from [lottery].[dbo].[member] with(nolock)";
            string where_str = " Where username = @username ";
            var m = conn.Query<member>(select_str + where_str,
                new { username = username }).FirstOrDefault();

            if (m != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckMemberExternalIdExist(string externalId, int parentId) // 變更 method name checkusernameexist
        {
            string select_str = "select id from [lottery].[dbo].[member] with(nolock)";
            string where_str = " Where externalId = @externalId and parentId = @parentId ";
            var m = conn.Query<member>(select_str + where_str,
                new { externalId = externalId, parentId = parentId }).FirstOrDefault();

            if (m != null)
            {
                return true;
            }
            return false;
        }

        public bool CheckMemberIdExist(int id) // 變更 method name checkusernameexist
        {
            string select_str = "select id from [lottery].[dbo].[member] with(nolock)";
            string where_str = " Where id = @id ";
            var m = conn.Query<member>(select_str + where_str,
                new { id = id }).FirstOrDefault();

            if (m != null)
            {
                return true;
            }
            return false;
        }
    }
}