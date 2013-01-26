// -----------------------------------------------------------------------------------------
// DpBench - SelectableValueList.cs
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
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelectableValueList : List<SelectableValue>, INotifyPropertyChanged
    {
        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public List<object> SelectedValues
        {
            get
            {
                return this.Where(v => v.Selected).Cast<object>().ToList();
            }
        }

        #endregion

        #region Public Indexers

        public bool this[object v]
        {
            get
            {
                // find the value and return its selection state
                var w = this.Find(x => Equals(x.Value, v));
                return w != null && w.Selected;
            }
            set
            {
                // change the value's selection state or add new value
                var w = this.Find(x => Equals(x.Value, v));
                if (w != null)
                {
                    w.Selected = value;
                }
                else
                {
                    Add(new SelectableValue(v, value));
                }
            }
        }

        #endregion

        #region Public Methods

        public void Add(SelectableValue v)
        {
            if (!Contains(v))
            {
                base.Add(v);
                v.PropertyChanged += SelectionChanged;
                NotifyPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Updates the values list to the specified new domain, keeping the existing selection
        /// </summary>
        /// <param name="newDomain">The new domain.</param>
        public void Update(List<object> newDomain)
        {
            // add
            foreach (var x in newDomain)
            {
                this[x] = this[x];
            }

            // remove
            var r = RemoveAll(x => !newDomain.Contains(x.Value));
            if (r > 0)
            {
                NotifyPropertyChanged("Count");
            }
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

        private void SelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Selection");
        }

        #endregion
    }
}