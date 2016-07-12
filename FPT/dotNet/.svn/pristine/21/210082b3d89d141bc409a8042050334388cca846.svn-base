using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib.Data
{
    public class CertificateModel
    {
        /// <summary>
        /// List string tên các cột của chứng chỉ mặt trước
        /// </summary>
        public List<string> NameInformation { get; set; }

        /// <summary>
        /// List string tên các cột điểm
        /// </summary>
        public List<string> NameScore { get; set; }
        public int Lenght { get; set; }

        /// <summary>
        /// Lưu giá trị của chứng chỉ
        /// </summary>
        public List<TemplateModel> ValueCertificate { get; set; }
        public CertificateModel()
        {
            NameInformation = new List<string>();
            NameScore = new List<string>();
            ValueCertificate = new List<TemplateModel>();
        }
        public CertificateModel(CertificateModel iCertificate)
        {
            NameInformation = iCertificate.NameInformation;
            NameScore = iCertificate.NameScore;
            LenghtContent();
            ValueCertificate = new List<TemplateModel>();
            foreach (var index in iCertificate.ValueCertificate)
            {
                TemplateModel templateModel = index;
                ValueCertificate.Add(templateModel);
            }
        }

        public void LenghtContent()
        {
            Lenght = NameInformation.Count() + NameScore.Count();
        }
    }
}
