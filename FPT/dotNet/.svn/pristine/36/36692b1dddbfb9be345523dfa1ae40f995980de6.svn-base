using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertMServiceLib.Interface
{
    public interface IGenerator
    {
        /// <summary>
        /// Generator certificate
        /// </summary>
        /// <returns>true if generator success, false otherwise</returns>
        bool Generate();
        // data to generate
        ICertificateData Data { get; set; }
        // close front side of document
        void CloseFrontSide();
        // close all document
        void CloseAll();
     
        // close back side
        void CloseBackSide();
        void CloseDocument();
    }
}
