<%@ Application Language="C#" CodeBehind="Global.asax.cs" Inherits="Global" %>
<%@ Import namespace="System.Web.Routing" %>
<%@ Import namespace="System.IO" %>

<script runat="server">

    private void Application_Start(object sender, EventArgs e)
    {
        //RouteTable.Routes.MapPageRoute("", "index", "~/index.aspx"); 
        //add route
        string appPath = AppDomain.CurrentDomain.BaseDirectory;
        string subPath = appPath + "api";
        AddRoute(subPath, subPath, false);
        subPath = appPath + "view";
        AddRoute(subPath, subPath, true);
    }

    private void AddRoute(string path, string path_full, bool hideParent)
    {
        //是資料夾
        if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                AddRoute(file, path_full, hideParent);
            }
            var view = Directory.GetDirectories(path);
            foreach (string subPath in view)
            {
                AddRoute(subPath, path_full, hideParent);
            }
        }
        else
        {
            string fileExt = Path.GetExtension(path);
            string fileName = Path.GetFileName(path);//index.aspx
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);//index
            if (fileExt.ToLower() != ".cs")
            {
                if (path_full.Length != path.Length)
                {
                    int start_relative = path.IndexOf('\\', path_full.Length + 1);
                    string parentDirName = Path.GetFileName(path_full);
                    if (start_relative != -1)
                    {
                        string relativeDirName = path.Substring(path_full.Length + 1, path.LastIndexOf('\\') - path_full.Length - 1).Replace("\\", "/");// member/user
                        RouteTable.Routes.MapPageRoute("", (hideParent ? "" : parentDirName + "/") + relativeDirName + "/" + fileNameWithoutExt, "~/" + parentDirName + "/" + relativeDirName + "/" + fileName);
                    }
                    else
                    {
                        RouteTable.Routes.MapPageRoute("", (hideParent ? "" : parentDirName + "/") + parentDirName + "/" + fileNameWithoutExt, "~/" + parentDirName + "/" + fileName);
                    }
                }
            }
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
