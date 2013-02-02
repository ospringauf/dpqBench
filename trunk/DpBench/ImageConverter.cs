// -----------------------------------------------------------------------------------------
// DpBench - ImageConverter.cs
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
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

    internal class ImageConverter
    {
        #region Constants and Fields

        public int imageHeight;

        public int imageQuality = 80;

        public int imageWidth;

        public ProgressBar progress;

        public bool resizeImages;

        public bool resizeThumbs;

        public int thumbHeight;

        public int thumbQuality = 80;

        public int thumbWidth;

        private bool overwriteAllowed = false; // false = unknown

        #endregion

        #region Public Methods

        public static Image Crop(Image imgToCrop, Rectangle detail)
        {
            Rectangle destRect = new Rectangle(new Point(0, 0), detail.Size);

            Bitmap b = new Bitmap(detail.Size.Width, detail.Size.Height);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.DrawImage(imgToCrop, destRect, detail, GraphicsUnit.Pixel);
                return (Image)b;
            }
        }

        public static Size FitInto(Size orig, Size tgt, bool dontEnlarge = false)
        {
            var f = ScaleFactor(orig, tgt);
            if (dontEnlarge)
            {
                f = Math.Min(f, 1.0f);
            }
            return new Size((int)(f * orig.Width), (int)(f * orig.Height));
        }

        public static float ScaleFactor(Size origSize, Size targetSize)
        {
            int sourceWidth = origSize.Width;
            int sourceHeight = origSize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (float)targetSize.Width / (float)sourceWidth;
            nPercentH = (float)targetSize.Height / (float)sourceHeight;

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }
            return nPercent;
        }

        public static Rectangle ScaleRectangle(Rectangle r, double factor)
        {
            return new Rectangle(
                new Point((int)(r.Location.X * factor), (int)(r.Location.Y * factor)), 
                new Size((int)(r.Size.Width * factor), (int)(r.Size.Height * factor)));
        }

        public static Image ResizeImage(Image imgToResize, Size targetSize, float maxScale = 1.0f)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            var factor = ScaleFactor(imgToResize.Size, targetSize);
            factor = Math.Min(factor, maxScale);

            int destWidth = (int)(sourceWidth * factor);
            int destHeight = (int)(sourceHeight * factor);

            Bitmap b = new Bitmap(destWidth, destHeight);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                return (Image)b;
            }
        }

        public void CreateScaledImage(FileInfo srcFile, FileInfo tgtFile, int width, int height, int imageQuality)
        {
            //System.Console.WriteLine("converting " + srcFile.FullName + " to " + tgtFile.FullName);

            using (Bitmap big = new Bitmap(srcFile.FullName))
            {
                Size newSize = new Size(width, height);
                using (var newImage = ResizeImage(big, newSize))
                {
                    big.Dispose();
                    SaveJpeg(tgtFile.FullName, (Bitmap)newImage, imageQuality);
                }
            }
        }

        //public void CreateThumb(FileInfo bigFile, FileInfo thumbFile)
        //{
        //    System.Console.WriteLine("converting " + bigFile.FullName + " to " + thumbFile.FullName);

        //    // scale to thumbnail width
        //    using (Bitmap big = new Bitmap(bigFile.FullName))
        //    {
        //        using (Image thumb1 = ResizeImage(big, thumbWidth))
        //        {
        //            // crop center 
        //            using (var croppedThumb = cropCenter(thumb1, thumbHeight))
        //            {
        //                SaveJpeg(thumbFile.FullName, (Bitmap)croppedThumb, thumbQuality);
        //            }
        //        }
        //    }
        //}

        #endregion

        /*
         * Resize image to given width, keeping aspect ratio
         */

        /**
         * wandelt ein Hochformat-Thumbnail ins Querformat um, indem es auf den mittleren Bereich
         * beschnitten wird. Die Breite muss bereits auf die Zielgröße heruntergerechnet sein.
         */
        #region Methods

        //private static Image cropCenter(Image imgToCrop, int height)
        //{
        //    Rectangle srcRect = new Rectangle(0, (imgToCrop.Height - height) / 2, imgToCrop.Width, height);
        //    Rectangle destRect = new Rectangle(0, 0, imgToCrop.Width, height);

        //    Bitmap b = new Bitmap(imgToCrop.Width, height);
        //    using (Graphics g = Graphics.FromImage((Image)b))
        //    {
        //        g.DrawImage(imgToCrop, destRect, srcRect, GraphicsUnit.Pixel);
        //        return (Image)b;
        //    }
        //}

        //private static Image ResizeImage(Image imgToResize, int width)
        //{
        //    int sourceWidth = imgToResize.Width;
        //    int sourceHeight = imgToResize.Height;

        //    float nPercent = (float)width / (float)sourceWidth;

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);

        //    Bitmap b = new Bitmap(destWidth, destHeight);
        //    using (Graphics g = Graphics.FromImage((Image)b))
        //    {
        //        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        //        return (Image)b;
        //    }
        //}

        private static ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType == mimeType)
                {
                    return codecs[i];
                }
            }
            return null;
        }

        public static void SaveJpeg(string path, Image img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
            {
                return;
            }

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private string webFileName(FileInfo f)
        {
            string n = f.Name;
            n = n.Replace(" ", "_");

            // n = n.Replace("©", "(c)");
            foreach (char c in n.ToCharArray())
            {
                if ((c & (1 << 7)) > 0)
                {
                    n = n.Replace(c, '_');
                }
            }
            return n;
        }

        private string webPath(DirectoryInfo siteRoot, FileInfo file)
        {
            string path = file.FullName;
            path = path.Substring(siteRoot.FullName.Length + 1, path.Length - siteRoot.FullName.Length - 1);
            return path.Replace("\\", "/");
        }

        #endregion
    }
}