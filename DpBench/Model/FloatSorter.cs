// -----------------------------------------------------------------------------------------
// DpBench - FloatSorter.cs
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

namespace Paguru.DpBench.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Comparer for floating point numbers written as strings ("f/1.7")
    /// </summary>
    public class FloatSorter : IComparer<string>, IComparer
    {
        #region Constants and Fields

        private char decimalPoint = Util.DecimalPoint();
        private const string allowed = "+-0123456789.,";

        #endregion

        #region Public Methods

        public int Compare(string x, string y)
        {
            var f1 = ToDouble(Util.FilterString(x, allowed));
            var f2 = ToDouble(Util.FilterString(y, allowed));
            return f1 < f2 ? -1 : f1 > f2 ? 1 : 0;
        }

        public int Compare(object x, object y)
        {
            return Compare(x as string, y as string);
        }

        #endregion

        #region Methods

        private double ToDouble(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return 0;
            }
            x = x.Replace(',', decimalPoint);
            x = x.Replace('.', decimalPoint);
            return Convert.ToDouble(x);
        }

        #endregion
    }
}