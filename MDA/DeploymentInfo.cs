using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Management;

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
    
        public void GetSiteInfo()
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();

            connectionOptions.Username = "BR\\Administrator";
            connectionOptions.Password = "Password1!";

            ManagementScope managementScope = new ManagementScope(@"\\10.10.21.10\root\microsoftiisv2", connectionOptions);

            managementScope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(managementScope, query);

            throw new System.NotImplementedException();
        }
    }
}
