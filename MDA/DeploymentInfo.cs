using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Management;
using Microsoft.Web.Administration;

namespace MDA
{
    public class DeploymentInfo
    {
        public ServerManager GetSiteInfo(string SiteIP)
        {
            var manager = ServerManager.OpenRemote(SiteIP);

            Configuration config = manager.GetApplicationHostConfiguration();
            return manager;
        }

        public void DeleteApp(string SiteIP, string sitePath)
        {
            ServerManager manager = ServerManager.OpenRemote(SiteIP);
            Application app = manager.Sites[0].Applications[0];
            VirtualDirectory virdir = app.VirtualDirectories.CreateElement();
            virdir.Path = @"/cc";
            virdir.PhysicalPath = @"C:\inetpub\wwwroot\cc";
            app.VirtualDirectories.Remove(virdir);
            manager.CommitChanges();

        }
    }
}
