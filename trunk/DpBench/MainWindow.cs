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

        private DetailEditor detailEditor;

        private PhotoPropertyWindow propertyWindow;

        public string[] StartupFiles;

        #endregion

        #region Constructors and Destructors

        private MainWindow()
        {
            InitializeComponent();
            dockPanel.DocumentStyle = DocumentStyle.DockingMdi;

            //if (Program.MonoMode)
            //{
            //    dockPanel.DocumentStyle = DocumentStyle.SystemMdi;    
            //    this.IsMdiContainer = true;
            //    Controls.Remove(dockPanel);
            //}
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when a photo is selected in the project window.
        /// </summary>
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

        public DetailEditor DetailEditor
        {
            get
            {
                return detailEditor;
            }
            set
            {
                detailEditor = value;
                imagePreviewToolStripMenuItem.Checked = detailEditor != null;
                if (detailEditor != null)
                {
                    // detail editor receives "photo selected" events
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
                propertiesToolStripMenuItem.Checked = propertyWindow != null;
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
            //Console.Out.WriteLine("show preview " + photo.BaseFilename);
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

        /// <summary>
        /// Initialize desktop after start.
        /// Show selected project or empty project, and the "detail" and "properties" views
        /// </summary>
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            var firstProject = StartupProjectFile != null ? Project.Load(StartupProjectFile) : new Project(); 
            OpenProjectWindow(firstProject);

            // open files given on command line (or from external tool)
            if (StartupFiles != null)
            {
                firstProject.AddFiles(StartupFiles);
            }

            if (!Program.MonoMode)
            {
                MenuPropertiesClick(null, null);
            }
            MenuDetailEditorClick(null, null);

            StatusMessage("ready");
        }

        private void MenuDetailEditorClick(object sender, EventArgs e)
        {
            if (DetailEditor != null)
            {
                DetailEditor.Close();
            }
            else
            {
                ShowSubWindow(DetailEditor = new DetailEditor());
            }
        }

        private void ShowSubWindow(Form f)
        {
            if (Program.MonoMode)
            {
                //if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                //{
                //    f.MdiParent = this;
                //}
                f.Show();
            }
            else
            {
                var dc = f as DockContent;


                dc.Show(dockPanel);
            }
        }

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
                ShowSubWindow(PropertyWindow = new PhotoPropertyWindow());
            }
        }

        private void OpenProjectWindow(Project p)
        {
            Form projectWindow = Program.MonoMode ? (Form)new SimpleProjectWindow(p) : new ProjectWindow(p);
            ShowSubWindow(projectWindow);
        }

        #endregion
    }
}