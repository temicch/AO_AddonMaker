using AO_AddonMaker.Views;

namespace AO_AddonMaker
{
    static class DebugOutput
    {
        private static MainWindowViewModel _viewModel;

        public static void Init(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public static void Write(string msg)
        {
            _viewModel?.DebugWrite(msg);
        }
    }
}
