// -----------------------------------------------------------------------------------------
// DpBench - Form1.cs
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
    using System.Collections.Generic;
    using System.Windows.Forms;

    using Paguru.DpBench.Model;

    public partial class Form1 : Form
    {
        #region Constructors and Destructors

        public Form1()
        {
            InitializeComponent();

            var v = new List<SelectableValue> {
                    new SelectableValue("a"), 
                    new SelectableValue("b", false), 
                    new SelectableValue("c"), 
                    new SelectableValue("d", false), 
                    new SelectableValue("e"), 
                };
            listOrderControl1.Values = v;
        }

        #endregion
    }
}