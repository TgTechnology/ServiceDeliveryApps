using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Web.Security;
using System.Web.SessionState;
using System.Security.Principal;
using Microsoft.Web.Administration;
using System.Management;
using Microsoft.Web.Administration;

namespace MDA
{
    public partial class mainPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (ddlCustomers.Items.Count == 0)
            {
                ddlCustomers_BuildList(ddlCustomers, e);
                ddlApps.Items.Add(new ListItem("Select", "0"));
            }
            loginName.Text = "Welcome " + Context.User.Identity.Name + ".";
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlApps_BuildList(ddlApps, e, ddlCustomers.SelectedValue);
        }

        protected void ddlCustomers_BuildList(object sender, EventArgs e)
        {
            Customer Hotels = new Customer();
            Array HotelList = Hotels.GetCustomers();
            ddlCustomers.Items.Clear();
            int GroupID = 1;

            ddlCustomers.Items.Add(new ListItem("Select", "0"));
            foreach (PrincipalManagement.Group1 o in HotelList)
            {
                if (!o.ExternalID.ToString().Contains("Everyone"))
                {
                    string HotelName = GetSubstringByString("(", ")", o.Type.ToString());
                    ddlCustomers.Items.Insert(GroupID, new ListItem(o.ExternalID.ToString() + "-" + HotelName, o.ExternalID.ToString()));
                    GroupID = GroupID + 1;
                }
            }
        }

        protected void ddlApps_BuildList(object sender, EventArgs e, string SubscriberId)
        {
            ddlApps.Items.Clear();
            ddlApps.Items.Add(new ListItem("Select", "0"));
            if (!String.IsNullOrEmpty(SubscriberId))
            {
                DeploymentInfo HotelInfo = new DeploymentInfo();
                Customer CustomerInfo = new Customer();
                Object ipAddress = CustomerInfo.GetCustomer(SubscriberId);
                ServerManager SiteInfo = HotelInfo.GetSiteInfo(ipAddress.ToString());
                
                int AppID = 1;

                foreach (var site in SiteInfo.Sites)
                {
                    foreach (Microsoft.Web.Administration.Application app in site.Applications)
                    {
                        if (app.VirtualDirectories.Count > 0)
                        {
                            foreach (VirtualDirectory vdir in app.VirtualDirectories)
                            {
                                if (!vdir.PhysicalPath.Contains("SystemDrive"))
                                {
                                    ddlApps.Items.Insert(AppID, new ListItem(vdir.PhysicalPath.Trim().ToUpper().Replace(@"C:\INETPUB\WWWROOT\", "")));
                                    AppID = AppID + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void LogOut_Click(Object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("MainPage.aspx");
        }

        protected void DeleteSite_Click(Object sender, EventArgs e)
        {
            Customer CustomerInfo = new Customer();
            Object ipAddress = CustomerInfo.GetCustomer(ddlCustomers.SelectedValue);

            DeploymentInfo HotelInfo = new DeploymentInfo();
            ServerManager SiteInfo = HotelInfo.GetSiteInfo(ipAddress.ToString());

            MessageBox.Show("Delete: " + ddlApps.SelectedValue);
            HotelInfo.DeleteApp(ipAddress.ToString(), ddlApps.SelectedValue);
            ddlApps_BuildList(sender, e, ddlCustomers.SelectedValue);
            MessageBox.Show("Delete Complete");
        }

        protected void AddSite_Click(Object sender, EventArgs e)
        {
            Customer CustomerInfo = new Customer();
            Object ipAddress = CustomerInfo.GetCustomer(ddlCustomers.SelectedValue);

            DeploymentInfo HotelInfo = new DeploymentInfo();
            ServerManager SiteInfo = HotelInfo.GetSiteInfo(ipAddress.ToString());

            Site site = SiteInfo.Sites[0];
            Microsoft.Web.Administration.Application application = site.Applications["/CC"];
            site.Applications.Remove(application);
            SiteInfo.CommitChanges();
        }

        protected void EditSite_Click(Object sender, EventArgs e)
        {
            Customer CustomerInfo = new Customer();
            Object ipAddress = CustomerInfo.GetCustomer(ddlCustomers.SelectedValue);

            DeploymentInfo HotelInfo = new DeploymentInfo();
            ServerManager SiteInfo = HotelInfo.GetSiteInfo(ipAddress.ToString());

            Site site = SiteInfo.Sites[0];
            Microsoft.Web.Administration.Application application = site.Applications["/CC"];
            site.Applications.Remove(application);
            SiteInfo.CommitChanges();
        }

        public string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }
    }
}