<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TermProject.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <title>Home Page</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h2>Welcome to you cloud storage home page</h2>

    <h3>This application allows you to upload supported file types up to 4 mb per file.</h3>

    <h3>Use the nav bar up top to get around the application</h3>

    <h3>Some pages are only accessible if you are an admin. You will be redirected to your cloud page if you do not have sufficient privileges</h3>    
</asp:Content>
