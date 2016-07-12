using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.Web;
using CertMServiceLib;

namespace CertMService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CertMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CertMService.svc or CertMService.svc.cs at the Solution Explorer and start debugging.
    public class CertMService : ICertMService
    {

        private CERTIFICATE_MANAGEMENTEntities dbcontext = new CERTIFICATE_MANAGEMENTEntities();
        //public CERTIFICATE_MANAGEMENTEntities dataCertM = new CERTIFICATE_MANAGEMENTEntities();
        public static CertificateModel Certificate = new CertificateModel();
        //hàm search chứng chỉ theo option 1 là search mã chứng chỉ, 2 là search theo mã lớp , 3 là theo tên học viên

        // AnhLT
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CertMService)); // logging
        private CertMServiceLib.Certificate certificate; // certificate library tool
        public CertMService()
        {
            log4net.Config.XmlConfigurator.Configure();
            certificate = new CertMServiceLib.Certificate(HttpContext.Current.Server.MapPath("."));
            log.Info("Start Service");
        }
        //hàm search chứng chỉ theo option 1 là search mã chứng chỉ, 2 là search theo mã lớp , 3 là theo tên học viên với các điều kiện Rank và Place
        //sau đó phân trang cho danh sách kết quả theo page và pagesize 
        /// <summary>
        /// Tìm Kiếm chứng chỉ
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public List<CERTIFICATE> SearchCert(int Option, string Value, string Rank, string Place, int Page, int PageSize)
        {
            List<CERTIFICATE> lsCert = new List<CERTIFICATE>();
            switch (Option)
            {
                //search theo mã chứng chỉ
                case 1:
                    {
                        using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
                        {
                            //cấu hình Proxy
                            db.Configuration.ProxyCreationEnabled = false;
                            try
                            {
                                //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào với các điều kiện Rank,Place
                                if ((Rank != "ALL" && Rank != "undefined" && Rank != "") || (Place != "undefined" && Place != ""))
                                {
                                    var data = from cert in db.CERTIFICATE where cert.CertNo.Contains(Value) && cert.Rank == Rank && cert.Place.Contains(Place) select cert;
                                    lsCert = data.OrderBy(x => x.CertNo).Skip<CERTIFICATE>((Page - 1) * PageSize).Take<CERTIFICATE>(PageSize).ToList();
                                }
                                else
                                {
                                    //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào không có các điều kiện 
                                    var data = from cert in db.CERTIFICATE where cert.CertNo.Contains(Value) select cert;
                                    lsCert = data.OrderBy(x => x.CertNo).Skip<CERTIFICATE>((Page - 1) * PageSize).Take<CERTIFICATE>(PageSize).ToList();
                                }
                            }
                            catch (Exception e)
                            {
                                return lsCert;
                            }
                        }
                    }
                    break;
                //search theo mã lớp
                case 2:
                    {
                        using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
                        {
                            db.Configuration.ProxyCreationEnabled = false;
                            try
                            {
                                //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào với các điều kiện 
                                if ((Rank != "ALL" && Rank != "undefined" && Rank != "") || (Place != "undefined" && Place != ""))
                                {
                                    var data = from cert in db.CERTIFICATE where cert.ClassNo.Contains(Value) && cert.Rank == Rank && cert.Place.Contains(Place) select cert;
                                    lsCert = data.OrderBy(x => x.CertNo).Skip<CERTIFICATE>((Page - 1) * PageSize).Take<CERTIFICATE>(PageSize).ToList();
                                }
                                else
                                {
                                    //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào không có các điều kiện 
                                    var data = from cert in db.CERTIFICATE where cert.ClassNo.Contains(Value) select cert;
                                    lsCert = data.OrderBy(x => x.CertNo).Skip<CERTIFICATE>((Page - 1) * PageSize).Take<CERTIFICATE>(PageSize).ToList();
                                }
                            }
                            catch (Exception e)
                            {
                                return lsCert;
                            }
                        }
                    }
                    break;
                //Search theo tên học viên
                case 3:
                    {
                        using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
                        {
                            db.Configuration.ProxyCreationEnabled = false;
                            try
                            {
                                //Lấy ra những chứng chỉ có Tên người đạt chứng chỉ chứa Value 
                                //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào với các điều kiện 
                                if ((Rank != "ALL" && Rank != "undefined" && Rank != "") || (Place != "undefined" && Place != ""))
                                {
                                    var data = from cert in db.CERTIFICATE where cert.Name.Contains(Value) && cert.Rank == Rank && cert.Place.Contains(Place) select cert;
                                    lsCert = data.OrderBy(x => x.CertNo).Skip<CERTIFICATE>((Page - 1) * PageSize).Take<CERTIFICATE>(PageSize).ToList();
                                }
                                else
                                {
                                    //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào không có các điều kiện 
                                    var data = from cert in db.CERTIFICATE where cert.Name.Contains(Value) select cert;
                                    lsCert = data.OrderBy(x => x.CertNo).Skip<CERTIFICATE>((Page - 1) * PageSize).Take<CERTIFICATE>(PageSize).ToList();
                                }
                            }
                            catch (Exception e)
                            {
                                return lsCert;
                            }
                        }
                    }
                    break;
            }
            return lsCert;
        }
        //Hàm lấy bảng điểm theo mã chứng chỉ
        /// <summary>
        /// Lấy bảng điểm theo mã mã chứng chỉ
        /// </summary>
        /// <param name="CertNo"></param>
        /// <returns></returns>
        public List<SCOREBOARD> GetScoreBoard(string CertNo)
        {
            //tạo ra list Bảng điểm
            List<SCOREBOARD> lsScoreBoard = new List<SCOREBOARD>();
            using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
            {
                //Cấu hình Proxy
                db.Configuration.ProxyCreationEnabled = false;
                try
                {
                    //lấy ra những dòng của bảng điểm có mã chứng chỉ là CertNo
                    var data = from ScoreBoard in db.SCOREBOARD where ScoreBoard.CertNo == CertNo select ScoreBoard;
                    lsScoreBoard = data.ToList<SCOREBOARD>();
                }
                catch
                {
                    return lsScoreBoard;
                }
            }

            return lsScoreBoard;
        }
        //Hàm lấy số lượng trang có thể có trong 1 kết quả search
        /// <summary>
        /// Hàm lấy số lượng trang có thể có trong 1 kết quả search khi biết được số lượng dòng của một trang
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <param name="Rank"></param>
        /// <param name="Place"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public int SizePageSearch(int Option, string Value, string Rank, string Place, int PageSize)
        {
            List<CERTIFICATE> lsCert = new List<CERTIFICATE>();
            switch (Option)
            {
                //search theo mã chứng chỉ
                case 1:
                    {
                        using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
                        {
                            //cấu hình Proxy
                            db.Configuration.ProxyCreationEnabled = false;
                            try
                            {
                                //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào với các điều kiện Rank,Place
                                if ((Rank != "ALL" && Rank != "undefined" && Rank != "") || (Place != "undefined" && Place != ""))
                                {
                                    var data = from cert in db.CERTIFICATE where cert.CertNo.Contains(Value) && cert.Rank == Rank && cert.Place.Contains(Place) select cert;
                                    //tính ra số trang khi có pagesize
                                    int tem = data.Count();
                                    return tem / PageSize + (tem % PageSize > 0 ? 1 : 0);
                                }
                                else
                                {
                                    //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào không có các điều kiện 
                                    var data = from cert in db.CERTIFICATE where cert.CertNo.Contains(Value) select cert;
                                    //tính ra số trang khi có pagesize
                                    int tem = data.Count();
                                    return tem / PageSize + (tem % PageSize > 0 ? 1 : 0);
                                }
                            }
                            catch (Exception e)
                            {
                                return 0;
                            }
                        }
                    }
                    break;
                //search theo mã lớp
                case 2:
                    {
                        using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
                        {
                            db.Configuration.ProxyCreationEnabled = false;
                            try
                            {
                                //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào với các điều kiện 
                                if ((Rank != "ALL" && Rank != "undefined" && Rank != "") || (Place != "undefined" && Place != ""))
                                {
                                    var data = from cert in db.CERTIFICATE where cert.ClassNo.Contains(Value) && cert.Rank == Rank && cert.Place.Contains(Place) select cert;
                                    //tính ra số trang khi có pagesize
                                    int tem = data.Count();
                                    return tem / PageSize + (tem % PageSize > 0 ? 1 : 0);
                                }
                                else
                                {
                                    //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào không có các điều kiện 
                                    var data = from cert in db.CERTIFICATE where cert.ClassNo.Contains(Value) select cert;
                                    //tính ra số trang khi có pagesize
                                    int tem = data.Count();
                                    return tem / PageSize + (tem % PageSize > 0 ? 1 : 0);
                                }
                            }
                            catch (Exception e)
                            {
                                return 0;
                            }
                        }
                    }
                    break;
                //Search theo tên học viên
                case 3:
                    {
                        using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
                        {
                            db.Configuration.ProxyCreationEnabled = false;
                            try
                            {
                                //Lấy ra những chứng chỉ có Tên người đạt chứng chỉ chứa Value 
                                //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào với các điều kiện 
                                if ((Rank != "ALL" && Rank != "undefined" && Rank != "") || (Place != "undefined" && Place != ""))
                                {
                                    var data = from cert in db.CERTIFICATE where cert.Name.Contains(Value) && cert.Rank == Rank && cert.Place.Contains(Place) select cert;
                                    //tính ra số trang khi có pagesize
                                    int tem = data.Count();
                                    return tem / PageSize + (tem % PageSize > 0 ? 1 : 0);
                                }
                                else
                                {
                                    //Lấy ra những chứng chỉ có mã chứng chỉ chứa chuỗi Value nhập vào không có các điều kiện 
                                    var data = from cert in db.CERTIFICATE where cert.Name.Contains(Value) select cert;
                                    //tính ra số trang khi có pagesize
                                    int tem = data.Count();
                                    return tem / PageSize + (tem % PageSize > 0 ? 1 : 0);
                                }
                            }
                            catch (Exception e)
                            {
                                return 0;
                            }
                        }
                    }
                    break;
            }
            return 0;
        }

        #region Đỗ  Hoàng Phương
        /// <summary>
        /// Kiểm tra và add Certificate xuống database
        /// </summary>
        /// <param name="CertificateContent">List<string>: Nội dung của Certificate</param>
        /// <param name="NameInformation">List<string>: Danh sách tên thông tin Certificate</param>
        /// <param name="NameScore">List<string>: Danh sách tên cột điểm của Certificate</param>
        /// <returns>String: Kết quả thực hiện</returns>
        public string AddCertificate(List<string> CertificateContent, List<string> NameInformation, List<string> NameScore)
        {
            int row = 0;
            try
            {
                CERTIFICATE_MANAGEMENTEntities dataCertM = new CERTIFICATE_MANAGEMENTEntities();
                List<ACCOUNT> lsAccount = new List<ACCOUNT>();
                List<CERTIFICATE> lsCert = new List<CERTIFICATE>();
                List<CLASS> lsClass = new List<CLASS>();
                dataCertM.Configuration.ProxyCreationEnabled = false;

                try
                {
                    string ClassNo, CertNo, Email, DateCert;
                    int Lenght = NameInformation.Count() + NameScore.Count();
                    ClassNo = CertificateContent[6 + Lenght];
                    var DataClass = from Class in dataCertM.CLASS where Class.ClassNo == ClassNo select Class;
                    lsClass = DataClass.ToList<CLASS>();   // K ĐƯỢC CÓ TRÙNG LỚP

                    if (lsClass.Count() < 1)
                    {
                        CLASS classCertM = new CLASS();
                        classCertM.ClassNo = ClassNo;
                        dataCertM.CLASS.Add(classCertM);
                        //dataCertM.SaveChanges();

                        for (int i = Lenght; i < CertificateContent.Count(); i = i + NameInformation.Count() + NameScore.Count())
                        {
                            //có thể bị trùng tên chứng chỉ
                            try
                            {
                                CertNo = CertificateContent[i + 0];
                                Email = CertificateContent[i + 1];

                                var DataFalse = from Cert in dataCertM.CERTIFICATE where Cert.CertNo == CertNo select Cert;
                                lsCert = DataFalse.ToList<CERTIFICATE>();   // K ĐƯỢC CÓ TRÙNG CHỨNG CHỈ
                                var DataTrue = from Account in dataCertM.ACCOUNT where Account.Email == Email select Account;
                                lsAccount = DataTrue.ToList<ACCOUNT>();    // PHẢI CÓ ACCOUNT TRONG HỆ THỐNG

                                if (lsCert.Count() < 1)
                                {
                                    if (lsAccount.Count <= 0)
                                    {
                                        AddAccountStudent(Email);
                                    }
                                        //------Lưu ý: kiểm tra điều kiện-------------------
                                        CERTIFICATE cert = new CERTIFICATE();
                                        cert.CertNo = CertificateContent[i + 0];
                                        cert.Email = CertificateContent[i + 1];
                                        try
                                        {
                                            DateCert = CertificateContent[i + 2];// "01/04/2016";
                                            cert.Date = DateTime.Parse(DateCert);
                                        }catch
                                        {
                                            cert.Date = new DateTime();
                                            //return ("Error: Ngày của dòng " + (row + 2).ToString() + " là " + CertNo.ToString() + " không đúng");
                                        }
                                        cert.Place = CertificateContent[i + 3];
                                        cert.Rank = CertificateContent[i + 4];
                                        cert.Major = CertificateContent[i + 5];
                                        cert.ClassNo = CertificateContent[i + 6];
                                        cert.Name = CertificateContent[i + 7];
                                        dataCertM.CERTIFICATE.Add(cert);
                                       // dataCertM.SaveChanges();

                                        for (int j = 0; j < NameScore.Count(); j++)
                                        {
                                            SCOREBOARD score = new SCOREBOARD();
                                            score.CertNo = cert.CertNo;
                                            score.Category = NameScore[j];              //name score
                                            Double sorceValue=Double.Parse(CertificateContent[i + 8 + j]);
                                            if (sorceValue >= 1 && sorceValue <= 10)
                                            {
                                                score.Mark = sorceValue;   //value score
                                            }
                                            else
                                            {
                                                return "Error: Điểm của chứng chỉ dòng " + (row + 2).ToString() + " là " + sorceValue.ToString() + " không đúng: 1-10";
                                            }
                                            dataCertM.SCOREBOARD.Add(score);
                                        }                                        
                                        row++;
                                   
                                }
                                else
                                {
                                    return ("Error: Mã chứng chỉ của dòng " + (row + 2).ToString() + " là " + CertNo.ToString() + " đã tồn tại");
                                }
                            }
                            catch
                            {
                                return "Error";
                            }
                        }
                        try
                        {
                            dataCertM.SaveChanges();
                        }
                        catch
                        {
                            return ("Error: Dữ liệu bạn đưa vào chưa đúng chuẩn định dạng vui lòng kiểm tra lại!");
                        }
                    }
                    else
                    {
                        return "Error: Tên lớp đã bị trùng vui lòng nhập tên khác.";
                    }
                }
                catch
                {
                    return ("Error: Việc tương tác với database bị lỗi");
                }
            }
            catch
            {
                return ("Error: Thao tác kết nối database bị lỗi");
            }

            return ("Success: Bạn đã tạo được " + row +" chứng chỉ.");
        }

        /// <summary>
        /// Đọc file excel, lấy các cột điểm trong template
        /// </summary>
        /// <param name="Data">String: Đường dẫn đến file excel</param>
        /// <returns>List<tring>: Danh sách tên các cột điểm </returns>
        public List<string> ReadExcel(string NameTemplate)
        {
           string PathData = GetEndFilePath(NameTemplate);
            var DataExcelApp = new Microsoft.Office.Interop.Excel.Application();
            List<string> ListNameScore = new List<string>();

            try
            {
                Workbook ExcelWorkbook = DataExcelApp.Workbooks.Open(PathData);
                _Worksheet ExcelWorkSheet = ExcelWorkbook.Sheets["data"];//tên sheet
                Range ExcelRange = ExcelWorkSheet.UsedRange;

                int ColumnCount = ExcelRange.Columns.Count;
                //  string d = xlWorksheet.Range["C7"].Value;
                char NameColumn = 'B';
                for (int i = 1; i < ColumnCount; i++)
                {
                    try
                    {
                        ColumnCount += 1;
                        NameColumn = (char)(NameColumn + 1);
                        var ContentColumn = ExcelWorkSheet.Range[NameColumn + "7"].Value;
                        if (ContentColumn == null)
                            break;

                        ListNameScore.Add(ContentColumn);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (DataExcelApp.Workbooks != null)
                {
                    ((_Application)DataExcelApp).Quit();
                }
            }
            return ListNameScore;
        }

        /// <summary>
        /// Lấy danh sách template
        /// </summary>
        /// <returns>Danh sách template</returns>
        public List<TEMPLATE> GetListTemplateName()
        {
            List<string> list = new List<string>();
            CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var listquery = from p in db.TEMPLATE
                        select p;

            return listquery.ToList();
        }
        #endregion

        #region Le Tuan Anh
        // Created: AnhLT
        // Date: 01/04/2016
        /// <summary>
        /// Tạo chứng chỉ từ chuỗi json
        /// </summary>
        /// <param name="jSon"><string>: Nội dung của chuỗi json</param>
        /// <param name="CertificateContent">List<string>: Nội dung của Certificate</param>
        /// <param name="InformationCount"><int>: Số lượng cột của chứng chỉ mặt trước</param>
        /// <param name="NameScoreCount"><int>: Số lượng cột điểm của mặt sau</param>
        /// <param name="nameTemplate"><string>: Tên của template tạo chứng chỉ</param>
        /// <param name="tempFolder"><string>: Thư mục lưu chứng chỉ</param>
        /// <param name="option"><ExportOption>: Lựa chọn in chứng chỉ</param>
        /// <returns>bool: Kết quả thực hiện</returns>
        public bool createCertificateFromJson(string jSon, List<string> certificateContent, int InformationCount, int NameScoreCount, string nameTemplate, string tempFolder, CertMServiceLib.Generator.Generator.ExportOption option = CertMServiceLib.Generator.Generator.ExportOption.All)
        {
            try
            {
                CERTIFICATE_MANAGEMENTEntities dbcontext = new CERTIFICATE_MANAGEMENTEntities();
                dbcontext.Configuration.ProxyCreationEnabled = false;

                // recall certificate.cs in GSTCertificateLib to create data.
                // MapPath connect with lib reference.
                if (certificate == null)
                {
                    certificate = new CertMServiceLib.Certificate(HttpContext.Current.Server.MapPath("."));
                }
                else
                {
                    //do nothing
                }
                
                TEMPLATE t = new TEMPLATE();
                t = dbcontext.TEMPLATE.Where(a => a.Name == nameTemplate).SingleOrDefault();

                // get backside and front side template from configuration file - option all.
                var dir = HttpContext.Current.Server.MapPath(".");
                string backSide = dir + t.EndLink;
                string frontSide = dir + t.FrontLink;

                bool result = certificate.CreateCertificate(jSon, certificateContent, InformationCount, NameScoreCount, backSide, frontSide, tempFolder, option);


                //Phần lưu link vào bảng CERTIFICATE
                int length = certificate.list.certNo.Count;
                for (int i = 0; i < length; i++)
                {
                    string certNo = certificate.list.certNo[i];
                    string link = certificate.list.link[i];
                    link = link.Substring(link.LastIndexOf("\\Data"), link.Length - link.LastIndexOf("\\Data"));
                    var query = from p in dbcontext.CERTIFICATE
                                where p.CertNo == certNo
                                select p;
                    if (query.ToList<CERTIFICATE>().Count != 0)
                    {
                        CERTIFICATE certObject = query.ToList<CERTIFICATE>()[0];
                        certObject.Link = link;
                        dbcontext.Entry(certObject).State = System.Data.Entity.EntityState.Modified;
                        dbcontext.SaveChanges();
                    }
                }

                string zipLink = certificate.list.zipLink;
                zipLink = zipLink.Substring(zipLink.LastIndexOf("\\Data"), zipLink.Length - zipLink.LastIndexOf("\\Data"));
                var query1 = from p in dbcontext.CLASS
                             where p.ClassNo == certificate.list.classNo
                             select p;
                if (query1.ToList<CLASS>().Count != 0)
                {
                    CLASS classObject = query1.ToList<CLASS>()[0];
                    classObject.Link = zipLink;
                    dbcontext.Entry(classObject).State = System.Data.Entity.EntityState.Modified;
                    dbcontext.SaveChanges();
                }

                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// Download file from server ( to byte stream) 
        /// file type is Zip type
        /// </summary>
        /// <param name="session"> Name of temporary folder in server</param>
        /// <returns></returns>

        public byte[] DownLoadFile(string session)
        {
            return certificate.DownLoadFile(session);
        }

        public WorkingState GetWorkingState()
        {
            return certificate.State;
        }

        /// <summary>
        /// Close service
        /// </summary>

        public void Close()
        {
            log.Info("Close Service");
            if (certificate != null)
            {
                certificate = null;
            }
        }

        #endregion

        //thuan
        //Return List Account from table ACCOUNT 
        public List<ACCOUNT> getAccounts()
        {
            dbcontext.Configuration.ProxyCreationEnabled = false;
            var data = from acount in dbcontext.ACCOUNT select acount;
            List<ACCOUNT> ac = data.ToList<ACCOUNT>();
            return ac;
        }


        //check account, return true if account esixt
        public bool CheckAccount(string email)
        {
            int flag = 0;
            foreach (var ac in getAccounts())
            {
                if (ac.Email == email)
                    flag = 1;
            }
            if (flag == 1)
                return true;
            else
                return false;
        }

        //return type of account
        public string GetTypeAccount(string email) //get type of account
        {
            string type = "";
            var data = from acount in dbcontext.ACCOUNT where acount.Email == email select acount;

            foreach (var a in data)
            {
                type = a.Type;
            }
            return type;

        }

        // insert a account with defaulttype (type=student)
        public void AddAccountStudent(string email) // insert anonymouse in database wihthout type
        {
            
            ACCOUNT newaccount = new ACCOUNT();
            newaccount.Email = email;
            newaccount.Type = "Student";
            dbcontext.ACCOUNT.Add(newaccount);
            dbcontext.SaveChanges();
        }

        // insert a newaccount
        public bool AddAccount(ACCOUNT newaccount)
        {
            if (!CheckAccount(newaccount.Email))
            {
                dbcontext.ACCOUNT.Add(newaccount);
                dbcontext.SaveChanges();
                return true;
            }

            return false;
        }

        //public IEnumerable<ACCOUNT> getAccount(string email)
        //{
        //    IEnumerable<ACCOUNT> data = from p in dbcontext.ACCOUNT
        //                                where p.Email == email
        //                                select p;
        //    ACCOUNT account = data.ToList()[0];

        //    return data;

        //}

        //modify type of account
        public void modifyAccount(ACCOUNT account)
        {
            dbcontext.Entry(account).State = System.Data.Entity.EntityState.Modified;
            dbcontext.SaveChanges();
        }
        //thuan

        //Nguyễn Trần Thịnh
        public void UploadFile(RemoteFileInfo request)
        {
            FileStream targetStream = null;
            Stream sourceStream = request.FileByteStream;
            //string uploadFolder = @"~\TEMPLATE\";
            string uploadFolder = HostingEnvironment.ApplicationPhysicalPath + "\\Templates";

            //string filePath = Path.Combine(uploadFolder, request.FileName);
            string filePath = uploadFolder + "\\" + request.FileName;

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
                targetStream.Close();
                sourceStream.Close();
                //request.FileByteStream.CopyTo(targetStream);

            }
        }

        public bool CheckTemplateName(string name)
        {
            CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var query = from p in db.TEMPLATE
                        where p.Name == name
                        select p;

            if (query.ToList<TEMPLATE>().Count == 0) return true;
            else return false;
        }

        public bool SaveToDatabase(string name, string file1, string file2)
        {
            try
            {
                CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
                db.Configuration.ProxyCreationEnabled = false;
                TEMPLATE template = new TEMPLATE();
                template.Name = name;
                template.FrontLink = "\\Templates\\" + file1;
                if (file2 != "") template.EndLink = "\\Templates\\" + file2;
                db.TEMPLATE.Add(template);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public string GetEndFilePath(string name)
        {
            CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var query = from p in db.TEMPLATE
                        where p.Name == name
                        select p;

            if (query.ToList<TEMPLATE>().Count != 0)
            {
                return HostingEnvironment.ApplicationPhysicalPath + query.ToList<TEMPLATE>()[0].EndLink;
            }
            else return "";
        }

        public List<string> GetListCertOfClass(string classNo)
        {
            List<string> list = new List<string>();
            CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var query = from p in db.CERTIFICATE
                        where p.ClassNo == classNo
                        select p;

            foreach (CERTIFICATE cert in query.ToList<CERTIFICATE>())
            {
                list.Add(cert.CertNo);
            }

            return list;
        }

        public RemoteFileInfo DownloadStudentCert(DownloadRequest request)
        {
            CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var query = from p in db.CERTIFICATE
                        where p.CertNo == request.Code
                        select p;

            if (query.ToList<CERTIFICATE>().Count == 0) return null;
            else
            {
                string filePath = HostingEnvironment.ApplicationPhysicalPath + query.ToList<CERTIFICATE>()[0].Link;

                RemoteFileInfo result = new RemoteFileInfo();
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

                    // check if exists
                    if (fileInfo.Exists)
                    {
                        // open stream
                        System.IO.FileStream stream = new System.IO.FileStream(filePath,
                                  System.IO.FileMode.Open, System.IO.FileAccess.Read);

                        // return result 
                        result.FileName = fileInfo.Name;
                        result.Length = fileInfo.Length;
                        result.FileByteStream = stream;
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                return result;
            }
        }

        public RemoteFileInfo DownloadClassCert(DownloadRequest request)
        {
            CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var query = from p in db.CLASS
                        where p.ClassNo == request.Code
                        select p;

            if (query.ToList<CLASS>().Count == 0) return null;
            else
            {
                string filePath = HostingEnvironment.ApplicationPhysicalPath + query.ToList<CLASS>()[0].Link;

                RemoteFileInfo result = new RemoteFileInfo();
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

                    // check if exists
                    if (fileInfo.Exists)
                    {
                        // open stream
                        System.IO.FileStream stream = new System.IO.FileStream(filePath,
                                  System.IO.FileMode.Open, System.IO.FileAccess.Read);

                        // return result 
                        result.FileName = fileInfo.Name;
                        result.Length = fileInfo.Length;
                        result.FileByteStream = stream;
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                return result;
            }
        }
        /// <summary>
        /// Hàm xóa Lớp học gồm : Xóa lớp , xóa chứng chỉ nằm trong lớp, xóa bảng điểm nằm trong chứng chỉ.
        /// </summary>
        /// <param name="ClassNo"></param>
        /// <returns></returns>
        public bool DeleteClass(string ClassNo)
        {
            //new mới ra một lisk Certificate
            List<CERTIFICATE> lsCert = new List<CERTIFICATE>();
            using (CERTIFICATE_MANAGEMENTEntities db = new CERTIFICATE_MANAGEMENTEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                //Tìm những chứng chỉ có mã lớp nhập vào
                try
                {
                    //Lấy ra những chứng chỉ có mã lớp nhập vào là ClassNo
                    var data = from cert in db.CERTIFICATE where cert.ClassNo == ClassNo select cert;
                    lsCert = data.ToList<CERTIFICATE>();
                }
                catch (Exception e)
                {
                    return false;
                }
                if (lsCert.Count > 0)
                {
                    //Tìm và xóa bảng điểm có mã trong list chứng chỉ cộng xóa chứng chỉ đó ra khỏi csdl
                    foreach (var certm in lsCert)
                    {
                        try
                        {
                            //Tìm kiếm
                            var data = from scoreboard in db.SCOREBOARD where scoreboard.CertNo == certm.CertNo select scoreboard;
                            var lsScore = data.ToList<SCOREBOARD>();
                            //Xóa
                            foreach (var Score in lsScore)
                            {
                                db.SCOREBOARD.Remove(Score);
                            }
                            db.CERTIFICATE.Remove(certm);
                        }
                        catch (Exception e)
                        {
                            return false;
                        }
                    }
                    //Tìm và xóa những lớp có mã bằng mã nhập vào
                    try
                    {
                        //Tìm kiếm
                        var data = from Class in db.CLASS where Class.ClassNo == ClassNo select Class;
                        var lsClass = data.ToList<CLASS>();
                        //Xóa
                        foreach (var Class in lsClass)
                        {
                            db.CLASS.Remove(Class);
                        }
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                else
                    return false;
                db.SaveChanges();
                return true;
            }
        }
    }
}
