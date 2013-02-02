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
    using System.Xml.Serialization;

    using Paguru.DpBench.Renderer;

    /// <summary>
    /// Defines the parameter (eg. "Lens"), parameter value filter and parameter value order for one grouping level.
    /// Grouping filters can be chained into a double linked list that defines multiple grouping levels.
    /// An <see cref="IRenderer"/> will then render a multi-level comparison chart based on the grouping/filter settings.
    /// </summary>
    public class GroupFilter : INotifyPropertyChanged
    {
        #region Constants and Fields

        private PhotoDetailCollection input;

        private string parameter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupFilter"/> class.
        /// </summary>
        /// <param name="basis">The basis.</param>
        public GroupFilter(PhotoDetailCollection basis) : this()
        {
            Input = basis;
            Parameter = Parameters[0];
        }

        /// <summary>
        /// Only for serializing
        /// </summary>
        public GroupFilter()
        {
            Name = "GroupFilter";
            ParameterValues = new SelectableValueList();
            ParameterValues.PropertyChanged += SelectedParametersChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        [XmlIgnore]
        public PhotoDetailCollection Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
                if (Parameter != null && Input != null)
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
        [XmlIgnore]
        public GroupFilter Last
        {
            get
            {
                return NextGroupFilter != null ? NextGroupFilter.Last : this;
            }
        }

        [XmlIgnore]
        public bool IsLast
        {
            get
            {
                return this == Last;
            }
        }

        public string Name { get; set; }

        [XmlIgnore]
        public int TotalTiles
        {
            get
            {
                return IsLast
                           ? ParameterValues.SelectedValues.Count
                           : ParameterValues.SelectedValues.Count * NextGroupFilter.TotalTiles;
            }
        }

        private GroupFilter nextGroupFilter;

        public GroupFilter NextGroupFilter
        {
            get
            {
                return nextGroupFilter;
            }
            set
            {
                nextGroupFilter = value;
                if (nextGroupFilter != null)
                {
                    nextGroupFilter.PrevGroupFilter = this;
                }
                Propagate();
            }
        }

        [XmlIgnore]
        public PhotoDetailCollection Output
        {
            get
            {
                // all matching images (having one of the selected values for the parameter)
                return
                    new PhotoDetailCollection(
                        Input.Where(pd => (Parameter == null) || ParameterValues[pd.Parameters[Parameter]]));
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
                if (!Equals(parameter, value))
                {
                    parameter = value;
                    ParameterValues.Clear();
                    if (value != null && Input != null)
                    {
                        // build list of parameter values to choose from
                        // if selected parameters exists, merge them with the new list
                        ParameterValues.Update(Input.FindAllValues(parameter), true);
                    }
                    Propagate();
                    NotifyPropertyChanged("Parameter");
                }
            }
        }

        /// <summary>
        /// Gets all available parameters (eg. "Lens", "Camera", "Aperture") for the input images.
        /// </summary>
        [XmlIgnore]
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

        [XmlIgnore]
        public GroupFilter PrevGroupFilter { get; set; }

        /// <summary>
        /// Gets or sets the distinct values of <see cref="Parameter"/> in the <see cref="Input"/>.
        /// </summary>
        public SelectableValueList ParameterValues { get; set; }

        [XmlIgnore]
        public bool Valid
        {
            get
            {
                return Output.Count > 0;
            }
        }

        [XmlIgnore]
        public bool AllValid
        {
            get
            {
                return NextGroupFilter != null ? NextGroupFilter.AllValid && Valid : Valid;
            }
        }

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
        /// Removes this instance from the filter chain, linking the previous and next levels to each other.
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

        /// <summary>
        /// Propagates the filter output (matching photo details) to the next filter level
        /// </summary>
        private void Propagate()
        {
            if (NextGroupFilter != null && Input != null)
            {
                NextGroupFilter.Input = Output;
            }
        }

        /// <summary>
        /// Output set might have changed, update the next level.
        /// </summary>
        private void SelectedParametersChanged(object sender, PropertyChangedEventArgs e)
        {
            Propagate();
        }

        public override string ToString()
        {
            return Parameter + "=" + string.Join(",", ParameterValues.SelectedValues.ToArray());
        }

        #endregion
    }
}