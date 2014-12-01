using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using Microsoft.Web.Administration;
using MDA.PrincipalManagement;


namespace MDA
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {

            //PrincipalManagement.DeviceValue[] deviceVal = { new PrincipalManagement.DeviceValue() { Key = "PConBlockUnratedPrograms", Value = "1" }, new PrincipalManagement.DeviceValue() { Key = "PinRetryTimeoutMinutes", Value = "0" } };
            //PrincipalManagementSoapClient oPrincipalManagement = new PrincipalManagementSoapClient();

            //oPrincipalManagement.UpdateDeviceValuesAndNotify("TANG-100010-22222-01", deviceVal);
            RemoteRecord2.RemoteRecordScheduler2SoapClient oDVRStorage = new RemoteRecord2.RemoteRecordScheduler2SoapClient();
            RemoteRecord2.StringExternalId accountID = new RemoteRecord2.StringExternalId();
            accountID.Id = "TANG-100005-00462";

            Array arrRecordings = oDVRStorage.GetStorageInfos(accountID);
            foreach (RemoteRecord2.StorageInfo o in arrRecordings)
            {
                Response.Write(o.AvailableBytes.ToString());
            }

        }
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}