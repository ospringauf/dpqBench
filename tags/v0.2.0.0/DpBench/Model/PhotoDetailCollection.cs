// -----------------------------------------------------------------------------------------
// DpBench - PhotoDetailCollection.cs
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PhotoDetailCollection : List<PhotoDetail>, IEnumerable
    {
        #region Constructors and Destructors

        public PhotoDetailCollection()
        {
        }

        public PhotoDetailCollection(IEnumerable<PhotoDetail> b)
            : base(b)
        {
        }

        #endregion

        #region Public Methods

        public List<string> FindAllValues(string parameterName)
        {
            var l = new List<string>();
            foreach (var pd in this)
            {
                var v = pd.Parameters[parameterName];
                if (!string.IsNullOrEmpty(v) && !l.Contains(v))
                {
                    l.Add(v);
                }
            }
            return l;
        }

        #endregion
    }
}