using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CertMServiceLib.ExcelAPI
{
   /// <summary>
   /// call excel work sheet 
   /// </summary>
    public class WorkSheet
    {
       private readonly Microsoft.Office.Interop.Excel.Worksheet workSheet; // Com worksheet
        /// <summary>
        /// mapping value for header
        /// </summary>
        private static readonly Dictionary<string,int> cellMapping  =  new Dictionary<string,int>
        {
            { "A", 1 }, { "B", 2 }, { "C", 3 }, { "D", 4 }, { "E", 5 }, { "F", 6 }, { "G", 7 }, { "H", 8 }, { "I", 9 }, { "J", 10 }, { "K", 11 }, { "L", 12 }, { "M", 13 }, { "N", 14 }, { "O", 15 }, { "P", 16 }, 
            { "Q", 17 }, { "R", 18 }, { "S", 19 }, { "T", 20 }, { "U", 21 }, { "V", 22 }, { "W", 23 }, { "X", 24 }, { "Y", 25 }, { "Z", 26 } ,{"AA",27},{"AB",28},{"AC",29},{"AD",30},{"AE",31},
            {"AF",32},{"AG",33},{"AH",34},{"AI",35},{"AJ",36},{"AK",37},{"AL",38},{"AM",39},{"AN",40},{"AO",41},{"AP",42},{"AQ",43},{"AR",44},{"AS",45},{"AT",46},{"AU",47},{"AV",48},{"AW",49},{"AX",50},{"AY",51},{"AZ",52},
            {"AAA",53},{"AAB",54},{"AAC",55},{"AAD",56},{"AAE",57},{"AAF",58},{"AAG",59},{"AAH",60},{"AAI",61},{"AAJ",62},{"AAK",63},{"AAL",64},{"AAM",65},{"AAN",66},{"AAO",67},{"AAP",68},{"AAQ",69},{"AAR",70},
            {"AAS",71},{"AAT",72},{"AAU",73},{"AAV",74},{"AAW",75},{"AAX",76},{"AAY",77},{"AAZ",78}
        };

        public WorkSheet(Microsoft.Office.Interop.Excel.Worksheet workSheet)
        {
            this.workSheet = workSheet;
        }
        /// <summary>
        /// save current worksheet as a new document
        /// </summary>
        /// <param name="path">Path to new document</param>
        public void SaveAs(string path)
        {
            workSheet.SaveAs(path);
        }

        /// <summary>
        /// Set cell value 
        /// </summary>
        /// <param name="row"> Row of current cell</param>
        /// <param name="column">Column of current cell</param>
        /// <param name="value">value to set</param>
        public void SetCellValue(int row, int column, object value)
        {
            workSheet.Cells[row, column] = value;
        }

        /// <summary>
        /// Set value for cell 
        /// </summary>
        /// <param name="cell">Which cell (sample B2 or C2)</param>
        /// <param name="value">Value to set</param>
        public void SetCellValue(string cell, object value)
        {
            var regex = new Regex(@"([A-Z]+)(\d+)");
            var result = regex.Match(cell);
            string header = result.Groups[1].ToString();
            int row = int.Parse(result.Groups[2].ToString());
             SetCellValue(row, cellMapping[header], value);
        }

        /// <summary>
        /// hide cell 
        /// </summary>
        /// <param name="cell">cell to hide</param>
        public void HideCell(string cell)
        {
            workSheet.Range[cell].EntireColumn.Hidden = true;
        }

        /// <summary>
        /// Unhide cell 
        /// </summary>
        /// <param name="cell">cell to hide</param>
        public void UnHideCell(string cell)
        {
            workSheet.Range[cell].EntireColumn.Hidden = false;
        }

        /// <summary>
        /// Get Cell Value
        /// </summary>
        /// <param name="row">Row of cell (sample 2)</param>
        /// <param name="column">Cell index Colum  in sheet</param>
        /// <returns>value of cell at (row, cell)</returns>
        public object GetCellValue(int row, int column)
        {
            return workSheet.Cells[row, column].Value;
        }

        /// <summary>
        /// Get Cell Value
        /// </summary>
        /// <param name="cell"> Cell to get value : sample H3</param>
        public string  GetCellValue(string cell)
        {
            string value = null;
            var regex = new Regex(@"([A-Z]+)(\d+)");
            var result = regex.Match(cell);
            var header = result.Groups[1].ToString();
            var row = int.Parse(result.Groups[2].ToString());
            var obj = GetCellValue(row, cellMapping[header]);
            if(obj != null)
            {
                Type t = obj.GetType();
                if (t.Equals(typeof(double)))
                {
                    value = obj.ToString();
                }
                else
                    value = (string)obj;
            }
                       
            return value;             
        }

        /// <summary>
        /// Export file to pdf format
        /// </summary>
        /// <param name="path"> path to save: sample : c:\documents\abc.pdf</param>
        public void ExportPdf(string path)
        {
            workSheet.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, path);
        }

        /// <summary>
        /// apply filter for this document
        /// </summary>
        /// <param name="value">filter object</param>
        /// <param name="column">Column to apply filter</param>
        private void Filter(object[] value, string column)
        {
            workSheet.UsedRange.AutoFilter(cellMapping[column], value);
        }

        /// <summary>
        /// Name of current document
        /// </summary>
        public string Name
        {
            get { return workSheet.Name; }
        }

        /// <summary>
        /// Number of row data in this document
        /// </summary>
        public int DataRowCount
        {
            get { return workSheet.UsedRange.Rows.Count; }
        }

        /// <summary>
        /// Number of column data in this document
        /// </summary>
        public int DataColCount
        {
            get { return workSheet.UsedRange.Columns.Count; }
        }
    }
}
