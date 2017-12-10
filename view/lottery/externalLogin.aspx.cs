using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_member_oneTimeLinkLogin : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
    JToken req;

    protected void Page_Load(object sender, EventArgs e)
    {
        oneTimeLinkLogin();
        Response.End();
    }
    void oneTimeLinkLogin()
    {
        JToken result = new JObject();

        Model.Member.Define memberDefine = new Model.Member.Define();

        byte[] decbuff = Convert.FromBase64String(Request["post"]);
        var o = System.Text.Encoding.UTF8.GetString(decbuff);
        var json = JsonConvert.DeserializeObject<JObject>(o);

        int memberId;
        string ip = glbf.GetClientIP();

        int agentId;
        string memberGuid;

        string agentId_s = (json["agentId"] ?? "").ToString();

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

        string externalId_s = (json["externalId"] ?? "").ToString();

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
        memberGuid = memberDefine.GetMemberGuid(memberId);

        string guid_s = (json["guid"] ?? "").ToString();

        if (guid_s != memberGuid)
        {
            result["result"] = "fail";
            result["msg"] = "guid錯誤";
            result["errorcode"] = ApiErrorCodes.NotFindUsername;
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Edit memberEdit = new Model.Member.Edit();
        Model.Member.Edit.MemberLoginStruct s = new Model.Member.Edit.MemberLoginStruct();

        s.MemberId = memberId;
        s.LoginIP = ip;

        memberEdit.MemberLoginStructHandle(s);


        result["result"] = "success";
        result["msg"] = "登入成功";

        Response.Redirect("./index");
    }
}