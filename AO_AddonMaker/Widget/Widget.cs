using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    public class BindSection
    {
        public string bindSection;
        public List<string> bindedReactions;
    }

    [Serializable]
    abstract public class Widget : IUIElement
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public int Priority { get; set; }
        public WidgetPlacementXY Placement { get; set; }

        [XmlArrayItem("Item")]
        public List<href> Children { get; set; }

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

        [XmlIgnore]
        public string Path { get; set; }

        public Widget()
        {
            WidgetManager.SetRootWidget(this);
            Visible = true;
            Enabled = true;
            Priority = 0;
            //Placement = new WidgetPlacementXY();

            clipContent = false;
            fade = 1.0f;
            PickChildrenOnly = false;
            forceWheel = false;
            IgnoreDblClick = false;
            TransparentInput = false;

            isProtected = false;
            TabOrder = 0;
        }

        public IEnumerable<IUIElement> GetChildren() => Children.Select(x => x.UIElement);
        public string GetName() => Name;

        public void Dispose()
        {
            foreach (var widget in Children)
                widget.UIElement.Dispose();
        }
    }
}
