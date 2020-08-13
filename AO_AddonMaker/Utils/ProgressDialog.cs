using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Application.PL.Views;
using MahApps.Metro.Controls;

namespace Application.PL.Utils
{
    internal class ProgressDialog
    {
        /// <summary>
        ///     Show a modal window with a progress bar that simulates doing some work.
        /// </summary>
        /// <param name="work">The delegate to run as a background operation</param>
        public static void ShowModal(Action work)
        {
            var splash = new WorkInProgress
            {
                Owner = System.Windows.Application.Current.Windows.OfType<MetroWindow>()
                    .SingleOrDefault(x => x.IsActive),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            splash.Loaded += (_, args) =>
            {
                var worker = new BackgroundWorker();

                worker.DoWork += (s, workerArgs) => work();

                worker.RunWorkerCompleted +=
                    (s, workerArgs) => splash.Close();

                worker.RunWorkerAsync();
            };

            splash.ShowDialog();
        }
    }
}