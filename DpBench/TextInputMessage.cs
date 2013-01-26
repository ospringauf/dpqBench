// -----------------------------------------------------------------------------------------
// DpBench - TextInputMessage.cs
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
    using System.Windows.Forms;

    public partial class TextInputMessage : Form
    {
        #region Constructors and Destructors

        public TextInputMessage()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public string InputText { get; set; }

        #endregion

        #region Methods

        private void textInput_TextChanged(object sender, EventArgs e)
        {
            InputText = textInput.Text;
        }

        #endregion
    }
}