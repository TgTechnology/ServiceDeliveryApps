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
        public int Id
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int DeployedBy
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int DeployedDate
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int DeployedMenu
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int DeploymentType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public ServerManager GetSiteInfo(string SiteIP)
        {
            var manager = ServerManager.OpenRemote(SiteIP);

            Configuration config = manager.GetApplicationHostConfiguration();
            return manager;
            throw new System.NotImplementedException();
        }
    }
}
