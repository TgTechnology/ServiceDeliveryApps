using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using MDA.PrincipalManagement;

namespace MDA
{
    public class Customer
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

        public string Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public String IpAddress
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Array Menus
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void GetCustomers()
        {
            PrincipalManagement.PrincipalManagementInterfaceSoapClient IPrincipalManagement = new PrincipalManagementInterfaceSoapClient();
            Array Hotels = IPrincipalManagement.ReadAllGroups();


            throw new System.NotImplementedException();
        }


        public void GetCustomer()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCustomer()
        {
            throw new System.NotImplementedException();
        }
    }
}
