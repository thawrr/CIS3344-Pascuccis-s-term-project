<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomControlTimer.ascx.cs" Inherits="TermProject.CustomControlTimer" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Timer ID="TimerCLOCK" runat="server" Interval="1000"></asp:Timer>
        <asp:Label ID="lblCLOCK" runat="server" Text=""> </asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>