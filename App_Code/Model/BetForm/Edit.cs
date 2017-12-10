using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Model.BetForm
{
    public class Edit
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
        public Edit()
        {

        }

        public class LotteryBonusStruct
        {
            public int id;
            public decimal? WinAmount;
            public decimal? r8;
            public decimal? r7;
            public decimal? r1;
            public string Status;
            public string LotteryResult;
        }

        public void LotteryBonusHandle(LotteryBonusStruct LotteryBonusStruct)
        {

            string update_str = "update [lottery].[dbo].[betForm] ";
            string set_str = "";
            string where_str = "";

            if (LotteryBonusStruct.WinAmount.HasValue)
            {
                if (set_str != "") set_str += ",";
                set_str += " winAmount =" + LotteryBonusStruct.WinAmount;
            }

            if (LotteryBonusStruct.r8.HasValue)
            {
                if (set_str != "") set_str += ",";
                set_str += " r8 = " + LotteryBonusStruct.r8;
            }

            if (LotteryBonusStruct.r7.HasValue)
            {
                if (set_str != "") set_str += ",";
                set_str += " r7 = " + LotteryBonusStruct.r7;
            }

            if (LotteryBonusStruct.r1.HasValue)
            {
                if (set_str != "") set_str += ",";
                set_str += " r1 = " + LotteryBonusStruct.r1;
            }

            if (LotteryBonusStruct.Status != "")
            {
                if (set_str != "") set_str += ",";
                set_str += " status = '" + LotteryBonusStruct.Status + "'";
            }

            where_str += " id = " + LotteryBonusStruct.id;

            if (set_str != "")
            {
                if (set_str != "") set_str += ",";
                set_str += "  checkoutDateTime='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                conn.Execute(update_str + " set " + set_str + " where " + where_str);
            }
        }

        public class SettlementBonusStruct
        {
            public string PeriodId;
            public string LotteryResult;
        }

        public void SettlementBonusHandle(SettlementBonusStruct SettlementBonusStruct) // 將 status = 4 & isDeliver = 0 的注單取出，若會員贏錢，將金額加到錢包，並完成派彩 isDeliver = 1。
        {
            string select_str = "select externalId, parentId from [lottery].[dbo].[betForm] with(nolock)";
            string where_str_s = " where periodId = @periodId group by externalId,parentId";
            var p = conn.Query<betForm>(select_str + where_str_s,
               new { periodId = SettlementBonusStruct.PeriodId }).ToList();

            foreach (var o in p)
            {
                decimal walletAmount = 0;
                select_str = "select id, winAmount, parentId from [lottery].[dbo].[betForm] with(nolock)";
                where_str_s = " where periodId = @periodId and externalId = @externalId and status = '" + Define.BetFormStatus.Success + "' and isDeliver = 0 ";
                var b = conn.Query<betForm>(select_str + where_str_s,
                   new { periodId = SettlementBonusStruct.PeriodId, externalId = o.externalId }).ToList();

                foreach (var n in b)
                {
                    string update_str = "update [lottery].[dbo].[betForm]  ";
                    string set_str = " isDeliver = 1 , deliverAmount = " + n.winAmount + " , lotteryResult = '" + SettlementBonusStruct.LotteryResult + "' ";
                    string where_str = " externalId = '" + o.externalId + "' and parentId = " + o.parentId + " and id = " + n.id;

                    conn.Execute(update_str + " set " + set_str + " where " + where_str);

                    if (n.winAmount > 0)
                    {
                        select_str = "select walletAmount from [lottery].[dbo].[member] with(nolock)";
                        where_str_s = " where parentId = @parentId and externalId = @externalId ";
                        var m = conn.Query<member>(select_str + where_str_s,
                           new { parentid = o.parentId, externalId = o.externalId }).FirstOrDefault();

                        update_str = "update [lottery].[dbo].[member]  ";
                        set_str = "";
                        where_str = "";

                        walletAmount = m.walletAmount + n.winAmount;

                        set_str += " WalletAmount = " + walletAmount.ToString();

                        where_str += " externalId = '" + o.externalId + "' and parentId = " + o.parentId;

                        if (set_str != "") set_str += ",";

                        set_str += " updateDateTime = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "'";

                        conn.Execute(update_str + " set " + set_str + " where " + where_str);
                    }
                }
            }
        }
    }
}