<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="TermProject.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Label ID="lblStatus" Class="lblStatus" runat="server" />
    <br />

   <div class="row">
        <div class="col-lg-12">
            <h3>Account Info - Add a User</h3>
            <p>New user's role is automatically 'Cloud User' and storage capacity will be set at 256,000 bytes</p>
            <p>Consult an admin to elevate status</p>
            <asp:Table ID="tblNewUser" runat="server">
                <asp:TableRow>
                    <asp:TableCell>Full Name:</asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtFullName" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>Email (LoginID):</asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" TextMode="Email" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>Password:</asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPassword" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddUser" Text="Submit" OnClick="btnAddUser_Click" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Label ID="lblAddStatus" runat="server" />
        </div>
    </div>
</asp:Content>
