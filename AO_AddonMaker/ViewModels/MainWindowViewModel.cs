using AddonElement;
using AO_AddonMaker.Utility;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
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

        private object debugObject = new object();

        public ObservableCollection<IUIElement> RootFile { get; set; }
        public ObservableCollection<string> Samples { get; private set; }

        private const string samplesPath = "../../../Samples";
        private const string addonDescName = "AddonDesc.(UIAddon).xdb";

        private StringBuilder textDebug = new StringBuilder();

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

        RelayCommand _sampleSelect;
        public ICommand sampleSelect
        {
            get
            {
                if (_sampleSelect == null)
                    _sampleSelect = new RelayCommand(SampleSelect);
                return _sampleSelect;
            }
        }

        public MainWindowViewModel()
        {
            AO_AddonMaker.DebugOutput.Init(this);
            FileManager.OnDebug += DebugWrite;
            FileManager.Clear();
            RootFile = new ObservableCollection<IUIElement>();

            InitSampleProjects();
        }

        private void InitSampleProjects()
        {
            Samples = new ObservableCollection<string>();
            var previousDirectory = Directory.GetCurrentDirectory();
            try
            {
                foreach(var path in Directory.GetDirectories(samplesPath))
                {
                    var addonDesc = $"{path}\\{addonDescName}";
                    if (!System.IO.File.Exists(addonDesc))
                        continue;
                    var q = Directory.GetParent(addonDesc).Name;
                    Samples.Add(q);
                }
            }
            catch (Exception) { }
            Directory.SetCurrentDirectory(previousDirectory);
        }

        private void OpenFile(object parameter)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".xdb",
                Filter = $"AddonDesc|{addonDescName}|All files|*.*"
            };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                AsyncLoadProject(dlg.FileName);
            }
        }

        private async void AsyncLoadProject(string filePath)
        {
            var workInProgress = new WorkInProgress()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.Windows.OfType<MetroWindow>().SingleOrDefault(x => x.IsActive)
            };
            workInProgress.Show();
            RootFile.Clear();
            await Task.Run(() => LoadProject(filePath));
            RootFile.Add(Project.RootWidget);
            workInProgress.Close();
        }

        private void LoadProject(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                MessageBox.Show("The specified file does not exist");
                return;
            }
            GC.Collect();
            GC.WaitForFullGCComplete();
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

        private void SampleSelect(object parameter)
        {
            AsyncLoadProject($"{samplesPath}\\{parameter}\\{addonDescName}");
        }
    }
}
