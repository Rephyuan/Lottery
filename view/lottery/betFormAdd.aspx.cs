using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class api_member_betFormAdd : System.Web.UI.Page
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
            if (action != "view" && action != "betFormAdd")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            else if (action == "view")
            {

            }
            else if (action == "betFormAdd")
            {
                betFormAdd();
            }
        }
        Response.End();
    }

    void betFormAdd()
    {
        JToken result = new JObject();

        if (glbf.GetLoginStatus() != 2)
        {
            result["result"] = "fail";
            result["msg"] = "未完成登入程序";
            result["column"] = "session";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.Member.Define memberDefine = new Model.Member.Define();
        Model.BetForm.Define betFormDefine = new Model.BetForm.Define();

        int memberId = (int)Session["id"];
        int parentId = (int)Session["parentId"];
        string ip = glbf.GetClientIP();
        string periodId = betFormDefine.GetLotteryPeriodId();
        JObject chooseBallObj;
        string betType;
        string betBranch;
        string betRemark = null;
        decimal betAmount;
        int combo;
        decimal rate;
        decimal totalBet;
        decimal walletAmount = memberDefine.GetMemberWalletAmount(memberId);

        string betType_s = (req["betType"] ?? "").ToString();

        var checkBetTypeExit = Model.BetForm.Define.BetTypesLangMap.ContainsKey(betType_s);

        if (checkBetTypeExit == false)
        {
            result["result"] = "fail";
            result["msg"] = "找不到此BetType";
            result["column"] = "betType";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        betType = betType_s;

        string betBranch_s = (req["betBranch"] ?? "").ToString();

        int betBranchTemp;

        var chkBetBranchTempFmt = int.TryParse(betBranch_s, out betBranchTemp);
        if (chkBetBranchTempFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "betBranch格式錯誤";
            result["column"] = "betBranch";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        betBranch = betBranch_s;

        string betRemark_s = (req["betRemark"] ?? "").ToString();

        if (betRemark_s != "")
        {
            var checkBetRemark = memberDefine.TitleValidate(betRemark_s);
            if (checkBetRemark == 2)
            {
                result["result"] = "fail";
                result["msg"] = "只能輸入中文或英數";
                result["column"] = "betRemark";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
            else if (checkBetRemark == 1)
            {
                result["result"] = "fail";
                result["msg"] = "字串長度錯誤";
                result["column"] = "betRemark";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }
        }

        betRemark = betRemark_s;

        string betAmount_s = (req["betAmount"] ?? "").ToString();

        bool chkBetBetAmountFmt = decimal.TryParse(betAmount_s, out betAmount);
        if (chkBetBetAmountFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "BetAmount格式錯誤";
            result["column"] = "betAmount";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        decimal singleBetMinValue = memberDefine.GetMemberSingleBetMinValue(memberId, betType);

        if (betAmount < singleBetMinValue)
        {
            result["result"] = "fail";
            result["msg"] = "小於玩法單注限額";
            result["column"] = "betAmount";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        decimal singleBetMaxValue = memberDefine.GetMemberSingleBetMaxValue(memberId, betType);

        if (betAmount > singleBetMaxValue)
        {
            result["result"] = "fail";
            result["msg"] = "大於玩法單注限額";
            result["column"] = "betAmount";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        //bool checkBetTime = define.CheckBetTime();
        //if (checkBalance == false)
        //{
        //    result["result"] = "fail";
        //    result["msg"] = "超過時間";
        //    result["errorcode"] = ApiErrorCodes.OverTime;
        //    Response.Write(JsonConvert.SerializeObject(result));
        //    return;
        //}

        string chooseBall_s = (req["chooseBall"] ?? "").ToString().TrimEnd(',').Replace("\r\n", "").Replace(" ", "");

        try
        {
            chooseBallObj = JsonConvert.DeserializeObject<JObject>(chooseBall_s);
        }
        catch (Exception excep)
        {
            result["result"] = "fail";
            result["msg"] = "chooseBall轉換json失敗";
            result["column"] = "chooseBall";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var checkChooseBallCntCorrect = betFormDefine.CheckChooseBallCntCorrect(chooseBallObj, betType);
        if (checkChooseBallCntCorrect == false)
        {
            result["result"] = "fail";
            result["msg"] = "chooseBall數量錯誤";
            result["column"] = "chooseBall";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        var chkChooseBallItem = betFormDefine.CheckChooseBallItemCorrect(chooseBallObj, betType, parentId);
        if (chkChooseBallItem == false)
        {
            result["result"] = "fail";
            result["msg"] = "賠率異常或球號異常";
            result["column"] = "chooseBall";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string rate_s = (req["rate"] ?? "").ToString();

        var chkRateFmt = decimal.TryParse(rate_s, out rate);
        if (chkRateFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "rate格式錯誤";
            result["column"] = "rate";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (rate != betFormDefine.CalcBetFormRate(chooseBallObj, betType))
        {
            result["result"] = "fail";
            result["msg"] = "rate錯誤";
            result["column"] = "rate";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        string combo_s = (req["combo"] ?? "").ToString();

        int comboTemp;
        var chkComboFmt = int.TryParse(combo_s, out comboTemp);
        if (chkComboFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "combo格式錯誤";
            result["column"] = "combo";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (betFormDefine.CalcBetFormCombo(chooseBallObj, betType) != comboTemp)
        {
            result["result"] = "fail";
            result["msg"] = "combo錯誤";
            result["column"] = "combo";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        combo = comboTemp;

        string totalBet_s = (req["totalBet"] ?? "").ToString();

        var chkTotalBetFmt = decimal.TryParse(totalBet_s, out totalBet);
        if (chkTotalBetFmt == false)
        {
            result["result"] = "fail";
            result["msg"] = "totalBet格式錯誤";
            result["column"] = "totalBet";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (totalBet != betAmount * combo)
        {
            result["result"] = "fail";
            result["msg"] = "totalBet錯誤";
            result["column"] = "totalBet";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        decimal betMaxValueByBetType = memberDefine.GetMemberBetMaxValueByBetType(memberId, betType);
        decimal agentBetMaxValue = memberDefine.GetMemberAgentBetMaxValue(memberId, betType);

        decimal totalBetByMember = betFormDefine.GetTotalBetByMember(memberId, periodId, betType);
        decimal totalBetByAgent = betFormDefine.GetTotalBetByAgent(parentId, periodId);

        if (totalBet + totalBetByMember > betMaxValueByBetType)
        {
            result["result"] = "fail";
            result["msg"] = "超過單一玩法限制最大總計金額";
            result["column"] = "totalBet";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (totalBet + totalBetByAgent > agentBetMaxValue)
        {
            result["result"] = "fail";
            result["msg"] = "超過代理限制最大總計金額";
            result["column"] = "";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        if (totalBet > walletAmount)
        {
            result["result"] = "fail";
            result["msg"] = "餘額不足";
            result["column"] = "";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        walletAmount -= totalBet; // 錢包餘額扣掉下注總金額

        bool checkBetFormExist = betFormDefine.CheckBetFormExist(memberId, chooseBallObj, betType);
        if (checkBetFormExist == true)
        {
            result["result"] = "fail";
            result["msg"] = "重複下注";
            result["column"] = "";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        Model.BetForm.Add betFormAdd = new Model.BetForm.Add();
        Model.BetForm.Add.BetFormStruct b = new Model.BetForm.Add.BetFormStruct();

        b.memberId = memberId;
        b.BetType = betType;
        b.BetBranch = betBranch; // 暫時未使用
        b.ChooseBall = JsonConvert.SerializeObject(chooseBallObj);
        b.Rate = rate;
        b.Combo = combo;
        b.BetAmount = betAmount;
        b.TotalBet = totalBet;
        b.IP = ip;
        b.PeriodId = periodId;
        b.BetRemark = betRemark;

        betFormAdd.BetFormStructtHandle(b); // add 注單

        Model.Member.Edit memberEdit = new Model.Member.Edit();
        Model.Member.Edit.BalanceTransferStruct eb = new Model.Member.Edit.BalanceTransferStruct();

        eb.MemberId = memberId;
        eb.WalletAmount = totalBet;

        memberEdit.BalanceTransferStructHandle(eb); // 修改 walletAmount

        result["result"] = "success";
        result["msg"] = "第" + periodId + "期下注成功，下注金額為:" + totalBet.ToString() + "賠率為：" + rate.ToString() + " ，剩餘金額:" + walletAmount.ToString();
        result["column"] = "";

        Response.Write(JsonConvert.SerializeObject(result));
    }
}