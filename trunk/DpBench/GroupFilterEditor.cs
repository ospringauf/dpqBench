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

    /// <summary>
    /// Editor for group levels, parameter order and filter
    /// </summary>
    public partial class GroupFilterEditor : Form
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupFilterEditor"/> class.
        /// </summary>
        /// <param name="project">the dpqb project</param>
        /// <param name="root">The root filter</param>
        public GroupFilterEditor(Project project, GroupFilter root)
        {
            InitializeComponent();

            Project = project;
            Root = root;

            var f = root;
            while (f != null)
            {
                var glc = AddGroupFilterControl(f);

                // enable close only on child filters
                glc.buttonClose.Enabled = glc.buttonClose.Visible = glc.GroupFilter.PrevGroupFilter != null;
                f = f.NextGroupFilter;
            }

            // propagate size change to group columns
            this.Layout += AdjustGroupLevelControlSizes;

            // TODO support custom renderers
            panelRendererSettings.Controls.Add(new YxTableRendererSettingsControl(new YxImageTableRenderer(project), Root));
        }

        #endregion

        #region Public Properties

        public GroupFilter Root { get; set; }

        public Project Project { get; private set; }

        #endregion

        #region Methods

        private GroupFilterControl AddGroupFilterControl(GroupFilter root)
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
        }

        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddGroupFilterControl(Root.Last.BuildNextLevel());
        }
    }
}