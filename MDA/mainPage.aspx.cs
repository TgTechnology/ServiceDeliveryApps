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
            if (ddlCustomers.SelectedValue == "Select")
            {
                //Enable add fields
                uploadButton.Enabled = false;
                inputedName.Enabled = false;
                inputedDate.Enabled = false;
                inputedVersion.Enabled = false;
                inputedComments.Enabled = false;
                pcrinput.Enabled = false;
            }

            if (ddlCustomers.SelectedValue != "Select")
            {
                //Enable add fields
                uploadButton.Enabled = true;
                inputedName.Enabled = true;
                inputedDate.Enabled = true;
                inputedVersion.Enabled = true;
                inputedComments.Enabled = true;
                pcrinput.Enabled = true;
            
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

            }
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

        protected void uploadButton_Click(object sender, EventArgs e)
        {
            //Get customer information to map paths to PF Server
            DeploymentInfo HotelInfo = new DeploymentInfo();
            Customer CustomerInfo = new Customer();
            Dictionary<string, string> objCustomer = CustomerInfo.GetCustomer(ddlCustomers.SelectedItem.Value.ToString());
            string ipaddress = objCustomer["MRCCIPPrimary"].ToString();
            string sitetype = objCustomer["SiteType"].ToString();
            string EGRESSIPPrimary = objCustomer["EGRESSIPPrimary"].ToString();
            string EGRESSIPSecondary = objCustomer["EGRESSIPSecondary"].ToString();
            string site = System.Configuration.ConfigurationManager.AppSettings["Environment"].ToString();

            //Create paths to PF Servers for menu deployment
            Utilities oUtil = new Utilities();
            oUtil = new Utilities();
            string subscribergroup = oUtil.GetSubscriberNumber(ddlCustomers.SelectedItem.Value.ToString());
            string filename = subscribergroup + ".zip";
            string MenuZipPath = "C:\\MRDOT\\Custom Applications\\MDA\\Upload\\" + filename;
            string MenuFolderPath = "C:\\MRDOT\\Custom Applications\\MDA\\Upload\\" + subscribergroup;

            DirectoryInfo destPath = new DirectoryInfo(@"\\" + ipaddress + "\\c$\\inetpub\\wwwroot\\" + site + "\\" + subscribergroup);
            DirectoryInfo backupPath = new DirectoryInfo(@"\\" + ipaddress + "\\c$\\inetpub\\wwwroot\\Backup\\" + subscribergroup + "-" + versionInput.Text);

            try
            {
                if (File.Exists(MenuZipPath))
                {
                    //Expand
                    System.IO.Compression.ZipFile.ExtractToDirectory(MenuZipPath, "C:\\MRDOT\\Custom Applications\\MDA\\Upload\\");

                    if (sitetype != "")
                    {
                        //Backup existing folder and delete existing folders
                        if (sitetype == "RDP")
                        {
                            oUtil.StopNLB(EGRESSIPPrimary);
                        }
                        oUtil.CopyFolder(destPath, backupPath);
                        Directory.Delete(destPath.ToString() + @"\APPS", true);
                        Directory.Delete(destPath.ToString() + @"\IMAGES", true);

                        MessageBox.Show("Delete Primary Complete");

                        //Before moving folders set attributes to not read only
                        DirectoryInfo appSrcPath = new DirectoryInfo(MenuFolderPath + @"\APPS");
                        DirectoryInfo imgSrcPath = new DirectoryInfo(MenuFolderPath + @"\IMAGES");
                        DirectoryInfo appDestPath = new DirectoryInfo(destPath.ToString() + @"\APPS");
                        DirectoryInfo imgDestPath = new DirectoryInfo(destPath.ToString() + @"\IMAGES");
                        Directory.CreateDirectory(appDestPath.ToString());
                        Directory.CreateDirectory(imgDestPath.ToString());

                        appSrcPath.Attributes = FileAttributes.Normal;
                        imgSrcPath.Attributes = FileAttributes.Normal;
                        appDestPath.Attributes = FileAttributes.Normal;
                        imgDestPath.Attributes = FileAttributes.Normal;
                        oUtil.CopyFolder(appSrcPath, appDestPath);
                        oUtil.CopyFolder(imgSrcPath, imgDestPath);

                        MessageBox.Show("Copy Primary Complete");

                        //add application
                        string appRootPath = "/" + site + "/" + subscribergroup + "/Apps/";
                        string sitePhyscialPath = @"C:\inetpub\wwwroot\" + site + @"\" + subscribergroup + @"\Apps\";
                        
                        string status = "";
                        status = oUtil.StopService(ipaddress, "W3SVC");
                        status = oUtil.StartService(ipaddress, "W3SVC");
                        if (sitetype == "RDP")
                        {
                            oUtil.StartNLB(EGRESSIPPrimary);
                        }

                        if (sitetype == "RDP")
                        {
                            oUtil.StopNLB(EGRESSIPSecondary);
                        }

                        destPath = new DirectoryInfo(@"\\" + EGRESSIPSecondary + "\\c$\\inetpub\\wwwroot\\" + site + "\\" + subscribergroup);
                        Directory.Delete(destPath.ToString() + @"\APPS", true);
                        Directory.Delete(destPath.ToString() + @"\IMAGES", true);

                        appDestPath = new DirectoryInfo(destPath.ToString() + @"\APPS");
                        imgDestPath = new DirectoryInfo(destPath.ToString() + @"\IMAGES");
                        Directory.CreateDirectory(appDestPath.ToString());
                        Directory.CreateDirectory(imgDestPath.ToString());
                        appDestPath.Attributes = FileAttributes.Normal;
                        imgDestPath.Attributes = FileAttributes.Normal;
                        oUtil.CopyFolder(appSrcPath, appDestPath);
                        oUtil.CopyFolder(imgSrcPath, imgDestPath);

                        if (sitetype == "RDP")
                        {
                            oUtil.StartNLB(EGRESSIPSecondary);
                        }

                        //Clean up source paths on MDA server
                        File.Delete(MenuZipPath);
                        Directory.Delete(MenuFolderPath, true);
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

                        //StatusLabel.Text = "Upload status: File uploaded!";
                        inputedName.Text = "";
                        inputedDate.Text = "";
                        inputedVersion.Text = "";
                        inputedComments.Text = "";
                        versionInput.Text = "";
                        updatedByWhoInput.Text = "";
                        dateUpdatedInput.Text = "";
                        pcrinput.Text = "";
                    }
                    MessageBox.Show("Deployment Complete!");
                }
            }
            catch (Exception ex)
            {
                //Clean-up files and directories
                if (File.Exists(MenuZipPath)) { File.Delete(MenuZipPath); }
                if (Directory.Exists(MenuFolderPath)) { Directory.Delete(MenuFolderPath, true); }
                if (Directory.Exists(backupPath.ToString())) { Directory.Delete(backupPath.ToString(), true); }
            }  
        }
    }
}