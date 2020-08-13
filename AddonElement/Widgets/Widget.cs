using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Files;
using Application.BL.Files.Provider;
using Application.BL.Utils;
using Application.BL.Widgets.Placement;

namespace Application.BL.Widgets
{
    [Serializable]
    public abstract class Widget : File, IUIElement
    {
        public Widget()
        {
            Visible = true;
            Enabled = true;
            Priority = 0;
            Placement = new WidgetPlacementXY();

            СlipContent = false;
            Fade = 1.0f;
            PickChildrenOnly = false;
            ForceWheel = false;
            IgnoreDblClick = false;
            TransparentInput = false;

            IsProtected = false;
            TabOrder = 0;
        }

        [XmlElement("Visible")]
        public string _Visible
        {
            get => Visible.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    Visible = result;
            }
        }

        [XmlElement("Enabled")]
        public string _Enabled
        {
            get => Enabled.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    Enabled = result;
            }
        }

        /// <summary>
        ///     Display priority
        /// </summary>
        [Category("Base properties")]
        [ResourceDescription("Desc_Priority")]
        public int Priority { get; set; }

        /// <summary>
        ///     Child widgets
        /// </summary>
        [Category("Children widgets")]
        [ResourceDescription("Desc_Widgets")]
        [XmlArray("Children")]
        [XmlArrayItem("Item")]
        public List<Reference<XmlFileProvider>> Widgets { get; set; }

        /// <summary>
        ///     Clip content or not
        /// </summary>
        [Category("Children widgets")]
        [ResourceDescription("Desc_СlipContent")]
        [XmlIgnore]
        public bool СlipContent { get; set; }

        [XmlElement("clipContent")]
        public string _ClipContent
        {
            get => СlipContent.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    СlipContent = result;
            }
        }

        /// <summary>
        ///     Layer to display the bottom of the texture
        /// </summary>
        [Category("Display")]
        [ResourceDescription("Desc_BackLayer")]
        public Reference<XmlFileProvider> BackLayer { get; set; }

        /// <summary>
        ///     Layer to display the top of the texture
        /// </summary>
        [Category("Display")]
        [ResourceDescription("Desc_FrontLayer")]
        public Reference<XmlFileProvider> FrontLayer { get; set; }

        /// <summary>
        ///     Alpha texture. It is used to set the mask by which the main texture of this control and all its children will be
        ///     cut
        /// </summary>
        [Category("Display")]
        [ResourceDescription("Desc_TextureMask")]
        [XmlElement("textureMask")]
        public Reference<BlankFileProvider> TextureMask { get; set; }

        /// <summary>
        ///     Visual transparency of the widget. Default 1.0f - opaque
        /// </summary>
        [Category("Display")]
        [ResourceDescription("Desc_Fade")]
        [XmlElement("fade")]
        public float Fade { get; set; }

        /// <summary>
        ///     Black and white texture (in powers of 2) to define the active (white pixels) area for mouse clicks. You need to
        ///     manually set mipSW = 0 when exporting
        /// </summary>
        [Category("Track and limit reactions")]
        [ResourceDescription("Desc_PickMask")]
        [XmlElement("pickMask")]
        public Reference<XmlFileProvider> PickMask { get; set; }

        /// <summary>
        ///     Handle mouse reactions only for the children of this widget, ignoring the widget itself
        /// </summary>
        [Category("Track and limit reactions")]
        [ResourceDescription("Desc_PickChildrenOnly")]
        [XmlIgnore]
        public bool PickChildrenOnly { get; set; }

        [XmlElement("PickChildrenOnly")]
        public string _PickChildrenOnly
        {
            get => PickChildrenOnly.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    PickChildrenOnly = result;
            }
        }

        /// <summary>
        ///     Ignore PickChildrenOnly on mouse wheel scrolling and hover. Always handle scroll response with the mouse wheel
        /// </summary>
        [Category("Track and limit reactions")]
        [ResourceDescription("Desc_ForceWheel")]
        [XmlIgnore]
        [XmlElement("forceWheel")]
        public bool ForceWheel { get; set; }

        [XmlElement("forceWheel")]
        public string _ForceWheel
        {
            get => ForceWheel.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    ForceWheel = result;
            }
        }

        /// <summary>
        ///     Ignore double click for the widget and for its children
        /// </summary>
        [Category("Track and limit reactions")]
        [ResourceDescription("Desc_IgnoreDblClick")]
        [XmlIgnore]
        public bool IgnoreDblClick { get; set; }

        [XmlElement("IgnoreDblClick")]
        public string _IgnoreDblClick
        {
            get => IgnoreDblClick.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    IgnoreDblClick = result;
            }
        }

        /// <summary>
        ///     Is the widget transparent for input
        /// </summary>
        [Category("Track and limit reactions")]
        [ResourceDescription("Desc_TransparentInput")]
        [XmlIgnore]
        public bool TransparentInput { get; set; }

        [XmlElement("TransparentInput")]
        public string _TransparentInput
        {
            get => TransparentInput.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    TransparentInput = result;
            }
        }

        /// <summary>
        ///     Whether to prevent custom addons from operations with the widget
        /// </summary>
        [Category("Special")]
        [ResourceDescription("Desc_IsProtected")]
        [XmlIgnore]
        public bool IsProtected { get; set; }

        [XmlElement("isProtected")]
        public string _IsProtected
        {
            get => IsProtected.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    IsProtected = result;
            }
        }

        /// <summary>
        ///     The order of bypassing controls by the Tab key
        /// </summary>
        [Category("Special")]
        [ResourceDescription("Desc_TabOrder")]
        public int TabOrder { get; set; }

        /// <summary>
        ///     Sound on the started display of the widget
        /// </summary>
        [Category("Special")]
        [ResourceDescription("Desc_SoundShow")]
        [XmlElement("soundShow")]
        public Reference<BlankFileProvider> SoundShow { get; set; }

        /// <summary>
        ///     Sound to hide the widget
        /// </summary>
        [Category("Special")]
        [ResourceDescription("Desc_SoundHide")]
        [XmlElement("soundHide")]
        public Reference<BlankFileProvider> SoundHide { get; set; }

        /// <summary>
        ///     List of reactions to keystrokes
        /// </summary>
        [Category("Keyboard reactions")]
        [ResourceDescription("Desc_BindSections")]
        [XmlArray("bindSections")]
        public List<BindSection> BindSections { get; set; }

        /// <summary>
        ///     Widget hover notification
        /// </summary>
        [Category("Mouse reactions")]
        [ResourceDescription("Desc_ReactionOnPointing")]
        [XmlElement("reactionOnPointing")]
        public string ReactionOnPointing { get; set; }

        /// <summary>
        ///     Hover notification on a widget, regardless of whether it is clickable
        /// </summary>
        [Category("Mouse reactions")]
        [ResourceDescription("Desc_ForceReactionOnPointing")]
        [XmlElement("forceReactionOnPointing")]
        public string ForceReactionOnPointing { get; set; }

        /// <summary>
        ///     Mouse wheel scroll up notification
        /// </summary>
        [Category("Mouse reactions")]
        [ResourceDescription("Desc_ReactionWheelUp")]
        [XmlElement("reactionWheelUp")]
        public string ReactionWheelUp { get; set; }

        /// <summary>
        ///     Mouse wheel scroll down notification
        /// </summary>
        [Category("Mouse reactions")]
        [ResourceDescription("Desc_ReactionWheelDown")]
        [XmlElement("reactionWheelDown")]
        public string ReactionWheelDown { get; set; }

        public ImageSource Bitmap => GetBitmap();

        [Category("Base properties")]
        [ResourceDescription("Desc_Name")]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Category("Base properties")]
        [ResourceDescription("Desc_Visible")]
        [XmlIgnore]
        public bool Visible { get; set; }

        [Category("Base properties")]
        [ResourceDescription("Desc_Enabled")]
        [XmlIgnore]
        public bool Enabled { get; set; }

        [Category("Placement")]
        [ResourceDescription("Desc_Placement")]
        public WidgetPlacementXY Placement { get; set; }

        [XmlIgnore]
        public IEnumerable<IUIElement> Children => Widgets?.Select(x => x.File as IUIElement);

        [XmlIgnore]
        public int ChildrenCount => Children.Count() + Children.Sum(child => child.ChildrenCount);

        /// <summary>
        ///     Get a bitmap for the widget. Each widget type can override this behavior
        /// </summary>
        /// <returns>
        ///     <seealso cref="ImageSource" />
        /// </returns>
        protected virtual ImageSource GetBitmap()
        {
            return (BackLayer?.File as WidgetLayer)?.Bitmap;
        }
    }
}