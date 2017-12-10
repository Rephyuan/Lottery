using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class view_control_member_agentEdit : System.Web.UI.Page
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
            if (action != "view" && action != "agentEdit")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            else if (action == "view")
            {

            }
            else if (action == "agentEdit")
            {
                Edit();
            }
        }
        Response.End();
    }

    void Edit()
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

        int targetAgentId;
        string password = null;
        string title = null;
        string nickname = null;
        string targetAgentStatus = null;

        int targetAgentLevelId;
        int agentId = (int)Session["id"]; ;
        int agentLevelId = (int)Session["levelId"];

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

        if (targetAgentLevelId < 7)
        {
            result["result"] = "fail";
            result["msg"] = "修改對象非代理帳號";
            result["column"] = "AgentId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string password_s = (req["password"] ?? "").ToString();

        if (password_s != "")
        {
            var chkpassword = memberDefine.PasswordValidate(password_s);
            if (chkpassword == 1)
            {
                result["result"] = "fail";
                result["msg"] = "password長度必須為4~10";
                result["column"] = "password";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            else if (chkpassword == 2)
            {
                result["result"] = "fail";
                result["msg"] = "password字元須為英數";
                result["column"] = "password";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            targetAgentStatus = Model.Member.Define.MemberStauts.NeedToChangePassword;

            password = password_s;
        }

        string title_s = (req["title"] ?? "").ToString();

        if (title_s != "")
        {
            var chktitle = memberDefine.TitleValidate(title_s);
            if (chktitle == 1)
            {
                result["result"] = "fail";
                result["msg"] = "title長度必須為1~15";
                result["column"] = "title";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            else if (chktitle == 2)
            {
                result["result"] = "fail";
                result["msg"] = "title字元須為英數";
                result["column"] = "title";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            title = title_s;
        }

        string nickname_s = (req["nickname"] ?? "").ToString();

        if (nickname_s != "")
        {
            var chkNicknameFmt = memberDefine.TitleValidate(nickname_s);
            if (chkNicknameFmt == 1)
            {
                result["result"] = "fail";
                result["msg"] = "nickname長度必須為1~15";
                result["column"] = "nickname";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            else if (chkNicknameFmt == 2)
            {
                result["result"] = "fail";
                result["msg"] = "nickname字元須為英數";
                result["column"] = "nickname";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            nickname = nickname_s;
        }

        int? targetAgentLN = memberDefine.GetMemberLN(targetAgentId, agentLevelId);

        if (agentId != targetAgentLN || agentId == targetAgentId)
        {
            result["result"] = "fail";
            result["msg"] = "無修改權限";
            result["column"] = "levelId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Edit.AgentStruct s = new Model.Member.Edit.AgentStruct();
        Model.Member.Edit update = new Model.Member.Edit();

        s.AgentId = targetAgentId;
        s.Password = password;
        s.Title = title;
        s.Nickname = nickname;
        s.UpdateUserId = agentId;
        s.Stauts = targetAgentStatus;

        update.AgentStructHandle(s);

        result["result"] = "success";
        result["msg"] = "更新成功";
        result["column"] = "";

        Response.Write(JsonConvert.SerializeObject(result));
    }
}