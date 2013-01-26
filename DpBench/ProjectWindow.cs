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
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class ProjectWindow : DockContent
    {
        #region Constructors and Destructors

        public ProjectWindow(Project project)
        {
            Project = project;
            InitializeComponent();
            Text = project.Name ?? "New Project";

            objectListView1.SetObjects(Project.Photos);

            // somehow, the list view does not properly refresh when the list changes .. so let's force it
            Project.Photos.ListChanged += RefreshListContent;

            // support drag & drop of (jpeg) files into the project list
            objectListView1.DragEnter += ProjectWindow_DragEnter;
            objectListView1.DragDrop += ProjectWindow_DragDrop;
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

        public Project Project { get; set; }

        #endregion

        #region Public Methods

        public void AddFiles()
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    Project.AddFile(fileName);
                }
                objectListView1.SetObjects(Project.Photos);
            }
        }

        #endregion

        #region Methods

        private void RefreshListContent(object sender, ListChangedEventArgs e)
        {
            objectListView1.RefreshObjects(Project.Photos);
        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFiles();
        }

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

        private void groupLevelEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GroupFilterEditor(new GroupFilter(Project.CreateAllDetails())).Show();
        }

        private void objectListView1_SelectionChanged(object sender, EventArgs e)
        {
            var x = objectListView1.SelectedObject as Photo;
            if (x != null)
            {
                MainWindow.Instance.ShowPreview(x);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fsd = new SaveFileDialog() { DefaultExt = ".xml", AddExtension = true };
            if (fsd.ShowDialog(this) == DialogResult.OK)
            {
                Project.Save(fsd.FileName);
            }
            Text = Project.Name ?? "New Project";
        }

        #endregion

        private void ProjectWindow_DragDrop(object sender, DragEventArgs e)
        {
            //Console.Out.WriteLine(e);
            //var t = e.Data.GetType();
            //Console.Out.WriteLine(t);
            //var d = e.Data.GetFormats();
            //Console.Out.WriteLine(d);
            //var f = e.Data.GetData("FileName");
            //Console.Out.WriteLine(f);
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            try
            {
                foreach (string file in files)
                {
                    Project.AddFile(file);
                }
            }
            catch (Exception ex)
            {
                MainWindow.Instance.ShowError(ex);
            }
            objectListView1.SetObjects(Project.Photos);
        }
    }
}