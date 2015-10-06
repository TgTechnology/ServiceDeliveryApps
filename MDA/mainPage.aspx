<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="mainPage.aspx.cs" Inherits="MDA.mainPage" EnableViewState="True" %>
<%@ Import Namespace="System.Security.Principal" %>
<html>
<head>
    <title>Main Page</title>
    <link rel="stylesheet" type="text/css" href="css/stylesheet.css">
</head>
  <body>
    <form id="Form1" method="post" runat="server" enctype="multipart/form-data">
    <div class="indexTopper">
        <div class="title"></div>
        <asp:Label ID="loginName" Text="" runat="server"></asp:Label>
        <asp:Button ID="LogOut" class="logout" OnClick="LogOut_Click" runat="server" />
        <%--<button type="button" class="profile" id="Button1" onclick="location.href='profilePage.html'"></button>--%>
        <button type="button" class="profile" id="profile" ></button>
    </div>
    <div class="contentLeft" style="z-index:1">
        <asp:Label ID="lblName" Runat=server />
        <asp:Label ID="lblAuthType" Runat=server />
        <div id="customerSite"></div>
        <asp:DropDownList id="ddlCustomers" style="margin-top:5px" runat="server" AutoPostBack="true" EnableViewState="true" onselectedindexchanged="ddlCustomers_SelectedIndexChanged">
        </asp:DropDownList>
        <div id="pcr" style="top:200px"></div>
        <asp:TextBox id="pcrinput" style="margin-top:5px" runat="server" Enabled="false" />
        <object id="MultiFileUploader" data="data:application/x-silverlight-2," type="application/x-silverlight-2" style="margin-top:30px; margin-left:15px;" width="500" height="65">
            <param name="source" value="ClientBin/mpost.SilverlightSingleFileUpload.xap" />
            <param name="onerror" value="onSilverlightError" />
            <param name="initParams" value="MaxFileSizeKB=,FileFilter=,ChunkSize=4194304,CustomParams=yourparameters,DefaultColor=White" />
            <param name="background" value="white" />
            <param name="onload" value="pluginLoaded" />
            <param name="minRuntimeVersion" value="5.0.61118.0" />
            <param name="autoUpgrade" value="true" />
            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.0.61118.0" style="text-decoration: none">
            <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight"
            style="border-style: none" />
            </a>
        </object>
        <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe> 
        <div class="uploadDetails" style="z-index:1">
            <div id="uploadedFileDetails"></div>
            <div id="name"></div>
            <asp:TextBox runat="server" id="inputedName" Enabled="false" style="width:312px; height:20px; margin-top:5px; margin-left:15px;" /><p></p>
            <div id="date"></div>
            <asp:TextBox runat="server" id="inputedDate" Enabled="false" style=" width:312px; height:20px; margin-top:15px; margin-left:15px;" /><p></p>
            <div id="versionNumber"></div>
            <asp:TextBox runat="server" id="inputedVersion" Enabled="false" style="width:312px; height:20px; margin-left:15px; margin-top:15px" />
            <div id="comments" style="float:right"></div>
            <asp:TextBox runat="server" style="float:right" Enabled="false" TextMode="multiline" id="inputedComments" Columns="40" Rows="10"/>
            <div></div>
            <asp:Button id="uploadButton" runat="server" Enabled="false" onclick="uploadButton_Click" />
        </div>
    </div>
    <div class="contentRight" style="z-index:2;float:right">
	    <div id="information"></div>
        <div id="currentVersion"></div>
        <asp:TextBox id="versionInput" runat="server" Enabled="false"></asp:TextBox>
        <div id="updatedByWho"></div>
        <asp:TextBox id="updatedByWhoInput" runat="server" Enabled="false"></asp:TextBox>
        <div id="dateUpdated"></div>
        <asp:TextBox id="dateUpdatedInput" runat="server" Enabled="false"></asp:TextBox>
	</div>
    </form>
  </body>
</html>
