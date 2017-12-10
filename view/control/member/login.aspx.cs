using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class view_control_login : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
    JToken req;

    protected void Page_Load(object sender, EventArgs e)
    {
        string method = Request.HttpMethod;
        JToken result = new JObject();
        req = glbf.GetReqInputStream();

        if (method == "POST")
        {
            string action = (req["action"] ?? "").ToString();
            if (action != "view" && action != "login")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            else if (action == "view")
            {

            }
            else if (action == "login")
            {
                Login();
            }
        }
        Response.End();
    }

    void Login()
    {
        JToken result = new JObject();

        Model.Member.Define memberDefine = new Model.Member.Define();

        string clientIP = glbf.GetClientIP();
        int agentId;

        int agentLevelId;
        string agentStatus;

        string username = (req["username"] ?? "").ToString();

        var chkUsernameExit = memberDefine.CheckMemberUsernameExist(username);
        if (chkUsernameExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "無此帳號";
            result["column"] = "username";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        agentId = memberDefine.GetMemberId(username);
        agentLevelId = memberDefine.GetMemberLevelId(agentId);
        agentStatus = memberDefine.GetMemberStatus(agentId);

        if (agentLevelId < 7)
        {
            result["result"] = "fail";
            result["msg"] = "非代理帳號";
            result["column"] = "agentId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (agentStatus == Model.Member.Define.MemberStauts.Disable)
        {
            result["result"] = "fail";
            result["msg"] = "帳號鎖定";
            result["column"] = "status";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }
        else if (agentStatus == Model.Member.Define.MemberStauts.NeedToChangePassword)
        {
            result["result"] = "fail";
            result["msg"] = "請修改密碼後再次登入";
            result["column"] = "status";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string password = (req["password"] ?? "").ToString();
        if (password != memberDefine.GetMemberPassword(agentId))
        {
            result["result"] = "fail";
            result["msg"] = "密碼錯誤";
            result["column"] = "password";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Edit e = new Model.Member.Edit();
        Model.Member.Edit.AgentLoginStruct s = new Model.Member.Edit.AgentLoginStruct();

        s.AgentId = agentId;
        s.LoginIP = clientIP;

        e.AgentLoginStructHandle(s);

        result["result"] = "success";
        result["msg"] = "登入成功";
        result["id"] = agentId.ToString();

        Response.Write(JsonConvert.SerializeObject(result));
    }
}