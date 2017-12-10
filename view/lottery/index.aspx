<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="view_lottery_index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        var id = <%=Session["id"]%>;
        var externalId = <%=Session["externalId"]%>;
        var levelId = <%=Session["levelId"]%>;
        var parentId =<%=Session["parentId"]%>;
        var companyId = <%=Session["companyId"]%>;
        var walletAmount = <%=Session["walletAmount"]%>;
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    </div>
    </form>
</body>
</html>
