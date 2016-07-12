using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CertMClient.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        /// <summary>
        /// Trả về trang tìm kiếm
        /// </summary>
        /// <returns></returns>
        public ActionResult viewSearch()
        {
            return View();
        }
        /// <summary>
        /// Trả về trang kết quả search theo tham số
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <param name="Rank"></param>
        /// <param name="Place"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public PartialViewResult viewResultSearch(int Option, string Value, string Rank, string Place, int Page, int PageSize)
        {
            //New mới một service để truy xuất dữ liệu
            CertMServiceData.CertMServiceClient obj = new CertMServiceData.CertMServiceClient();
            //Bắt Exception nếu service không thể kết nối hoặc có lỗi bất ngờ
            try
            {
                //new mới một ls danh sách các chứng chỉ
                List<CertMServiceData.CERTIFICATE> lsCert = obj.SearchCert(Option, Value, Rank, Place, Page, PageSize);
                return PartialView(lsCert);
            }
            catch (Exception e)
            {
                return PartialView(new List<CertMServiceData.CERTIFICATE>());
            }
        }
        // GET: Search
        /// <summary>
        /// Trả về trang tìm kiếm của teacher
        /// </summary>
        /// <returns></returns>
        public ActionResult viewSearchTeacher()
        {
            return View();
        }
        /// <summary>
        /// Trả về trang kết quả search theo tham số của teacher
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <param name="Rank"></param>
        /// <param name="Place"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public PartialViewResult viewResultSearchTeacher(int Option, string Value, string Rank, string Place, int Page, int PageSize)
        {
            //New mới một service để truy xuất dữ liệu
            CertMServiceData.CertMServiceClient obj = new CertMServiceData.CertMServiceClient();
            //Bắt Exception nếu service không thể kết nối hoặc có lỗi bất ngờ
            try
            {
                //new mới một ls danh sách các chứng chỉ
                List<CertMServiceData.CERTIFICATE> lsCert = obj.SearchCert(Option, Value, Rank, Place, Page, PageSize);
                return PartialView(lsCert);
            }
            catch (Exception e)
            {
                return PartialView(new List<CertMServiceData.CERTIFICATE>());
            }
        }
        /// <summary>
        /// Trả về trang coi kết quả học tập của người học có mã chứng chỉ là CertNo
        /// </summary>
        /// <param name="CertNo"></param>
        /// <returns></returns>
        public PartialViewResult ViewDetail(string CertNo)
        {
            //Tạo mới một service 
            CertMServiceData.CertMServiceClient obj = new CertMServiceData.CertMServiceClient();
            //Bắt ngoại lệ trên service khi gặp lỗi
            try
            {
                return PartialView(obj.GetScoreBoard(CertNo));
            }
            catch (Exception e)
            {
                return PartialView(new List<CertMServiceData.SCOREBOARD>());
            }
        }
        /// <summary>
        /// Trả về view của trang xóa lớp 
        /// </summary>
        /// <returns></returns>
        public ViewResult viewDeleteClass()
        {
            //trả về trang viewDeleteClass
            return View();
        }
        /// <summary>
        /// Hàm xóa Lớp học gồm : Xóa lớp , xóa chứng chỉ nằm trong lớp, xóa bảng điểm nằm trong chứng chỉ.
        /// </summary>
        /// <param name="ClassNo"></param>
        /// <returns></returns>
        public bool DeleteClass(string ClassNo)
        {
            // tạo mới một service
            CertMServiceData.CertMServiceClient obj = new CertMServiceData.CertMServiceClient();
            try
            {
                //trả về true or fasle khi xóa được or không class có mã lớp là classNo nhập vào 
                return obj.DeleteClass(ClassNo);
            }
            catch (Exception e)
            {
                //trả về false nếu có lỗi Exception xảy ra
                return false;
            }
        }
        /// <summary>
        /// Hàm lấy ra số lượng trang có thể phân theo số chứng chỉ tìm kiếm
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int CountPageCertMSearch(int Option, string Value, string Rank, string Place, int PageSize)
        {
            //New mới một service để truy xuất dữ liệu
            CertMServiceData.CertMServiceClient obj = new CertMServiceData.CertMServiceClient();
            try
            {
                //Trả lại số lượng tìm kiếm
                return obj.SizePageSearch(Option, Value, Rank, Place, PageSize);
            }
            catch (Exception e)
            {
                //Gặp lỗi trong quá trình truy xuất Service trả về 0
                return 1;
            }
        }
    }
}