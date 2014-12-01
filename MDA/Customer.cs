using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using MDA.PrincipalManagement;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;

namespace MDA
{
    public class Customer
    {
        public Array GetCustomers()
        {
            PrincipalManagement.PrincipalManagementInterfaceSoapClient IPrincipalManagement = new PrincipalManagementInterfaceSoapClient();
            Array Hotels = IPrincipalManagement.ReadAllGroups();
            return Hotels;
        }


        public Object GetCustomer(string SubscriberCode)
        {
            SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=TGNDP1SCOMRS001\OPSMGR;Initial Catalog=MDA;Integrated Security=True");
            SqlCommand cmd = new SqlCommand();
            Object returnValue;

            string sql = "SELECT SubscriberNetwork.MRCCIPPrimary";
                   sql += " FROM SubscriberGroup INNER JOIN";
                   sql += " SubscriberNetwork ON SubscriberGroup.SubscriberId = SubscriberNetwork.SubscriberId";
                   sql += " WHERE  (SubscriberGroup.SubscriberCode = '" + SubscriberCode + "')";
                   cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            returnValue = cmd.ExecuteScalar();
            sqlConnection1.Close();

            return returnValue;
        }
    }
}
