using CertMServiceLib.Data;
using CertMServiceLib.Interface;
using log4net;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib
{
    /// <summary>
    /// New change to match FU certificate, change FillInDevCertificate
    /// Changed:02/04/2015
    /// ManhP
    /// </summary>
    /// 
    /// <summary>
    /// New change 
    /// Changed:01/04/2016
    /// Le Tuan Anh
    /// </summary>
    public class Certificate
    {
        public LinkDownload list = new LinkDownload();
        public CertificateModel certificateModel = new CertificateModel();

        // check certificate is running or not
        private WorkingState workingState;
        private string workingFolder; // this folder is temple folder (will delete when working finish)
        //private static Dictionary<string, string> workingPath;
        private readonly string parentDir; // parent directory
        private string temporaryFolder; // temporary directory
        private List<string> fileExportList; // file list to compress
        // doc extension
        private string docExt;
        private string excelExt;
        // logger for this class
        private static readonly ILog log = LogManager.GetLogger(typeof(Certificate));
        // get service working state
        public WorkingState State { get { return workingState; } }

        public Certificate(string parentDir)
        {
            //if (workingPath == null)
            //    workingPath = new Dictionary<string, string>();
            this.parentDir = parentDir;
            log4net.Config.XmlConfigurator.Configure();
            workingState = WorkingState.Idle;
            //BasicConfigurator.Configure();
        }

        /// <summary>
        /// Copy frontside to temporary folder
        /// </summary>
        /// <param name="frontSide">Path to front side</param>
        private void CopyFrontSide(string frontSide)
        {
            log.Debug(string.Format("Copy Front side: {0}", frontSide));
            if (!File.Exists(frontSide))
            {
                log.Error(string.Format("File: {0} not exist", frontSide));
                return;
            }
            var fileInfo = new FileInfo(frontSide);
            string copyTo = string.Format("{0}\\frontSide{1}", workingFolder, fileInfo.Extension);
            if (!File.Exists(copyTo))
            {
                log.Debug(string.Format("Cpopy To: {0}", copyTo));
                File.Copy(frontSide, copyTo);
            }
            docExt = fileInfo.Extension;
            log.Debug(string.Format("Ended Copy Front side: {0}", frontSide));
        }

        /// <summary>
        /// Copy backside to temporary folder
        /// </summary>
        /// <param name="backSide">Path to backside</param>
        private void CopyBackSide(string backSide)
        {
            log.Debug(string.Format("Copy back side: {0}", backSide));
            if (!File.Exists(backSide))
            {
                log.Error(string.Format("File: {0} not exist", backSide));
                return;
            }
            var fileInfo = new FileInfo(backSide);
            string copyTo = string.Format("{0}\\backSide{1}", workingFolder, fileInfo.Extension);
            if (!File.Exists(copyTo))
            {
                log.Debug(string.Format("Copy to: {0}", copyTo));
                File.Copy(backSide, copyTo);
            }
            excelExt = fileInfo.Extension;

            log.Debug(string.Format("Ended Copy back side:"));
        }

        /// <summary>
        /// copy document to working directory
        /// document  will be copy to temporary working directory and will be deleted after working done
        /// </summary>
        /// <param name="backSide">path of backSide</param>
        /// <param name="frontSide">path od front Side</param>
        private void CopyDocument(string backSide, string frontSide)
        {
            log.Debug("Copy Document to temporary directory");
            if (!string.IsNullOrEmpty(backSide))
            {
                CopyBackSide(backSide);
            }

            if (!string.IsNullOrEmpty(frontSide))
            {
                CopyFrontSide(frontSide);
            }
            log.Debug("Ended Copy Document to temporary directory.");
        }

        /// <summary>
        /// Export certificate from a list of certificate data
        /// </summary>
        /// <param name="datas">List of Certificate dataSource</param>
        /// <param name="backSide">backside template </param>
        /// <param name="frontSide">frontside template</param>
        /// <param name="session">working session (temporary folder)</param>
        /// <param name="option">export option, frontside or backside or all</param>
        /// <returns></returns>
        private bool Export(List<CertificateData> datas, List<string> certificateContent, int InformationCount, int NameScoreCount, string backSide, string frontSide, Generator.Generator.ExportOption option = Generator.Generator.ExportOption.All)
        {
            log.Debug("Export from list of Json data");
            log.Debug(string.Format("Parameter: data size: {0}, back side: {1}, front side: {2}, generator option: {3}",
                datas != null ? datas.Count : 0, backSide, frontSide, option));

            if (datas == null || datas.Count == 0)
                return false;

            // Parse certificate to model
            certificateModel = new CertificateModel(CertificateUtility.ParseCertificateModel(certificateContent, InformationCount, NameScoreCount));


            CreateWorkingDirectory();
            fileExportList.Clear();
            if (option == Generator.Generator.ExportOption.All)
            {
                CopyDocument(backSide, frontSide);
            }
            else
            {
                if (option == Generator.Generator.ExportOption.Back)
                {
                    CopyBackSide(backSide);
                }
                else
                {
                    CopyFrontSide(frontSide);
                }
            }
            IGenerator generator = Generator.GeneratorFactory.Create(datas[0].GetType());
            // iterator all data in source and export to file
            foreach (CertificateData data in datas)
            {
                generator.Data = data;

                log.Info("Generating data from  source object");
                bool result;
                string outPath;
                switch (option)
                {
                    case Generator.Generator.ExportOption.All:
                        outPath = string.Format("{0}\\{1}_FullSide.pdf", workingFolder, data.CerNo);
                        ((Generator.Generator)generator).OutPutPath = outPath;
                        ((Generator.Generator)generator).Option = option;
                        ((Generator.Generator)generator).DocExt = docExt;
                        ((Generator.Generator)generator).ExcelExt = excelExt;
                        ((Generator.Generator)generator).certificateModel = certificateModel;
                        ((Generator.Generator)generator).WorkingFolder = workingFolder;
                        result = generator.Generate();
                        break;
                    case Generator.Generator.ExportOption.Back:
                        outPath = string.Format("{0}\\{1}_BackSide.pdf", workingFolder, data.CerNo);
                        ((Generator.Generator)generator).OutPutPath = outPath;
                        ((Generator.Generator)generator).Option = option;
                        ((Generator.Generator)generator).DocExt = docExt;
                        ((Generator.Generator)generator).ExcelExt = excelExt;
                        ((Generator.Generator)generator).certificateModel = certificateModel;
                        ((Generator.Generator)generator).WorkingFolder = workingFolder;
                        result = generator.Generate();
                        break;
                    default:
                        outPath = string.Format("{0}\\{1}_FrontSide.pdf", workingFolder, data.CerNo);
                        ((Generator.Generator)generator).OutPutPath = outPath;
                        ((Generator.Generator)generator).Option = option;
                        ((Generator.Generator)generator).DocExt = docExt;
                        ((Generator.Generator)generator).ExcelExt = excelExt;
                        ((Generator.Generator)generator).certificateModel = certificateModel;
                        ((Generator.Generator)generator).WorkingFolder = workingFolder;
                        result = generator.Generate();
                        break;
                }
                // check and add to file List
                if (!result || !File.Exists(outPath))
                    return false;
                log.Debug(string.Format("Export to :{0}", outPath));
               
                fileExportList.Add(outPath);

                list.certNo.Add(data.CerNo);
                list.link.Add(outPath);
            }

            CertificateData temp = datas[0];
            list.classNo = temp.ClassNo;

            #region Close document
            log.Debug(string.Format("Close document"));
            if (option == Generator.Generator.ExportOption.All)
            {
                //generator.CloseDocument();
                generator.CloseAll();
            }
            else
            {
                if (option == Generator.Generator.ExportOption.Back)
                {
                    generator.CloseBackSide();
                }
                else
                {
                    generator.CloseFrontSide();
                }
            }
            #endregion

            if (fileExportList.Count == 0)
            {
                // delete current working directory
                DeleteWorkingDirectory();
                log.Debug("Ended Export from list of Json data (Nothing Exported)");
                return false;
            }
            ZipDocument(fileExportList, string.Format("{0}\\Certificates {1}-{2}-{3}({4}).zip", workingFolder, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, fileExportList.Count));
            //workingPath[session] = string.Format("{0}\\{1}\\Certificates.zip", parentDir, session);
            log.Debug("Ended Export from list of Json data (Exported)");
            return true;
        }

        /// <summary>
        /// New version of download.
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="backSide"></param>
        /// <param name="frontSide"></param>
        /// <param name="option"></param>
        /// <param name="tempFolder">Temporary folder for working </param>
        /// <returns>null if no data is received. Return the content of zip file. This file content pdf certificates.</returns>
        public bool CreateCertificate(List<CertificateData> datas, List<string> certificateContent,int InformationCount, int NameScoreCount, string backSide, string frontSide, string tempFolder, Generator.Generator.ExportOption option = Generator.Generator.ExportOption.All)
        {
            workingState = WorkingState.Working;
            if (datas == null)
            {
                workingState = WorkingState.Error;
                return false;
            }
            this.temporaryFolder = tempFolder;

            if (Export(datas, certificateContent, InformationCount, NameScoreCount, backSide, frontSide, option))
            // call function download file
            {
                return true;
            }

            workingState = WorkingState.Error;
            return false;
        }

        public bool CreateCertificate(string json, List<string> certificateContent,int InformationCount, int NameScoreCount, string backSide, string frontSide, string tempFolder, Generator.Generator.ExportOption option = Generator.Generator.ExportOption.All)
        {
            try
            {
                bool result = CreateCertificate(CertificateUtility.ParseListJsonData(json), certificateContent, InformationCount, NameScoreCount, backSide, frontSide, tempFolder, option);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                workingState = WorkingState.Error;
                return false;
            }
        }

        /// <summary>
        /// This function will compress result and stream to client, then delete all file in working folder
        /// </summary>
        /// <returns></returns>
        public byte[] DownLoadFile(string session)
        {
            if (session != temporaryFolder)
                return null;

            if (!File.Exists(string.Format("{0}\\Certificates.zip", workingFolder)))
            {
                return null;
            }

            byte[] result = GetDocument(string.Format("{0}\\Certificates.zip", workingFolder));

            string[] files = Directory.GetFiles(string.Format("{0}", workingFolder));

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            Directory.Delete(workingFolder);
            temporaryFolder = "";
            //Directory.Delete(workingFolder);
            //workingFolder = null;

            return result;
        }

        /// <summary>
        /// New function of DownLoadFile
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private byte[] DownLoadZipFile(string temporaryFolders /* un used*/)
        {
            if (!File.Exists(string.Format("{0}\\Certificates.zip", workingFolder)))
            {
                workingState = WorkingState.Error;
                return null;
            }

            byte[] result = GetDocument(string.Format("{0}\\Certificates.zip", workingFolder));

            string[] files = Directory.GetFiles(workingFolder);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            Directory.Delete(string.Format("{0}", workingFolder));
            temporaryFolder = "";
            workingState = WorkingState.Success;
            return result;
        }

        /// <summary>
        ///  delete temporary folder
        /// </summary>
        private void DeleteWorkingDirectory()
        {
            log.Debug(string.Format("Delete working directory"));
            string[] files = Directory.GetFiles(workingFolder);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    log.Debug(string.Format("Deleting: {0}", file));
                    File.Delete(file);
                    log.Debug(string.Format("Deleted"));
                }
            }
            Directory.Delete(workingFolder);
        }

        /// <summary>
        /// Compress multi file into a zip file
        /// </summary>
        /// <param name="fileInput">list of input file path</param>
        /// <param name="outPath">path of output file</param>
        /// <returns>true if success , false if file list is null or empty</returns>
        private bool ZipDocument(List<string> fileInput, string outPath)
        {
            log.Debug(string.Format("Zipping documents"));
            if (fileInput == null || fileInput.Count == 0)
                return false;

            log.Debug(string.Format("Zipping documents for : {0}", fileInput.Count));

            using (var stream = new FileStream(outPath, FileMode.Create))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    var combineDoc = new PdfSharp.Pdf.PdfDocument();
                    foreach (var file in fileInput)
                    {
                        log.Debug(string.Format("Add file: {0}", file));
                        var doc = PdfSharp.Pdf.IO.PdfReader.Open(file, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                        //read and add page to combine page
                        for (int i = 0; i < doc.PageCount; i++)
                        {
                            var page2 = combineDoc.AddPage(doc.Pages[i]);
                            if (combineDoc.PageCount != 1)
                            {
                                var graphic = XGraphics.FromPdfPage(page2);
                                graphic.ScaleTransform(combineDoc.Pages[0].Width / page2.Width, 1);
                                graphic.Save();
                            }
                        }

                        var info = new FileInfo(file);
                        archive.CreateEntryFromFile(file, info.Name, CompressionLevel.Optimal);
                    }
                    string allCertificate = string.Format(@"{0}\{1} files AllCertificate {2} {3} {4}.pdf", new FileInfo(fileInput[0]).DirectoryName, fileInput.Count, DateTime.Now.Hour, DateTime.Now.Millisecond, DateTime.Now.Second);

                    combineDoc.Save(allCertificate);
                    archive.CreateEntryFromFile(allCertificate, "AllCertificate.pdf", CompressionLevel.Optimal);
                }
            }

            log.Debug(string.Format("Ended zip document"));

            list.zipLink = outPath;

            return true;
        }

        /// <summary>
        /// get ducument content
        /// </summary>
        /// <param name="documentPath">Path of document</param>
        /// <returns>byte array of file content</returns>
        private Byte[] GetDocument(string documentPath)
        {
            var objfilestream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);
            var len = (int)objfilestream.Length;
            var documentcontents = new Byte[len];
            objfilestream.Read(documentcontents, 0, len);
            objfilestream.Close();

            return documentcontents;
        }

        /// <summary>
        /// create working directory
        /// </summary>
        private void CreateWorkingDirectory()
        {
            log.Debug("Create working directory");
            fileExportList = new List<string>();
            // change api to get temporary folder form client send
            //temporaryFolder = Path.GetRandomFileName();
            workingFolder = string.Format("{0}\\Data\\{1}", parentDir, temporaryFolder);
            log.Debug(string.Format("Creating directory: {0}", workingFolder));
            // create working directory if not exist
            if (!Directory.Exists(workingFolder))
            {
                log.Debug("Directory created");
                Directory.CreateDirectory(workingFolder);
            }
            else
            {
                log.Debug("Directory exists (Ignored task)");
            }
            log.Debug("Ended Create working directory");
        }

    }
}
