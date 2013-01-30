// -----------------------------------------------------------------------------------------
// DpBench - YxImageTableRenderer.cs
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

namespace Paguru.DpBench.Renderer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Linq;

    using Paguru.DpBench.Model;

    using ImageConverter = Paguru.DpBench.ImageConverter;

    /// <summary>
    /// Renders detail images into a destination image in table layout.
    /// The last filter level defines the columns, the previous filter levels define the rows and row groups.
    /// Labels will be written into the image tiles (last level) or at the left border (hiher levels).
    /// Typical appliation is: (Detail) -> (Lens) -> (Aperture)
    /// </summary>
    public class YxImageTableRenderer : IRenderer
    {
        public Size BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the actual size of the individual tiles.
        /// </summary>
        private Size TileSize { get; set; }

        private int TextHeight { get; set; }

        public int Padding { get; set; }

        public Font LabelFont { get; set; }

        public bool ScaleToBoundingBox { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YxImageTableRenderer"/> class.
        /// </summary>
        /// <param name="p">The project</param>
        public YxImageTableRenderer(Project p)
        {
            TileSize = p.MaxCropSize();
            LabelFont = new Font("Calibri", 12, FontStyle.Bold);
            Padding = 2;
            BoundingBox = new Size(250, 250);
            ScaleToBoundingBox = true;
        }

        /// <summary>
        /// Renders a photo detail comparison as defined by the grouping filter
        /// </summary>
        /// <param name="f">The grouping/filter</param>
        /// <returns>
        /// the result of the rendering (eg. an <see cref="Image"/>
        /// </returns>
        public object Render(GroupFilter f)
        {
            using (var ic = ImageCache.CreateCache())
            {
                MainWindow.Instance.ScaleProgress("rendering", f.TotalTiles);
                TextHeight = CalcTextHeight() + (2 * Padding);
                var table = RenderGroup(f, f.Input);
                MainWindow.Instance.ClearProgress();
                return table;
            }
        }

        private int CalcTextHeight()
        {
            using (Bitmap b = new Bitmap(100, 100))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    var sizef = g.MeasureString("dqdb", LabelFont);
                    return (int)sizef.Height;
                }
            }
        }

        /// <summary>
        /// Renders a group. The selected parameter names are written (-90° ccw) at the left, the images
        /// are displayed on the right.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private Image RenderGroup(GroupFilter f, PhotoDetailCollection input = null)
        {
            Console.Out.WriteLine("rendering " + f);
            if (f.IsLast)
            {
                return RenderRow(f, input);
            }
            else
            {
                var rowImages = new List<Image>();
                foreach (var pv in f.ParameterValues.SelectedValues)
                {
                    Console.Out.WriteLine("--> " + f.Parameter + "=" + pv);
                    var renderedRows = RenderGroup(f.NextGroupFilter, f.Filter(pv, input));
                    rowImages.Add(renderedRows);
                }

                // calculate total size of group
                var h = rowImages.Sum(rowImage => rowImage.Height) + (Padding * (rowImages.Count + 1));
                var w = rowImages.Max(rowImage => rowImage.Width);

                // create destination bitmap: text | rowImage
                var tableSize = new Size(TextHeight + (3 * Padding) + w, h);

                Bitmap b = new Bitmap(tableSize.Width, tableSize.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.FillRectangle(new SolidBrush(Color.White), new Rectangle(new Point(0, 0), tableSize));

                    var destRect = new Rectangle(Padding, Padding, TextHeight, 1);
                    for (int i = 0; i < f.ParameterValues.SelectedValues.Count; i++)
                    {
                        // draw text
                        var text = f.ParameterValues.SelectedValues[i];
                        destRect.Height = rowImages[i].Height;
                        RenderLabel(g, text, destRect, true);

                        // draw strips
                        destRect.Offset(TextHeight + Padding, 0);
                        g.DrawImage(rowImages[i], destRect.Location);
                        rowImages[i].Dispose();

                        // move to next row (left edge and down)
                        destRect.Offset((-1 * destRect.X) + Padding, destRect.Height + Padding);
                    }

                    var strip = (Image)b;
                    return strip;
                }
            }
        }

        /// <summary>
        /// Renders last group level in horizontal direction, as a strip of detail tiles
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private Image RenderRow(GroupFilter f, PhotoDetailCollection input)
        {
            var boxes = f.ParameterValues.SelectedValues.Count;

            // calc size of individual images, fit into configured bounding box
            var boxSize = ScaleToBoundingBox ? ImageConverter.FitInto(TileSize, BoundingBox, true) : TileSize;
            var stripSize = new Size(boxes * (boxSize.Width + Padding), boxSize.Height);

            Bitmap b = new Bitmap(stripSize.Width, stripSize.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(new Point(0, 0), stripSize));

                var destRect = new Rectangle(new Point(0, 0), boxSize);

                // fill the image tiles in X-direction
                foreach (var pv in f.ParameterValues.SelectedValues)
                {
                    // g.FillRectangle(new SolidBrush(Color.GreenYellow), destRect);

                    var tileLabel = string.Empty;
                    var tiles = f.Filter(pv, input);
                    if (tiles.Count == 0)
                    {
                        // no matching tile image, leave empty
                    }
                    else if (tiles.Count > 1)
                    {
                        // more than one matching image, write warning
                        tileLabel = pv + ": " + tiles.Count + " matches";
                    }
                    else
                    {
                        // exactly one matching image
                        var photoDetail = tiles[0];
                        using (var tileImg = photoDetail.Image)
                        {
                            using (var scaledTileImg = ImageConverter.resizeImage(tileImg, boxSize))
                            {
                                g.DrawImage(scaledTileImg, destRect.Location);
                            }
                        }

                        tileLabel = pv;
                    }

                    if (!string.IsNullOrEmpty(tileLabel))
                    {
                        RenderLabel(g, tileLabel, destRect, false);
                    }

                    // move target rect to the left
                    destRect.Offset(boxSize.Width + Padding, 0);
                    MainWindow.Instance.Progress++;
                }

                var strip = (Image)b;
                return strip;
            }
        }

        /// <summary>
        /// Renders the label into the specified rectangle.
        /// </summary>
        /// <param name="g">The graphics</param>
        /// <param name="labelText">The label text.</param>
        /// <param name="destRect">The destination rectangle</param>
        /// <param name="rotate">if set to <c>true</c> [rotate -90° ccw].</param>
        private void RenderLabel(Graphics g, string labelText, Rectangle destRect, bool rotate)
        {
            var sizef = g.MeasureString(labelText, LabelFont);
            
            //// do we have to rotate the string (-90° CCW) to fit into the destination rectangle?
            //bool rotate = sizef.Width > destRect.Width && sizef.Width <= destRect.Height;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (rotate)
            {
                // rotate label by -90° and align to top right corner
                // g.FillRectangle(new SolidBrush(Color.LightGray), destRect); // debugging
                var lgb = new LinearGradientBrush(destRect, Color.DarkSlateGray, Color.White, 90);
                g.FillRectangle(lgb, destRect.Right - 2, destRect.Top, 1, destRect.Height);

                // see http://stackoverflow.com/questions/4460258/c-rotated-text-align
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Far;

                //// 90 degrees
                //g.TranslateTransform(destRect.Width, destRect.Y);
                //g.RotateTransform(90);
                //g.DrawString(labelText, LabelFont, Brushes.Black, new RectangleF(0, 0, destRect.Height, destRect.Width), format);
                //g.ResetTransform(); 

                // -90 degrees
                g.TranslateTransform(destRect.X, destRect.Bottom);
                g.RotateTransform(270);
                g.DrawString(labelText, LabelFont, Brushes.Black, new RectangleF(0, 0, destRect.Height, destRect.Width), format);
                g.ResetTransform();
            }
            else
            {
                // align tile label to bottom right corner
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Far;
                var textRect = new Rectangle(destRect.Location, destRect.Size);
                textRect.Offset(-1 * Padding, textRect.Height - (int)sizef.Height);
                //g.DrawString(labelText, LabelFont, new SolidBrush(Color.DarkSlateGray), destRect.Location + new Size(Padding, Padding));
                //g.DrawString(labelText, LabelFont, new SolidBrush(Color.White), destRect.Location);

                g.DrawString(labelText, LabelFont, Brushes.DarkSlateGray, textRect, format);
                textRect.Offset(-1, -1);
                g.DrawString(labelText, LabelFont, Brushes.White, textRect, format);
            }
        }
    }
}