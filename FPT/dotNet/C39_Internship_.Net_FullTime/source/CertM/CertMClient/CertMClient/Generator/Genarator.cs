//using GSTCertificateInput.localhost;

//using GSTCertificateInput.GstServiceV3;
#if !DEBUG

#else
using CertMClient.CertMServiceData;
using System.Collections.Generic;
#endif

namespace CertMClient.Generator
{
    public class Genarator
    {
        // service
        private static readonly CertMServiceClient service = new CertMServiceClient();

        /// <summary>
        /// Generate certificate from json string 
        /// </summary>
        /// <param name="jsonSource">source of certificate , must be in result group of certificate</param>
        /// <returns>null if export failed, byte data of zip stream from file of certificate</returns>
        public static bool GeneratorCertificate(string jsonSource, List<string> certificateContent, int InformationCount, int NameScoreCount, string nameTemplate, string temp, CertMClient.CertMServiceData.GeneratorExportOption option)
        {
            return service.createCertificateFromJson(jsonSource, certificateContent, InformationCount, NameScoreCount, nameTemplate, temp, option);          
        }

        //public static bool GeneratorCertificate(CertificateData[] datas, string backSideTemplate, string frontSide, string temp, CertMClient.CertMServiceData.GeneratorExportOption option)
        //{
        //    return service.createCertificate(datas, backSideTemplate, frontSide, temp, option);           
        //}

        public static byte [] GetData()
        {
            return service.DownLoadFile("");
        }

        public static WorkingState GetWorkingState()
        {
            return service.GetWorkingState();
        }
    }
}