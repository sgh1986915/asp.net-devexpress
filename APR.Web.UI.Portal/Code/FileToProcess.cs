namespace APR.Web.UI.Portal.Code
{
    public class FileToProcess
    {
        /// <summary>
        /// File Name with Extension
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File Name without Extension
        /// </summary>
        public string FullFileName { get; set; }

        /// <summary>
        /// File Extension
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Original Filepath
        /// </summary>
        public string OriginalFilePath { get; set; }
    }
}
