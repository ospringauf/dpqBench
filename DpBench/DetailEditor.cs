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
    using System.Drawing;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    public partial class DetailEditor : DockContent
    {
        #region Constructors and Destructors

        public DetailEditor()
        {
            InitializeComponent();
            MainWindow.Instance.OnSelectPhoto += ShowPreview;
            pictureBox1.OnDetailSelected += DetailSelected;
        }

        #endregion

        #region Properties

        private Photo Photo { get; set; }

        #endregion

        #region Methods

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            MainWindow.Instance.OnSelectPhoto -= ShowPreview;
        }

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
        }

        private void ShowPreview(object sender, PhotoSelectedEvent e)
        {
            Photo = e.Photo;
            Fill();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Photo.Project.DetailAreas.Remove(comboBoxCrop.SelectedItem as DetailArea);
            Fill();
        }

        private void comboBoxCrop_SelectedValueChanged(object sender, EventArgs e)
        {
            var detail = comboBoxCrop.SelectedItem as DetailArea;
            numericUpDownWidth.DataBindings.Clear();
            numericUpDownHeight.DataBindings.Clear();
            numericUpDownWidth.DataBindings.Add("Value", detail, "Width");
            numericUpDownHeight.DataBindings.Add("Value", detail, "Height");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var detail = comboBoxCrop.SelectedItem as DetailArea;
            var img = Photo.Image;
            img = ImageConverter.Crop(img, detail.Crop);
            img = ImageConverter.resizeImage(img, new Size(200, 200));
            new ImagePreview(img).Show(this);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var detail = comboBoxCrop.SelectedItem as DetailArea;
            var img = Photo.Image;
            img = ImageConverter.Crop(img, detail.Crop);
            new ImagePreview(img).Show(this);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var project = Photo.Project;

            var maxBoxSize = project.MaxCropSize();
            var scaledBoxSize = ImageConverter.FitInto(maxBoxSize, new Size(300, 300), true);
            Console.WriteLine("max box size: " + maxBoxSize);
            Console.WriteLine("scaled box size: " + scaledBoxSize);

            var all = project.CreateAllDetails();

            foreach (var lens in all.FindAllValues("Lens"))
            {
                Console.Out.WriteLine(lens);
                var l1 = all.FindAll(pd => pd.Parameters["Lens"] == lens);

                foreach (var detail in all.FindAllValues("Detail"))
                {
                    Console.Out.WriteLine("\t" + detail);
                    var l2 = l1.FindAll(pd => pd.Parameters["Detail"] == detail);

                    foreach (var aperture in all.FindAllValues("Aperture"))
                    {
                        var x = l2.FindAll(pd => pd.Parameters["Aperture"] == aperture);
                        var crop = (x.Count == 1) ? x[0].ToString() : "!!!" + x.Count + "!!!";
                        Console.Out.WriteLine("\t\t" + aperture + "=" + crop);
                    }
                }
            }
        }

        private void linkLabelApertureSeries_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var detail = comboBoxCrop.SelectedItem as DetailArea;
            var boxes = Photo.Project.Photos.Count;
            var spacing = 1;

            // calc size of individual images, fit into 300x300 bounding box
            var boxSize = ImageConverter.FitInto(detail.Crop.Size, new Size(300, 300), true);
            var stripSize = new Size(boxes * (boxSize.Width + spacing), boxSize.Height);

            Bitmap b = new Bitmap(stripSize.Width, stripSize.Height);
            Graphics g = Graphics.FromImage((Image)b);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(new Point(0, 0), stripSize));

            Font a = new Font("Calibri", 16, FontStyle.Regular);

            var destRect = new Rectangle(new Point(0, 0), boxSize);
            foreach (var photo in Photo.Project.Photos)
            {
                var img = photo.Image;
                img = ImageConverter.Crop(img, detail.Crop);
                img = ImageConverter.resizeImage(img, boxSize);
                g.DrawImage(img, destRect.Location);

                string fvalue = "f/" + photo.Aperture;
                g.DrawString(fvalue, a, new SolidBrush(Color.DarkSlateGray), destRect.Location + new Size(2, 2));
                g.DrawString(fvalue, a, new SolidBrush(Color.White), destRect.Location);

                // move target rect to the left
                destRect.Offset(boxSize.Width + spacing, 0);
            }

            g.Dispose();
            var strip = (Image)b;
            new ImagePreview(strip).Show(this);
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Fill();
        }

        #endregion
    }
}