﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_betForm_editDefaultBetSetting : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
    JToken req;

    protected void Page_Load(object sender, EventArgs e)
    {
        JToken result = new JObject();
        string method = Request.HttpMethod;
        if (method == "POST")
        {
            req = glbf.GetReqInputStream();
            string action = (req["action"] ?? "").ToString();
            if (action != "view" && action != "editDefaultBetSetting")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            if (action == "view")
            {

            }
            else if (action == "editDefaultBetSetting")
            {
                EditDefaultBetSetting();
            }
        }
        Response.End();
    }

    void EditDefaultBetSetting()
    {
        JToken result = new JObject();


        if (glbf.GetLoginStatus() != 1)
        {
            result["result"] = "fail";
            result["msg"] = "未完成登入程序";
            result["column"] = "session";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Define memberDefine = new Model.Member.Define();

        int agentId = (int)Session["id"];
        int targetAgentId;
        JObject defaultBetSettingObj;

        int agentLevelId = (int)Session["levelId"];
        int targetAgentLevelId;

        string defaultBetSetting = (req["defaultBetSetting"] ?? "").ToString();
        string targetAgentUsername = (req["agentUsername"] ?? "").ToString();

        string targetAgentId_s = (req["agentId"] ?? "").ToString();

        var chkAgentIdFmt = int.TryParse(targetAgentId_s, out targetAgentId);
        if (chkAgentIdFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號格式錯誤";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var chkAgentIdExit = memberDefine.CheckMemberIdExist(targetAgentId);
        if (chkAgentIdExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號不存在";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        targetAgentLevelId = memberDefine.GetMemberLevelId(targetAgentId);

        try
        {
            defaultBetSettingObj = JsonConvert.DeserializeObject<JObject>(defaultBetSetting);
        }
        catch (Exception excep)
        {
            result["result"] = "fail";
            result["msg"] = "betSetting轉換json失敗";
            result["column"] = "betSetting";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        int? targetAgentLN = memberDefine.GetMemberLN(targetAgentId, agentLevelId);

        if (agentId != targetAgentLN)
        {
            result["result"] = "fail";
            result["msg"] = "權限不足";
            result["column"] = "levelId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Edit edit = new Model.Member.Edit();
        Model.Member.Edit.AgentStruct bs = new Model.Member.Edit.AgentStruct();

        bs.AgentId = targetAgentId;
        bs.DefaultBetSetting = defaultBetSettingObj;
        bs.UpdateUserId = agentId;

        edit.AgentStructHandle(bs);

        result["result"] = "success";
        result["msg"] = "修改預設賽事設定完成";
        result["column"] = "";

        Response.Write(JsonConvert.SerializeObject(result));
    }
}