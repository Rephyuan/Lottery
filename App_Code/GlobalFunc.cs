using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Configuration;

public class GlobalFunc
{
    public JToken GetReqInputStream()
    {
        using (StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream))
        {
            JToken jt;
            string ss = sr.ReadToEnd();
            try
            {
                jt = JsonConvert.DeserializeObject<JObject>(ss);
                if (jt.Type != JTokenType.Object) jt = new JObject();
                return jt;
            }
            catch
            {
                jt = new JObject();
                return jt;
            }
        }
    }

    public string GetClientIP()
    {
        HttpRequest request = HttpContext.Current.Request;
        string request_url = HttpContext.Current.Request.Url.Host;
        string ip;
        string true_client_ip_main_cdn = (request.ServerVariables["HTTP_TRUE_CLIENT_IP"] ?? "").ToString();
        string true_client_ip_second_cdn = (request.ServerVariables["HTTP_CF_CONNECTING_IP"] ?? "").ToString();
        string true_client_ip_third_cdn = (request.ServerVariables["HTTP_INCAP_CLIENT_IP"] ?? "").ToString();

        if (true_client_ip_main_cdn == "")
        {
            if (true_client_ip_second_cdn == "")
            {
                if (true_client_ip_third_cdn == "")
                {
                    
                        //if (request_url.IndexOf("api") != 0)
                        //{
                        //    HttpContext.Current.Response.End();
                        //}
                    
                    string x_forwarded = (request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? "").ToString();
                    if (x_forwarded == "") ip = request.ServerVariables["REMOTE_ADDR"].ToString();
                    else ip = x_forwarded;
                }
                else
                {
                    ip = true_client_ip_third_cdn;
                }
            }
            else
            {
                ip = true_client_ip_second_cdn;
            }
        }
        else ip = true_client_ip_main_cdn;
        int ip_comm = ip.IndexOf(",");
        if (ip_comm != -1) ip = ip.Remove(ip_comm);
        return ip;
    }

    public int GetLoginStatus()
    {
        if(HttpContext.Current.Session["levelId"] != null)
        {
            int levelId = int.Parse(HttpContext.Current.Session["levelId"].ToString());

            if (levelId >= 7) return 1; // agent account

            return 2; // member account
        }
        return 0; // 未登入
    }

    public string GetSQLSameLine()
    {
        string where_str = "";

        if (HttpContext.Current.Session["id"] != null)
        {
            int id = (int)HttpContext.Current.Session["id"];
            int levelId = (int)HttpContext.Current.Session["levelId"];

            where_str = "l" + levelId + "=" + id;
        }

        return where_str;
    }
}