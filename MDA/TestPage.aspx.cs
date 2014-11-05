using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using Microsoft.Web.Administration;


namespace MDA
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            DeploymentInfo HotelInfo = new DeploymentInfo();
            ServerManager SiteInfo = HotelInfo.GetSiteInfo("10.200.1.72");

            foreach (var site in SiteInfo.Sites)
            {
                Response.Write("Web-Site : " + site.Name);
                foreach (Application app in site.Applications)
                {
                    if (app.VirtualDirectories.Count > 0)
                    {
                        foreach (VirtualDirectory vdir in app.VirtualDirectories)
                        {
                            Response.Write("  Virtual Directory: " + vdir.PhysicalPath + "<br>");
                        }
                    }
                }
            }
           
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}