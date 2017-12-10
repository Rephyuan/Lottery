using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Global
/// </summary>
public class GlobalVar
{
    public static string sql_con_str_main = ConfigurationManager.ConnectionStrings["lotteryConnectionString"].ConnectionString;
}

public class ApiErrorCodes
{
    public const string ActionError = "0001";//傳入動作字串錯誤
    public const string UsernameFormatError = "0002";//帳號格式錯誤
    public const string PasswordFormatError = "0003";//密碼格式錯誤
    public const string NotFindUsername = "0004";//帳號不存在
    public const string UsernameExist = "0005";//帳號重複
    public const string AgentIdExistFalse = "0025";//AgentId 不存在
    public const string DateTimeError = "0029";//日期格式錯誤
    public const string DateNull = "0031";//日期必須輸入
    public const string DateTimeInterval30 = "0032";//時間間隔超過30分
    public const string ServerError = "0033";//請稍後再試
}





