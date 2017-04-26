<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="TermProject.UserPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label><br />

    <h2>User Cloud Storage Manager</h2><br />
    <asp:Label ID="lblStatus2" runat="server" Text=""></asp:Label>
    <asp:GridView ID="gvUserCloud" runat="server" AutoGenerateColumns="False" OnRowCommand="gvUserCloud_RowCommand">
        <Columns>
            <asp:BoundField DataField="FileID" HeaderText="File ID" />
            <asp:BoundField DataField="UserID" HeaderText="User ID" />
            <asp:BoundField DataField="FileName" HeaderText="File Name" />
            <asp:BoundField DataField="FileType" HeaderText="File Type" />
            <asp:BoundField DataField="FileVersion" HeaderText="Version" />
            <asp:BoundField DataField="FileSize" HeaderText="File Size" />
            <asp:ButtonField ButtonType="Button" runat="server" HeaderText="Download File" Text="Download" CommandName="gvCommandDownload" CausesValidation="false"/>
        </Columns>
    </asp:GridView><br />



</asp:Content>
