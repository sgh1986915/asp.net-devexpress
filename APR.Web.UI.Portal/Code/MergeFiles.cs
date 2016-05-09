using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace APR.Web.UI.Portal.Code
{
    public class MergeFiles
    {
        public bool MergeFile(string inputfoldername1)
        {
            var Output = false;
            try
            {
                var tmpfiles = Directory.GetFiles(inputfoldername1, "*.tmp");
                var outPutFile = (FileStream )null;
                var PrevFileName = string.Empty;
                foreach (string tempFile in tmpfiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(tempFile);
                    var baseFileName = fileName.Substring(0, fileName.IndexOf(Convert.ToChar(".")));
                    var extension = Path.GetExtension(fileName);
                    if (!PrevFileName.Equals(baseFileName))
                    {
                        if (outPutFile != null)
                        {
                            outPutFile.Flush();
                            outPutFile.Close();
                        }
                        outPutFile = new FileStream( "\\" + baseFileName + extension, FileMode.OpenOrCreate, FileAccess.Write);
                    }
                    var bytesRead = 0;
                    var buffer = new byte[1024];
                    var inputTempFile = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Read);
                    while ((bytesRead = inputTempFile.Read(buffer, 0, 1024)) > 0)
                    {
                        outPutFile.Write(buffer, 0, bytesRead);
                    }
                    inputTempFile.Close();
                    File.Delete(tempFile);
                    PrevFileName = baseFileName;
                }
                outPutFile.Close();
            }
            catch
            {
            }
            return Output;
        }
    }
}
