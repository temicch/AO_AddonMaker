using System.Windows.Media;

namespace AddonElement
{
    public abstract class WidgetLayer : File
    {
        public abstract ImageSource Bitmap { get; }
    }
}