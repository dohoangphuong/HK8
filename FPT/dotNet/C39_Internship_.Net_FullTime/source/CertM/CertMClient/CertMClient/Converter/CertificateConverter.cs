using CertMClient.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CertMClient.Converter
{
    public class CertificateConverter : BaseConverter
    {
        public override List<Dictionary<string, List<CertificateData>>> ConvertToList(string content)
        {
            var serializer = new JavaScriptSerializer();
            var obj = (Dictionary<string, object>)serializer.DeserializeObject(content);
            List<Dictionary<string, List<CertificateData>>> items = new List<Dictionary<string, List<CertificateData>>>();

            foreach (var key in obj)
            {
                var rowDatas = serializer.Deserialize<IList<CertificateData>>(key.Value as string);
                var certificateList = new Dictionary<string, List<CertificateData>>();
                foreach (var item in rowDatas)
                {
                    if (item.Specialty.ToUpper().Contains(CPP))
                    {
                        if (!certificateList.ContainsKey(CPP))
                        {
                            certificateList[CPP] = new List<CertificateData>();
                        }
                        else
                        {
                            // do nothing
                        }
                        certificateList[CPP].Add(item);
                    }
                    else
                        if (item.Specialty.ToUpper().Contains(NET))
                        {
                            if (!certificateList.ContainsKey(NET))
                            {
                                certificateList[NET] = new List<CertificateData>();
                            }
                            certificateList[NET].Add(item);
                        }
                        else
                            if (item.Specialty.ToUpper().Contains(TEST))
                            {
                                if (!certificateList.ContainsKey(TEST))
                                {
                                    certificateList[TEST] = new List<CertificateData>();
                                }
                                else
                                {
                                    // do nothing
                                }
                                certificateList[TEST].Add(item);
                            }
                            else
                                if (item.Specialty.ToUpper().Contains(JAVA))
                                {
                                    // java
                                    if (!certificateList.ContainsKey(JAVA))
                                    {
                                        certificateList[JAVA] = new List<CertificateData>();
                                    }
                                    certificateList[JAVA].Add(item);
                                }
                                else
                                    if (item.Specialty.ToUpper().Contains(EMBEDDED))
                                    {
                                        // Embedded Developer
                                        if (!certificateList.ContainsKey(EMBEDDED))
                                        {
                                            certificateList[EMBEDDED] = new List<CertificateData>();
                                        }
                                        else
                                        {
                                            // do nothing
                                        }
                                        certificateList[EMBEDDED].Add(item);
                                    }
                }
                items.Add(certificateList);
            }
            return items;
        }
    }
}