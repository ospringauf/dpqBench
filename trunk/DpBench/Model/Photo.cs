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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Photo : INotifyPropertyChanged
    {
        #region Constants and Fields

        private string aperture;

        private string camera;

        private string exposure;

        private string filename;

        private string focalLength;

        private string iso;

        private string keywords;

        private string lens;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        [PhotoParameter]
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
                return Path.GetFileName(Filename);
            }
        }

        [PhotoParameter]
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

        [PhotoParameter]
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

        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
                NotifyPropertyChanged("Filename");
            }
        }

        [PhotoParameter]
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
                return Image.FromFile(Filename);
            }
        }

        [PhotoParameter]
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