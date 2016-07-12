using CertMServiceLib.ExcelAPI;
using CertMServiceLib.Interface;
using log4net;
using System;
using System.Collections.Generic;
using CertMServiceLib.Data;
using System.Globalization;

namespace CertMServiceLib.Generator
{
    /// <summary>
    /// Generator base class define behavior of generator
    /// </summary>
    public abstract class Generator : IGenerator
    {
        public CertificateModel certificateModel;
        public string DocExt; // doc file extension
        public string ExcelExt; // excel file extention
        protected string errorString = ""; // last error 
        protected WorkSheet backSideSheet; // back side sheet data
        // present template document (GST backside excel file)
        protected WorkBook backSideTemplate; // backside template document
        protected WordAPI.WordDocument frontSideDoc; // working by instance
        protected static ExcelApp excelAppInstance; // instance of excel application
        protected static WordAPI.WordApp wordApp; // instance of word app
        // export option
        public ExportOption Option { get; set; }
        // output path 
        public string OutPutPath;
        // current working folder
        public string WorkingFolder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public enum ExportOption
        {
            All,
            Front,
            Back
        }
        // date suffix
        protected static readonly Dictionary<string, string> suffixes = new Dictionary<string, string>
        {
            { "0","th" }, {"1","st" }, {"2", "nd" }, {"3", "rd" }, {"4", "th" }, {"5", "th" }, {"6", "th" }, {"7", "th" }, {"8", "th" }, {"9", "th" },
            {"10",  "th" }, {"11", "th" }, {"12", "th" },{"13", "th" }, {"14", "th" }, {"15", "th" }, {"16", "th" }, {"17", "th" }, {"18", "th" }, {"19", "th" },
            {"20", "th" }, {"21" ,"st" }, {"22", "nd" }, {"23", "rd" }, {"24", "th" }, {"25", "th" }, {"26", "th" }, {"27", "th" }, {"28", "th" }, {"29", "th" },
            {"30", "th" }, {"31", "st" }
        };

        // month in shorten form
        protected static readonly Dictionary<int, string> shortMonth = new Dictionary<int, string>
        {
            {1, "Jan" }, {2, "Feb" }, {3, "Mar" }, {4, "Apr" }, {5, "May" }, {6, "Jun" }, {7, "Jul" },
            {8, "Aug" }, {9, "Sep" }, {10, "Oct" }, {11, "Nov" }, {12, "Dec" }
        };

        // logger for this class
        protected static readonly ILog log = LogManager.GetLogger(typeof(Certificate));
        // data of certificate
        private ICertificateData data;
        public ICertificateData Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        // version of generator dev or test
        private string version;
        public string Version
        {
            get
            {
                return version;
            }
            protected set { version = value; }
        }

        public Generator()
        {
            CloseAll();
            //excelAppInstance = null;
            //wordApp = null;
            log.Info("Call library");
            
            if (wordApp == null)
            {
                log.Info("Create word instance");
                wordApp = WordAPI.WordApp.GetInstance();
            }
            if (excelAppInstance == null)
            {
                log.Info("Create excel instance");
                excelAppInstance = ExcelApp.GetInstance(); // instance of excel application
            }
            log.Info("Create working directory");
        }

        /// <summary>
        /// fill value on cell with content as value
        /// </summary>
        /// <param name="cell">cell to fill</param>
        /// <param name="value">value to fill to cell</param>
        protected bool  FillCell(WorkSheet sheet, string cell, object value, ref string UndoCount, bool hideColumnIfNA = true)
        {
            if(value is int)
            {
                if((int)value == -1 || (int)value == 0)
                {
                    if(hideColumnIfNA)
                    {
                        // no fill
                        // hide column
                        sheet.HideCell(cell);
                        UndoCount += cell + ";" ;
                        log.Info("Mark data missing at:" + cell);
                        return true;
                    }
                }
            }
            sheet.SetCellValue(cell, value);

            return false;
        }

        /// <summary>
        /// generate back side of certificate
        /// </summary>
        /// <returns>true if generate document success, false otherwise</returns>
        protected virtual bool GenerateBackSide(string outPath)
        {
            try
            {
                string cellChanges = "";
                FillBackSideCerticicate(ref cellChanges);
                log.Debug("Exporting backside pdf");
                backSideSheet.ExportPdf(outPath);
                log.Debug("Exported backside pdf");
                log.Debug("Rolling back");
                if (cellChanges != "")
                {
                    string[] cells = cellChanges.Split(';');
                    foreach(string cell in cells)
                    {
                        if (cell != "")
                        {
                            backSideTemplate.GetSheet("Data").UnHideCell(cell);
                        }
                    }
                }
                log.Debug("Done");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                errorString += Environment.NewLine + ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// fill Developer certificate 
        /// </summary>
        protected virtual bool FillBackSideCerticicate(ref string changeCell)
        {
            return true;
        }

        /// <summary>
        /// Generate frontside of certificate document
        /// </summary>
        /// <returns>true if generate document success, false otherwise</returns>
        protected virtual bool GenerateFrontSide(string outPath)
        {
            log.Debug(string.Format("Generate front side "));
            if (Option == ExportOption.Back)
            {
                log.Debug(string.Format("Nothing to do"));
                return true;
            }

            try
            {
                int day;
                int month;
                int year;
                if (Data.Date.Contains("-"))
                {
                    var splitContent = Data.Date.Split('-');
                    day = int.Parse(splitContent[0]);
                    month = int.Parse(splitContent[1]);
                    year = int.Parse(splitContent[2]);
                }
                else
                {
                    var splitContent = Data.Date.Split('/');
                    day = int.Parse(splitContent[0]);
                    month = int.Parse(splitContent[1]);
                    year = int.Parse(splitContent[2]);
                }

                if (frontSideDoc == null)
                {
                    frontSideDoc = wordApp.Open(String.Format("{0}\\frontSide{1}", WorkingFolder, DocExt));
                }

                #region Find and replace data
                int refCount = 0;
                log.Debug(string.Format("Find and replace pattern"));
                log.Debug(string.Format("find and replace Student Name: {0}", data.Name));
                if (frontSideDoc.FindAndReplace("[Name]", Data.Name))
                {
                    refCount++;
                }
                log.Debug(string.Format("Find and replace: Day: {0}", day));
                if(frontSideDoc.FindAndReplace("[Day]", day))
                {
                    refCount++;
                }
                log.Debug(string.Format("Find and replace Month: {0}", shortMonth[month]));
                if(frontSideDoc.FindAndReplace("[Month]", shortMonth[month]))
                {
                    refCount++;
                }
                log.Debug(string.Format("Find and replace year: {0}", year));
                if(frontSideDoc.FindAndReplace("[Year]", year))
                {
                    refCount++;
                }
                log.Debug(string.Format("Find and replace Specialty: {0}", data.Specialty));
                if (frontSideDoc.FindAndReplace("[Specialty]", Data.Specialty))
                {
                    refCount++;
                }
                log.Debug(string.Format("Find and replace Certificate No: {0}", Data.CerNo));
                if(frontSideDoc.FindAndReplace("[CerNo]", Data.CerNo))
                {
                    refCount++;
                }
                if (!string.IsNullOrEmpty(data.Rank))
                {
                    log.Debug(string.Format("Find and replace Rank: {0}", Data.Rank));
                    if(frontSideDoc.FindAndReplace("[Rank]", Data.Rank))
                    {
                        refCount++;
                    }
                }

                if (suffixes.ContainsKey(day.ToString()))
                {
                    log.Debug(string.Format("Find and replace Index: {0}", suffixes[day.ToString()]));
                    if(frontSideDoc.FindAndReplace("[Index]", suffixes[day.ToString()]))
                    {
                        refCount++;
                    }
                }

                #endregion
                log.Debug("Exporting front side");
                frontSideDoc.ExportPdf(outPath);
                log.Debug("Done");
                log.Debug("Rolling back");
                frontSideDoc.Undo(refCount);

                //frontSideDoc.Close();
                log.Debug("Done");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
            log.Debug(string.Format("Ended Generate Front Side"));
            return true;
        }

        /// <summary>
        /// Generate certificate, this will call generate front size and back side
        /// </summary>
        /// <returns>true if both generate front side and backside are success</returns>
        public virtual bool Generate()
        {
            log.Debug(string.Format("Generate()"));
            string frontSide = string.Format("{0}\\{1}_FrontSide.pdf", WorkingFolder, Data.CerNo);
            string backSide = string.Format("{0}\\{1}_BackSide.pdf", WorkingFolder, Data.CerNo);
            log.Debug(string.Format("Front side output: {0}, Back side output: {1}", frontSide, backSide));

            bool result = Option == ExportOption.All? (GenerateBackSide(backSide)&&
                GenerateFrontSide(frontSide)) : Option == ExportOption.Back? GenerateBackSide(backSide) : GenerateFrontSide(frontSide);
#if DEBUG
            // backSideTemplate.GetSheet(1).ExportPdf(backSide + ".pdf");
#endif
            if(result && Option == ExportOption.All)
            {
                log.Debug("Combine pdf");
                // combine pdf file
                CertificateUtility.CombinePdf(frontSide,backSide, string.Format("{0}\\{1}_FullSide.pdf", WorkingFolder, Data.CerNo));  
                            
                log.Debug("Done");
            }
            log.Debug(string.Format("Ended Generate()"));
            return result;
        }

        /// <summary>
        /// Close backside document
        /// </summary>
        public void CloseBackSide()
        {
            log.Debug(string.Format("Close Back side"));
            if (backSideTemplate == null)
            {
                return;
            }

            backSideTemplate.Close();
            backSideTemplate = null;
            backSideSheet = null;
            log.Debug(string.Format("Ended Close back side"));
        }

        /// <summary>
        /// Close FrontSide document
        /// </summary>
        public void CloseFrontSide()
        {
            log.Debug(string.Format("Close Front side"));
            if (frontSideDoc == null)
            {
                return;
            }

            frontSideDoc.Close();
            frontSideDoc = null;
            log.Debug(string.Format("Ended close front side"));
        }

        /// <summary>
        /// Close current document (excel and word, app instance continue in use)
        /// </summary>
        public void CloseDocument()
        {
            log.Debug(string.Format("Close Documents"));
            CloseBackSide();
            CloseFrontSide();
            log.Debug(string.Format("Ended close document"));
        }

        /// <summary>
        /// need to call to exit excel app
        /// close excel app instance and word app instance
        /// </summary>
        public void CloseAll()
        {
            log.Debug("Call close all");
            try
            {
                excelAppInstance.CloseAll();               
                CloseDocument();
            }
            catch (Exception ex)
            {
                log.Info(ex);
            }
            finally
            {
                excelAppInstance = null;
                wordApp = null;
                frontSideDoc = null;
                backSideTemplate = null;
                backSideSheet = null;
            }
            log.Debug(string.Format("Ended close all"));
        }
    }
}
