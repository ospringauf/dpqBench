// -----------------------------------------------------------------------------------------
// DpBench - GroupFilterControl.cs
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

namespace Paguru.DpBench.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    public partial class GroupFilterControl : UserControl
    {
        #region Constructors and Destructors

        public GroupFilterControl()
        {
            InitializeComponent();
        }

        public GroupFilterControl(GroupFilter groupFilter)
        {
            InitializeComponent();

            GroupFilter = groupFilter;
            comboBoxParameter.DataSource = groupFilter.Parameters;

            // comboBoxParameter.DataBindings.Add("Text", GroupFilter, "Parameter");
            GroupFilter.PropertyChanged += GlPropertyChanged;
            GroupFilter.ParameterValues.PropertyChanged += SelectedParameterValuesChanged;
            comboBoxParameter.TextChanged += ParameterChanged;
            ParameterChanged(null, null);
        }

        #endregion

        #region Public Properties

        public GroupFilter GroupFilter { get; set; }

        #endregion

        #region Methods

        private void GlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Parameter")
            {
                // parameter changed --> update values list
                listOrderControl1.Values = GroupFilter.ParameterValues;
            }
        }

        private void ParameterChanged(object sender, EventArgs e)
        {
            GroupFilter.Parameter = comboBoxParameter.Text;
        }

        private void SelectedParameterValuesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Count")
            {
                listOrderControl1.Values = GroupFilter.ParameterValues;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
        }

        #endregion
    }
}