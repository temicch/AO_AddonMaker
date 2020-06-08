﻿using AddonElement;
using AO_AddonMaker.Utility;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AO_AddonMaker.Views
{
    public class MainWindowViewModel : BaseViewModel
    {
        public Project Project { get; set; }

        public ObservableCollection<IUIElement> RootFile { get; set; }

        private StringBuilder textDebug = new StringBuilder();

        public string DebugOutput 
        { 
            get 
            { 
                return textDebug.ToString(); 
            } 
            set 
            {
                textDebug.Append(value);
                OnPropertyChanged();
            }
        }

        RelayCommand _openFile;
        public ICommand openFile
        {
            get
            {
                if (_openFile == null)
                    _openFile = new RelayCommand(OpenFile);
                return _openFile;
            }
        }

        RelayCommand _clearDebug;
        public ICommand clearDebug
        {
            get
            {
                if (_clearDebug == null)
                    _clearDebug = new RelayCommand(ClearDebug);
                return _clearDebug;
            }
        }

        public MainWindowViewModel()
        {
            AO_AddonMaker.DebugOutput.Init(this);
            WidgetManager.OnDebug += DebugWrite;
            WidgetManager.Clear();
            RootFile = new ObservableCollection<IUIElement>();
        }

        private async void OpenFile(object parameter)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".xdb",
                Filter = "AddonDesc|AddonDesc.(UIAddon).xdb|All files|*.*"
            };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                var workInProgress = new WorkInProgress()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive)
                };
                workInProgress.Show();
                await Task.Run(() => LoadProject(dlg.FileName));
                RootFile.Clear();
                RootFile.Add(Project.RootWidget);
                workInProgress.Close();
            }
        }

        private void LoadProject(string fileName)
        {
            GC.Collect();
            Project = new Project(fileName);
            Project.Load();
        }

        public void DebugWrite(string msg)
        {
            DebugOutput = string.Format("{0}\n", msg);
        }

        private void ClearDebug(object parameter)
        {
            textDebug.Clear();
            OnPropertyChanged(nameof(DebugOutput));
        }
    }
}
