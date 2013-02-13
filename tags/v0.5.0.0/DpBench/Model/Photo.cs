// -----------------------------------------------------------------------------------------
// DpBench - Photo.cs
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

namespace Paguru.DpBench.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    using BrightIdeasSoftware;

    /// <summary>
    /// Model data for a photo (image + parameters)
    /// </summary>
    public class Photo : INotifyPropertyChanged
    {
        #region Constants and Fields

        private string aperture;

        private string camera;

        private string exposure;

        private string relFilePath;

        private string absFilePath;

        private string focalLength;

        private string iso;

        private string keywords;

        private string lens;

        private string param1;

        private string param2;

        private string param3;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        [PhotoParameter(Sorter = typeof(FloatSorter))]
        [OLVColumn("Aperture", Width = 70, DisplayIndex = 3, TextAlign = HorizontalAlignment.Right)]
        public string Aperture
        {
            get
            {
                return aperture;
            }
            set
            {
                aperture = value;
                NotifyPropertyChanged("Aperture");
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public string BaseFilename
        {
            get
            {
                return Path.GetFileName(absFilePath);
            }
        }

        [PhotoParameter]
        [OLVColumn(Width = 70, DisplayIndex = 1)]
        public string Camera
        {
            get
            {
                return camera;
            }
            set
            {
                camera = value;
                NotifyPropertyChanged("Camera");
            }
        }

        [PhotoParameter(Sorter = typeof(FractionSorter))]
        [OLVColumn("Exposure", Width = 70, DisplayIndex = 4, TextAlign = HorizontalAlignment.Right)]
        public string Exposure
        {
            get
            {
                return exposure;
            }
            set
            {
                exposure = value;
                NotifyPropertyChanged("Exposure");
            }
        }

        [XmlElement("File")]
        [Browsable(false)]
        [OLVColumn("File", Width = 170, DisplayIndex = 0, TextAlign = HorizontalAlignment.Right)]
        public string RelFile
        {
            get
            {
                return relFilePath;
            }
            set
            {
                relFilePath = value;
                NotifyPropertyChanged("Filename");
            }
        }

        //[OLVColumn(Width = 170, DisplayIndex = 0, TextAlign = HorizontalAlignment.Right)]
        [Browsable(false)]
        [XmlIgnore]
        public string AbsFile
        {
            get
            {
                return absFilePath;
            }
            set
            {
                absFilePath = value;
            }
        }

        [PhotoParameter(Sorter = typeof(FloatSorter))]
        [OLVColumn("FocalLength", Width = 70, DisplayIndex = 5, TextAlign = HorizontalAlignment.Right)]
        public string FocalLength
        {
            get
            {
                return focalLength;
            }
            set
            {
                focalLength = value;
                NotifyPropertyChanged("FocalLength");
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Image Image
        {
            get
            {
                try
                {
                    return ImageCache.LoadImage(absFilePath);
                }
                catch (Exception e)
                {
                    // TODO show error image?
                    return null;
                }
            }
        }

        [PhotoParameter(Sorter = typeof(FloatSorter))]
        [OLVColumn(Title = "ISO", Width = 50, DisplayIndex = 6, TextAlign = HorizontalAlignment.Right)]
        public string Iso
        {
            get
            {
                return iso;
            }
            set
            {
                iso = value;
                NotifyPropertyChanged("Iso");
            }
        }

        [OLVColumn(Width = 150, DisplayIndex = 10)]
        public string Keywords
        {
            get
            {
                return keywords;
            }
            set
            {
                keywords = value;
                NotifyPropertyChanged("Keywords");
            }
        }

        [PhotoParameter]
        [OLVColumn(Width = 80, DisplayIndex = 2)]
        public string Lens
        {
            get
            {
                return lens;
            }
            set
            {
                lens = value;
                NotifyPropertyChanged("Lens");
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public Project Project { get; set; }

        [PhotoParameter]
        [OLVColumn(Width = 50, DisplayIndex = 21)]
        public string Param1
        {
            get
            {
                return param1;
            }
            set
            {
                param1 = value;
            }
        }

        [PhotoParameter]
        [OLVColumn(Width = 50, DisplayIndex = 22)]
        public string Param2
        {
            get
            {
                return param2;
            }
            set
            {
                param2 = value;
            }
        }

        [PhotoParameter]
        [OLVColumn(Width = 50, DisplayIndex = 23)]
        public string Param3
        {
            get
            {
                return param3;
            }
            set
            {
                param3 = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the parameters values (properties annotated as photo parameter).
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetParameters()
        {
            var result = new Dictionary<string, string>();
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(PhotoParameterAttribute), true).Length > 0)
                {
                    var value = propertyInfo.GetValue(this, null);
                    result[propertyInfo.Name] = (value != null) ? value.ToString() : null;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the comparer for olv column.
        /// </summary>
        /// <param name="columnTitle">column title</param>
        /// <returns></returns>
        public static IComparer GetComparerForOlvColumn(string columnTitle)
        {
            foreach (var pi in typeof(Photo).GetProperties())
            {
                var olvca = Util.GetAttribute<OLVColumnAttribute>(pi);
                var ppa = Util.GetAttribute<PhotoParameterAttribute>(pi);
                if (olvca != null && ppa != null &&
                    string.Equals(olvca.Title, columnTitle, StringComparison.OrdinalIgnoreCase))
                {
                    var s = Activator.CreateInstance(ppa.Sorter);
                    return (IComparer)s;
                }
            }
            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method is called by the Set accessor of each property.
        /// The CallerMemberName attribute that is applied to the optional propertyName
        /// parameter causes the property name of the caller to be substituted as an argument.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}