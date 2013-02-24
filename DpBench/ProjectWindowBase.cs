// -----------------------------------------------------------------------------------------
// DpBench - ProjectWindowBase.cs
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
    using System.IO;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    /// <summary>
    /// Base class for both "simple" (Mono compatible) and "standard" project windows
    /// </summary>
    public abstract class ProjectWindowBase : DockContent
    {
        #region Public Properties

        public Project Project { get; set; }

        #endregion

        #region Public Methods

        public virtual void AddFiles()
        {
            // dataGridView1.DataSource = null;
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                using (Suspender.ShowSandTimerAndSuspend(this))
                {
                    Project.AddFiles(ofd.FileNames);
                    FillList();
                }
            }
        }

        #endregion

        #region Methods

        protected abstract void FillList();

        /// <summary>
        /// Called when a file is dropped in the project window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        protected virtual void ProjectWindow_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            try
            {
                using (Suspender.ShowSandTimerAndSuspend(this))
                {
                    Project.AddFiles(files);
                }
            }
            catch (Exception ex)
            {
                MainWindow.Instance.ShowError(ex);
            }

            FillList();
        }

        protected virtual void ProjectWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        protected virtual void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFiles();
        }

        protected virtual void groupLevelEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project.DetailAreas.Count == 0)
            {
                MessageBox.Show("Please define at least one detail area in the detail editor");
            }
            else
            {
                // TODO do not assume that there is only one filter
                if (Project.DefaultFilter != null)
                {
                    Project.DefaultFilter.Input = Project.CreateAllDetails();
                }
                else
                {
                    Project.DefaultFilter = new GroupFilter(Project.CreateAllDetails());
                }

                new GroupFilterEditor(Project, Project.DefaultFilter).Show();
            }
        }

        protected virtual void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fsd = new SaveFileDialog()
                {
                    DefaultExt = ".xml", 
                    AddExtension = true, 
                    Filter = "Project files (*.xml)|*.xml", 
                    FileName = Path.GetFileName(Project.ProjectFile)
                };
            if (fsd.ShowDialog(this) == DialogResult.OK)
            {
                Project.Save(fsd.FileName);
            }
            Text = Project.Name ?? "New Project";
        }

        protected virtual void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project.ProjectFile != null)
            {
                Project.Save(Project.ProjectFile);
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            MainWindow.Instance.StatusMessage("project saved");
        }

        #endregion
    }
}