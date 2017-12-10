using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class api_member_oneTimeLink : System.Web.UI.Page
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
            if (action != "view" && action != "forwardGame")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {

            }
            else if (action == "forwardGame")
            {
                ForwardGame();
            }
        }
        Response.End();
    }
    void ForwardGame()
    {
        JToken result = new JObject();
        Model.Member.Define memberDefine = new Model.Member.Define();

        int memberId;
        string guid;

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

        guid = Guid.NewGuid().ToString("N");

        Model.Member.Edit edit = new Model.Member.Edit();
        Model.Member.Edit.ForwardGameStruct fg = new Model.Member.Edit.ForwardGameStruct();

        fg.MemberId = memberId;
        fg.Guid = guid;

        edit.ForwardGameStructHandle(fg);

        JToken jtk = new JObject();

        jtk["agentId"] = agentId;
        jtk["externalId"] = externalId_s;
        jtk["guid"] = guid;

        string str = JsonConvert.SerializeObject(jtk);
        byte[] bt = Encoding.UTF8.GetBytes(str);

        string webaddr = Convert.ToBase64String(bt);

        result["result"] = "success";
        result["errorCode"] = 0;
        result["agentId"] = agentId;
        result["externalId"] = externalId_s;
        result["url"] = "http://192.168.1.131:82/lottery/externalLogin?post=" + webaddr;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}