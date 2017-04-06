<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TermProject.LogInPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Custom styles for this template -->
    <link href="loginstyles.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="loginstyles.css">

    <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
</head>

<body>
    <div class="container">
        <form id="frmLogin" runat="server" class="form-signin">
            <h2 class="form-signin-heading">Login:</h2>
            <asp:TextBox ID="txtEmail" class="form-control" placeholder="Email address" TextMode="Email" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
            <br />
            <br />
            <asp:CheckBox ID="chkRem" runat="server" value="remember-me" Text="Remember Me" />
            <br />
            <br />
            <asp:Button ID="btnLogin" Class="btnLogin" type="submit" OnClick="btnLogin_Click" runat="server" Text="Log In" />
            <br />
            <br />
            <asp:Label ID="lblStatus" Class="lblStatus" runat="server" />
        </form>
    </div>
</body>
</html>
