using AO_AddonMaker.Utility;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace AO_AddonMaker
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Project Project { get; set; }

        public ObservableCollection<IUIElement> Widgets { get; set; }

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

        BasicCommand _openFile;
        public ICommand openFile
        {
            get
            {
                if (_openFile == null)
                    _openFile = new BasicCommand(OpenFile);
                return _openFile;
            }
        }

        public MainWindowViewModel()
        {
            AO_AddonMaker.DebugOutput.Init(this);
            WidgetManager.Clear();
            Widgets = new ObservableCollection<IUIElement>();
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
                Widgets.Add(Project.RootWidget);
            }
        }

        public void DebugWrite(string msg)
        {
            DebugOutput = string.Format("{0}\n", msg);
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
