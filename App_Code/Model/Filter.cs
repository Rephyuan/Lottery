using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Filter
/// </summary>
public class Filter
{
    public Filter()
    {
        
    }

    public string GetMemberLevels()
    {
        JArray ja = new JArray();
        foreach (var o in Model.Member.Define.MemberLevelsLangMap)
        {
            JToken jt = new JObject();
            jt["name"] = o.Key;
            jt["value"] = o.Value;
            ja.Add(jt);
        }
        var j = JsonConvert.SerializeObject(ja).Replace("\"", "'");
        return j;
    }

    public string GetNormalSpecialBallStatus()
    {
        JArray ja = new JArray();
        foreach (var o in Model.BetForm.Define.NormalSpecialBallOneLangMap)
        {
            JToken jt = new JObject();
            jt["name"] = o.Key;
            jt["value"] = o.Value;
            ja.Add(jt);
        }
        var j = JsonConvert.SerializeObject(ja).Replace("\"", "'");
        return j;
    }
    public string GetZodiacStatus()
    {
        JArray ja = new JArray();
        foreach (var o in Model.BetForm.Define.ZodiacStatusLangMap)
        {
            JToken jt = new JObject();
            jt["name"] = o.Key;
            jt["value"] = o.Value;
            ja.Add(jt);
        }
        var j = JsonConvert.SerializeObject(ja).Replace("\"", "'");
        return j;
    }

    public string GetBallColorNormalSpecialStatus()
    {
        JArray ja = new JArray();
        foreach (var o in Model.BetForm.Define.BallColorNormalSpecialLangMap)
        {
            JToken jt = new JObject();
            jt["name"] = o.Key;
            jt["value"] = o.Value;
            ja.Add(jt);
        }
        var j = JsonConvert.SerializeObject(ja).Replace("\"", "'");
        return j;
    }

    public string GetBetTypesStatus()
    {
        JArray ja = new JArray();
        foreach (var o in Model.BetForm.Define.BetTypesLangMap)
        {
            JToken jt = new JObject();
            jt["name"] = o.Key;
            jt["value"] = o.Value;
            ja.Add(jt);
        }
        var j = JsonConvert.SerializeObject(ja).Replace("\"", "'");
        return j;
    }

}