using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Management;
using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Web.Configuration;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;

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

        public Dictionary<string, string> GetTransaction(string SubscriberCode)
        {
            SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=TGNDP1SCOMRS001\OPSMGR;Initial Catalog=MDA;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;

            Dictionary<string, string> DeployInfo = new Dictionary<string, string>();

            string sql = "SELECT *";
            sql += " FROM [MDA].[dbo].[TransactionLog]";
            sql += "  WHERE (SubscriberCode = '" + SubscriberCode + "') AND";
            sql += " (UploadDateTime in ";
            sql += "(select MAX(UploadDateTime) from [MDA].[dbo].[TransactionLog] where SubscriberCode = '" + SubscriberCode + "'))";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DeployInfo.Add("FileTransferred", rdr["FileTransferred"].ToString());
                DeployInfo.Add("Version", rdr["Version"].ToString());
                DeployInfo.Add("SubscriberCode", rdr["SubscriberCode"].ToString());
                DeployInfo.Add("Username", rdr["Username"].ToString());
                DeployInfo.Add("UploadDateTime", rdr["UploadDateTime"].ToString());

            }

            sqlConnection1.Close();
            return DeployInfo;
        }

        public Dictionary<string,string> GetUrlRewrite(string SiteIP)
        {
            using (ServerManager mgr = ServerManager.OpenRemote(SiteIP))
            {

                Configuration config = mgr.GetWebConfiguration("Default Web Site", "/MainMenu");
                ConfigurationSection rulesSection = config.GetSection("system.webServer/rewrite/rules");
                ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
                Dictionary<string, string> UrlRewrite = new Dictionary<string, string>();
                int i = 0;
                foreach (var r in rulesCollection)
                {
                    foreach (var e in r.ChildElements)
                    {
                        if (e.Schema.Name.ToString() == "conditions")
                        {
                            
                            ConfigurationElementCollection conditionCollection = e.GetCollection();
                            int nodeCount = conditionCollection.Count();
                            if (nodeCount != 0)
                            {
                                foreach (var condition in conditionCollection){
                                    UrlRewrite.Add(e.Schema.Name.ToString() + i++, condition.GetAttribute("pattern").Value.ToString());
                                }
                            }
                        }
                    }
                }
                return UrlRewrite;

            }
        }

        public void DeleteApp(string SiteIP, string appPath)
        {
            ServerManager manager = ServerManager.OpenRemote(SiteIP);
            Microsoft.Web.Administration.Application application = manager.Sites["Default Web Site"].Applications[appPath];
            manager.Sites["Default Web Site"].Applications.Remove(application);
            manager.CommitChanges();

        }

        public void AddApp(string SiteIP, string appPath, string sitePath)
        {
            ServerManager manager = ServerManager.OpenRemote(SiteIP);

            manager.Sites["Default Web Site"].Applications.Add(appPath, sitePath);
            manager.CommitChanges();
        }


    }
}
