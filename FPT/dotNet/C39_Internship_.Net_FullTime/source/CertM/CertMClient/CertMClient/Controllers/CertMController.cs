using CertMClient.CertMServiceData;
using CertMClient.Converter;
using CertMClient.Models;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CertMClient.Controllers
{
    public class CertMController : Controller
    {
        #region Đỗ Hoàng Phương - UC02: Nhập thông tin chứng chỉ: Thực hiện lưu xuống database và gọi đến UC03, 04.
        /// <summary>
        /// Khai báo khởi tạo các biến dùng chung
        /// </summary>
        public CertMServiceClient service = new CertMServiceClient();
        public static CertMClient.Models.CertificateModel certificate;
        public List<string> certificateContent = new List<string>();
        public static TemplateModel templateModel;
        public static string NameTemplate;

        //Created: AnhLT
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CertMController));//logging
        private static string data;

        private const string CPP = "C++";
        private const string NET = "NET";
        private const string TEST = "TEST";
        private const string JAVA = "JAVA";
        private const string EMBEDDED = "EMBEDDED";
        public static string Status { get; set; }

        /// <summary>
        /// Mở trang web mặc định
        /// </summary>
        /// <Request>GET</Request>
        /// <returns>View: Index</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Màn hình manager: Nhập thông tin chứng chỉ
        /// </summary>
        /// <Request>ActionLinh</Request>
        /// <returns>View: CerMInput chứa model "certificate" </returns>
        public ActionResult CertMInput()
        {
            //add NameInformation
            certificate = new CertMClient.Models.CertificateModel();
            certificate.NameInformation = new List<string>();
            certificate.NameInformation.Add("CerNo");
            certificate.NameInformation.Add("Email");
            certificate.NameInformation.Add("Date");
            certificate.NameInformation.Add("Place");
            certificate.NameInformation.Add("Rank");
            certificate.NameInformation.Add("Specialty");
            certificate.NameInformation.Add("ClassNo");
            certificate.NameInformation.Add("Name");
           
            // add NameScore
            certificate.LenghtContent();

            return View(certificate);
        }

        /// <summary>
        /// Cập nhật lại danh sách các cột điểm trong model certificate
        /// </summary>
        /// <Request>POST</Request>
        /// <returns>View: CertMInput</returns>
        [HttpPost]
        public ActionResult Template()
        {
            var RequestJson = HttpContext.Request;
            RequestJson.InputStream.Seek(0, SeekOrigin.Begin);
            var DataJson = new StreamReader(RequestJson.InputStream).ReadToEnd();
            string certificateContent = JsonConvert.DeserializeObject<string>(DataJson);
            // add NameScore
            NameTemplate = certificateContent;
            bool Result = ReadExcel(NameTemplate);

            return View("CertMInput", certificate);
        }

        /// <summary>
        /// Lấy danh sách template bên service
        /// </summary>
        /// <returns>Trả về danh sách các template cho view</returns>
        public string GetTemplate()
        {
            List<TEMPLATE> listTemplate = service.GetListTemplateName();
            return JsonConvert.SerializeObject(listTemplate);
        }

        /// <summary>
        /// Lưu CSDL xuống database và gọi hàm tạo chứng chỉ
        /// </summary>
        /// <Request>POST</Request>
        /// <returns>Json: Hiện thông báo kết quả thực hiện</returns>
        [HttpPost]
        public ActionResult Submit()
        {
            string Result = "";
            
            try
            {
                var RequestJson = HttpContext.Request;
                RequestJson.InputStream.Seek(0, SeekOrigin.Begin);
                var DataJson = new StreamReader(RequestJson.InputStream).ReadToEnd();
                certificateContent = JsonConvert.DeserializeObject<List<string>>(DataJson);

                //Làm việc với dữ liệu
                if (certificateContent.Count() == 0)
                    return Json("Error: Chưa có dữ liệu. Vui lòng nhập liệu đầy đủ trước khi thực hiện", JsonRequestBehavior.AllowGet);
                else
                    if (certificateContent.Count() < certificate.Lenght * 2)
                        return Json("Error: Dữ liệu chưa đủ số cột", JsonRequestBehavior.AllowGet);

                for (int i = certificate.Lenght * 2; i < certificateContent.Count(); i += certificate.Lenght)       //bắt đầu từ dòng thứ 3 do bỏ dòng 1 là tên cột
                {
                    if (certificateContent[i + 6] != certificateContent[i + 6 - certificate.Lenght])
                        return Json("Error: Vui lòng nhập các chứng chỉ phải cùng một lớp.", JsonRequestBehavior.AllowGet);
                }

                // add value NameInformation and NameScore
                int InformationCount = certificate.NameInformation.Count();
                certificate.NameInformation = new List<string>();
                certificate.NameScore = new List<string>();
                for (int i = 0; i < InformationCount; i++)
                {
                    certificate.NameInformation.Add(certificateContent[i]);
                }

                for (int i = InformationCount; i < certificate.Lenght;i++ )
                {
                    certificate.NameScore.Add(certificateContent[i]);
                }

                //Đưa dữ liệu xuống database

                Result = service.AddCertificate(certificateContent, certificate.NameInformation, certificate.NameScore);
                certificate.ValueCertificate = new List<TemplateModel>();

                for (int i = certificate.Lenght; i < certificateContent.Count(); i += certificate.Lenght)
                {
                    templateModel = new TemplateModel();
                    templateModel.Information = new List<string>();
                    templateModel.Score = new List<double>();
                    int j;
                    for(j = 0; j < certificate.NameInformation.Count(); j++)
                    {
                        templateModel.Information.Add(certificateContent[i + j]);
                    }

                    for(j = 0; j < certificate.NameScore.Count(); j++)
                    {
                        templateModel.Score.Add(double.Parse(certificateContent[i + certificate.NameInformation.Count() + j]));
                    }

                    certificate.ValueCertificate.Add(templateModel);                 
                }

                // tạo chuỗi json
                string jSon = @"{'dataJson':'[{";

                for (int i = certificate.Lenght; i < certificateContent.Count(); i += certificate.Lenght)    //chú ý i=1 do bỏ tên của bảng
                {
                    int j;
                    for (j = 0; j < certificate.NameInformation.Count(); j++)
                    {
                        jSon += @"""" + certificate.NameInformation[j] + @"""" + @":" + @"""" + certificateContent[i + j] + @"""" + ",";
                    }
                    for (j = 0; j < certificate.NameScore.Count() - 1; j++)
                    {
                        jSon += @"""" + certificate.NameScore[j] + @"""" + @":" + @"""" + certificateContent[i + certificate.NameInformation.Count() + j] + @"""" + ",";
                    }
                    if (i + certificate.NameInformation.Count() + j == certificateContent.Count() - 1)
                        jSon += @"""" + certificate.NameScore[j] + @"""" + @":" + @"""" + certificateContent[i + certificate.NameInformation.Count()];
                    else
                        jSon += @"""" + certificate.NameScore[j] + @"""" + @":" + @"""" + certificateContent[i + certificate.NameInformation.Count()] + @"""" + "},{";
                }

                jSon += @"""}]'}" + "ExportOption:FrontSide:true,BackSide:true";
                
             
                // Gọi hàm xuất file chứng chỉ
                string result = Export(jSon);

                if (result.StartsWith("Success"))
                {
                    log.Info("OK");
                    data = result;                    
                    Status = "Success";
                    log.Debug("Call function download");               
                }
                else
                {
                    log.Info("Can not download");                    
                    data = null;
                    Status = "Fail";
                }
            }
            catch
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Gọi hàm ReadExcel trong service lấy list string thêm vào tên cột điểm
        /// Đọc file excel, lấy các cột điểm trong template
        /// Modifiled: Lê Tuấn Anh
        /// Xuất file chứng chỉ
        /// </summary>        
        public string Export(string dataJson)
        {
            if (dataJson == null)
            {
                return "Source string is null";
            }
            else
            {
                //do nothing
            }

            string[] contentSprit = System.Text.RegularExpressions.Regex.Split(dataJson, "ExportOption:");
            // FrontSide:true,BackSide:false
            string[] exportOptions = contentSprit[1].Split(',');
            var exportFront = exportOptions[0].Split(':')[1] == "true";
            var exportBack = exportOptions[1].Split(':')[1] == "true";
            GeneratorExportOption option = exportFront ? exportBack ? GeneratorExportOption.All : GeneratorExportOption.Front : GeneratorExportOption.Back;
            string message = "";
            var serializer = new JavaScriptSerializer();
            string workingDirectory = Server.MapPath(".");
            BaseConverter cvt = new CertificateConverter();
            var allCertificate = cvt.ConvertToList(contentSprit[0]);
            string temp = Path.GetRandomFileName();

            foreach (var certificateList in allCertificate)
            {
                // group certificate and send to server to generate certificate
                foreach (var group in certificateList)
                {
                    string nameTemplate = NameTemplate;
                    //
                    bool result = Generator.Genarator.GeneratorCertificate(serializer.Serialize(group.Value.ToArray()), certificateContent, certificate.NameInformation.Count(), certificate.NameScore.Count(), nameTemplate, temp, option);

                }
            }
            return message.Contains("Error occur") ? message : "Success" + message;
        }


        /// <summary>
        /// Đọc file excel, lấy các cột điểm trong template
        /// </summary>
        /// <param name="iNameTemplate">String: Tên file excel</param>
        /// <returns>Bool: kết quả</returns>
        private bool ReadExcel(string iNameTemplate)
        {
            try
            {
                var scor = service.ReadExcel(iNameTemplate);
                if (scor.Count > 0)
                {
                    certificate.NameScore = new List<string>();
                    foreach (var sc in scor)
                    {
                        certificate.NameScore.Add(sc);
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                certificate.LenghtContent();
            }

            return true;
        }
        #endregion

        //Nguyen Tran Thinh
        //Nguyễn Trần Thịnh
        public ActionResult DownloadCertificate()
        {
            return View("DownloadCertificate");
        }

        public FileContentResult DownloadStudentCert(string certNo)
        {
            try
            {
                //CertMServiceData.CertMServiceClient client = new CertMServiceData.CertMServiceClient();
                //FileStream targetStream = null;
                //long length;
                //Stream sourceStream;
                //string name = client.DownloadStudentCert(certNo, out length, out sourceStream);
                //if (name != null)
                //{
                //    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\" + name;
                //    using (targetStream = new FileStream(filePath, FileMode.Create,
                //              FileAccess.Write, FileShare.None))
                //    {
                //        //read from the input stream in 65000 byte chunks

                //        const int bufferLen = 65000;
                //        byte[] buffer = new byte[bufferLen];
                //        int count = 0;
                //        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                //        {
                //            // save to output stream
                //            targetStream.Write(buffer, 0, count);
                //        }
                //        //sourceStream.CopyTo(targetStream);
                //        targetStream.Close();
                //        sourceStream.Close();
                //    }
                //}

                CertMServiceData.CertMServiceClient client = new CertMServiceData.CertMServiceClient();
                long length;
                Stream sourceStream;
                string name = client.DownloadStudentCert(certNo, out length, out sourceStream);

                MemoryStream ms = new MemoryStream();
                sourceStream.CopyTo(ms);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = name,
                    Inline = false,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(ms.ToArray(), "application/pdf");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DownloadClassCert(string classNo)
        {
            try
            {
                CertMServiceData.CertMServiceClient client = new CertMServiceData.CertMServiceClient();
                FileStream targetStream = null;
                long length;
                Stream sourceStream;
                string name = client.DownloadClassCert(classNo, out length, out sourceStream);
                if (name != null)
                {
                    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\" + name;
                    using (targetStream = new FileStream(filePath, FileMode.Create,
                              FileAccess.Write, FileShare.None))
                    {
                        //read from the input stream in 65000 byte chunks

                        const int bufferLen = 65000;
                        byte[] buffer = new byte[bufferLen];
                        int count = 0;
                        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                        {
                            // save to output stream
                            targetStream.Write(buffer, 0, count);
                        }
                        //sourceStream.CopyTo(targetStream);
                        targetStream.Close();
                        sourceStream.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        [HttpGet]
        public ActionResult DownloadCert(string code)
        {
            try
            {
                CertMServiceData.CertMServiceClient client = new CertMServiceData.CertMServiceClient();
                if (!code.Substring(0, 2).Equals("C3"))
                {
                    DownloadStudentCert(code);
                }
                else
                {
                    DownloadClassCert(code);
                }
            }
            catch (Exception ex)
            {
                return Content("Đã có lỗi xảy ra!" + ex.Message);
            }

            return Content("Download thành công!");
        }

        [HttpGet]
        public ActionResult DownloadAfterInput()
        {
            //string classNo = certificate.ValueCertificate[0].Information[6];
            //DownloadClassCert(classNo);
            //return View("CertMInput", certificate);

            string classNo = certificate.ValueCertificate[0].Information[6];
            CertMServiceData.CertMServiceClient client = new CertMServiceData.CertMServiceClient();
            long length;
            Stream sourceStream;
            string name = client.DownloadClassCert(classNo, out length, out sourceStream);

            MemoryStream ms = new MemoryStream();
            sourceStream.CopyTo(ms);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = name,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(ms.ToArray(), "application/pdf");
        }
    }
}