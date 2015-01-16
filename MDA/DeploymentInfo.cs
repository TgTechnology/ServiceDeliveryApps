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
            Site site = manager.Sites[0];
            Microsoft.Web.Administration.Application application = site.Applications["/" + sitePath];
            site.Applications.Remove(application);
            manager.CommitChanges();

        }
    }
}
