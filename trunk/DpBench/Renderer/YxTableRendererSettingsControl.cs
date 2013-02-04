// -----------------------------------------------------------------------------------------
// DpBench - YxTableRendererSettingsControl.cs
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
    using System.Drawing;
    using System.Windows.Forms;

    using Paguru.DpBench.Controls;
    using Paguru.DpBench.Model;

    /// <summary>
    /// Settings editor for the <see cref="YxImageTableRenderer"/>
    /// </summary>
    public partial class YxTableRendererSettingsControl : UserControl
    {
        #region Constructors and Destructors

        public YxImageTableRenderer Renderer { get; private set; }

        public GroupFilter Root { get; set; }

        public YxTableRendererSettingsControl(YxImageTableRenderer renderer, GroupFilter root)
        {
            InitializeComponent();
            Renderer = renderer;
            Root = root;
            
            SetFontLabel();
            numericUpDownBBHeight.Value = renderer.BoundingBox.Height;
            numericUpDownBBWidth.Value = renderer.BoundingBox.Width;
            numericUpDownPadding.Value = renderer.Padding;

            radioButtonScale.Checked = renderer.ScaleToBoundingBox;
            radioButtonNoScale.Checked = !renderer.ScaleToBoundingBox;
            radioButtonNoScale.CheckedChanged +=
                (s, args) => { radioButtonScale.Checked = !radioButtonNoScale.Checked; };
            radioButtonScale.CheckedChanged +=
                (s, args) => { radioButtonNoScale.Checked = !radioButtonScale.Checked; };
        }

        #endregion

        private void SetFontLabel()
        {
            var font = Renderer.LabelFont;
            var fontname = string.Format("{0} {1}pt {2}", font.Name, font.SizeInPoints, font.Style);
            linkLabelFont.Text = "Font: " + fontname;
        }

        private void linkLabelFont_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var fsd = new FontDialog() { Font = Renderer.LabelFont };
            if (fsd.ShowDialog(this) == DialogResult.OK)
            {
                Renderer.LabelFont = fsd.Font;
                SetFontLabel();
            }
        }

        private void buttonRender_Click(object sender, System.EventArgs e)
        {
            if (!Root.AllValid)
            {
                MessageBox.Show("Layout invalid - please review your group/filter settings");
                return;
            }
            using (Suspender.ShowSandTimerAndSuspend(this))
            {
                Renderer.Padding = (int)numericUpDownPadding.Value;
                Renderer.BoundingBox = new Size((int)numericUpDownBBWidth.Value, (int)numericUpDownBBHeight.Value);
                Renderer.ScaleToBoundingBox = radioButtonScale.Checked;

                var img = Renderer.Render(Root) as Image;
                new ImagePreview(img).Show(this);
            }
        }
    }
}