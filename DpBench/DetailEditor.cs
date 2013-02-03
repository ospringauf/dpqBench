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
    using System.Reflection;
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

        private readonly PropertyInfo _imgRectProperty;

        private readonly Size defaultCropSize = new Size(250, 250);

        private readonly ToolTip toolTip;

        #endregion

        #region Constructors and Destructors

        public DetailEditor()
        {
            InitializeComponent();
            pictureBox1.ContextMenuStrip = contextMenuStrip1;
            toolTip = new ToolTip();

            // http://stackoverflow.com/questions/3307271/how-to-get-the-value-of-non-public-members-of-picturebox            
            _imgRectProperty = pictureBox1.GetType().GetProperty(
                "ImageRectangle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

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
                        var r = ToScreenCoords(panel.DetailArea.Crop);
                        panel.Location = r.Location;
                        panel.Size = r.Size;
                    }
                };
        }

        #endregion

        #region Properties

        /// <summary>
        /// ImageRectangle is a private property in PictureBox, containing the location and size of the (zoomed)
        /// Image. This is very useful for translating the detail areas to screeen coordinates.
        /// 
        /// alternative approach here:
        /// http://stackoverflow.com/questions/10473582/how-to-retrieve-zoom-factor-of-a-winforms-picturebox
        /// </summary>
        private Rectangle ImageRectangle
        {
            get
            {
                return (Rectangle)_imgRectProperty.GetValue(pictureBox1, null);
            }
        }

        private Photo Photo { get; set; }

        private Size RealImageSize
        {
            get
            {
                return pictureBox1.Image.Size;
            }
        }

        private DetailArea SelectedDetailArea { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the image with detail rectangle areas painted onto it (same size as current display).
        /// </summary>
        /// <returns></returns>
        public Image GetImageWithDetailAreas()
        {
            using (var pen = new Pen(Color.Orange, 3))
            {
                Bitmap b = new Bitmap(ImageRectangle.Width, ImageRectangle.Height);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    // scale the image to picturebox size
                    var f = ImageConverter.ScaleFactor(pictureBox1.Image.Size, ImageRectangle.Size);
                    var scaledImage = ImageConverter.ResizeImage(pictureBox1.Image, ImageRectangle.Size);
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
                var r = ToScreenCoords(detailArea.Crop);
                var p = new RubberbandControl() { Location = r.Location, Size = r.Size, DetailArea = detailArea };
                AddRubberbandControl(p);
            }
        }

        public Rectangle ToImageCoords(Rectangle screenRect)
        {
            var scalef = ImageConverter.ScaleFactor(RealImageSize, ImageRectangle.Size);
            var result = new Rectangle(
                screenRect.X - ImageRectangle.X, screenRect.Y - ImageRectangle.Y, screenRect.Width, screenRect.Height);
            result = ImageConverter.ScaleRectangle(result, 1.0 / scalef);
            return result;
        }

        public Point ToImageCoords(Point p)
        {
            var scalef = ImageConverter.ScaleFactor(RealImageSize, ImageRectangle.Size);
            var result = new Point((int)((p.X - ImageRectangle.X) / scalef), (int)((p.Y - ImageRectangle.Y) / scalef));

            return result;
        }

        public Rectangle ToScreenCoords(Rectangle imageRect)
        {
            var scalef = ImageConverter.ScaleFactor(RealImageSize, ImageRectangle.Size);
            var result = ImageConverter.ScaleRectangle(imageRect, scalef);
            result.Offset(ImageRectangle.Location);
            return result;
        }

        public Size ToScreenCoords(Size imageRect)
        {
            var scalef = ImageConverter.ScaleFactor(RealImageSize, ImageRectangle.Size);
            var result = ImageConverter.ScaleRectangle(new Rectangle(0, 0, imageRect.Width, imageRect.Height), scalef);
            return result.Size;
        }

        #endregion

        #region Methods

        private void AddNewRubberbandOnLeftClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || Photo == null)
            {
                return;
            }

            var crop = new Rectangle(ToImageCoords(e.Location), defaultCropSize);
                
                // ToImageCoords(new Rectangle(p.Location, p.Size));
            var newDetail = new DetailArea() { Name = "area" + _detailControls.Count, Crop = crop };
            Photo.Project.DetailAreas.Add(newDetail);

            var p = new RubberbandControl()
                {
                   Location = e.Location, Size = ToScreenCoords(defaultCropSize), DetailArea = newDetail 
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

            // set coordinate translation method
            p.ToScreenCoords = ToScreenCoords;
            p.ToImageCoords = ToImageCoords;
            //p.IntersectImage = (r) => { return Rectangle.Intersect(r, new Rectangle(new Point(0, 0), RealImageSize)); };
            p.IntersectImage = (r) => Rectangle.Intersect(r, ImageRectangle);

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

            // var r = new Rectangle(p.Location, p.Size);
            p.DetailArea.Crop = ToImageCoords(new Rectangle(p.Location, p.Size));
        }

        /// <summary>
        /// Updates the detail coordinates when the rubberband control was moved.
        /// Does not update the size to avoid rounding errors.
        /// </summary>
        private void UpdateRubberbandCoordinatesMove(object sender, EventArgs e)
        {
            //Console.Out.WriteLine("UpdateRubberbandCoordinatesMove");
            var p = sender as RubberbandControl;

            // var r = new Rectangle(p.Location, p.Size);
            var oldSize = p.DetailArea.Crop.Size;
            p.DetailArea.Crop = ToImageCoords(new Rectangle(p.Location, p.Size));

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
            var p = _detailControls.Find(x => x.DetailArea == SelectedDetailArea);
            DeleteDetail(p);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (var i = GetImageWithDetailAreas())
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