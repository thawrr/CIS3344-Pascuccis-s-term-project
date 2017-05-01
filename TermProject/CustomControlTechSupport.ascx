<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomControlTechSupport.ascx.cs" Inherits="TermProject.CustomControlTechSupport" %>

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

<asp:ScriptManager ID="ScriptManagerQA" runat="server"></asp:ScriptManager>

<asp:UpdatePanel ID="UpdatePanelQA" runat="server">
    <ContentTemplate>
        <asp:Timer ID="timerQA" runat="server" Interval="1" OnTick="timerQA_Tick"></asp:Timer>
        <asp:Panel ID="pnlViewQuestions" runat="server">
            <h3>You will be able to see questions as they are asked and answered.</h3>
            <br />
            <h3>Live Cloud User Transactions</h3>
            <p>Grid updates every second</p>
            <br />
            <asp:Label ID="lblQAStatus" runat="server" />
            <asp:GridView ID="gvViewQuestions" AutoGenerateColumns="false" runat="server">
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
