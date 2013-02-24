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
    using System.Drawing;
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
            comboBoxParameter.DataSource = GroupFilter.Parameters;
            if (GroupFilter.Parameter != null)
            {
                comboBoxParameter.SelectedItem = GroupFilter.Parameter;
            }
            else
            {
                //GroupFilter.Parameter = GroupFilter.Parameters[0];
            }
            SelectableValueListControl1.Values = GroupFilter.ParameterValues;

            // comboBoxParameter.DataBindings.Add("Text", GroupFilter, "Parameter");
            GroupFilter.PropertyChanged += GlPropertyChanged;
            GroupFilter.ParameterValues.PropertyChanged += SelectedParameterValuesChanged;
            //comboBoxParameter.TextChanged += ParameterChanged;
            comboBoxParameter.SelectedValueChanged += ParameterChanged;
            CheckValid();
        }

        #endregion

        #region Public Properties

        public GroupFilter GroupFilter { get; set; }

        #endregion

        #region Methods

        private void GlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //MessageBox.Show("GlPropertyChanged");
            if (e.PropertyName == "Parameter")
            {
                // parameter changed --> update values list
                SelectableValueListControl1.Values = GroupFilter.ParameterValues;
                CheckValid();
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
                SelectableValueListControl1.Values = GroupFilter.ParameterValues;
            }

            // also check if selection has changed
            CheckValid();
        }

        private void CheckValid()
        {
            comboBoxParameter.BackColor = GroupFilter.Valid ? Color.White : Color.LightCoral;

        }

        #endregion
    }
}