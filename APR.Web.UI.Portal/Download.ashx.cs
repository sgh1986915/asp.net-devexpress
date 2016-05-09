using APR.Web.UI.Portal.Code;
using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System;
using System.Web.SessionState;
namespace APR.Web.UI.Portal
{
    /// <summary>
    /// Summary description for Download
    /// </summary>
    public class Download : IHttpHandler, IReadOnlySessionState
    {
        readonly string FileUploadPath = System.Configuration.ConfigurationManager.AppSettings["fileUploadPath"].ToString();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["InvoiceNO"] == null)
            {
                if (context.Request.QueryString["multipleFiles"] != null)
                {
                    var files = context.Request.QueryString["multipleFiles"].Split(',');
                    List<FileToProcess> objeFileProcess = new List<FileToProcess>();
                    foreach (var file in files)
                    {
                        objeFileProcess.Add(
                            new FileToProcess
                            {
                                FileName = Path.GetFileName(file),
                                FullFileName = Path.GetFileNameWithoutExtension(file),
                                FileExtension = Path.GetExtension(file),
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
                        else
                            pdffiles.Add(x);
                    });
                    if (pdffiles.Count > 0 && Otherfiles.Count > 0)
                    {
                        ProcessFIles(context, pdffiles, Otherfiles);
                    }
                    else
                        if (pdffiles.Count > 0 && Otherfiles.Count < 1)
                        {
                            DownloadPdfMerged(context, pdffiles);
                        }
                        else
                            if (Otherfiles.Count > 0 && pdffiles.Count < 1)
                            {
                                DownloadAllFileZIP(context, files, Otherfiles);
                            }
                    context.Response.End();
                }
                else if (context.Request.QueryString["SaveAll"] != null)
                {
                    var path = Path.Combine(FileUploadPath, context.Request.QueryString["InvoiceNo"]);
                    // Add a HTTP header to the output stream that specifies the default filename
                    // for the browser's download dialog
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=Invoices.ZIP");

                    // Set the HTTP MIME type of the output stream
                    context.Response.ContentType = "application/x-zip-compressed";
                    var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".doc", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".docx", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase));



                    using (ZipFile zip = new ZipFile())
                    {
                        List<byte[]> filesByte = new List<byte[]>();
                        foreach (var file in files)
                        {
                            using (var wc = new WebClient())
                            {
                                if (file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                                    filesByte.Add(wc.DownloadData(file));
                                else
                                    zip.AddEntry(Path.GetFileName(file), wc.DownloadData(file));
                            }
                        }
                        var response = PdfMerger.MergeFiles(filesByte);
                        if (response != null)
                            zip.AddEntry("Invoices.pdf", response);
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Level9;
                        zip.CompressionMethod = CompressionMethod.BZip2;

                        // Write the data out to the client.
                        zip.Save(context.Response.OutputStream);
                    }
                    context.Response.Flush();
                    context.Response.End();

                }
                else if (context.Request.QueryString["printAll"] != null)
                {
                    var path = Path.Combine(FileUploadPath, context.Request.QueryString["InvoiceNo"]);
                    var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));
                    DownloadPdfMerged(context, files.ToList());
                }
                else
                {
                    if (context.Request.QueryString["printPDFFiles"] != null)
                    {
                        var files = context.Request.QueryString["printPDFFiles"].Split(',');
                        List<FileToProcess> objeFileProcess = new List<FileToProcess>();
                        foreach (var file in files)
                        {
                            if (Path.GetExtension(file).ToLower() == ".pdf")
                                objeFileProcess.Add(
                                new FileToProcess
                                {
                                    FileName = Path.GetFileName(file),
                                    FullFileName = Path.GetFileNameWithoutExtension(file),
                                    FileExtension = Path.GetExtension(file),
                                    OriginalFilePath = file
                                });
                        }
                        List<byte[]> filesByte = new List<byte[]>();

                        //SmartSoft.PdfLibrary.PdfMerger.
                        foreach (var file in objeFileProcess)
                        {
                            using (var wc = new WebClient())
                            {
                                filesByte.Add(wc.DownloadData(file.OriginalFilePath));
                            }
                        }

                        var response = PdfMerger.MergeFiles(filesByte);
                        context.Response.ContentType = "application/pdf";
                        context.Response.BinaryWrite(response);
                        context.Response.Flush();
                    }

                    else
                    {
                        string filename = context.Request.QueryString["File"];

                        //Validate the file name and make sure it is one that the user may access
                        context.Response.Buffer = true;
                        context.Response.Clear();
                        context.Response.AddHeader("content-disposition", "attachment; filename=" + Path.GetFileName(filename));
                        context.Response.ContentType = "application/" + Path.GetExtension(filename);
                        context.Response.TransmitFile(filename);
                        //context.Response.WriteFile(filename);
                        context.Response.Flush();
                    }
                }
                context.Response.End();
            }
            else
            {
                var Invoices = context.Session["InvoiceNO"] as List<string>;
            }
        }

        private void ProcessFIles(HttpContext context, List<FileToProcess> pdffiles, List<FileToProcess> Otherfiles)
        {
            context.Response.Clear();
            List<byte[]> filesByte = new List<byte[]>();

            //SmartSoft.PdfLibrary.PdfMerger.
            foreach (var file in pdffiles)
            {
                using (var wc = new WebClient())
                {
                    filesByte.Add(wc.DownloadData(file.OriginalFilePath));
                }
            }

            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=Invoices.ZIP");

            // Set the HTTP MIME type of the output stream
            context.Response.ContentType = "application/x-zip-compressed";

            using (ZipFile zip = new ZipFile())
            {
                //Add Merged Pdf with Zip
                var response = PdfMerger.MergeFiles(filesByte);

                if (response != null)
                    zip.AddEntry("Invoices.pdf", response);
                foreach (var toZip in Otherfiles)
                {
                    using (var wc = new WebClient())
                    {
                        zip.AddEntry(toZip.FileName, wc.DownloadData(toZip.OriginalFilePath));

                        //ZipEntry e = zip.AddFile(toZip.OriginalFilePath, "");
                    }
                }

                // Set the Compress Level
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Level9;

                // Set the CompressionMethod
                zip.CompressionMethod = CompressionMethod.BZip2;

                // Write the data out to the client.
                zip.Save(context.Response.OutputStream);
            }
            context.Response.End();
        }

        private static void DownloadAllFileZIP(HttpContext context, string[] files, List<FileToProcess> Otherfiles)
        {
            if (Otherfiles.Count != 0)
            {
                context.Response.Clear();

                // Add a HTTP header to the output stream that specifies the default filename
                // for the browser's download dialog
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=Invoices.ZIP");

                // Set the HTTP MIME type of the output stream
                context.Response.ContentType = "application/x-zip-compressed";
                using (ZipFile zip = new ZipFile())
                {
                    foreach (var toZip in Otherfiles)
                    {
                        using (var wc = new WebClient())
                        {
                            zip.AddEntry(toZip.FileName, wc.DownloadData(toZip.OriginalFilePath));

                            //ZipEntry e = zip.AddFile(toZip.OriginalFilePath, "");
                        }
                    }
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Level9;
                    zip.CompressionMethod = CompressionMethod.BZip2;

                    // Write the data out to the client.
                    zip.Save(context.Response.OutputStream);
                }
                context.Response.Flush();
                context.Response.End();
            }
        }

        private static void DownloadPdfMerged(HttpContext context, List<FileToProcess> pdffiles)
        {
            if (pdffiles.Count != 0)
            {
                List<byte[]> filesByte = new List<byte[]>();

                //SmartSoft.PdfLibrary.PdfMerger.
                foreach (var file in pdffiles)
                {
                    using (var wc = new WebClient())
                    {
                        filesByte.Add(wc.DownloadData(file.OriginalFilePath));
                    }
                }

                var response = PdfMerger.MergeFiles(filesByte);

                // Clear all content output from the buffer stream
                context.Response.Clear();

                // Add a HTTP header to the output stream that specifies the default filename
                // for the browser's download dialog
                context.Response.AddHeader("Content-Disposition", "attachment; filename=Invoices.pdf");

                // Add a HTTP header to the output stream that contains the
                // content length(File Size). This lets the browser know how much data is being transfered
                context.Response.AddHeader("Content-Length", response.Length.ToString());

                // Set the HTTP MIME type of the output stream
                context.Response.ContentType = "application/octet-stream";

                // Write the data out to the client.
                context.Response.BinaryWrite(response);
            }
        }
        private static void DownloadPdfMerged(HttpContext context, List<string> pdffiles)
        {
            if (pdffiles.Count != 0)
            {
                List<byte[]> filesByte = new List<byte[]>();

                //SmartSoft.PdfLibrary.PdfMerger.
                foreach (var file in pdffiles)
                {
                    using (var wc = new WebClient())
                    {
                        filesByte.Add(wc.DownloadData(file));
                    }
                }

                var response = PdfMerger.MergeFiles(filesByte);

                // Clear all content output from the buffer stream
                context.Response.Clear();

                // Add a HTTP header to the output stream that specifies the default filename
                // for the browser's download dialog
                context.Response.AddHeader("Content-Disposition", "inline; filename=Invoices.pdf");

                // Add a HTTP header to the output stream that contains the
                // content length(File Size). This lets the browser know how much data is being transfered
                context.Response.AddHeader("Content-Length", response.Length.ToString());

                // Set the HTTP MIME type of the output stream
                context.Response.ContentType = "application/application/pdf";

                // Write the data out to the client.
                context.Response.BinaryWrite(response);
            }
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
