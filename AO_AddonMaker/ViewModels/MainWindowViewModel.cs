using Addon.Widgets;
using Application.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace Application.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private const string addonDescName = "AddonDesc.(UIAddon).xdb";

        private readonly object debugObject = new object();

        private readonly string samplesPath =
            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Samples";

        private readonly StringBuilder textDebug = new StringBuilder();

        public MainWindowViewModel(Project project)
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            ClearDebugCommand = new RelayCommand(ClearDebug);
            SampleSelectCommand = new RelayCommand(SampleSelect);

            Project = project;
            RootFile = new ObservableCollection<IUIElement>();

            InitSampleProjects();

            App.OnLogHandler += (logEvent, objects) => DebugWrite(logEvent.FormattedMessage);
        }

        public Project Project { get; set; }

        public ICollection<IUIElement> RootFile { get; set; }
        public ICollection<string> Samples { get; private set; }

        public string DebugOutput
        {
            get
            {
                lock (debugObject)
                {
                    return textDebug.ToString();
                }
            }
            set
            {
                lock (debugObject)
                {
                    textDebug.Append(value);
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand ClearDebugCommand { get; set; }
        public RelayCommand SampleSelectCommand { get; set; }

        private void InitSampleProjects()
        {
            Samples = new ObservableCollection<string>();
            var previousDirectory = Directory.GetCurrentDirectory();
            try
            {
                foreach (var path in Directory.GetDirectories(samplesPath))
                {
                    var addonDesc = $"{path}{Path.DirectorySeparatorChar}{addonDescName}";
                    if (!File.Exists(addonDesc))
                        continue;
                    var q = Directory.GetParent(addonDesc).Name;
                    Samples.Add(q);
                }
            }
            catch (Exception)
            {
            }

            Directory.SetCurrentDirectory(previousDirectory);
        }

        private void OpenFile(object parameter)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".xdb",
                Filter = $"AddonDesc|{addonDescName}|All files|*.*"
            };
            var result = dlg.ShowDialog();
            if (result == true) LoadProject(dlg.FileName);
        }

        public void LoadProject(string fileName)
        {
            if (!File.Exists(fileName))
            {
                MessageBox.Show("The specified file does not exist");
                return;
            }

            ClearDebug(null);
            ProgressDialog.ShowModal(() => Project.Load(fileName));
            RootFile.Clear();
            RootFile.Add(Project.RootWidget);
        }

        public void DebugWrite(string msg)
        {
            DebugOutput = $"{msg}\n";
        }

        private void ClearDebug(object parameter)
        {
            lock (debugObject)
            {
                textDebug.Clear();
            }

            OnPropertyChanged(nameof(DebugOutput));
        }

        private void SampleSelect(object parameter)
        {
            LoadProject(
                $"{samplesPath}{Path.DirectorySeparatorChar}{parameter}{Path.DirectorySeparatorChar}{addonDescName}");
        }
    }
}