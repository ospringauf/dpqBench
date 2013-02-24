// -----------------------------------------------------------------------------------------
// DpBench - SimpleProjectWindow.cs
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
    using System.Collections.Generic;
    using System.Windows.Forms;

    using BrightIdeasSoftware;

    using Paguru.DpBench.Model;

    /// <summary>
    /// Alternative implementation of the project table window (does not use ObjectListView)
    /// </summary>
    public partial class SimpleProjectWindow : ProjectWindowBase
    {
        #region Constructors and Destructors

        public SimpleProjectWindow(Project project)
        {
            Project = project;
            InitializeComponent();

            dataGridView1.AutoGenerateColumns = false;

            // generate columns from model annotations
            GenerateColumns(dataGridView1, typeof(Photo));

            // objectListView1.CustomSorter = (column, order) =>
            // {
            // objectListView1.ListViewItemSorter = new ProjectColumnComparer(column, order);
            // };
            Text = project.Name ?? "New Project";

            dataGridView1.DataSource = Project.Photos;

            // somehow, the list view does not properly refresh when the list changes .. so let's force it
            Project.Photos.ListChanged += (s, e) => { FillList(); };

            // support drag & drop of (jpeg) files into the project list
            dataGridView1.DragEnter += ProjectWindow_DragEnter;
            dataGridView1.DragDrop += ProjectWindow_DragDrop;

            dataGridView1.SelectionChanged += objectListView1_SelectionChanged;
            dataGridView1.RowStateChanged += OnRowStateChanged;

            BindMenuItems();
        }

        #endregion

        #region Methods

        protected override void FillList()
        {
            dataGridView1.DataSource = Project.Photos;
        }

        private void BindMenuItems()
        {
            // menu items
            addFilesToolStripMenuItem.Click += addFilesToolStripMenuItem_Click;
            createBenchmarkChartToolStripMenuItem.Click += groupLevelEditorToolStripMenuItem_Click;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
        }

        private void GenerateColumns(DataGridView dataGridView, Type type)
        {
            var cols = new List<DataGridViewTextBoxColumn>();

            foreach (var pi in type.GetProperties())
            {
                var attr = Util.GetAttribute<OLVColumnAttribute>(pi);
                if (attr != null)
                {
                    var col = new DataGridViewTextBoxColumn()
                        {
                            DataPropertyName = pi.Name, 
                            HeaderText = attr.Title ?? pi.Name, 
                            DisplayIndex = attr.DisplayIndex, 
                            Width = attr.Width
                        };
                    cols.Add(col);
                }
            }
            cols.Sort((a, b) => a.DisplayIndex - b.DisplayIndex);
            cols.ForEach(col => dataGridView1.Columns.Add(col));
        }

        private Photo GetSelectedPhoto()
        {
            // Mono problem: https://bugzilla.xamarin.com/show_bug.cgi?id=3415
            // SelectedRows will return "0" 
            var sr = dataGridView1.SelectedRows;
            //Console.Out.WriteLine("selection changed: " + sr.Count);
            if (sr.Count <= 0)
            {
                return null;
            }
            var x = sr[0].DataBoundItem as Photo;
            return x;
        }

        private void OnRowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //Console.Out.WriteLine("RowStateChanged: " + e.Row + "/" + e.StateChanged);
            if ((e.StateChanged & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
            {
                var x = e.Row.DataBoundItem as Photo;
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
        }

        // private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        // {
        // var x = GetSelectedPhoto();
        // if (x != null)
        // {
        // Project.Photos.Remove(x);
        // FillList();
        // }
        // }
        private void objectListView1_SelectionChanged(object sender, EventArgs e)
        {
            var x = GetSelectedPhoto();
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

        #endregion
    }
}