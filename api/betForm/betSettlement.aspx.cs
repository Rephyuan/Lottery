using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

public partial class api_betSettlement : System.Web.UI.Page
{
    GlobalFunc glbf = new GlobalFunc();
    SqlConnection conn = new SqlConnection(GlobalVar.sql_con_str_main);
    JToken req;
    Model.BetForm.Define betFormDefine = new Model.BetForm.Define();

    protected void Page_Load(object sender, EventArgs e)
    {
        JToken result = new JObject();
        string method = Request.HttpMethod;
        if (method == "POST")
        {
            req = glbf.GetReqInputStream();
            string action = (req["action"] ?? "").ToString();
            if (action != "view" && action != "betSettlement")
            {
                result["result"] = "fail";
                result["errorCode"] = ApiErrorCodes.ActionError;
                Response.Write(JsonConvert.SerializeObject(result));
            }
            if (action == "view")
            {

            }
            else if (action == "betSettlement")
            {
                BetSettlement();
            }
        }
        Response.End();
    }

    void BetSettlement()
    {

        JToken result = new JObject();
        string lotteryResult = "01,02,03,44,05,07,04";
        string periodId = (req["period"] ?? "").ToString();

        Model.BetForm.List list = new Model.BetForm.List();
        Model.BetForm.List.BetFormPeriodStruct b = new Model.BetForm.List.BetFormPeriodStruct();
        b.PeriodId = periodId;

        var betList = list.BetFormPeriodStructHandle(b);

        foreach (var m in betList)
        {
            try
            {
                int id = (int)m["id"];
                string chooseBallStr = m["chooseBall"].ToString().TrimEnd(',');
                string betType = m["betType"].ToString();
                decimal betAmount = (decimal)m["betAmount"];
                decimal rate = (decimal)m["rate"];
                decimal winAmount = 0;

                switch (betType)
                {
                    case Model.BetForm.Define.BetTypes.TwoStar:
                        winAmount = TwoStar(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.ThreeStar:
                        winAmount = ThreeStar(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.FourStar:
                        winAmount = FourStar(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.ThreeBingoTwo:
                        winAmount = ThreeBingoTwo(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.TwoBingoSpecial:
                        winAmount = TwoBingoSpecial(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.Special3:
                        winAmount = Special3(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.Terrace:
                        winAmount = Terrace(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallOne:
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallTwo:
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallThree:
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallFour:
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallFive:
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallSix:
                    case Model.BetForm.Define.BetTypes.NormalSpecialBallSpecial:
                        winAmount = NormalSpecialBall(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialOne:
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialTwo:
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialThree:
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialFour:
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialFive:
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialSix:
                    case Model.BetForm.Define.BetTypes.BallColorNormalSpecialSpecial:
                        winAmount = BallColorNormalSpecial(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialOne:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialTwo:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialThree:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialFour:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialFive:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSix:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSpecial:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialOneSix:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialOneSpecial:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialNotBingo:
                        winAmount = ZodiacNormalSpecial(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSingle:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialDouble:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialBig:
                    case Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSmall:
                        winAmount = ZodiacNormalSpecialOther(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SpecialZodiacTwo:
                    case Model.BetForm.Define.BetTypes.SpecialZodiacThree:
                    case Model.BetForm.Define.BetTypes.SpecialZodiacFour:
                    case Model.BetForm.Define.BetTypes.SpecialZodiacFive:
                    case Model.BetForm.Define.BetTypes.SpecialZodiacSix:
                        winAmount = SpecialZodiac(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SpecialConnectTwo:
                        winAmount = SpecialConnectTwo(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SpecialConnectThree:
                        winAmount = SpecialConnectThree(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.DoubleConnetThree:
                        winAmount = DoubleConnetThree(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.DoubleConnetFour:
                        winAmount = DoubleConnetFour(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SkyBingoTwo:
                        winAmount = SkyBingoTwo(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SkyBingoThree:
                        winAmount = SkyBingoThree(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SkyThreePillar:
                        winAmount = SkyThreePillar(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.AllPass:
                        winAmount = AllPass(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SevenBingoSingle:
                    case Model.BetForm.Define.BetTypes.SevenBingoDouble:
                    case Model.BetForm.Define.BetTypes.SevenBingoBig:
                    case Model.BetForm.Define.BetTypes.SevenBingoSmall:
                        winAmount = SevenBingo(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.ZodiacConnetTwo:
                    case Model.BetForm.Define.BetTypes.ZodiacConnetThree:
                    case Model.BetForm.Define.BetTypes.ZodiacConnetFour:
                    case Model.BetForm.Define.BetTypes.ZodiacConnetTwoNotBingo:
                    case Model.BetForm.Define.BetTypes.ZodiacConnetThreeNotBingo:
                    case Model.BetForm.Define.BetTypes.ZodiacConnetFourNotBingo:
                        winAmount = ZodiacConnet(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SpecialConnectPillarTwo:
                    case Model.BetForm.Define.BetTypes.SpecialConnectPillarThree:
                        winAmount = SpecialConnectPillarTwo(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.PillarTwo:
                    case Model.BetForm.Define.BetTypes.PillarThree:
                    case Model.BetForm.Define.BetTypes.PillarFour:
                        winAmount = Pillar(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.AllCarBig:
                    case Model.BetForm.Define.BetTypes.AllCarSmall:
                    case Model.BetForm.Define.BetTypes.AllCarSingle:
                    case Model.BetForm.Define.BetTypes.AllCarDouble:
                    case Model.BetForm.Define.BetTypes.AllCar:
                        winAmount = AllCar(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleFive:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleSix:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleSeven:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleEight:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleNine:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleTen:
                        winAmount = SelectNumberBingoOneDouble(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneSingleFive:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneSingleSix:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneSingleSeven:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneSingleEight:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneSingleNine:
                    case Model.BetForm.Define.BetTypes.SelectNumberBingoOneSingleTen:
                        winAmount = SelectNumberBingoOneSingle(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleFive:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleSix:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleSeven:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleEight:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleNine:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleTen:
                        winAmount = SelectNumberNotBingoOneDouble(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneSingleFive:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneSingleSix:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneSingleSeven:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneSingleEight:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneSingleNine:
                    case Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneSingleTen:
                        winAmount = SelectNumberNotBingoOneSingle(betType, lotteryResult, chooseBallStr, betAmount, rate);
                        break;
                    default:

                        Exception(id);
                        break;
                }

                Model.BetForm.Edit edit = new Model.BetForm.Edit();
                Model.BetForm.Edit.LotteryBonusStruct lb = new Model.BetForm.Edit.LotteryBonusStruct();
                Model.Member.Define memberDefine = new Model.Member.Define();

                decimal l8PayProportion = memberDefine.GetMemberAgentPayProportion((int)m["l8"]);
                decimal l7PayProportion = memberDefine.GetMemberAgentPayProportion((int)m["l7"]);

                decimal r8 = 0;
                decimal r7 = 0;
                decimal r1 = 0;

                if (winAmount == 0)
                {
                    r1 = 0 - (decimal)m["totalBet"];
                    r8 = r1 * l8PayProportion;
                    r7 = r1 * l7PayProportion;
                }
                else if (winAmount > 0)
                {
                    r1 = winAmount - (decimal)m["totalBet"];
                    r8 = r1 * l8PayProportion;
                    r7 = r1 * l7PayProportion;
                }

                lb.WinAmount = winAmount;
                lb.id = id;
                lb.r8 = r8;
                lb.r7 = r7;
                lb.r1 = r1;
                lb.Status = Model.BetForm.Define.BetFormStatus.Success;

                edit.LotteryBonusHandle(lb);
            }
            catch (Exception e)
            {
                Model.BetForm.Edit edit = new Model.BetForm.Edit();
                Model.BetForm.Edit.LotteryBonusStruct lb = new Model.BetForm.Edit.LotteryBonusStruct();

                lb.id = (int)m["id"];
                lb.Status = Model.BetForm.Define.BetFormStatus.Invalid;
                edit.LotteryBonusHandle(lb);
            }

        }
        if (betList.Count > 0)
        {
            Model.BetForm.Edit.SettlementBonusStruct s = new Model.BetForm.Edit.SettlementBonusStruct();
            Model.BetForm.Edit e = new Model.BetForm.Edit();
            s.PeriodId = periodId;
            s.LotteryResult = lotteryResult;
            e.SettlementBonusHandle(s);
        }
    }

    void Exception(int id)
    {
        Model.BetForm.Edit edit = new Model.BetForm.Edit();
        Model.BetForm.Edit.LotteryBonusStruct lb = new Model.BetForm.Edit.LotteryBonusStruct();

        lb.id = id;
        lb.Status = Model.BetForm.Define.BetFormStatus.Invalid;
        edit.LotteryBonusHandle(lb);
    }
    decimal TwoStar(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//2二星
    {

        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);

        decimal winAmount = 0;

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 2);//組合(陣列,組合數列)

        foreach (string[] arr in ListCombination)
        {
            int count = 0;
            string ee = "";
            decimal[] rateArray = new decimal[2];
            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0) // 比對item內是否符合中獎號碼
                {
                    rateArray[count] = betFormDefine.MappingNumToRate(chooseBallStr, item); // Add賠率
                    count++;
                    ee += item + ",";
                }
            }
            if (arr.Length == count)
            {
                Array.Sort(rateArray);//排序賠率 低到高
                rate = rateArray[0];//排序陣列0為最低賠率
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal ThreeStar(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//3三星
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);

        decimal winAmount = 0;

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 3);//組合(陣列,組合數列)
        List<string> bingo = new List<string>();

        foreach (string[] arr in ListCombination)
        {
            int count = 0;
            string ee = "";
            decimal[] rateArray = new decimal[3];

            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)// 比對item內是否符合中獎號碼
                {
                    rateArray[count] = betFormDefine.MappingNumToRate(chooseBallStr, item);// Add賠率
                    count++;
                    ee += item + ",";
                }
            }
            if (arr.Length == count)
            {
                Array.Sort(rateArray);//排序賠率 低到高
                rate = rateArray[0];//排序陣列0為最低賠率
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal FourStar(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//4四星
    {
        List<string> combinaltion = new List<string>();

        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);

        decimal winAmount = 0;

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 4);
        List<string> bingo = new List<string>();

        foreach (string[] arr in ListCombination)
        {
            int count = 0;
            string ee = "";
            decimal[] rateArray = new decimal[4];

            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[count] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    count++;
                    ee += item + ",";
                }
            }
            if (arr.Length == count)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal Special3(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//20特三
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);

        decimal winAmount = 0;

        string chooseBall = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];

        string[] convertBall = normalBall.ToString().Split(',');

        //台號需要排序 小到大
        Array.Sort(convertBall);
        string ball = "";
        for (int i = 0; i < convertBall.Length - 1; i++)
        {
            ball += convertBall[i].ToString().Substring(1, 1) + convertBall[i + 1].ToString().Substring(1, 1) + ",";
        }

        string[] arrayBall = ball.ToString().TrimEnd(',').Split(',');

        //台號中間三碼尾數
        string s3 = arrayBall[1].ToString().Substring(1, 1) + arrayBall[2].ToString().Substring(1, 1) + arrayBall[3].ToString().Substring(1, 1);


        if (s3 == chooseBall)
        {
            winAmount += betAmount * rate;
        }

        return winAmount;
    }
    decimal Terrace(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//21台號
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);

        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string chooseBall = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];

        string[] convertBall = normalBall.ToString().Split(',');
        //台號需要排序 小到大
        Array.Sort(convertBall);
        string ball = "";
        for (int i = 0; i < convertBall.Length - 1; i++)
        {
            ball += convertBall[i].ToString().Substring(1, 1) + convertBall[i + 1].ToString().Substring(1, 1) + ",";
        }

        if (ball.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
        {
            winAmount += betAmount * rate;
        }

        return winAmount;
    }
    decimal NormalSpecialBall(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//13特碼 正碼 1~7
    {
        string normalBall1 = lotteryResult.Substring(0, 17);
        string specialBalls = lotteryResult.Substring(18, 2);
        string chooseBalls = "";

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string ballType = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];

        switch (ballType)
        {
            case Model.BetForm.Define.NormalSpecialBallOneDefine.Big:
                chooseBalls = "25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.Small:
                chooseBalls = "01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.SumBig:
                chooseBalls = "07,08,09,16,17,18,19,25,26,27,28,29,34,35,36,37,38,39,43,44,45,46,47,48,49";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.SumSmall:
                chooseBalls = "01,02,03,04,05,06,10,11,12,13,14,15,20,21,22,23,24,30,31,32,33,40,41,42";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.SumSingle:
                chooseBalls = "01,03,05,07,09,10,12,14,16,18,21,23,25,27,29,30,32,34,36,38,41,43,45,47,49";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.SumDouble:
                chooseBalls = "02,04,06,08,11,13,15,17,19,20,22,24,26,28,31,33,35,37,39,40,42,44,46,48";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.Single:
                chooseBalls = "01,03,05,07,09,11,13,15,17,19,21,23,25,27,29,31,33,35,37,39,41,43,45,47,49";
                break;
            case Model.BetForm.Define.NormalSpecialBallOneDefine.Double:
                chooseBalls = "02,04,06,08,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38,40,42,44,46,48";
                break;
            default:
                chooseBalls = ballType;
                break;
        }

        decimal winAmount = 0;

        string[] normalBall = normalBall1.ToString().Split(',');
        string[] specialBall = specialBalls.ToString().Split(',');
        string[] chooseBall = chooseBalls.ToString().Split(',');

        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallOne)
        {
            foreach (var o in chooseBall)
            {
                if (normalBall[0].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallTwo)
        {
            foreach (var o in chooseBall)
            {
                if (normalBall[1].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallThree)
        {
            foreach (var o in chooseBall)
            {
                if (normalBall[2].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallFour)
        {
            foreach (var o in chooseBall)
            {
                if (normalBall[3].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallFive)
        {
            foreach (var o in chooseBall)
            {
                if (normalBall[4].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallSix)
        {
            foreach (var o in chooseBall)
            {
                if (normalBall[5].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.NormalSpecialBallSpecial)
        {
            foreach (var o in chooseBall)
            {
                if (specialBalls.IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }

        return winAmount;
    }
    decimal BallColorNormalSpecial(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//14球色特碼 正碼 1~7
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        string chooseBalls = "";
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string ballType = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];

        switch (ballType)
        {
            case Model.BetForm.Define.BallColorNormalSpecialDefine.Blue:
                chooseBalls = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueSingle:
                chooseBalls = "03,09,15,25,31,37,41,47";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueDouble:
                chooseBalls = "04,10,14,20,26,36,42,48";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueBig:
                chooseBalls = "25,26,31,36,37,41,42,47,48";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueSmall:
                chooseBalls = "03,04,09,10,14,15,20";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueBigSingle:
                chooseBalls = "25,31,37,41,47";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueBigDouble:
                chooseBalls = "26,36,42,48";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueSmallSingle:
                chooseBalls = "03,09,15";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.BlueSmallDouble:
                chooseBalls = "04,10,14,20";
                break;

            case Model.BetForm.Define.BallColorNormalSpecialDefine.Red:
                chooseBalls = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedSingle:
                chooseBalls = "01,07,13,19,23,29,35,45";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedDouble:
                chooseBalls = "02,08,12,18,24,30,34,40,46";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedBig:
                chooseBalls = "29,30,34,35,40,45,46";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedSmall:
                chooseBalls = "01,02,07,08,12,13,18,19,23,24";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedBigSingle:
                chooseBalls = "29,35,45";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedBigDouble:
                chooseBalls = "30,34,40,46";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedSmallSingle:
                chooseBalls = "01,07,13,19,23";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.RedSmallDouble:
                chooseBalls = "02,08,12,18,24";
                break;

            case Model.BetForm.Define.BallColorNormalSpecialDefine.Green:
                chooseBalls = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenSingle:
                chooseBalls = "05,11,17,21,27,33,39,43,49";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenDouble:
                chooseBalls = "06,16,22,28,32,38,44";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenBig:
                chooseBalls = "27,28,32,33,38,39,43,44,49";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenSmall:
                chooseBalls = "05,06,11,16,17,21,22";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenBigSingle:
                chooseBalls = "27,33,39,43,49";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenBigDouble:
                chooseBalls = "28,32,38,44";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenSmallSingle:
                chooseBalls = "05,11,17,21";
                break;
            case Model.BetForm.Define.BallColorNormalSpecialDefine.GreenSmallDouble:
                chooseBalls = "06,16,22";
                break;

        }
        decimal winAmount = 0;

        string[] normalBallArray = normalBall.ToString().Split(',');
        string[] specialBallArray = specialBall.ToString().Split(',');
        string[] chooseBall = chooseBalls.ToString().Split(',');

        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialOne)
        {
            foreach (var o in chooseBall)
            {
                if (normalBallArray[0].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialTwo)
        {
            foreach (var o in chooseBall)
            {
                if (normalBallArray[1].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialThree)
        {
            foreach (var o in chooseBall)
            {
                if (normalBallArray[2].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialFour)
        {
            foreach (var o in chooseBall)
            {
                if (normalBallArray[3].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialFive)
        {
            foreach (var o in chooseBall)
            {
                if (normalBallArray[4].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialSix)
        {
            foreach (var o in chooseBall)
            {
                if (normalBallArray[5].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.BallColorNormalSpecialSpecial)
        {
            foreach (var o in chooseBall)
            {
                if (specialBallArray[0].IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    winAmount = betAmount * rate;
                }
            }
        }

        return winAmount;
    }
    decimal ZodiacNormalSpecial(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//161~7生肖特碼;正碼 
    {
        string lotteryResultZodiac = betFormDefine.ConvertNormalToZodiac(lotteryResult);
        string normalBall = lotteryResultZodiac.Substring(0, 17);
        string specialBall = lotteryResultZodiac.Substring(18, 2);

        decimal winAmount = 0;
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string chooseBall = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];

        string[] strArr = chooseBall.TrimEnd(',').Split(',');

        string[] normalBallArray = normalBall.ToString().Split(',');
        string[] specialBallArray = specialBall.ToString().Split(',');


        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialOne)
        {
            if (normalBallArray[0].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialTwo)
        {
            if (normalBallArray[1].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialThree)
        {
            if (normalBallArray[2].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialFour)
        {
            if (normalBallArray[3].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialFive)
        {
            if (normalBallArray[4].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSix)
        {
            if (normalBallArray[5].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSpecial)
        {
            if (specialBallArray[0].IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }

        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialOneSix)
        {
            if (normalBall.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount += betAmount * rate;
            }
        }

        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialOneSpecial)
        {
            if (normalBall.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }

            if (specialBall.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }

        }

        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialNotBingo)
        {
            if (normalBall.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) < 0 && specialBall.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) < 0)
            {
                winAmount = betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal ZodiacNormalSpecialOther(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//171~174生肖總和
    {

        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;
        string[] normalBallArray = normalBall.ToString().Split(',');
        string[] specialBallArray = specialBall.ToString().Split(',');

        int sumNumber = 0;
        int singleDouble = 0;
        foreach (var o in normalBallArray)
        {
            sumNumber += int.Parse(o);
        }
        sumNumber += int.Parse(specialBallArray[0]);
        if (sumNumber % 2 == 0)
        {
            singleDouble = 0;
        }
        if (sumNumber % 2 == 1)
        {
            singleDouble = 1;
        }

        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSingle)
        {
            if (singleDouble == 1)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialDouble)
        {
            if (singleDouble == 0)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialBig)
        {
            if (sumNumber >= 175)
            {
                winAmount = betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.ZodiacNormalSpecialSmall)
        {
            if (sumNumber <= 174)
            {
                winAmount = betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SpecialZodiac(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate) // 352-356 特別號生肖
    {
        string lotteryResultZodiac = betFormDefine.ConvertNormalToZodiac(lotteryResult);
        string specialBall = lotteryResultZodiac.Substring(18, 2);
        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] chooseBallArray = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        foreach (var b in chooseBallArray)
        {
            if (specialBall.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount = betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SelectNumberBingoOneDouble(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate) // 231-236 N 選中一(複) 
    {
        decimal winAmount = 0;

        List<string> combinaltion = new List<string>();

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = new List<string[]>();

        if (betType == Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleFive)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 5);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleSix)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 6);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleSeven)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 7);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleEight)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 8);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleNine)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 9);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberBingoOneDoubleTen)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 10);
        }

        foreach (string[] arr in ListCombination)
        {
            int count = 0;
            int index = 0;
            decimal[] rateArray = new decimal[arr.Length];

            foreach (string item in arr)
            {
                rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                index++;
                if (lotteryResult.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    count++;
                }
            }
            if (count == 1)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SelectNumberBingoOneSingle(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate) // 241-246 N 選中一(單) 
    {
        decimal winAmount = 0;

        int count = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        int index = 0;
        decimal[] rateArray = new decimal[strArr.Length];
        foreach (var o in strArr)
        {
            rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, o);
            index++;
            if (lotteryResult.IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                count++;
            }
        }

        if (count == 1)
        {
            Array.Sort(rateArray);
            rate = rateArray[0];
            winAmount += betAmount * rate;
        }

        return winAmount;
    }
    decimal SelectNumberNotBingoOneDouble(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//251 - 256 N選不中(複) 
    {
        decimal winAmount = 0;

        List<string> combinaltion = new List<string>();

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = new List<string[]>();

        if (betType == Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleFive)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 5);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleSix)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 6);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleSeven)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 7);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleEight)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 8);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleNine)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 9);
        }
        if (betType == Model.BetForm.Define.BetTypes.SelectNumberNotBingoOneDoubleTen)
        {
            ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 10);
        }

        List<string> bingo = new List<string>();

        foreach (string[] arr in ListCombination)
        {
            int count = 0;
            int index = 0;
            decimal[] rateArray = new decimal[arr.Length];

            foreach (string item in arr)
            {
                rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                index++;
                if (lotteryResult.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SelectNumberNotBingoOneSingle(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//261-266 選不中(單) 
    {
        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();
        int count = 0;

        int index = 0;
        decimal[] rateArray = new decimal[strArr.Length];
        foreach (var o in strArr)
        {
            rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, o);
            index++;

            if (lotteryResult.IndexOf(o, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                count++;
            }
        }

        if (count == 0)
        {
            Array.Sort(rateArray);
            rate = rateArray[0];
            winAmount += betAmount * rate;
        }

        return winAmount;
    }
    decimal SpecialConnectTwo(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//29特串2
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string> combinaltion = new List<string>();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 2);

        foreach (string[] arr in ListCombination)
        {
            int count1 = 0;
            int count2 = 0;
            int index = 0;

            decimal[] rateArray = new decimal[2];

            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count1++;
                }
                if (specialBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count2++;
                }
            }

            if (count1 > 0 && count2 > 0)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SpecialConnectThree(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//30特串3
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string> combinaltion = new List<string>();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 3);

        foreach (string[] arr in ListCombination)
        {
            int count1 = 0;
            int count2 = 0;
            int index = 0;
            decimal[] rateArray = new decimal[3];
            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    count1++;
                }

                if (specialBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    count2 = 1;
                }

                rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                index++;
            }

            if (count1 > 1 && count2 > 0)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal DoubleConnetThree(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//8雙星連柱碰三星
    {
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        string chooseBalls = chooseBallStr;
        decimal winAmount = 0;

        decimal[] rateArray = new decimal[50];
        string strTwo = "";
        string strThree = "";

        var s = JsonConvert.DeserializeObject<JObject>(chooseObj["1"].ToString()); //連碰二星
        foreach (var a in s)
        {
            rateArray[int.Parse(a.Key)] = (decimal)a.Value;
            strTwo += a.Key + ",";
        }
        var n = JsonConvert.DeserializeObject<JObject>(chooseObj["2"].ToString()); //組三星
        foreach (var b in n)
        {
            rateArray[int.Parse(b.Key)] = (decimal)b.Value;
            strThree += b.Key + ",";
        }

        string[] twoArr = strTwo.TrimEnd(',').Split(',');
        string[] threeArr = strThree.TrimEnd(',').Split(',');


        List<string> combinaltion = new List<string>();


        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(twoArr, 2);
        List<string> bingo = new List<string>();
        foreach (var t in threeArr)
        {
            foreach (string[] arr in ListCombination)
            {
                int count = 0;
                string ee = "";
                string[] chooseArr = new string[] { };
                decimal[] ballRateArray = new decimal[3];
                //新陣列 先把t新增[0]
                Array.Resize(ref chooseArr, chooseArr.Length + 1);
                chooseArr[chooseArr.Length - 1] = t.ToString();
                //再把arr 加入相同陣列[1][2]
                for (int i = 0; i < arr.Length; i++)
                {
                    Array.Resize(ref chooseArr, chooseArr.Length + 1);
                    chooseArr[chooseArr.Length - 1] = arr[i].ToString();
                }

                foreach (string item in chooseArr)
                {
                    if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ballRateArray[count] = rateArray[int.Parse(item)];
                        count++;
                        ee += item + ",";
                    }
                }

                if (count == 3)
                {
                    Array.Sort(ballRateArray);
                    rate = ballRateArray[0];
                    winAmount += betAmount * rate;
                }
            }
        }

        return winAmount;
    }
    decimal DoubleConnetFour(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//9雙連碰四星
    {
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        string chooseBalls = chooseBallStr;
        decimal winAmount = 0;
        decimal[] rateArray = new decimal[50];
        string strTwo1 = "";
        string strTwo2 = "";

        var s = JsonConvert.DeserializeObject<JObject>(chooseObj["1"].ToString()); //連碰二星
        foreach (var a in s)
        {
            rateArray[int.Parse(a.Key)] = (decimal)a.Value;
            strTwo1 += a.Key + ",";
        }
        var n = JsonConvert.DeserializeObject<JObject>(chooseObj["2"].ToString()); //連碰二星 組四星
        foreach (var b in n)
        {
            rateArray[int.Parse(b.Key)] = (decimal)b.Value;
            strTwo2 += b.Key + ",";
        }

        string[] twoArr1 = strTwo1.TrimEnd(',').Split(',');
        string[] twoArr2 = strTwo2.TrimEnd(',').Split(',');

        List<string> combinaltion = new List<string>();

        int testcount = 0;

        List<string[]> twoArrList1 = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(twoArr1, 2);
        List<string[]> twoArrList2 = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(twoArr2, 2);
        List<string> bingo = new List<string>();
        foreach (string[] t in twoArrList2)
        {
            foreach (string[] arr in twoArrList1)
            {
                int count = 0;
                testcount++;
                string ee = "";
                string[] chooseArr = new string[] { };
                decimal[] ballRateArray = new decimal[4];

                for (int i = 0; i < t.Length; i++)
                {
                    Array.Resize(ref chooseArr, chooseArr.Length + 1);
                    chooseArr[chooseArr.Length - 1] = t[i].ToString();
                }

                for (int i = 0; i < arr.Length; i++)
                {
                    Array.Resize(ref chooseArr, chooseArr.Length + 1);
                    chooseArr[chooseArr.Length - 1] = arr[i].ToString();
                }

                foreach (string item in chooseArr)
                {
                    if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ballRateArray[count] = rateArray[int.Parse(item)];
                        count++;
                        ee += item + ",";
                    }
                }

                if (count == 4)
                {
                    Array.Sort(ballRateArray);
                    rate = ballRateArray[0];
                    winAmount += betAmount * rate;
                }
            }
        }

        return winAmount;
    }
    decimal SkyBingoTwo(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//27天地碰二星
    {
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;
        string special = "";
        string normal = "";

        string[] specialArray = new string[] { };
        string[] normalArray = new string[] { };

        var s = JsonConvert.DeserializeObject<JObject>(chooseObj["s"].ToString());
        foreach (var a in s)
        {
            special += a.Key + ",";
        }
        var n = JsonConvert.DeserializeObject<JObject>(chooseObj["1"].ToString());
        foreach (var b in n)
        {
            normal += b.Key + ",";
        }

        specialArray = special.TrimEnd(',').Split(',');
        normalArray = normal.TrimEnd(',').Split(',');

        string[] twoArrS = chooseObj["s"].ToString().Split(',');
        string[] chooseBall = chooseObj["1"].ToString().Split(',');

        List<string> combinaltion = new List<string>();

        int testcount = 0;

        List<string> bingo = new List<string>();

        foreach (string t in specialArray)
        {
            foreach (string arr in normalArray)
            {
                int count = 0;
                testcount++;
                string ee = "";
                decimal[] rateArray = new decimal[2];

                if (normalBall.IndexOf(arr, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[count] = decimal.Parse(chooseObj["1"][arr].ToString());
                    count++;
                    ee += arr + ",";
                }
                if (specialBall.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[count] = decimal.Parse(chooseObj["s"][t].ToString());
                    count++;
                    ee += t + ",";
                }

                if (count == 2)
                {
                    Array.Sort(rateArray);
                    rate = rateArray[0];
                    winAmount += betAmount * rate;
                }
            }
        }

        return winAmount;
    }
    decimal SkyBingoThree(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//28天地碰三星
    {
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;
        string[] twoArrS = chooseObj["s"].ToString().Split(',');
        string[] chooseBall = chooseObj["1"].ToString().Split(',');

        string special = "";
        string normal = "";
        string[] specialArray = new string[] { };
        string[] normalArray = new string[] { };
        var s = JsonConvert.DeserializeObject<JObject>(chooseObj["s"].ToString());
        foreach (var a in s)
        {
            special += a.Key + ",";
        }
        var n = JsonConvert.DeserializeObject<JObject>(chooseObj["1"].ToString());
        foreach (var b in n)
        {
            normal += b.Key + ",";
        }

        specialArray = special.TrimEnd(',').Split(',');
        normalArray = normal.TrimEnd(',').Split(',');



        int testcount = 0;
        List<string[]> ThreeArrList = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(normalArray, 2);

        List<string> bingo = new List<string>();

        foreach (string t in specialArray)
        {
            foreach (string[] arr in ThreeArrList)
            {
                int count = 0;
                testcount++;
                string ee = "";
                decimal[] rateArray = new decimal[arr.Length + 1];
                foreach (var item in arr)
                {
                    if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rateArray[count] = decimal.Parse(chooseObj["1"][item].ToString());
                        count++;
                        ee += arr + ",";
                    }
                }

                if (specialBall.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[count] = decimal.Parse(chooseObj["s"][t].ToString());
                    count++;
                    ee += t + ",";
                }

                if (count == 3)
                {
                    Array.Sort(rateArray);
                    rate = rateArray[0];
                    winAmount += betAmount * rate;
                }
            }
        }

        return winAmount;
    }
    decimal AllPass(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//過關
    {
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string redBall = "01,02,07,08,12,13,18,19,23,24,29,30,34,35,40,45,46";
        string blueBall = "03,04,09,10,14,15,20,25,26,31,36,37,41,42,47,48";
        string greenBall = "05,06,11,16,17,21,22,27,28,32,33,38,39,43,44,49";

        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        string chooseBalls = chooseBallStr;
        decimal winAmount = 0;


        string[] normalBallArray = normalBall.ToString().Split(',');
        string[] specialBallArray = specialBall.ToString().Split(',');
        Array.Resize(ref normalBallArray, normalBallArray.Length + 1);
        normalBallArray[normalBallArray.Length - 1] = specialBall.ToString();

        int count = 0;
        foreach (var o in chooseObj)
        {
            var a = JsonConvert.DeserializeObject<JObject>(o.Value.ToString());

            foreach (var q in a)
            {
                int key = int.Parse(o.Key.ToString());
                if (rate == 0)
                {
                    rate = (decimal)q.Value;
                }
                else
                {
                    rate = rate * (decimal)q.Value;
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Single)
                {
                    if (int.Parse(normalBallArray[key - 1]) % 2 == 1)
                    {
                        count++;
                    }
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Double)
                {
                    if (int.Parse(normalBallArray[key - 1]) % 2 == 0)
                    {
                        count++;
                    }
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Big)
                {
                    if (int.Parse(normalBallArray[key - 1]) >= 25)
                    {
                        count++;
                    }
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Small)
                {
                    if (int.Parse(normalBallArray[key - 1]) <= 24)
                    {
                        count++;
                    }
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Red)
                {
                    if (redBall.IndexOf(normalBall[key - 1].ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        count++;
                    }
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Blue)
                {
                    if (blueBall.IndexOf(normalBall[key - 1].ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        count++;
                    }
                }
                if (q.Key.ToString() == Model.BetForm.Define.AllPassDefine.Green)
                {
                    if (greenBall.IndexOf(normalBall[key - 1].ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        count++;
                    }
                }
            }
        }

        if (count == chooseObj.Count)
        {
            winAmount = betAmount * rate;
        }

        return winAmount;
    }
    decimal SevenBingo(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate) // 7碼位數
    {
        string[] ball = lotteryResult.ToString().Split(',');
        int singleNum = 0;
        int doubleNum = 0;
        int bigNum = 0;
        int smallNum = 0;
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string chooseBall = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];
        decimal winAmount = 0;

        foreach (string o in ball)
        {
            if (int.Parse(o) % 2 == 1)
            {
                singleNum++;
            }
            if (int.Parse(o) % 2 == 0)
            {
                doubleNum++;
            }
            if (int.Parse(o) >= 25)
            {
                bigNum++;
            }
            if (int.Parse(o) <= 24)
            {
                smallNum++;
            }
        }

        if (betType == Model.BetForm.Define.BetTypes.SevenBingoSingle)
        {
            if (chooseBall == singleNum.ToString("00"))
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.SevenBingoDouble)
        {
            if (chooseBall == doubleNum.ToString("00"))
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.SevenBingoBig)
        {
            if (chooseBall == bigNum.ToString("00"))
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.SevenBingoSmall)
        {
            if (chooseBall == smallNum.ToString("00"))
            {
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal ZodiacConnet(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//18-二三次肖連
    {
        string zodiacLotteryResult = betFormDefine.ConvertNormalToZodiac(lotteryResult);
        decimal winAmount = 0;
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] zodiacArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        int chooseCnt = 0;

        int bingoCnt = 0;

        if (betType == Model.BetForm.Define.BetTypes.ZodiacConnetTwo)
        {
            chooseCnt = 2;
            bingoCnt = 2;
        }

        else if (betType == Model.BetForm.Define.BetTypes.ZodiacConnetThree)
        {
            chooseCnt = 3;
            bingoCnt = 3;
        }
        else if (betType == Model.BetForm.Define.BetTypes.ZodiacConnetFour)
        {
            chooseCnt = 4;
            bingoCnt = 4;
        }
        else if (betType == Model.BetForm.Define.BetTypes.ZodiacConnetTwoNotBingo)
        {
            chooseCnt = 2;
        }
        else if (betType == Model.BetForm.Define.BetTypes.ZodiacConnetThreeNotBingo)
        {
            chooseCnt = 3;
        }
        else if (betType == Model.BetForm.Define.BetTypes.ZodiacConnetFourNotBingo)
        {
            chooseCnt = 4;
        }
        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(zodiacArr, chooseCnt);

        foreach (string[] arr in ListCombination)
        {
            decimal[] rateArray = new decimal[arr.Length];

            int index = 0;

            int matchCnt = 0;

            foreach (string item in arr)
            {
                rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                index++;

                if (zodiacLotteryResult.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchCnt++;
                }
            }

            if (matchCnt == bingoCnt)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }

    decimal Pillar(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//立柱二三四星
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;
        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string allBall = "";
        List<string[]> listTwo = new List<string[]>();

        decimal[] rateArrayAll = new decimal[50];
        //取出各柱組合
        foreach (var o in chooseObj)
        {
            var jj = JsonConvert.DeserializeObject<JObject>(o.Value.ToString());
            string okey = "";
            foreach (var b in jj)
            {
                okey += b.Key.ToString() + ",";
                allBall += b.Key.ToString() + ",";
                rateArrayAll[int.Parse(b.Key)] = (decimal)b.Value;
            }
            string[] temp = okey.TrimEnd(',').Split(',');
            for (int a = 2; a < 5; a++)
            {
                if (temp.Length >= a)
                {
                    List<string[]> List1 = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(temp, a);
                    foreach (var ee in List1)
                    {
                        listTwo.Add(ee);
                    }
                }
            }
        }

        //根據BETTYPES 全部球組合
        string[] allBallArr = allBall.ToString().TrimEnd(',').Split(',');
        List<string[]> ListCombinationCheck = new List<string[]>();
        if (betType == Model.BetForm.Define.BetTypes.PillarTwo)
        {
            ListCombinationCheck = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(allBallArr, 2);
        }
        if (betType == Model.BetForm.Define.BetTypes.PillarThree)
        {
            ListCombinationCheck = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(allBallArr, 3);
        }
        if (betType == Model.BetForm.Define.BetTypes.PillarFour)
        {
            ListCombinationCheck = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(allBallArr, 4);
        }

        //全部球組合移除各柱組合
        foreach (var pp in ListCombinationCheck.ToArray())
        {
            int index = 0;
            foreach (var rr in listTwo)
            {
                int count = 0;

                foreach (var d in rr)
                {
                    foreach (var w in pp)
                    {
                        if (w.IndexOf(d, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            count++;
                        }
                    }
                    if (count >= 2)
                    {
                        ListCombinationCheck.Remove(pp);
                    }
                }
            }
            index++;
        }

        List<string> bingo = new List<string>();

        foreach (string[] arr in ListCombinationCheck)
        {
            int count = 0;

            decimal[] rateArray = new decimal[arr.Length];

            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[count] = rateArrayAll[int.Parse(item)];
                    count++;
                }
            }
            if (arr.Length == count)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SkyThreePillar(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//天三立柱
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;

        List<string[]> listTwo = new List<string[]>();

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        foreach (var o in chooseObj)
        {
            if (o.Key.ToString() != "s")
            {
                string[] temp = betFormDefine.GetSelectedNum(chooseBallStr, o.Key.ToString(), true).ToArray();

                if (temp.Length >= 2)
                {
                    List<string[]> list1 = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(temp, 2);

                    listTwo.AddRange(list1);
                }
            }
        }

        string[] allBallArr = betFormDefine.GetSelectedNum(chooseBallStr, "all", true).ToArray();
        string[] specialBallArray = betFormDefine.GetSelectedNum(chooseBallStr, "s", false).ToArray();

        List<string[]> ListCombinationCheck = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(allBallArr, 2);

        foreach (var pp in ListCombinationCheck.ToArray())
        {
            int index = 0;
            foreach (var rr in listTwo)
            {
                int count = 0;

                foreach (var d in rr)
                {
                    foreach (var w in pp)
                    {
                        if (w.IndexOf(d, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            count++;
                        }
                    }
                    if (count >= 2)
                    {
                        ListCombinationCheck.Remove(pp);
                    }
                }
            }
            index++;
        }

        foreach (string[] arr in ListCombinationCheck)
        {
            int count1 = 0;
            int count2 = 0;
            int index = 0;
            decimal[] ballRateArray = new decimal[arr.Length + 1];

            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ballRateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count1++;
                }

            }
            if (specialBallArray.Contains<string>(specialBall))
            {
                ballRateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, specialBall);
                index++;
                count2++;
            }
            if (count1 == 2 && count2 == 1)
            {
                Array.Sort(ballRateArray);
                rate = ballRateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
    decimal SpecialConnectPillarTwo(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//特串二三立柱
    {
        int chooseCnt = 0;
        int bingoCnt = 0;
        if (betType == Model.BetForm.Define.BetTypes.SpecialConnectPillarTwo)
        {
            chooseCnt = 2;
            bingoCnt = 2;
        }
        else if (betType == Model.BetForm.Define.BetTypes.SpecialConnectPillarThree)
        {
            chooseCnt = 3;
            bingoCnt = 3;
        }

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;
        string[] chooseBallArray = chooseBallStr.Split(',');


        List<string[]> listTwo = new List<string[]>();
        foreach (var o in chooseObj)
        {
            if (o.Key.ToString() != "s")
            {
                string[] temp = betFormDefine.GetSelectedNum(chooseBallStr, o.Key.ToString(), true).ToArray();

                if (temp.Length >= 2)
                {
                    List<string[]> list1 = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(temp, 2);

                    listTwo.AddRange(list1);
                }
            }
        }

        string[] allBallArr = betFormDefine.GetSelectedNum(chooseBallStr, "all", true).ToArray();

        //所有球組合 (包含各柱內)
        List<string[]> ListCombinationCheck = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(allBallArr, chooseCnt);
        //所有球組合中移除各柱組合
        foreach (var b in ListCombinationCheck.ToArray())
        {
            int index = 0;
            foreach (var rr in listTwo)
            {
                int count = 0;

                foreach (var d in rr)
                {
                    foreach (var w in b)
                    {
                        if (w.IndexOf(d, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            count++;
                        }
                    }
                    if (count >= 2)
                    {
                        ListCombinationCheck.Remove(b);
                    }
                }
            }
            index++;
        }

        foreach (string[] arr in ListCombinationCheck)
        {
            int count1 = 0;
            int count2 = 0;
            decimal[] ballRateArray = new decimal[arr.Length];
            int index = 0;
            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    //ADD 賠率陣列1~49 到中獎的賠率陣列
                    ballRateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count1++;
                }
                if (specialBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ballRateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count2++;
                }
            }

            if (count1 >= bingoCnt - 1 && count2 >= 1)
            {
                Array.Sort(ballRateArray);
                rate = ballRateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }

    decimal ThreeBingoTwo(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//三中二
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 3);

        List<string> bingo = new List<string>();

        foreach (string[] arr in ListCombination)
        {
            int count = 0;
            int index = 0;
            decimal[] rateArray = new decimal[arr.Length];

            foreach (string item in arr)
            {
                rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                index++;
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    count++;
                }
            }
            if (count == 2)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }

    decimal TwoBingoSpecial(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//二中特
    {
        string normalBall = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;

        var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

        string[] strArr = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray();

        List<string[]> ListCombination = Model.BetForm.Define.PermutationAndCombination<string>.GetCombination(strArr, 2);

        foreach (string[] arr in ListCombination)
        {
            int count1 = 0;
            int count2 = 0;
            int index = 0;
            decimal[] rateArray = new decimal[arr.Length];
            foreach (string item in arr)
            {
                if (normalBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count1++;
                }
                if (specialBall.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    rateArray[index] = betFormDefine.MappingNumToRate(chooseBallStr, item);
                    index++;
                    count2++;
                }
            }
            if (count1 == 1 && count2 == 1)
            {
                Array.Sort(rateArray);
                rate = rateArray[0];
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }

    decimal AllCar(string betType, string lotteryResult, string chooseBallStr, decimal betAmount, decimal rate)//全車
    {
        string normalBall_n = lotteryResult.Substring(0, 17);
        string specialBall = lotteryResult.Substring(18, 2);
        decimal winAmount = 0;


        string chooseBall = "";

        if (betType == Model.BetForm.Define.BetTypes.AllCar)
        {
            var chooseObj = JsonConvert.DeserializeObject<JObject>(chooseBallStr);

            chooseBall = betFormDefine.GetSelectedNum(chooseBallStr, "0", true).ToArray()[0];
            rate = betFormDefine.MappingNumToRate(chooseBallStr, chooseBall);
        }

        string[] normalBall = normalBall_n.ToString().Split(',');

        int sum = 0;
        foreach (var s in normalBall)
        {
            sum += int.Parse(s);
        }

        if (betType == Model.BetForm.Define.BetTypes.AllCarBig)
        {
            if (sum >= 150)
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.AllCarSmall)
        {
            if (sum <= 149)
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.AllCarSingle)
        {
            if (sum % 2 == 1)
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.AllCarDouble)
        {
            if (sum % 2 == 0)
            {
                winAmount += betAmount * rate;
            }
        }
        if (betType == Model.BetForm.Define.BetTypes.AllCar)
        {
            if (normalBall_n.IndexOf(chooseBall, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                winAmount += betAmount * rate;
            }
        }

        return winAmount;
    }
}
