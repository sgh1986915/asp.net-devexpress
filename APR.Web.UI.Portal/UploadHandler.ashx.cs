using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;

namespace APR.Web.UI.Portal
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private readonly string _fileUploadPath = ConfigurationManager.AppSettings["fileUploadPath"];
        public void ProcessRequest(HttpContext context)
        {
            var file = context.Request.Files["file"];
            var invoiceNumber = context.Request.Form["invoice"];
            var folder = context.Request.Form["folder"];
            var path = Path.Combine(_fileUploadPath, folder, "portal", "claims");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fileName = invoiceNumber + new Random().Next(int.MaxValue) + Path.GetExtension(file.FileName);
            path = Path.Combine(path, fileName);
            file.SaveAs(path);
            context.Response.Write(fileName);
            context.Response.End();
        }
        private byte[] GetByteFromInputStream(Stream strm)
        {
            using (var memoryStream = new MemoryStream())
            {
                strm.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public static string GetUniqueFileName(string name, string savePath, string ext)
        {
            name = name.Replace(ext, "").Replace(" ", "_");
            name = System.Text.RegularExpressions.Regex.Replace(name, @"[^\w\s]", "");

            var newName = name;
            var i = 0;
            if (System.IO.File.Exists(savePath + newName + ext))
            {
                do
                {
                    i++;
                    newName = String.Format("{0}_{1}", name, i);
                }
                while (System.IO.File.Exists(savePath + newName + ext));
            }

            return newName;
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
