// -----------------------------------------------------------------------------------------
// DpBench - DetailArea.cs
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
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;

    /// <summary>
    /// A detail area (crop rectangle) for the photos of a project. Consists of a name (eg. "center")
    /// and the selected rectangle in image coordinates.
    /// </summary>
    public class DetailArea : INotifyPropertyChanged
    {
        #region Constants and Fields

        private Rectangle crop;

        private string name;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public Rectangle Crop
        {
            get
            {
                return crop;
            }
            set
            {
                crop = value;
                NotifyPropertyChanged("Crop");
            }
        }

        [XmlIgnore]
        public int Height
        {
            get
            {
                return crop.Height;
            }
            set
            {
                crop.Height = value;
                NotifyPropertyChanged("Height");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        [XmlIgnore]
        public int Width
        {
            get
            {
                return crop.Width;
            }
            set
            {
                crop.Width = value;
                NotifyPropertyChanged("Width");
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
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