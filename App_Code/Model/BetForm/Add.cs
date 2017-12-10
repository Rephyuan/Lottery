using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Model.BetForm
{
    /// <summary>
    /// Summary description for Add
    /// </summary>
    public class Add
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
        DataClassesDataContext dcdc = new DataClassesDataContext(GlobalVar.sql_con_str_main);
        public Add()
        {

        }

        public class BetFormStruct
        {
            public int memberId;
            public string BetType;
            public string BetBranch;
            public string ChooseBall;
            public int Combo;
            public decimal Rate;
            public decimal BetAmount;
            public decimal TotalBet;
            public string IP;
            public string PeriodId;
            public string BetRemark;
            public string Username;
        }

        public void BetFormStructtHandle(BetFormStruct BetFormStruct)
        {
            string select_str = "select username , externalId ,parentId, l9, l8, l7, l1, companyId from [lottery].[dbo].[member] with(nolock)";
            string where_str = " Where id = @id";
            var e = conn.Query<member>(select_str + where_str,
               new { id = BetFormStruct.memberId}).FirstOrDefault();

            betForm m = new betForm()
            {
                externalId = e.externalId,
                companyId = e.companyId,
                parentId = e.parentId,
                betType = BetFormStruct.BetType,
                betBranch = BetFormStruct.BetBranch,
                chooseBall = BetFormStruct.ChooseBall,
                combo = BetFormStruct.Combo,
                rate = BetFormStruct.Rate,
                betAmount = BetFormStruct.BetAmount,
                totalBet = BetFormStruct.TotalBet,
                createIP = BetFormStruct.IP,
                createDateTime = DateTime.Now,
                lastModifyDateTime = DateTime.Now,
                beginDateTime = DateTime.Now, // 待修正
                isDeliver = false,
                periodId = BetFormStruct.PeriodId,
                status = Define.BetFormStatus.Effective,
                betRemark = BetFormStruct.BetRemark,
                username = e.username,
                l9 = e.l9,
                l8 = e.l8,
                l7 = e.l7,
                l1 = e.l1
            };
            
            dcdc.betForms.InsertOnSubmit(m);
            dcdc.SubmitChanges();
        }
    }
}
