using System;
using System.Collections.Generic;

namespace CertMServiceLib.ExcelAPI
{
    public class WorkBook
    {
        private readonly Dictionary<string, int> sheetIndexs; // work sheet index
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkBook)); // logging tool
        private readonly Microsoft.Office.Interop.Excel.Workbook workbook; // instance of excel Com workbook

        public WorkBook(Microsoft.Office.Interop.Excel.Workbook workBook)
        {
            IsClosed = false;
            workbook = workBook;
            sheetIndexs = new Dictionary<string, int>();
            // add workbook to list and to index list
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                var worksheet = workbook.Worksheets[i + 1] as Microsoft.Office.Interop.Excel.Worksheet;
                if (worksheet != null)
                {
                    sheetIndexs[worksheet.Name] = (i + 1);
                }
            }
        }

        /// <summary>
        /// Save current document with other name
        /// </summary>
        /// <param name="path">path to save, sample : C:\Users\Document\abc.xlsx</param>
        public void SaveAs(string path)
        {
            workbook.SaveAs(path);
        }

        /// <summary>
        /// Save current workbook .document
        /// </summary>
        public void Save()
        {
            workbook.Save();
        }

        /// <summary>
        /// Close current document
        /// </summary>
        public void Close()
        {
            log.Info("Close Workbook: " + Name);
            try
            {
                if (IsClosed)
                {
                    return;
                }

                IsClosed = true;
                workbook.Close();
                ExcelApp.GetInstance().CloseAllDocument();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// determind if document is closed or not
        /// </summary>
        public bool IsClosed
        {
            get;
            set;
        }

        /// <summary>
        /// Get Workbook Name
        /// </summary>
        public string Name
        {
            get { return workbook.Name; }
        }

        /// <summary>
        /// Get number of sheetBook count
        /// </summary>
        public int SheetCount
        {
            get { return workbook.Worksheets.Count; }
        }
        /// <summary>
        ///  get worksheet by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public WorkSheet GetSheet(int index)
        {
            log.Debug(string.Format("Opening sheet: {0}", index));
            return new WorkSheet(workbook.Worksheets[index]);
        }

        /// <summary>
        /// Get Work sheet by name (use this when all sheet in workbook is not the same Name)
        /// </summary>
        /// <param name="sheetName">Name of sheet</param>
        /// <returns></returns>
        public WorkSheet GetSheet(string sheetName)
        {
            log.Debug(string.Format("Get sheet by name: {0}", sheetName));
            if (sheetIndexs.ContainsKey(sheetName))
                return new WorkSheet(workbook.Worksheets[sheetIndexs[sheetName]]);

            log.Error(sheetName + " Not exist");
            throw new NullReferenceException("The given name is not a worksheet name");
        }

        /// <summary>
        /// export excel worksheet to pdf
        /// </summary>
        /// <param name="path"></param>
        public void ExportPdf(string path)
        {
            log.Info("Export : " + path);
            workbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, path);
        }
    }
}
