using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class view_control_member_agentDelete : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
    JToken req;

    protected void Page_Load(object sender, EventArgs e)
    {
        string method = Request.HttpMethod;
        if (method == "POST")
        {
            req = glbf.GetReqInputStream();

            string action = (req["action"] ?? "").ToString();
            if (action == "view")
            {

            }
            else if (action == "agentDelete")
            {
                DeleteAgent();
            }
        }
        Response.End();
    }

    void DeleteAgent()
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

        int agentId = (int)Session["id"];
        int agentLevelId = (int)Session["levelId"];
        int targetAgentLevelId;

        string targetAgentId_s = (req["agentId"] ?? "").ToString();

        var chkAgentIdFmt = int.TryParse(targetAgentId_s, out targetAgentId);
        if (chkAgentIdFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號格式錯誤";
            result["column"] = "agentId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var chkAgentIdExit = memberDefine.CheckMemberIdExist(targetAgentId);
        if (chkAgentIdExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "代理帳號不存在";
            result["column"] = "agentId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        targetAgentLevelId = memberDefine.GetMemberLevelId(targetAgentId);

        int? targetAgentLN = memberDefine.GetMemberLN(targetAgentId, agentLevelId);

        if (agentId != targetAgentLN || agentId == targetAgentId)
        {
            result["result"] = "fail";
            result["msg"] = "權限不足";
            result["column"] = "levelId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Edit memberEdit = new Model.Member.Edit();
        Model.Member.Edit.AgentStruct bs = new Model.Member.Edit.AgentStruct();

        bs.AgentId = targetAgentId;
        bs.Stauts = Model.Member.Define.MemberStauts.Disable;
        bs.UpdateUserId = agentId;

        memberEdit.AgentStructHandle(bs);

        result["result"] = "success";
        result["msg"] = "鎖定帳戶成功";

        Response.Write(JsonConvert.SerializeObject(result));
    }
}