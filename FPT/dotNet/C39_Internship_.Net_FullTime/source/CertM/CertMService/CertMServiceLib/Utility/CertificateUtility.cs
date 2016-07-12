using CertMServiceLib.Data;
using log4net;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CertMServiceLib
{
    /// <summary>
    /// contain some thethod for parsing from Json Object
    /// </summary>
    public class CertificateUtility
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Certificate));
        /// <summary>
        /// Parse mark data from json string
        /// </summary>
        /// <param name="jsonSouce"></param>
        /// <returns></returns>
        //public static DevCertificateData GenerateDevCertificateData(string jsonSouce)
        //{
        //    return  Newtonsoft.Json.JsonConvert.DeserializeObject<DevCertificateData>(jsonSouce);
        //}

        
        /// <summary>
        /// Created: Le Tuan Anh
        /// Date: 01/04/2016
        /// 
        /// parse to CertificateModel from certificateContent List String
        /// </summary>
        /// <param name="certificateContent">certificate content to parse</param>
        /// <returns>CertificateModel</returns>
        public static CertificateModel ParseCertificateModel(List<string> certificateContent, int InformationCount, int NameScoreCount)
        {
            CertificateModel certificateModel = new CertificateModel();
            TemplateModel templateModel = new TemplateModel();
            log.Debug(string.Format("Parse certificate from json source"));
            if (certificateContent == null)
            {
                log.Error("Json source is null");
                throw new ArgumentNullException("Source Certificate string is null");
            }

            // add value NameInformation and NameScore
            
            for (int i = 0; i < InformationCount; i++)
            {
                certificateModel.NameInformation.Add(certificateContent[i]);
            }

            for (int i = InformationCount; i < InformationCount + NameScoreCount; i++)
            {
                certificateModel.NameScore.Add(certificateContent[i]);
            }
            certificateModel.LenghtContent();

            for (int i = InformationCount + NameScoreCount; i < certificateContent.Count; i += InformationCount + NameScoreCount)
            {
                templateModel = new TemplateModel();
                templateModel.Information = new List<string>();
                templateModel.Score = new List<double>();
                int j;
                for (j = 0; j < InformationCount; j++)
                {
                    templateModel.Information.Add(certificateContent[i + j]);
                }

                for (j = 0; j < NameScoreCount; j++)
                {
                    templateModel.Score.Add(double.Parse(certificateContent[i + InformationCount + j]));
                }

                certificateModel.ValueCertificate.Add(templateModel);
            }
            return certificateModel;
        }

        /// <summary>
        /// parse to CertificateData from json Source String
        /// </summary>
        /// <param name="jsonSource">source to parse</param>
        /// <returns></returns>
        public static CertificateData ParseCertificateData(string jsonSource)
        {
            log.Debug(string.Format("Parse certificate from json source"));
            if (jsonSource == null)
            {
                log.Error("Json source is null");
                throw new ArgumentNullException("Source Certificate string is null");
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<CertificateData>(jsonSource);
        }

        /// <summary>
        /// convert certificateData object to json string
        /// </summary>
        /// <param name="source"> data to convert</param>
        /// <returns>string contain jobject parser if success, throw ArgumentNullException if source is null</returns>
        public static string Certificate2JsonString(CertificateData source)
        {
            log.Debug("Certificate2JsonString()");
            if (source == null)
            {
                log.Error("Source is null");
                throw new ArgumentNullException("Source Certificate data is null");
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// Convert Mark data to json string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        //public string Mark2JsonString(DevCertificateData source)
        //{
        //    log.Debug("Mark2JsonString()");
        //    if (source == null)
        //    {
        //        log.Error("Source is null");
        //        throw new ArgumentNullException("Source Mark is null");
        //    }

        //    return Newtonsoft.Json.JsonConvert.SerializeObject(source);
        //}

        /// <summary>
        /// merge 2 pdf file to one file
        /// </summary>
        /// <param name="path1">path to first file</param>
        /// <param name="path2">path to second file</param>
        /// <param name="pathOut">path to combine file</param>
        /// <returns>none</returns>
        public static void CombinePdf(string path1, string path2, string pathOut)
        {
            log.Debug("CombinePdf()");
            log.Debug(string.Format("Parameter: Path1: {0}, Path2: {1}, Out path: {2}", path1, path2, pathOut));
            try
            {
                using (var doc1 = PdfSharp.Pdf.IO.PdfReader.Open(path1, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                using (var doc2 = PdfSharp.Pdf.IO.PdfReader.Open(path2, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                using (var combineDoc = new PdfSharp.Pdf.PdfDocument())
                {
                    //add doc1 to out page
                    for (int i = 0; i < doc1.PageCount; i++)
                    {
                        combineDoc.AddPage(doc1.Pages[i]);
                    }
                    var page2 = combineDoc.AddPage(doc2.Pages[0]);
                    var graphic = XGraphics.FromPdfPage(page2);
                    graphic.ScaleTransform(doc1.Pages[0].Width / page2.Width, 1);
                    page2.Width = doc1.Pages[0].Width;
                    graphic.Save();
                    combineDoc.Save(pathOut);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return;
            }
            log.Debug("Ended CombinePdf");

            return;
        }

        public void DrawHeaderImage(string filePath, string imagePath, int x, int y)
        {
            DrawHeaderImage(filePath, imagePath, new Point(x, y));
        }

        public void DrawHeaderImage(string filePath, string imagePath, Point position)
        {
            try
            {
                using (var doc = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Modify))
                {
                    var page = doc.Pages[0].Clone();
                    var graphic = XGraphics.FromPdfPage(doc.Pages[0]);
                    XImage img = XImage.FromFile(imagePath);
                    graphic.DrawImage(img, position);
                    graphic.Save();
                    doc.Pages[0].Stream = page.Stream;
                    doc.Save(filePath + "Copy.pdf");
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }

        public static List<CertificateData> ParseListJsonData(string json)
        {
            log.Debug("ParseListJsonData()");
            var datas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CertificateData>>(json);
            if (datas == null || datas.Count == 0)
                return null;

            List<CertificateData> result = new List<CertificateData>();
            log.Debug(string.Format("Data Type: {0}", datas[0].DataType));
            if (datas[0].DataType == DataType.Basic)
            {
                var dev3Datas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CertificateData>>(json);
                foreach (CertificateData data in dev3Datas)
                {
                    result.Add(data);
                }
            }
            else
            {
                // do nothing
            }
            log.Debug("Ended ParseListJsonData");
            return result;
        }
    }
}
