using OMInsurance.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OMInsurance.WebApps.Security
{
    public sealed class AuthenticationPackage
    {
        #region Constants

        private readonly static Encoding XmlEncoding = Encoding.Unicode;

        private const byte Salt = 1;

        #endregion

        #region Fields

        private readonly static byte[] EncryptionKey = Encoding.UTF8.GetBytes(ConfiguraionProvider.AuthenticationCookieEncryptionKey);

        #endregion

        /// <summary>
        /// Date and time, in UTC, when package data become obsolete.
        /// </summary>
        public DateTime Expires { get; set; }

        public string PrincipalName { get; set; }

        #region Import and Export

        /// <summary>
        /// Serializes package to XML and encodes result to base-64 string.
        /// </summary>
        /// <returns>Base-64 string with XML representaton of package.</returns>
        public string ToXml()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                string xmlString = writer.ToString();

                List<byte> xmlBinary = XmlEncoding.GetBytes(xmlString).ToList();
                xmlBinary.Add(Salt);

                Cryptographer cryptographer = new Cryptographer(EncryptionKey);
                byte[] cryptedBinary = cryptographer.Encrypt(xmlBinary.ToArray());

                return Convert.ToBase64String(cryptedBinary);
            }
        }

        /// <summary>
        /// Decodes package from base-64 string with XML serialized value.
        /// </summary>
        /// <param name="cryptedXml">Base-64 string of XML serialized value.</param>
        /// <returns>Authentication package.</returns>
        public static AuthenticationPackage FromXml(string cryptedXml)
        {
            byte[] cryptedBinary = Convert.FromBase64String(cryptedXml);

            Cryptographer cryptographer = new Cryptographer(EncryptionKey);
            List<byte> xmlBinary = cryptographer.Decrypt(cryptedBinary).ToList();
            xmlBinary.RemoveAt(xmlBinary.Count - 1);

            string xmlString = XmlEncoding.GetString(xmlBinary.ToArray());

            XmlSerializer serializer = new XmlSerializer(typeof(AuthenticationPackage));
            using (StringReader reader = new StringReader(xmlString))
            {
                try
                {
                    return (AuthenticationPackage)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
        }

        #endregion
    }
}