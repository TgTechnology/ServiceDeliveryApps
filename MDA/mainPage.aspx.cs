using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MDA
{
    public partial class mainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Customer Hotels = new Customer();
            Array HotelList = Hotels.GetCustomers();
            Response.Write(HotelList);
        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomers.SelectedIndex > 0)
            {
                string goToWebsite = ddlCustomers.SelectedValue;
                Response.Redirect(goToWebsite);
            }
        }
    }
}