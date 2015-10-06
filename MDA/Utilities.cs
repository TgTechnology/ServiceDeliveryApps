using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Management;

namespace MDA
{
    public class Utilities
    {
        public string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }

        public string GetSubscriberNumber(string a)
        {
            a = a.Replace("CDP-", "");
            a = a.Replace("SG-", "");
            return a;
        }

        public void MoveFolder(string srcPath, string destPath)
        {

            try
            {
                Directory.Move(srcPath, destPath);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("The following error occured: " + ex.Message);
            }
        }

        public void CopyFolder(DirectoryInfo sourceFolder, DirectoryInfo destFolder)
        {
            foreach (DirectoryInfo dir in sourceFolder.GetDirectories())
                CopyFolder(dir, destFolder.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in sourceFolder.GetFiles())
                file.CopyTo(Path.Combine(destFolder.FullName, file.Name));
        }

        public string StartService(string compname, string service)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = service;
            sc.MachineName = compname;
            string result = sc.Status.ToString();

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    result = sc.Status.ToString();

                }
                catch (InvalidOperationException)
                {
                    // do something
                }
            }
            return result;
        }

        public string StopService(string compname, string service)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = service;
            sc.MachineName = compname;
            string result = sc.Status.ToString();

            if (sc.Status == ServiceControllerStatus.Running)
            {
                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    result = sc.Status.ToString();

                }
                catch (InvalidOperationException)
                {
                    // do something
                }
            }
            return result;
        }

        public int StopNLB(string hostAddress)
        {

            ManagementScope NLBScope = new ManagementScope("\\\\" + hostAddress + "\\root\\MicrosoftNLB");
            ManagementClass NLBClass = new ManagementClass(NLBScope, new ManagementPath("MicrosoftNLB_Node"), null);

            try
            {
                NLBClass.Get();
                object status = null;
                hostAddress = hostAddress + ":1";
                foreach (ManagementObject node in NLBClass.GetInstances())
                {
                    string compName = node.GetPropertyValue("Name").ToString();

                    if (compName == hostAddress)
                    {
                        node.Get();
                        node.InvokeMethod("Stop", null);
                        status = node.GetPropertyValue("StatusCode");
                    }
                }
                
                return Convert.ToInt32(status);
            }
            catch (InvalidOperationException)
            {
                // do something
                return 0;
            }
            
        }

        public int StartNLB(string hostAddress)
        {

            ManagementScope NLBScope = new ManagementScope("\\\\" + hostAddress + "\\root\\MicrosoftNLB");
            ManagementClass NLBClass = new ManagementClass(NLBScope, new ManagementPath("MicrosoftNLB_Node"), null);

            try
            {
                NLBClass.Get();
                object status = null;
                hostAddress = hostAddress + ":1";

                foreach (ManagementObject node in NLBClass.GetInstances())
                {
                    string compName = node.GetPropertyValue("Name").ToString();

                    if (compName == hostAddress)
                    {
                        node.Get();
                        node.InvokeMethod("Start", null);
                        status = node.GetPropertyValue("StatusCode");
                    }
                }

                return Convert.ToInt32(status);
            }
            catch (InvalidOperationException)
            {
                // do something
                return 0;
            }

        }

    }
}