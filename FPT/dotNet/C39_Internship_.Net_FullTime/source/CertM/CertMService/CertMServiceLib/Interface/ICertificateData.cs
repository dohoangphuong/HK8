using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib.Interface
{
    public interface ICertificateData
    {
        // name of class
        string ClassNo { get; set; }
        // Name of student
        string Name { get; set; }
        // specialty
        string Specialty { get; set; }
        // certificate number
        string CerNo { get; set; }
        // level
        string Rank { get; set; }
        /// <summary>
        /// Date create certificate (signed date)
        /// </summary>
        string Date { get; set; }

        bool IsValidData();
    }
}
