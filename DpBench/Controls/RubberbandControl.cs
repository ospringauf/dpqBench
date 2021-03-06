// -----------------------------------------------------------------------------------------
// DpBench - RubberbandControl.cs
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

namespace Paguru.DpBench.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    /// <summary>
    /// A moveable, sizeable rubber-band control to select a detail area of the underlying image
    /// see http://social.msdn.microsoft.com/forums/en-US/winforms/thread/55509105-949f-404e-819b-12d38aebecfd/
    /// 
    /// Implemented as a transparent panel with a "resize" handle area in its SE corner.
    /// The "real" image detail data are stored in the "DetailArea" member.
    /// </summary>
    internal class RubberbandControl : Panel
    {
        #region Constants and Fields

        private const int MoveHandleArea = 15;

        private DetailArea detailArea;

        private bool moving = false;

        private bool sizing = false;

        private bool wasMoved;

        private bool wasSized;

        private Point startLoc;

        private Size startSize;

        #endregion

        #region Constructors and Destructors

        public RubberbandControl()
        {
            BorderStyle = BorderStyle.FixedSingle;

            // fill with 40% alpha blend / transparency
            BackColor = Color.FromArgb((int)(0.4 * 255), Color.Orange);

            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint, 
                true);

            MouseDown += (s, e) =>
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        return;
                    }
                    sizing = InMoveHandleArea(e);
                    moving = !sizing;
                    wasMoved = wasSized = false;
                    startLoc = e.Location;
                    startSize = Size;

                    //Console.Out.WriteLine("mousedown: " + startLoc);
                };

            MouseUp += (s, e) =>
                {
                    if (e.Button != MouseButtons.Left)
                    {
                        return;
                    }
                    
                    moving = false;
                    sizing = false;
                    Cursor = Cursors.Default;

                    if (wasSized)
                    {
                        RubberbandSizeChanged(this, e);
                    } 
                    if (wasMoved)
                    {
                        RubberbandMoved(this, e);
                    }
                };

            MouseMove += (sender, e) =>
                {
                    //Console.Out.WriteLine("mousemove: " + e.Location + " moving:" + moving + " sizing:" + sizing);
                    Point newLocationOffset = e.Location - new Size(startLoc);
                    var ctrl = (Control)sender;

                    if (moving)
                    {   
                        // are we still within the image bounds?
                        var newControlRect =
                                    new Rectangle(
                                        ctrl.Left + newLocationOffset.X,
                                        ctrl.Top + newLocationOffset.Y,
                                        ctrl.Width,
                                        ctrl.Height);

                        var leavingImage = !IsInsideImage(newControlRect);
                        if (leavingImage)
                        {
                            return;
                        }

                        ctrl.Left += newLocationOffset.X;
                        ctrl.Top += newLocationOffset.Y;
                        wasMoved = true;
                    }
                    else if (sizing)
                    {
                        // are we still within the image bounds?
                        var newControlRect =
                                    new Rectangle(
                                        ctrl.Left,
                                        ctrl.Top,
                                        startSize.Width + newLocationOffset.X,
                                        startSize.Height + newLocationOffset.Y);

                        var leavingImage = !IsInsideImage(newControlRect);
                        if (leavingImage)
                        {
                            return;
                        }


                        ctrl.Width = startSize.Width + newLocationOffset.X;
                        ctrl.Height = startSize.Height + newLocationOffset.Y;

                        // if ctrl key is pressed, make the area a square
                        if ((Control.ModifierKeys & Keys.Control) != Keys.None)
                        {
                            ctrl.Width = Math.Max(ctrl.Height, ctrl.Width);
                            ctrl.Height = Math.Max(ctrl.Height, ctrl.Width);
                        }

                        // don't make the control too small
                        ctrl.Width = Math.Max(MoveHandleArea, ctrl.Width);
                        ctrl.Height = Math.Max(MoveHandleArea, ctrl.Height);

                        wasSized = true;
                    }
                    else
                    {
                        // is the cursor in the lower right corner of the control?
                        Cursor = InMoveHandleArea(e) ? Cursors.SizeNWSE : Cursors.SizeAll;
                    }
                };
        }

        private bool InMoveHandleArea(MouseEventArgs e)
        {
            return Width - e.X < MoveHandleArea && Height - e.Location.Y < MoveHandleArea;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when the rubberband control has been moved.
        /// </summary>
        public event EventHandler<EventArgs> RubberbandMoved;

        /// <summary>
        /// Occurs when the rubberband control's size has changed.
        /// </summary>
        public event EventHandler<EventArgs> RubberbandSizeChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Coordinate transformation 
        /// </summary>
        public IPictureBoxTransform Transform { get; set; }

        /// <summary>
        /// Gets or sets the detail area represented by this rubberband control.
        /// </summary>
        public DetailArea DetailArea
        {
            get
            {
                return detailArea;
            }
            set
            {
                if (detailArea != null)
                {
                    detailArea.PropertyChanged -= DetailChanged;
                }
                detailArea = value;
                if (detailArea != null)
                {
                    detailArea.PropertyChanged += DetailChanged;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// adjust the control size and position if the detail area was modified
        /// </summary>
        private void DetailChanged(object sender, PropertyChangedEventArgs e)
        {
            //Console.Out.WriteLine("DetailChanged");
            var r = Transform.ToScreenCoords(DetailArea.Crop);
            Location = r.Location + new Size(1, 1); // why 1,1? control border?
            Size = r.Size;

            // RubberbandSizeChanged(this, null);
        }


        /// <summary>
        /// Determines whether the specified rectangle r lies completely inside the 
        /// picture box's image bounds (via rect intersect)
        /// </summary>
        private bool IsInsideImage(Rectangle r)
        {
            return Rectangle.Intersect(r, Transform.ImageRectangle) == r;
        }

        #endregion
    }
}