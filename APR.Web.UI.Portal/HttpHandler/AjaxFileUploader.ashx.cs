using System.IO;
using System.Web;

namespace APR.Web.UI.Portal.HttpHandler
{
    /// <summary>
    /// Summary description for AjaxFileUploader
    /// </summary>
    public class AjaxFileUploader : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                var path = context.Server.MapPath("~/Temp");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = context.Request.Files[0];

                string fileName;

                if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                {
                    var files = file.FileName.Split(new char[] { '\\' });
                    fileName = files[files.Length - 1];
                }
                else
                {
                    fileName = file.FileName;
                }
                var strFileName = fileName;
                fileName = Path.Combine(path, fileName);
                file.SaveAs(fileName);

                var msg = "{";
                msg += string.Format("error:'{0}',\n", string.Empty);
                msg += string.Format("msg:'{0}'\n", strFileName);
                msg += "}";
                context.Response.Write(msg);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}
