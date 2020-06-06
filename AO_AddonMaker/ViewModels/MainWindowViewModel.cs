using AO_AddonMaker.Utility;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace AO_AddonMaker
{
    public class MainWindowViewModel : BaseViewModel
    {
        public Project Project { get; set; }

        public ObservableCollection<IUIElement> RootFile { get; set; }

        private readonly StringBuilder textDebug = new StringBuilder();

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

        public MainWindowViewModel()
        {
            AO_AddonMaker.DebugOutput.Init(this);
            WidgetManager.Clear();
            RootFile = new ObservableCollection<IUIElement>();
        }

        private void OpenFile(object parameter)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".xdb",
                Filter = "AddonDesc|AddonDesc.(UIAddon).xdb|All files|*.*"
            };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                Project = new Project(dlg.FileName);
                Project.Load();
                RootFile.Clear();
                RootFile.Add(Project.RootWidget);
            }
        }

        public void DebugWrite(string msg)
        {
            DebugOutput = string.Format("{0}\n", msg);
        }
    }
}
