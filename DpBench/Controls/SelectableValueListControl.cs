// -----------------------------------------------------------------------------------------
// DpBench - SelectableValueListControl.cs
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    public partial class SelectableValueListControl : UserControl
    {
        #region Constants and Fields

        private List<SelectableValue> values;

        #endregion

        #region Constructors and Destructors

        public SelectableValueListControl()
        {
            InitializeComponent();
            checkedListBox1.KeyDown += MoveItem;
            checkedListBox1.ItemCheck += ItemChecked;
        }

        #endregion

        #region Public Properties

        public List<SelectableValue> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                checkedListBox1.Items.Clear();
                if (values != null)
                {
                    values.ForEach(x => checkedListBox1.Items.Add(x, x.Selected));

                    // listen to changes in the model
                    values.ForEach(x => x.PropertyChanged += SelectableValuePropertyChanged);
                }
            }
        }

        #endregion

        #region Methods

        private void Debug()
        {
            var l = new List<string>();
            foreach (var x in checkedListBox1.Items)
            {
                l.Add(x.ToString());
            }
            Console.Out.WriteLine(string.Join(",", l));
        }

        private void ItemChecked(object sender, ItemCheckEventArgs e)
        {
            var s = (SelectableValue)checkedListBox1.Items[e.Index];
            if (s.Selected != (e.NewValue == CheckState.Checked))
            {
                s.Selected = e.NewValue == CheckState.Checked;
            }
        }

        private void MoveDown(int incr = 0)
        {
            var i = checkedListBox1.SelectedIndex;

            // Console.Out.WriteLine("down:"+i);
            if (i >= 0 && i < checkedListBox1.Items.Count - 1)
            {
                var x = checkedListBox1.Items[i] as SelectableValue;
                checkedListBox1.BeginUpdate();
                checkedListBox1.Items.RemoveAt(i);
                checkedListBox1.Items.Insert(i + 1, x);
                values.RemoveAt(i);
                values.Insert(i + 1, x);
                checkedListBox1.SetItemChecked(i + 1, x.Selected);
                checkedListBox1.SelectedIndex = i + incr;
                checkedListBox1.EndUpdate();
            }
        }

        private void MoveItem(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Up)
            {
                MoveUp();
            }
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Down)
            {
                MoveDown();
            }

            // checkedListBox1.Refresh();
        }

        private void MoveUp(int decr = 0)
        {
            var i = checkedListBox1.SelectedIndex;

            // Console.Out.WriteLine("up:" + i);
            if (i > 0)
            {
                var x = checkedListBox1.Items[i] as SelectableValue;
                checkedListBox1.BeginUpdate();
                checkedListBox1.Items.RemoveAt(i);
                checkedListBox1.Items.Insert(i - 1, x);
                values.RemoveAt(i);
                values.Insert(i - 1, x);
                checkedListBox1.SetItemChecked(i - 1, x.Selected);
                checkedListBox1.SelectedIndex = i - decr;
                checkedListBox1.EndUpdate();
            }
        }

        private void SelectableValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            checkedListBox1.BeginUpdate();
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            {
                var s = (SelectableValue)checkedListBox1.Items[i];
                if (checkedListBox1.GetItemChecked(i) != s.Selected)
                {
                    checkedListBox1.SetItemCheckState(i, s.Selected ? CheckState.Checked : CheckState.Unchecked);
                }
            }
            checkedListBox1.EndUpdate();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            MoveDown(1);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            MoveUp(1);
        }

        private void labelAll_Click(object sender, EventArgs e)
        {
            values.ForEach(x => x.Selected = true);
        }

        private void labelDown_Click(object sender, EventArgs e)
        {
            MoveDown(1);
        }

        private void labelNone_Click(object sender, EventArgs e)
        {
            values.ForEach(x => x.Selected = false);
        }

        private void labelUp_Click(object sender, EventArgs e)
        {
            MoveUp(1);
        }

        #endregion
    }
}