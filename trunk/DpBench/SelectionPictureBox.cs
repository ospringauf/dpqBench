// -----------------------------------------------------------------------------------------
// DpBench - SelectionPictureBox.cs
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
    using System.Drawing;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    public partial class SelectionPictureBox : PictureBox
    {
        #region Constants and Fields

        private Photo _photo;

        private double _scaleFactor;

        private bool bHaveMouse;

        private Point ptLast = new Point();

        private Point ptOriginal = new Point();

        #endregion

        #region Constructors and Destructors

        public SelectionPictureBox()
        {
            InitializeComponent();

            // SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            MouseDown += new MouseEventHandler(MyMouseDown);
            MouseUp += new MouseEventHandler(MyMouseUp);
            MouseMove += new MouseEventHandler(MyMouseMove);
            bHaveMouse = false;
        }

        #endregion

        #region Public Events

        public event EventHandler<EventArgs> OnDetailSelected;

        #endregion

        #region Public Properties

        public Rectangle Crop { get; set; }

        public Photo Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                _photo = value;
                if (_photo != null)
                {
                    var img = Image.FromFile(_photo.Filename);
                    _scaleFactor = ImageConverter.ScaleFactor(img.Size, Size);
                    img = ImageConverter.resizeImage(img, Size);
                    Image = img;
                }
            }
        }

        #endregion

        // Called when the left mouse button is pressed. 
        #region Public Methods

        public void MyMouseDown(object sender, MouseEventArgs e)
        {
            // Make a note that we "have the mouse".
            bHaveMouse = true;

            // Store the "starting point" for this rubber-band rectangle.
            ptOriginal.X = e.X;
            ptOriginal.Y = e.Y;

            // Special value lets us know that no previous
            // rectangle needs to be erased.
            ptLast.X = -1;
            ptLast.Y = -1;
        }

        public void MyMouseMove(object sender, MouseEventArgs e)
        {
            Point ptCurrent = new Point(e.X, e.Y);

            // If we "have the mouse", then we draw our lines.
            if (bHaveMouse)
            {
                // If we have drawn previously, draw again in
                // that spot to remove the lines.
                if (ptLast.X != -1)
                {
                    MyDrawReversibleRectangle(ptOriginal, ptLast);
                }

                // Update last point.
                ptLast = ptCurrent;

                // Draw new lines.
                MyDrawReversibleRectangle(ptOriginal, ptCurrent);
            }
        }

        // Convert and normalize the points and draw the reversible frame.

        // Called when the left mouse button is released.
        public void MyMouseUp(object sender, MouseEventArgs e)
        {
            // Set internal flag to know we no longer "have the mouse".
            bHaveMouse = false;

            // If we have drawn previously, draw again in that spot
            // to remove the lines.
            if (ptLast.X != -1)
            {
                Point ptCurrent = new Point(e.X, e.Y);
                StoreRect(ptOriginal, ptCurrent);
                MyDrawReversibleRectangle(ptOriginal, ptLast);
            }

            // Set flags to know that there is no "previous" line to reverse.
            ptLast.X = -1;
            ptLast.Y = -1;
            ptOriginal.X = -1;
            ptOriginal.Y = -1;
        }

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (Photo != null)
            {
                foreach (var detail in Photo.Project.DetailAreas)
                {
                    var pen = new Pen(Color.Orange, 3);
                    pe.Graphics.DrawRectangle(pen, ImageConverter.ScaleRectangle(detail.Crop, _scaleFactor));
                }
            }
        }

        private void MyDrawReversibleRectangle(Point p1, Point p2)
        {
            Rectangle rc = new Rectangle();

            // Convert the points to screen coordinates.
            p1 = PointToScreen(p1);
            p2 = PointToScreen(p2);

            // Normalize the rectangle.
            if (p1.X < p2.X)
            {
                rc.X = p1.X;
                rc.Width = p2.X - p1.X;
            }
            else
            {
                rc.X = p2.X;
                rc.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                rc.Y = p1.Y;
                rc.Height = p2.Y - p1.Y;
            }
            else
            {
                rc.Y = p2.Y;
                rc.Height = p1.Y - p2.Y;
            }

            // Draw the reversible frame.
            ControlPaint.DrawReversibleFrame(rc, Color.Yellow, FrameStyle.Thick);
        }

        private void StoreRect(Point p1, Point p2)
        {
            if (Image == null)
            {
                return;
            }
            var p0 = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            var sz = new Size(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));
            Crop = new Rectangle(p0, sz);
            Crop = Rectangle.Intersect(Crop, new Rectangle(new Point(0, 0), Image.Size));

            // transform to image coordinates
            Crop = ImageConverter.ScaleRectangle(Crop, 1.0 / _scaleFactor);
            if (OnDetailSelected != null)
            {
                OnDetailSelected(this, new EventArgs());
            }
        }

        #endregion

        // Called when the mouse is moved.
    }
}