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
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;

namespace MDA
{
    public partial class mainPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (ddlCustomers.Items.Count == 0)
            {
                ddlCustomers_BuildList(ddlCustomers, e);
                //ddlApps.Items.Add(new ListItem("Select", "0"));
            }
            loginName.Text = "Welcome " + Context.User.Identity.Name + ".";
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlApps_BuildList(ddlApps, e, ddlCustomers.SelectedValue);
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
            subnetinput.Enabled = true;
            pcrinput.Enabled = true;
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
            subnetinput.Enabled = false;
            pcrinput.Enabled = false;
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
                    string subscribergroup = ddlCustomers.SelectedItem.Value.ToString().Replace("CDP-", "");
                    //MessageBox.Show("Uploading....." + subscribergroup);
                    
                    string filename = Path.GetFileName(browse.FileName);
                    browse.SaveAs(Server.MapPath("~/") + filename);


                    Customer CustomerInfo = new Customer();
                    Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(ddlCustomers.SelectedItem.Value.ToString());

                    foreach (KeyValuePair<string, string> c in objCustomer)
                    {
                        // Copy from the current directory, include subdirectories.
                        File.Copy(@"C:\MRDOT\Custom Applications\MDA\" + filename, @"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\" + filename);
                        File.Delete(@"C:\MRDOT\Custom Applications\MDA\" + filename);

                        string zipPath = @"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\" + filename;
                        string extractPath = @"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\RDPSites\\";

                        // Expand and then delete
                        System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
                        File.Delete(@"\\" + c.Value.ToString() + "\\c$\\inetpub\\wwwroot\\" + filename);

                        //add application
                        DeploymentInfo HotelInfo = new DeploymentInfo();
                        HotelInfo.AddApp(c.Value.ToString(), @"C:\inetpub\wwwroot\RDPSites\" + subscribergroup + @"\Apps\MainMenu", @"/RDPSites/" + subscribergroup + @"/APPS/MAINMENU");
                        HotelInfo.AddApp(c.Value.ToString(), @"C:\inetpub\wwwroot\RDPSites\" + subscribergroup + @"\Apps\Checkout", @"/RDPSites/" + subscribergroup + @"/APPS/CHECKOUT");
                        HotelInfo.AddApp(c.Value.ToString(), @"C:\inetpub\wwwroot\RDPSites\" + subscribergroup + @"\Apps\Event", @"/RDPSites/" + subscribergroup + @"/APPS/EVENT");
                        HotelInfo.AddApp(c.Value.ToString(), @"C:\inetpub\wwwroot\RDPSites\" + subscribergroup + @"\Apps\TGFlight", @"/RDPSites/" + subscribergroup + @"/APPS/TGFlight");

                        using(SqlConnection openCon=new SqlConnection(@"Data Source=TGNDP1SCOMRS001\OPSMGR;Initial Catalog=MDA;Integrated Security=True"))
                        {
                            string saveStaff = "INSERT into TransactionLog (FileTransferred,Version,UploadDateTime,Username,PCRNumber) VALUES (@FileTransferred,@Version,@UploadDateTime,@Username,@PCRNumber)";

                          using(SqlCommand querySaveLog = new SqlCommand(saveStaff))
                           {
                                querySaveLog.Connection = openCon;
                                querySaveLog.Parameters.Add("@FileTransferred", SqlDbType.VarChar, 30).Value = filename;
                                string file = System.IO.Path.GetFileNameWithoutExtension(filename);
                                querySaveLog.Parameters.Add("@Version", SqlDbType.VarChar, 30).Value = file;
                                querySaveLog.Parameters.Add("@UploadDateTime", SqlDbType.DateTime, 30).Value = DateTime.Now;
                                querySaveLog.Parameters.Add("@Username", SqlDbType.VarChar, 30).Value = Context.User.Identity.Name;
                                querySaveLog.Parameters.Add("@PCRNumber", SqlDbType.VarChar, 30).Value = pcrinput.Text;


                                openCon.Open();
                                querySaveLog.ExecuteNonQuery();
                                openCon.Close();
                           }
                         }


                        StatusLabel.Text = "Upload status: File uploaded!";


                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "The following error occured: " + ex.Message;
                }
            }
        }
   
    }
}