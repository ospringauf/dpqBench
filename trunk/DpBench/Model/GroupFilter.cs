// -----------------------------------------------------------------------------------------
// DpBench - GroupFilter.cs
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
    public class GroupFilter : INotifyPropertyChanged
    {
        // private static int seq = 0;
        #region Constants and Fields

        private PhotoDetailCollection input;

        private string parameter;

        #endregion

        // private int id = seq++;
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupFilter"/> class.
        /// </summary>
        /// <param name="basis">The basis.</param>
        public GroupFilter(PhotoDetailCollection basis)
        {
            Input = basis;
            ParameterValues = new SelectableValueList<string>();
            ParameterValues.PropertyChanged += SelectedParametersChanged;
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
                    ParameterValues.Update(Input.FindAllValues(Parameter)); //.ConvertAll(s => (object)s));

                    Propagate();
                }
                NotifyPropertyChanged("Input");
            }
        }

        /// <summary>
        /// Gets the last group level in the chain
        /// </summary>
        public GroupFilter Last
        {
            get
            {
                return NextGroupFilter != null ? NextGroupFilter.Last : this;
            }
        }

        public bool IsLast
        {
            get
            {
                return this == Last;
            }
        }

        public GroupFilter NextGroupFilter { get; set; }

        public PhotoDetailCollection Output
        {
            get
            {
                // all matching images (having one of the selected values for the parameter)
                return
                    new PhotoDetailCollection(
                        Input.Where(pd => (Parameter == null) || ParameterValues[pd.Parameters[Parameter]]));

                // var n = new PhotoDetailCollection();
                // foreach (var pd in Input)
                // {
                // if (Parameter != null)
                // {
                // var pv = pd.Parameters[Parameter];
                // if (ParameterValues[pv])
                // {
                // n.Add(pd);
                // }
                // }
                // }
                // return n;
            }
        }

        /// <summary>
        /// Gets or sets the name for the discriminating parameter for this level (eg. "Lens").
        /// </summary>
        public string Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
                ParameterValues.Clear();
                if (value != null)
                {
                    // build list of parameter values to choose from
                    // TODO if selected parameters exists, merge them with the new list
                    ParameterValues.Update(Input.FindAllValues(parameter), true);
                    //foreach (var pv in Input.FindAllValues(value))
                    //{
                    //    ParameterValues.Add(new SelectableValue(pv));
                    //}
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

        public GroupFilter PrevGroupFilter { get; set; }

        /// <summary>
        /// Gets or sets the distinct values of <see cref="Parameter"/> in the <see cref="Input"/>
        /// </summary>
        public SelectableValueList<string> ParameterValues { get; set; }

        #endregion

        #region Public Methods

        public GroupFilter BuildNextLevel()
        {
            // add all matching images (having one of the selected values for the parameter)
            var n = new GroupFilter(Output) { PrevGroupFilter = this };
            NextGroupFilter = n;
            return n;
        }

        /// <summary>
        /// Filters the details collection by the specified value.
        /// </summary>
        /// <param name="value">a parameter value (eg. "2.8" for parameter "Aperture").</param>
        /// <param name="fromDetails">the input set of photo details</param>
        /// <returns>the matching photo details</returns>
        public PhotoDetailCollection Filter(string value, PhotoDetailCollection fromDetails = null)
        {
            fromDetails = fromDetails ?? Input;
            return
                    new PhotoDetailCollection(
                        fromDetails.Where(pd => pd.Parameters[Parameter] == value));
        }

        /// <summary>
        /// Removes this instance from the filter chain.
        /// </summary>
        public void Remove()
        {
            var p = PrevGroupFilter;
            var n = NextGroupFilter;
            p.NextGroupFilter = n;
            if (n != null)
            {
                n.PrevGroupFilter = p;
            }
            p.Propagate();
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
            if (NextGroupFilter != null)
            {
                NextGroupFilter.Input = Output;
            }
        }

        private void SelectedParametersChanged(object sender, PropertyChangedEventArgs e)
        {
            Propagate();
        }

        #endregion
    }
}