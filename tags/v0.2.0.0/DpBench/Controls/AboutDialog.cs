// -----------------------------------------------------------------------------------------
// DpBench - AboutDialog.cs
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
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    public partial class AboutDialog : Form
    {
        #region Constructors and Destructors

        public AboutDialog()
        {
            InitializeComponent();
            labelVersion.Text = Version;
        }

        #endregion

        #region Public Properties

        public static string Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi.ProductVersion;
            }
        }

        #endregion

        #region Methods

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://sourceforge.net/projects/dpbench");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://github.com/dockpanelsuite");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://objectlistview.sourceforge.net/cs/index.html");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(
                "http://www.codeproject.com/Articles/27242/ExifTagCollection-An-EXIF-metadata-extraction-libr");
        }

        private void linkLabelLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.apache.org/licenses/LICENSE-2.0.html");
        }

        #endregion
    }
}