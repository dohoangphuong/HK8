using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace CertMService
{
    [DataContract(IsReference = true)]
    public class CertificateModel
    {
        [DataMember]
        public IList<string> NameInformation { get; set; }
        [DataMember]
        public IList<string> NameScore { get; set; }
        [DataMember]
        public int Lenght { get; set; }
        [DataMember]
        public IList<TemplateModel> ValueCertificate { get; set; }
        
        public CertificateModel()
        {
            NameInformation = new List<string>();
            NameScore = new List<string>();
            ValueCertificate = new List<TemplateModel>();
        }
      
        public void LenghtContent()
        {
            Lenght = NameInformation.Count() + NameScore.Count();
        }
    }
}