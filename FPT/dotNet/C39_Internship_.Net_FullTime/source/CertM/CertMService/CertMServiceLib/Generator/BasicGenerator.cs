using CertMServiceLib.Data;
using CertMServiceLib.ExcelAPI;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib.Generator
{
    public class BasicGenerator : Generator
    {
        private const string CLASS_NAME = "C6"; //vị trí class name trong file excel
        private const string SPESIALTY = "C8"; // vị trí specialty trong file excel
        private const string STUDENT_NAME = "C7"; // vị trí student name trong file excel
        public BasicGenerator()
        {
            Version = "Ver - 1.0";
        }

        /// <summary>
        /// Modifiled: Le Tuan Anh
        /// Date: 01/04/2016
        /// 
        /// Keep generate certificate for old version of certificate 
        /// </summary>
        /// <param name="jsonSource"></param>
        /// <returns></returns>
        protected override bool FillBackSideCerticicate(ref string cellChanges)
        {
            log.Debug("FillBackSideCerticicate()");
            if (Option == ExportOption.Front)
                return true;

            var studentInfo = (CertificateData)Data;
            
            //check if data is enought to export
            if (!studentInfo.IsValidData())
            {
                log.Error("Data is invalid");
                errorString += Environment.NewLine + "Input data is invalid";
                throw new ArgumentException("Invalid data");
            }

            if (backSideTemplate == null || backSideTemplate.IsClosed)
            {
                backSideTemplate = excelAppInstance.Open(String.Format("{0}\\backSide{1}", WorkingFolder, ExcelExt));
            }

            try
            {
                // read sheet BackSide
                if (backSideSheet == null)
                {
                    backSideSheet = backSideTemplate.GetSheet(1);
                }

                CertificateData devdata = (CertificateData)Data;

                backSideSheet.SetCellValue(CLASS_NAME, devdata.ClassNo); // set class Name
                backSideSheet.SetCellValue(SPESIALTY, devdata.Specialty); // set specialty
                // read sheet data
                WorkSheet dataSheet = backSideTemplate.GetSheet(2);

                log.Info("Writing result for " + devdata.Name); // log

                backSideSheet.SetCellValue(STUDENT_NAME, devdata.Name); // set student name

 
                int rowCount = dataSheet.DataRowCount; // number row
                int colCount = dataSheet.DataColCount; // number column
                char column = 'B';

                for (int i = 1; i < colCount; i++)
                {
                    try
                    {                        
                        column = (char)(column + 1);
                        // lấy tên cột điểm trong bảng excel
                        string ContentColumn = Replace(dataSheet.GetCellValue(column + "7").ToString());                       

                        if (ContentColumn == null)
                            break;

                        Double score = -1;
                        for (int k = 0; k < certificateModel.ValueCertificate.Count(); k++)
                        {
                            if (devdata.CerNo == certificateModel.ValueCertificate[k].Information[0])
                            {
                                for (int l = 0; l < certificateModel.NameScore.Count(); l++)
                                {
                                    if (Replace(certificateModel.NameScore[l]).Equals(ContentColumn)) // so sánh tên cột điểm trong certificate model với tên cột điểm trong file excel
                                    {
                                        score = certificateModel.ValueCertificate[k].Score[l]; // lấy điểm trong certificate model
                                        for (int j = 0; j < rowCount; j++)
                                        {
                                            string ContentRow = null;
                                            ContentRow = (string)dataSheet.GetCellValue(column + (8 + j).ToString()); // lấy giá trị của ô trong bảng excel
                                            if (!string.IsNullOrEmpty(ContentRow) && score != -1)
                                            {
                                                // điền dữ liệu vào cột
                                                FillCell(dataSheet, column + (8 + j).ToString(), score, ref cellChanges);
                                                break;
                                            }                                           
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    catch { }
                }

                log.Info("Result setted");

                return true;
            }
            catch (Exception ex)
            {
                errorString += Environment.NewLine + ex.Message;
                log.Error(ex.Message);
            }

            log.Debug("FillBackSideCerticicate()");
            return false;
            // load template and 
        }

        /// <summary>
        /// Modifiled: Le Tuan Anh
        /// Date: 01/04/2016
        /// 
        /// Hàm thay thế các ký tự đặc biệt trong chuỗi
        /// </summary>
        /// <param name="chuoi"> chuoi string cần thay thế</param>
        /// <returns>string</returns>
        public string Replace(string chuoi)
        {
            string[] kyTu = { " ", ",", "&", ".", "+", "(", ")", "\r", "\n" };
            chuoi.Trim();
            for(int i=0; i<kyTu.Length; i++)
            {
                chuoi = chuoi.Replace(kyTu[i], "");
            }
            return chuoi;
        }
    }
}
