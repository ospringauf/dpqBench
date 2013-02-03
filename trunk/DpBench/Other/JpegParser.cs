using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using System.Text;
using System.Reflection;
using System.Xml;

namespace DeepZoomPublisher
{
    /// <remarks>
    /// from: http://deepzoompublisher.com/TagUpdater/
    /// 
    /// 
    /// Lossless JPEG rewrites in C#
    /// http://www.eggheadcafe.com/articles/20030706.asp
    /// 
    /// Another option for image metadata
    /// http://www.codeproject.com/KB/GDI-plus/ImageInfo.aspx
    /// 
    /// With Photoshop tags (VB.NET)
    /// http://groups.google.com/group/microsoft.public.dotnet.framework.drawing/browse_thread/thread/d265cef12e7c099f/662e714cac8ce31f?lnk=st&q=Dim+beginCapture+As+String+%3D+%22%3Cx%3Axmpmeta%22&rnum=1#662e714cac8ce31f
    /// </remarks>
    public class JpegParser 
    {
        public string Title;
        public string Description;
        public List<string> Keywords = new List<string>();
        public Int32 Rating;

        public string filename { get; set; }
        public string OutputPath { get; set; }
        public int Dpi { get; set; }
        public string KeywordString 
        {
            get 
            {
                string text = "";
                foreach (string k in Keywords)
                {
                    if (text == "")
                        text = k;
                    else
                        text += ","+k;
                }
                return text;
            }
        }

        public JpegParser(string jpeg)
        {
            filename = jpeg;
        }

        [STAThread]
        public bool ParseDocument()
        {
            Goheer.EXIF.EXIFextractor er2 = new Goheer.EXIF.EXIFextractor(filename, "", "");//"F:\\webdev\\raw.images.pk\\saripaya\\lonely Tree - Paey.jpg", "", "");

            string txtPage = String.Empty;

            foreach (DictionaryEntry de in er2)
            {
                switch (de.Key.ToString())
                {
                    case "Equip Make"://: SONY
                        txtPage += " " + de.Value;
                        break;
                    case "Equip Model": // : DSC-H9
                        txtPage += " " + de.Value;
                        break;
                    default:
                        txtPage = txtPage + de.Key + " : " + de.Value + Environment.NewLine;
                        break;
                }
            }

            string xmp = GetXmpXmlDocFromImage(filename);
            if (!String.IsNullOrEmpty(xmp))
            {
                LoadDoc(xmp);
            }
            return true;
        }

        /// <remarks>
        /// http://www.shahine.com/omar/ReadingXMPMetadataFromAJPEGUsingC.aspx
        /// </remarks>
        private static string GetXmpXmlDocFromImage(string filename)
        {
            string contents;
            string xmlPart;
            string beginCapture = "<rdf:RDF";
            string endCapture = "</rdf:RDF>";
            int beginPos;
            int endPos;

            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                contents = sr.ReadToEnd();
                System.Diagnostics.Debug.Write(contents.Length + " chars" + Environment.NewLine);
                sr.Close();
            }

            beginPos = contents.IndexOf(beginCapture, 0);
            endPos = contents.IndexOf(endCapture, 0);
            if (beginPos > 0 && endPos > 0)
            {
                System.Diagnostics.Debug.Write("xml found at pos: " + beginPos.ToString() + " - " + endPos.ToString());

                xmlPart = contents.Substring(beginPos, (endPos - beginPos) + endCapture.Length);

                System.Diagnostics.Debug.Write("Xml len: " + xmlPart.Length.ToString());

                return xmlPart;
            }
            return "";
        }

        /// <remarks>
        /// http://www.shahine.com/omar/ReadingXMPMetadataFromAJPEGUsingC.aspx
        /// </remarks>
        private void LoadDoc(string xmpXmlDoc)
        {
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager NamespaceManager;
            try
            {
                doc.LoadXml(xmpXmlDoc);
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("An error occured while loading XML metadata from image. The error was: " + ex.Message);
            }

            try
            {
                doc.LoadXml(xmpXmlDoc);

                NamespaceManager = new XmlNamespaceManager(doc.NameTable);
                NamespaceManager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                NamespaceManager.AddNamespace("exif", "http://ns.adobe.com/exif/1.0/");
                NamespaceManager.AddNamespace("x", "adobe:ns:meta/");
                NamespaceManager.AddNamespace("xap", "http://ns.adobe.com/xap/1.0/");
                NamespaceManager.AddNamespace("tiff", "http://ns.adobe.com/tiff/1.0/");
                NamespaceManager.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

                // get ratings
                XmlNode xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/xap:Rating", NamespaceManager);

                // Alternatively, there is a common form of RDF shorthand that writes simple properties as
                // attributes of the rdf:Description element.
                if (xmlNode == null)
                {
                    xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description", NamespaceManager);
                    xmlNode = xmlNode.Attributes["xap:Rating"];
                }

                if (xmlNode != null)
                {
                    this.Rating = Convert.ToInt32(xmlNode.InnerText);
                }

                // get keywords
                xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/dc:subject/rdf:Bag", NamespaceManager);

                if (xmlNode != null)
                {

                    foreach (XmlNode li in xmlNode)
                    {
                        Keywords.Add(li.InnerText);
                    }
                }

                // get description
                xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/dc:description/rdf:Alt", NamespaceManager);

                if (xmlNode != null)
                {
                    this.Description = xmlNode.ChildNodes[0].InnerText;
                }

                // get title
                xmlNode = doc.SelectSingleNode("/rdf:RDF/rdf:Description/dc:title/rdf:Alt", NamespaceManager);

                if (xmlNode != null)
                {
                    this.Title = xmlNode.ChildNodes[0].InnerText;
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while readning meta-data from image. The error was: " + ex.Message);
            }
            finally
            {
                doc = null;
            }
        }


    }
    
}

