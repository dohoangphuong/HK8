using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CertMService
{
    [DataContract]
    public class TemplateModel
    {
        [DataMember]
        public List<string> Information { get; set; }
        [DataMember]
        public List<double> Score { get; set; }

        public TemplateModel()
        {
            Information = new List<string>();
            Score = new List<double>();
        }
    }
}