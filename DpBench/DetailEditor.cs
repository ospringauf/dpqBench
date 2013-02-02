// -----------------------------------------------------------------------------------------
// DpBench - DetailEditor.cs
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

    using Paguru.DpBench.Controls;
    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class DetailEditor : DockContent
    {
        #region Constructors and Destructors

        public DetailEditor()
        {
            InitializeComponent();
            //MainWindow.Instance.OnSelectPhoto += ShowPreview;
            pictureBox1.OnDetailSelected += DetailSelected;

            numericUpDownHeight.ValueChanged += (s, ea) =>
                {
                    if (Detail != null)
                    {
                        Detail.Height = (int)numericUpDownHeight.Value;
                        pictureBox1.Invalidate();
                    }
                };
            numericUpDownWidth.ValueChanged += (s, ea) =>
            {
                if (Detail != null)
                {
                    Detail.Width = (int)numericUpDownWidth.Value;
                    pictureBox1.Invalidate();
                }
            };

            // I would have like to data-bind the spinner controls to the crop area, but the value update
            // does not work reliably
            //numericUpDownWidth.DataBindings.Clear();
            //numericUpDownHeight.DataBindings.Clear();
            //numericUpDownWidth.DataBindings.Add("Value", detail, "Width");
            //numericUpDownHeight.DataBindings.Add("Value", detail, "Height");
        }

        #endregion

        #region Properties

        private Photo Photo { get; set; }

        private DetailArea Detail { get; set; }

        #endregion

        #region Methods

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    base.OnFormClosing(e);
        //    MainWindow.Instance.OnSelectPhoto -= ShowPreview;
        //}

        private void DetailSelected(object sender, EventArgs e)
        {
            var dlg = new TextInputMessage() { Text = "Enter a name for the new detail area" };
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var name = dlg.InputText;
                Photo.Project.DetailAreas.Add(new DetailArea() { Crop = pictureBox1.Crop, Name = name });
                Fill();
            }

            // labelCropSize.Text = pictureBox1.Crop.Width + " x " + pictureBox1.Crop.Height;
        }

        private void DrawDetails(object sender, ListChangedEventArgs e)
        {
            Detail = comboBoxCrop.SelectedItem as DetailArea;
            pictureBox1.Invalidate();
        }

        private void Fill()
        {
            if (Photo == null)
            {
                return;
            }
            this.Text = "Details: " + Photo.BaseFilename;
            pictureBox1.Photo = Photo;
            comboBoxCrop.DataSource = Photo.Project.DetailAreas;
            Refresh();

            Photo.Project.DetailAreas.ListChanged += DrawDetails;

            if (pictureBox1.Image != null)
            {
                saveToolStripMenuItem.Text = string.Format(
                    saveToolStripMenuItem.Tag.ToString(), pictureBox1.Image.Width, pictureBox1.Image.Height);
            }
        }

        public void ShowPreview(object sender, PhotoSelectedEvent e)
        {
            Photo = e.Photo;
            Fill();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (comboBoxCrop.SelectedItem != null)
            {
                Photo.Project.DetailAreas.Remove(comboBoxCrop.SelectedItem as DetailArea);
            }
            Fill();
        }

        private void comboBoxCrop_SelectedValueChanged(object sender, EventArgs e)
        {
            Detail = comboBoxCrop.SelectedItem as DetailArea;
            if (Detail != null)
            {
                numericUpDownHeight.Value = Detail.Crop.Height;
                numericUpDownWidth.Value = Detail.Crop.Width;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Detail = comboBoxCrop.SelectedItem as DetailArea;
            if (Detail != null)
            {
                var pimg = Photo.Image;
                var img = ImageConverter.Crop(pimg, Detail.Crop);
                pimg.Dispose();
                new ImagePreview(img).Show(this);
            }
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Fill();
        }

        #endregion

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (var i = pictureBox1.GetImageWithDetailAreas())
                {
                    var fd = new SaveFileDialog() { DefaultExt = ".jpg", Filter = "JPEG images|*.jpg" };
                    if (fd.ShowDialog(this) == DialogResult.OK)
                    {
                        ImageConverter.SaveJpeg(fd.FileName, i, 85);
                    }
                }
            }
            else
            {
                MessageBox.Show("no image");
            }
        }
    }
}