using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;
using MDA.PrincipalManagement;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;

namespace MDA
{
    public class Customer
    {
        public string Name
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        public string Type
        {
            get
            {
                return this.Type;
            }
            set
            {
                this.Type = value;
            }
        }

        public string ExternalId
        {
            get
            {
                return this.ExternalId;
            }
            set
            {
                this.ExternalId = value;
            }
        }

        public string IpAddress
        {
            get
            {
                return this.IpAddress;
            }
            set
            {
                this.IpAddress = value;
            }
        }

        public Dictionary<string, string> GetCustomers()
        {
            PrincipalManagement.PrincipalManagementInterfaceSoapClient IPrincipalManagement = new PrincipalManagementInterfaceSoapClient();
            Array Hotels = IPrincipalManagement.ReadAllGroups();

            Dictionary<string, string> SubscriberGroups = new Dictionary<string, string>();


            foreach (PrincipalManagement.Group1 h in Hotels)
            {
                if (h.Type.ToString().Contains("Site"))
                {
                    SubscriberGroups.Add(h.ExternalID.ToString(), h.Type.ToString());
                }
            }
            return SubscriberGroups;
        }

        public Dictionary<string, string> GetCustomer(string SubscriberCode)
        {
            SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=TGNDP1SCOMRS001\OPSMGR;Initial Catalog=MDA;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;

            Dictionary<string, string> CustomerInfo = new Dictionary<string, string>();

            string sql = "SELECT     SubscriberGroup.*, SubscriberNetwork.*";
            sql += " FROM SubscriberGroup INNER JOIN";
            sql += " SubscriberNetwork ON SubscriberGroup.SubscriberId = SubscriberNetwork.SubscriberId";
            sql += " WHERE  (SubscriberGroup.SubscriberCode = '" + SubscriberCode + "')";
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                CustomerInfo.Add("MRCCIPPrimary", rdr["MRCCIPPrimary"].ToString());
                CustomerInfo.Add("CustomerName", rdr["Name"].ToString());
                CustomerInfo.Add("SiteType", rdr["SiteType"].ToString());
                CustomerInfo.Add("SubscriberCode", rdr["SubscriberCode"].ToString());
                CustomerInfo.Add("EGRESSIPPrimary", rdr["EGRESSIPPrimary"].ToString());
                CustomerInfo.Add("EGRESSIPSecondary", rdr["EGRESSIPSecondary"].ToString());
            }

            sqlConnection1.Close();
            return CustomerInfo;
        }


    }
}
