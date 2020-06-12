using AO_AddonMaker.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace AO_AddonMaker.Utility
{
    class ProgressDialog
    {
        public static void ShowModal(Action work)
        {
            WorkInProgress splash = new WorkInProgress();

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
