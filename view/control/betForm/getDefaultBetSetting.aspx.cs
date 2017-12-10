using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_member_getDefaultBetSetting : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();

    protected void Page_Load(object sender, EventArgs e)
    {
        GetDefaultBetSetting();
        Response.End();
    }

    void GetDefaultBetSetting()
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
        Model.Company.Define companyDefine = new Model.Company.Define();

        int agentId = (int)Session["id"];
        int agentLevelId = (int)Session["levelId"];
        int targetAgentId;
        int targetAgentLevelId;
        JToken defaultBetSetting;

        string targetAgentId_s = (Request["agentId"] ?? "").ToString();

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
        defaultBetSetting = JsonConvert.DeserializeObject<JObject>(companyDefine.GetCompanyDefaultBetSetting(targetAgentId));

        int? targetAgentLN = memberDefine.GetMemberLN(targetAgentId, agentLevelId);

        if (agentId != targetAgentLN)
        {
            result["result"] = "fail";
            result["msg"] = "權限不足";
            result["column"] = "levelId";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        result["result"] = "success";
        result["msg"] = "取得成功";
        result["column"] = "";
        result["betRate"] = defaultBetSetting;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}