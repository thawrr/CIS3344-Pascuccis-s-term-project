<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="AdminTools.aspx.cs" Inherits="TermProject.AdminTools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>Admin Tools</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Label ID="lblStatus" runat="server" />

    <asp:Panel ID="pnlAdminTools" runat="server">
        <div class="row">
            <%--Add a User--%>
            <div class="col-lg-8">
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

            <%--Update a User--%>
            <div class="col-lg-8">
                <h3>Account Info - Update/Deactivate Account</h3>
                <p>Select a user to edit</p>
                <p>You can change basic info, storage capacity, and de/activate users.</p>
                <p>Select a user from the list below to Update</p>
                <asp:DropDownList ID="ddlUser" AutoPostBack="true" DataTextField="Name" DataValueField="UserID" runat="server" />
                <br />
                <asp:Button ID="btnSelectUpdateUser" Text="Select" OnClick="btnSelectUpdateUser_Click" runat="server" />
                <br />
                <br />
                <asp:Table ID="tblUpdateUser" Visible="false" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>User ID:</asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblUserID" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Full Name:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtUpdateName" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Email (LoginID):</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtUpdateEmail" TextMode="Email" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Password:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtUpdatePassword" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Storage Capactiy:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtUpdateCapacity" TextMode="Number" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Role:</asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlRole" AutoPostBack="true" DataTextField="RoleDescription" DataValueField="RoleID" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Active:</asp:TableCell>
                        <asp:TableCell>
                            <asp:CheckBox ID="chkActive" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <br />
                            <asp:Button ID="btnUpdateAccount" Text="Submit" OnClick="btnUpdateAccount_Click" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Label ID="lblUpdateStatus" runat="server" />
            </div>

            <%--View User Transactions--%>
            <div class="col-lg-8">
                <h3>User Transactions</h3>
                <p>Select a user from the list below to view their transactions or select to view all transactions from a given interval.</p>
                <asp:DropDownList ID="ddlUserTrans" AutoPostBack="true" DataTextField="Name" DataValueField="UserID" runat="server" />
                <br />
                <asp:Button ID="btnSelectUserTrans" Text="Select" OnClick="btnSelectUserTrans_Click" runat="server" />
                <br />
                <br />
                <asp:DropDownList ID="ddlInterval" AutoPostBack="true" DataTextField="Description" DataValueField="Interval" runat="server" />
                <br />
                <asp:Button ID="btnSelectTransByDate" Text="Select" OnClick="btnSelectTransByDate_Click" runat="server" />
                <br />
                <br />
                <asp:GridView ID="gvTransactions" AutoGenerateColumns="False" Visible="false" runat="server">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="User Name" />
                        <asp:BoundField DataField="TransDesc" HeaderText="Description" />
                        <asp:BoundField DataField="DateTimeStamp" HeaderText="Date" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblTransactionStatus" runat="server" />
            </div>
            <br />
            <div class="col-lg-8">
                <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server"></asp:ScriptManagerProxy>
                <script type="text/javascript">
                    var xmlhttp;
                    try {
                        // Code for IE7+, Firefox, Chrome, Opera, Safari
                        xmlhttp = new XMLHttpRequest();
                    }
                    catch (try_older_microsoft) {

                        try {
                            // Code for IE6, IE5
                            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                        }
                        catch (other) {
                            xmlhttp = false;
                            alert("Your browser doesn't support AJAX!");
                        }
                    }
                </script>
                <h3>Live Cloud User Transactions</h3>
                <p>Grid updates every second</p>
                <asp:UpdatePanel ID="UpdatePanelTransaction" runat="server">
                    <ContentTemplate>
                        <asp:Timer ID="TimerTransaction" runat="server" Interval="1000" OnTick="TimerTransaction_Tick"></asp:Timer>
                        <asp:GridView ID="gvAllCloudUserTrans" runat="server" AutoGenerateColumns="true">
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlSuperAdminTools" Visible="false" runat="server">
        <div class="row">
            <%--View Admin Transactions--%>
            <div class="col-lg-8">
                <h3>Admin Transactions</h3>
                <p>Select an admin from the list below to view their transactions or select to view all transactions from a given interval.</p>
                <asp:DropDownList ID="ddlAdmins" AutoPostBack="false" DataTextField="Name" DataValueField="UserID" runat="server" />
                <br />
                <asp:Button ID="btnSelectAdminTrans" Text="Select" OnClick="btnSelectAdminTrans_Click" runat="server" />
                <br />
                <br />
                <asp:DropDownList ID="ddlSuperInterval" AutoPostBack="false" DataTextField="Description" DataValueField="Interval" runat="server" />
                <br />
                <asp:Button ID="btnAdminTransByDate" Text="Select" OnClick="btnAdminTransByDate_Click" runat="server" />
                <br />
                <br />
                <asp:GridView ID="gvAdminTransactions" AutoGenerateColumns="False" Visible="false" runat="server">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="User Name" />
                        <asp:BoundField DataField="RoleDescription" HeaderText="Role" />
                        <asp:BoundField DataField="TransDesc" HeaderText="Description" />
                        <asp:BoundField DataField="DateTimeStamp" HeaderText="Date" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblSuperTransactionStatus" runat="server" />
            </div>

            <%--View Admin Transactions--%>
            <div class="col-lg-8">
                <h3>View User Files</h3>
                <p>Select a cloud user from the list below to view their current active files.</p>
                <asp:DropDownList ID="ddlCloudUsers" DataTextField="Name" DataValueField="UserID" AutoPostBack="true" OnSelectedIndexChanged="ddlCloudUsers_SelectedIndexChanged" runat="server" />
                <br />
                <br />
                <asp:GridView ID="gvUserCloud" AutoGenerateColumns="false" runat="server">
                    <Columns>
                        <asp:BoundField DataField="FileID" HeaderText="File ID" />
                        <asp:BoundField DataField="UserID" HeaderText="User ID" />
                        <asp:BoundField DataField="FileName" HeaderText="File Name" />
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Image ImageUrl='<% #Eval("ImageURL")%>' Height="40" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FileType" HeaderText="File Type" />
                        <asp:BoundField DataField="FileVersion" HeaderText="Version" />
                        <asp:BoundField DataField="FileSize" HeaderText="File Size" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblUserFileStatus" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <br />
    <br />
</asp:Content>
