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
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Paguru.DpBench.Controls;
    using Paguru.DpBench.Model;

    using WeifenLuo.WinFormsUI.Docking;

    /// <summary>
    /// Editor for detail areas
    /// 
    /// TODO prevent rubber band to be drawn outside of the image
    /// TODO rubber band moves one pixel on each click
    /// </summary>
    public partial class DetailEditor : DockContent
    {
        #region Constants and Fields

        private readonly List<RubberbandControl> _detailControls = new List<RubberbandControl>();

        private readonly Size defaultCropSize = new Size(250, 250);

        private readonly ToolTip toolTip;

        private IPictureBoxTransform _pbt;

        #endregion

        #region Constructors and Destructors

        public DetailEditor()
        {
            InitializeComponent();
            pictureBox1.ContextMenuStrip = contextMenuStrip1;
            toolTip = new ToolTip();

            //_pbt = Program.MonoMode ? (IPictureBoxTransform)new PictureBoxTransformMono(pictureBox1) : new PictureBoxTransform(pictureBox1);
            _pbt = new PictureBoxTransformMono(pictureBox1);

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // on left click, add new detail control
            pictureBox1.MouseClick += AddNewRubberbandOnLeftClick;

            // when image is zoomed or moved in the picture box, update the ImageRectangle and adjust the
            // sizes and positions of the details
            pictureBox1.SizeChanged += (s, e) =>
                {
                    // translate the detail rubberbands to the new coordinates
                    foreach (var panel in _detailControls)
                    {
                        var r = _pbt.ToScreenCoords(panel.DetailArea.Crop);
                        panel.Location = r.Location;
                        panel.Size = r.Size;
                    }
                };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current photo.
        /// </summary>
        private Photo Photo { get; set; }

        /// <summary>
        /// Gets or sets the currently selected detail area.
        /// </summary>
        private DetailArea SelectedDetailArea { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Paints an overview image with detail rectangle areas painted onto it (same size as current display).
        /// </summary>
        /// <returns></returns>
        public Image PaintOverviewImage()
        {
            using (var pen = new Pen(Color.Orange, 3))
            {
                Bitmap b = new Bitmap(_pbt.ImageRectangle.Width, _pbt.ImageRectangle.Height);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    // scale the image to picturebox size
                    var f = ImageConverter.ScaleFactor(pictureBox1.Image.Size, _pbt.ImageRectangle.Size);
                    var scaledImage = ImageConverter.ResizeImage(pictureBox1.Image, _pbt.ImageRectangle.Size);
                    g.DrawImage(scaledImage, 0, 0);
                    scaledImage.Dispose();
                    foreach (var detail in Photo.Project.DetailAreas)
                    {
                        g.DrawRectangle(pen, ImageConverter.ScaleRectangle(detail.Crop, f));
                    }
                    return b;
                }
            }
        }

        /// <summary>
        /// Shows the preview for the specified image (selected in the project window)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public void ShowPreview(object sender, PhotoSelectedEvent e)
        {
            Photo = e.Photo;
            pictureBox1.Image = Photo.Image;
            this.Text = "Details: " + Photo.BaseFilename;

            // reset / initialize rubber band controls
            _detailControls.ForEach(RemoveRubberbandControl);
            _detailControls.Clear();
            foreach (var detailArea in Photo.Project.DetailAreas)
            {
                var r = _pbt.ToScreenCoords(detailArea.Crop);
                var p = new RubberbandControl() { Location = r.Location, Size = r.Size, DetailArea = detailArea };
                AddRubberbandControl(p);
            }
        }


        #endregion

        #region Methods

        private void AddNewRubberbandOnLeftClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || Photo == null)
            {
                return;
            }

            // initial size and position of the detail area
            var crop = new Rectangle(_pbt.ToImageCoords(e.Location), defaultCropSize);
                
            var newDetail = new DetailArea() { Name = "area" + _detailControls.Count, Crop = crop };
            Photo.Project.DetailAreas.Add(newDetail);

            var p = new RubberbandControl()
                {
                    Location = e.Location,
                    Size = _pbt.ToScreenCoords(defaultCropSize),
                    DetailArea = newDetail 
                };

            AddRubberbandControl(p);
        }

        private void AddRubberbandControl(RubberbandControl p)
        {
            pictureBox1.Controls.Add(p);

            // move new detail control to front so that it appears on top (z-order)
            pictureBox1.Controls.SetChildIndex(p, 0);

            // update image coordinates after move/resize operation
            p.RubberbandMoved += UpdateRubberbandCoordinatesMove;
            p.RubberbandSizeChanged += UpdateRubberbandCoordinates;

            // show current detail in editor bar
            p.MouseClick += SelectDetailArea;
            p.SizeChanged += SelectDetailArea;
            p.RubberbandMoved += SelectDetailArea;
            p.RubberbandSizeChanged += SelectDetailArea;

            // set coordinate translation methods
            p.Transform = _pbt;

            _detailControls.Add(p);

            // context menu
            var deleteCommand = new ToolStripMenuItem("Delete");
            var renameCommand = new ToolStripMenuItem("Rename ...");
            var previewCommand = new ToolStripMenuItem("Preview 1:1");
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(deleteCommand);
            contextMenu.Items.Add(renameCommand);
            contextMenu.Items.Add(previewCommand);

            // delete DetailArea from project and dispose the rubberband control
            deleteCommand.Click += (s, ev) => DeleteDetail(p);

            // open rename dialog
            renameCommand.Click += (s, ev) =>
                {
                    var dlg = new TextInputMessage() { InputText = p.DetailArea.Name, Text = "Detail Area Name" };
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        p.DetailArea.Name = dlg.InputText;
                        toolTip.SetToolTip(p, p.DetailArea.Name);
                    }
                };

            // create 100% preview of this area
            previewCommand.Click += (s, ev) =>
                {
                    var pimg = pictureBox1.Image;
                    var img = ImageConverter.Crop(pimg, p.DetailArea.Crop);
                    new ImagePreview(img).Show(this);
                };

            p.ContextMenuStrip = contextMenu;

            // tool tip
            toolTip.SetToolTip(p, p.DetailArea.Name);
        }

        private void DeleteDetail(RubberbandControl p)
        {
            Photo.Project.DetailAreas.Remove(p.DetailArea);
            RemoveRubberbandControl(p);
            _detailControls.Remove(p);
        }

        private void RemoveRubberbandControl(RubberbandControl p)
        {
            Controls.Remove(p);
            p.DetailArea = null; // release propery listener
            p.Dispose();
        }

        private void SelectDetailArea(object sender, EventArgs e)
        {
            SelectDetailArea(sender as RubberbandControl);
        }

        private void SelectDetailArea(RubberbandControl p)
        {
            SelectedDetailArea = p.DetailArea;
            textBoxDetailName.Text = SelectedDetailArea.Name;
            numericUpDownWidth.Value = SelectedDetailArea.Crop.Width;
            numericUpDownHeight.Value = SelectedDetailArea.Crop.Height;
        }

        /// <summary>
        /// Updates the detail coordinates when the rubberband control was resized.
        /// </summary>
        private void UpdateRubberbandCoordinates(object sender, EventArgs e)
        {
            //Console.Out.WriteLine("UpdateRubberbandCoordinates");
            var p = sender as RubberbandControl;
            p.DetailArea.Crop = _pbt.ToImageCoords(new Rectangle(p.Location, p.Size));
        }

        /// <summary>
        /// Updates the detail coordinates when the rubberband control was moved.
        /// Does not update the size to avoid rounding errors.
        /// </summary>
        private void UpdateRubberbandCoordinatesMove(object sender, EventArgs e)
        {
            //Console.Out.WriteLine("UpdateRubberbandCoordinatesMove");
            var p = sender as RubberbandControl;

            var oldSize = p.DetailArea.Crop.Size;
            p.DetailArea.Crop = _pbt.ToImageCoords(new Rectangle(p.Location, p.Size));

            // keep old size
            p.DetailArea.Crop = new Rectangle(p.DetailArea.Crop.Location, oldSize);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (SelectedDetailArea != null)
            {
                SelectedDetailArea.Name = textBoxDetailName.Text;
                SelectedDetailArea.Height = (int)numericUpDownHeight.Value;
                SelectedDetailArea.Width = (int)numericUpDownWidth.Value;
                _detailControls.ForEach(p => toolTip.SetToolTip(p, p.DetailArea.Name));
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (SelectedDetailArea != null)
            {
                var p = _detailControls.Find(x => x.DetailArea == SelectedDetailArea);
                if (p != null)
                {
                    DeleteDetail(p);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (var i = PaintOverviewImage())
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

        #endregion
    }
}