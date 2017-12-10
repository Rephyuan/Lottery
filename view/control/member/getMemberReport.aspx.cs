using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_member_getMemberReport : System.Web.UI.Page
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
            if (action != "view" && action != "getMemberReport")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {
            }
            else if (action == "getMemberReport")
            {
                GetMemberReport();
            }
        }
        Response.End();
    }

    void GetMemberReport()
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
        Model.BetForm.Define betFormDefine = new Model.BetForm.Define();

        int? l8 = null;
        int? l7 = null;
        int? l1 = null;
        int? levelId = null;
        string periodId = null;
        string status = null;
        string betType = null;
        bool? isDeliver = null;
        string reportType;
        DateTime beginDateTime;
        DateTime endDateTime;

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

        string l1_s = (req["l1"] ?? "").ToString();
        if (l1_s != "")
        {
            int l1Temp;
            var chkL7fmt = int.TryParse(l1_s, out l1Temp);
            if (chkL7fmt == false)
            {
                result["result"] = "fail";
                result["msg"] = "L1格式錯誤";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            l1 = l1Temp;
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

        string periodId_s = (req["periodId"] ?? "").ToString();
        if (periodId_s != "")
        {
            int periodIdTemp;
            var chkperiodIdfmt = int.TryParse(periodId_s, out periodIdTemp);
            if (chkperiodIdfmt == false)
            {
                result["result"] = "fail";
                result["msg"] = "periodId格式錯誤";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            periodId = periodId_s;
        }

        string betType_s = (req["betType"] ?? "").ToString();//找玩法
        if (betType_s != "")
        {
            var chkBetTypeExist = Model.BetForm.Define.BetTypesLangMap.ContainsKey(betType_s);
            if (chkBetTypeExist == false)
            {
                result["result"] = "fail";
                result["msg"] = "betType不存在";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            betType = betType_s;
        }

        string status_s = (req["status"] ?? "").ToString();//找狀態
        if (status_s != "")
        {
            var chkStatusExist = Model.BetForm.Define.BetFormStatusLangMap.ContainsKey(status_s);
            if (chkStatusExist == false)
            {
                result["result"] = "fail";
                result["msg"] = "status不存在";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            status = status_s;
        }

        string isDeliver_s = (req["isDeliver"] ?? "").ToString();
        if (isDeliver_s != "")
        {
            bool isDeliverTemp;
            var chkIsDeliverExist = bool.TryParse(isDeliver_s,out isDeliverTemp);
            if (chkIsDeliverExist == false)
            {
                result["result"] = "fail";
                result["msg"] = "isDeliver格式錯誤";
                result["errorCode"] = ApiErrorCodes.UsernameFormatError;
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            isDeliver = isDeliverTemp;
        }

        string beginDateTime_s = (req["beginDateTime"] ?? "").ToString();
        string endDateTime_s = (req["endDateTime"] ?? "").ToString();
        if (beginDateTime_s == "" || endDateTime_s == "")
        {
            result["result"] = "fail";
            result["msg"] = "日期必須輸入";
            result["column"] = "DateTime";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var s = DateTime.TryParse(beginDateTime_s, out beginDateTime);
        if (s == false)
        {
            result["result"] = "fail";
            result["msg"] = "起始日期格式錯誤";
            result["column"] = "beginDateTime";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var e = DateTime.TryParse(endDateTime_s, out endDateTime);
        if (e == false)
        {
            result["result"] = "fail";
            result["msg"] = "結束日期格式錯誤";
            result["column"] = "endDateTime";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (endDateTime < beginDateTime)
        {
            result["result"] = "fail";
            result["msg"] = "日期不能倒置";
            result["column"] = "endDateTime";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string reportType_s = (req["reportType"] ?? "").ToString();
        var checkReportType = Model.BetForm.Define.ReportTypesLangMap.ContainsKey(reportType_s);
        if (checkReportType == false)
        {
            result["result"] = "fail";
            result["msg"] = "報表型態錯誤";
            result["column"] = "reportType";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        reportType = reportType_s;

        Model.BetForm.List list = new Model.BetForm.List();
        JArray brList;

        if (reportType == "m")
        {
            Model.BetForm.List.BetFormSummaryStruct br = new Model.BetForm.List.BetFormSummaryStruct();

            br.L8 = l8;
            br.L7 = l7;
            br.L1 = l1;
            br.LevelId = levelId;
            br.BetType = betType;
            br.PeriodId = periodId;
            br.Status = status;
            br.BeginDateTime = beginDateTime;
            br.EndDateTime = endDateTime;
            br.IsDeliver = isDeliver;

            brList = list.BetFormSummaryStructHandle(br);
        }
        else
        {
            Model.BetForm.List.BetFormDetailStruct br = new Model.BetForm.List.BetFormDetailStruct();

            br.L8 = l8;
            br.L7 = l7;
            br.L1 = l1;
            br.BetType = betType;
            br.PeriodId = periodId;
            br.Status = status;
            br.BeginDateTime = beginDateTime;
            br.EndDateTime = endDateTime;
            br.IsDeliver = isDeliver;

            brList = list.BetFormDetailStructHandle(br);
        }

        result["result"] = 0;
        result["column"] = 0;
        result["msg"] = "";
        result["list"] = brList;

        Response.Write(JsonConvert.SerializeObject(result));
    }
}