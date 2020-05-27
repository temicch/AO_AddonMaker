using AO_AddonMaker.Utility;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;

namespace AO_AddonMaker
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            //DebugController.Init(window.tboxDebug);
            WidgetManager.Clear();
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
                FileParser fileParser = new FileParser(dlg.FileName);
                fileParser.StartParse();
                WidgetManager.GetRootWidget();
            }
        }
    }
}
