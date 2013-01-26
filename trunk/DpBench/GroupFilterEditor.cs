// -----------------------------------------------------------------------------------------
// DpBench - GroupFilterEditor.cs
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

namespace Paguru.DpBench
{
    using System;
    using System.Windows.Forms;

    using Paguru.DpBench.Controls;
    using Paguru.DpBench.Model;
    using Paguru.DpBench.Renderer;

    public partial class GroupFilterEditor : Form
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupFilterEditor"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        public GroupFilterEditor(GroupFilter root)
        {
            InitializeComponent();

            Root = root;
            var glc = AddGroupLevelControl(root);
            glc.buttonClose.Enabled = glc.buttonClose.Visible = false;

            // propagate size change to group columns
            this.Layout += AdjustGroupLevelControlSizes;
        }

        #endregion

        #region Public Properties

        public GroupFilter Root { get; set; }

        #endregion

        #region Methods

        private GroupFilterControl AddGroupLevelControl(GroupFilter root)
        {
            var glc = new GroupFilterControl(root);

            glc.Height = tableLayoutPanel1.Height;
            glc.Width = 150;
            glc.buttonClose.Click += RemoveGroupControl;
            tableLayoutPanel1.Controls.Add(glc);
            return glc;
        }

        private void AdjustGroupLevelControlSizes(object sender, EventArgs e)
        {
            foreach (GroupFilterControl groupLevelControl in tableLayoutPanel1.Controls)
            {
                groupLevelControl.Height = tableLayoutPanel1.Height;
            }
        }

        private void RemoveGroupControl(object sender, EventArgs e)
        {
            var c = ((Control)sender).Parent as GroupFilterControl;
            tableLayoutPanel1.Controls.Remove(c);

            // connect previous to next
            c.GroupFilter.Remove();
            //var p = c.GroupFilter.PrevGroupFilter;
            //var n = c.GroupFilter.NextGroupFilter;
            //p.NextGroupFilter = n;
            //if (n != null)
            //{
            //    n.PrevGroupFilter = p;
            //}
        }

        #endregion

        private void buttonTest_Click(object sender, EventArgs e)
        {
            var r = new TextMosaicRenderer();
            var x = r.Render(Root);

            Console.Out.WriteLine(x);
        }

        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            AddGroupLevelControl(Root.Last.BuildNextLevel());
        }
    }
}