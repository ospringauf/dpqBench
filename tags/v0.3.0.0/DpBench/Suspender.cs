// -----------------------------------------------------------------------------------------
// DpBench - Suspender.cs
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

    /// <summary>
    /// Use with the using() {} clause to show the sand-timer while the contained code-block is running, 
    /// automatically restoring the cursor to the previous state when the code-block is finished.
    /// </summary>
    /// <remarks>
    /// Just use one of the methods
    /// <see cref="ShowSandTimer"/>, 
    /// <see cref="ShowSandTimerAndSuspend(Control)"/>, or 
    /// <see cref="SuspendControls(Control[])"/>, i.e.:
    /// <code>
    /// using(Suspender.ShowSandTimerAndSuspend(this))
    /// {
    ///     // some code during which a sand timer is to be shown, and the control 'this'
    ///     // is to be suspended....
    /// }
    /// </code>
    /// </remarks>
    public sealed class Suspender : IDisposable
    {
        #region Constants and Fields

        private readonly Control _controlWithCursor;

        private readonly Cursor _originalCursor;

        private Control[] _suspendedControls;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Show a wait cursor if modifyCursor true, and suspend any given controls.
        /// </summary>
        private Suspender(Control controlWithCursor, bool modifyCursor, params Control[] controlsToSuspend)
        {
            // After all dialogs have been added in advance the wait cursor did not disappear
            // after hovering over the alop forms for ear. Might the "messed up" mentioned below.
            if (controlWithCursor != null && controlWithCursor.Visible)
            {
                // by default, always suspend cursor for main window (important not to do for each
                // individual dialog, else gets messed up!)
                _controlWithCursor = controlWithCursor;
                _originalCursor = _controlWithCursor.Cursor;

                if (modifyCursor)
                {
                    _controlWithCursor.Cursor = Cursors.WaitCursor;
                }

                // now suspend ....
                if (controlsToSuspend != null)
                {
                    foreach (Control control in controlsToSuspend)
                    {
                        control.SuspendLayout();
                    }
                }

                _suspendedControls = controlsToSuspend;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the cursor to WaitCursor (a sand timer). The cursor will then be restored
        /// to its previous form when <see cref="Dispose"/> is called (this happens 
        /// automatically when used by a using clause).
        /// </summary>
        /// <remarks>
        /// When the cursor is changed, it is always set for a specific control. It is 
        /// important that the same control is always used (thus it is best to always use
        /// this function rather than manually changing it).
        /// </remarks>
        public static Suspender ShowSandTimer(Control controlWithCursor)
        {
            return new Suspender(controlWithCursor, true, new Control[] { });
        }

        /// <summary>
        /// Combines the functionality of <see cref="ShowSandTimer"/> and 
        /// <see cref="SuspendControls"/>.
        /// </summary>
        public static Suspender ShowSandTimerAndSuspend(Control controlWithCursor)
        {
            return new Suspender(controlWithCursor, true, controlWithCursor);
        }

        /// <summary>
        /// Suspends the layout of all given controls, along with all their children.
        /// Their layout will be resumed when <see cref="Dispose"/> is called (this happens 
        /// automatically when used by a using clause).
        /// </summary>
        public static Suspender SuspendControls(params Control[] controlsToSuspend)
        {
            return new Suspender(controlsToSuspend[0], false, controlsToSuspend);
        }

        /// <summary>
        /// Set Mousepointer back to origin pointer
        /// </summary>
        public void Dispose()
        {
            if (_suspendedControls != null)
            {
                foreach (Control c in _suspendedControls)
                {
                    c.ResumeLayout();
                }

                _suspendedControls = null;
            }

            if (_originalCursor != null)
            {
                _controlWithCursor.Cursor = _originalCursor;
            }
        }

        #endregion
    }
}