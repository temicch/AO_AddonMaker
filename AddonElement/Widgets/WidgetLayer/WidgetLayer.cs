using System.Windows.Media;
using Addon.Files;

namespace Addon.Widgets
{
    public abstract class WidgetLayer : File
    {
        public abstract ImageSource Bitmap { get; }
    }
}