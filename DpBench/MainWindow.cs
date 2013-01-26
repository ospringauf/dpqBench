// -----------------------------------------------------------------------------------------
// DpBench - MainWindow.cs
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
    using System.Linq;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class MainWindow : Form
    {
        #region Constants and Fields

        private static MainWindow _instance;

        #endregion

        #region Constructors and Destructors

        private MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Events

        public event EventHandler<PhotoSelectedEvent> OnSelectPhoto;

        #endregion

        #region Public Properties

        public static MainWindow Instance
        {
            get
            {
                return _instance ?? (_instance = new MainWindow());
            }
        }

        #endregion

        #region Properties

        private ProjectWindow ActiveProject
        {
            get
            {
                return
                    dockPanel.DocumentsToArray().Cast<ProjectWindow>().FirstOrDefault(
                        document => document.DockHandler.IsActivated);
            }
        }

        #endregion

        #region Public Methods

        public void ShowPreview(Photo photo)
        {
            if (OnSelectPhoto != null)
            {
                OnSelectPhoto(this, new PhotoSelectedEvent(photo));
            }
        }

        #endregion

        #region Methods

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            newProjectToolStripMenuItem_Click(null, null);
            propertiesToolStripMenuItem_Click(null, null);
            detailEditorToolStripMenuItem_Click(null, null);
        }

        private void OpenProjectWindow(Project p)
        {
            var projectWindow = new ProjectWindow(p);
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                projectWindow.MdiParent = this;
                projectWindow.Show();
            }
            else
            {
                projectWindow.Show(dockPanel);
            }
        }

        private void detailEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var detailEditor = new DetailEditor();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                detailEditor.MdiParent = this;
                detailEditor.Show();
            }
            else
            {
                detailEditor.Show(dockPanel);
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProjectWindow(new Project());
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fod = new OpenFileDialog() { DefaultExt = ".xml" };
            if (fod.ShowDialog(this) == DialogResult.OK)
            {
                OpenProjectWindow(Project.Load(fod.FileName));
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var propertyWindow = new PhotoPropertyWindow();
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                propertyWindow.MdiParent = this;
                propertyWindow.Show();
            }
            else
            {
                propertyWindow.Show(dockPanel);
            }
        }

        #endregion

        public void ShowError(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}