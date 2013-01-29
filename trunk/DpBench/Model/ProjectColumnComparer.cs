﻿// -----------------------------------------------------------------------------------------
// DpBench - ProjectColumnComparer.cs
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using BrightIdeasSoftware;

    /// <summary>
    /// back-map the column title to the Photo property, get the custom property sorter, sort
    /// </summary>
    public class ProjectColumnComparer : IComparer, IComparer<OLVListItem>
    {
        #region Constants and Fields

        private readonly OLVColumn column;

        private readonly SortOrder order;

        private readonly IComparer sorter;

        #endregion

        #region Constructors and Destructors

        public ProjectColumnComparer(OLVColumn col, SortOrder ord)
        {
            column = col;
            order = ord;
            sorter = Photo.GetComparerForOlvColumn(column.Name);
        }

        #endregion

        #region Public Methods

        public int Compare(object x, object y)
        {
            return this.Compare((OLVListItem)x, (OLVListItem)y);
        }

        public int Compare(OLVListItem x, OLVListItem y)
        {
            object x1 = this.column.GetValue(x.RowObject);
            object y1 = this.column.GetValue(y.RowObject);
            var s = (sorter != null) ? sorter.Compare(x1, y1) : string.Compare(x1 as string, y1 as string);
            return (order == SortOrder.Ascending) ? s : 0 - s;
        }

        #endregion
    }
}