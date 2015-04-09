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
using SevenZip;

namespace MDA
{
    public partial class mainPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (ddlCustomers.Items.Count == 0)
            {
                ddlCustomers_BuildList(ddlCustomers, e);
                //ddlRedirects.Items.Add(new ListItem("Select", "Selected"));
            }
            loginName.Text = "Welcome " + Context.User.Identity.Name + ".";
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomers.SelectedValue != "Select")
            {
                //Enable add fields
                uploadButton.Enabled = true;
                inputedName.Enabled = true;
                inputedDate.Enabled = true;
                inputedVersion.Enabled = true;
                inputedComments.Enabled = true;
                pcrinput.Enabled = true;
            }
            if (ddlCustomers.SelectedValue == "0")
            {
                //Enable add fields
                uploadButton.Enabled = false;
                inputedName.Enabled = false;
                inputedDate.Enabled = false;
                inputedVersion.Enabled = false;
                inputedComments.Enabled = false;
                pcrinput.Enabled = false;
            }

            string version = "";
            string filename = "";
            string username = "";
            string uploaddatetime = "";
            Customer CustomerInfo = new Customer();
            Dictionary<string,string> info = CustomerInfo.GetCustomer(ddlCustomers.SelectedValue);

            DeploymentInfo DeployInfo = new DeploymentInfo();
            Dictionary<string, string> deploy = DeployInfo.GetTransaction(ddlCustomers.SelectedValue);

            string ipaddress = info["MRCCIPPrimary"].ToString();
            string sitetype = info["SiteType"].ToString();
            string subscribercode = info["SubscriberCode"].ToString();
            subscribercode = subscribercode.ToString().Replace("CDP-", "");
            subscribercode = subscribercode.ToString().Replace("SG-", "");
            subscribercode = subscribercode.Trim();

            if (deploy.Count != 0)
            {
                version = deploy["Version"].ToString();
                filename = deploy["FileTransferred"].ToString();
                username = deploy["Username"].ToString();
                uploaddatetime = deploy["UploadDateTime"].ToString();
            }

            versionInput.Text = version;
            updatedByWhoInput.Text = username;
            dateUpdatedInput.Text = uploaddatetime;


            if (String.IsNullOrEmpty(version))
            {
                version = "1.0.0";
            }
            else
            {
                string[] numbers = version.Split('.');
                int patch = Convert.ToInt16(numbers[2]) + 1;
                version = numbers[0] + "." + numbers[1] + "." + patch.ToString();
            }

            if (String.IsNullOrEmpty(filename))
            {
                filename = subscribercode + ".zip";
            }

            inputedName.Text = filename;
            inputedDate.Text = DateTime.Now.ToString();
            inputedVersion.Text = version;

            //if (sitetype == "RDP")
            //{
            //    ddlRedirects.Enabled = true;
            //    DeploymentInfo HotelInfo = new DeploymentInfo();
            //    Dictionary<string, string> UrlRewrite = HotelInfo.GetUrlRewrite(ipaddress);
            //    ddlRedirects.Items.Clear();

            //    foreach (KeyValuePair<string, string> u in UrlRewrite)
            //    {
            //        ddlRedirects.Items.Add(new ListItem(u.Value.ToString(), u.Key.ToString()));
            //    }
            //}
            //if (sitetype == "CDP")
            //{
            //    ddlRedirects.Items.Clear();
            //    ddlRedirects.Items.Add(new ListItem("Select", "Selected"));
            //    ddlRedirects.Enabled = false;
            //}

            StatusLabel.Text = "Upload status:";

        }

        protected void ddlRedirects_SelectedIndexChanged(object sender, EventArgs e)
        {
           //code here
        }

        protected void ddlCustomers_BuildList(object sender, EventArgs e)
        {
            Customer Hotels = new Customer();
            Dictionary<string, string> HotelList = Hotels.GetCustomers();
            ddlCustomers.Items.Clear();

            ddlCustomers.Items.Add(new ListItem("Select", "Select"));
            foreach (KeyValuePair<string, string> o in HotelList)
            {
                Customer CustomerInfo = new Customer();
                Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(o.Key.ToString());

                foreach (KeyValuePair<string, string> c in objCustomer)
                {
                    if (c.Key.ToString() == "CustomerName")
                    {
                        ddlCustomers.Items.Add(new ListItem(o.Key.ToString() + "-" + c.Value.ToString(), o.Key.ToString()));
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
            //Get customer information to map paths to PF Server
            DeploymentInfo HotelInfo = new DeploymentInfo();
            Customer CustomerInfo = new Customer();
            Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(ddlCustomers.SelectedItem.Value.ToString());
            string ipaddress = objCustomer["MRCCIPPrimary"].ToString();
            string sitetype = objCustomer["SiteType"].ToString();

            //Create paths to PF Servers for menu deployment
            string site = System.Configuration.ConfigurationManager.AppSettings["Environment"].ToString();
            string subscribergroup = ddlCustomers.SelectedItem.Value.ToString().Replace("CDP-", "");
            subscribergroup = subscribergroup.ToString().Replace("SG-", "");
            string filename = subscribergroup + ".zip";
            string rootpath = "C:\\MRDOT\\Custom Applications\\MDA\\Upload\\" + filename;
            string folderPath = "C:\\MRDOT\\Custom Applications\\MDA\\Upload\\" + subscribergroup;

            string startPath = @"\\" + ipaddress + "\\c$\\inetpub\\wwwroot\\" + site + "\\" + subscribergroup;
            string backupPath = @"\\" + ipaddress + "\\c$\\inetpub\\wwwroot\\Backup\\" + subscribergroup + "-" + versionInput.Text;

            try
                {   
                    if (File.Exists(rootpath))
                    {
                        // Expand
                        System.IO.Compression.ZipFile.ExtractToDirectory(rootpath, "C:\\MRDOT\\Custom Applications\\MDA\\Upload\\");

                        if (sitetype != "")
                        {
                            //Move folder to backup folder for archive
                            HotelInfo.DeleteApp(ipaddress, @"/" + site + "/" + subscribergroup + @"/APPS/MainMenu");
                            HotelInfo.DeleteApp(ipaddress, @"/" + site + "/" + subscribergroup + @"/APPS/Checkout");
                            HotelInfo.DeleteApp(ipaddress, @"/" + site + "/" + subscribergroup + @"/APPS/Events");
                            HotelInfo.DeleteApp(ipaddress, @"/" + site + "/" + subscribergroup + @"/APPS/TGFlight");
                            HotelInfo.DeleteApp(ipaddress, @"/" + site + "/" + subscribergroup + @"/APPS/Chooser");

                            //Backup existing folder and delete
                            Utilities oUtil = new Utilities();
                            oUtil = new Utilities();
                            oUtil.MoveFolder(startPath, backupPath);
                            Directory.CreateDirectory(startPath);
                            DirectoryInfo srcPath = new DirectoryInfo("C:\\MRDOT\\Custom Applications\\MDA\\Upload\\" + subscribergroup);
                            DirectoryInfo newPath = new DirectoryInfo(@"\\" + ipaddress + "\\c$\\inetpub\\wwwroot\\" + site + "\\" + subscribergroup);
                            srcPath.Attributes = FileAttributes.Normal;
                            newPath.Attributes = FileAttributes.Normal;
                            oUtil.CopyFolder(srcPath, newPath);
                            File.Delete(rootpath);
                            Directory.Delete(folderPath, true);

                            //add application
                            string appPath = "/" + site + "/" + subscribergroup + "/Apps/";
                            string sitePath = @"C:\inetpub\wwwroot\" + site + @"\" + subscribergroup + @"\Apps\";

                            HotelInfo.AddApp(ipaddress, appPath + "MainMenu", sitePath + "MainMenu");
                            HotelInfo.AddApp(ipaddress, appPath + "Checkout", sitePath + "Checkout");
                            HotelInfo.AddApp(ipaddress, appPath + "Events", sitePath + "Events");
                            HotelInfo.AddApp(ipaddress, appPath + "TGFlight", sitePath + "TGFlight");
                            HotelInfo.AddApp(ipaddress, appPath + "Chooser", sitePath + "Chooser");

                            string status = "";
                            status = oUtil.StopService(ipaddress, "W3SVC");
                            status = oUtil.StartService(ipaddress, "W3SVC");

                        }
                        using (SqlConnection openCon = new SqlConnection(@"Data Source=TGNDP1SCOMRS001\OPSMGR;Initial Catalog=MDA;Integrated Security=True"))
                        {
                            string saveStaff = "INSERT into TransactionLog (FileTransferred,Version,UploadDateTime,Username,PCRNumber,Comment, SubscriberCode) VALUES (@FileTransferred,@Version,@UploadDateTime,@Username,@PCRNumber,@inputedComments,@SubscriberCode)";

                            using (SqlCommand querySaveLog = new SqlCommand(saveStaff))
                            {
                                querySaveLog.Connection = openCon;
                                querySaveLog.Parameters.Add("@FileTransferred", SqlDbType.VarChar, 50).Value = filename;
                                string file = System.IO.Path.GetFileNameWithoutExtension(filename);
                                querySaveLog.Parameters.Add("@Version", SqlDbType.VarChar, 50).Value = inputedVersion.Text;
                                querySaveLog.Parameters.Add("@UploadDateTime", SqlDbType.DateTime, 50).Value = DateTime.Now;
                                querySaveLog.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = Context.User.Identity.Name;
                                querySaveLog.Parameters.Add("@PCRNumber", SqlDbType.VarChar, 50).Value = pcrinput.Text;
                                querySaveLog.Parameters.Add("@inputedComments", SqlDbType.VarChar, 50).Value = inputedComments.Text;
                                querySaveLog.Parameters.Add("@SubscriberCode", SqlDbType.VarChar, 50).Value = ddlCustomers.SelectedItem.Value.ToString();

                                openCon.Open();
                                querySaveLog.ExecuteNonQuery();
                                openCon.Close();
                            }
                            StatusLabel.Text = "Upload status: File uploaded!";
                            inputedName.Text = "";
                            inputedDate.Text = "";
                            inputedVersion.Text = "";
                            inputedComments.Text = "";
                            versionInput.Text = "";
                            updatedByWhoInput.Text = "";
                            dateUpdatedInput.Text = "";
                            pcrinput.Text = "";
                        }
                    }
                }
            catch (Exception ex)
                {
                    //Clean-up files and directories
                    if(File.Exists(rootpath)){File.Delete(rootpath);}
                    if (Directory.Exists(folderPath)) { Directory.Delete(folderPath, true); }
                    if (Directory.Exists(backupPath)) { Directory.Delete(backupPath, true); }
                    StatusLabel.Text = "The following error occured: " + ex.Message;
                }  
        }  
    }
}