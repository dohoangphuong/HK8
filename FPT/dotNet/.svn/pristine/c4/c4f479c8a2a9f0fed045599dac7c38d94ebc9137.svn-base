using System;

namespace CertMServiceLib.WordAPI
{
    public class WordApp
    {
        readonly Microsoft.Office.Interop.Word.Application application; // Com word service instance
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WordApp)); // logging for this class

        /// <summary>
        /// default constructor
        /// note: this is private constructor (can not call by new outside of this class, use GetInstance instate of)
        /// </summary>
        private WordApp()
        {
            if (application == null)
            {
                application = new Microsoft.Office.Interop.Word.Application { DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone };
                //application.Options.DisplayPasteOptions = false;
                application.Options.ConfirmConversions = false;
                application.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
            }

        }

        /// <summary>
        /// get word Com Application
        /// </summary>
        /// <returns></returns>
        public Microsoft.Office.Interop.Word.Application GetWordApplication()
        {
            return application;
        }

        private static WordApp instance; // instance of word app(singleton implementation)

        /// <summary>
        /// get instance of word app (only one instance of wordapp is created when program is running)
        /// </summary>
        /// <returns>Instance of Wordapp</returns>
        public static WordApp GetInstance()
        {
            //if instance is null then create it, other else return instance
            if (instance != null)
            {
                return instance;
            }

            instance = new WordApp();

            return instance;
        }

        /// <summary>
        /// open a word document 
        /// </summary>
        /// <param name="path">Path of document</param>
        /// <returns>a Word document object (that contain document  in  path)</returns>
        public WordDocument Open(string path)
        {            
            log.Info("Open Document: " + path);
            // check if document exist
            try
            {
                if (System.IO.File.Exists(path))
                {                   
                    return new WordDocument(application.Documents.Open(path));                 
                }
            }
            catch
            {
                return null;
            }
           
            log.Error(String.Format("File: {0} not exist", path));
            return null;
        }

        /// <summary>
        ///  Close  word application
        /// </summary>
        public void Close()
        {
            try
            {
                log.Info("Close word application");
                
                application.Documents.Close();                
                application.Quit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
