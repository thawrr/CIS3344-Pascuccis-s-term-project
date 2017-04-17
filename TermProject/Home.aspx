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
                                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" />
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
                    <h3>Account Info - Edit or Delete Fields</h3>
                    <div class="col-lg-12">
                    </div>
                </div>
            </div>
            <!--end row 3-->
            <div class="row">
                <div class="col-lg-8">
                    <h3>File Delete</h3>
                    <asp:GridView ID="gvFiles" AutoGenerateColumns="true" OnRowDeleting="gvFiles_RowDeleting" runat="server">
                        <Columns>
                            <asp:CommandField ButtonType="Button" HeaderText="Delete File" ShowDeleteButton="True" />
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
                    <asp:DropDownList ID="ddlFiles" DataTextField="FileName" DataValueField="FileID" runat="server" />
                    <asp:FileUpload ID="FileUploadUpdate" runat="server" /><br />
                    <asp:Button ID="btnUpdateFile" runat="server" Text="Update" OnClick="btnUpdateFile_Click" />
                </div>
            </div>
            <!--end row 5-->
            <div class="row">
                <div class="col-lg-8">
                    <h3>File Transactions</h3>
                    <asp:GridView ID="gvTransactions" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="LoginID" HeaderText="Email" SortExpression="LoginID" />
                            <asp:BoundField DataField="HashedPassword" HeaderText="Password" SortExpression="HashedPassword" />
                            <asp:BoundField DataField="StorageCapacity" HeaderText="Storage Capacity " ReadOnly="True" SortExpression="StorageCapacity" />
                            <asp:BoundField DataField="StorageUsed" HeaderText="Storage Used" ReadOnly="True" SortExpression="StorageUsed" />
                            <asp:BoundField DataField="Account" HeaderText="Account" ReadOnly="True" SortExpression="Account" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!--end row 6-->
        </div>
    </div>
</asp:Content>
