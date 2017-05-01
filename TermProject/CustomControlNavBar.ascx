<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomControlNavBar.ascx.cs" Inherits="TermProject.CustomControlNavBar" %>

<nav class="navbar" id="top" role="banner">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                </div>
                <div class="subtitle col-md-4">
                    <a class="navbar-brand" style="color: antiquewhite">Term Project</a>
                </div>
                <div class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-left">
                        <li class="navlinks"><a href="Home.aspx">Home</a></li>
                        <li class="navlinks"><a href="AdminTools.aspx">Admin Tools</a></li>
                        <li class="navlinks"><a href="UserPage.aspx">User Cloud</a></li>
                        <li class="navlinks"><a href="TechSupport.aspx">Tech Support</a></li>
                        <li class="navlinks"><a href="Login.aspx">Login</a></li>
                        <li class="navlinks"><a href="Registration.aspx">Registration</a></li>
                        <li class="navlinks">
                            <a>
                                <asp:Button ID="btnLogout" Text="Log Out" Class="navButton" OnClick="btnLogout_Click" runat="server" />
                            </a>
                        </li>
                        <li class="navlinks">
                            <a>
                                <asp:Button ID="btnSync" Text="Sync" Class="navButton" OnClick="btnSync_Click" runat="server" />
                            </a>
                        </li>
                        <li class="navlinks">
                            <a>
                                <asp:Label ID="lblSyncStatus" runat="server" />
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
