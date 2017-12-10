using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class api_member_depositWalletAmount : System.Web.UI.Page
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
            if (action != "view" && action != "depositWalletAmount")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {

            }
            else if (action == "depositWalletAmount")
            {
                depositWalletAmount();
            }
            Response.End();
        }
    }

    void depositWalletAmount()
    {
        JToken result = new JObject();

        Model.Member.Define memberDefine = new Model.Member.Define();

        int memberId;
        decimal memberWalletAmount;

        string transferType;
        decimal walletDiff;

        int agentId;

        string transferType_s = (req["transferType"] ?? "").ToString();
        string walletAmount_s = (req["walletAmount"] ?? "").ToString();

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
        memberWalletAmount = memberDefine.GetMemberWalletAmount(memberId);

        bool conversionWalletAmount = decimal.TryParse(walletAmount_s, out walletDiff);
        if (conversionWalletAmount == false)
        {
            result["result"] = "fail";
            result["msg"] = "金額格式錯誤";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }

        bool checkTransferType = Model.Member.Define.TransactionTypesLangMap.ContainsKey(transferType_s);
        if (checkTransferType == false)
        {
            result["result"] = "fail";
            result["msg"] = "交易型態錯誤";
            Response.Write(JsonConvert.SerializeObject(result));
            return;
        }
        transferType = transferType_s;

        if (transferType == Model.Member.Define.TransactionType.Debits)
        {
            if (walletDiff > memberWalletAmount)
            {
                result["result"] = "fail";
                result["msg"] = "餘額不足";
                Response.Write(JsonConvert.SerializeObject(result));
                return;
            }

            memberWalletAmount -= walletDiff;
        }
        else if (transferType == Model.Member.Define.TransactionType.Deposit)
        {
            memberWalletAmount += walletDiff;
        }

        Model.Member.Edit.BalanceTransferStruct m = new Model.Member.Edit.BalanceTransferStruct();
        Model.Member.Edit edit = new Model.Member.Edit();

        m.MemberId = memberId;
        m.WalletAmount = memberWalletAmount;

        edit.BalanceTransferStructHandle(m);

        result["result"] = "success";
        result["msg"] = "交易成功,交易金額為:" + walletDiff.ToString() + "，目前餘額為:" + memberWalletAmount.ToString();

        Response.Write(JsonConvert.SerializeObject(result));
    }
}