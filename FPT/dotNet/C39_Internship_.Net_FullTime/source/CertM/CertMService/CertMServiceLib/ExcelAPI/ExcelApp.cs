using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CertMServiceLib.ExcelAPI
{
    public class ExcelApp
    {
        private static Application excelInstance; // excel app instance
        private static ExcelApp instance; // implement singleton pattern
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExcelApp));// log
        private readonly Dictionary<string, WorkBook> openDocument; // current open documents
        private readonly Dictionary<string, string> openDocumentName; // open document name

        private ExcelApp()
        {
            if (excelInstance == null)
            {
                excelInstance = new Microsoft.Office.Interop.Excel.Application();
                openDocument = new Dictionary<string, WorkBook>();
                openDocumentName = new Dictionary<string, string>();
                DisplayDialog = false;
                excelInstance.DisplayAlerts = false;
            }
        }
        /// <summary>
        /// get instance of excel app (singleton implement)
        /// </summary>
        /// <returns>instance of excel app (only one instance will be created)</returns>
        public static ExcelApp GetInstance()
        {
            if (instance != null)
            {
                return instance;
            }

            log.Info("Create new instance of excel app");
            instance = new ExcelApp();

            return instance;
        }

        /// <summary>
        /// undo an action
        /// </summary>
        /// <param name="refCount">number of undo count</param>
        public void UnDo(int refCount)
        {
            for (int i = 0; i < refCount; i++)
            {
                excelInstance.Undo();
            }
        }


        /// <summary>
        /// force save or close (set this is false)
        /// </summary>
        public bool DisplayDialog
        {
            get { return excelInstance.DisplayAlerts; }
            set { excelInstance.DisplayAlerts = value; }
        }

        /// <summary>
        /// Close document
        /// </summary>
        /// <param name="documentName">Document to close</param>
        public void Close(string documentName)
        {
            try
            {
                log.Info("Close document: " + documentName);
                if (!openDocument.ContainsKey(documentName))
                {
                    return;
                }

                openDocument.Remove(documentName);
                string path = (from item in openDocumentName where item.Value.Equals(documentName) select item.Key).FirstOrDefault();
                // find document in dictinonary
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
                openDocumentName.Remove(path);
                openDocument.Remove(path);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// open a document
        /// </summary>
        /// <param name="path">Path of open document</param>
        /// <returns>workbook contain data of open document, null if file not exist</returns>
        public WorkBook Open(string path)
        {
            log.Info("Open document: " + path);
      
            if(!System.IO.File.Exists(path))
            {
                log.Error(String.Format("File: {0} not exist", path));
                return null;
            }

            if (openDocumentName.ContainsKey(path))
                return openDocument[openDocumentName[path]];

            //var workbook = new WorkBook(excelInstance.Workbooks.Open(path));
            var workbook = new WorkBook(excelInstance.Workbooks.Open(path));
            openDocument[workbook.Name] = workbook;
            openDocumentName[path] = workbook.Name;

            return workbook;
        }

        /// <summary>
        /// Close all open documents
        /// </summary>
        public void CloseAllDocument()
        {
            //foreach (var doc in openDocument.Values)
            //{
            //    try
            //    {
            //        doc.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error("Error while close document: " + ex.Message);
            //    }
            //}
            openDocument.Clear();
            openDocumentName.Clear();
        }

        /// <summary>
        /// close all document
        /// </summary>
        public void CloseAll()
        {
            try
            {
                log.Info("Close all document");
                foreach (WorkBook wb in openDocument.Values)
                {
                    if (!wb.IsClosed)
                    {
                        log.Info("Close: " + wb.Name);
                        wb.Close();
                    }
                }
                openDocument.Clear();
                openDocumentName.Clear();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                excelInstance.Quit();
            }
            
        }
    }
}
