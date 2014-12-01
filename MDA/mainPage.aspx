<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="mainPage.aspx.cs" Inherits="MDA.mainPage" EnableViewState="True" %>
<%@ Import Namespace="System.Security.Principal" %>
<html>
<head>
    <title>Main Page</title>
    <link rel="stylesheet" type="text/css" href="css/stylesheet.css">
</head>
  <body>
    <form id="Form1" method="post" runat="server">
      <div class="indexTopper">
			<div class="title"></div>
                <asp:Label ID="loginName" Text="" runat="server"></asp:Label>
                <asp:Button ID="LogOut" class="logout" OnClick="LogOut_Click" runat="server" AutoPostBack="true" />
				<button type="button" class="profile" id="Button1" onclick="location.href='profilePage.html'"></button>
				<button type="button" class="updateButtonSelected" id="button0"></button>
			</div>
		<div class="contentLeft">
			<div id="customerSite">
                <asp:Label ID="lblName" Runat=server />
                <asp:Label ID="lblAuthType" Runat=server />
			</div>
		    
            <asp:Table Runat=server BorderWidth="0" CellSpacing="5">
                <asp:TableRow>
                    <asp:TableCell>
                       <div id="hotelDropDown"></div>
                    </asp:TableCell>
                    <asp:TableCell>
                        <div id="appsDropDown"></div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                          Action  
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="true" EnableViewState="true" onselectedindexchanged="ddlCustomers_SelectedIndexChanged">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlApps" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Button ID="delete" OnClick="DeleteSite_Click" runat="server" AutoPostBack="true" Text="Delete" />
                        &nbsp&nbsp
                        <asp:Button ID="add" OnClick="AddSite_Click" runat="server" AutoPostBack="true" Text="Add" />
                        &nbsp&nbsp
                        <asp:Button ID="edit" OnClick="EditSite_Click" runat="server" AutoPostBack="true" Text="Edit" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <p><div id="selectFile"></div></p>
			<input type="file" name="browse" style="margin-left:15px;">
			<button type="button" id="uploadButton"></button><p></p>
				<div class="uploadDetails">
					<div id="uploadedFileDetails"></div>
					<div id="name"></div><p></p>
						<input id="inputedName" style="width:312px; height:20px; margin-top:-10px; margin-left:15px;"></input><p></p>
					<div id="date"></div><p></p>
						<input type="text" id="inputedDate" style=" width:312px; height:20px; margin-top:-10:px; margin-left:15px;"></input><p></p>
					<div id="versionNumber"></div><p></p>
						<input type="text" id="inputedVersion" style="width:312px; height:20; margin-left:15px;"></input>
					<div id="comments" style="float:right; margin-top:-159px; margin-right:92px;"></div><p></p>
						<textarea id="inputedComments" cols="40" rows="10" style="float:right; margin-top:-167px; margin-right:5px;"></textarea>
				</div>
		</div>
		<div class="contentRight">
			<div id="information">
                <div id="currentVersion"></div>
                <asp:TextBox id="versionInput" runat="server"></asp:TextBox>
                <div id="updatedByWho"></div>
                <asp:TextBox id="updatedByWhoInput" runat="server"></asp:TextBox>
                <div id="dateUpdated"></div>
                <asp:TextBox id="dateUpdatedInput" runat="server"></asp:TextBox>
			</div>
		</div>
        </form>
  </body>
</html>
