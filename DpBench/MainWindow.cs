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
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using Paguru.DpBench.Controls;
    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class MainWindow : Form
    {
        #region Constants and Fields

        private static MainWindow _instance;

        //private OldDetailEditor oldDetailEditor;
        private DetailEditor detailEditor;

        private PhotoPropertyWindow propertyWindow;

        #endregion

        #region Constructors and Destructors

        private MainWindow()
        {
            InitializeComponent();
            dockPanel.DocumentStyle = DocumentStyle.DockingMdi;
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

        //public OldDetailEditor OldDetailEditor
        //{
        //    get
        //    {
        //        return oldDetailEditor;
        //    }
        //    set
        //    {
        //        oldDetailEditor = value;
        //        imagePreviewToolStripMenuItem.Image = oldDetailEditor != null ? Properties.Resources.check_16x13 : null;
        //        if (oldDetailEditor != null)
        //        {
        //            OnSelectPhoto += OldDetailEditor.ShowPreview;
        //            OldDetailEditor.FormClosing += (s, ev) =>
        //            {
        //                OnSelectPhoto -= OldDetailEditor.ShowPreview;
        //                OldDetailEditor = null;
        //            };
        //        }
        //    }
        //}

        public DetailEditor DetailEditor
        {
            get
            {
                return detailEditor;
            }
            set
            {
                detailEditor = value;
                imagePreviewToolStripMenuItem.Image = detailEditor != null ? Properties.Resources.check_16x13 : null;
                if (detailEditor != null)
                {
                    OnSelectPhoto += DetailEditor.ShowPreview;
                    DetailEditor.FormClosing += (s, ev) =>
                    {
                        OnSelectPhoto -= DetailEditor.ShowPreview;
                        DetailEditor = null;
                    };
                }
            }
        }

        public int Progress
        {
            get
            {
                return toolStripProgressBar1.Value;
            }
            set
            {
                toolStripProgressBar1.Value = value;
                statusStrip1.Update();
            }
        }

        public PhotoPropertyWindow PropertyWindow
        {
            get
            {
                return propertyWindow;
            }
            set
            {
                propertyWindow = value;
                propertiesToolStripMenuItem.Image = propertyWindow != null ? Properties.Resources.check_16x13 : null;
                if (propertyWindow != null)
                {
                    OnSelectPhoto += PropertyWindow.ShowPhotoProperties;
                    PropertyWindow.FormClosing += (s, ev) =>
                    {
                        OnSelectPhoto -= PropertyWindow.ShowPhotoProperties;
                        PropertyWindow = null;
                    };
                }
            }
        }

        /// <summary>
        /// Gets or sets the startup project file (command line parameter).
        /// </summary>
        public string StartupProjectFile { get; set; }

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

        public void ClearProgress()
        {
            toolStripStatusLabel1.Text = string.Empty;
            toolStripProgressBar1.Value = 0;
            statusStrip1.Update();
        }

        public void ScaleProgress(string processStep, int max)
        {
            toolStripProgressBar1.Maximum = max;
            toolStripStatusLabel1.Text = processStep;
            statusStrip1.Update();
        }

        public void ShowError(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }

        public void ShowPreview(Photo photo)
        {
            if (OnSelectPhoto != null)
            {
                OnSelectPhoto(this, new PhotoSelectedEvent(photo));
            }
        }

        public void StatusMessage(string s)
        {
            toolStripStatusLabel1.Text = s;
            statusStrip1.Update();
            var t = new Timer() { Interval = 1000 };
            t.Tick += delegate
                {
                    toolStripStatusLabel1.Text = string.Empty;
                    t.Stop();
                    t.Dispose();
                };
            t.Start();
        }

        #endregion

        #region Methods

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (StartupProjectFile != null)
            {
                OpenProjectWindow(Project.Load(StartupProjectFile));
            }
            else
            {
                OpenProjectWindow(new Project());
            }
            MenuPropertiesClick(null, null);
            MenuDetailEditorClick(null, null);
        }

        //private void MenuDetailEditorClick(object sender, EventArgs e)
        //{
        //    if (OldDetailEditor != null)
        //    {
        //        OldDetailEditor.Close();
        //    }
        //    else
        //    {
        //        OldDetailEditor = new OldDetailEditor();
        //        OldDetailEditor.Show(dockPanel);
        //    }
        //}

        private void MenuFileAboutClick(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog(this);
        }

        private void MenuNewProjectClick(object sender, EventArgs e)
        {
            OpenProjectWindow(new Project());
        }

        private void MenuOpenClick(object sender, EventArgs e)
        {
            var fod = new OpenFileDialog() { DefaultExt = ".xml", Filter = "Project files (*.xml)|*.xml" };
            if (fod.ShowDialog(this) == DialogResult.OK)
            {
                var project = Project.Load(fod.FileName);
                project.Name = Path.GetFileNameWithoutExtension(fod.FileName);
                OpenProjectWindow(project);
            }
        }

        private void MenuPropertiesClick(object sender, EventArgs e)
        {
            if (PropertyWindow != null)
            {
                PropertyWindow.Close();
            }
            else
            {
                PropertyWindow = new PhotoPropertyWindow();
                PropertyWindow.Show(dockPanel);
            }
        }

        private void OpenProjectWindow(Project p)
        {
            var projectWindow = new ProjectWindow(p);
            projectWindow.Show(dockPanel);
        }

        #endregion

        private void MenuDetailEditorClick(object sender, EventArgs e)
        {
            if (DetailEditor != null)
            {
                DetailEditor.Close();
            }
            else
            {
                DetailEditor = new DetailEditor();
                DetailEditor.Show(dockPanel);
            }
        }
    }
}