// -----------------------------------------------------------------------------------------
// DpBench - GroupLevel.cs
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
    public class GroupLevel : INotifyPropertyChanged
    {
        // private static int seq = 0;
        #region Constants and Fields

        private PhotoDetailCollection input;

        private string parameter;

        #endregion

        // private int id = seq++;
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupLevel"/> class.
        /// </summary>
        /// <param name="basis">The basis.</param>
        public GroupLevel(PhotoDetailCollection basis)
        {
            Input = basis;
            SelectedParameterValues = new SelectableValueList();
            SelectedParameterValues.PropertyChanged += SelectedParametersChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public PhotoDetailCollection Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                if (Parameter != null)
                {
                    // merge values list
                    SelectedParameterValues.Update(Input.FindAllValues(Parameter).ConvertAll(s => (object)s));

                    Propagate();
                }
                NotifyPropertyChanged("Input");
            }
        }

        /// <summary>
        /// Gets the last group level in the chain
        /// </summary>
        public GroupLevel Last
        {
            get
            {
                return NextGroupLevel != null ? NextGroupLevel.Last : this;
            }
        }

        public GroupLevel NextGroupLevel { get; set; }

        public PhotoDetailCollection Output
        {
            get
            {
                // all matching images (having one of the selected values for the parameter)
                return
                    new PhotoDetailCollection(
                        Input.Where(pd => (Parameter == null) || SelectedParameterValues[pd.Parameters[Parameter]]));

                // var n = new PhotoDetailCollection();
                // foreach (var pd in Input)
                // {
                // if (Parameter != null)
                // {
                // var pv = pd.Parameters[Parameter];
                // if (SelectedParameterValues[pv])
                // {
                // n.Add(pd);
                // }
                // }
                // }
                // return n;
            }
        }

        public string Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
                SelectedParameterValues.Clear();
                if (value != null)
                {
                    // build list of parameter values to choose from
                    // TODO if selected parameters exists, merge them with the new list
                    foreach (var pv in Input.FindAllValues(value))
                    {
                        SelectedParameterValues.Add(new SelectableValue(pv));
                    }
                }
                Propagate();
                NotifyPropertyChanged("Parameter");
            }
        }

        public List<string> Parameters
        {
            get
            {
                var r = new List<string>();
                if (Input != null && Input.Count > 0)
                {
                    r.AddRange(Input[0].Parameters.Keys);
                }
                return r;
            }
        }

        public GroupLevel PrevGroupLevel { get; set; }

        /// <summary>
        /// Gets or sets the distinct values of <see cref="Parameter"/> in the <see cref="Input"/>
        /// </summary>
        /// <value>
        /// The selected parameter values.
        /// </value>
        public SelectableValueList SelectedParameterValues { get; set; }

        #endregion

        #region Public Methods

        public GroupLevel BuildNextLevel()
        {
            // add all matching images (having one of the selected values for the parameter)
            var n = new GroupLevel(Output) { PrevGroupLevel = this };
            NextGroupLevel = n;
            return n;
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

        private void Propagate()
        {
            if (NextGroupLevel != null)
            {
                NextGroupLevel.Input = Output;
            }
        }

        private void SelectedParametersChanged(object sender, PropertyChangedEventArgs e)
        {
            Propagate();
        }

        #endregion
    }
}