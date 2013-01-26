// -----------------------------------------------------------------------------------------
// DpBench - PhotoSelectedEvent.cs
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

    using Paguru.DpBench.Model;

    /// <summary>
    /// Event triggered by the project window when a photo is selected.
    /// <see cref="PhotoPropertyWindow"/> and <see cref="DetailEditor"/> are listening and updating themselves.
    /// </summary>
    public class PhotoSelectedEvent : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoSelectedEvent"/> class.
        /// </summary>
        /// <param name="p">The p.</param>
        public PhotoSelectedEvent(Photo p)
        {
            Photo = p;
        }

        #region Public Properties

        /// <summary>
        /// Gets the currently selected photo.
        /// </summary>
        public Photo Photo { get; private set; }

        #endregion
    }
}