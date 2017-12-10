using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_member_queryWalletAmount : System.Web.UI.Page
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
            if (action != "view" && action != "queryWalletAmount")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {

            }
            else if (action == "queryWalletAmount")
            {
                queryWalletAmount();
            }
        }
        Response.End();
    }

    void queryWalletAmount()
    {
        JToken result = new JObject();
        Model.Member.Define memberDefine = new Model.Member.Define();

        int memberId;

        int agentId;

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

        string externalId_s = (req["externalId"] ?? "").ToString();

        var checkExternalIdExit = memberDefine.CheckMemberExternalIdExist(externalId_s, agentId);
        if (checkExternalIdExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "externalId不存在";
            result["errorCode"] = ApiErrorCodes.NotFindUsername;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        memberId = memberDefine.GetMemberId(externalId_s, agentId);

        decimal walletAmount = memberDefine.GetMemberWalletAmount(memberId);

        result["result"] = "success";
        result["msg"] = "查詢完成";
        result["walletAmount"] = walletAmount;
        result["errorcode"] = 0;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}