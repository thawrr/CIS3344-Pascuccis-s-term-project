<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TermProject.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>Home Page</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div class="row">
        <div class="col-lg-8">
            <asp:Label ID="lblStatus" Class="lblStatus" runat="server" />
            <h2>WebService Method Test Page</h2>
            <br />
            <p>All WebMethods are secured using User Credentials</p>
            <br />
            <div class="row">

                <div class="col-lg-12">
                    <h3>Upload a file</h3>
                    <asp:Label ID="lblFileError" runat="server" Text="" Visible="false"></asp:Label>&nbsp;<asp:Label ID="lblTest" runat="server" Text=""></asp:Label><br />
                    <asp:FileUpload ID="FileUpload1" runat="server" /><br />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                </div>
            </div>
            <!--end row 1-->
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
            <!--end row 2-->
            <div class="row">
                <div class="col-lg-12">
                    <h3>Account Info - Update Account</h3>
                    <p>Select a user to edit</p>
                    <p>Method assumes user is an admin, therefore can update any user's information.</p>
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
                            <asp:TableCell>
                                <asp:Button ID="btnUpdateAccount" Text="Submit" OnClick="btnUpdateAccount_Click" runat="server" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:Label ID="lblUpdateStatus" runat="server" />
                </div>
            </div>
            <!--end row 3-->
            <div class="row">
                <div class="col-lg-8">
                    <h3>File Delete</h3>
                    <asp:Label ID="lblStorageInfo" runat="server" />
                    <br />
                    <asp:GridView ID="gvFiles" AutoGenerateColumns="true" OnRowDeleting="gvFiles_RowDeleting" runat="server">
                        <Columns>
                            <asp:CommandField ButtonType="Button" HeaderText="Delete File" ShowDeleteButton="True" />
                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <asp:Image ImageURL='<% #Eval("ImageURL")%>' Height="40" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblDeleteStatus" runat="server" />
                </div>
            </div>
            <!--end row 4-->
            <div class="row">
                <div class="col-lg-8">
                    <h3>File Update</h3>
                    <p>Choose a file to update:</p>
                    <asp:DropDownList ID="ddlFiles" AutoPostBack="true" DataTextField="FileName" DataValueField="FileID" runat="server" />
                    <asp:FileUpload ID="FileUploadUpdate" runat="server" /><br />
                    <asp:Button ID="btnUpdateFile" runat="server" Text="Update" OnClick="btnUpdateFile_Click" />
                </div>
            </div>
            <!--end row 5-->
            <div class="row">
                <div class="col-lg-8">
                    <h3>File Transactions</h3>
                    <p>Select a user from the list below to view their transactions.</p>
                    <p>Currently set to show all transactions from the last 2 days.</p>
                    <asp:DropDownList ID="ddlUserTrans" AutoPostBack="true" DataTextField="Name" DataValueField="UserID" runat="server" />
                    <br />
                    <asp:Button ID="btnSelectUserTrans" Text="Select" OnClick="btnSelectUserTrans_Click" runat="server" />
                    <br />
                    <br />
                    <asp:GridView ID="gvTransactions" AutoGenerateColumns="False" Visible="false" runat="server">
                        <Columns>
                            <asp:BoundField DataField="UserID" HeaderText="UserID" />
                            <asp:BoundField DataField="FileID" HeaderText="File Num" />
                            <asp:BoundField DataField="TransDesc" HeaderText="Description" />
                            <asp:BoundField DataField="DateTimeStamp" HeaderText="Date" />
                        </Columns>
                    </asp:GridView>

                    <asp:Label ID="lblTransactionStatus" runat="server" />
                </div>
            </div>
            <!--end row 6-->
        </div>
    </div>
</asp:Content>
