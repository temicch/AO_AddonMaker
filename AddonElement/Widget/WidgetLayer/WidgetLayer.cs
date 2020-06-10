using System.Windows.Media;

namespace AddonElement
{
    abstract public class WidgetLayer : File
    {
        public abstract ImageSource Bitmap { get; }
    }
}