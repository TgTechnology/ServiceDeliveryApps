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
    public string strLocation = "C:\\MRDOT\\Custom Applications\\MDA\\ToUpload\\";
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