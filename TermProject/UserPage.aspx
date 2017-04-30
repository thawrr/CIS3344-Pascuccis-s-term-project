<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="TermProject.UserPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label><br />

    <h2>User Cloud Storage Manager</h2>
    <br />
    <asp:Label ID="lblStatus2" runat="server" Text=""></asp:Label>
    <asp:GridView ID="gvUserCloud" runat="server" AutoGenerateColumns="False" OnRowCommand="gvUserCloud_RowCommand">
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
        </Columns>
    </asp:GridView>

    <br />
    <asp:Panel ID="pnlUpgrade" runat="server">
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
</asp:Content>
