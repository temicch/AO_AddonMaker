using Application.BL;
using Application.BL.Services.SamplesProvider.Models;
using Application.BL.Services.SamplesProvider.Services;
using Application.BL.Widgets;
using Application.PL.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace Application.PL.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private const string addonDescName = "AddonDesc.(UIAddon).xdb";
        private readonly object debugObject = new object();
        
        private readonly StringBuilder textDebug = new StringBuilder();

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger, 
            Project project,
            SamplesProviderService samplesProviderService)
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            ClearDebugCommand = new RelayCommand(ClearDebug);
            SampleSelectCommand = new RelayCommand(SampleSelect);

            Logger = logger;
            Project = project;
            SampleProviderService = samplesProviderService;
            RootFile = new ObservableCollection<IUIElement>();
            
            App.OnLogHandler += (logEvent, objects) => DebugWrite(logEvent.FormattedMessage);
        }

        public ILogger<MainWindowViewModel> Logger { get; }
        public Project Project { get; set; }
        public SamplesProviderService SampleProviderService { get; }

        public ICollection<IUIElement> RootFile { get; set; }

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


        private void OpenFile(object parameter)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".xdb",
                Filter = $"AddonDesc|{addonDescName}|All files|*.*"
            };
            var result = dlg.ShowDialog();
            if (result == true)
                LoadProject(dlg.FileName);
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
            var sampleModel = (SampleModel)parameter;
            LoadProject(sampleModel.FullPath);
        }
    }
}