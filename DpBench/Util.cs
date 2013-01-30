// -----------------------------------------------------------------------------------------
// DpBench - Util.cs
// http://sourceforge.net/projects/dpbench/
// -----------------------------------------------------------------------------------------
// Copyright 2013 Oliver Springauf
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// -----------------------------------------------------------------------------------------

namespace Paguru.DpBench
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// helper functions
    /// </summary>
    public class Util
    {
        #region Public Methods

        /// <summary>
        /// Deserializes an object out of an XML file
        /// </summary>
        /// <typeparam name="T">Type of the class</typeparam>
        /// <param name="filename">The name of the XML file</param>
        /// <returns>an instance of the given class</returns>
        /// <exception cref="InvalidOperationException">if the xml is invalid or not well formed</exception>
        public static T DeserializeFromXmlFile<T>(string filename) where T : class
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var result = DeserializeFromXmlStream<T>(fs);
                fs.Close();
                return result;
            }
        }

        /// <summary>
        /// Create an object from a stream containing XML text. The XML mapping information
        /// must be annotated to the object's class.
        /// </summary>
        /// <typeparam name="T">target class</typeparam>
        /// <param name="fs">an input stream</param>
        /// <returns>an instance of the target class, de-serialized from the given stream</returns>
        /// <exception cref="InvalidOperationException">if the xml is invalid or not well formed</exception>
        public static T DeserializeFromXmlStream<T>(Stream fs) where T : class
        {
            T xmlObject;
            using (TextReader reader = new StreamReader(fs))
            {
                var ser = new XmlSerializer(typeof(T));
                xmlObject = (T)ser.Deserialize(reader);
                reader.Close();
            }
            fs.Close();
            return xmlObject;
        }

        public static XmlDocument SerializeToXmlDocument<T>(T obj)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(SerializeToXmlString(obj));

            return xmlDoc;
        }

        public static void SerializeToXmlFile<T>(T obj, string xmlFile)
        {
            // Create file to save the data to
            using (var fs = new FileStream(xmlFile, FileMode.Create))
            {
                // Create a XmlSerializer object to perform the serialization
                var xs = new XmlSerializer(typeof(T));

                // Use the XMLSerializer object to serialize the data to the file
                xs.Serialize(fs, obj);

                // Close the file
                fs.Close();
            }
        }

        /// <summary>
        /// Serialize the object to an UTF-8 encoded XmlString
        /// </summary>
        /// <param name="obj"> The obj. </param>
        /// <typeparam name="T"> a object of type T </typeparam>
        /// <returns> the XmlDocument </returns>
        public static string SerializeToXmlString<T>(T obj)
        {
            using (var memStrm = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                UTF8Encoding utf8e = new UTF8Encoding(false);
                XmlTextWriter xmlSink = new XmlTextWriter(memStrm, utf8e);
                xmlSerializer.Serialize(xmlSink, obj);

                var result = utf8e.GetString(memStrm.ToArray());
                return result;
            }
        }

        public static string FilterString(string s, string allowedCharacters)
        {
            var r = String.Empty;
            for (int i = 0; i < s.Length; ++i)
            {
                if (allowedCharacters.IndexOf(s[i]) >= 0)
                {
                    r += s[i];
                }
            }
            return r; //.Replace(',', '.');
        }

        public static string RelativePath(string filepath, string projectDir)
        {
            var workdir = projectDir; // Environment.CurrentDirectory;
            if (string.IsNullOrEmpty(workdir))
            {
                return filepath;
            }

            Uri uri1 = new Uri(filepath);
            Uri uri2 = new Uri(workdir + Path.DirectorySeparatorChar);
            Uri relativeUri = uri2.MakeRelativeUri(uri1);

            string relativePath =
                Uri.UnescapeDataString(relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
            return relativePath;
        }

        public static T GetAttribute<T>(PropertyInfo pi) where T : Attribute
        {
            if (pi.GetCustomAttributes(typeof(T), true).Length > 0)
            {
                object[] a = pi.GetCustomAttributes(typeof(T), true);
                var typeAtt = a[0] as T;
                return typeAtt;
            }
            return null;
        }

        public static bool HasAttribute<T>(PropertyInfo pi) where T : Attribute
        {
            return pi.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        public static char DecimalPoint()
        {
            var s = string.Format(CultureInfo.CurrentCulture, "{0}", 1.1f);
            return s[1];
        }

        #endregion
    }
}