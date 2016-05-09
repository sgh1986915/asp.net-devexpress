using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace APR.Web.UI.Portal.Code
{
    public static class PdfMerger
    {
        /// <summary>
        /// Merge pdf files.
        /// </summary>
        /// <param name="sourceFiles">PDF files being merged.</param>
        /// <returns></returns>
        public static byte[] MergeFiles(List<byte[]> sourceFiles)
        {
            // returns memory stream buffer
            byte[] returnOutPut;

            try
            {
                var document = new Document();
                
                   var output = new MemoryStream();
                    
                        // Initialize pdf writer
                        var writer = PdfWriter.GetInstance(document, output);
                        
                            writer.PageEvent = new PdfPageEvents();

                            // Open document to write
                            document.Open();
                            PdfContentByte content = writer.DirectContent;

                            // Iterate through all pdf documents
                            foreach (byte[] sourceFile in sourceFiles)
                            {
                                // Create pdf reader
                                using (var reader = new PdfReader(sourceFile))
                                {
                                    int numberOfPages = reader.NumberOfPages;

                                    // Iterate through all pages
                                    for (int currentPageIndex = 1;
                                                                                 currentPageIndex <= numberOfPages;
                                                                                                                                                                 currentPageIndex++)
                                    {
                                        // Determine page size for the current page
                                        document.SetPageSize(reader.GetPageSizeWithRotation(currentPageIndex));

                                        // Create page
                                        document.NewPage();
                                        PdfImportedPage importedPage = writer.GetImportedPage(reader,
                                                                                              currentPageIndex);
                                        // Determine page orientation
                                        int pageOrientation = reader.GetPageRotation(currentPageIndex);
                                        if ((pageOrientation == 90) || (pageOrientation == 270))
                                        {
                                            content.AddTemplate(importedPage, 0, -1f, 1f, 0, 0,
                                                                reader.GetPageSizeWithRotation(currentPageIndex)
                                                                      .Height);
                                        }
                                        else
                                        {
                                            content.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                                        }
                                    }
                                }
                            }
                        
                        returnOutPut = output.GetBuffer();
                       // output.Close();
            }
            catch
            (Exception exception)
            {
                throw new Exception("There has an unexpected exception occured during the pdf merging process.",
                                    exception);
            }
            return returnOutPut;
        }
    }

    /// <summary>
    /// Implements custom page events.
    /// </summary>
    internal class PdfPageEvents : IPdfPageEvent
    {
        #region members

        private BaseFont _baseFont = null;
        private PdfContentByte _content;

        #endregion members

        #region IPdfPageEvent Members

        public void OnOpenDocument(PdfWriter writer, Document document)
        {
            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            _content = writer.DirectContent;
        }

        public void OnStartPage(PdfWriter writer, Document document)
        {
        }

        public void OnEndPage(PdfWriter writer, Document document)
        {
            // Write header text
            string headerText = "PDF Merger by APR";
            _content.BeginText();
            _content.SetFontAndSize(_baseFont, 8);
            _content.SetTextMatrix(GetCenterTextPosition(headerText, writer), writer.PageSize.Height - 10);
            _content.ShowText(headerText);
            _content.EndText();

            // Write footer text (page numbers)
            string text = "Page " + writer.PageNumber;
            _content.BeginText();
            _content.SetFontAndSize(_baseFont, 8);
            _content.SetTextMatrix(GetCenterTextPosition(text, writer), 10);
            _content.ShowText(text);
            _content.EndText();
        }

        public void OnCloseDocument(PdfWriter writer, Document document)
        {
        }

        public void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
        {
        }

        public void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
        }

        public void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
        {
        }

        public void OnChapterEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
        }

        public void OnSection(PdfWriter writer, Document document, float paragraphPosition, int depth, Paragraph title)
        {
        }

        public void OnSectionEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
        }

        public void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
        {
        }

        #endregion IPdfPageEvent Members

        private float GetCenterTextPosition(string text, PdfWriter writer)
        {
            return writer.PageSize.Width / 2 - _baseFont.GetWidthPoint(text, 8) / 2;
        }
    }
}
