using CertMServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CertMService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICertMService" in both code and config file together.
    [ServiceContract]
    public interface ICertMService
    {
        [OperationContract]
        int SizePageSearch(int Option, string Value, string Rank, string Place, int PageSize);
        [OperationContract]
        List<CERTIFICATE> SearchCert(int Option, string Value, string Rank, string Place, int Page, int PageSize);
        [OperationContract]
        List<SCOREBOARD> GetScoreBoard(string CertNo);
        [OperationContract]
        bool DeleteClass(string ClassNo);

        //Modifiled: Đỗ Hoàng Phuong
        [OperationContract]
        string AddCertificate(List<string> CertificateContent, List<string> NameInformation, List<string> NameScore);
        [OperationContract]
        List<string> ReadExcel(string NameTemplate);
        [OperationContract]
        List<TEMPLATE> GetListTemplateName();

        // Modifiled: Lê Tuấn Anh
        // Date: 01/04/2016
        [OperationContract]
        bool createCertificateFromJson(string jSon, List<string> certificateContent, int InformationCount, int NameScoreCount, string nameTemplate, string tempFolder, CertMServiceLib.Generator.Generator.ExportOption option);

        [OperationContract]
        byte[] DownLoadFile(string session);

        [OperationContract]
        WorkingState GetWorkingState();

        [OperationContract]
        void Close();

        //thuan
        [OperationContract]
        bool AddAccount(ACCOUNT newaccount);
        [OperationContract]
        bool CheckAccount(string email);

        [OperationContract]
        string GetTypeAccount(string email);

        [OperationContract]
        void AddAccountStudent(string email);

        [OperationContract]
        List<ACCOUNT> getAccounts();

        //[OperationContract]
        //IEnumerable<ACCOUNT> getAccount(string email);

        [OperationContract]
        void modifyAccount(ACCOUNT account);

        //Nguyễn Trần Thịnh
        [OperationContract]
        void UploadFile(RemoteFileInfo request);

        [OperationContract]
        bool CheckTemplateName(String name);

        [OperationContract]
        bool SaveToDatabase(String name, string file1, string file2);

        [OperationContract]
        string GetEndFilePath(string name);

        [OperationContract]
        List<string> GetListCertOfClass(string classNo);

        [OperationContract]
        RemoteFileInfo DownloadStudentCert(DownloadRequest request);

        [OperationContract]
        RemoteFileInfo DownloadClassCert(DownloadRequest request);
         


    }

    //Nguyen Tran Thinh
    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public long Length;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember]
        public string Code;
    }
}
