using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class view_control_member_agentAdd : System.Web.UI.Page
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
            if (action != "view" && action != "agentAdd")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            else if (action == "view")
            {

            }
            else if (action == "agentAdd")
            {
                AddAgent();
            }
        }
        Response.End();
    }

    void AddAgent()
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

        if ((int)Session["levelId"] < 8)
        {
            result["result"] = "fail";
            result["msg"] = "無新增代理權限";
            result["column"] = "authorization";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Define memberDefine = new Model.Member.Define();

        int agentId = (int)Session["id"];
        string username;
        string password;
        string title;
        string nickname;

        string username_s = (req["username"] ?? "").ToString();

        var chkUsernameFmt = memberDefine.UsernameValidate(username_s);
        if (chkUsernameFmt == 1)
        {
            result["result"] = "fail";
            result["msg"] = "username長度必須為4~10";
            result["column"] = "username";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }
        else if (chkUsernameFmt == 2)
        {
            result["result"] = "fail";
            result["msg"] = "username字元須為英數";
            result["column"] = "username";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var chkUsernameExist = memberDefine.CheckMemberUsernameExist(username_s);
        if (chkUsernameExist == true)
        {
            result["result"] = "fail";
            result["msg"] = "帳號重複";
            result["column"] = "username";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        username = username_s;

        string password_s = (req["password"] ?? "").ToString();

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

        password = password_s;

        string title_s = (req["title"] ?? "").ToString();

        var chkTitleFmt = memberDefine.TitleValidate(title_s);
        if (chkTitleFmt == 1)
        {
            result["result"] = "fail";
            result["msg"] = "title長度必須為1~15";
            result["column"] = "title";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }
        else if (chkTitleFmt == 2)
        {
            result["result"] = "fail";
            result["msg"] = "title字元須為中文英數";
            result["column"] = "title";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        title = title_s;

        string nickname_s = (req["nickname"] ?? "").ToString();

        var chkNicknameFmt = memberDefine.TitleValidate(nickname_s);
        if (chkNicknameFmt == 1)
        {
            result["result"] = "fail";
            result["msg"] = "nickname長度必須為1~15";
            result["column"] = "title";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }
        else if (chkNicknameFmt == 2)
        {
            result["result"] = "fail";
            result["msg"] = "nickname字元須為中文英數";
            result["column"] = "title";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        nickname = nickname_s;

        Model.Member.Add add = new Model.Member.Add();
        Model.Member.Add.AgentStruct s = new Model.Member.Add.AgentStruct();

        s.Username = username;
        s.Password = password;
        s.ParentId = agentId;
        s.Title = title;
        s.Nickname = nickname;

        add.AgentStructHandle(s);

        result["result"] = "success";
        result["msg"] = "新增完成";
        result["column"] = "";

        Response.Write(JsonConvert.SerializeObject(result));
    }
}