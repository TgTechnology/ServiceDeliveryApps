Progress Bar in ASP.NET 2.0 Using File uploader user controll:

I had written couple of articles on uploading file user control, Bulk file uploader using C# and Asp.net 1.1 & ASP.net 2.0 Many of users have written to me that if i can give thought for writing a progress bar while uploading file, so i just find a way as how we can write a progress bar, may be you can extend it as per your idea later.

To give you a detail in my sample application i have "Uploader.ascx" user control which hold Fileupload control and upload submit button whose code look as below:

<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Uploader.ascx.cs" Inherits="Uploader" %>
<asp:FileUpload ID="upUserCtrl" runat="server" />
<asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />


"Uploader.ascx.cs" which is code behind page in which i have written seperate function to print & once the upload finish clear the progress bar, both the functions are javascript functions, which are written at the server side, here i have done a trick to have a sort of progress bar, i have function ShowWait() javascript function which prints progressbar in "<div>" tag

Entire functionality as below:

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Threading;

public partial class Uploader : System.Web.UI.UserControl
{
    public string strLocation = "c:\\inetpub\\wwwroot\\ProgressBar\\ToUpload\\";
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    //Javascript function to print progressbar
    public static void PrintProgressBar()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");
        sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
        sb.Append("<script language=javascript>");
        sb.Append("var dts=0; var dtmax=10;");
        sb.Append("function ShowWait(){var output;output='Please wait while uploading!';dts++;if(dts>=dtmax)dts=1;");
        sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='red';}");
        sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
        sb.Append("StartShowWait();</script>");
        HttpContext.Current.Response.Write(sb.ToString());
        HttpContext.Current.Response.Flush();
    }
    //Javascript function to clear progressbar
    public static void ClearProgressBar()
    {
        StringBuilder sbc = new StringBuilder();
        sbc.Append("<script language='javascript'>");
        sbc.Append("alert('Upload process completed successfully!');");
        sbc.Append("up_div.style.visibility='hidden';");
        sbc.Append("history.go(-1)");
        sbc.Append("</script>");
        HttpContext.Current.Response.Write(sbc);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string strFileName = System.IO.Path.GetFileName(upUserCtrl.PostedFile.FileName);
        try
        {
            if (strFileName != "")
            { 
                //print progressbar
                PrintProgressBar(); 
                upUserCtrl.PostedFile.SaveAs(strLocation + strFileName);
                Thread.Sleep(2000);
                ClearProgressBar();
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}

Note: "ToUpload" is a folder where i am uploading the files and by the time you must be aware that to upload file in any of the server side folder you should have permission i.e. "ASPNET" user should have full permission on the folder so that you can uplod file.

I have "Default.aspx" page on which i had called the user controll for uploading file, which is as below:
<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Src="Uploader.ascx" TagName="Uploader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:Uploader ID="Uploader1" runat="server" />
    </div>
    </form>
</body>
</html>

I have attached image here along with which will give you clear idea as how this functionality works. May be some of you can put this progress bar functionality in Pop up window also.
