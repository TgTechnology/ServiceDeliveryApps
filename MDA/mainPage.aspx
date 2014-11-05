<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="mainPage.aspx.cs" Inherits="MDA.mainPage" %>
<%@ Import Namespace="System.Security.Principal" %>
<html>
<head>
    <title>Main Page</title>
    <link rel="stylesheet" type="text/css" href="css/stylesheet.css">
</head>
  <body>
    <form id="Form1" method="post" runat="server">
      <div class="indexTopper">
			<div class="title"></div><p></p>
                <asp:Button ID="LogOut" class="logout" OnClick="LogOut_Click" runat="server" AutoPostBack="true" />
				<button type="button" class="profile" id="Button1" onclick="location.href='profilePage.html'"></button>
				<button type="button" class="updateButtonSelected" id="button0"></button>
			</div>
		<div class="contentLeft">
			<p><div id="customerSite">
                <asp:Label ID="lblName" Runat=server />
                <asp:Label ID="lblAuthType" Runat=server />
			   </div></p>
			<div id="hotelDropDown"></div>
			<div id="currentVersion"></div>
			<div id="updatedByWho"></div>
			<div id="dateUpdated"></div>
            <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="true" style="margin-left:15px; margin-right:25px; margin-top:20px;">
                <asp:ListItem Value="0">Select</asp:ListItem>
            </asp:DropDownList>
			<input type="text" id="versionInput"></input>
			<input type="text" id="updatedByWhoInput"></input>
			<input type="text" id="dateUpdatedInput"></input>
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
			<div id="information"></div>
		</div>
        </form>
  </body>
</html>
<script runat=server>
void Page_Load(object sender, EventArgs e)
{
  lblName.Text = "Hello " + Context.User.Identity.Name + ".";
  lblAuthType.Text = "You were authenticated using " + Context.User.Identity.AuthenticationType + ".";
}
</script>