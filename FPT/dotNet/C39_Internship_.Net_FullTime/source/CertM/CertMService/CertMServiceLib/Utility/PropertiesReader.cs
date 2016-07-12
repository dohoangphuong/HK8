using log4net;
using System;
using System.Collections.Generic;
using System.IO;

namespace CertMServiceLib.Utility
{
    public class PropertiesReader
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PropertiesReader));
        Dictionary<string, string> properties;
        public void Load(string filePath)
        {
            log.Debug("Load properties file: " + filePath);
            if(string.IsNullOrEmpty(filePath))
            {
                log.Error("Empty or null file path");
                throw new ArgumentNullException("File path is null or invalid");
            }

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException("File: " + filePath + " not found");
            }
            properties = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string lineContent;
                string[] splitData;
                while(!sr.EndOfStream)
                {
                    lineContent = sr.ReadLine().Trim();
                    if(!lineContent.StartsWith("#") && lineContent.Contains("="))
                    {
                        splitData = lineContent.Split('=');
                        if (splitData.Length == 2)
                        {
                            properties.Add(splitData[0].Trim(), splitData[1].Trim());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// get properties by it's value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (properties.ContainsKey(key))
                    return properties[key];

                return null;
            }
        }
    }
}
