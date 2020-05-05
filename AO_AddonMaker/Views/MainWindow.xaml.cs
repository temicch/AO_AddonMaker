using System.Collections.Generic;
using System.Windows;

namespace AO_AddonMaker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindowViewModel();
        }

        private void Click_OpenFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
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
                var q = WidgetManager.paths;
            }
        }
    }
}
