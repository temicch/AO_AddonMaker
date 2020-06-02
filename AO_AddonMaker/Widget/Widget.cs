using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    [Serializable]
    abstract public class Widget : AddonFile, IUIElement
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public int Priority { get; set; }

        public WidgetPlacementXY Placement { get; set; }

        [XmlArrayItem("Item")]
        public List<href> Children { get; set; }

        [XmlIgnore]
        public List<AddonFile> Widgets
        {
            get => Children.Select(x => x.File).ToList();
        }

        public bool clipContent { get; set; }
        public href BackLayer { get; set; }
        public href FrontLayer { get; set; }
        public href textureMask { get; set; }
        public float fade { get; set; }

        public href pickMask { get; set; }
        public bool PickChildrenOnly { get; set; }
        public bool forceWheel { get; set; }
        public bool IgnoreDblClick { get; set; }
        public bool TransparentInput { get; set; }

        public bool isProtected { get; set; }
        public int TabOrder { get; set; }
        public href soundShow { get; set; }
        public href soundHide { get; set; }

        public List<BindSection> bindSections { get; set; }

        public string reactionOnPointing { get; set; }
        public string forceReactionOnPointing { get; set; }
        public string reactionWheelUp { get; set; }
        public string reactionWheelDown { get; set; }

        public Widget()
        {
            Visible = true;
            Enabled = true;
            Priority = 0;
            Placement = new WidgetPlacementXY();

            clipContent = false;
            fade = 1.0f;
            PickChildrenOnly = false;
            forceWheel = false;
            IgnoreDblClick = false;
            TransparentInput = false;

            isProtected = false;
            TabOrder = 0;
        }
        public IEnumerable<AddonFile> GetChildren() => Children.Select(x => x.File);
    }
}
