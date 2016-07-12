using CertMServiceLib.Data;
using CertMServiceLib.Interface;
using log4net;
using System;

namespace CertMServiceLib.Generator
{
    /// <summary>
    /// Class to generator Generator instance
    /// </summary>
    public class GeneratorFactory
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GeneratorFactory));
        /// <summary>
        /// Create generator
        /// </summary>
        /// <param name="type">type of data to use, valid value are: certificate</param>
        /// <returns>isntance of generator</returns>
        public static IGenerator Create(Type type)
        {
            log.Debug(string.Format("Create Generator type of: {0}", type.Name));
            Type genType = null;
            if (type == typeof(CertificateData))
            {
                genType = typeof(BasicGenerator);
            }

            return (IGenerator)Activator.CreateInstance(genType);
        }
    }
}
