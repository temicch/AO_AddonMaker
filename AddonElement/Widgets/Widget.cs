using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Files;
using Application.BL.Files.Provider;
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
        [Description(
            "Приоритет отображения (также влияет на обработку мышиных событий) виджета в списке виджетов своего родителя. То есть с помощью этого поля можно сформировать иерархию отображения виджетов всего аддона")]
        public int Priority { get; set; }

        /// <summary>
        ///     Child widgets
        /// </summary>
        [Category("Children widgets")]
        [Description(
            "Дочерние виджеты. Почти каждый виджет может содержать дочерние виджеты, за исключением особых случаев типа слайдера и т.п. Дочерние виджеты отображаются поверх родителя и перехватывают реакции (если они объявлены и на них подписаны обработчики) раньше родительского виджета за исключением особых случаев")]
        [XmlArray("Children")]
        [XmlArrayItem("Item")]
        public List<Reference<XmlFileProvider>> Widgets { get; set; }

        /// <summary>
        ///     Clip content or not
        /// </summary>
        [Category("Children widgets")]
        [Description("Нужно ли обрезать содержимое, включая дочерние виджеты, по границам данного. По умолчанию false")]
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
        [Description("Слой для отображения нижней части текстуры")]
        public Reference<XmlFileProvider> BackLayer { get; set; }

        /// <summary>
        ///     Layer to display the top of the texture
        /// </summary>
        [Category("Display")]
        [Description("Слой для отображения верхней части текстуры")]
        public Reference<XmlFileProvider> FrontLayer { get; set; }

        /// <summary>
        ///     Alpha texture. It is used to set the mask by which the main texture of this control and all its children will be
        ///     cut
        /// </summary>
        [Category("Display")]
        [Description(
            "Текстура с альфой. Используется для задания маски, по которой будет обрезана основная текстура данного контрола и всех его детей")]
        [XmlElement("textureMask")]
        public Reference<BlankFileProvider> TextureMask { get; set; }

        /// <summary>
        ///     Visual transparency of the widget. Default 1.0f - opaque
        /// </summary>
        [Category("Display")]
        [Description("Визуальная прозрачность виджета. По умолчанию 1.0f - непрозрачен")]
        [XmlElement("fade")]
        public float Fade { get; set; }

        /// <summary>
        ///     Black and white texture (in powers of 2) to define the active (white pixels) area for mouse clicks. You need to
        ///     manually set mipSW = 0 when exporting
        /// </summary>
        [Category("Track and limit reactions")]
        [Description(
            "Черно-белая текстура (по степеням 2) для задания активной (белые пиксели) области для кликов мышью. Нужно вручную выставлять mipSW = 0 при экспорте")]
        [XmlElement("pickMask")]
        public Reference<XmlFileProvider> PickMask { get; set; }

        /// <summary>
        ///     Handle mouse reactions only for the children of this widget, ignoring the widget itself
        /// </summary>
        [Category("Track and limit reactions")]
        [Description("Обрабатывать мышиные реакции только для детей этого виджета, игнорируя сам виджет")]
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
        [Description(
            "Игнорировать PickChildrenOnly при скролировании колесом мыши и наведении. Всегда обрабатывать реакцию скролла колесом мыши")]
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
        [Description("Игнорировать двойной клик мышью для виджета и для его детей")]
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
        [Description("Является ли виджет прозрачным для ввода. По умолчанию false")]
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
        [Description("Запрещать ли пользовательским аддонам операции с виджетом. По умолчанию false")]
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
        [Description(
            "Задаёт порядок обхода контролов по клавише Tab. По умолчанию 0(не учавствует в обходе). Для участия в обходе значение должно быть больше 0")]
        public int TabOrder { get; set; }

        /// <summary>
        ///     Sound on the started display of the widget
        /// </summary>
        [Category("Special")]
        [Description("Звук на начатое отображение виджета")]
        [XmlElement("soundShow")]
        public Reference<BlankFileProvider> SoundShow { get; set; }

        /// <summary>
        ///     Sound to hide the widget
        /// </summary>
        [Category("Special")]
        [Description("Звук на скрытие виджета")]
        [XmlElement("soundHide")]
        public Reference<BlankFileProvider> SoundHide { get; set; }

        /// <summary>
        ///     List of reactions to keystrokes
        /// </summary>
        [Category("Keyboard reactions")]
        [Description("Список реакций на клавиатурные нажатия")]
        [XmlArray("bindSections")]
        public List<BindSection> BindSections { get; set; }

        /// <summary>
        ///     Widget hover notification
        /// </summary>
        [Category("Mouse reactions")]
        [Description("Уведомление о наведении на виджет. (Кроме виджетов со специальной обработкой - кнопок и т.д.)")]
        [XmlElement("reactionOnPointing")]
        public string ReactionOnPointing { get; set; }

        /// <summary>
        ///     Hover notification on a widget, regardless of whether it is clickable
        /// </summary>
        [Category("Mouse reactions")]
        [Description(
            "Уведомление о наведении на виджет вне зависимости от его доступности для кликов. (Использовать только при сильной необходимости - потребляет много ресурсов.)")]
        [XmlElement("forceReactionOnPointing")]
        public string ForceReactionOnPointing { get; set; }

        /// <summary>
        ///     Mouse wheel scroll up notification
        /// </summary>
        [Category("Mouse reactions")]
        [Description("Уведомление о прокрутке колёсика мыши вверх")]
        [XmlElement("reactionWheelUp")]
        public string ReactionWheelUp { get; set; }

        /// <summary>
        ///     Mouse wheel scroll down notification
        /// </summary>
        [Category("Mouse reactions")]
        [Description("Уведомление о прокрутке колёсика мыши вниз")]
        [XmlElement("reactionWheelDown")]
        public string ReactionWheelDown { get; set; }

        public ImageSource Bitmap => GetBitmap();

        [Category("Base properties")]
        [Description("Системное название виджета")]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Category("Base properties")]
        [Description("Виден ли виджет. По умолчанию true. если виджет не виден, то он недоступен и для реакций")]
        [XmlIgnore]
        public bool Visible { get; set; }

        [Category("Base properties")]
        [Description(
            "Доступен ли виджет и все его дочерние виджеты для реакций. Может влиять на внешний вид (виджет \"засеривается\"). По умолчанию true")]
        [XmlIgnore]
        public bool Enabled { get; set; }

        [Category("Placement")]
        [Description("Описание расположение виджета")]
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