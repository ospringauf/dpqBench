// -----------------------------------------------------------------------------------------
// DpBench - ProjectWindow.cs
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
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;

    using BrightIdeasSoftware;

    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    /// <summary>
    /// Project window, consists of a list of the photos to be analyzed.
    /// </summary>
    public partial class ProjectWindow : ProjectWindowBase
    {
        #region Constructors and Destructors

        public ProjectWindow(Project project)
        {
            Project = project;
            InitializeComponent();

            // generate columns from model annotations
            Generator.GenerateColumns(objectListView1, typeof(Photo));

            objectListView1.CustomSorter = (column, order) =>
                {
                    objectListView1.ListViewItemSorter = new ProjectColumnComparer(column, order);
                };

            Text = project.Name ?? "New Project";

            FillList();

            // somehow, the list view does not properly refresh when the list changes .. so let's force it
            Project.Photos.ListChanged += RefreshListContent;

            // support drag & drop of (jpeg) files into the project list
            objectListView1.DragEnter += ProjectWindow_DragEnter;
            objectListView1.DragDrop += ProjectWindow_DragDrop;

            BindMenuItems();
        }

        private void BindMenuItems()
        {
            // menu items
            addFilesToolStripMenuItem.Click += addFilesToolStripMenuItem_Click;
            createBenchmarkChartToolStripMenuItem.Click += groupLevelEditorToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
        }

        protected override void FillList()
        {
            objectListView1.SetObjects(Project.Photos);
        }

        private void ProjectWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        #endregion

        #region Public Properties

        //public Project Project { get; set; }

        #endregion

        #region Public Methods

        //public void AddFiles()
        //{
        //    var ofd = new OpenFileDialog();
        //    ofd.Multiselect = true;
        //    if (ofd.ShowDialog(this) == DialogResult.OK)
        //    {
        //        using (Suspender.ShowSandTimerAndSuspend(this))
        //        {
        //            Project.AddFiles(ofd.FileNames);
        //            FillList();
        //        }
        //    }
        //}

        #endregion

        #region Methods

        private void RefreshListContent(object sender, ListChangedEventArgs e)
        {
            objectListView1.RefreshObjects(Project.Photos);
            objectListView1.SetObjects(Project.Photos);
        }

        //private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    AddFiles();
        //}

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // var x = objectListView1.SelectedObject as Photo;
            foreach (Photo x in objectListView1.SelectedObjects)
            {
                if (x != null)
                {
                    Project.Photos.Remove(x);
                    objectListView1.SetObjects(Project.Photos);
                }
            }
        }

        //private void groupLevelEditorToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (Project.DetailAreas.Count == 0)
        //    {
        //        MessageBox.Show("Please define at least one detail area in the detail editor");
        //    }
        //    else
        //    {
        //        // TODO do not assume that there is only one filter
        //        if (Project.DefaultFilter != null)
        //        {
        //            Project.DefaultFilter.Input = Project.CreateAllDetails();
        //        }
        //        else
        //        {
        //            Project.DefaultFilter = new GroupFilter(Project.CreateAllDetails());
        //        }
                
        //        new GroupFilterEditor(Project, Project.DefaultFilter).Show();
        //    }
        //}

        private void objectListView1_SelectionChanged(object sender, EventArgs e)
        {
            var x = objectListView1.SelectedObject as Photo;
            if (x != null)
            {
                try
                {
                    MainWindow.Instance.ShowPreview(x);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not show preview", ex);
                }
            }
        }

        //private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (Project.ProjectFile != null)
        //    {
        //        Project.Save(Project.ProjectFile);
        //    }
        //    else
        //    {
        //        saveAsToolStripMenuItem_Click(sender, e);
        //    }
        //    MainWindow.Instance.StatusMessage("project saved");
        //}

        #endregion

        ///// <summary>
        ///// Called when a file is dropped in the project window
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        //private void ProjectWindow_DragDrop(object sender, DragEventArgs e)
        //{
        //    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        //    try
        //    {
        //        using (Suspender.ShowSandTimerAndSuspend(this))
        //        {
        //            Project.AddFiles(files);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MainWindow.Instance.ShowError(ex);
        //    }
        //    FillList();
        //}

        //private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    var fsd = new SaveFileDialog()
        //        {
        //            DefaultExt = ".xml", 
        //            AddExtension = true, 
        //            Filter = "Project files (*.xml)|*.xml",
        //            FileName = Path.GetFileName(Project.ProjectFile)
        //        };
        //    if (fsd.ShowDialog(this) == DialogResult.OK)
        //    {
        //        Project.Save(fsd.FileName);
        //    }
        //    Text = Project.Name ?? "New Project";
        //}
    }
}