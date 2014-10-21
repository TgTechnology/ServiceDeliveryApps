using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;


namespace MDA
{
    public partial class mainPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            ddlCustomers_SelectedIndexChanged(ddlCustomers, e);
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Customer Hotels = new Customer();
            Array HotelList = Hotels.GetCustomers();
            int GroupID = 1;

            foreach (PrincipalManagement.Group1 o in HotelList)
            {
                ddlCustomers.Items.Insert(GroupID, new ListItem(o.ExternalID.ToString(), o.ExternalID.ToString()));
                GroupID = GroupID + 1;
            }
        }
    }
}