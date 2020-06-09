using System.Windows.Media;

namespace AddonElement
{
    abstract public class WidgetLayer : AddonFile
    {
        public abstract ImageSource Bitmap { get; }
    }
}