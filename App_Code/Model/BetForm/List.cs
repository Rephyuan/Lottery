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
    public class List
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
        GlobalFunc glbf = new GlobalFunc();

        public List()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public class BetFormPeriodStruct
        {
            public string PeriodId;
        }

        /// <summary>
        /// 取得列表
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns>a:username; b:levelId; c:externalId; d:loginDateTime; e:loginIP; f:createDateTime; g:updatedatetime; h:updateUserId</returns>
        public JArray BetFormPeriodStructHandle(BetFormPeriodStruct BetSettlementStruct) // 取出未完成計算的注單 (status = 1)，計算完成後 status = 4。
        {
            //string select_str = "select id from [lottery].[dbo].[member] with(nolock)";
            //string where_str = " Where username = @username ";
            //var i = conn.Query<member>(select_str + where_str,
            //    new { username = BetSettlementStruct.AgentUsername }).FirstOrDefault();

            string select_str = "select id,externalId,betType,betBranch,chooseBall,betAmount,combo,rate,totalBet,winAmount,parentId,l8,l7 from [lottery].[dbo].[betForm] with(nolock) ";
            string where_str = " where  status = @status and periodId = @periodId";

            var agentList = conn.Query<betForm>(select_str + where_str, new { status = Define.BetFormStatus.Effective, periodId = BetSettlementStruct.PeriodId }).ToList();
            JArray ja = new JArray();
            foreach (var m in agentList)
            {
                JToken jt = new JObject();
                jt["id"] = m.id;
                jt["externalId"] = m.externalId;
                jt["parentId"] = m.parentId;
                jt["betType"] = m.betType;
                jt["betBranch"] = m.betBranch;
                jt["chooseBall"] = m.chooseBall;
                jt["betAmount"] = m.betAmount;
                jt["combo"] = m.combo;
                jt["rate"] = m.rate;
                jt["totalBet"] = m.totalBet;
                jt["winAmount"] = m.winAmount;
                jt["l8"] = m.l8;
                jt["l7"] = m.l7;
                ja.Add(jt);
            }
            return ja;
        }

        public class BetFormSummaryStruct
        {
            public List<int> MemberIdList;
            public int? L8;
            public int? L7;
            public int? L1;
            public int? LevelId;
            public string PeriodId;
            public string Status;
            public string BetType;
            public DateTime BeginDateTime;
            public DateTime EndDateTime;
            public bool? IsDeliver;
        }

        public JArray BetFormSummaryStructHandle(BetFormSummaryStruct br)
        {
            JArray ja = new JArray();

            string where_str_bet = glbf.GetSQLSameLine();

            if (br.MemberIdList != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";

                string memberid = "";

                foreach (var item in br.MemberIdList)
                {
                    memberid += item.ToString() + ",";
                }

                memberid.TrimEnd(',');

                where_str_bet += " id in (" + memberid + ")";
            }

            if (br.L8 != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l8 = " + br.L8;
            }

            if (br.L7 != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l7 = " + br.L7;
            }

            if (br.L1 != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l1 = " + br.L1;
            }

            if (br.LevelId != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " levelId = " + br.LevelId;
            }

            string select_str = "SELECT id, username, externalId , companyId, levelId FROM [lottery].[dbo].[member] with(nolock) ";

            List<member> memberList;

            if (where_str_bet == "")
                memberList = conn.Query<member>(select_str).ToList();
            else
                memberList = conn.Query<member>(select_str + " where " + where_str_bet).ToList();

            foreach (var mem in memberList)
            {
                where_str_bet = "";

                if (br.IsDeliver != null)
                {
                    if (where_str_bet != "") where_str_bet += " and ";
                    where_str_bet += " isDeliver = " + Convert.ToInt32(br.IsDeliver).ToString();
                }

                if (br.Status != null)
                {
                    if (where_str_bet != "") where_str_bet += " and ";
                    where_str_bet += " status = " + br.Status.ToString();
                }

                if (br.PeriodId != null)
                {
                    if (where_str_bet != "") where_str_bet += " and ";
                    where_str_bet += " periodId  = '" + br.PeriodId + "'";
                }

                if (br.BetType != null)
                {
                    if (where_str_bet != "") where_str_bet += " and ";
                    where_str_bet += " betType  = " + br.BetType.ToString();
                }

                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " beginDateTime >= '" + br.BeginDateTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";

                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " beginDateTime <= '" + br.EndDateTime.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";

                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l" + mem.levelId.ToString() + " = " + mem.id.ToString();

                select_str = "SELECT count(id) as id, sum(r8) as r8, sum(r7) as r7, sum(r1) as r1 FROM [lottery].[dbo].[betForm] with(nolock)";

                var betFormSum = conn.Query<betForm>(select_str + " where " + where_str_bet).FirstOrDefault();

                JToken jt = new JObject();

                jt["a"] = mem.id;
                if (mem.levelId == Member.Define.MemberLevels.SA)
                {
                    jt["b"] = betFormSum.r8;
                    jt["c"] = betFormSum.r7;
                    jt["d"] = betFormSum.r1;
                }
                if (mem.levelId == Member.Define.MemberLevels.UpAgent)
                {
                    jt["b"] = betFormSum.r8;
                    jt["c"] = betFormSum.r7;
                    jt["d"] = betFormSum.r1;
                }
                if (mem.levelId == Member.Define.MemberLevels.Agent)
                {
                    jt["b"] = "";
                    jt["c"] = betFormSum.r7;
                    jt["d"] = betFormSum.r1;
                }

                if (mem.levelId == Member.Define.MemberLevels.Member)
                {
                    jt["b"] = "";
                    jt["c"] = "";
                    jt["d"] = betFormSum.r1;
                }

                jt["e"] = betFormSum.id; // betform count
                jt["f"] = mem.companyId;
                jt["g"] = mem.username;
                jt["h"] = mem.externalId;

                ja.Add(jt);
            }

            return ja;
        }

        public class BetFormDetailStruct
        {
            public int? L8;
            public int? L7;
            public int? L1;
            public string PeriodId;
            public string Status;
            public string BetType;
            public DateTime? BeginDateTime;
            public DateTime? EndDateTime;
            public bool? IsDeliver;
        }

        public JArray BetFormDetailStructHandle(BetFormDetailStruct br)
        {
            JArray ja = new JArray();

            string where_str_bet = glbf.GetSQLSameLine();

            if (br.IsDeliver != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " isDeliver = " + Convert.ToInt32(br.IsDeliver).ToString();
            }

            if (br.Status != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " status = " + br.Status;
            }

            if (br.PeriodId != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " periodId  = '" + br.PeriodId + "'";
            }

            if (br.BetType != null)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " betType  = " + br.BetType;
            }

            if (br.L8.HasValue)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l8 = " + br.L8.ToString();
            }

            if (br.L7.HasValue)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l7 = " + br.L7.ToString();
            }

            if (br.L1.HasValue)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " l1 = " + br.L1.ToString();
            }

            if (br.BeginDateTime.HasValue)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " beginDateTime >= '" + ((DateTime)br.BeginDateTime).ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";
            }

            if (br.EndDateTime.HasValue)
            {
                if (where_str_bet != "") where_str_bet += " and ";
                where_str_bet += " beginDateTime <= '" + ((DateTime)br.EndDateTime).ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";
            }

            string select_str = "select * from [lottery].[dbo].[betForm] with(nolock)";

            List<betForm> a7 = new List<betForm>();

            if(where_str_bet != "")
                a7.AddRange(conn.Query<betForm>(select_str + " where " + where_str_bet).ToList());

            foreach (var c in a7)
            {
                JToken jt = new JObject();

                jt["a"] = c.id;
                jt["b"] = c.parentId;
                jt["c"] = c.externalId;
                jt["d"] = c.betType;
                jt["e"] = c.r8;
                jt["f"] = c.r7;
                jt["g"] = c.r1;
                jt["h"] = JsonConvert.DeserializeObject<JObject>(c.chooseBall);
                jt["i"] = c.betAmount;
                jt["j"] = c.status;
                jt["k"] = c.rate;
                jt["l"] = c.combo;
                jt["m"] = c.winAmount;
                jt["n"] = c.totalBet;
                jt["o"] = c.betRemark;
                jt["p"] = c.lotteryResult;
                jt["q"] = c.beginDateTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
                jt["r"] = c.periodId;
                jt["s"] = c.username;
                ja.Add(jt);
            }

            return ja;
        }
    }
}