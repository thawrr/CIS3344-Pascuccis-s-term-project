<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="TechSupport.aspx.cs" Inherits="TermProject.TechSupport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>Tech Support</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

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

    <asp:UpdatePanel ID="UpdatePanelQA" runat="server">
        <ContentTemplate>
            <asp:Timer ID="timerQA" runat="server" Interval="1000" OnTick="timerQA_Tick"></asp:Timer>
            <asp:Panel ID="pnlViewQuestions" runat="server">
                <h3>You will be able to see questions as they are asked and answered.</h3>
                <br />
                <h3>Live Cloud User Transactions</h3>
                <p>Grid updates every second</p>
                <br />
                <asp:Label ID="lblQAStatus" runat="server" />
                <asp:GridView ID="gvViewQuestions" AllowPaging="true" PageSize="5" AutoGenerateColumns="false" OnPageIndexChanging="gvViewQuestions_PageIndexChanging" runat="server">
                    <Columns>
                        <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" />
                        <asp:BoundField DataField="Answer" HeaderText="Admin Answer" SortExpression="Answer" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
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
        <asp:TextBox ID="txtQuestion" runat="server" Rows="2" TextMode="MultiLine" Width="274px"></asp:TextBox>
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
        <asp:TextBox ID="txtAnswer" runat="server" Rows="2" TextMode="MultiLine" Width="274px"></asp:TextBox>
        <br />
        <asp:Button ID="btnSubmitAnswer" runat="server" Text="Submit" OnClick="btnSubmitAnswer_Click" />
        <br />
        <asp:Label ID="lblAnswerStatus" runat="server" />
    </asp:Panel>
    <br />
    <br />
</asp:Content>
