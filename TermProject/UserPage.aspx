<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="TermProject.UserPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>User Page</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlUpgradeManager" runat="server">
        <h4>Need more storage? Upgrade you max storage capacity here!</h4>
        <h4>Please select a plan, enter your credit card info and hit submit</h4>
        <asp:DropDownList ID="ddlPlanOptions" DataTextField="Description" DataValueField="OptionID" AutoPostBack="true" OnSelectedIndexChanged="ddlPlanOptions_SelectedIndexChanged" runat="server" />
        <br />
        <br />
        <asp:Table ID="tblTransaction" Visible="false" runat="server" Height="165px" Width="436px">
            <asp:TableRow>
                <asp:TableCell>
                    <p>Enter Credit Card Information</p>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="lblAmountDue" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>CreditCard Number:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="txtCC" TextMode="Number" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>CCV Number:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="txtCCV" TextMode="Number" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" runat="server" />
                </asp:TableCell><asp:TableCell>
                    <asp:Button ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Label ID="lblPaymentStatus" runat="server" />
    </asp:Panel>

    <asp:Panel ID="pnlFileManager" runat="server">
        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        <br />
        <h2>User Cloud Storage Manager</h2>
        <br />
        <br />
        <h4>Upload a new file</h4>
        <asp:Label ID="lblFileError" runat="server" Text="" Visible="false"></asp:Label>&nbsp;<asp:Label ID="lblTest" runat="server" Text=""></asp:Label><br />
        <asp:FileUpload ID="FileUploadNew" runat="server" /><br />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
        <br />
        <br />
        <asp:Label ID="lblFileStatus" runat="server" Text=""></asp:Label>
        <asp:GridView ID="gvUserCloud" runat="server" AutoGenerateColumns="false" OnRowCommand="gvUserCloud_RowCommand">
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
                <asp:ButtonField ButtonType="Button" runat="server" HeaderText="Download File" Text="Download" CommandName="gvCommandDownload" CausesValidation="false" />
                <asp:ButtonField ButtonType="Button" runat="server" HeaderText="View Versions" Text="View" CommandName="gvCommandViewVersions" CausesValidation="false" />
                <asp:ButtonField ButtonType="Button" runat="server" HeaderText="Delete File" Text="Delete" CommandName="gvCommandDelete" CausesValidation="false" />
            </Columns>
        </asp:GridView>
        <br />
        <br />
        <h4>File Update</h4>
        <p>Choose a file to update:</p>
        <asp:DropDownList ID="ddlFiles" AutoPostBack="true" DataTextField="FileName" DataValueField="FileID" runat="server" />
        <asp:FileUpload ID="FileUploadUpdate" runat="server" /><br />
        <asp:Button ID="btnUpdateFile" runat="server" Text="Update" OnClick="btnUpdateFile_Click" />
        <br />
        <br />
        <asp:GridView ID="gvViewVersions" AutoGenerateColumns="false" OnRowCommand="gvViewVersions_RowCommand" Visible="false" runat="server">
            <Columns>
                <asp:BoundField DataField="VaultID" HeaderText="File ID" />
                <asp:BoundField DataField="MasterFileID" HeaderText="Master File ID" />
                <asp:BoundField DataField="FileName" HeaderText="File Name" />
                <asp:BoundField DataField="FileType" HeaderText="File Type" />
                <asp:BoundField DataField="FileVersion" HeaderText="Version" />
                <asp:BoundField DataField="FileSize" HeaderText="File Size" />
                <asp:BoundField DataField="DateTimeStamp" HeaderText="Time Stamp" />
                <asp:ButtonField ButtonType="Button" runat="server" HeaderText="Restore Version" Text="Restore" CommandName="gvCommandRestoreVersion" CausesValidation="false" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblViewVersionStatus" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnViewDeletedFiles" Text="View Deleted Files" OnClick="btnViewDeletedFiles_Click" runat="server" />
        <br />
        <br />
        <asp:GridView ID="gvDeletedFiles" AutoGenerateColumns="false" OnRowCommand="gvDeletedFiles_RowCommand" Visible="false" runat="server">
            <Columns>
                <asp:BoundField DataField="FileID" HeaderText="File ID" />
                <asp:BoundField DataField="FileName" HeaderText="File Name" />
                <asp:BoundField DataField="FileType" HeaderText="File Type" />
                <asp:BoundField DataField="FileVersion" HeaderText="Version" />
                <asp:BoundField DataField="FileSize" HeaderText="File Size" />
                <asp:ButtonField ButtonType="Button" runat="server" HeaderText="Restore Version" Text="Restore" CommandName="gvCommandRestoreVersion" CausesValidation="false" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblViewDeletedStatus" runat="server" />
        <br />
        <br />
        <h4>Clear All Storage</h4>
        <p>Would you like to delete all of your files and reset the amount of storage you are using?</p>
        <asp:Table ID="tblClearStorage" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btnClearStorage" Text="Clear Storage" OnClick="btnClearStorage_Click" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="lblClearStorageStatus" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btnYes" Text="Yes" Visible="false" OnClick="btnConfirm_Click" runat="server" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="btnNo" Text="No" Visible="false" OnClick="btnConfirm_Click" runat="server" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <br />
    </asp:Panel>
</asp:Content>
