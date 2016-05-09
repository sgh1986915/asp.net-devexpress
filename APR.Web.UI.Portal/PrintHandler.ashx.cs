using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using APR.Web.UI.Portal.Code;
using Ionic.Zip;
using System.Configuration;

namespace APR.Web.UI.Portal
{
    /// <summary>
    /// Summary description for PrintHandler
    /// </summary>
    public class PrintHandler : IHttpHandler
    {
        //const string FileUploadPath = @"C:\Users\Mamoon\Documents\Visual Studio 2012\Projects\patrick\apr.web.ui\APR.Web.UI.Portal\App_Data";
        readonly string FileUploadPath = System.Configuration.ConfigurationManager.AppSettings["fileUploadPath"].ToString();
        //const string FileUploadPath = @"\\apr-DEVSQL2-srv.apauditcorp.com\Client Data\";
        public void ProcessRequest(HttpContext context)
        {
            var invoiceNumber = context.Request.QueryString["invoice"];
            var folder = context.Request.QueryString["folder"];
            var path = Path.Combine(FileUploadPath, folder, "portal", "claims");
            var filter = context.Request.QueryString["files"];
            var files = Directory.GetFiles(path, "*.*").Where(p => p.ToLower().Contains(invoiceNumber.ToLower())).ToArray();

            if (!string.IsNullOrEmpty(filter))
            {
                var filterFIles = filter.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < filterFIles.Length; i++)
                {
                    filterFIles[i] = filterFIles[i].ToLower();
                }
                files = files.Where(p => filterFIles.Contains(Path.GetFileName(p.ToLower()))).ToArray();
            }
            var mode = context.Request.QueryString["model"];
            if (!files.Any())
            {
                ZeroFiles(context, mode);
                return;
            }
            if (mode.Equals("save"))
            {
                //var files=Directory.GetFiles(path, "*.*").Where(p => p.ToLower().Contains(invoiceNumber.ToLower())).ToList();
                //var files = context.Request.QueryString["multipleFiles"].Split(',');
                var objeFileProcess = new List<FileToProcess>();
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
            else // print file sequence
            {
                //var files = context.Request.QueryString["printPDFFiles"].Split(',');
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
                if (!objeFileProcess.Any())
                {
                    ZeroFiles(context, mode);
                    return;
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

            context.Response.End();
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
        private static void ZeroFiles(HttpContext context, string mode)
        {
            context.Response.Clear();
            if (mode == "save")
                context.Response.Write("No files found to save.");
            else
                context.Response.Write("No Pdf files selected or found to print.");
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
