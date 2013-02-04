// -----------------------------------------------------------------------------------------
// DpBench - ImagePreview.cs
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
    using System.Drawing;
    using System.Windows.Forms;

    using ImageConverter = Paguru.DpBench.ImageConverter;

    /// <summary>
    /// Image preview window with save to file option (context menu)
    /// </summary>
    public partial class ImagePreview : Form
    {
        private Image image;

        #region Constructors and Destructors
        
        public ImagePreview(Image img)
        {
            image = img;
            InitializeComponent();
            pictureBox1.Image = img;
            pictureBox1.Size = img.Size;

            FormClosing += (s, e) => img.Dispose();
        }

        #endregion

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var fd = new SaveFileDialog() { DefaultExt = ".jpg", Filter = "JPEG images|*.jpg" };
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                ImageConverter.SaveJpeg(fd.FileName, image, 85);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetImage(image);
        }
    }
}