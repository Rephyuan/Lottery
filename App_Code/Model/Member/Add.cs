using Dapper;
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
    public class Add
    {
        SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
        DataClassesDataContext dcdc = new DataClassesDataContext(GlobalVar.sql_con_str_main);

        public Add()
        {

        }

        public class MemberStruct
        {
            public int AgentId;
            public string ExternalId;
        }

        public void MemberStructHandle(MemberStruct memberStruct)
        {
            string select_str = "select l9,l8,l7,companyId from [lottery].[dbo].[member] with(nolock) ";
            string where_str = " Where id = @id ";
            var e = conn.Query<member>(select_str + where_str,
               new { id = memberStruct.AgentId }).FirstOrDefault();

            select_str = "select defaultBetSetting from [lottery].[dbo].[company] with(nolock) ";
            where_str = " Where principalId = @principalId ";
            var c = conn.Query<company>(select_str + where_str,
               new { principalId = memberStruct.AgentId }).FirstOrDefault();

            member m = new member()
            {
                username = Guid.NewGuid().ToString("N"),
                password = null,
                levelId = Define.MemberLevels.Member,
                companyId = e.companyId,
                externalId = memberStruct.ExternalId,
                nickname = memberStruct.ExternalId,
                parentId = memberStruct.AgentId,
                createDateTime = DateTime.Now,
                l9 = e.l9,
                l8 = e.l8,
                l7 = e.l7,
                betSetting = c.defaultBetSetting,
                status = Define.MemberStauts.Enable
            };

            dcdc.members.InsertOnSubmit(m);
            dcdc.SubmitChanges();
            int a = m.id;

            m.l1 = a;
            dcdc.SubmitChanges();
        }

        public class AgentStruct
        {
            public string Username;
            public string Password;
            public int ParentId;
            public string Title;
            public string Nickname;
        }

        public void AgentStructHandle(AgentStruct AgentStruct)
        {
            string select_str = "select levelId, l9, l8, l7,companyId , betSetting from [lottery].[dbo].[member] with(nolock) ";
            string where_str = " Where id = @id ";
            var e = conn.Query<member>(select_str + where_str,
               new { Id = AgentStruct.ParentId }).FirstOrDefault();

            select_str = "select betRate, defaultBetSetting from [lottery].[dbo].[company] with(nolock) ";
            where_str = " Where principalId = @principalId ";
            var h = conn.Query<company>(select_str + where_str,
               new { principalId = AgentStruct.ParentId }).FirstOrDefault();

            Define memberDefine = new Define();

            member m = new member()
            {
                username = AgentStruct.Username,
                password = AgentStruct.Password,
                levelId = memberDefine.GetNextLevelId(e.levelId),
                companyId = 0,
                parentId = AgentStruct.ParentId,
                externalId = "",
                createDateTime = DateTime.Now,
                l9 = e.l9,
                l8 = e.l8,
                l7 = e.l7,
                betSetting = e.betSetting,
                status = Define.MemberStauts.Enable,
                nickname = AgentStruct.Nickname
            };

            dcdc.members.InsertOnSubmit(m);
            dcdc.SubmitChanges();

            int principalId = m.id;

            company c = new company()
            {
                title = AgentStruct.Title,
                createDateTime = DateTime.Now,
                principalId = principalId,
                betRate = h.betRate,
                defaultBetSetting = h.defaultBetSetting
            };

            dcdc.companies.InsertOnSubmit(c);
            dcdc.SubmitChanges();

            int companyId = c.Id;

            if (AgentStruct.LevelId == Define.MemberLevels.UpAgent)
            {
                m.l8 = principalId;
            }
            if (AgentStruct.LevelId == Define.MemberLevels.Agent)
            {
                m.l7 = principalId;
            }

            m.companyId = companyId;

            dcdc.SubmitChanges();
        }
    }
}