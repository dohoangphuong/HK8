using log4net;
using System;

namespace CertMServiceLib.WordAPI
{
    public class WordDocument
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WordDocument));
        // word interop 
        private readonly Microsoft.Office.Interop.Word.Document document;
        /// <summary>
        /// create new word document from system API
        /// </summary>
        /// <param name="document"></param>
        internal WordDocument(Microsoft.Office.Interop.Word.Document document)
        {
            this.document = document;
        }

        /// <summary>
        /// Save document with other name
        /// </summary>
        /// <param name="path">path to save to new doc</param>
        public void SaveAs(string path)
        {
            document.SaveAs(path);
        }

        /// <summary>
        /// export file to pdf
        /// </summary>
        /// <param name="path"></param>
        public void ExportPdf(string path)
        {
            document.ExportAsFixedFormat(path, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF);
        }

        /// <summary>
        /// undo last doc
        /// </summary>
        /// <param name="refCount"></param>
        public void Undo(int refCount)
        {
            document.Undo(refCount);
        }

        /// <summary>
        /// find and replace a text in word document
        /// </summary>
        /// <param name="findText">Text to find</param>
        /// <param name="replaceWithText">text to replace</param>
        /// <returns>true if find and replace success, false other wise</returns>
        public bool FindAndReplace(object findText, object replaceWithText)
        {
            //options
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object replace = 2;
            object wrap = 1;
            //execute find and replace
            return document.Content.Find.Execute(findText, ref matchCase, ref matchWholeWord,
                    ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, replaceWithText, ref replace,
                    ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }

        /// <summary>
        /// Save document
        /// </summary>
        public void Save()
        {
            document.Save();
        }

        /// <summary>
        /// Force close document (discard anything not saved)
        /// </summary>
        public void Close()
        {
            try
            {
                document.Close(false);                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
