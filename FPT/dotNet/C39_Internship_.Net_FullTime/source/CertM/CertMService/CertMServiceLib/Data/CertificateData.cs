using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib.Data
{
    public enum DataType
    {
        Basic, // type of Certificate Data
        DevV2, // Type of Developer version 2
        DevV3, // Type of Developer version 3
        Embedded, // type of embedded 
        TestV2, // Type of tester version 2
        TestV3 // Type of Tester version 3
    }
    public class CertificateData : Interface.ICertificateData
    {
        public DataType DataType { get; set; }
        /// <summary>
        /// Full Name of student
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Class Name    
        /// </summary>
        public string ClassNo
        {
            get;
            set;
        }

        /// <summary>
        /// Student Specialty
        /// </summary>
        public string Specialty
        {
            get;
            set;
        }

        /// <summary>
        /// Student Specialty
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Certifivate Number
        /// </summary>
        public string CerNo
        {
            get;
            set;
        }

        /// <summary>
        /// Date create certificate (signed date)
        /// </summary>
        public string Date
        {
            get;
            set;
        }

        public string Rank
        {
            get;
            set;
        }

        public virtual bool IsValidData()
        {
            return !string.IsNullOrEmpty(CerNo) && !string.IsNullOrEmpty(ClassNo)
               && !string.IsNullOrEmpty(Date) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Specialty);               
        }
    }
}
