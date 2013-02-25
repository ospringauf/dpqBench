// -----------------------------------------------------------------------------------------
// DpBench - PictureBoxTransform.cs
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
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Coordinate transformation functions for mapping between screen coords
    /// and image coords (wrt. a zoomed image in a picturebox)
    /// </summary>
    public interface IPictureBoxTransform
    {
        Rectangle ImageRectangle { get; }

        Rectangle ToImageCoords(Rectangle screenRect);

        Point ToImageCoords(Point p);

        Rectangle ToScreenCoords(Rectangle imageRect);

        Size ToScreenCoords(Size imageRect);
    }

    /// <summary>
    /// "Clean" implementation.
    /// Since PictureBox does not expose the size and position of the zoomed image,
    /// we try to re-implement the layout logic here.
    /// </summary>
    class PictureBoxTransformMono : IPictureBoxTransform
    {
        private readonly PictureBox _pictureBox;

        public PictureBoxTransformMono(PictureBox pb)
        {
            _pictureBox = pb;
        }

        public Rectangle ImageRectangle
        {
            get
            {
                // image and container dimensions
                int w_i = _pictureBox.Image.Width;
                int h_i = _pictureBox.Image.Height;
                int w_c = _pictureBox.Width;
                int h_c = _pictureBox.Height;

                float imageRatio = w_i / (float)h_i; // image W:H ratio
                float containerRatio = w_c / (float)h_c; // container W:H ratio

                if (imageRatio >= containerRatio)
                {
                    // horizontal image
                    float scaleFactor = w_c / (float)w_i;
                    float scaledHeight = h_i * scaleFactor;

                    // calculate gap between top of container and top of image
                    float filler = Math.Abs(h_c - scaledHeight) / 2;

                    return new Rectangle(0, (int)filler, (int)(w_i * scaleFactor), (int)(h_i * scaleFactor));
                }
                else
                {
                    // vertical image
                    float scaleFactor = h_c / (float)h_i;
                    float scaledWidth = w_i * scaleFactor;
                    float filler = Math.Abs(w_c - scaledWidth) / 2;
                    return new Rectangle((int)filler, 0, (int)(w_i * scaleFactor), (int)(h_i * scaleFactor));
                }
            }
        }

        public Rectangle ToImageCoords(Rectangle screenRect)
        {
            var p1 = ToImageCoords(new Point(screenRect.X, screenRect.Y));
            var p2 = ToImageCoords(new Point(screenRect.X + screenRect.Width, screenRect.Y + screenRect.Height));
            return new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
        }

        /// <summary>
        /// http://stackoverflow.com/questions/10473582/how-to-retrieve-zoom-factor-of-a-winforms-picturebox
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public Point ToImageCoords(Point p)
        {
            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = _pictureBox.Image.Width;
            int h_i = _pictureBox.Image.Height;
            int w_c = _pictureBox.Width;
            int h_c = _pictureBox.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;

                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(p.X / scaleFactor);
                unscaled_p.Y = (int)((p.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((p.X - filler) / scaleFactor);
                unscaled_p.Y = (int)(p.Y / scaleFactor);
            }

            return unscaled_p;
        }

        public Point ToScreenCoords(Point p)
        {
            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = _pictureBox.Image.Width;
            int h_i = _pictureBox.Image.Height;
            int w_c = _pictureBox.Width;
            int h_c = _pictureBox.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;

                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(p.X * scaleFactor);
                unscaled_p.Y = (int)((p.Y * scaleFactor) + filler);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((p.X * scaleFactor) + filler);
                unscaled_p.Y = (int)(p.Y * scaleFactor);
            }

            return unscaled_p;
        }

        public Rectangle ToScreenCoords(Rectangle imageRect)
        {
            var p1 = ToScreenCoords(new Point(imageRect.X, imageRect.Y));
            var p2 = ToScreenCoords(new Point(imageRect.X + imageRect.Width, imageRect.Y + imageRect.Height));
            return new Rectangle(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y);
        }

        public Size ToScreenCoords(Size imageRect)
        {
            var r = ToScreenCoords(new Rectangle(0, 0, imageRect.Width, imageRect.Height));
            return r.Size;
        }
    }

    /// <summary>
    /// Transforms screen coordinates to/from image coordinates for a 
    /// zoomed image in a picture box.
    /// Not-so-clean implementation: uses reflection to access the private "ImageRectangle"
    /// property of the PictureBox. Fails on Mono.
    /// </summary>
    public class PictureBoxTransform : IPictureBoxTransform
    {
        #region Constants and Fields

        private readonly PropertyInfo _imgRectProperty;

        private readonly PictureBox _pictureBox;

        #endregion

        #region Constructors and Destructors

        public PictureBoxTransform(PictureBox pb)
        {
            _pictureBox = pb;

            // http://stackoverflow.com/questions/3307271/how-to-get-the-value-of-non-public-members-of-picturebox            
            _imgRectProperty = _pictureBox.GetType().GetProperty(
                "ImageRectangle", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// ImageRectangle is a private property in PictureBox, containing the location and size of the (zoomed)
        /// Image. This is very useful for translating the detail areas to screeen coordinates.
        /// 
        /// alternative approach here:
        /// http://stackoverflow.com/questions/10473582/how-to-retrieve-zoom-factor-of-a-winforms-picturebox
        /// </summary>
        public Rectangle ImageRectangle
        {
            get
            {
                return (Rectangle)_imgRectProperty.GetValue(_pictureBox, null);
            }
        }

        #endregion

        #region Properties

        public Size RealImageSize
        {
            get
            {
                return _pictureBox.Image.Size;
            }
        }

        #endregion

        #region Public Methods

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
    }
}