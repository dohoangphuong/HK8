//using GSTCertificateInput.localhost;
#if !DEBUG
#else
using CertMClient.CertMServiceData;
#endif
using System.Collections.Generic;
using CertMClient.Data;


namespace CertMClient.Converter
{
    public class BaseConverter
    {
        protected const string CPP = "C++";
        protected const string NET = "NET";
        protected const string TEST = "TEST";
        protected const string JAVA = "JAVA";
        protected const string EMBEDDED = "EMBEDDED";

        public virtual List<Dictionary<string, List<CertificateData>>> ConvertToList(string content)
        {
            return null;
        }

        /// <summary>
        /// convert an object to json string
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>null if object is null, other return json string of object</returns>
        public  string ConvertObjectToJson(object obj)
        {
            if (obj == null)
                return null;

            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
        }
    }
}