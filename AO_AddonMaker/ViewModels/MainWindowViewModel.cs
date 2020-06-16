﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using AddonElement;
using AO_AddonMaker.Utility;
using Microsoft.Win32;
using File = System.IO.File;

namespace AO_AddonMaker.Views
{
    public class MainWindowViewModel : BaseViewModel
    {
        private const string samplesPath = "../../../Samples";
        private const string addonDescName = "AddonDesc.(UIAddon).xdb";

        private readonly object debugObject = new object();

        private readonly StringBuilder textDebug = new StringBuilder();

        public MainWindowViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            ClearDebugCommand = new RelayCommand(ClearDebug);
            SampleSelectCommand = new RelayCommand(SampleSelect);

            AO_AddonMaker.DebugOutput.Init(this);
            FileManager.OnDebug += DebugWrite;
            FileManager.Clear();
            RootFile = new ObservableCollection<IUIElement>();

            InitSampleProjects();
        }

        public Project Project { get; set; }

        public ObservableCollection<IUIElement> RootFile { get; set; }
        public ObservableCollection<string> Samples { get; private set; }

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
                    var addonDesc = $"{path}\\{addonDescName}";
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

            GC.Collect();
            GC.WaitForFullGCComplete();
            ClearDebug(null);
            Project = new Project(fileName);
            ProgressDialog.ShowModal(() => Project.Load());
            RootFile.Clear();
            RootFile.Add(Project.RootWidget);
        }

        public void DebugWrite(string msg)
        {
            DebugOutput = $"{msg}\n";
        }

        private void ClearDebug(object parameter)
        {
            textDebug.Clear();
            OnPropertyChanged(nameof(DebugOutput));
        }

        private void SampleSelect(object parameter)
        {
            LoadProject($"{samplesPath}\\{parameter}\\{addonDescName}");
        }
    }
}