using AO_AddonMaker.Views;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace AO_AddonMaker.Utility
{
    class ProgressDialog
    {
        public static void ShowModal(Action work)
        {
            WorkInProgress splash = new WorkInProgress()
            {
                Owner = Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            splash.Loaded += (_, args) =>
            {
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (s, workerArgs) => work();

                worker.RunWorkerCompleted +=
                    (s, workerArgs) => splash.Close();

                worker.RunWorkerAsync();
            };

            splash.ShowDialog();
        }
    }
}
