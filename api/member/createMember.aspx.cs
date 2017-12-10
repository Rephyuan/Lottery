using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_createMember : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    JToken data = new JObject();
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
            if (action != "view" && action != "createMember")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {

            }
            else if (action == "createMember")
            {
                createMember();
            }
        }
        Response.End();
    }

    void createMember()
    {
        JToken result = new JObject();
        Model.Member.Define memberDefine = new Model.Member.Define();

        int agentId;
        string externalId;

        int agentLevelId;

        string agentId_s = (req["agentId"] ?? "").ToString();

        var chkAgentIdFmt = int.TryParse(agentId_s, out agentId);
        if (chkAgentIdFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號格式錯誤";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var chkAgentIdExit = memberDefine.CheckMemberIdExist(agentId);
        if (chkAgentIdExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號不存在";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        agentLevelId = memberDefine.GetMemberLevelId(agentId);

        if (agentLevelId != 7)
        {
            result["result"] = "fail";
            result["msg"] = "levelId 錯誤";
            result["errorCode"] = ApiErrorCodes.AgentIdExistFalse;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string externalId_s = (req["externalId"] ?? "").ToString();

        var chkExternalIdFmt = memberDefine.UsernameValidate(externalId_s);
        if (chkExternalIdFmt == 1)
        {
            result["result"] = "fail";
            result["msg"] = "externalId長度必須為4~10";
            result["column"] = "externalId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }
        else if (chkExternalIdFmt == 2)
        {
            result["result"] = "fail";
            result["msg"] = "externalId字元須為英數";
            result["column"] = "externalId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var checkExternalIdExist = memberDefine.CheckMemberExternalIdExist(externalId_s, agentId);
        if (checkExternalIdExist == true)
        {
            result["result"] = "fail";
            result["msg"] = "externalId重複";
            result["errorcode"] = ApiErrorCodes.UsernameExist;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        externalId = externalId_s;

        Model.Member.Add add = new Model.Member.Add();
        Model.Member.Add.MemberStruct s = new Model.Member.Add.MemberStruct();
        s.ExternalId = externalId;
        s.AgentId = agentId;

        add.MemberStructHandle(s);

        result["result"] = "success";
        result["msg"] = "新增完成";
        result["errorcode"] = 0;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}