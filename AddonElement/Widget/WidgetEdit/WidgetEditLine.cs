using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetEditLine : WidgetEditBase
    {
        [XmlIgnore]
        public bool isPassword { get; set; }
        [XmlElement("isPassword")]
        public string _isPassword
        {
            get => isPassword.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                    isPassword = result;
            }
        }

        public string ReactionEnter { get; set; }
        public string reactionUp { get; set; }
        public string reactionDown { get; set; }

        public WidgetEditLine()
        {
            isPassword = false;
        }
    }
}
