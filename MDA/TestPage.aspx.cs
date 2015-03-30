using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Management;
using Microsoft.Web.Administration;
using MDA.PrincipalManagement;
using System.Threading.Tasks;


namespace MDA
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            string status = "";
            Utilities oUtil = new Utilities();
            status = oUtil.StopService("10.5.30.13", "W3SVC");
            Label1.Text = status;
            status = oUtil.StartService("10.5.30.13", "W3SVC");
            Label1.Text = status;
            //PrincipalManagement.DeviceValue[] deviceVal = { new PrincipalManagement.DeviceValue() { Key = "PConBlockUnratedPrograms", Value = "1" }, new PrincipalManagement.DeviceValue() { Key = "PinRetryTimeoutMinutes", Value = "0" } };
            //PrincipalManagementSoapClient oPrincipalManagement = new PrincipalManagementSoapClient();
            //string externalID = "";
            //Account[] arrAccounts = oPrincipalManagement.ReadAccount(externalID);
            //foreach (Account o in arrAccounts)
            //{
            //    Response.Write(o.ExternalID.ToString());
            //}

            //oPrincipalManagement.UpdateDeviceValuesAndNotify("TANG-100010-22222-01", deviceVal);
            //RemoteRecord2.RemoteRecordScheduler2SoapClient oDVRStorage = new RemoteRecord2.RemoteRecordScheduler2SoapClient();
            //RemoteRecord2.StringExternalId accountID = new RemoteRecord2.StringExternalId();
            //accountID.Id = "TANG-100005-00462";

            //Array arrRecordings = oDVRStorage.GetStorageInfos(accountID);
            //foreach (RemoteRecord2.StorageInfo o in arrRecordings)
            //{
            //    Response.Write(o.AvailableBytes.ToString());
            //}

        }
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}