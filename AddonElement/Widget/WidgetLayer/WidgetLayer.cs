using System.Windows.Media;

namespace AddonElement.Widgets
{
    public abstract class WidgetLayer : File.File
    {
        public abstract ImageSource Bitmap { get; }
    }
}