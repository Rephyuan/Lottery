using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_member_getMemberBetForm : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
    JToken req;

    protected void Page_Load(object sender, EventArgs e)
    {
        string method = Request.HttpMethod;
        JToken result = new JObject();

        if (method == "POST")
        {
            req = glbf.GetReqInputStream();
            string action = (req["action"] ?? "").ToString();
            if (action != "view" && action != "getMemberBetForm")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {

            }
            else if (action == "getMemberBetForm")
            {
                GetMemberBetForm();
            }
        }
        Response.End();
    }

    void GetMemberBetForm()
    {
        JToken result = new JObject();

        Model.Member.Define memberDefine = new Model.Member.Define();
        Model.BetForm.Define betFormDefine = new Model.BetForm.Define();

        int agentLevelId;
        int? l7 = null;
        int? l1 = null;
        string status = null;
        DateTime beginDateTime;
        DateTime endDateTime;

        string agentId_s = (req["agentId"] ?? "").ToString();
        int agentIdTemp;

        var chkAgentIdFmt = int.TryParse(agentId_s, out agentIdTemp);
        if (chkAgentIdFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號格式錯誤";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var chkAgentIdExit = memberDefine.CheckMemberIdExist(agentIdTemp);
        if (chkAgentIdExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號不存在";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        l7 = agentIdTemp;
        agentLevelId = memberDefine.GetMemberLevelId(agentIdTemp);

        if (agentLevelId != 7)
        {
            result["result"] = "fail";
            result["msg"] = "levelId 錯誤";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string externalId_s = (req["externalId"] ?? "").ToString();
        if (externalId_s != "")
        {
            var checkExternalIdExit = memberDefine.CheckMemberExternalIdExist(externalId_s, agentIdTemp);
            if (checkExternalIdExit == false)
            {
                result["result"] = "fail";
                result["msg"] = "externalId不存在";
                result["errorCode"] = ApiErrorCodes.NotFindUsername;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            l1 = memberDefine.GetMemberId(externalId_s, agentIdTemp);
        }

        string status_s = (req["status"] ?? "").ToString();//找狀態
        if (status_s != "")
        {
            var chkStatusExist = Model.BetForm.Define.BetFormStatusLangMap.ContainsKey(status_s);
            if (chkStatusExist == false)
            {
                result["result"] = "fail";
                result["msg"] = "status不存在";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            status = status_s;
        }

        string beginDateTime_s = (req["beginDateTime"] ?? "").ToString();
        string endDateTime_s = (req["endDateTime"] ?? "").ToString();
        if (beginDateTime_s == "" || endDateTime_s == "")
        {
            result["result"] = "fail";
            result["msg"] = "日期必須輸入";
            result["errorCode"] = ApiErrorCodes.DateNull;

            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var s = DateTime.TryParse(beginDateTime_s, out beginDateTime);
        if (s == false)
        {
            result["result"] = "fail";
            result["msg"] = "beginDateTime格式錯誤";
            result["errorCode"] = ApiErrorCodes.DateTimeError;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var e = DateTime.TryParse(endDateTime_s, out endDateTime);
        if (e == false)
        {
            result["result"] = "fail";
            result["msg"] = "endDateTime格式錯誤";
            result["errorCode"] = ApiErrorCodes.DateTimeError;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var i = new TimeSpan(endDateTime.Ticks - beginDateTime.Ticks).TotalMinutes;

        if (i > 30000)
        {
            result["result"] = "fail";
            result["msg"] = "日期區間間隔必須小於30分鐘";
            result["errorCode"] = ApiErrorCodes.DateTimeInterval30;

            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.BetForm.List list = new Model.BetForm.List();
        Model.BetForm.List.BetFormDetailStruct br = new Model.BetForm.List.BetFormDetailStruct();

        br.L7 = l7;
        br.L1 = l1;
        br.Status = status;
        br.BeginDateTime = beginDateTime;
        br.EndDateTime = endDateTime;

        var brList = list.BetFormDetailStructHandle(br);

        result["result"] = 0;
        result["errorCode"] = 0;
        result["msg"] = "";
        result["list"] = brList;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}