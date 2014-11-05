<%@ Page language="c#" AutoEventWireup="true" %>
<%@ Import Namespace="FormsAuth" %>
<html>
    <head>
		<title>MDA LogIn</title>
		<link rel="stylesheet" type="text/css" href="css/stylesheet.css">
	</head>
    <body>
        <div class="indexTopper" style="margin-top:-10px;"></div>
        <div id="loginContainer">
            <form id="Login" method="post" runat="server">
                <div id="usernameLogin"></div><br />
                <asp:TextBox ID=txtUsername Runat=server style="margin-left:20px; margin-top:-10px;"></asp:TextBox>
                <div id="passwordLogin"></div><br />
                <asp:TextBox ID="txtPassword" Runat=server TextMode=Password style="margin-left:20px; margin-top:-10px;"></asp:TextBox>
                <asp:Button ID="loginButton" Runat=server OnClick="Login_Click"></asp:Button><br />
                <asp:Label ID="errorLabel" Runat=server ForeColor=#ff3300></asp:Label><br />
                <asp:CheckBox ID=chkPersist Runat=server Text="Persist Cookie" />
            </form>
        </div>
    </body>
</html>
<script runat=server>
void Login_Click(object sender, EventArgs e)
{

    string adPath = "LDAP://TGMGMTDC04.MGMT.IPTVTG.COM/DC=mgmt,DC=iptvtg,DC=com"; //Path to your LDAP directory server
    //string adPath = "LDAP://TGNDP1MRDC001/DC=BR,DC=iptvtg,DC=com"; //Path to your LDAP directory server
    LdapAuthentication adAuth = new LdapAuthentication(adPath);
  try
  {
    if(true == adAuth.IsAuthenticated("mgmt", txtUsername.Text, txtPassword.Text))
    {
        string groups = "empty"; //adAuth.GetGroups();

      //Create the ticket, and add the groups.
      bool isCookiePersistent = chkPersist.Checked;
      FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, 
                txtUsername.Text,DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups);

      //Encrypt the ticket.
      string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

      //Create a cookie, and then add the encrypted ticket to the cookie as data.
      HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

      if(true == isCookiePersistent)
      authCookie.Expires = authTicket.Expiration;

      //Add the cookie to the outgoing cookies collection.
      Response.Cookies.Add(authCookie);

      //You can redirect now.
      Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, false));
    }
    else
    {
      errorLabel.Text = "Authentication did not succeed. Check user name and password.";
    }
  }
  catch(Exception ex)
  {
    errorLabel.Text = "Error authenticating. " + ex.Message;
  }
}
</script>