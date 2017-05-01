<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="TechSupport.aspx.cs" Inherits="TermProject.TechSupport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>Tech Support</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Label ID="lblStatus" runat="server" />

    <asp:Panel ID="pnlChangePassword" runat="server">
        <h3>Change your login password here.</h3>
        <p>*Password must contain at least 6 characters and at least 1 special character.</p>
        <asp:TextBox ID="txtUpdatePassword" runat="server" />
        <br />
        <asp:Button ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" runat="server" />
        <br />
        <asp:Label ID="lblUpdateStatus" runat="server" />
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnlAsk" Visible="false" runat="server">
        <h3>Post a question here.</h3>
        <asp:TextBox ID="txtQuestion" runat="server" Rows="3" TextMode="MultiLine" Width="274px"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmitQuestion" runat="server" Text="Submit" OnClick="btnSubmitQuestion_Click" />
        <br />
        <asp:Label ID="lblAskStatus" runat="server" />
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnlAnswer" Visible="true" runat="server">
        <h3>Answer a question here.</h3>
        <br />
        <asp:DropDownList ID="ddlUnansweredQuestions" DataTextField="Question" DataValueField="SupportID" AutoPostBack="false" runat="server" />
        <br />
        <asp:TextBox ID="txtAnswer" runat="server" Rows="3" TextMode="MultiLine" Width="274px"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmitAnswer" runat="server" Text="Submit" OnClick="btnSubmitAnswer_Click" />
        <br />
        <asp:Label ID="lblAnswerStatus" runat="server" />
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnlViewQuestions" runat="server">
        <h3>View already answered questions here.</h3>
        <br />
        <asp:GridView ID="gvViewQuestions" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" />
                <asp:BoundField DataField="Answer" HeaderText="Admin Answer" SortExpression="Answer" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
