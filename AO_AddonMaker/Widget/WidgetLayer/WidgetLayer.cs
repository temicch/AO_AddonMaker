using System.Collections.Generic;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    abstract public class WidgetLayer : IUIElement
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IUIElement> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        [XmlIgnore]
        public string Path { get; set; }

        public string GetName()
        {
            throw new System.NotImplementedException();
        }
    }
}