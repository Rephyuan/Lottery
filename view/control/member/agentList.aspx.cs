using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class view_agentList : System.Web.UI.Page
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
            if (action != "view" && action != "agentList")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {
            }
            else if (action == "agentList")
            {
                AgentList();
            }
        }
        Response.End();
    }

    void AgentList()
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
        Model.Member.View memberView = new Model.Member.View();

        int? l8 = null;
        int? l7 = null;
        int? levelId = null;
        string status = null;
        DateTime? createBeginDateTime = null;
        DateTime? createEndDateTime = null;

        string l8_s = (req["l8"] ?? "").ToString();
        if (l8_s != "")
        {
            int l8Temp;
            var chkL8fmt = int.TryParse(l8_s, out l8Temp);
            if (chkL8fmt == false)
            {
                result["result"] = "fail";
                result["msg"] = "L8格式錯誤";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            l8 = l8Temp;
        }

        string l7_s = (req["l7"] ?? "").ToString();
        if (l7_s != "")
        {
            int l7Temp;
            var chkL7fmt = int.TryParse(l7_s, out l7Temp);
            if (chkL7fmt == false)
            {
                result["result"] = "fail";
                result["msg"] = "L7格式錯誤";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            l7 = l7Temp;
        }

        string levelId_s = (req["levelId"] ?? "").ToString();

        if (levelId_s != "")
        {
            int levelIdTemp;
            var checkLevelIdFmt = int.TryParse(levelId_s, out levelIdTemp);
            if (checkLevelIdFmt == false)
            {
                result["result"] = "fail";
                result["msg"] = "levelId格式錯誤";
                result["column"] = "levelId";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            var checkLevelIdExit = Model.Member.Define.MemberLevelsLangMap.ContainsKey(levelIdTemp);
            if (checkLevelIdExit == false)
            {
                result["result"] = "fail";
                result["msg"] = "levelId不存在";
                result["column"] = "levelId";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            levelId = levelIdTemp;
        }

        string status_s = (req["status"] ?? "").ToString();

        if (status_s != "")
        {
            var chkStatusExist = Model.Member.Define.MemberStautsLangMap.ContainsKey(status_s);
            if (chkStatusExist == false)
            {
                result["result"] = "fail";
                result["msg"] = "status不存在";
                result["column"] = "status";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            status = status_s;
        }

        string createBeginDateTime_s = (req["createBeginDateTime"] ?? "").ToString(); //時間
        string createEndDateTime_s = (req["createEndDateTime"] ?? "").ToString();

        if (createBeginDateTime_s != "" && createEndDateTime_s != "")
        {
            DateTime createBeginDateTimeTemp;
            var s = DateTime.TryParse(createBeginDateTime_s, out createBeginDateTimeTemp);
            if (s == false)
            {
                result["result"] = "fail";
                result["msg"] = "日期格式錯誤";
                result["column"] = "createBeginDateTime";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            createBeginDateTime = createBeginDateTimeTemp;

            DateTime createEndDateTimeTemp;
            var e = DateTime.TryParse(createEndDateTime_s, out createEndDateTimeTemp);
            if (e == false)
            {
                result["result"] = "fail";
                result["msg"] = "日期格式錯誤";
                result["column"] = "createEndDateTime";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            createEndDateTime = createEndDateTimeTemp;
        }

        Model.Member.List list = new Model.Member.List();
        Model.Member.List.AgentListStruct br = new Model.Member.List.AgentListStruct();

        br.L8 = l8;
        br.L7 = l7;
        br.status = status;
        br.LevelId = levelId;
        br.CreateBeginDateTime = createBeginDateTime;
        br.CreateEndDateTime = createEndDateTime;

        var agentListResult = list.AgentListStructHandle(br);

        result["result"] = 0;
        result["column"] = 0;
        result["list"] = agentListResult;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}