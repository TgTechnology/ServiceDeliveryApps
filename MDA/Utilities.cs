using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Diagnostics;
using System.Diagnostics.Eventing;

namespace MDA
{
    public class Utilities
    {
        public string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }

        public void MoveFolder(string srcPath, string destPath)
        {
            string sourceDirectory = srcPath;
            string destinationDirectory = destPath;

            try
            {
                Directory.Move(sourceDirectory, destinationDirectory);
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
    }
}