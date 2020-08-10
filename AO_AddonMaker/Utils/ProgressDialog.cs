using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Application.Views;
using MahApps.Metro.Controls;

namespace Application.Utils
{
    internal class ProgressDialog
    {
        public static void ShowModal(Action work)
        {
            var splash = new WorkInProgress
            {
                Owner = System.Windows.Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive),
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