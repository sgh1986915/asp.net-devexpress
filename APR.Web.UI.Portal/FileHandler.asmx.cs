using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.IO;
using Ionic.Zip;
using APR.Web.UI.Portal.Code;

namespace APR.Web.UI.Portal
{
    /// <summary>
    /// Summary description for Filehandles
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class FileHandler : System.Web.Services.WebService
    {

        [WebMethod]
       
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        
        public void DowloadFiles(string[] Files )
        {
            List<FileToProcess> objeFileProcess = new List<FileToProcess>();
            foreach (var file in Files)
            {

                var fileName = Path.GetFileName(file);
                var fileOriginalName = Path.GetFileNameWithoutExtension(file);
                var fileExtension = Path.GetExtension(file);
                var fileStream = File.OpenRead(file);
                objeFileProcess.Add(
                    new FileToProcess
                {
                    FileName = fileName,
                    FullFileName = fileOriginalName,
                    FileExtension = fileExtension,
                    //FileStream = fileStream,
                    OriginalFilePath = file
                });


            }
            var pdffiles = new List<FileToProcess>();
            var Otherfiles = new List<FileToProcess>();
            objeFileProcess.ForEach(x =>
            {
                if (x.FileExtension.ToLower() != ".pdf")
                {
                    Otherfiles.Add(x);

                }
                else pdffiles.Add(x);
            });
            //DownLoadAsZip(pdffiles);
           //DownLoadAsZip(Otherfiles);
            DownloadPDFMerge(pdffiles);
            //HttpContext.Current.Response.Close();

          
        }
        private void DownloadPDFMerge(List<FileToProcess> pdffiles)
        {
            if (pdffiles.Count == 0) return;
            var filesByte = new List<byte[]>();

            //SmartSoft.PdfLibrary.PdfMerger.
            foreach (var file in pdffiles)
            {
                using (var wc = new WebClient())
                {
                    //wc.ResponseHeaders.Add("content-disposition", "attachment; filename=InvoiceFiles.pdf");
                    //filesByte.Add(File.ReadAllBytes(file.OriginalFilePath));
                    filesByte.Add(wc.DownloadData(file.OriginalFilePath));
                    var response = PdfMerger.MergeFiles(filesByte);

                    HttpContext.Current.Response.Clear();

                    using (var ms = new MemoryStream(response))
                    {
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader
                            ("content-disposition", "attachment;filename=Invoice.pdf");
                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.OutputStream.Write
                            (ms.GetBuffer(), 0, ms.GetBuffer().Length);
                        HttpContext.Current.Response.OutputStream.Flush();
                        HttpContext.Current.Response.End();
                    }
                }
            }

            //context.Response.Buffer = true;
                //context.Response.Clear();
                //context.Response.AddHeader("content-disposition", "inline; filename=InvoseFiles.pdf");
                //context.Response.ContentType = "application/pdf";

                //byte[] response = PdfMerger.MergeFiles(filesByte);

               // using(var f =File.Create("D:\"))
                //WebClient client = new WebClient();
                //client.ResponseHeaders.Add("content-disposition", "inline; filename=InvoseFiles.pdf");
                ////HttpContext.Current.Response.AddHeader("Content-Length", response.Length.ToString());
                //client.ResponseHeaders.Add("Content-Length", response.Length.ToString());
                //client.DownloadFile(pdffiles[0].OriginalFilePath, "");
                //client.DownloadFile(

                #region Download without Webclient
                ////HttpContext.Current.Response.AddHeader("Content-Length", response.Length.ToString());
                //////byte[] pdf;
                ////context.Response.BinaryWrite(response);
                ////context.Response.OutputStream.Write(response, 0, response.Length);
                //// Clear all content output from the buffer stream
                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.Buffer = true;
                //// Add a HTTP header to the output stream that specifies the default filename
                //// for the browser's download dialog
                //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Invoice.pdf");
                //// Add a HTTP header to the output stream that contains the 
                //// content length(File Size). This lets the browser know how much data is being transfered
                //HttpContext.Current.Response.AddHeader("Content-Length", response.Length.ToString());
                //// Set the HTTP MIME type of the output stream
                //HttpContext.Current.Response.ContentType = "application/octet-stream";
                // Write the data out to the client.
                //HttpContext.Current.Response.BinaryWrite(response);
                //HttpContext.Current.Response.OutputStream.Write(response, 0, response.Length);
                //HttpContext.Current.Response.Flush();
                //HttpContext.Current.Response.Close();
                //HttpContext.Current.Response.End(); 
                #endregion
               
        }
        private static string DownLoadAsZip(List<FileToProcess> files)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=InvoseFiles.zip");
            HttpContext.Current.Response.ContentType = "application/zip";
            using (ZipFile zip = new ZipFile())
            {
                //zip.AddFiles(files);

                foreach (var file in files)
                {

                    //var fileName = Path.GetFileName(file);
                    //var fileOriginalName = Path.GetFileNameWithoutExtension(file);
                  
                   // var fileStream = File.OpenRead(file);
                    if (file.FileExtension != ".pdf")
                    {
                        //ZipEntry e = zip.AddFile(file.OriginalFilePath, "");
                        //zip.AddEntry(file.FileName, file.FileStream);

                    }
                }
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.Save(HttpContext.Current.Response.OutputStream);
            }
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            return "InvoseFiles.zip";
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
        }

        void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            
        }

    }
     
}

