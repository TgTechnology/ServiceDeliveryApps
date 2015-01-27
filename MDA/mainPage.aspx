﻿<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="mainPage.aspx.cs" Inherits="MDA.mainPage" EnableViewState="True" %>
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
				<button type="button" class="profile" id="Button1" onclick="location.href='profilePage.html'"></button>
				<button type="button" class="updateButtonSelected" id="button0"></button>
			</div>
		<div class="contentLeft">
			<div id="customerSite">
                <asp:Label ID="lblName" Runat=server />
                <asp:Label ID="lblAuthType" Runat=server />
                <div id="pcr"></div>
                <asp:TextBox runat="server" id="pcrinput" Enabled="false" />
			</div>
            <asp:Table ID="Table1" Runat=server BorderWidth="0" CellSpacing="5">
                <asp:TableRow>
                    <asp:TableCell>
                       <div id="hotelDropDown"></div>
                    </asp:TableCell>
                    <asp:TableCell>
                        <!--div id="appsDropDown"></!--div-->
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                          Action  
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                          <div id="subnet"></div>  
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="true" EnableViewState="true" onselectedindexchanged="ddlCustomers_SelectedIndexChanged">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell>
                        &nbsp;<!--<asp:DropDownList ID="ddlApps" runat="server" AutoPostBack="true">
                        </asp:DropDownList>-->
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Button ID="delete" OnClick="DeleteSite_Click" runat="server" Text="Delete" />
                        &nbsp&nbsp
                        <asp:Button ID="add" OnClick="AddSite_Click" runat="server" Text="Add" />
                        <asp:Button ID="cancel" OnClick="CancelSite_Click" runat="server" Text="Cancel" Visible="false"/>
                        &nbsp&nbsp
                        <asp:Button ID="edit" OnClick="EditSite_Click" runat="server" Text="Edit" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:TextBox runat="server" id="subnetinput" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="StatusLabel" runat="server" text="Upload Status: " Font-Name="Arial" Font-Size="10"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        &nbsp;
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        &nbsp;
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        &nbsp;
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p><div id="selectFile"></div></p>
            <asp:FileUpload ID="browse" style="margin-left:15px;" runat="server" Enabled="false" />  
			<asp:Button id="uploadButton" runat="server" Enabled="false" onclick="uploadButton_Click" /><p></p>
				<div class="uploadDetails">
					<div id="uploadedFileDetails"></div>
					<div id="name"></div><p></p>
						<asp:TextBox runat="server" id="inputedName" Enabled="false" style="width:312px; height:20px; margin-top:-10px; margin-left:15px;" /><p></p>
					<div id="date"></div><p></p>
						<asp:TextBox runat="server" id="inputedDate" Enabled="false" style=" width:312px; height:20px; margin-top:-10:px; margin-left:15px;" /><p></p>
					<div id="versionNumber"></div><p></p>
						<asp:TextBox runat="server" id="inputedVersion" Enabled="false" style="width:312px; height:20; margin-left:15px;" />
					<div id="comments" style="float:right; margin-top:-159px; margin-right:92px;"></div><p></p>
						<asp:TextBox runat="server" Enabled="false" TextMode="multiline" id="inputedComments" Columns="40" Rows="10" style="float:right; margin-top:-167px; margin-right:5px;" />
				</div>
		</div>
		<div class="contentRight">
			<div id="information">
                <div id="currentVersion"></div>
                <asp:TextBox id="versionInput" runat="server" Enabled="false"></asp:TextBox>
                <div id="updatedByWho"></div>
                <asp:TextBox id="updatedByWhoInput" runat="server" Enabled="false"></asp:TextBox>
                <div id="dateUpdated"></div>
                <asp:TextBox id="dateUpdatedInput" runat="server" Enabled="false"></asp:TextBox>
			</div>
		</div>
        </form>
  </body>
</html>
