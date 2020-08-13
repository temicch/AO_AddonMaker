using System.Xml.Serialization;

namespace Application.BL.Widgets
{
    public class WidgetEditLine : WidgetEditBase
    {
        public WidgetEditLine()
        {
            isPassword = false;
        }

        [XmlIgnore]
        public bool isPassword { get; set; }

        [XmlElement("isPassword")]
        public string _isPassword
        {
            get => isPassword.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    isPassword = result;
            }
        }

        public string ReactionEnter { get; set; }
        public string reactionUp { get; set; }
        public string reactionDown { get; set; }
    }
}