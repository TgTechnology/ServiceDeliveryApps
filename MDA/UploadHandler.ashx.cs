using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDA
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            foreach (string key in files)
            {
                HttpPostedFile file = files[key];
                string fileName = file.FileName;
                fileName = context.Server.MapPath("~/uploads/" + fileName);
                file.SaveAs(fileName);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("File(s) uploaded successfully!");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}