// -----------------------------------------------------------------------------------------
// DpBench - Program.cs
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
    using System.Threading;
    using System.Windows.Forms;

    using Paguru.DpBench.Controls;

    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args != null & args.Length > 0)
            {
                MainWindow.Instance.StartupProjectFile = args[0];
            }

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += HandleUIThreadException;

            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

            Application.Run(MainWindow.Instance);
        }

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            new ExceptionDialog(exception).ShowDialog();
        }

        private static void HandleUIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception exception = e.Exception;
            new ExceptionDialog(exception).ShowDialog();
        }

        #endregion
    }
}