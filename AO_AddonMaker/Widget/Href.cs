using System.Xml.Serialization;

namespace AO_AddonMaker
{
    public class href
    {
        [XmlAttribute("href")]
        public string Path
        {
            get => path;
            set
            {
                path = value;
                if (path != string.Empty)
                {
                    path = System.IO.Path.GetFullPath(path);
                    UIElement = WidgetManager.GetUIElement(path);
                }
            }
        }
        [XmlIgnore]
        public IUIElement UIElement { get; set; }
        [XmlIgnore]
        string path;
    }
}
