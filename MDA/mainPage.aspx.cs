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
using System.IO;
using System.IO.Compression;

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
            Dictionary<string, string> HotelList = Hotels.GetCustomers();
            ddlCustomers.Items.Clear();

            ddlCustomers.Items.Add(new ListItem("Select", "0"));
            foreach (KeyValuePair<string, string> o in HotelList)
            {
                Customer CustomerInfo = new Customer();
                Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(o.Key.ToString());

                foreach (KeyValuePair<string, string> c in objCustomer)
                {
                    ddlCustomers.Items.Add(new ListItem(o.Key.ToString() + "-" + c.Key.ToString(), o.Key.ToString()));
                }
            }
        }

        protected void ddlApps_BuildList(object sender, EventArgs e, string SubscriberId)
        {
            ServerManager SiteInfo = null;
            ddlApps.Items.Clear();
            ddlApps.Items.Add(new ListItem("Select", "0"));
            if (!String.IsNullOrEmpty(SubscriberId))
            {
                DeploymentInfo HotelInfo = new DeploymentInfo();
                Customer CustomerInfo = new Customer();
                Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(SubscriberId);

                foreach (KeyValuePair<string, string> o in objCustomer)
                {
                    SiteInfo = HotelInfo.GetSiteInfo(o.Value);
                }

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
            //Enable add fields
            browse.Enabled = true;
            uploadButton.Enabled = true;
            inputedName.Enabled = true;
            inputedDate.Enabled = true;
            inputedVersion.Enabled = true;
            inputedComments.Enabled = true;
            add.Visible = false;
            cancel.Visible = true;
        }

        protected void CancelSite_Click(Object sender, EventArgs e)
        {
            //Enable add fields
            browse.Enabled = false;
            uploadButton.Enabled = false;
            inputedName.Enabled = false;
            inputedDate.Enabled = false;
            inputedVersion.Enabled = false;
            inputedComments.Enabled = false;
            add.Visible = true;
            cancel.Visible = false;
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

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            if (browse.HasFile)
            {
                try
                {
                    MessageBox.Show("Uploading.....");
                    MessageBox.Show(ddlCustomers.SelectedItem.Value.ToString());
                    string filename = Path.GetFileName(browse.FileName);
                    browse.SaveAs(Server.MapPath("~/") + filename);
                    StatusLabel.Text = "Upload status: File uploaded!";

                    Customer CustomerInfo = new Customer();
                    Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(ddlCustomers.SelectedItem.Value.ToString());

                    foreach (KeyValuePair<string, string> c in objCustomer)
                    {
                        // Copy from the current directory, include subdirectories.
                        File.Copy(@"C:\MRDOT\Custom Applications\MDA\" + filename, @"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\" + filename);

                        string zipPath = @"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\" + filename;
                        string extractPath = @"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\";

                        System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "The following error occured: " + ex.Message;
                }
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        
    }
}