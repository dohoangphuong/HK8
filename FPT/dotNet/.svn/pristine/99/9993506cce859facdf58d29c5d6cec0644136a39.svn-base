using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertMClient.Data
{
    public class CertificateData
    {       
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

        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(CerNo) && !string.IsNullOrEmpty(ClassNo)
               && !string.IsNullOrEmpty(Date) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Specialty);               
        }

    }
}