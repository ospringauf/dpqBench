// -----------------------------------------------------------------------------------------
// DpBench - SelectableValue.cs
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
    using System.ComponentModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelectableValue : INotifyPropertyChanged
    {
        #region Constants and Fields

        private bool selected;

        private string value;

        #endregion

        #region Constructors and Destructors

        public SelectableValue(string v, bool sel = true)
        {
            value = v;
            selected = sel;
        }

        /// <summary>
        /// Only for serializing
        /// </summary>
        public SelectableValue()
        {
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected != value)
                {
                    selected = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                if (!Equals(this.value, value))
                {
                    this.value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region Methods

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