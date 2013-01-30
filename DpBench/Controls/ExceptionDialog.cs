// -----------------------------------------------------------------------------------------
// DpBench - ExceptionDialog.cs
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
    using System.Diagnostics;
    using System.Windows.Forms;

    /// <summary>
    /// Generic exception dialog
    /// </summary>
    public partial class ExceptionDialog : Form
    {
        private Exception exception;

        #region Constructors and Destructors

        public ExceptionDialog(Exception e)
        {
            exception = e;
            InitializeComponent();

            var m = string.Empty;
            var inner = e;
            while (inner != null)
            {
                m += inner.GetType().Name + ": " + inner.Message + "\r\n";
                inner = inner.InnerException;
            }
            labelErrorMessage.Text = m;
        }

        #endregion

        #region Methods


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://sourceforge.net/projects/dpbench/support");
        }

        #endregion

        private void linkLabelCopyClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string fullException = string.Empty;

            var ex = exception;
            while (ex != null)
            {
                fullException += GetExceptionMessage(ex) + Environment.NewLine + Environment.NewLine;
                ex = ex.InnerException;
            }

            Clipboard.SetDataObject(fullException, true);
            MessageBox.Show(
                "Error information has been copied into your clipboard.\r\nPlease paste this information into an email to get help.");
        }

        private string GetExceptionMessage(Exception exception)
        {
            string msg = exception.GetType().Name + ": ";

            msg += exception.Message +
                   Environment.NewLine + Environment.NewLine +
                   exception.StackTrace;
            return msg;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}